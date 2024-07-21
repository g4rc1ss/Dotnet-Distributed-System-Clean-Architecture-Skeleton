param (
    [string]$imageName = "distributed/weatherforecastconsumersyncdatabases",
    [string]$imageTag = "latest",
    [string]$prevTag = "previous",
    [string]$vpsUser = "",
    [string]$vpsHost = "192.168.66.2",
    [string]$vpsDest = "/home/",
    [string]$composeDir = "/home/",
    [string]$envFile = "env.test",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = "./id_rsa"
)
$tarImageName = "weatherForecastSyncConsumer_${imageTag}.tar"
$dockerComposeDeploy = "docker-compose.WFSyncConsumer.yml"
$dockerComposeBuildDeploy = "docker-compose.WFSyncConsumerBuild.yml"


# Build the Docker image
docker-compose -f $dockerComposeBuildDeploy build

# Save the image to a tar file
docker save -o $tarImageName "${imageName}:${imageTag}"

# Transfer the image to the VPS
scp -i $sshKeyPath $tarImageName "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $dockerComposeDeploy "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $envFile "$vpsUser@${vpsHost}:${vpsDest}/"

# Load the image on the VPS and deploy with Docker Compose
$deployScript = @"
echo $sudoPassword | sudo -S bash -c '
    # Tag the current latest as previous
    if docker images --format "{{.Repository}}:{{.Tag}}" | grep -q ${imageName}:${imageTag}; then
        echo "Establecemos la actual imagen como previous"
        docker tag ${imageName}:${imageTag} ${imageName}:${prevTag}
    fi
    
    # Load the new image
    echo "Cargamos la nueva imagen"
    docker load -i ${vpsDest}/${tarImageName}
    
    # Deploy the new image with Docker Compose
    cd ${composeDir}

    echo "Ejecutamos el docker compose para levantar la nueva imagen"
    docker-compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d

    echo "Limpiamos recursos"
    rm -rf ${tarImageName}
    docker image prune -f
'
"@

Invoke-Command -ScriptBlock {
    param($script, $user, $vpsHost, $sshKeyPath)
    ssh -i $sshKeyPath $user@$vpsHost $script
} -ArgumentList $deployScript, $vpsUser, $vpsHost, $sshKeyPath
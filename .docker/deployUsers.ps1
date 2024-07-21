param (
    [string]$imageName = "distributed/usersapi",
    [string]$imageTag = "latest",
    [string]$prevTag = "previous",
    [string]$vpsUser = "",
    [string]$vpsHost = "192.168.66.2",
    [string]$vpsDest = "/home/",
    [string]$composeDir = "/home/",
    [string]$envFile = "env.test",
    [string]$healthCheckUrl = "http://192.168.66.2:8550/health",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = "./id_rsa"
)
$tarImageName = "users_${imageTag}.tar"
$dockerComposeDeploy = "docker-compose.users.yml"
$dockerComposeBuildDeploy = "docker-compose.usersBuild.yml"


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

# Check the health of the deployed service
for ($i = 0; $i -lt 10; $i++) {
    if (Invoke-RestMethod -Uri $healthCheckUrl | grep -w "[Hh]ealthy") {
        Write-Host "Service is up and running"
        exit 0;
    }
    Write-Host "Waiting for the service to be ready..."
    sleep 5
}

Write-Host "Health check failed. Rolling back..."

# Rollback to the previous image
$rollbackScript = @"
    echo $sudoPassword | sudo -S bash -c '
    # Tag the previous image as latest
    docker tag ${imageName}:${prevTag} ${imageName}:${imageTag}
    
    # Redeploy with Docker Compose
    cd $composeDir
    docker-compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d
'
"@

Invoke-Command -ScriptBlock {
    param($script, $user, $vpsHost, $sshKeyPath)
    ssh -i $sshKeyPath $user@$vpsHost $script
} -ArgumentList $rollbackScript, $vpsUser, $vpsHost, $sshKeyPath
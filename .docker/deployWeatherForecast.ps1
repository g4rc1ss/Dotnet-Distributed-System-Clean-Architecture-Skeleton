param (
    [string]$vpsUser = "",
    [string]$vpsHost = "",
    [string]$vpsDest = "",
    [string]$envFile = "",
    [string]$healthCheckUrl = "",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = "",
    [string]$imageName = "distributed/weatherforecastapi",
    [string]$imageTag = "latest",
    [string]$prevTag = "previous"
)
$tarImageName = "weatherForecast_${imageTag}.tar"
$dockerComposeDeploy = "docker-compose.weatherForecast.yml"
$dockerComposeBuildDeploy = "docker-compose.weatherForecastBuild.yml"


# Build the Docker image
docker compose -f $dockerComposeBuildDeploy build

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
    cd ${vpsDest}

    echo "Ejecutamos el docker compose para levantar la nueva imagen"
    docker compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d

    echo "Limpiamos recursos"
    rm -rf ${tarImageName}
    docker image prune -f

    # Success
    echo "0";
'
"@

$response = Invoke-Command -ScriptBlock {
    param($script, $user, $vpsHost, $sshKeyPath)
    ssh -i $sshKeyPath $user@$vpsHost $script
} -ArgumentList $deployScript, $vpsUser, $vpsHost, $sshKeyPath

if ($response[$response.Length - 1] -ne "0") {
    Write-Error "Error al desplegar";
    exit 1;
}

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

pwsh -File ./rollback.ps1 $imageName $prevTag $imageTag $dockerComposeDeploy $vpsUser $vpsHost $vpsDest $envFile $sudoPassword $sshKeyPath

# Cerramos con error porque ha habido que hacer rollback
exit 1;
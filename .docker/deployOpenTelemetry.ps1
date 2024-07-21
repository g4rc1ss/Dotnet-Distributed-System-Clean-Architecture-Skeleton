param (
    [string]$vpsUser = "",
    [string]$vpsHost = "192.168.66.2",
    [string]$vpsDest = "/home/",
    [string]$composeDir = "/home/",
    [string]$envFile = "env.test",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = "./id_rsa"
)
$dockerComposeDeploy = "docker-compose.openTelemetry.yml"


# Transfer the image to the VPS
scp -i $sshKeyPath -r ./Observability "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $dockerComposeDeploy "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $envFile "$vpsUser@${vpsHost}:${vpsDest}/"

# Load the image on the VPS and deploy with Docker Compose
$deployScript = @"
echo $sudoPassword | sudo -S bash -c ' 
    # Deploy the new image with Docker Compose
    cd ${composeDir}

    echo "Updateamos las imagenes de los contenedores"
    docker-compose -f ${dockerComposeDeploy} pull

    echo "Ejecutamos el docker compose para levantar la nueva imagen"
    docker-compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d --force-recreate
'
"@

Invoke-Command -ScriptBlock {
    param($script, $user, $vpsHost, $sshKeyPath)
    ssh -i $sshKeyPath $user@$vpsHost $script
} -ArgumentList $deployScript, $vpsUser, $vpsHost, $sshKeyPath
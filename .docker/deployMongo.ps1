param (
    [string]$vpsUser = "",
    [string]$vpsHost = "",
    [string]$vpsDest = "",
    [string]$envFile = "",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = ""
)
$dockerComposeDeploy = "docker compose.mongo.yml"


# Transfer the image to the VPS
scp -i $sshKeyPath -r ./MongoDB "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $dockerComposeDeploy "$vpsUser@${vpsHost}:${vpsDest}/"
scp -i $sshKeyPath $envFile "$vpsUser@${vpsHost}:${vpsDest}/"

# Load the image on the VPS and deploy with Docker Compose
$deployScript = @"
echo $sudoPassword | sudo -S bash -c ' 
    # Deploy the new image with Docker Compose
    cd ${vpsDest}

    echo "Updateamos las imagenes de los contenedores"
    docker compose -f ${dockerComposeDeploy} pull

    echo "Ejecutamos el docker compose para levantar la nueva imagen"
    docker compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d --force-recreate

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

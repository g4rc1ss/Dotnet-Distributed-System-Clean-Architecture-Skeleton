param (
    [string]$imageName = "",
    [string]$prevTag = "",
    [string]$imageTag = "",
    [string]$dockerComposeDeploy = "",
    [string]$vpsUser = "",
    [string]$vpsHost = "",
    [string]$vpsDest = "",
    [string]$envFile = "",
    [string]$sudoPassword = "",
    [string]$sshKeyPath = ""
)


# Rollback to the previous image
$rollbackScript = @"
    echo $sudoPassword | sudo -S bash -c '
    # Tag the previous image as latest
    docker tag ${imageName}:${prevTag} ${imageName}:${imageTag}
    
    # Redeploy with Docker Compose
    cd $vpsDest
    docker-compose --env-file ${envFile} -f ${dockerComposeDeploy} up -d

    # Success
    echo "0";
'
"@

$rollbackResponse = Invoke-Command -ScriptBlock {
    param($script, $user, $vpsHost, $sshKeyPath)
    ssh -i $sshKeyPath $user@$vpsHost $script
} -ArgumentList $rollbackScript, $vpsUser, $vpsHost, $sshKeyPath

if ($rollbackResponse[$rollbackResponse.Length - 1] -ne "0") {
    Write-Error "Error al desplegar";
}
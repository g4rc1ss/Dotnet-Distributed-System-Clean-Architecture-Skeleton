param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("up", "down")]
    [string]$action,

    [Parameter(Mandatory = $true)]
    [ValidateSet("local", "dev", "pro", "pre")]
    [string]$environment,

    [Parameter(Mandatory = $false)]
    [ValidateSet("v")]
    [string]$removeVolumes,

    [Parameter(Mandatory = $false)]
    [string]$manualEnvPath
)

$commadDockerComposeToExecute = "docker compose "
$dockerComposeDotnetAppCommand = "docker-compose.app.yml "
$dockerComposeGrafanaCommand = "docker-compose.grafana.yml "
$dockerComposeFile = "docker-compose.yml "
$enviromentFile = ".env.$environment "

# Agregamos el parametro -f donde estan los servicios que usa la app para ejecutar en docker
$commadDockerComposeToExecute += "-f " + $dockerComposeFile + " "

if ($manualEnvPath -ne "") {
    $enviromentFile = "$manualEnvPath "
}
$commadDockerComposeToExecute += "--env-file " + $enviromentFile

if ($environment -eq "local") {
    $commadDockerComposeToExecute += "-f $dockerComposeGrafanaCommand "
}
elseif ($environment -eq "pro" -or $environment -eq "pre" -or $environment -eq "dev") {
    if ($action -eq "up") {
        $buildExec = "docker compose -f $dockerComposeDotnetAppCommand -f $dockerComposeFile --env-file $enviromentFile build" 
        Write-Output $buildExec
        Invoke-Expression $buildExec
    }

    $commadDockerComposeToExecute += "-f " + $dockerComposeDotnetAppCommand + " "
}

if ($action -eq "up") {
    $commadDockerComposeToExecute += "up -d "

}
elseif ($action -eq "down") {
    $commadDockerComposeToExecute += "down "
    $commadDockerComposeToExecute += $removeVolumes -eq "v" ? "-v " : ""

}

Write-Output "Comando a ejecutar" + $commadDockerComposeToExecute
Invoke-Expression $commadDockerComposeToExecute

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("up", "down")]
    [string]$action,

    [Parameter(Mandatory = $true)]
    [ValidateSet("local", "test")]
    [string]$environment,

    [Parameter(Mandatory = $false)]
    [ValidateSet("v")]
    [string]$removeVolumes
)

$composeToExecuteAlways = (
    "docker-compose.grafana.yml",
    "docker-compose.mongo.yml",
    "docker-compose.mySQL.yml",
    "docker-compose.openTelemetry.yml",
    "docker-compose.rabbitMQ.yml",
    "docker-compose.redis.yml"
);

$commadDockerComposeToExecute = "docker compose"
$dockerComposeDotnetAppCommand = "docker-compose.app.yml"
$enviromentFile = ".env.$environment"



$commadDockerComposeToExecute += " --env-file $enviromentFile"
foreach ($dockerComposeFile in $composeToExecuteAlways) {
    $commadDockerComposeToExecute += " -f $dockerComposeFile";
}

if ($environment -eq "test") {
    $commadDockerComposeToExecute += " -f $dockerComposeDotnetAppCommand";
    
    if ($action -eq "up") {
        $buildExec = "$commadDockerComposeToExecute build" 
        Write-Output $buildExec
        # Invoke-Expression $buildExec
    }
}

if ($action -eq "up") {
    $commadDockerComposeToExecute += " up -d"

}
elseif ($action -eq "down") {
    $commadDockerComposeToExecute += " down"
    $commadDockerComposeToExecute += $removeVolumes -eq "v" ? " -v" : ""

}

Write-Output "Comando a ejecutar" + $commadDockerComposeToExecute
# Invoke-Expression $commadDockerComposeToExecute

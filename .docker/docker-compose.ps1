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

$composeToExecuteAlways = @(
    "docker-compose.grafana.yml",
    "docker-compose.mongo.yml",
    "docker-compose.mySQL.yml",
    "docker-compose.openTelemetry.yml",
    "docker-compose.rabbitMQ.yml",
    "docker-compose.seqLogs.yml",
    "docker-compose.redis.yml"
);


$composeToExecuteOnTest = @(
    "docker-compose.weatherForecast.yml"
    "docker-compose.users.yml"
);

$composeToExecuteOnLocal = @(

);

$commadDockerComposeToExecute = "docker compose"
$enviromentFile = ".env.$environment"



$commadDockerComposeToExecute += " --env-file $enviromentFile"
foreach ($dockerComposeFile in $composeToExecuteAlways) {
    $commadDockerComposeToExecute += " -f $dockerComposeFile";
}


if ($environment -eq "local") {
    foreach ($dockerComposeFile in $composeToExecuteOnLocal) {
        $commadDockerComposeToExecute += " -f $dockerComposeFile";
    }
}

if ($environment -eq "test") {
    foreach ($dockerComposeFile in $composeToExecuteOnTest) {
        $commadDockerComposeToExecute += " -f $dockerComposeFile";
    }

    if ($action -eq "up") {
        $buildExec = "$commadDockerComposeToExecute build" 
        Write-Output $buildExec
        Invoke-Expression $buildExec
    }
}

if ($action -eq "up") {
    $commadDockerComposeToExecute += " up -d"

}
elseif ($action -eq "down") {
    $commadDockerComposeToExecute += " down"

    if ($removeVolumes -eq "v") {
        $commadDockerComposeToExecute += " -v"
    }

}

Write-Output "Comando a ejecutar" + $commadDockerComposeToExecute
Invoke-Expression $commadDockerComposeToExecute
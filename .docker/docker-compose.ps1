param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("up", "down")]
    [string]$action = "up",

    [Parameter(Mandatory = $true)]
    [ValidateSet("local", "test")]
    [string]$environment = "test",

    [Parameter(Mandatory = $false)]
    [ValidateSet("v")]
    [string]$removeVolumes
)

$composeToExecuteAlways = @(
    "docker-compose.mongo.yml",
    "docker-compose.mySQL.yml",
    "docker-compose.openTelemetry.yml",
    "docker-compose.rabbitMQ.yml",
    "docker-compose.redis.yml"
);

$composeBuildFiles = @(
    "docker-compose.usersBuild.yml",
    "docker-compose.weatherForecastBuild.yml",
    "docker-compose.WFSyncConsumerBuild.yml"
);

$composeToExecuteOnTest = @(
    "docker-compose.weatherForecast.yml",
    "docker-compose.WFSyncConsumer.yml",
    "docker-compose.users.yml"
);

$composeToExecuteOnLocal = @(
);

$commadDockerComposeToExecute = "docker-compose"
$enviromentFile = "env.$environment"


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
        $commandToExecuteBuildApps = "docker-compose"

        foreach ($dockerComposeFile in $composeBuildFiles) {
            $commandToExecuteBuildApps += " -f $dockerComposeFile";
        }

        $buildExec = "$commandToExecuteBuildApps build" 
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
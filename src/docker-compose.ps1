param (
    [Parameter(Mandatory=$true)]
    [ValidateSet("up", "down")]
    [string]$action,

    [Parameter(Mandatory=$true)]
    [ValidateSet("dev", "pro", "stagging")]
    [string]$environment,

    [Parameter(Mandatory=$false)]
    [ValidateSet("v")]
    [string]$removeVolumes
)

$commadDockerComposeToExecute = "docker-compose "
$dockerComposeDotnetAppCommand = "docker-compose.dotnetapp.yml" +" "
$dockerComposeFile = "docker-compose.yml" + " "

# Agregamos el parametro -f donde estan los servicios que usa la app para ejecutar en docker
$commadDockerComposeToExecute += "-f " + $dockerComposeFile + " "

if ($environment -eq "dev") {
    # Agregamos las variables para la ejecucion en local(dev)
    $commadDockerComposeToExecute += "-f " + ".env " 
}

if ($environment -eq "pro" -or $environment -eq "stagging") {
    $commadDockerComposeToExecute += "-f " + $dockerComposeDotnetAppCommand + " "
}
$commadDockerComposeToExecute += "-f " + $dockerComposeDotnetAppCommand + " "

if ($action -eq "up") {
    
    $commadDockerComposeToExecute += "up -d "
} elseif ($action -eq "down") {
    $commadDockerComposeToExecute += "down "
    $commadDockerComposeToExecute += $removeVolumes -eq "v" ? "-v " : ""
}

Invoke-Expression $commadDockerComposeToExecute
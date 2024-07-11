param (
    [Parameter(Mandatory=$true)]
    [string]$sourceDir,

    [Parameter(Mandatory=$true)]
    [string]$destDir
)

$sourceDir = Resolve-Path -Path $sourceDir | Select-Object -ExpandProperty Path
$destDir = Resolve-Path -Path $destDir | Select-Object -ExpandProperty Path

$directories = Get-ChildItem -Path $sourceDir -Recurse -Directory

foreach ($dir in $directories) {
    
    $targetDir = $dir.FullName.Replace($sourceDir, $destDir)
    New-Item -Path $targetDir -ItemType Directory -Force
}

Write-Output ""
Write-Output "Folder structure replicated from $sourceDir to $destDir successfully."
$currentDir = Get-Location
$coverageDir = Join-Path -Path $currentDir -ChildPath "/.coverage/"

$coverageFile = Join-Path -Path $coverageDir -ChildPath "coverage.cobertura.xml"
$coverageReportDir = Join-Path -Path $coverageDir -ChildPath "./report"

if (-not (Test-Path -Path $coverageDir)) {
    New-Item -ItemType Directory -Path $coverageDir
}

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$coverageDir
dotnet tool run reportgenerator -reports:$coverageFile -targetdir:$coverageReportDir

Write-Output "Coverage results are in: $coverageDir"
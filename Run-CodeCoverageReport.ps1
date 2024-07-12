$currentDir = Get-Location
$coverageDir = Join-Path -Path $currentDir -ChildPath "/.coverage/"

$coverageJsonFile = Join-Path -Path $coverageDir -ChildPath "coverage.json"
$coverageXmlFile = Join-Path -Path $coverageDir -ChildPath "coverage.cobertura.xml"
$coverageReportDir = Join-Path -Path $coverageDir -ChildPath "./report"

$coverageFormats = '\"json,cobertura\"'

if (-not (Test-Path -Path $coverageDir)) {
    New-Item -ItemType Directory -Path $coverageDir
}
else {
    Remove-Item -Recurse -Force $coverageDir
    New-Item -ItemType Directory -Path $coverageDir
}

dotnet test /p:CollectCoverage=true /p:CoverletOutput=$coverageDir /p:MergeWith=$coverageJsonFile /p:CoverletOutputFormat=$coverageFormats
dotnet tool run reportgenerator -reports:$coverageXmlFile -targetdir:$coverageReportDir -assemblyfilters:"-System.Text.RegularExpressions.Generator.*"

Write-Output "Coverage results are in: $coverageDir"

$indexFile = Join-Path -Path $coverageReportDir -ChildPath "index.html"
if (Test-Path -Path $indexFile) {
    Start-Process $indexFile
} else {
    Write-Output "Report file index.html not found."
}
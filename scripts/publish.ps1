param(
    [string]$Configuration = "Release",
    [string]$OutputDirectory = "dist\RemandMe"
)

$ErrorActionPreference = "Stop"

dotnet publish -c $Configuration

$publishDirectory = Join-Path $PSScriptRoot "..\bin\$Configuration\net8.0-windows\win-x64\publish"
$targetDirectory = Join-Path $PSScriptRoot "..\$OutputDirectory"

New-Item -ItemType Directory -Force -Path $targetDirectory | Out-Null
Copy-Item -Force -Path (Join-Path $publishDirectory "RemandMe.exe") -Destination (Join-Path $targetDirectory "RemandMe.exe")

Write-Host "Hazir exe: $targetDirectory\RemandMe.exe"

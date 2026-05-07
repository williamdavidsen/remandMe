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

$testCommand = @"
@echo off
start "" "%~dp0RemandMe.exe" --show-now
"@

$uninstallCommand = @"
@echo off
"%~dp0RemandMe.exe" --uninstall-startup
echo RemandMe Windows baslangicindan kaldirildi.
pause
"@

Set-Content -Encoding ASCII -Path (Join-Path $targetDirectory "Hemen Test Et.cmd") -Value $testCommand
Set-Content -Encoding ASCII -Path (Join-Path $targetDirectory "Baslangictan Kaldir.cmd") -Value $uninstallCommand

Write-Host "Hazir exe: $targetDirectory\RemandMe.exe"

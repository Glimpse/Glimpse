@echo off
.nuget\NuGet.exe install .nuget\packages.config -OutputDirectory packages
.nuget\NuGet.exe restore Glimpse.All.sln
powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& {Import-Module '.\packages\psake.*\tools\psake.psm1'; invoke-psake .\default.ps1 %*; if ($LastExitCode -ne 0) {write-host "ERROR: $LastExitCode" -fore RED; exit $lastexitcode} }"

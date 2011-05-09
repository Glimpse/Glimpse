# Helper script to return help text.

remove-module psake -ea 'SilentlyContinue'
$scriptPath = Split-Path -parent $MyInvocation.MyCommand.path
import-module (join-path $scriptPath psake.psm1)
Get-Help Invoke-psake -full
exit $lastexitcode
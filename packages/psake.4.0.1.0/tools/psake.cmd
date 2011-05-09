@echo off

SET DIR=%~dp0%

if '%1'=='/?' goto usage
if '%1'=='-?' goto usage
if '%1'=='?' goto usage
if '%1'=='/help' goto usage
if '%1'=='help' goto usage

powershell -NoProfile -ExecutionPolicy unrestricted -Command "& '%DIR%psake.ps1' %*"

goto :eof
:usage
powershell -NoProfile -ExecutionPolicy unrestricted -Command "& '%DIR%psake-help.ps1'"
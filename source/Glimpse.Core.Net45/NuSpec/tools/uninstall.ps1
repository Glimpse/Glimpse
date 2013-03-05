param($installPath, $toolsPath, $package, $project)

$tempDir = $env:TEMP
$tempDir = [System.IO.Path]::Combine($tempDir,"Glimpse")
if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir)}
$file = [System.IO.Path]::Combine($tempDir, "install.log")
$datetime = Get-Date

$package.Id + "," + $package.Version + "," + $datetime.ToString() + "," + $project.FileName | out-file $file -append
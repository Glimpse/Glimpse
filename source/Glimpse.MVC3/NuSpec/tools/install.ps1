param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]
$readmefile = $path::Combine($path::GetDirectoryName($project.FileName), "App_Readme\glimpse.mvc3.readme.txt")
$DTE.ItemOperations.OpenFile($readmefile)
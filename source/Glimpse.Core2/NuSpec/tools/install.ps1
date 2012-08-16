param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]
$readmefile = $path::Combine($installPath, "glimpse.readme.html")
$DTE.ItemOperations.Navigate($readmefile)
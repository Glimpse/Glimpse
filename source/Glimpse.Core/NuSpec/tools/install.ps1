param($installPath, $toolsPath, $package, $project)

$DTE.ItemOperations.Navigate("http://getglimpse.com/?id=" + $package.Id + "/" + $package.Version)
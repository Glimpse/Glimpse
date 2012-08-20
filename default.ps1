#properties ---------------------------------------------------------------------------------------------------------
$framework = '4.0'

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\builds"
    $source_dir = "$base_dir\source"
    $tools_dir = "$base_dir\tools"
    $package_dir = "$base_dir\packages"
    $framework_dir = $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory().Replace("v2.0.50727", "v4.0.30319"))
    $config = "release"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends compile

task clean {
    "Cleaning"
    
    "   Glimpse.Core"
    Delete-Directory "$source_dir\Glimpse.Core\bin"
    Delete-Directory "$source_dir\Glimpse.Core\obj"
    
    "   Glimpse.Core.Net35"
    Delete-Directory "$source_dir\Glimpse.Core.Net35\bin"
    Delete-Directory "$source_dir\Glimpse.Core.Net35\obj"
    
    "   Glimpse.AspNet"
    Delete-Directory "$source_dir\Glimpse.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet\obj"

    "   Glimpse.AspNet.Net35"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net35\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net35\obj"
    
    "   Glimpse.Mvc3"
    Delete-Directory "$source_dir\Glimpse.Mvc3\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc3\obj"
       
    "   Glimpse.Mvc3.MusicStore.Sample"
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\obj"
        
    "   Glimpse.Test.*"
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Core\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Core\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Core.Net35\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Core.net35\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Mvc3\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Mvc3\obj"
}

task compile -depends clean {
    "Compiling"
    "   Glimpse.All.sln"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config /nologo /verbosity:minimal }
}

task merge -depends test {
    "Merging"

    "   Glimpse.Core"
    del "$source_dir\Glimpse.Core\bin\Release\Newtonsoft.Json.pdb" #get rid of JSON PDF file, causes issues with Glimpse symbol package
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core\bin\Release\NLog.dll" "$source_dir\Glimpse.Core\bin\Release\AntiXssLibrary.dll" "$source_dir\Glimpse.Core\bin\Release\Tavis.UriTemplates.dll" }
    
    "   Glimpse.Core.Net35"
    del "$source_dir\Glimpse.Core.Net35\bin\Release\Newtonsoft.Json.pdb" #get rid of JSON PDF file, causes issues with Glimpse symbol package
    exec { & $tools_dir\ilmerge.exe /log /out:"$source_dir\Glimpse.Core\nuspec\lib\net35\Glimpse.Core.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core.Net35\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core.Net35\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core.Net35\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core.Net35\bin\Release\NLog.dll" "$source_dir\Glimpse.Core.Net35\bin\Release\AntiXssLibrary.dll"  "$source_dir\Glimpse.Core.Net35\bin\Release\Tavis.UriTemplates.dll"}
    
    "   Glimpse.AspNet"
    copy $source_dir\Glimpse.AspNet\bin\Release\Glimpse.AspNet.* $source_dir\Glimpse.AspNet\nuspec\lib\net40\
    
    "   Glimpse.AspNet.Net35"
    copy $source_dir\Glimpse.AspNet\bin\Release\Glimpse.AspNet.* $source_dir\Glimpse.AspNet\nuspec\lib\net35\
    
}

task pack -depends merge {
    "Packing"
    
    cd $package_dir\NuGet.CommandLine.*\tools\
    
    "   Glimpse.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Core\Properties\AssemblyInfo.cs
    exec { & .\nuget.exe pack $source_dir\Glimpse.Core\NuSpec\Glimpse.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.AspNet.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.AspNet\Properties\AssemblyInfo.cs
    exec { & .\nuget.exe pack $source_dir\Glimpse.AspNet\NuSpec\Glimpse.AspNet.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.zip"
    New-Item $build_dir\local\zip\Core\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\Core\net35 -Type directory -Force > $null
    New-Item $build_dir\local\zip\AspNet\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\AspNet\net35 -Type directory -Force > $null

    copy $base_dir\license.txt $build_dir\local\zip
        
    copy $source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.* $build_dir\local\zip\Core\net40
    copy $source_dir\Glimpse.Core\nuspec\lib\net35\Glimpse.Core.* $build_dir\local\zip\Core\net35
    
    copy $source_dir\Glimpse.AspNet\nuspec\lib\net40\Glimpse.AspNet.* $build_dir\local\zip\AspNet\net40
    copy $source_dir\Glimpse.AspNet\nuspec\lib\net35\Glimpse.AspNet.* $build_dir\local\zip\AspNet\net35
    copy $source_dir\Glimpse.AspNet\nuspec\readme.txt $build_dir\local\zip\AspNet
    
    #TODO: Add MVC
    #TODO: Add help .CHM file
        
    Create-Zip $build_dir\local\zip $build_dir\local\Glimpse.zip
    Delete-Directory $build_dir\local\zip
}

task test -depends compile{
    "Testing"
    
    New-Item $build_dir\local\artifacts -Type directory -Force > $null
    
    cd $package_dir\xunit.runners*\tools\
    
    exec { & .\xunit.console.clr4 $base_dir\tests.xunit }
}

task buildjs {
}

#functions ---------------------------------------------------------------------------------------------------------

function Delete-Directory($path)
{
  rd $path -recurse -force -ErrorAction SilentlyContinue | out-null
}

function Get-AssemblyInformationalVersion($path)
{
    $line = Get-Content $path | where {$_.Contains("AssemblyInformationalVersion")}
    $line.Split('"')[1]
}

function Create-Zip($sourcePath, $destinationFile)
{
    cd $package_dir\SharpZipLib.*\lib\20\
    
    Add-Type -Path ICSharpCode.SharpZipLib.dll

    $zip = New-Object ICSharpCode.SharpZipLib.Zip.FastZip
    $zip.CreateZip("$destinationFile", "$sourcePath", $true, $null)
}
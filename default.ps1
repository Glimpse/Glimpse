#properties ---------------------------------------------------------------------------------------------------------
$framework = '4.0'

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\builds"
    $source_dir = "$base_dir\source"
    $tools_dir = "$base_dir\tools"
    $framework_dir = $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory().Replace("v2.0.50727", "v4.0.30319"))
    
    $config = "release"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends compile

task clean {
    "Cleaning Glimpse.Core and Glimpse.Mvc3 bin and obj"

    delete_directory "$source_dir\Glimpse.Core\bin"
    delete_directory "$source_dir\Glimpse.Core\obj"
    delete_directory "$source_dir\Glimpse.Mvc3\bin"
    delete_directory "$source_dir\Glimpse.Mvc3\obj"

}

task compile -depends clean {
    "Compiling Glimpse.All.sln"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config }
}

task merge -depends compile {
    "Merging Glimpse.Core & Glimpse.Mvc3 to nuspec dir"

    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core\bin\Release\Newtonsoft.Json.Net35.dll" "$source_dir\Glimpse.Core\bin\Release\NLog.dll" }
    del $source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.pdb
    
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Mvc3\bin\Release\Glimpse.Mvc3.dll" "$source_dir\Glimpse.Mvc3\bin\Release\Castle.Core.dll" }
    del $source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.pdb

}

task pack -depends merge {
    "Creating Glimpse.nupkg & Glimpse.Mvc3.nupkg"

    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Core\nuspec\Glimpse.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Mvc3\nuspec\Glimpse.Mvc3.nuspec -OutputDirectory $build_dir\local }
}

task test -depends compile{
    "Testing Glimpse.Test.Core"
    
    exec { & $tools_dir\nunit\nunit-console.exe $tools_dir\nunit\GlimpseTests.nunit /labels /nologo }
}


#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}
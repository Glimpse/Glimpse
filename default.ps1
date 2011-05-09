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

task echo {
   $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory().Replace("v2.0.50727", "v4.0.30319"))
}

task clean {
    "Cleaning Glimpse.Net bin and obj"

    delete_directory "$source_dir\Glimpse.net\bin"
    delete_directory "$source_dir\Glimpse.net\obj"
}

task compile -depends clean {
    "Compiling Glimpse.All"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config }
}

task merge -depends compile {
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Net\nuspec\lib\net40\Glimpse.Net.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Net\bin\Release\Glimpse.Net.dll" "$source_dir\Glimpse.Net\bin\Release\Castle.Core.dll" }
    del $source_dir\Glimpse.Net\nuspec\lib\net40\Glimpse.Net.pdb
}

task pack -depends merge {
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Net\nuspec\Glimpse.nuspec -OutputDirectory $build_dir\local }
}


#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}
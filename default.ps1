#properties ---------------------------------------------------------------------------------------------------------
$framework = '4.0'

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\build"
    $source_dir = "$base_dir\source"
    $tools_dir = "$base_dir\tools"
    
    
    
    $config = "release"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends compile

task echo {
   
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

task merge -depends compile{
    create_directory "$build_dir\merge"
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$build_dir\merge\AutoMapper.dll" /internalize:AutoMapper.exclude "$build_dir\$config\AutoMapper\AutoMapper.dll" "$build_dir\$config\AutoMapper\Castle.Core.dll" /keyfile:"$source_dir\AutoMapper.snk" }
}


#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}


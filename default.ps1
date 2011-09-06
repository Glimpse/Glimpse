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
    "Cleaning Glimpse.Core, Glimpse.Mvc3 & Glimpse.Ef bin and obj"

    delete_directory "$source_dir\Glimpse.Core\bin"
    delete_directory "$source_dir\Glimpse.Core\obj"
    delete_directory "$source_dir\Glimpse.Mvc3\bin"
    delete_directory "$source_dir\Glimpse.Mvc3\obj"
    delete_directory "$source_dir\Glimpse.Ef\bin"
    delete_directory "$source_dir\Glimpse.Ef\obj"
}

task compile -depends clean {
    "Compiling Glimpse.All.sln"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config }
}

task merge -depends compile {
    "Merging Glimpse.Core, Glimpse.Mvc3 & Glimpse.Ef to nuspec dir"

    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core\bin\Release\Newtonsoft.Json.Net35.dll" "$source_dir\Glimpse.Core\bin\Release\NLog.dll" }
    del $source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.pdb
    
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Mvc3\bin\Release\Glimpse.Mvc3.dll" "$source_dir\Glimpse.Mvc3\bin\Release\Castle.Core.dll" }
    del $source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.pdb
    
    copy $source_dir\Glimpse.Ef\bin\Release\Glimpse.Ef.dll $source_dir\Glimpse.Ef\nuspec\lib\net40\Glimpse.Ef.dll

}

task pack -depends merge {
    "Creating Glimpse.nupkg, Glimpse.Mvc3.nupkg * Glimpse.Ef.nupkg"

    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Core\nuspec\Glimpse.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Mvc3\nuspec\Glimpse.Mvc3.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Ef\nuspec\Glimpse.Ef.nuspec -OutputDirectory $build_dir\local }
    
    mkdir $build_dir\local\zip
    copy $source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.dll $build_dir\local\zip
    copy $source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.dll $build_dir\local\zip
    copy $source_dir\Glimpse.Ef\nuspec\lib\net40\Glimpse.Ef.dll $build_dir\local\zip
    
    copy $source_dir\Glimpse.Core\nuspec\content\App_Readme\glimpse.readme.txt $build_dir\local\zip
    copy $source_dir\Glimpse.Mvc3\nuspec\content\App_Readme\glimpse.mvc3.readme.txt $build_dir\local\zip
    copy $source_dir\Glimpse.Ef\nuspec\content\App_Readme\glimpse.ef.readme.txt $build_dir\local\zip
    
    copy $base_dir\license.txt $build_dir\local\zip
    
    dir $build_dir\local\zip\*.* -Recurse | add-Zip $build_dir\local\Glimpse.zip
    del $build_dir\local\zip -Recurse
}

task test -depends compile{
    "Testing Glimpse.Test.Core"
    
    exec { & $tools_dir\nunit\nunit-console.exe $tools_dir\nunit\GlimpseTests.nunit /labels /nologo }
}

task buildjs {
}

#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}

function Add-Zip
{
	param([string]$zipfilename)

	if(-not (test-path($zipfilename)))
	{
		set-content $zipfilename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
		(dir $zipfilename).IsReadOnly = $false	
	}
	
	$shellApplication = new-object -com shell.application
	$zipPackage = $shellApplication.NameSpace($zipfilename)
	
	foreach($file in $input) 
	{ 
            $zipPackage.CopyHere($file.FullName)
            Start-sleep -milliseconds 500
	}
}
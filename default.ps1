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
    "Cleaning"
    "   Glimpse.Core2"
    "   Glimpse.Core2.Net35"
    "   Glimpse.AspNet"
    "   Glimpse.Mvc"
    "   Glimpse.JavaScript"
    "   Glimpse.Mvc3.MusicStore.Sample"
    "   Glimpse.Test.*"

    Delete-Directory "$source_dir\Glimpse.Core2\bin"
    Delete-Directory "$source_dir\Glimpse.Core2\obj"
    
    Delete-Directory "$source_dir\Glimpse.Core2.Net35\bin"
    Delete-Directory "$source_dir\Glimpse.Core2.Net35\obj"
    
    Delete-Directory "$source_dir\Glimpse.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet\obj"
    
    Delete-Directory "$source_dir\Glimpse.Mvc\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc\obj"
    
    Delete-Directory "$source_dir\Glimpse.JavaScript\bin"
    Delete-Directory "$source_dir\Glimpse.Glimpse.JavaScript\obj"
    
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\obj"
        
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Core2\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Core2\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Core2.Net35\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Core2.net35\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Mvc\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Mvc\obj"
}

task compile -depends clean {
    "Compiling"
    "   Glimpse.All.sln"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config /nologo /verbosity:minimal }
}

task merge -depends compile {
    "Merging"
    "   Glimpse.Core2"
    
    exec { & $tools_dir\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core2\nuspec\lib\net40\Glimpse.Core2.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core2\bin\Release\Glimpse.Core2.dll" "$source_dir\Glimpse.Core2\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core2\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core2\bin\Release\NLog.dll" "$source_dir\Glimpse.Core2\bin\Release\AntiXssLibrary.dll" }
    del $source_dir\Glimpse.Core2\nuspec\lib\net40\Glimpse.Core2.pdb
    
    exec { & $tools_dir\ilmerge.exe /log /out:"$source_dir\Glimpse.Core2\nuspec\lib\net35\Glimpse.Core2.dll" /internalize:$tools_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core2.Net35\bin\Release\Glimpse.Core2.dll" "$source_dir\Glimpse.Core2.Net35\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core2.Net35\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core2.Net35\bin\Release\NLog.dll" "$source_dir\Glimpse.Core2.Net35\bin\Release\AntiXssLibrary.dll" }
    del $source_dir\Glimpse.Core2\nuspec\lib\net35\Glimpse.Core2.pdb
}

task pack -depends merge {
    "Creating Glimpse.nupkg, Glimpse.Mvc3.nupkg, Glimpse.Ef.nupkg & Glimpse.Elmah.nupkg"

    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Core\nuspec\Glimpse.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Mvc3\nuspec\Glimpse.Mvc3.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Ef\nuspec\Glimpse.Ef.nuspec -OutputDirectory $build_dir\local }
    exec { & $tools_dir\nuget.exe pack $source_dir\Glimpse.Elmah\nuspec\Glimpse.Elmah.nuspec -OutputDirectory $build_dir\local }
    
    mkdir $build_dir\local\zip
    copy $source_dir\Glimpse.Core\nuspec\lib\net40\Glimpse.Core.dll $build_dir\local\zip
    copy $source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.dll $build_dir\local\zip
    copy $source_dir\Glimpse.Ef\nuspec\lib\net40\Glimpse.Ef.dll $build_dir\local\zip
    copy $source_dir\Glimpse.Elmah\nuspec\lib\net40\Glimpse.Elmah.dll $build_dir\local\zip
    
    copy $source_dir\Glimpse.Core\nuspec\content\App_Readme\glimpse.readme.txt $build_dir\local\zip
    copy $source_dir\Glimpse.Mvc3\nuspec\content\App_Readme\glimpse.mvc3.readme.txt $build_dir\local\zip
    copy $source_dir\Glimpse.Ef\nuspec\content\App_Readme\glimpse.ef.readme.txt $build_dir\local\zip
    copy $source_dir\Glimpse.Elmah\nuspec\content\App_Readme\glimpse.elmah.readme.txt $build_dir\local\zip
    
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

function Delete-Directory($path)
{
  rd $path -recurse -force -ErrorAction SilentlyContinue | out-null
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
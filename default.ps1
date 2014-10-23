properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\builds"
    $source_dir = "$base_dir\source"
    $tools_dir = "$base_dir\tools"
    $package_dir = "$base_dir\packages"
    $framework_dir =  (Get-ProgramFiles) + "\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
    $config = "release"
    $preReleaseVersion = $null
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends compile

task clean {
    "Cleaning"
    
    "   builds/local"
    Remove-Item $build_dir\local\*.nupkg
    Remove-Item $build_dir\local\*.zip
    Remove-Item $build_dir\local\*.chm
    
    "   Glimpse.Core"
    Delete-Directory "$source_dir\Glimpse.Core\bin"
    Delete-Directory "$source_dir\Glimpse.Core\obj"

    "   Glimpse.Core.Net45"
    Delete-Directory "$source_dir\Glimpse.Core.Net45\bin"
    Delete-Directory "$source_dir\Glimpse.Core.Net45\obj"
    
    "   Glimpse.Core.Net40"
    Delete-Directory "$source_dir\Glimpse.Core.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.Core.Net40\obj"
    
    "   Glimpse.Owin"
    Delete-Directory "$source_dir\Glimpse.Owin\bin"
    Delete-Directory "$source_dir\Glimpse.Owin\obj"
    
    "   Glimpse.AspNet"
    Delete-Directory "$source_dir\Glimpse.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet\obj"

    "   Glimpse.AspNet.Net45"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net45\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net45\obj"
    
    "   Glimpse.AspNet.Net40"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.AspNet.Net40\obj"

    "   Glimpse.Mvc"
    Delete-Directory "$source_dir\Glimpse.Mvc\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc\obj"

    "   Glimpse.Mvc3"
    Delete-Directory "$source_dir\Glimpse.Mvc3\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc3\obj"

    "   Glimpse.Mvc4"
    Delete-Directory "$source_dir\Glimpse.Mvc4\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc4\obj"

    "   Glimpse.Mvc5"
    Delete-Directory "$source_dir\Glimpse.Mvc5\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc5\obj"
       
    "   Glimpse.Mvc3.MusicStore.Sample"
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc3.MusicStore.Sample\obj"
    
    "   Glimpse.Mvc4.MusicStore.Sample"
    Delete-Directory "$source_dir\Glimpse.Mvc4.MusicStore.Sample\bin"
    Delete-Directory "$source_dir\Glimpse.Mvc4.MusicStore.Sample\obj"
    
    "   Glimpse.WebForms.WingTip.Sample"
    Delete-Directory "$source_dir\Glimpse.WebForms.WingTip.Sample\bin"
    Delete-Directory "$source_dir\Glimpse.WebForms.WingTip.Sample\obj"
    
    "   Glimpse.Ado"
    Delete-Directory "$source_dir\Glimpse.Ado\bin"
    Delete-Directory "$source_dir\Glimpse.Ado\obj"

    "   Glimpse.Ado.Net40"
    Delete-Directory "$source_dir\Glimpse.Ado.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.Ado.Net40\obj"

    "   Glimpse.EF"
    Delete-Directory "$source_dir\Glimpse.EF\bin"
    Delete-Directory "$source_dir\Glimpse.EF\obj"
    
    "   Glimpse.EF43.Net40"
    Delete-Directory "$source_dir\Glimpse.EF43.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.EF43.Net40\obj"

    "   Glimpse.EF5.Net45"
    Delete-Directory "$source_dir\Glimpse.EF5.Net45\bin"
    Delete-Directory "$source_dir\Glimpse.EF5.Net45\obj"
    
    "   Glimpse.EF5.Net40"
    Delete-Directory "$source_dir\Glimpse.EF5.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.EF5.Net40\obj"

    "   Glimpse.EF6.Net45"
    Delete-Directory "$source_dir\Glimpse.EF6.Net45\bin"
    Delete-Directory "$source_dir\Glimpse.EF6.Net45\obj"
    
    "   Glimpse.EF6.Net40"
    Delete-Directory "$source_dir\Glimpse.EF6.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.EF6.Net40\obj"

    "   Glimpse.WebForms"
    Delete-Directory "$source_dir\Glimpse.WebForms\bin"
    Delete-Directory "$source_dir\Glimpse.WebForms\obj"

    "   Glimpse.WebForms.Net45"
    Delete-Directory "$source_dir\Glimpse.WebForms.Net45\bin"
    Delete-Directory "$source_dir\Glimpse.WebForms.Net45\obj"
    
    "   Glimpse.WebForms.Net40"
    Delete-Directory "$source_dir\Glimpse.WebForms.Net40\bin"
    Delete-Directory "$source_dir\Glimpse.WebForms.Net40\obj"

    "   Glimpse.Test.*"
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\bin"
    Delete-Directory "$source_dir\Glimpse.Test.AspNet\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Core\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Core\obj"
    
    Delete-Directory "$source_dir\Glimpse.Test.Mvc3\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Mvc3\obj"
}

task compile -depends clean {
    "Compiling"
    "   Glimpse.All.sln"
    
    exec { msbuild $base_dir\Glimpse.All.sln /p:Configuration=$config /nologo /verbosity:minimal }
}

task docs -depends compile {
    "Documenting"
    "   Glimpse.Core.Documentation.Api"
    
    exec { msbuild $source_dir\Glimpse.Core.Documentation.Api\Glimpse.Core.Documentation.Api.shfbproj /p:Configuration=$config /nologo /verbosity:minimal }
    copy $source_dir\Glimpse.Core.Documentation.Api\Help\Glimpse.Core.Documentation.chm $source_dir\Glimpse.Core.Net45\nuspec\docs\Glimpse.Core.Documentation.chm
}

task merge -depends test {
    "Merging"

    cd $package_dir\ilmerge.*\

    "   Glimpse.Core.Net45"
    exec { & .\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core.Net45\nuspec\lib\net45\Glimpse.Core.dll" /internalize:$base_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core.Net45\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\NLog.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\AntiXssLibrary.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\Tavis.UriTemplates.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\Antlr4.StringTemplate.dll"}
    
    "   Glimpse.Core.Net40"
    exec { & .\ilmerge.exe /targetplatform:"v4,$framework_dir" /log /out:"$source_dir\Glimpse.Core.Net45\nuspec\lib\net40\Glimpse.Core.dll" /internalize:$base_dir\ILMergeInternalize.txt "$source_dir\Glimpse.Core.Net40\bin\Release\Glimpse.Core.dll" "$source_dir\Glimpse.Core.Net40\bin\Release\Newtonsoft.Json.dll" "$source_dir\Glimpse.Core.Net40\bin\Release\Castle.Core.dll" "$source_dir\Glimpse.Core.Net40\bin\Release\NLog.dll" "$source_dir\Glimpse.Core.Net40\bin\Release\AntiXssLibrary.dll" "$source_dir\Glimpse.Core.Net40\bin\Release\Tavis.UriTemplates.dll" "$source_dir\Glimpse.Core.Net45\bin\Release\Antlr4.StringTemplate.dll"}

    "   Glimpse.Owin"
    copy $source_dir\Glimpse.Owin\bin\Release\Glimpse.Owin.* $source_dir\Glimpse.Owin\nuspec\lib\net40\
    
    "   Glimpse.AspNet.Net45"
    copy $source_dir\Glimpse.AspNet.Net45\bin\Release\Glimpse.AspNet.* $source_dir\Glimpse.AspNet.Net45\nuspec\lib\net45\
    
    "   Glimpse.AspNet.Net40"
    copy $source_dir\Glimpse.AspNet.Net40\bin\Release\Glimpse.AspNet.* $source_dir\Glimpse.AspNet.Net45\nuspec\lib\net40\   
    
    "   Glimpse.Ado.Net45"
    copy $source_dir\Glimpse.Ado.Net45\bin\Release\Glimpse.Ado.* $source_dir\Glimpse.Ado.Net45\nuspec\lib\net45\
    
    "   Glimpse.Ado.Net40"
    copy $source_dir\Glimpse.Ado.Net40\bin\Release\Glimpse.Ado.* $source_dir\Glimpse.Ado.Net45\nuspec\lib\net40\   
    
    "   Glimpse.Mvc3"
    copy $source_dir\Glimpse.Mvc3\bin\Release\Glimpse.Mvc3.* $source_dir\Glimpse.Mvc3\nuspec\lib\net40\

    "   Glimpse.Mvc4"
    copy $source_dir\Glimpse.Mvc4\bin\Release\Glimpse.Mvc4.* $source_dir\Glimpse.Mvc4\nuspec\lib\net40\

    "   Glimpse.Mvc5"
    copy $source_dir\Glimpse.Mvc5\bin\Release\Glimpse.Mvc5.* $source_dir\Glimpse.Mvc5\nuspec\lib\net45\
    
    "   Glimpse.EF43.Net40"
    copy $source_dir\Glimpse.EF43.Net40\bin\Release\Glimpse.EF43.* $source_dir\Glimpse.EF43.Net40\nuspec\lib\net40\   
    
    "   Glimpse.EF5.Net45"
    copy $source_dir\Glimpse.EF5.Net45\bin\Release\Glimpse.EF5.* $source_dir\Glimpse.EF5.Net45\nuspec\lib\net45\
    
    "   Glimpse.EF5.Net40"
    copy $source_dir\Glimpse.EF5.Net40\bin\Release\Glimpse.EF5.* $source_dir\Glimpse.EF5.Net45\nuspec\lib\net40\   

    "   Glimpse.EF6.Net45"
    copy $source_dir\Glimpse.EF6.Net45\bin\Release\Glimpse.EF6.* $source_dir\Glimpse.EF6.Net45\nuspec\lib\net45\
    
    "   Glimpse.EF6.Net40"
    copy $source_dir\Glimpse.EF6.Net40\bin\Release\Glimpse.EF6.* $source_dir\Glimpse.EF6.Net45\nuspec\lib\net40\ 
	 
    "   Glimpse.WebForms.Net45"
    copy $source_dir\Glimpse.WebForms.Net45\bin\Release\Glimpse.WebForms.* $source_dir\Glimpse.WebForms.Net45\nuspec\lib\net45\
    
    "   Glimpse.WebForms.Net40"
    copy $source_dir\Glimpse.WebForms.Net40\bin\Release\Glimpse.WebForms.* $source_dir\Glimpse.WebForms.Net45\nuspec\lib\net40\   
}

task pack -depends merge {
    "Packing"
    
    cd $base_dir\.NuGet
    
    "   Glimpse.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Core\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Core.Net45\NuSpec\Glimpse.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.Owin.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Owin\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Owin\NuSpec\Glimpse.Owin.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.AspNet.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.AspNet\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.AspNet.Net45\NuSpec\Glimpse.AspNet.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }

    "   Glimpse.Mvc3.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Mvc\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Mvc3\NuSpec\Glimpse.Mvc3.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }

    "   Glimpse.Mvc4.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Mvc\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Mvc4\NuSpec\Glimpse.Mvc4.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
	
    "   Glimpse.Mvc5.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Mvc\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Mvc5\NuSpec\Glimpse.Mvc5.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.Ado.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.Ado\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.Ado.Net45\NuSpec\Glimpse.Ado.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.EF43.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.EF\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.EF43.Net40\NuSpec\Glimpse.EF43.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.EF5.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.EF\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.EF5.Net45\NuSpec\Glimpse.EF5.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }

    "   Glimpse.EF6.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.EF\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.EF6.Net45\NuSpec\Glimpse.EF6.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
    
    "   Glimpse.WebForms.nuspec"
    $version = Get-AssemblyInformationalVersion $source_dir\Glimpse.WebForms\Properties\AssemblyInfo.cs | Update-AssemblyInformationalVersion
    exec { & .\nuget.exe pack $source_dir\Glimpse.WebForms.Net45\NuSpec\Glimpse.WebForms.nuspec -OutputDirectory $build_dir\local -Symbols -Version $version }
	
    "   Glimpse.zip"
    New-Item $build_dir\local\zip\Core\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\Core\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\Owin\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\AspNet\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\AspNet\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\MVC3\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\MVC4\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\MVC5\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\Ado\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\Ado\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\EF43\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\EF5\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\EF5\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\EF6\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\EF6\net40 -Type directory -Force > $null
    New-Item $build_dir\local\zip\WebForms\net45 -Type directory -Force > $null
    New-Item $build_dir\local\zip\WebForms\net40 -Type directory -Force > $null

    copy $base_dir\license.txt $build_dir\local\zip
    
    copy $source_dir\Glimpse.Core.Net45\nuspec\lib\net45\Glimpse.Core.* $build_dir\local\zip\Core\net45    
    copy $source_dir\Glimpse.Core.Net45\nuspec\lib\net40\Glimpse.Core.* $build_dir\local\zip\Core\net40

    copy $source_dir\Glimpse.Owin\nuspec\lib\net40\Glimpse.Owin.* $build_dir\local\zip\Owin\net40
    
    copy $source_dir\Glimpse.AspNet.Net45\nuspec\lib\net45\Glimpse.AspNet.* $build_dir\local\zip\AspNet\net45
    copy $source_dir\Glimpse.AspNet.Net45\nuspec\lib\net40\Glimpse.AspNet.* $build_dir\local\zip\AspNet\net40
    copy $source_dir\Glimpse.AspNet.Net45\nuspec\readme.txt $build_dir\local\zip\AspNet
    
    copy $source_dir\Glimpse.Mvc3\nuspec\lib\net40\Glimpse.Mvc3.* $build_dir\local\zip\Mvc3\net40
    copy $source_dir\Glimpse.Mvc4\nuspec\lib\net40\Glimpse.Mvc4.* $build_dir\local\zip\Mvc4\net40
    copy $source_dir\Glimpse.Mvc5\nuspec\lib\net45\Glimpse.Mvc5.* $build_dir\local\zip\Mvc5\net40
        
    copy $source_dir\Glimpse.Ado.Net45\nuspec\lib\net45\Glimpse.Ado.* $build_dir\local\zip\Ado\net45
    copy $source_dir\Glimpse.Ado.Net45\nuspec\lib\net40\Glimpse.Ado.* $build_dir\local\zip\Ado\net40
    
    copy $source_dir\Glimpse.EF43.Net40\nuspec\lib\Net40\Glimpse.EF43.* $build_dir\local\zip\EF43\Net40
    copy $source_dir\Glimpse.EF5.Net45\nuspec\lib\net45\Glimpse.EF5.* $build_dir\local\zip\EF5\net45
    copy $source_dir\Glimpse.EF5.Net45\nuspec\lib\net40\Glimpse.EF5.* $build_dir\local\zip\EF5\net40
    copy $source_dir\Glimpse.EF6.Net45\nuspec\lib\net45\Glimpse.EF6.* $build_dir\local\zip\EF6\net45
    copy $source_dir\Glimpse.EF6.Net45\nuspec\lib\net40\Glimpse.EF6.* $build_dir\local\zip\EF6\net40
    
    copy $source_dir\Glimpse.WebForms.Net45\nuspec\lib\net45\Glimpse.WebForms.* $build_dir\local\zip\WebForms\net45
    copy $source_dir\Glimpse.WebForms.Net45\nuspec\lib\net40\Glimpse.WebForms.* $build_dir\local\zip\WebForms\net40
    copy $source_dir\Glimpse.WebForms.Net45\nuspec\readme.txt $build_dir\local\zip\WebForms
	
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

task push {
    "Pushing"
    "`nPush the following packages:"
    
    cd $build_dir\local
    
    $packages = Get-ChildItem * -Include *.nupkg -Exclude *.symbols.nupkg
    
    foreach($package in $packages){ 
        Write-Host "`t$package" 
    } 
     
    #Get-ChildItem -Path .\builds\local -Filter *.nupkg | FT Name
    
    $input = Read-Host "to (N)uget, (M)yget, (B)oth or (Q)uit?"

    switch ($input) 
        { 
            N {
               "Pushing to NuGet...";
               Push-Packages https://www.nuget.org
               break;
               } 
            M {
               "Pushing to MyGet...";
               Push-Packages https://www.myget.org/F/glimpsemilestone/
               break;
              } 
            B {
               "Pushing to MyGet...";
               Push-Packages https://www.myget.org/F/glimpsemilestone/
               "Pushing to NuGet...";
               Push-Packages https://www.nuget.org
               break;
              } 
            default {
              "Push aborted";
              break;
              }
        }
}

task buildjs {
}

task integrate {
    "Integration Testing"
    
    "   Clean Glimpse.Test.Integration"
    Delete-Directory "$source_dir\Glimpse.Test.Integration\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Integration\obj"
    
    "   Clean Glimpse.Test.Integration.Site"
    Delete-Directory "$source_dir\Glimpse.Test.Integration.Site\bin"
    Delete-Directory "$source_dir\Glimpse.Test.Integration.Site\obj"

    "`nBuild Integration Sln"
    exec { msbuild $base_dir\Glimpse.Integration.sln /p:Configuration=$config /nologo /verbosity:minimal }
    
    "`nGlimpse must be manually installed while waiting for http://nuget.codeplex.com/workitem/2730"
    #cd $base_dir\.NuGet
    
    #nuget update -source "c:\glimpse\builds\local" -Id Glimpse.MVC3;Glimpse.AspNet;Glimpse.WebForms;Glimpse -Verbose "c:\glimpse\source\Glimpse.Test.Integration.Site\packages.config"
    #exec { & .\nuget.exe update -source $build_dir\local -id "Glimpse.MVC3;Glimpse.AspNet;Glimpse.WebForms;Glimpse" -Verbose "$source_dir\Glimpse.Test.Integration.Site\packages.config" }
    
    "`nIIS must be set up with Administrative privledges. Run: "
    "C:\Windows\System32\inetsrv\appcmd.exe add site /name:""Glimpse Integration Test Site"" /bindings:""http/*:1155:"" /physicalPath:""C:\Glimpse\source\Glimpse.Test.Integration.Site"
    "to support IIS testing"

    
    "`nEnding Cassini"
    kill -name WebDev.WebServer*

    "`nEnding IIS Express"
    kill -name iisexpress*
    
    $cassiniPath = "C:\Program Files (x86)\Common Files\microsoft shared\DevServer\11.0\WebDev.WebServer40.EXE"
    $exists = Test-Path($cassiniPath)
    if ($exists -eq $false)
    {
        $cassiniPath = "C:\Program Files\Common Files\microsoft shared\DevServer\11.0\WebDev.WebServer40.EXE"
        $exists = Test-Path($cassiniPath)
        if ($exists -eq $false)
        {
            "Using WebDev.WebServer40.EXE from PATH. Add directory containing 'WebDev.WebServer40.EXE' to PATH environment variable."
            $cassiniPath = "WebDev.WebServer40.EXE"
        }
    }
    
    $iisExpressPath = "C:\Program Files (x86)\IIS Express\iisexpress.exe"
    $exists = Test-Path($iisExpressPath)
    if ($exists -eq $false)
    {
        $iisExpressPath = "C:\Program Files\IIS Express\iisexpress.exe"
        $exists = Test-Path($iisExpressPath)
        if ($exists -eq $false)
        {
            "Using iisexpress.exe from PATH. Add directory containing 'iisexpress.exe' to PATH environment variable."
            $iisExpressPath = "iisexpress.exe"
        }
    }

    "`nStarting Cassini"
    &$cassiniPath /port:234 /path:"$source_dir\Glimpse.Test.Integration.Site"
    
    "`nStarting IIS Express"
    $iisExpressArgs = "/port:1153 /path:$source_dir\Glimpse.Test.Integration.Site /systray:true"
    start-process $iisExpressPath $iisExpressArgs 
    
    "`nRunning Tests"
    New-Item $build_dir\local\artifacts -Type directory -Force > $null
    cd $package_dir\xunit.runners*\tools\
    exec { & .\xunit.console.clr4.x86 $base_dir\integration.xunit }
    
    "`nEnding Cassini"
    kill -name WebDev.WebServer*

    "`nEnding IIS Express"
    kill -name iisexpress*
}

#functions ---------------------------------------------------------------------------------------------------------

function Push-Packages($uri)
{
  cd $build_dir\local
  $packages = Get-ChildItem * -Include *.nupkg -Exclude *.symbols.nupkg
  
  cd $base_dir\.NuGet
   
  foreach($package in $packages){
    exec { & .\nuget.exe push $package -src $uri}
  } 
}

function Delete-Directory($path)
{
  rd $path -recurse -force -ErrorAction SilentlyContinue | out-null
}

function Get-AssemblyInformationalVersion($path)
{
    $line = Get-Content $path | where {$_.Contains("AssemblyInformationalVersion")}
    $line.Split('"')[1]
}

function Update-AssemblyInformationalVersion
{
    if ($preReleaseVersion -ne $null)
    {
        $version = ([string]$input).Split('-')[0]
        $date = Get-Date
        $parsed = $preReleaseVersion.Replace("{date}", $date.ToString("yyMMdd"))
        return "$version-$parsed"
    }
    else
    {
        return $input
    }
}

function Create-Zip($sourcePath, $destinationFile)
{
    cd $package_dir\SharpZipLib.*\lib\20\
    
    Add-Type -Path ICSharpCode.SharpZipLib.dll

    $zip = New-Object ICSharpCode.SharpZipLib.Zip.FastZip
    $zip.CreateZip("$destinationFile", "$sourcePath", $true, $null)
}

function Get-ProgramFiles
{
    #TODO: Someone please come up with a better way of detecting this - Tried http://msmvps.com/blogs/richardsiddaway/archive/2010/02/26/powershell-pack-mount-specialfolder.aspx and some enums missing
    #      This is needed because of this http://www.mattwrock.com/post/2012/02/29/What-you-should-know-about-running-ILMerge-on-Net-45-Beta-assemblies-targeting-Net-40.aspx (for machines that dont have .net 4.5 and only have 4.0)
    if (Test-Path "C:\Program Files (x86)") {
        return "C:\Program Files (x86)"
    }
    return "C:\Program Files"
}
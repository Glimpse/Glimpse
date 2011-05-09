#-------------------------------------------------------------------
#Specify defaults and do not auto-load modules
#-------------------------------------------------------------------
$psake.config = new-object psobject -property @{
  defaultBuildFileName="default.ps1";
  taskNameFormat="Executing {0}";
  exitCode="1";
  verboseError=$false;
  modules=(new-object psobject -property @{ autoload=$false })
}

<#
-------------------------------------------------------------------
Specify defaults and auto-load modules from .\modules folder
-------------------------------------------------------------------
$psake.config = new-object psobject -property @{
  defaultBuildFileName="default.ps1";
  taskNameFormat="Executing {0}";
  exitCode="1";
  verboseError=$false;
  modules=(new-object psobject -property @{ autoload=$true})
}

-------------------------------------------------------------------
Specify defaults and auto-load modules from .\my_modules folder
-------------------------------------------------------------------
$psake.config = new-object psobject -property @{
  defaultBuildFileName="default.ps1";
  taskNameFormat="Executing {0}";
  exitCode="1";
  verboseError=$false;
  modules=(new-object psobject -property @{ autoload=$true; directory=".\my_modules" })
}

-------------------------------------------------------------------
Specify defaults and explicitly load module(s)
-------------------------------------------------------------------
$psake.config = new-object psobject -property @{
  defaultBuildFileName="default.ps1";
  taskNameFormat="Executing {0}";
  exitCode="1";
  verboseError=$false;
  modules=(new-object psobject -property @{
    autoload=$false; 
    module=(new-object psobject -property @{path="c:\module1dir\module1.ps1"}), 
           (new-object psobject -property @{path="c:\module1dir\module2.ps1"})
  })
}
#>
properties {
  $x = 1
}

FormatTaskName "[{0}]"

task default -depends Verify 

task Verify -description "This task verifies psake's variables" {

  #Verify the exported module variables
  cd variable:
  Assert (Test-Path "psake") "variable psake was not exported from module"
  
  Assert ($psake.ContainsKey("build_success")) "'psake' variable does not contain key 'build_success'"
  Assert ($psake.ContainsKey("version")) "'psake' variable does not contain key 'version'"
  Assert ($psake.ContainsKey("build_script_file")) "'psake' variable does not contain key 'build_script_file'"
  Assert ($psake.ContainsKey("framework_version")) "'psake' variable does not contain key 'framework_version'"
  Assert ($psake.ContainsKey("build_script_dir")) "'psake' variable does not contain key 'build_script_dir'"  
  Assert ($psake.ContainsKey("config")) "'psake' variable does not contain key 'config'"  
  
  Assert (!$psake.build_success) '$psake.build_success should be $false'
  Assert ($psake.version) '$psake.version was null or empty'
  Assert ($psake.build_script_file) '$psake.build_script_file was null' 
  Assert ($psake.build_script_file.Name -eq "writing_psake_variables_should_pass.ps1") ("psake variable: {0} was not equal to 'writing_psake_variables_should_pass.ps1'" -f $psake.build_script_file.Name)
  Assert ($psake.build_script_dir) '$psake variable: $psake.build_script_dir was null or empty'
  Assert ($psake.framework_version) '$psake variable: $psake.framework_version was null or empty'

  Assert ($psake.context.Count -eq 1) '$psake.context should have had a length of one (1) during script execution'
  
  Assert ($psake.config) '$psake.config is $null'
  Assert ($psake.config.defaultBuildFileName -eq "default.ps1") '$psake.config.defaultBuildFileName not equal to "default.ps1"'
  Assert ($psake.config.taskNameFormat -eq "Executing {0}") '$psake.config.taskNameFormat not equal to "Executing {0}"'
  Assert ($psake.config.verboseError -eq $false) '$psake.config.verboseError not equal to $true'
  Assert ($psake.config.modules) '$psake.config.modules is $null'
}
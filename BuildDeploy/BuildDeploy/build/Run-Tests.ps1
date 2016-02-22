function Run-Tests {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )
			
	$configPaths = Get-ConfigurationPaths
	New-Item -ItemType directory -Path ($configPaths.DeployScriptsPath + '\logs') -Force
    $resultPath = $configPaths.DeployScriptsPath + '\logs\xUnitResults.xml'

	$xunitExitCode = Invoke-RunXUnitTests -RunTestsFrom '*.UnitTests.*' -DoNotRunTestsFrom '*\obj\*', '*\Debug\*' -ResultFormat 'xml' -ResultPath $resultPath
		
	if ($xunitExitCode -eq 0) {
        Write-Log -Info 'All unit tests succeeded.'
    } else {
        throw "xUnit runner failed with exit code $xunitExitCode."
    }	
}

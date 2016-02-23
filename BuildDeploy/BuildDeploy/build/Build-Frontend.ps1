function Build-Frontend {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )

    $configPaths = Get-ConfigurationPaths
    $path = Join-Path -Path $configPaths.ProjectRootPath -ChildPath 'wwwroot'
	$outputPath = Join-Path -Path $configPaths.ProjectRootPath -ChildPath '../bin/KnightFrank.Antares.WebClient/KnightFrank.Antares.WebClient.zip'	
	$stackName = "build"
	
	Write-Host "npm package restore"
	
    Push-Location -Path (Join-Path -Path $configPaths.ProjectRootPath -ChildPath 'wwwroot') -StackName $stackName;
		
	& "npm" install --supress-warnings
		if ($LastExitCode -ne 0) {
			Write-Error "Npm package restore failed";
		exit 1;
	}

	Write-Host "gulp default task"

	& "gulp" default
		if ($LastExitCode -ne 0) {
			Write-Error "gulp default task";
		exit 1;
	}
    
	# TODO perhaps use Build-DirPackage
	New-Zip -Path $path -Include @("index.html","web.config") -OutputFile $outputPath

	Pop-Location -StackName $stackName
}
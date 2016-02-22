function Build-Frontend {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )

    $configPaths = Get-ConfigurationPaths
    
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
    Compress-With7Zip -PathsToCompress @("./index.html","./web.config") -OutputFile "../../bin/KnightFrank.Antares.WebClient/KnightFrank.Antares.WebClient.zip" -IncludeRecurse $true

	Pop-Location -StackName $stackName
}
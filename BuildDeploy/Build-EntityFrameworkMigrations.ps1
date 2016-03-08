function Build-EntityFrameworkMigrations {   
    [CmdletBinding()]
    [OutputType([void])]
    param(		
        [Parameter(Mandatory=$true)]
        [string]
        $PackagePath,

        [Parameter(Mandatory=$true)]
        [string]
        $PackageName,

		[Parameter(Mandatory=$true)]
        [string]
        $ProjectPath,

        [Parameter(Mandatory=$true)]
        [string]
        $MigrationsDir,

        [Parameter(Mandatory=$true)]
        [string]
        $EntityFrameworkDir,

		[Parameter(Mandatory=$true)]
        [string]
        $BuildConfiguration
    )
	
    Write-Host -Object "Building Entity Framework migrations."

	Import-Module -Name "$PSScriptRoot\Invoke-MsBuild.psm1"
	
    if (![string]::IsNullOrEmpty($ProjectPath) -and !(Test-Path -LiteralPath $ProjectPath)) {
        throw "Given project file '$ProjectPath' does not exist for '$PackageName'."
    }
	
    $requiredTools = @('EntityFramework*.dll', 'migrate.exe')
    $pathsToCheck = @($MigrationsDir)
    if ($EntityFrameworkDir) {
        $pathsToCheck += $EntityFrameworkDir
        $pathsToCheck += Join-Path -Path $EntityFrameworkDir -ChildPath 'lib\net45'
        $pathsToCheck += Join-Path -Path $EntityFrameworkDir -ChildPath 'tools'
    }
    $requiredToolsPaths = @()
    foreach ($toolName in $requiredTools) {
        $found = $false
        foreach ($basePath in $pathsToCheck) {
            $path = Join-Path -Path $basePath -ChildPath $toolName
            if (Test-Path -Path $path) {
                $requiredToolsPaths += $path
                $found = $true
                break
            }
        }
        if (!$found) {
            throw "$toolName cannot be found - tried $($pathsToCheck -join ', ') (package '$PackageName')."
        }
    }  
    
	$buildSucceeded = Invoke-MsBuild -Path $ProjectPath -Params "/target:Clean;Build /property:Configuration=$BuildConfiguration" -ShowBuildWindow -AutoLaunchBuildLog

    if ($buildSucceeded)
    { 
        Write-Host -Object "DAL build completed successfully." 
    }
    else
    {         
        throw "DAL build failed. Check the build log file for errors."
    }

    $MigrationsFileWildcard = '*.dll'

    if (!(Test-Path -Path "$migrationsDir\$MigrationsFileWildcard")) {
        throw "There are no $MigrationsFileWildcard file(s) at '$migrationsDir' - please ensure `$migrationsDir points to the directory with compiled migrations."
    }
		
    Write-Host "Building package '$PackageName'."
    [void](New-Item -Path $PackagePath -ItemType Directory -Force)

    [void](Copy-Item -Path "$migrationsDir\$MigrationsFileWildcard" -Destination $PackagePath)
    foreach ($path in $requiredToolsPaths) {
        [void](Copy-Item -Path $path -Destination $PackagePath)
    }

	Write-Host "Package '$PackageName' built successfully."
}
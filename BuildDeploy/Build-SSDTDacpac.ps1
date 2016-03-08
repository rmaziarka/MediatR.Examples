function Build-SSDTDacpac {
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $ProjectPath,

        [Parameter(Mandatory=$true)]
        [string] 
        $BuildConfiguration
    )	

    Write-Host -Object "Building Database project."

    Import-Module -Name "$PSScriptRoot\Invoke-MsBuild.psm1"

	$buildSucceeded = Invoke-MsBuild -Path $ProjectPath -Params "/target:Clean;Build /property:Configuration=$BuildConfiguration" -ShowBuildWindow -AutoLaunchBuildLog

    if ($buildSucceeded)
    { 
        Write-Host -Object "Database build completed successfully." 
    }
    else
    {         
        throw "Database build failed. Check the build log file for errors."
    }
}
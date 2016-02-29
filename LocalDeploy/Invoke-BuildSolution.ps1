function Invoke-BuildSolution
{
    [CmdletBinding()]
    [OutputType([void])]
    param
    (    
        [Parameter(Mandatory=$true)]
        [string]
        $SolutionPath,

        [Parameter(Mandatory=$false)]
        [string]
        $BuildConfiguration = 'Release'
    )	
    
    Write-Host -Object "Start build solutions ..."

    # ===========================================================
    # Ms-Build
    # ===========================================================	
    Import-Module -Name "$PSScriptRoot\Invoke-MsBuild.psm1"
        
    $buildSucceeded = Invoke-MsBuild -Path $SolutionPath -Params "/target:Clean;Build /property:Configuration=$BuildConfiguration" -ShowBuildWindow -AutoLaunchBuildLog

    if ($buildSucceeded)
    { 
        Write-Host -Object "Build completed successfully." 
    }
    else
    {         
        throw "Build failed. Check the build log file for errors."
    }
}

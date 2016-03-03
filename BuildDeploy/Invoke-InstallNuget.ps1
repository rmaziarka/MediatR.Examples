function Invoke-InstallNuget
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $SolutionPath,

        [Parameter(Mandatory=$true)]
        [string]
        $NugetPackageOutputPath,

        [Parameter(Mandatory=$false)]
        [string]
        $Nuget = "$env:TEMP\NuGet.exe",

        [Parameter(Mandatory=$true)]
        [string]
        $PSScriptPackagePath,

        [Parameter(Mandatory=$true)]
        [string]
        $XUnitRunnerConsoleVersion
    )

    Write-Host -Object "Start installing nuget packages ..."
	
    # ===========================================================
    # Download nuget.exe
    # ===========================================================	
	if (!(Test-Path -Path $Nuget)) 
    {
        Invoke-WebRequest -Uri 'http://nuget.org/nuget.exe' -OutFile $Nuget  

        if (!(Test-Path -Path $Nuget)) 
        {
            Write-Host -Object "Failed to download nuget.exe. Please download Nuget manually and set Nuget parameter in function Install-Nuget."
            throw "Failed to download nuget.exe. Please download Nuget manually and set Nuget parameter in function Install-Nuget."
        }

        Write-Host -Object 'Nuget.exe downloaded successfully'
    } 
    else 
    {
        Write-Host -Object 'Nuget.exe already exists'
    }

    # ===========================================================
    # Create \src\packages dictionary
    # ===========================================================     
    if (!(Test-Path -Path $NugetPackageOutputPath))
    {   
        New-Item -Path $NugetPackageOutputPath -ItemType directory
        Write-Host -Object "Created $NugetPackageOutputPath"
    }

    if (!(Test-Path -Path $NugetPackageOutputPath))
    {
        Write-Host -Object "Failed to create packages output directory '$NugetPackageOutputPath'"
        throw "Failed to create packages output directory '$NugetPackageOutputPath'"
    }
    
    # ===========================================================
    # Nuget install
    # ===========================================================
    & $Nuget restore $SolutionPath -OutputDirectory $NugetPackageOutputPath
    if ($LASTEXITCODE -ne 0) 
    {
        throw "Failed install packages for solution $SolutionPath"
    }

    # ===========================================================
    # Nuget install xunit.runner.console
    # ===========================================================
    if (Test-Path -Path $PSScriptPackagePath)
    {
        Remove-Item -Path $PSScriptPackagePath -Force -Recurse
        New-Item -Path $PSScriptPackagePath -ItemType directory
        Write-Host -Object "Cleanup $PSScriptPackagePath"
    }

    & $Nuget install 'xunit.runner.console' -Version $XUnitRunnerConsoleVersion -OutputDirectory $PSScriptPackagePath -ExcludeVersion    
    if ($LASTEXITCODE -ne 0) 
    {
        throw "Failed to install xunit.runner.console"
    }

    $xUnit = "$PSScriptPackagePath\xunit.runner.console\tools\xunit.console.x86.exe"
    if (!(Test-Path -Path $xUnit))
    {
        Write-Host -Object "XUnit $xUnit not exist"
        throw "XUnit $xUnit not exist"
    }
}

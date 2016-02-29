function Invoke-InstallNuget
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectSrcPath,

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

    $packagesOutput = Join-Path -Path $ProjectSrcPath -ChildPath 'packages'  
     
    if (!(Test-Path -Path $packagesOutput))
    {   
        New-Item -Path $packagesOutput -ItemType directory
        Write-Host -Object "Created $packagesOutput"
    }

    if (!(Test-Path -Path $packagesOutput))
    {
        Write-Host -Object "Failed to create packages output directory '$packagesOutput'"
        throw "Failed to create packages output directory '$packagesOutput'"
    }
    
    # ===========================================================
    # Nuget install
    # ===========================================================    
    $packages = Get-ChildItem -Path $ProjectSrcPath -Recurse -Filter "packages.config" | Select-Object -ExpandProperty FullName

    foreach($package in $packages) 
    {
        & $Nuget restore $package -OutputDirectory $packagesOutput
        if ($LASTEXITCODE -ne 0) 
        {
            throw "Failed install $package"
        }        
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

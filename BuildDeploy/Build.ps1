[CmdletBinding()]
param
(
    [Parameter(Mandatory=$false)]
    [string]
    $ProjectRootPath = '..', # Modify this path according to your project structure. This is relative to the directory where build.ps1 resides ($PSScriptRoot).

    [Parameter(Mandatory=$false)]
    [string]
    $XUnitRunnerConsoleVersion = '2.1.0'
)

$global:ErrorActionPreference = 'Stop'

Push-Location -Path $PSScriptRoot

try 
{       
    $ProjectSrcPath = Join-Path -Path $ProjectRootPath -ChildPath 'src'    
    $ProjectSrcWwwrootPath = Join-Path -Path $ProjectSrcPath -ChildPath 'wwwroot'
    $PSScriptPackagePath = Join-Path -Path $PSScriptRoot -ChildPath 'packages'
    $XUnit = Join-Path -Path $PSScriptPackagePath -ChildPath 'xunit.runner.console\tools\xunit.console.x86.exe'
    
    
    . ".\Invoke-InstallNuget.ps1" 
    . ".\Invoke-BuildSolution.ps1"
    . ".\Invoke-RunUnitTests.ps1"
    . ".\Invoke-InstallNpm.ps1"
    . ".\Invoke-BuildWebsite.ps1"
    . ".\Invoke-RunWebsiteTests.ps1"

    # ===========================================================	
    Import-Module -Name "$PSScriptRoot\Get-Files.psm1"
    
    Invoke-InstallNuget -SolutionPath "$ProjectSrcPath\KnightFrank.Antares.Backend.sln" -NugetPackageOutputPath "$ProjectSrcPath\packages" -PSScriptPackagePath $PSScriptPackagePath -XUnitRunnerConsoleVersion $XUnitRunnerConsoleVersion

    Invoke-BuildSolution -SolutionPath "$ProjectSrcPath\KnightFrank.Antares.Backend.sln"

    Invoke-RunUnitTests -ProjectSrcPath $ProjectSrcPath -XUnit $XUnit

    Invoke-InstallNpm -ProjectSrcWwwrootPath $ProjectSrcWwwrootPath

    Invoke-BuildWebsite -ProjectSrcWwwrootPath $ProjectSrcWwwrootPath

    Invoke-RunWebsiteTests -ProjectSrcWwwrootPath $ProjectSrcWwwrootPath    
} 
finally 
{
    Pop-Location
}

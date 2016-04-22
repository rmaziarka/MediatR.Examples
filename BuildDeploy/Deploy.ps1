[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]
    $ProjectRootPath = '..', # Modify this path according to your project structure. This is relative to the directory where build resides ($PSScriptRoot).
	
	[Parameter(Mandatory=$false)]
    [switch]
    $DropExistingDatabase
    )

Push-Location $PSScriptRoot

try {

    . ".\Deploy-Database.ps1" 
    . ".\Build-EntityFrameworkMigrations.ps1"
	. ".\Deploy-EntityFrameworkMigrations.ps1"
	. ".\Invoke-SqlQuery.ps1"    
    . ".\Build-SSDTDacpac.ps1"
    . ".\Deploy-SSDTDacpac.ps1"
    . ".\Drop-Database.ps1"

    Deploy-Database -ProjectRootPath $ProjectRootPath -DropExistingDatabase:$DropExistingDatabase
    
} finally {
    Pop-Location
}
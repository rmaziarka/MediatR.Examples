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
    . ".\Configure-Jdbc.ps1"
    . ".\Set-NssmService"
    . ".\Start-ExternalProcess"
    . ".\Configure-Elasticsearch"

    $sqlDatabaseName = "KnightFrank.Antares"
    $sqlUser = "antares"
    $sqlPassword = "ant@res!1"	

    Deploy-Database -ProjectRootPath $ProjectRootPath -DropExistingDatabase:$DropExistingDatabase -DatabaseName $sqlDatabaseName -SqlUser $sqlUser -SqlPassword $sqlPassword
    
    Configure-Elasticsearch
    
    Configure-Jdbc -PathToSettingsTemplate "$ProjectRootPath\tools\KnightFrank.Antares.ResourceGroup\Templates\settings.json"`
                -PathToNssm "C:\nssm\nssm-2.24"`
                -PathToJdbc "C:\jdbc\elasticsearch-jdbc-2.3.2.0"`
                -SqlDatabaseName $sqlDatabaseName `
                -Schedule "0 0/1 * 1/1 * ? *"`
                -SqlUser $sqlUser `
                -SqlPassword $sqlPassword
    
} finally {
    Pop-Location
}
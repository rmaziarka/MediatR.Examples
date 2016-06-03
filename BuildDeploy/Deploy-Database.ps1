function Deploy-Database {
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $ProjectRootPath,

        [Parameter(Mandatory=$false)]
        [switch]
        $DropExistingDatabase,

        [Parameter(Mandatory=$true)]
        [string] 
        $DatabaseName,

        [parameter(Mandatory=$true)]
        [string]
        $SqlUser,

        [parameter(Mandatory=$true)]
        [string]
        $SqlPassword
    )	

	$packageName = 'KnightFrank.Antares.Dal'
	$outputPath = Join-Path -Path $PSScriptRoot -ChildPath "\migrations\$packageName"		
	$buildConfiguration = 'Release'

    $projectDatabasePath = Join-Path -Path $PSScriptRoot -ChildPath '\..\src\KnightFrank.Antares.Database'
    $projectDatabaseSqlProjPath = Join-Path -Path $projectDatabasePath -ChildPath 'KnightFrank.Antares.Database.sqlproj'
    $projectDatabaseDocpacPath = Join-Path -Path $projectDatabasePath -ChildPath "bin\$buildConfiguration\KnightFrank.Antares.Database.dacpac"
    $ssdtProfileNamePath = Join-Path -Path $projectDatabasePath -ChildPath '\Publish\local.publish.xml'

	$buildParams = @{
		PackagePath = $outputPath
		ProjectPath = Join-Path -Path $ProjectRootPath -ChildPath '\src\KnightFrank.Antares.Dal\KnightFrank.Antares.Dal.csproj'
		PackageName = $packageName
		MigrationsDir = Join-Path -Path $ProjectRootPath -ChildPath "\src\KnightFrank.Antares.Dal\bin\$buildConfiguration"
		EntityFrameworkDir = Join-Path -Path $ProjectRootPath -ChildPath '\src\packages\EntityFramework.6.1.3' 
		BuildConfiguration = $buildConfiguration
	}
		
    $IntegrationTestsDatabaseName = 'KnightFrank.Antares.Api.IntegrationTests'    
	$ConnectionString = "Server=localhost;Database=$DatabaseName;User Id=$SqlUser;Password=$SqlPassword"
	$IntegrationTestsConnectionString = "Server=localhost;Database=$IntegrationTestsDatabaseName;Integrated Security=True;"
	$MigrateAssembly = 'KnightFrank.Antares.Dal.dll'
	
    $sqlServerVersion = "2014"

	try { 
	
        Push-Location

		Invoke-SqlQuery -InputFile 'sql/Update-SqlLogin.sql'`
			    -SqlVariable "Username = $SqlUser","Password = $SqlPassword"

        Invoke-SqlQuery -InputFile 'sql/Update-SqlLoginRole.sql'`
			    -SqlVariable "Username = $SqlUser","Role = dbcreator"

        Invoke-SqlQuery -InputFile 'sql/Update-SqlLoginRole.sql'`
			    -SqlVariable "Username = $SqlUser","Role = bulkadmin"
		
        Pop-Location

        if($DropExistingDatabase)
        {
            Drop-Database -SqlDatabase $DatabaseName
            Drop-Database -SqlDatabase $IntegrationTestsDatabaseName
        }

	    Build-EntityFrameworkMigrations @buildParams

	    Deploy-EntityFrameworkMigrations -MigrateAssembly $MigrateAssembly -ConnectionString $ConnectionString -PackagePath $outputPath
	    
        Deploy-EntityFrameworkMigrations -MigrateAssembly $MigrateAssembly -ConnectionString $IntegrationTestsConnectionString -PackagePath $outputPath
		
        #due to the bug https://connect.microsoft.com/SQLServer/feedback/details/1420992/import-module-sqlps-may-take-longer-to-load-but-always-returns-warnings-when-the-azure-powershell-module-is-also-installed
        #the first call to Invoke-SqlCmd will return warnings
   
        
        Build-SSDTDacpac -ProjectPath $projectDatabaseSqlProjPath -BuildConfiguration $buildConfiguration
        
        $sqlCmdVariables = @{
            'OutputPath' = $projectDatabasePath
        }

        Import-Module -Name "$PSScriptRoot\Import-SqlServerDacDll.ps1"

        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath `
                          -ProfileName $ssdtProfileNamePath `
                          -SqlServerVersion $sqlServerVersion `
                          -ConnectionString $ConnectionString `
                          -SqlCmdVariables $sqlCmdVariables 

        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath `
                          -ProfileName $ssdtProfileNamePath `
                          -SqlServerVersion $sqlServerVersion `
                          -ConnectionString $IntegrationTestsConnectionString `
                          -SqlCmdVariables $sqlCmdVariables 

    } finally {
        Remove-Item -Path $outputPath -Force -Recurse
    }
}
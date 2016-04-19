function Deploy-Database {
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $ProjectRootPath
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
	
	$DatabaseName = 'KnightFrank.Antares'
	$ConnectionString = "Server=localhost;Database=KnightFrank.Antares;User Id=antares;Password=ant@res!1"
	$IntergrationTestsConnectionString = "Server=localhost;Database=KnightFrank.Antares.Api.IntegrationTests;Integrated Security=True;"
	$MigrateAssembly = 'KnightFrank.Antares.Dal.dll'
	$Username = "antares"
    $Password = "ant@res!1"
    $sqlServerVersion = "2014"

	try { 

	    Build-EntityFrameworkMigrations @buildParams

	    Deploy-EntityFrameworkMigrations -MigrateAssembly $MigrateAssembly -ConnectionString $ConnectionString -PackagePath $outputPath
	    
        Deploy-EntityFrameworkMigrations -MigrateAssembly $MigrateAssembly -ConnectionString $IntergrationTestsConnectionString -PackagePath $outputPath
		
        #due to the bug https://connect.microsoft.com/SQLServer/feedback/details/1420992/import-module-sqlps-may-take-longer-to-load-but-always-returns-warnings-when-the-azure-powershell-module-is-also-installed
        #the first call to Invoke-SqlCmd will return warnings

	    Invoke-SqlQuery -InputFile 'sql/Update-SqlLogin.sql'`
			    -SqlVariable "Username = $Username","Password = $Password"

        Invoke-SqlQuery -InputFile 'sql/Update-SqlLoginRole.sql'`
			    -SqlVariable "Username = $Username","Role = dbcreator"

        Invoke-SqlQuery -InputFile 'sql/Update-SqlLoginRole.sql'`
			    -SqlVariable "Username = $Username","Role = bulkadmin"
        
        Build-SSDTDacpac -ProjectPath $projectDatabaseSqlProjPath -BuildConfiguration $buildConfiguration
        
        $sqlCmdVariables = @{
            'OutputPath' = $projectDatabasePath
        }

        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath `
                          -ProfileName $ssdtProfileNamePath `
                          -SqlServerVersion $sqlServerVersion `
                          -ConnectionString $ConnectionString `
                          -SqlCmdVariables $sqlCmdVariables 

        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath `
                          -ProfileName $ssdtProfileNamePath `
                          -SqlServerVersion $sqlServerVersion `
                          -ConnectionString $IntergrationTestsConnectionString `
                          -SqlCmdVariables $sqlCmdVariables 

    } finally {
        Remove-Item -Path $outputPath -Force -Recurse
    }
}
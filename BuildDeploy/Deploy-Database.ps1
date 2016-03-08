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

	$deployParams = @{	
		PackagePath = $outputPath
		DatabaseName = 'KnightFrank.Antares'
		ConnectionString = "Server=localhost;Database=KnightFrank.Antares;Integrated Security=SSPI"
		MigrateAssembly = 'KnightFrank.Antares.Dal.dll'
		DefaultAppPoolUserName = "IIS AppPool\dev.api.antares.knightfrank.com"
	}

    $sqlServerVersion = "2014"

	try { 

	    Build-EntityFrameworkMigrations @buildParams

	    Deploy-EntityFrameworkMigrations @deployParams
	
	    $Username = $deployParams.DefaultAppPoolUserName
	    $DatabaseName = $deployParams.DatabaseName
	
        #due to the bug https://connect.microsoft.com/SQLServer/feedback/details/1420992/import-module-sqlps-may-take-longer-to-load-but-always-returns-warnings-when-the-azure-powershell-module-is-also-installed
        #the first call to Invoke-SqlCmd will return warnings

	    Invoke-SqlQuery -DatabaseName $deployParams.DatabaseName -InputFile 'sql/Update-SqlLogin.sql'`
			    -SqlVariable "Username = $Username"

	    Invoke-SqlQuery -DatabaseName $deployParams.DatabaseName -InputFile 'sql/Update-SqlUser.sql'`
			    -SqlVariable "Username = $Username","DatabaseName = $DatabaseName"

	    Invoke-SqlQuery -DatabaseName $deployParams.DatabaseName -InputFile 'sql/Update-SqlUserRole.sql'`
			    -SqlVariable "Username = $Username","DatabaseName = $DatabaseName","Role = db_datareader"

	    Invoke-SqlQuery -DatabaseName $deployParams.DatabaseName -InputFile 'sql/Update-SqlUserRole.sql'`
			    -SqlVariable "Username = $Username","DatabaseName = $DatabaseName","Role = db_datawriter"
        
        Build-SSDTDacpac -ProjectPath $projectDatabaseSqlProjPath -BuildConfiguration $buildConfiguration
        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath -ProfileName $ssdtProfileNamePath -SqlServerVersion $sqlServerVersion -ConnectionString $deployParams.ConnectionString
    } finally {
        Remove-Item -Path $outputPath -Force -Recurse
    }
}
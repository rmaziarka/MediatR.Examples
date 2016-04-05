function Deploy-Database {
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $ProjectRootPath
    )	
    	
	$buildConfiguration = 'Release'

    $projectDatabasePath = Join-Path -Path $PSScriptRoot -ChildPath '\..\src\KnightFrank.Antares.Database'
    $projectDatabaseSqlProjPath = Join-Path -Path $projectDatabasePath -ChildPath 'KnightFrank.Antares.Database.sqlproj'
    $projectDatabaseDocpacPath = Join-Path -Path $projectDatabasePath -ChildPath "bin\$buildConfiguration\KnightFrank.Antares.Database.dacpac"
    $ssdtProfileNamePath = Join-Path -Path $projectDatabasePath -ChildPath '\Publish\local.publish.xml'
    	
	$ConnectionString = "Server=localhost;Database=KnightFrank.Antares;Integrated Security=SSPI"
	$Username = "antares"
	$Password = "ant@res!1"
    $Role = 'dbcreator'
    $sqlServerVersion = "2014"

	try { 
	    
        #due to the bug https://connect.microsoft.com/SQLServer/feedback/details/1420992/import-module-sqlps-may-take-longer-to-load-but-always-returns-warnings-when-the-azure-powershell-module-is-also-installed
        #the first call to Invoke-SqlCmd will return warnings

	    Invoke-SqlQuery -InputFile 'sql/Update-SqlLogin.sql'`
			    -SqlVariable "Username = $Username","Password = $Password"

        Invoke-SqlQuery -InputFile 'sql/Update-SqlLoginRole.sql'`
			    -SqlVariable "Username = $Username","Role = $Role"
        
        Build-SSDTDacpac -ProjectPath $projectDatabaseSqlProjPath -BuildConfiguration $buildConfiguration
        
        Deploy-SSDTDacpac -ProjectDatabaseDocpacPath $projectDatabaseDocpacPath -ProfileName $ssdtProfileNamePath -SqlServerVersion $sqlServerVersion -ConnectionString $ConnectionString
    } finally {
        
    }
}
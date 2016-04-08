Configuration SetupSQLVm
{

Param ( [string] $nodeName )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$isoFileName = "en_sql_server_2014_developer_edition_with_service_pack_1_x86_dvd_6668541.iso"
$sqlInstanceName = "MSSQLSERVER"
$sqlFeatures = "SQLENGINE,IS,SSMS,ADV_SSMS"    
<#
$sqlUserName = "KnightFrank.Antares.Db.User"
$sqlUserPassword = ConvertTo-SecureString -AsPlainText '$Kf@admin' -Force
$sqlUserCredential = new-object -typename System.Management.Automation.PSCredential -argumentlist $sqlUserName, $sqlUserPassword
#>
Node $nodeName
{
	PSDscAllowPlainTextPassword = $true
	LocalConfigurationManager
    {
        RebootNodeIfNeeded = $true
		ActionAfterReboot = "ContinueConfiguration"
    }

	Script MapIsoShare
	{
		TestScript = { 
			$isoExists = Test-Path "c:\temp\$isoFileName"
			$shareMounted = Test-Path "k:\"
			$isoExists -or $shareMounted 
		}
		GetScript = {@{Result = "MapIsoShare"}}
		SetScript =
		{
			net use K: \\kfantarescommon.file.core.windows.net\iso /u:kfantarescommon IQURddUmGFAe1d3/+OC2Ay1gT9YfdwPi66Nzhy+3vWKgCcy6YA02i3EfLStunvNjCw4M9RW7pxe6dL5z3Y0X9w==	
		}
	}
	
	File CopySqlIso
	{
		DependsOn = "[Script]MapIsoShare"
		Ensure = "Present"
		SourcePath = "k:\$isoFileName"
		DestinationPath = "c:\temp\$isoFileName"
		Type = "File"
		MatchSource = $true
	}
	
	Script UnMapIsoShare 
	{
		DependsOn = "[File]CopySqlIso"	
		TestScript = { -Not (Test-Path "k:\") }
		GetScript = {@{Result = "UnMapIsoShare"}}
		SetScript =
		{
			net use K: /delete
		}
	}
	
	xMountImage MountSqlIso
	{
		DependsOn = "[File]CopySqlIso"
		Name = "SQL Disc"
		ImagePath = "c:\temp\$isoFileName"
		DriveLetter = "s:"
		Ensure = "Present"
	}
	<#
	xSqlServerSetup InstallSql
	{
		DependsOn = "[xMountImage]MountSqlIso"
		SourcePath = "s:\"
		InstallSharedDir = "C:\Program Files\Microsoft SQL Server"
        InstallSharedWOWDir = "C:\Program Files (x86)\Microsoft SQL Server"
        InstanceDir = "C:\Program Files\Microsoft SQL Server"
        InstallSQLDataDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
        SQLUserDBDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
        SQLUserDBLogDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
        SQLTempDBDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
        SQLTempDBLogDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
        SQLBackupDir = "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Data"
		InstanceName = $sqlInstanceName
		SecurityMode = "Mixed"
	}
	<#
	xSqlServerFirewall SetFirewallRules
    {
        DependsOn = ("[xSqlServerSetup]InstallSql")
        SourcePath = "s:\"
        InstanceName = $sqlInstanceName
        Features = $sqlFeatures
    }

	xSQLServerLogin SetupSqlLogin
    {
        DependsOn = ("[xSqlServerSetup]InstallSql")
        Ensure = "Present"
        Name = $sqlUserName
        LoginType = "SQLLogin"
		LoginCredential = $sqlUserCredential
    }

	xSQLServerDatabaseRole SetupSqlUser
    {
        DependsOn = ("[xSqlServerSetup]InstallSql")
        Ensure = "Present"
        Name = $sqlUserName
        Role = "dbcreator"
    }
	
	xMountImage UnMountSqlIso
	{
		DependsOn = "[xSqlServerSetup]InstallSql"
		ImagePath = 'c:\temp\$isoFileName'
        DriveLetter = 's:'
        Ensure = 'Absent'
	}
	#>
}
}
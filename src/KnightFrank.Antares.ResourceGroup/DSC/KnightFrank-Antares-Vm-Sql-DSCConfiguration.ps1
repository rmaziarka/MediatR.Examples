Configuration SetupSQLVm
{

Param ( [string] $nodeName, [System.Management.Automation.PSCredential] $setupCredential, [System.Management.Automation.PSCredential] $SQLSvcAccount )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$isoFileName = "en_sql_server_2014_developer_edition_with_service_pack_1_x86_dvd_6668541.iso"
$sqlInstanceName = "MSSQLSERVER"
$sqlFeatures = "SQLENGINE,IS,SSMS,ADV_SSMS"    

Node $nodeName
{
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
	
	xSqlServerSetup InstallSql
	{
		DependsOn = "[xMountImage]MountSqlIso"
		SetupCredential = $setupCredential
		SourcePath = "s:\"
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
	#>
	<#
	xMountImage UnMountSqlIso
	{
		DependsOn = "[xMountImage]MountSqlIso" # TODO: change dependency to InstallSql
		Name = "SQL Disc"
		ImagePath = "c:\temp\$isoFileName"
		DriveLetter = "s:"
		Ensure = "Absent"
	}
	#>
}
}
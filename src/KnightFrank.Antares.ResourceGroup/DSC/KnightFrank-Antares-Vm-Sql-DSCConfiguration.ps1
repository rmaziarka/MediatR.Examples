Configuration SetupSQLVm
{

Param ( [System.Management.Automation.PSCredential] $setupCredential, [System.Management.Automation.PSCredential] $SQLSvcAccount, [string] $sqlUserName )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$isoFileName = "en_sql_server_2014_developer_edition_with_service_pack_1_x64_dvd_6668542.iso"
$sqlInstanceName = "MSSQLSERVER"
$sqlFeatures = "SQLENGINE"    

<# 
	Node has to be explicitly set to localhost (and not host name as it is by default set by visual studio templates) - otherwise PSCredentials won't work.	
	https://blogs.msdn.microsoft.com/powershell/2014/09/10/secure-credentials-in-the-azure-powershell-desired-state-configuration-dsc-extension/ - paragraph Limitations point 1.
#>

Node localhost
{
	LocalConfigurationManager
    {
        RebootNodeIfNeeded = $true
		ActionAfterReboot = "ContinueConfiguration"
    }

	WindowsFeature NetFramework35Core
    {
        Name = "NET-Framework-Core"
        Ensure = "Present"
    }
 
    WindowsFeature NetFramework45Core
    {
        Name = "NET-Framework-45-Core"
        Ensure = "Present"
    }

	Script MapIsoShare
	{
		TestScript = { 
			$isoExists = Test-Path "c:\temp\$using:isoFileName"
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
		SourceFolder = ""
		InstanceName = $sqlInstanceName
		SecurityMode = "Mixed"
		Features = $sqlFeatures
	}

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
	
	xMountImage UnMountSqlIso
	{
		DependsOn = "[xSqlServerSetup]InstallSql"
		Name = "Unmount SQL Disc"
		ImagePath = "c:\temp\$isoFileName"
		DriveLetter = "s:"
		Ensure = "Absent"
	}
}
}
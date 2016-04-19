Configuration SetupSQLVm
{

Param ( 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $setupCredential, 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $sqlLoginCredential, 
[string][Parameter(Mandatory = $true)] $commonShareUrl, 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $commonShareCredential,
[string][Parameter(Mandatory = $true)] $isoFileName,
[string][Parameter(Mandatory = $true)] $sqlInstanceName )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$sqlFeatures = "SQLENGINE"    
$downloadPath = "c:\WindowsAzure"

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

	Script CopySqlIso
	{
		TestScript = { 
			Test-Path "$using:downloadPath\\$using:isoFileName"
		}
		GetScript = {@{Result = "CopySqlIso"}}
		SetScript =
		{
			New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $using:commonShareCredential
			Copy-Item p:\$using:isoFileName "$using:downloadPath\\$using:isoFileName"
			Remove-PSDrive -Name P
		}
	}

	xMountImage MountSqlIso
	{
		DependsOn = "[Script]CopySqlIso"
		Name = "SQL Disk"
		ImagePath = "$downloadPath\$isoFileName"
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
		Features = $sqlFeatures
		SAPwd = $setupCredential
		SecurityMode = "SQL"
	}

	xSQLServerNetwork SetupTcp
	{
		DependsOn = "[xSqlServerSetup]InstallSql"
		InstanceName = $sqlInstanceName
		ProtocolName = "tcp"
		TCPPort = 1433
		IsEnabled = $true
		RestartService = $true
	}

	xSqlServerFirewall SetFirewallRules
    {
        DependsOn = "[xSqlServerSetup]InstallSql"
        SourcePath = "s:\"
		SourceFolder = ""
        InstanceName = $sqlInstanceName
        Features = $sqlFeatures
    }

	xSQLServerLogin SetupSqlLogin
    {
        DependsOn = "[xSqlServerSetup]InstallSql"
        Ensure = "Present"
        Name = $sqlLoginCredential.UserName
        LoginType = "SQLLogin"
		LoginCredential = $sqlLoginCredential
    }
	
	xMountImage UnMountSqlIso
	{
		DependsOn = "[xSqlServerSetup]InstallSql"
		Name = "Unmount SQL Disk"
		ImagePath = "$downloadPath\$isoFileName"
		DriveLetter = "s:"
		Ensure = "Absent"
	}
	
	Script AddSqlRolesToLogin
	{
		DependsOn = "[xSQLServerLogin]SetupSqlLogin"
		TestScript = { $false }
		GetScript = {@{Result = "AddSqlRolesToLogin"}}
		SetScript = {
			$sqlConnection = "localhost"
			if ($using:sqlInstanceName -ne "MSSQLSERVER"){
				$sqlConnection = "localhost/$using:sqlInstanceName"
			}
			$sqlServer = new-object Microsoft.SqlServer.Management.Smo.Server $sqlConnection
			$userName = $using:sqlLoginCredential.UserName
			$login = $sqlServer.Logins | where { $_.Name -eq $userName }
			if (!$login){
				throw "SQL Login $userName does not exist." 
			}
			
			$login.AddToRole('dbcreator');
			$login.AddToRole('bulkadmin');

			Write-Verbose "Added $userName to server roles dbcreator and bulkadmin"
		}
	}
}
}
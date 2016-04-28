Configuration SetupSQLVm
{

Param ( 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $setupCredential, 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $sqlLoginCredential, 
[string][Parameter(Mandatory = $true)] $commonShareUrl, 
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $commonShareCredential,
[string][Parameter(Mandatory = $true)] $isoFileName,
[string][Parameter(Mandatory = $true)] $sqlInstanceName, 
[string] $octopusThumbprint)

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$sqlFeatures = "SQLENGINE"    
$downloadPath = "c:\\WindowsAzure"
$octopusDir = "C:\\Octopus"

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
	
    Script WaitForMountingImage
	{
		TestScript = { 
			$false
		}
		GetScript = {@{Result = "WaitForMountingImage"}}
		SetScript =
		{
            Get-PSDrive | Out-Null
		}
		DependsOn = "[xMountImage]MountSqlIso"
	}

	xSqlServerSetup InstallSql
	{
		DependsOn = "[Script]WaitForMountingImage"
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

	Script DownloadOctopusTentacle
    {
        TestScript = {
            Test-Path "C:\WindowsAzure\Octopus.Tentacle.3.3.6-x64.msi"
        }
        SetScript ={
            $source = "https://download.octopusdeploy.com/octopus/Octopus.Tentacle.3.3.6-x64.msi"
            $dest = "C:\WindowsAzure\Octopus.Tentacle.3.3.6-x64.msi"
            Invoke-WebRequest $source -OutFile $dest
        }
        GetScript = {@{Result = "DownloadOctopusTentacle"}}
    }

    Package InstallOctopusTentacle
    {
        Ensure = "Present"  
        Path  = "C:\WindowsAzure\Octopus.Tentacle.3.3.6-x64.msi"
        Name = "Octopus Deploy Tentacle"
        ProductId = "{D2622F6E-1377-47A6-9F6D-ED9AF593205D}"
        DependsOn = "[Script]DownloadOctopusTentacle"
    }

	File OctopusDir
	{
		DestinationPath = $octopusDir
		Ensure = "Present"
		Type = "Directory"
	}

	Script ConfigureOctopusTentacleAndSetAccessPermissions
	{
		DependsOn = @("[Package]InstallOctopusTentacle", "[File]OctopusDir", "[xSqlServerSetup]InstallSql")
		TestScript = {
			Test-Path "$using:octopusDir\Tentacle.config"
		}
		SetScript = {
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" create-instance --instance "Tentacle" --config "$using:octopusDir\Tentacle.config" --console
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" new-certificate --instance "Tentacle" --if-blank --console
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --reset-trust --console
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --home $using:octopusDir --app "$using:octopusDir\Applications" --port "10933" --noListen "False" --console
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --trust "$using:octopusThumbprint" --console
			& "netsh" advfirewall firewall add rule "name=Octopus Deploy Tentacle" dir=in action=allow protocol=TCP localport=10933
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" service --instance "Tentacle" --install --start --console
			
			$sqlWmi = Get-WmiObject win32_service | where name -eq 'mssqlserver'
			$sqlUser = New-Object System.Security.Principal.NTAccount($sqlWmi.Name)
			$acl = get-acl $using:octopusDir
			$accessRule = new-object System.Security.AccessControl.FileSystemAccessRule($sqlUser, 'Read', 'Allow')
			$acl.SetAccessRule($accessRule)
			Set-Acl -path $using:octopusDir -AclObject $acl
		}
		GetScript = {@{Result = "ConfigureOctopusTentacleAndSetAccessPermissions"}}
	}
}
}
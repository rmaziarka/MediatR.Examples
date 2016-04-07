Configuration SetupSQLVm
{

Param ( [string] $nodeName )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xSQLServer
Import-DscResource -ModuleName xStorage

$isoFileName = "en_sql_server_2014_developer_edition_with_service_pack_1_x86_dvd_6668541.iso";

Node $nodeName
{
	Script MapIsoShare
	{
		TestScript = { Test-Path "K:\" }
		GetScript = {@{Result = "MountIso"}}
		SetScript =
		{
			$accountName = "kfantarescommon"
			$accountKey = ConvertTo-SecureString "IQURddUmGFAe1d3/+OC2Ay1gT9YfdwPi66Nzhy+3vWKgCcy6YA02i3EfLStunvNjCw4M9RW7pxe6dL5z3Y0X9w==" -AsPlainText -Force
			$credential = new-object -TypeName System.Management.Automation.PSCredential -ArgumentList $accountName, $accountKey
			New-PSDrive -Name K -Root \\kfantarescommon.file.core.windows.net\iso -Credential $credential -PSProvider FileSystem
		}
	}

	File CopySqlIso
	{
		DependsOn = "[Script]MapIsoShare"
		Ensure = "Present"
		SourcePath = "k:\en_sql_server_2014_developer_edition_with_service_pack_1_x86_dvd_6668541.iso"
		DestinationPath = "c:\temp\en_sql_server_2014_developer_edition_with_service_pack_1_x86_dvd_6668541.iso"
		Type = "File"
		MatchSource = $true
		Force = $true
	}
	
	Script UnMapIsoShare 
	{
		DependsOn = "[File]Test"	
		TestScript = { -Not (Test-Path "K:\") }
		GetScript = {@{Result = "UnMountIso"}}
		SetScript =
		{
			Remove-PSDrive -Name K -Force
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
		SourcePath = "s:"
	}

	xMountImage UnMountSqlIso
	{
		DependsOn = "[xSqlServerSetup]InstallSql"
		ImagePath = 'c:\temp\$isoFileName'
        DriveLetter = 's:'
        Ensure = 'Absent'
	}
}
}
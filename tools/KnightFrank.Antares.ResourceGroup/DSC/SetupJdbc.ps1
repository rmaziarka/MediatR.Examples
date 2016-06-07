Configuration SetupJdbc
{
	Param ( 
		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchHost,

		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchPort = "9300",

		[Parameter(Mandatory = $true)]
		[string]
		$SqlIp,

		[Parameter(Mandatory = $true)]
		[string]
		$SqlPort,

		[Parameter(Mandatory = $true)]
		[string]
		$SqlDatabaseName,

		[Parameter(Mandatory = $true)]
		[string]
		$SqlUser,

		[Parameter(Mandatory = $true)]
		[string]
		$SqlPassword,

		[Parameter(Mandatory = $false)]
		[string]
		$ElasticsearchIndex,

		[Parameter(Mandatory = $true)]
		[string]
		$JdbcSchedule,

		[Parameter(Mandatory = $false)]
		[string]
		$BranchName = "master",

		[Parameter(Mandatory = $false)]
		[hashtable]
		$AdditionalJdbcConfigValues,

		[Parameter(Mandatory = $true)]
		[string]
		$PathToSettingsTemplate
		)

	$tempDownloadFolder = "$env:SystemDrive\temp\download\"
		
	$jdbcFileName = 'elasticsearch-jdbc-2.3.2.0.zip'
	$jdbcUnpack = "$env:SystemDrive\jdbc\$BranchName"
	$jdbcVersion = '2.3.2.0'
	$jdbcFolder = "$jdbcUnpack\elasticsearch-jdbc-$jdbcVersion"

    $nssmVersion = '2.24'
    $nssmUnpack = "$env:SystemDrive\nssm"
    $nssmFolder = "$nssmUnpack\nssm-$nssmVersion"

	Import-DscResource -Module cGripDevDSC
	Import-DscResource -Module xPSDesiredStateConfiguration

	<# 
		Node has to be explicitly set to localhost (and not host name as it is by default set by visual studio templates) - otherwise PSCredentials won't work.	
		https://blogs.msdn.microsoft.com/powershell/2014/09/10/secure-credentials-in-the-azure-powershell-desired-state-configuration-dsc-extension/ - paragraph Limitations point 1.
	#>

	Node localhost
	{
		LocalConfigurationManager
		{
			RebootNodeIfNeeded = $false
			ActionAfterReboot = "ContinueConfiguration"
		}
		#####
		#Download and set env variable for JDK
		#####
  
		xArchive UnzipJdbc {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $jdbcFileName
			Destination = $jdbcUnpack
			DestinationType = "Directory"
		}

		Script ConfigureJdbc
	    {
		    TestScript = { 
			    $false
		    }
		    GetScript = {@{Result = "ConfigureJdbc"}}
		    SetScript =
		    {
				$pathToExecute = "$using:jdbcFolder\bin\execute.bat" 
				$pathToSettings = "$using:jdbcFolder\bin\settings.json" 
				
				(Get-Content -Path $pathToExecute).replace('$jdbcFolder', "$using:jdbcFolder").replace('$javaFolder', $using:javaFolder) | Set-Content $pathToExecute
				
				$settings = Get-Content -Path $using:PathToSettingsTemplate -Raw | ConvertFrom-Json				
				$settings.jdbc.url = "jdbc:sqlserver://" + $using:SqlIp + ":" + $using:SqlPort+ ";databaseName=$using:SqlDatabaseName"
				$settings.jdbc.user = $using:SqlUser
				$settings.jdbc.password = $using:SqlPassword				
				$settings.jdbc."elasticsearch.host" = $using:ElasticsearchHost
				$settings.jdbc."elasticsearch.port" = $using:ElasticsearchPort
				if($using:ElasticsearchIndex)
				{
					$settings.jdbc.index = $using:ElasticsearchIndex
				}
				if($using:JdbcSchedule)
				{
					$settings.jdbc.schedule = $using:JdbcSchedule
				}
				if($using:AdditionalJdbcConfigValues) 
				{
					$settings.jdbc | Add-Member -PassThru -NotePropertyMembers $using:AdditionalJdbcConfigValues
				}
				$json = $settings | ConvertTo-Json -Depth 999				
				[System.IO.File]::WriteAllLines($pathToSettings, $json)
		    }
			DependsOn = '[xArchive]UnzipJdbc'
	    }

        cNssm StartJdbc {
            ExeFolder = "$jdbcFolder\bin"
            ExeOrBatName = "execute.bat"
            NssmFolder = $nssmFolder
            ServiceName = "jdbc-$jdbcVersion-$BranchName"
            ServiceDisplayName = "JDBC importer for Elasticsearch ($BranchName)"
            DependsOn = @('[Script]ConfigureJdbc')
        }
	}
}
Configuration SetupElasticsearchVmLocal
{
	Param ( 
		[Parameter(Mandatory = $true)]
		[string] 
		$CommonShareUrl,

		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchIp,

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

		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchIndex,

		[Parameter(Mandatory = $true)]
		[string]
		$JdbcSchedule,

		[Parameter(Mandatory = $true)]
		[string]
		$BranchName,

		[Parameter(Mandatory = $false)]
		[hashtable]
		$AdditionalJdbcConfigValues,    

		[Parameter(Mandatory = $false)]
		[hashtable]
		$ElasticsearchAdditionalConfigValues
		)

	$tempDownloadFolder = "$env:SystemDrive\temp\download\"

	$javaSource =  'java'
	$javaFileName = 'jdk-8u91-windows-x64.exe'
	$javaVersion = '8u91'
	$javaFolder = "$env:SystemDrive\Program Files\java\jdk\$javaVersion"

	$elasticsearchSource = 'elasticsearch'
	$elasticsearchFileName = 'elasticsearch-2.3.2.zip'
	$elasticsearchUnpack = "$env:SystemDrive\elasticsearch"    
	$elasticsearchVersion = '2.3.2'
	$elasticsearchFolder = "$elasticsearchUnpack\$elasticsearchVersion\elasticsearch-$elasticsearchVersion"
	$elasticsearchHost = "localhost"
	$elasticsearchPort = "9200"

	$jdbcSource =  'elasticsearch'
	$jdbcFileName = 'elasticsearch-jdbc-2.3.2.0.zip'
	$jdbcUnpack = "$env:SystemDrive\jdbc\$BranchName"
	$jdbcVersion = '2.3.2.0'
	$jdbcFolder = "$jdbcUnpack\elasticsearch-jdbc-$jdbcVersion"

	Import-DscResource -Module cGripDevDSC
	Import-DscResource -Module xPSDesiredStateConfiguration
	Import-DscResource -Module xNetworking

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
  
		Script CopyJDK
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:javaFileName"
		    }
		    GetScript = {@{Result = "CopyJDK"}}
		    SetScript =
		    {
				$secpasswd = ConvertTo-SecureString �P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==� -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds                
				Copy-Item (Join-Path - Path "P:" -ChildPath $using:javaSource | Join-Path -ChildPath $using:javaFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:javaFileName)
			    Remove-PSDrive -Name P
		    }
		}

		xPackage Java {
			Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
			Path =  Join-Path -Path $tempDownloadFolder -ChildPath $javaFileName
			Arguments = "/s INSTALLDIR=`"$javaFolder`" STATIC=1"
			ProductId = ''
			DependsOn = '[Script]CopyJDK'
		}
		  
		Environment SetJDKEnviromentVar
		{
			Name = "JAVA_HOME"
			Ensure = "Present"
			Path = $false
			Value = $javaFolder
			DependsOn = "[xPackage]Java"
		}

		Environment AddJDKToPath
		{
			Name = "JDK"
			Ensure = "Present"
			Path = $true
			Value = "$javaFolder\bin"
			DependsOn = "[xPackage]Java"
		}

		#####
		#Download and install elasticsearch
		#####

		Script CopyElasticsearchZip
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:elasticsearchFileName"
		    }
		    GetScript = {@{Result = "CopyElasticsearchZip"}}
		    SetScript =
		    {
				$secpasswd = ConvertTo-SecureString �P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==� -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path "P:" -ChildPath $using:elasticsearchSource | Join-Path -ChildPath $using:elasticsearchFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:elasticsearchFileName)
			    Remove-PSDrive -Name P
		    }
	    }
  
		xArchive UnzipElasticsearch {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $elasticsearchFileName
			Destination = Join-Path -Path $elasticsearchUnpack -ChildPath $elasticsearchVersion
			DependsOn = @('[Script]CopyElasticsearchZip')
			DestinationType= "Directory"
		}
  
		Script ConfigureElasticsearch
	    {
		    TestScript = { 
			    $false
		    }
		    GetScript = {@{Result = "ConfigureElasticsearch"}}
		    SetScript =
		    {
				$path = "$using:elasticsearchFolder\config\elasticsearch.yml" 
				$parameters = @{}
				$parameters."host.name" = $using:ElasticsearchIp
				if($using:ElasticsearchAdditionalConfigValues) {
					$parameters | Add-Member -PassThru -NotePropertyMembers $using:ElasticsearchAdditionalConfigValues
				}
				$yaml = $parameters.Keys | % { "$_ : " + $parameters.Item($_) }
				[System.IO.File]::WriteAllLines($path, $yaml)
		    }
			DependsOn = '[xArchive]UnzipElasticsearch'
	    }

		cElasticsearch InstallElasticsearch
		{
			DependsOn = @('[Script]ConfigureElasticsearch','[Environment]SetJDKEnviromentVar','[Environment]SetJDKEnviromentVar')
			UnzipFolder = Join-Path -Path $elasticsearchUnpack -ChildPath $elasticsearchVersion
		}

		xFirewall OpenElasticsearchPort
		{
			Access = 'Allow'
			Name = 'Elasticsearch'
			Direction = 'Inbound'
			Ensure = 'Present'
			LocalPort = '9200'
			Protocol = 'TCP'
			DependsOn = '[cElasticsearch]InstallElasticsearch'
		}

		#####
		#Download and install JDBC
		#####

		Script CopyJdbcZip
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:jdbcFileName"
		    }
		    GetScript = {@{Result = "CopyjdbcZip"}}
		    SetScript =
		    {
				$secpasswd = ConvertTo-SecureString �P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==� -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path "P:" -ChildPath $using:jdbcSource | Join-Path -ChildPath $using:jdbcFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:jdbcFileName)
			    Remove-PSDrive -Name P
		    }
	    }

		xArchive UnzipJdbc {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $jdbcFileName
			Destination = $jdbcUnpack
			DependsOn = @('[Script]CopyJdbcZip')
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
				#$pathToExecute = "$using:jdbcFolder\bin\execute.bat" 
				$pathToSettings = "$using:jdbcFolder\bin\settings.json" 
				
				#(Get-Content -Path $pathToExecute).replace('$jdbcFolder', "$using:jdbcFolder").replace('$javaFolder', $using:javaFolder) | Set-Content $pathToExecute
				
				$settings = Get-Content -Path $pathToSettings -Raw | ConvertFrom-Json
				$settings.jdbc.url = "jdbc:sqlserver://" + $using:SqlIp + ":" + $using:SqlPort+ ";databaseName=$using:SqlDatabaseName"
				$settings.jdbc.user = $using:SqlUser
				$settings.jdbc.password = $using:SqlPassword
				$settings.jdbc.index = $using:ElasticsearchIndex
				#$settings.jdbc."elasticsearch.Host" = $using:elasticsearchHost
				#$settings.jdbc."elasticsearch.Port" = $using:elasticsearchPort
				$settings.jdbc.schedule = $using:JdbcSchedule
				if($using:AdditionalJdbcConfigValues) {
					$settings.jdbc | Add-Member -PassThru -NotePropertyMembers $using:AdditionalJdbcConfigValues
				}
				
				$json = $settings | ConvertTo-Json -Depth 999
				Write-Verbose $json
				[System.IO.File]::WriteAllLines($pathToSettings, $json)
		    }
			DependsOn = '[xArchive]UnzipJdbc'
	    }

		<#        
		xWindowsProcess StartJdbc {
			Path = "$jdbcFolder\bin\execute.bat" 
			Arguments = ''
			StandardErrorPath = "$jdbcUnpack\elasticsearch-jdbc-$jdbcVersion\bin\log.txt" 
			DependsOn = '[Script]ConfigureJdbc'
		}
		#>

		xWindowsProcess StartJdbc {
			Path = "$javaFolder\bin\java.exe"
			Arguments = "-cp `"$jdbcFolder\lib\*`" -Dlog4j.configurationFile=`"$jdbcFolder\bin\log4j2.xml`" org.xbib.tools.Runner org.xbib.tools.JDBCImporter `"$jdbcFolder\bin\settings.json`""
			StandardErrorPath = "$jdbcFolder\log.txt" 
			DependsOn = '[Script]ConfigureJdbc'
		}
	}
}

SetupElasticsearchVmLocal `
-CommonShareUrl "\\kfaneiedevcommonst.file.core.windows.net\setupfiles" `
-SqlIp "localhost"`
-SqlPort "1433"`
-SqlDatabaseName "KnightFrank.Antares"`
-SqlUser "antares"`
-SqlPassword "ant@res!1"`
-ElasticsearchIndex "properties_v7"`
-JdbcSchedule "0 0/10 * * * ?"`
-ElasticsearchIp "localhost"

Start-DscConfiguration -Path .\SetupElasticsearchVmLocal -ComputerName localhost -Force -Verbose -Wait
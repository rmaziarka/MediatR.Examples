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

    $nssmSource = 'nssm'
    $nssmFileName = 'nssm-2.24.zip'
    $nssmVersion = '2.24'
    $nssmUnpack = "$env:SystemDrive\Program Files\nssm"
    $nssmFolder = "$nssmUnpack\nssm-$nssmVersion"

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
  

        File TempFolder {
            DestinationPath = $tempDownloadFolder
            Type = "Directory"
        }

		Script CopyJDK
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:javaFileName"
		    }
		    GetScript = {@{Result = "CopyJDK"}}
		    SetScript =
		    {
				$secpasswd = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds                
				Copy-Item (Join-Path -Path "P:" -ChildPath $using:javaSource | Join-Path -ChildPath $using:javaFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:javaFileName)
			    Remove-PSDrive -Name P
		    }
            DependsOn = '[File]TempFolder'
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
			Name = "Path"
			Ensure = "Present"
			Path = $true
			Value = "%JAVA_HOME%\bin"
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
				$secpasswd = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path -Path "P:" -ChildPath $using:elasticsearchSource | Join-Path -ChildPath $using:elasticsearchFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:elasticsearchFileName)
			    Remove-PSDrive -Name P
		    }
            DependsOn = '[File]TempFolder'
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
				$secpasswd = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path -Path "P:" -ChildPath $using:jdbcSource | Join-Path -ChildPath $using:jdbcFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:jdbcFileName)
			    Remove-PSDrive -Name P
		    }
            DependsOn = '[File]TempFolder'
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
				$pathToExecute = "$using:jdbcFolder\bin\execute.bat" 
				$pathToSettings = "$using:jdbcFolder\bin\settings.json" 
				
				(Get-Content -Path $pathToExecute).replace('$jdbcFolder', "$using:jdbcFolder").replace('$javaFolder', $using:javaFolder) | Set-Content $pathToExecute
				
				$settings = Get-Content -Path $pathToSettings -Raw | ConvertFrom-Json
				$settings.jdbc.url = "jdbc:sqlserver://" + $using:SqlIp + ":" + $using:SqlPort+ ";databaseName=$using:SqlDatabaseName"
				$settings.jdbc.user = $using:SqlUser
				$settings.jdbc.password = $using:SqlPassword
				$settings.jdbc.index = $using:ElasticsearchIndex
				$settings.jdbc."elasticsearch.Host" = $using:elasticsearchHost
				$settings.jdbc."elasticsearch.Port" = $using:elasticsearchPort
				$settings.jdbc.schedule = $using:JdbcSchedule
				if($using:AdditionalJdbcConfigValues) {
					$settings.jdbc | Add-Member -PassThru -NotePropertyMembers $using:AdditionalJdbcConfigValues
				}
				
				$json = $settings | ConvertTo-Json -Depth 999				
				[System.IO.File]::WriteAllLines($pathToSettings, $json)
		    }
			DependsOn = '[xArchive]UnzipJdbc'
	    }
        
        Script CopyNssm
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:nssmFileName"
		    }
		    GetScript = {@{Result = "CopyNssm"}}
		    SetScript =
		    {
				$secpasswd = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
				$mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:CommonShareUrl -Credential $mycreds                
				Copy-Item (Join-Path -Path "P:" -ChildPath $using:nssmSource | Join-Path -ChildPath $using:nssmFileName) (Join-Path -Path $using:tempDownloadFolder -ChildPath $using:nssmFileName)
			    Remove-PSDrive -Name P
		    }
            DependsOn = '[File]TempFolder'
		}

        xArchive UnzipNssm {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Destination = $nssmUnpack
			DependsOn = @('[Script]CopyNssm')
			DestinationType = "Directory"
		}

        cNssm StartJdbc {
            ExeFolder = "$jdbcFolder\bin"
            ExeOrBatName = "execute.bat"
            NssmFolder = $nssmFolder
            ServiceName = "jdbc-$jdbcVersion-$BranchName"
            DependsON = @('[xArchive]UnzipNssm', '[Script]ConfigureJdbc')
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
-ElasticsearchIp "localhost"`
-BranchName 'feature1'

Start-DscConfiguration -Path .\SetupElasticsearchVmLocal -ComputerName localhost -Force -Verbose -Wait
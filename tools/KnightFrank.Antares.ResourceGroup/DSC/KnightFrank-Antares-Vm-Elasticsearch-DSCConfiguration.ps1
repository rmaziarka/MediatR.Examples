Configuration SetupElasticsearchVm
{
	Param ( 
		[Parameter(Mandatory = $true)] 
		[string]
		$CommonShareUrl,

		[Parameter(Mandatory = $true)] 
		[System.Management.Automation.PSCredential]
		$CommonShareCredential,
		
		[Parameter(Mandatory = $true)] 
		[string]
		$JavaVersion,

		[Parameter(Mandatory = $true)] 
		[string]
		$JavaFileName,

		[Parameter(Mandatory = $true)] 
		[string]
		$ElasticsearchVersion,

		[Parameter(Mandatory = $true)] 
		[string]
		$ElasticsearchFileName,

		[Parameter(Mandatory = $true)] 
		[string]
		$JdbcFileName,

		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchIp,  

		[Parameter(Mandatory = $false)]
		[hashtable]
		$ElasticsearchAdditionalConfigValues
	)

	$tempDownloadFolder = "$env:SystemDrive\temp\download\"

	$javaSource =  'java'
	$javaFolder = "$env:SystemDrive\Program Files\java\jdk\$JavaVersion"

	$elasticsearchSource = 'elasticsearch'
	$elasticsearchUnpack = "$env:SystemDrive\elasticsearch"
	$elasticsearchFolder = "$elasticsearchUnpack\$ElasticsearchVersion\elasticsearch-$ElasticsearchVersion"
	$elasticsearchHost = "localhost"
	$elasticsearchPort = "9200"

	$jdbcSource =  'elasticsearch'
	$jdbcFileName = 'elasticsearch-jdbc-2.3.2.0.zip'	

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

		xRemoteFile CopyJDK {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
			Uri = Join-Path $CommonShareUrl -ChildPath $javaSource | Join-Path -ChildPath $JavaFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		xPackage Java {
			Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
			Path =  Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
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

		xRemoteFile CopyElasticsearchZip {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
			Uri = Join-Path $CommonShareUrl -ChildPath $JavaSource | Join-Path -ChildPath $JavaFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

	    xArchive UnzipElasticsearch {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $ElasticsearchFileName
			Destination = Join-Path -Path $elasticsearchUnpack -ChildPath $ElasticsearchVersion
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
			UnzipFolder = Join-Path -Path $elasticsearchUnpack -ChildPath $ElasticsearchVersion
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

		xRemoteFile CopyJdbcZip {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $JdbcFileName
			Uri = Join-Path $CommonShareUrl -ChildPath $jdbcSource | Join-Path -ChildPath $JdbcFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		xRemoteFile CopyNssm
	    {
		    DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Uri = Join-Path $CommonShareUrl -ChildPath $nssmSource | Join-Path -ChildPath $nssmFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

        xArchive UnzipNssm {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Destination = $nssmUnpack
			DependsOn = @('[xRemoteFile]CopyNssm')
			DestinationType = "Directory"
		}
	}
}
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
		$JdbcVersion,

		[Parameter(Mandatory = $true)]
		[string]
		$ElasticsearchIp,  

		[Parameter(Mandatory = $false)]
		[hashtable]
		$ElasticsearchAdditionalConfigValues,

		[Parameter(Mandatory= $true)]
		[string] 
		$OctopusThumbprint
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
	$jdbcUnpack = "$env:SystemDrive\jdbc\$BranchName"
	$jdbcFolder = "$jdbcUnpack\elasticsearch-jdbc-$jdbcVersion"

	$nssmSource = 'nssm'
    $nssmFileName = 'nssm-2.24.zip'
    $nssmVersion = '2.24'
    $nssmUnpack = "$env:SystemDrive\Program Files\nssm"
    $nssmFolder = "$nssmUnpack\nssm-$nssmVersion"

    $octopusFolder = "$env:SystemDrive\octopus"

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
			RebootNodeIfNeeded = $true
			ActionAfterReboot = "ContinueConfiguration"
		}
		#####
		#Download and set env variable for JDK
		#####
		
        File TempFolder {
            DestinationPath = $tempDownloadFolder
            Type = "Directory"
        }

		File CopyJava {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $javaSource | Join-Path -ChildPath $JavaFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		xPackage InstallJava {
			Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
			Path =  Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
			Arguments = "/s INSTALLDIR=`"$javaFolder`" STATIC=1"
			ProductId = ''
			DependsOn = '[File]CopyJava'
		}
		  
		Environment SetJavaHome
		{
			Name = "JAVA_HOME"
			Ensure = "Present"
			Path = $false
			Value = $javaFolder
			DependsOn = "[xPackage]InstallJava"
		}

		Environment AddJavaToPath
		{
			Name = "Path"
			Ensure = "Present"
			Path = $true
			Value = "%JAVA_HOME%\bin"
			DependsOn = "[xPackage]InstallJava"
		}

		#####
		#Download and install elasticsearch
		#####

		File CopyElasticsearch {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $ElasticsearchFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $ElasticsearchSource | Join-Path -ChildPath $ElasticsearchFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

	    xArchive UnzipElasticsearch {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $ElasticsearchFileName
			Destination = Join-Path -Path $elasticsearchUnpack -ChildPath $ElasticsearchVersion
			DestinationType= "Directory"
			DependsOn = @('[File]CopyElasticsearch')
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
			UnzipFolder = Join-Path -Path $elasticsearchUnpack -ChildPath $ElasticsearchVersion
			DependsOn = @('[Script]ConfigureElasticsearch','[Environment]SetJavaHome','[Environment]AddJavaToPath')
		}

		xFirewall OpenElasticsearchPort
		{
			Access = 'Allow'
			Name = 'Elasticsearch'
			Direction = 'Inbound'
			Ensure = 'Present'
			LocalPort = '9200'
			Protocol = 'TCP'
		}

		#####
		#Download and install JDBC
		#####

		File CopyJdbc {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $JdbcFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $jdbcSource | Join-Path -ChildPath $JdbcFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		File CopyNssm
	    {
		    DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $nssmSource | Join-Path -ChildPath $nssmFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

        xArchive UnzipNssm {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Destination = $nssmUnpack
			DependsOn = @('[File]CopyNssm')
			DestinationType = "Directory"
		}

		#####
		#Install Octopus Tentacle
		#####

        File OctopusFolder
		{
			DestinationPath = $octopusFolder
			Ensure = "Present"
			Type = "Directory"
		}

		xRemoteFile DownloadOctopusTentacle
		{
            Uri = "https://download.octopusdeploy.com/octopus/Octopus.Tentacle.3.3.6-x64.msi"
            DestinationPath = "$tempDownloadFolder\Octopus.Tentacle.3.3.6-x64.msi"
            DependsOn = @('[File]TempFolder')
		}

		Package InstallOctopusTentacle
		{
			Ensure = "Present"  
			Path  = "$tempDownloadFolder\Octopus.Tentacle.3.3.6-x64.msi"
			Name = "Octopus Deploy Tentacle"
			ProductId = "{D2622F6E-1377-47A6-9F6D-ED9AF593205D}"
			DependsOn = "[xRemoteFile]DownloadOctopusTentacle"
		}

		Script ConfigureOctopusTentacle
		{
			TestScript = {
				Test-Path "$using:octopusFolder\Tentacle.config"
			}
			SetScript = {
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" create-instance --instance "Tentacle" --config "$using:octopusFolder\Tentacle.config" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" new-certificate --instance "Tentacle" --if-blank --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --reset-trust --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --home $using:octopusFolder --app "$using:octopusFolder\Applications" --port "10933" --noListen "False" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --trust "$using:OctopusThumbprint" --console
				& "netsh" advfirewall firewall add rule "name=Octopus Deploy Tentacle" dir=in action=allow protocol=TCP localport=10933
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" service --instance "Tentacle" --install --start --console
			}
			GetScript = {@{Result = "ConfigureOctopusTentacle"}}
			DependsOn = @("[Package]InstallOctopusTentacle", "[File]OctopusFolder")
		}
	}
}
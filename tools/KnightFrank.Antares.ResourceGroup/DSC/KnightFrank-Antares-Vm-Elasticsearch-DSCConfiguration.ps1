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

		#####
		#Install Octopus Tentacle
		#####

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
			DestinationPath = "c:\Octopus"
			Ensure = "Present"
			Type = "Directory"
		}

		Script ConfigureOctopusTentacle
		{
			DependsOn = @("[Package]InstallOctopusTentacle", "[File]OctopusDir")
			TestScript = {
				Test-Path "C:\Octopus\Tentacle.config"
			}
			SetScript = {
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" create-instance --instance "Tentacle" --config "C:\Octopus\Tentacle.config" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" new-certificate --instance "Tentacle" --if-blank --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --reset-trust --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --home "C:\Octopus" --app "C:\Octopus\Applications" --port "10933" --noListen "False" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --trust "$using:OctopusThumbprint" --console
				& "netsh" advfirewall firewall add rule "name=Octopus Deploy Tentacle" dir=in action=allow protocol=TCP localport=10933
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" service --instance "Tentacle" --install --start --console
			}
			GetScript = {@{Result = "ConfigureOctopusTentacle"}}
		}
	}
}
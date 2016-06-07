Configuration SetupElasticsearchVm
{
Param (
[string][Parameter(Mandatory = $true)] $CommonShareUrl,
[string][Parameter(Mandatory = $true)] $JavaVersion,
[string][Parameter(Mandatory = $true)] $JavaFileName,
[string][Parameter(Mandatory = $true)] $ElasticsearchVersion,
[string][Parameter(Mandatory = $true)] $ElasticsearchFileName,
[string][Parameter(Mandatory = $true)] $JdbcFileName,
[string][Parameter(Mandatory = $true)] $JdbcVersion,
[string][Parameter(Mandatory = $false)]	$ElasticsearchIp,
[System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] $CommonShareCredential,
[string][Parameter(Mandatory= $true)] $OctopusThumbprint)

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
    $nssmUnpack = "$env:SystemDrive\nssm"
    $nssmFolder = "$nssmUnpack\nssm-$nssmVersion"

    $octopusFolder = "$env:SystemDrive\octopus"

	$CommonShareUrl = "$CommonShareUrl\setupfiles"

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

		Script CopyFiles
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder/$using:javaFileName"
		    }
		    GetScript = {@{Result = "CopyFiles"}}
		    SetScript =
		    {
			    New-PSDrive -Name P -PSProvider FileSystem -Root "$using:CommonShareUrl" -Credential $using:CommonShareCredential
			    Copy-Item "P:\$using:javaSource\$using:javaFileName" "$using:tempDownloadFolder\$using:javaFileName"
			    Copy-Item "P:\$using:jdbcSource\$using:jdbcFileName" "$using:tempDownloadFolder\$using:jdbcFileName"
			    Remove-PSDrive -Name P
		    }
	    }

		xRemoteFile CopyElasticsearch {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $elasticsearchFileName
			Uri = "https://download.elastic.co/elasticsearch/release/org/elasticsearch/distribution/zip/elasticsearch/2.3.2/elasticsearch-2.3.2.zip"			
            DependsOn = '[File]TempFolder'
		}

        xRemoteFile CopyNssm
	    {
		    DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Uri = "https://nssm.cc/release/nssm-2.24.zip"			
            DependsOn = '[File]TempFolder'
		}

		xPackage InstallJava {
			Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
			Path =  Join-Path -Path $tempDownloadFolder -ChildPath $JavaFileName
			Arguments = "/s INSTALLDIR=`"$javaFolder`" STATIC=1"
			ProductId = ''
			DependsOn = '[Script]CopyFiles'
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

		
	    xArchive UnzipElasticsearch {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $ElasticsearchFileName
			Destination = Join-Path -Path $elasticsearchUnpack -ChildPath $ElasticsearchVersion
			DestinationType= "Directory"
			DependsOn = @('[xRemoteFile]CopyElasticsearch')
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
                if($using:ElasticsearchIp)
                {
                    $parameters."network.host" = $using:ElasticsearchIp
                }
                else
                {
                    $parameters."network.host" = ((ipconfig | findstr [0-9].\.)[0]).Split()[-1]
                }
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
				
        xArchive UnzipNssm {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $nssmFileName
			Destination = $nssmUnpack
			DependsOn = @('[xRemoteFile]CopyNssm')
			DestinationType = "Directory"
		}

		#####
		#Install Octopus Tentacle
		#####

        File OctopusDir
    {
        DestinationPath = "c:\Octopus"
        Ensure = "Present"
        Type = "Directory"
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
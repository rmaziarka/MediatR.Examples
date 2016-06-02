Configuration SetupElasticsearchVmLocal
{
	Param ( 
		[Parameter(Mandatory = $true)]
		[string] 
		$CommonShareUrl,

		[Parameter(Mandatory = $true)] 
		[System.Management.Automation.PSCredential]
		$CommonShareCredential,

		[Parameter(Mandatory = $false)]
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

	$OctopusThumbprint = 'C478B5952045B390F15F0C444D4917E911713EF0'

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

        File CopyJava {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $javaFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $javaSource | Join-Path -ChildPath $javaFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		xPackage InstallJava {
			Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
			Path =  Join-Path -Path $tempDownloadFolder -ChildPath $javaFileName
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
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $elasticsearchFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $elasticsearchSource | Join-Path -ChildPath $elasticsearchFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}
  
		xArchive UnzipElasticsearch {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $elasticsearchFileName
			Destination = Join-Path -Path $elasticsearchUnpack -ChildPath $elasticsearchVersion
			DependsOn = @('[File]CopyElasticsearch')
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
				if($using:ElasticsearchIp)
                {
                    $parameters."host.name" = $using:ElasticsearchIp
                }
                else
                {
                    $parameters."host.name" = ((ipconfig | findstr [0-9].\.)[0]).Split()[-1]
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
			DependsOn = @('[Script]ConfigureElasticsearch','[Environment]SetJavaHome','[Environment]AddJavaToPath')
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

		File CopyJdbc {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $jdbcFileName
			SourcePath = Join-Path $CommonShareUrl -ChildPath $jdbcSource | Join-Path -ChildPath $jdbcFileName
			Credential = $CommonShareCredential
            DependsOn = '[File]TempFolder'
		}

		xArchive UnzipJdbc {
			Path = Join-Path -Path $tempDownloadFolder -ChildPath $jdbcFileName
			Destination = $jdbcUnpack
			DependsOn = @('[File]CopyJdbc')
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
				if($using:AdditionalJdbcConfigValues) {
					$settings.jdbc | Add-Member -PassThru -NotePropertyMembers $using:AdditionalJdbcConfigValues
				}
				
				$json = $settings | ConvertTo-Json -Depth 999				
				[System.IO.File]::WriteAllLines($pathToSettings, $json)
		    }
			DependsOn = '[xArchive]UnzipJdbc'
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

        cNssm StartJdbc {
            ExeFolder = "$jdbcFolder\bin"
            ExeOrBatName = "execute.bat"
            NssmFolder = $nssmFolder
            ServiceName = "jdbc-$jdbcVersion-$BranchName"
            DependsOn = @('[xArchive]UnzipNssm', '[Script]ConfigureJdbc')
        }

		Script DownloadOctopusTentacle
		{
			TestScript = {
				Test-Path "$using:tempDownloadFolder\Octopus.Tentacle.3.3.6-x64.msi"
			}
			SetScript ={
				$source = "https://download.octopusdeploy.com/octopus/Octopus.Tentacle.3.3.6-x64.msi"
				$dest = "$using:tempDownloadFolder\Octopus.Tentacle.3.3.6-x64.msi"
				Invoke-WebRequest $source -OutFile $dest
			}
			GetScript = {@{Result = "DownloadOctopusTentacle"}}
		}

		Package InstallOctopusTentacle
		{
			Ensure = "Present"  
			Path  = "$tempDownloadFolder\Octopus.Tentacle.3.3.6-x64.msi"
			Name = "Octopus Deploy Tentacle"
			ProductId = "{D2622F6E-1377-47A6-9F6D-ED9AF593205D}"
			DependsOn = "[Script]DownloadOctopusTentacle"
		}

		File OctopusFolder
		{
			DestinationPath = $octopusFolder
			Ensure = "Present"
			Type = "Directory"
		}

		Script ConfigureOctopusTentacle
		{
			DependsOn = @("[Package]InstallOctopusTentacle", "[File]OctopusFolder")
			TestScript = {
				Test-Path "$using:octopusDir\Tentacle.config"
			}
			SetScript = {
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" create-instance --instance "Tentacle" --config "$using:octopusDir\Tentacle.config" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" new-certificate --instance "Tentacle" --if-blank --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --reset-trust --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --home $using:octopusDir --app "$using:octopusDir\Applications" --port "10933" --noListen "False" --console
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --trust "$using:OctopusThumbprint" --console
				& "netsh" advfirewall firewall add rule "name=Octopus Deploy Tentacle" dir=in action=allow protocol=TCP localport=10933
				& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" service --instance "Tentacle" --install --start --console
			}
			GetScript = {@{Result = "ConfigureOctopusTentacle"}}
		}
	}
}

$ConfigData = @{
    AllNodes = @(
        @{
            NodeName                    = "localhost"
            PSDscAllowPlainTextPassword = $true
        }
    )
}

$securedPassword = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $securedPassword)

SetupElasticsearchVmLocal `
-ConfigurationData $ConfigData `
-CommonShareCredential $credentials `
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
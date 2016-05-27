Configuration SetupElasticsearchVm
{
	Param ( 
		[Parameter(Mandatory = $true)] 
        [string]
        $commonShareUrl,

		[Parameter(Mandatory = $true)] 
		[System.Management.Automation.PSCredential]		
		$commonShareCredential,
		
		[Parameter(Mandatory = $true)] 
		[string]
		$javaVersion,

		[Parameter(Mandatory = $true)] 
		[string]
		$javaFileName,

		[Parameter(Mandatory = $true)] 
		[string]
		$elasticsearchVersion,

		[Parameter(Mandatory = $true)] 
		[string]
		$elasticsearchFileName,

		[Parameter(Mandatory = $true)] 
		[string]
		$jdbcFileName,

		[Parameter(Mandatory = $true)] 
		[string]
		$jdbcVersion
	)

    $tempDownloadFolder = "$env:SystemDrive\temp\download\"
    
    $javaSource =  'java'
    $javaFolder = "$env:SystemDrive\Program Files\java\jdk\$javaVersion\"
        
    $elasticsearchSource = 'elasticsearch'
    $elasticsearchFolder = "$env:SystemDrive\elasticsearch"
        
    $jdbcSource =  'elasticsearch'
    $jdbcFolder = "$env:SystemDrive\jdbc"
    
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
  
        xPackage _7zip {
            Name = '7-Zip 9.20 (x64 edition)'
            Path = 'http://www.7-zip.org/a/7z920-x64.msi'
            ProductId = '23170F69-40C1-2702-0920-000001000000'
            Ensure = 'Present'
        }        

        xRemoteFile CopyJDK {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $javaFileName
			Uri = Join-Path $commonShareUrl -ChildPath $javaSource | Join-Path -ChildPath $javaFileName
			Credential = $commonShareCredential
		}

        xPackage Java {
            Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
            Path =  Join-Path $tempDownloadFolder $javaFileName
            Arguments = "/s INSTALLDIR=`"$javaFolder`" STATIC=1"
            ProductId = ''
			DependsOn = '[xRemoteFile]CopyJDK'
        }
          
        Environment SetJDKEnviromentVar
        {
            Name = "JAVA_HOME"
            Ensure = "Present"
            Path = $false
            Value = $javaFolder
            DependsOn = "[xPackage]Java"
        }

        #####
        #Download and install elasticsearch
        #####
          
	    xRemoteFile CopyElasticsearchZip {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $javaFileName
			Uri = Join-Path $commonShareUrl -ChildPath $javaSource | Join-Path -ChildPath $javaFileName
			Credential = $commonShareCredential
		}
  
        xArchive UnzipElasticsearch {
            Path = Join-Path $tempDownloadFolder $elasticsearchFileName
            Destination = Join-Path $elasticsearchFolder $elasticsearchVersion
            DependsOn = @('[xPackage]_7zip', '[xRemoteFile]CopyElasticsearchZip')
			DestinationType = "Directory"
        }
  
        Script ConfigureElasticsearch
	    {
		    TestScript = { 
			    $true
		    }
		    GetScript = {@{Result = "ConfigureElasticsearch"}}
		    SetScript =
		    {
                $path = "$elasticsearchFolder\$elasticsearchVersion\elasticsearch-$elasticsearchVersion\config\elasticsearch.yml" 
                #replace placeholders (host)
		    }
            DependsOn = '[xArchive]UnzipElasticsearch'
	    }

        cElasticsearch InstallElasticsearch
        {
            DependsOn = @('[Script]ConfigureElasticsearch','[Environment]SetJDKEnviromentVar')
            UnzipFolder = Join-Path $elasticsearchFolder $elasticsearchVersion
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

        xRemoteFile CopyJdbcZip {
			DestinationPath = Join-Path -Path $tempDownloadFolder -ChildPath $jdbcFileName
			Uri = Join-Path $commonShareUrl -ChildPath $jdbcSource | Join-Path -ChildPath $jdbcFileName
			Credential = $commonShareCredential
		}

        xArchive UnzipJdbc {
            Path = Join-Path $tempDownloadFolder $jdbcFileName
            Destination = $jdbcFolder
            DependsOn = @('[xPackage]_7zip', '[xRemoteFile]CopyJdbcZip')
			DestinationType = "Directory"
        }

        Script ConfigureJdbc
	    {
		    TestScript = { 
			    $true
		    }
		    GetScript = {@{Result = "ConfigureJdbc"}}
		    SetScript =
		    {
                $pathToExecute = "$jdbcFolder\elasticsearch-jdbc-$jdbcVersion\bin\execute.bat" 
                $pathToSettings = "$jdbcFolder\elasticsearch-jdbc-$jdbcVersion\bin\settings.json" 
                
                #replace placeholders in $pathToExecute (paths)
                #replace placeholders in $pathToSettings (all)
		    }
            DependsOn = '[xArchive]UnzipJdbc'
	    }

        xWindowsProcess StartJdbc {
            Path =  Join-Path $jdbcFolder 'bin\execute.bat'
            Arguments = ''
            DependsOn = '[Script]ConfigureJdbc'
        }

	}
}
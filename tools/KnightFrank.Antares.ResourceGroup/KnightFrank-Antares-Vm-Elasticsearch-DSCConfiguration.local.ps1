Configuration SetupElasticsearchVmLocal
{
	Param ( 
        [string][Parameter(Mandatory = $true)] 
        $commonShareUrl)

    $tempDownloadFolder = "$env:SystemDrive\temp\download\"
    
    $javaSource =  'java'
    $javaFileName = 'jdk-8u91-windows-x64.exe'
    $javaFolder = "$env:SystemDrive\Program Files\java\jdk\$javaVersion\"
    $javaVersion = '8u91'
        
    $elasticsearchSource = 'elasticsearch'
    $elasticsearchFileName = 'elasticsearch-2.3.2.zip'
    $elasticsearchFolder = "$env:SystemDrive\elasticsearch"    
    $elasticsearchVersion = '2.3.2'
        
    $jdbcSource =  'elasticsearch'
    $jdbcFileName = 'elasticsearch-jdbc-2.3.1.0-dist.zip'
    $jdbcFolder = "$env:SystemDrive\jdbc"
    $jdbcVersion = '2.3.1.0'
    
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

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $mycreds                
                Copy-Item (Join-Path "P:" -ChildPath $using:javaSource | Join-Path -ChildPath $using:javaFileName) (Join-Path $using:tempDownloadFolder $using:javaFileName)
			    Remove-PSDrive -Name P
		    }
        }

        xPackage Java {
            Name = 'Java SE Development Kit 8 Update 91 (64-bit)'
            Path =  Join-Path $tempDownloadFolder $javaFileName
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

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path "P:" -ChildPath $using:elasticsearchSource | Join-Path -ChildPath $using:elasticsearchFileName) (Join-Path $using:tempDownloadFolder $using:elasticsearchFileName)
			    Remove-PSDrive -Name P
		    }
	    }
  
        xArchive UnzipElasticsearch {
            Path = Join-Path $tempDownloadFolder $elasticsearchFileName
            Destination = Join-Path $elasticsearchFolder $elasticsearchVersion
            DependsOn = @('[xPackage]_7zip', '[Script]CopyElasticsearchZip')
			DestinationType= "Directory"
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

        <#
        Script ConfigureFirewall
        {
            DependsOn = @("[Package]InstallOctopusTentacle", "[File]OctopusDir")
            TestScript = {
                Test-Path "C:\Octopus\Tentacle.config"
            }
            SetScript = {
                & "netsh" advfirewall firewall add rule "name=Elasticsearch" dir=in action=allow protocol=TCP localport=9200                
            }
            GetScript = {@{Result = "ConfigureOctopusTentacle"}}
        }#>

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

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $mycreds
			    Copy-Item (Join-Path "P:" -ChildPath $using:jdbcSource | Join-Path -ChildPath $using:jdbcFileName) (Join-Path $using:tempDownloadFolder $using:jdbcFileName)
			    Remove-PSDrive -Name P
		    }
	    }

        xArchive UnzipJdbc {
            Path = Join-Path $tempDownloadFolder $jdbcFileName
            Destination = $jdbcFolder
            DependsOn = @('[xPackage]_7zip', '[Script]CopyJdbcZip')
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
            Path = Join-Path $jdbcFolder "elasticsearch-jdbc-$jdbcVersion\bin\execute.bat" 
            Arguments = ''
            DependsOn = '[Script]ConfigureJdbc'
            StandardErrorPath = Join-Path $jdbcFolder "elasticsearch-jdbc-$jdbcVersion\bin\log.txt" 
        }

	}
}
SetupElasticsearchVmLocal -commonShareUrl "\\kfaneiedevcommonst.file.core.windows.net\setupfiles"
Start-DscConfiguration -Path .\SetupElasticsearchVmLocal -ComputerName localhost -Force -Verbose -Wait
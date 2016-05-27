Configuration SetupElasticsearchVm
{

	Param ( 
        [string][Parameter(Mandatory = $true)] 
        $commonShareUrl, 

        [System.Management.Automation.PSCredential][Parameter(Mandatory = $true)] 
        $commonShareCredential)

    $tempDownloadFolder = '$env:SystemDrive\temp\download\'
    
    $javaSource =  'java'
    $javaFileName = 'jre-8u91-windows-x64.exe'
    $javaFolder = '$env:SystemDrive\Program Files\java\jre'
    $javaVersion = '8u91'
        
    $elasticsearchSource = 'elasticsearch'
    $elasticsearchFileName = 'elasticsearch-2.3.2.zip'
    $elasticsearchFolder = '$env:SystemDrive\elasticsearch'    
    $elasticsearchVersion = '2.3.2'
        
    $jdbcSource =  'elasticsearch'
    $jdbcFileName = 'elasticsearch-jdbc-2.3.1.0-dist.zip'
    $jdbcFolder = '$env:SystemDrive\jdbc'
    
    Import-DscResource -Module cGripDevDSC
    Import-DscResource -Module c7zip
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
        #Download and set env variable for JRE
        #####
  
        xPackage _7zip {
            Name = '7-Zip 9.20 (x64 edition)'
            Path = 'http://www.7-zip.org/a/7z920-x64.msi'
            ProductId = '23170F69-40C1-2702-0920-000001000000'
            Ensure = 'Present'
        }        

        #maybe mount share as separate task
 
        Script CopyJRE
	    {
		    TestScript = { 
			    Test-Path "$using:tempDownloadFolder\\$using:javaFileName"
		    }
		    GetScript = {@{Result = "CopyJRE"}}
		    SetScript =
		    {
                $secpasswd = ConvertTo-SecureString “P7G+0uLD8g5q73RxshNg4GQdXJHr0EXasWSk6pe5UhCsp2QphVtPzZd+IcNdRlt9zoFjszQXMrK8XbIOcClakA==” -AsPlainText -Force
                $mycreds = New-Object System.Management.Automation.PSCredential("kfaneiedevcommonst", $secpasswd)

			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $mycreds                
                Copy-Item (Join-Path "P:" -ChildPath $using:javaSource | Join-Path -ChildPath $using:javaFileName) (Join-Path $using:tempDownloadFolder $using:javaFileName)
			    Remove-PSDrive -Name P
		    }
        }
        
        xPackage Java
        {
             Ensure = “Present”
             Path = Join-Path $tempDownloadFolder $javaFileName
             Name = “Java 8 Update 91 (64-bit)” 
             ProductId = “26A24AE4-039D-4CA4-87B4-2F83218091F0” 
             Arguments = "/s INSTALLDIR=$javaFolder" #put $javaFolder parameter
        }
          
        Environment SetJREEnviromentVar
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
			    Test-Path "$using:downloadroot\\$using:elasticsearchInstallationFileName"
		    }
		    GetScript = {@{Result = "CopyElasticsearchZip"}}
		    SetScript =
		    {
			    New-PSDrive -Name P -PSProvider FileSystem -Root $using:commonShareUrl -Credential $using:commonShareCredential
			    Copy-Item p:\$using:elasticsearchInstallationFileName "$using:downloadFolder\\$using:elasticsearchInstallationFileName"
			    Remove-PSDrive -Name P
		    }
	    }
  
        cArchive7zip UnzipElasticsearch {
            SourcePath = Join-Path $tempDownloadFolder $elasticsearchFileName
            DestinationPath = Join-Path $elasticsearchFolder $elasticsearchVersion
            DependsOn = @('[xPackage]_7zip')
        }
  
        cElasticsearch ElasticInstall
        {
            DependsOn = @('[cArchive7zip]UnzipElasticsearch')
            UnzipFolder = Join-Path $elasticsearchFolder $elasticsearchVersion
        }
	}
}
SetupElasticsearchVm -commonShareUrl "\\kfaneiedevcommonst.file.core.windows.net\setupfiles" -commonShareCredential null
Start-DscConfiguration -Path .\SetupElasticsearchVm -ComputerName localhost -Wait -Force -Verbose
Configuration SetupElasticsearchVm
{

	Param ()

    $installRoot = 'c:\elkInstall\'
    $downloadroot = 'c:\elkTempDownload\'

    $elasticZipLocation = Join-Path $downloadroot 'elastic.zip'
    $elasticUnpacked = Join-Path $installRoot 'elasticsearch'
    $elasticDownloadUri = 'https://download.elasticsearch.org/elasticsearch/elasticsearch/elasticsearch-1.4.4.zip'

    $javaZipLocation = Join-Path $downloadroot 'jre.tar.gz'
    $javaUnpackLocation = Join-Path $installRoot 'jre'
    $javaFolder =  Join-Path $javaUnpackLocation 'jdk1.8.0_45'
    $javaDownloadUri = 'http://download.oracle.com/otn-pub/java/jdk/8u45-b15/server-jre-8u45-windows-x64.tar.gz'

    Import-DscResource -Module cGripDevDSC
	Import-DscResource -ModuleName PSDesiredStateConfiguration

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
        #Download and set env variable for JRE
        #####
  
        cSimpleDownloader JREDownload
        {
            DestinationPath = $javaZipLocation
            RemoteFileLocation = $javaDownloadUri
            CookieName = "oraclelicense"
    		CookieValue = "accept-securebackup-cookie"
    		CookieDomain = ".oracle.com"        
        }
  
        c7zip JREUnzip
        {
            DependsOn = "[cSimpleDownloader]JREDownload"
            ZipFileLocation = $javaZipLocation
            UnzipFolder = $javaUnpackLocation
        }
  
        Environment SetJREEnviromentVar
        {
            Name = "JAVA_HOME"
            Ensure = "Present"
            Path = $false
            Value = $javaFolder
            DependsOn = "[c7zip]JREUnzip"
        }
  
        #####
        #Download and install elasticsearch
        #####
  
        cSimpleDownloader ElasticDownloadZip
        {
            DestinationPath = $elasticZipLocation
            RemoteFileLocation = $elasticDownloadUri
        }
  
        c7zip ElasticUnzip
        {
            DependsOn = "[cSimpleDownloader]ElasticDownloadZip"
            ZipFileLocation = $elasticZipLocation
            UnzipFolder = $elasticUnpacked
        }
  
        cElasticsearch ElasticInstall
        {
            DependsOn = @('[Environment]SetJREEnviromentVar', '[c7zip]ElasticUnzip')
            UnzipFolder = $elasticUnpacked
        }
	}
}
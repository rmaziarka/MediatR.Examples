configuration TestConfig
{
    #####
    #Configure locations for downloads and installs of JRE, Elasticsearch, NSSM and Kibana
    #####

    $installRoot = 'c:\elkInstall\'
    $downloadroot = 'c:\elkTempDownload\'

    $elasticZipLocation = Join-Path $downloadroot 'elastic.zip'
    $elasticUnpacked = Join-Path $installRoot 'elasticsearch'
    $elasticDownloadUri = 'https://download.elasticsearch.org/elasticsearch/elasticsearch/elasticsearch-1.4.4.zip'

    $kibanaZipLocation = Join-Path $downloadroot 'kibana.zip'
    $kibanaUnpacked = Join-Path $installRoot 'kibana'
    $kibanaDownloadUri = 'https://download.elasticsearch.org/kibana/kibana/kibana-4.0.1-windows.zip'

    $javaZipLocation = Join-Path $downloadroot 'jre.tar.gz'
    $javaUnpackLocation = Join-Path $installRoot 'jre'
    $javaFolder =  Join-Path $javaUnpackLocation 'jdk1.8.0_45'
    $javaDownloadUri = 'http://download.oracle.com/otn-pub/java/jdk/8u45-b15/server-jre-8u45-windows-x64.tar.gz'

    $nssmZipLocation = Join-Path $downloadroot 'nssm.zip'
    $nssmUnpackLocation = Join-Path $installRoot 'nssmkibana'
    $nssmDownloadUri = 'https://nssm.cc/release/nssm-2.24.zip'

    Import-DscResource -Module cGripDevDSC
    
    
    node ("localhost")
    {
        LocalConfigurationManager
        {
            DebugMode = 'ForceModuleImport'
        }

        #####
        #Downlaod and set env variable for JRE
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
  
        
        #####
        # Downlaod and Install Kibana, using nssm to host as windows service
        # Default Port: 5601
        #####
  
        cSimpleDownloader NSSMDownloadZip
        {
            DestinationPath = $nssmZipLocation
            RemoteFileLocation = $nssmDownloadUri
            DependsOn = "[cElasticsearch]ElasticInstall"
        }
  
  
        c7zip NSSMExtractForKibana
        {
            ZipFileLocation = $nssmZipLocation
            UnzipFolder = $nssmUnpackLocation
            DependsOn = "[cSimpleDownloader]NSSMDownloadZip"
        }
  
        cSimpleDownloader KibanaDownloadZip
        {
            DestinationPath = $kibanaZipLocation
            RemoteFileLocation = $kibanaDownloadUri
            DependsOn = "[cElasticsearch]ElasticInstall"
        }
  
        c7zip KibanaUnzip
        {
            DependsOn = "[cSimpleDownloader]KibanaDownloadZip"
            ZipFileLocation = $kibanaZipLocation
            UnzipFolder = $kibanaUnpacked
        }

        cNssm KibanaInstall
        {
            DependsOn = @('[c7zip]KibanaUnzip', '[c7zip]NSSMExtractForKibana')          
            ExeFolder = $kibanaUnpacked
            NssmFolder = $nssmUnpackLocation
            ServiceName = 'KibanaNSSM'
            ExeOrBatName = 'kibana.bat'
        }
    }
}

TestConfig

Start-DscConfiguration .\TestConfig -wait -verbose -force

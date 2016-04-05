Configuration Main
{

Param ( [string] $nodeName )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xWebAdministration

Node $nodeName
  {
    WindowsFeature WebServerRole
    {
      Name = "Web-Server"
      Ensure = "Present"
    }
    WindowsFeature WebManagementConsole
    {
      Name = "Web-Mgmt-Console"
      Ensure = "Present"
    }
    WindowsFeature WebManagementService
    {
      Name = "Web-Mgmt-Service"
      Ensure = "Present"
    }
    WindowsFeature ASPNet45
    {
      Name = "Web-Asp-Net45"
      Ensure = "Present"
    }
    WindowsFeature HTTPRedirection
    {
      Name = "Web-Http-Redirect"
      Ensure = "Present"
    }
    WindowsFeature CustomLogging
    {
      Name = "Web-Custom-Logging"
      Ensure = "Present"
    }
    WindowsFeature LogginTools
    {
      Name = "Web-Log-Libraries"
      Ensure = "Present"
    }
    WindowsFeature RequestMonitor
    {
      Name = "Web-Request-Monitor"
      Ensure = "Present"
    }
    WindowsFeature Tracing
    {
      Name = "Web-Http-Tracing"
      Ensure = "Present"
    }
    WindowsFeature BasicAuthentication
    {
      Name = "Web-Basic-Auth"
      Ensure = "Present"
    }
    WindowsFeature WindowsAuthentication
    {
      Name = "Web-Windows-Auth"
      Ensure = "Present"
    }
    WindowsFeature ApplicationInitialization
    {
      Name = "Web-AppInit"
      Ensure = "Present"
    }
    Script DownloadWebDeploy
    {
        TestScript = {
            Test-Path "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
        }
        SetScript ={
            $source = "http://download.microsoft.com/download/0/1/D/01DC28EA-638C-4A22-A57B-4CEF97755C6C/WebDeploy_amd64_en-US.msi"
            $dest = "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
            Invoke-WebRequest $source -OutFile $dest
        }
        GetScript = {@{Result = "DownloadWebDeploy"}}
        DependsOn = "[WindowsFeature]WebServerRole"
    }
    Package InstallWebDeploy
    {
        Ensure = "Present"  
        Path  = "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
        Name = "Microsoft Web Deploy 3.6"
        ProductId = "{ED4CC1E5-043E-4157-8452-B5E533FE2BA1}"
        Arguments = "ADDLOCAL=ALL"
        DependsOn = "[Script]DownloadWebDeploy"
    }
    Service StartWebDeploy
    {                    
        Name = "WMSVC"
        StartupType = "Automatic"
        State = "Running"
        DependsOn = "[Package]InstallWebDeploy"
    }

	$DirectoriesToCreate = @(
		"C:\inetpub\wwwroot\test\app",
		"C:\inetpub\wwwroot\test\api",
		"C:\inetpub\wwwroot\dev\app",
		"C:\inetpub\wwwroot\dev\api"
	)
	foreach ($DirectoryToCreate in $DirectoriesToCreate){
		File WebAppTestDir{
			Type = "Directory"
			DestinationPath = $DirectoryToCreate
			Ensure = "Present"
		}
	}

	xWebAppPool TestWebApiPool 
    { 
        Name            = "dev.api.antares.knightfrank.com"
        Ensure          = "Present"
        State           = "Started"
    }  
	xWebAppPool TestWebAppPool 
    { 
        Name            = "dev.app.antares.knightfrank.com"
        Ensure          = "Present"
        State           = "Started"
    }  
	xWebsite TestWebApi
	{
		Name = "dev.api.antares.knightfrank.com"
		DependsOn = "[xWebAppPool]WebApiPool"
		ApplicationPool = "dev.api.antares.knightfrank.com"
		PhysicalPath = "C:\inetpub\wwwroot\test\api"
		State = "Started"
		Ensure = "Present"
	}
	xWebsite TestWebApp
	{
		Name = "dev.app.antares.knightfrank.com"
		DependsOn = "[xWebAppPool]WebAppPool"
		ApplicationPool = "dev.app.antares.knightfrank.com"
		PhysicalPath = "C:\inetpub\wwwroot\test\app"
		State = "Started"
		Ensure = "Present"
	}

	<#
	File WebAppTestDir{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\test\app"
		Ensure = "Present"
	}
	File WebApiTestDir{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\test\api"
		Ensure = "Present"
	}
	File WebAppDevDir{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\dev\app"
		Ensure = "Present"
	}
	File WebApiDevDir{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\dev\api"
		Ensure = "Present"
	}#>
  }
}
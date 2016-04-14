Configuration SetupIISVm
{

Param ( [string] $nodeName, [string] $environmentPrefix )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xWebAdministration

$webApiHostName = "$environmentPrefix.api.antares.knightfrank.com"
$webAppHostName = "$environmentPrefix.app.antares.knightfrank.com"

Node $nodeName
  {
	LocalConfigurationManager
    {
        RebootNodeIfNeeded = $true
		ActionAfterReboot = "ContinueConfiguration"
    }

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
	
	File WebAppDir
	{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\app"
		Ensure = "Present"
	}
	File WebApiDir
	{
		Type = "Directory"
		DestinationPath = "C:\inetpub\wwwroot\api"
		Ensure = "Present"
	}
	
	xWebAppPool WebApiPool 
    { 
        Name = $webApiHostName
        Ensure = "Present"
        State = "Started"
		DependsOn = @('[WindowsFeature]WebServerRole', '[File]WebApiDir')
    }  
	xWebAppPool WebAppPool 
    { 
        Name = $webAppHostName
        Ensure = "Present"
        State = "Started"
		DependsOn = @('[WindowsFeature]WebServerRole', '[File]WebAppDir')
    }   
	xWebsite WebApi
	{
		Name = $webApiHostName
		DependsOn = "[xWebAppPool]WebApiPool"
		ApplicationPool = $webApiHostName
		PhysicalPath = "C:\inetpub\wwwroot\api"
		State = "Started"
		Ensure = "Present"
		BindingInfo = MSFT_xWebBindingInformation 
        { 
			Protocol = "http" 
			Port = 80  
			HostName = $webApiHostName
        }
		EnabledProtocols = "http"
	}
	xWebsite WebApp
	{
		Name = $webAppHostName
		DependsOn = "[xWebAppPool]WebAppPool"
		ApplicationPool = $webAppHostName
		PhysicalPath = "C:\inetpub\wwwroot\app"
		State = "Started"
		Ensure = "Present"
		BindingInfo = MSFT_xWebBindingInformation 
        { 
			Protocol = "http" 
			Port = 80  
			HostName = $webAppHostName
        }
		EnabledProtocols = "http"
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
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" configure --instance "Tentacle" --trust "C478B5952045B390F15F0C444D4917E911713EF0" --console
			& "netsh" advfirewall firewall add rule "name=Octopus Deploy Tentacle" dir=in action=allow protocol=TCP localport=10933
			& "C:\Program Files\Octopus Deploy\Tentacle\Tentacle.exe" service --instance "Tentacle" --install --start --console
		}
		GetScript = {@{Result = "ConfigureOctopusTentacle"}}
	}
  }
}
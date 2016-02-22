Configuration ConfigureIISProvision {
    param ($NodeName, $Environment, $Tokens)

    Import-DSCResource -Module xDismFeature

    Node $NodeName {
        if ($Environment -eq "Local") {
            
            xDismFeature IISWebServer { 
                Name = "IIS-WebServerRole"
            }

            xDismFeature IISASPNET45 { 
                Name = "IIS-ASPNET45"
            }
        } else {
            WindowsFeature IIS {
                Ensure = "Present"
                Name = "Web-Server"
            }

            WindowsFeature IISAuth {
                Ensure = "Present"
                Name = "Web-Windows-Auth"
                DependsOn = "[WindowsFeature]IIS"
            }
                     
            WindowsFeature IISASP { 
              Ensure = "Present"
              Name = "Web-Asp-Net45"
              DependsOn = "[WindowsFeature]IIS"
            } 
        }
    }
}
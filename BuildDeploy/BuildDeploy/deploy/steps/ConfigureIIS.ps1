Configuration ConfigureIIS {
    param ($NodeName, $Environment, $Tokens)
	
    Import-DSCResource -Module cIIS
    # DSC PSGallery resources are included in PSCI
    Import-DSCResource -Module xNetworking

    Node $NodeName {

        # configure Web API application pool
        cAppPool $Tokens.WebAPIConfig.AppPoolName { 
            Name   = $Tokens.WebAPIConfig.AppPoolName
            Ensure = 'Present'
            AutoStart = $true
            StartMode = 'AlwaysRunning'
            ManagedRuntimeVersion = 'v4.0'
            IdentityType = 'ApplicationPoolIdentity'
        }

        # configure client application pool		
		cAppPool $Tokens.WebClientConfig.AppPoolName { 
            Name   = $Tokens.WebClientConfig.AppPoolName
            Ensure = 'Present'
            AutoStart = $true
            StartMode = 'AlwaysRunning'
            ManagedRuntimeVersion = 'v4.0'
            IdentityType = 'ApplicationPoolIdentity'
        }  
		
        # create WebAPI application directory
        File WebAPIDir {
            DestinationPath = $Tokens.WebAPIConfig.WebsitePhysicalPath
            Ensure = 'Present'
            Type = 'Directory'
        }

		# create website directory
        File WebClientDir {
            DestinationPath = $Tokens.WebClientConfig.WebsitePhysicalPath
            Ensure = 'Present'
            Type = 'Directory'
        }

		# create web site on IIS
        cWebsite WebAPIWebsite { 
            Name   = $Tokens.WebAPIConfig.WebsiteName
            ApplicationPool = $Tokens.WebAPIConfig.AppPoolName 
            BindingInfo = OBJ_cWebBindingInformation { 
                Port = $Tokens.WebAPIConfig.WebsitePort
            } 
            PhysicalPath = $Tokens.WebAPIConfig.WebsitePhysicalPath
            Ensure = 'Present' 
            State = 'Started' 
            DependsOn = @('[File]WebAPIDir')
        }

		# create web site on IIS
        cWebsite WebClientWebsite { 
            Name   = $Tokens.WebClientConfig.WebsiteName
            ApplicationPool = $Tokens.WebClientConfig.AppPoolName 
            BindingInfo = OBJ_cWebBindingInformation { 
                Port = $Tokens.WebClientConfig.WebsitePort
            } 
            PhysicalPath = $Tokens.WebClientConfig.WebsitePhysicalPath
            Ensure = 'Present' 
            State = 'Started' 
            DependsOn = @('[File]WebClientDir')
        }
    }
}


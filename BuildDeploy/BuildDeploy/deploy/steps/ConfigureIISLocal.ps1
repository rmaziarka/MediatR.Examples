Configuration ConfigureIISLocal {
    param ($NodeName, $Environment, $Tokens)

	$webApiSrcPath = Join-Path -Path ((Get-ConfigurationPaths).ProjectRootPath) -ChildPath 'KnightFrank.Antares.Api' 
	$webClientSrcPath = Join-Path -Path ((Get-ConfigurationPaths).ProjectRootPath) -ChildPath 'wwwroot' 

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

		# create web site on IIS
        cWebsite WebAPIWebsite { 
            Name   = $Tokens.WebAPIConfig.WebsiteName
            ApplicationPool = $Tokens.WebAPIConfig.AppPoolName 
            BindingInfo = OBJ_cWebBindingInformation { 
                Port = $Tokens.WebAPIConfig.WebsitePort
            } 
            PhysicalPath = $webApiSrcPath
            Ensure = 'Present' 
            State = 'Started'
        }

		# create web site on IIS
        cWebsite WebClientWebsite { 
            Name   = $Tokens.WebClientConfig.WebsiteName
            ApplicationPool = $Tokens.WebClientConfig.AppPoolName 
            BindingInfo = OBJ_cWebBindingInformation { 
                Port = $Tokens.WebClientConfig.WebsitePort
            } 
            PhysicalPath = $webClientSrcPath
            Ensure = 'Present' 
            State = 'Started'
        }
    }
}


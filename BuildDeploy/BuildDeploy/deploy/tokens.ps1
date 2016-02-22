Environment Default {
   
    # Credentials used during deployment - sensitive data can be stored in separate file (e.g. tokensSensitive.ps1)
    Tokens Remoting @{
        RemotingCredential = { ConvertTo-PSCredential -User $Tokens.Remoting.UserName -Password $Tokens.Remoting.Password }
        MSDeployDestination = New-MsDeployDestinationString -Url '${Node}'
		UserName = 'TODO'
		Password = 'TODO'
    }
	
    # IIS configuration
    Tokens WebAPIConfig @{
        AppPoolName = 'KnightFrank.Antares.WebAPI.AppPool'
        WebsiteName = 'KnightFrank.Antares.WebAPI'
        WebsitePort = 80
        WebsitePhysicalPath = '${WebConfig.WebsitePhysicalPath}\KnightFrank.Antares.WebAPI'
    }

	# IIS configuration
    Tokens WebClientConfig @{
        AppPoolName = 'KnightFrank.Antares.WebClient.AppPool'
        WebsiteName = 'KnightFrank.Antares.WebClient'
        WebsitePort = 80
        WebsitePhysicalPath = '${WebConfig.WebsitePhysicalPath}\KnightFrank.Antares.WebClient'
    }

    # Tokens used to update Web.config file - all occurrences of '${TestConnectionString}' string will be replaced
    Tokens WebTokens @{
        TestConnectionString = { $Tokens.DatabaseConfig.ConnectionString }
    }

    # Tokens related to database deployment
    Tokens DatabaseConfig @{
        DatabaseName = 'KnightFrank.Antares'
        ConnectionString = 'Server=localhost;Database=${DatabaseName};Integrated Security=SSPI'
        DropDatabase = $true
    }
}

Environment Local {   

	# IIS configuration
    Tokens WebConfig @{
        WebsitePhysicalPath = 'c:\inetpub\wwwroot\KnightFrank.Antares'
    }

    # IIS configuration
    Tokens WebAPIConfig @{
        AppPoolName = 'KnightFrank.Antares.WebAPI.AppPool'
        WebsiteName = 'KnightFrank.Antares.WebAPI'
        WebsitePort = 81
        WebsitePhysicalPath = '${WebConfig.WebsitePhysicalPath}\KnightFrank.Antares.WebAPI'
    }

	# IIS configuration
    Tokens WebClientConfig @{
        AppPoolName = 'KnightFrank.Antares.WebClient.AppPool'
        WebsiteName = 'KnightFrank.Antares.WebClient'
        WebsitePort = 80
        WebsitePhysicalPath = '${WebConfig.WebsitePhysicalPath}\KnightFrank.Antares.WebClient'
    }

    # Tokens used to update Web.config file - all occurrences of '${TestConnectionString}' string will be replaced
    Tokens WebTokens @{
        TestConnectionString = { $Tokens.DatabaseConfig.ConnectionString }
    }

    # Tokens related to database deployment
    Tokens DatabaseConfig @{
        DatabaseName = 'KnightFrank.Antares'
        ConnectionString = 'Server=localhost;Database=${DatabaseName};Integrated Security=SSPI'
        DropDatabase = $true
    }
}

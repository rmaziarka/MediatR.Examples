function Deploy-WebAPI {
    param ($NodeName, $Environment, $Tokens, $ConnectionParams)

    $msDeployParams = @{ PackageName = $Tokens.WebAPIConfig.WebsiteName;
                         PackageType = 'Web';
                         Node = $NodeName;
                         MsDeployDestinationString = $Tokens.Remoting.MsDeployDestination;
                         TokensForConfigFiles = $Tokens.WebTokens;
                         FilesToIgnoreTokensExistence = @( 'NLog.config' );
                         Website = $Tokens.WebConfig.WebsiteName;
                         WebApplication = $Tokens.WebAPIConfig.WebsiteName;
                         SkipDir = 'App_Data';
                         Environment = $Environment
                       }

    Deploy-MsDeployPackage @msDeployParams
}
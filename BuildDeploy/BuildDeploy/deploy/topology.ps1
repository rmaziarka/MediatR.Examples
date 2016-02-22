Environment Default {
    ServerConnection WebServer -Nodes 'localhost'
    ServerConnection DatabaseServer -BasedOn WebServer
    ServerRole Web -Steps @('PSCIWindowsFeature','ConfigureIIS','Deploy-WebAPI','Deploy-WebClient') -ServerConnections WebServer
    ServerRole Database -Steps @('Deploy-Database') -ServerConnections DatabaseServer
}

Environment Local {
    ServerConnection WebServer -Nodes 'localhost'
    ServerConnection DatabaseServer -BasedOn WebServer
    ServerRole Web -Steps @('PSCIWindowsFeature','ConfigureIISLocal') -ServerConnections WebServer
    ServerRole Database -Steps @('Deploy-Database') -ServerConnections DatabaseServer
}
function Configure-Elasticsearch {    
    [CmdletBinding()]
    param (
    )

    Write-Host -Info "Configuring Elasticsearch service"
			
    #Get service
    $serviceObject = Get-Service -Name "elasticsearch-service-x64"
    $serviceObject | Set-Service -StartupType Automatic

    Write-Host -Info "Successfully configured Elasticsearch service" -ForegroundColor Green
}
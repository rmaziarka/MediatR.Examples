function Configure-Elasticsearch {    
    [CmdletBinding()]
    param (
        [parameter(Mandatory=$true)]
        [string]
        $PathToMappingsFile
    )

    Write-Host -Info "Configuring Elasticsearch service"
			
    #Get service
    $serviceObject = Get-Service -Name "elasticsearch-service-x64"
    $serviceObject | Set-Service -StartupType Automatic

    Write-Host -Info "Successfully configured Elasticsearch service" -ForegroundColor Green
    
    Write-Host -Info "Configuring Elasticsearch mappings"
    
    $elasticSearchUrl = "http://localhost:9200"
    $elasticSearchIndex = "localhost"
    $mappingsFile = Get-Content -Path $PathToMappingsFile
    
    try {
        Invoke-RestMethod -Method Delete -Uri "$elasticSearchUrl/$elasticSearchIndex" 
    }
    catch {
        Write-Host -Info "Index '$elasticSearchIndex' does not exist. Creating index with mappings..." -ForegroundColor Yellow
    }

    Invoke-RestMethod -Method Put -Uri "$elasticSearchUrl/$elasticSearchIndex" -Body $mappingsFile
    
    Write-Host -Info "Successfully configured Elasticsearch mappings" -ForegroundColor Green
}
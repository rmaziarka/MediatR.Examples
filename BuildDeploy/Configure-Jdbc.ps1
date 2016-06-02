function Configure-Jdbc {    
    [CmdletBinding()]
    param (
        [parameter(Mandatory=$true)]
        [string]
        $PathToSettingsTemplate,
   
        [parameter(Mandatory=$true)]
        [string]
        $PathToNssm,

        [parameter(Mandatory=$true)]
        [string]
        $PathToJdbc
    )

    Write-Log -Info "Configuring Elasticsearch-Jdbc"
			
    Write-Log -Info "Reading $PathToSettingsTemplate"    
				
    $settings = Get-Content -Path $PathToSettingsTemplate -Raw | ConvertFrom-Json

    Write-Log -Info "Populating settings with values: SqlIp=$SqlIp;SqlPort=$SqlPort;databaseName=$SqlDatabaseName;"

    $settings.jdbc.url = "jdbc:sqlserver://" + $SqlIp + ":" + $SqlPort+ ";databaseName=$SqlDatabaseName"
    $settings.jdbc.user = $SqlUser
    $settings.jdbc.password = $SqlPassword
    $settings.jdbc.index = $ElasticsearchIndex
    				
    $json = $settings | ConvertTo-Json -Depth 999				
    
    $pathToSettings = "$PathToJdbc\bin\settings.json" 

    Write-Log -Info "Saving settings to $pathToSettings"

    [System.IO.File]::WriteAllLines($pathToSettings, $json)
    
    $pathToExecute = "$PathToJdbc\bin\execute.bat" 
    
    Write-Log -Info "Starting $pathToExecute as a service"

    Set-NssmService -ServiceName "jdbc-2.3.2.0" -Path $pathToExecute -Status Running -NssmPath $PathToNssm

    Write-Log -Info "Succesfully configured Jdbc." -ForegroundColor Green
}
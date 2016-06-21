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
        $PathToJdbc,

        [parameter(Mandatory=$false)]
        [string]
        $SqlIp = "localhost",

        [parameter(Mandatory=$false)]
        [string]
        $SqlPort = "1433",

        [parameter(Mandatory=$true)]
        [string]
        $SqlUser,

        [parameter(Mandatory=$true)]
        [string]
        $SqlPassword,

        [parameter(Mandatory=$true)]
        [string]
        $SqlDatabaseName,

        [parameter(Mandatory=$false)]
        [string]
        $ElasticsearchIndex = "localhost",
                
        [parameter(Mandatory=$true)]
        [string]
        $Schedule
    )

    Write-Host -Object "Configuring Elasticsearch-Jdbc"
			
    Write-Host -Object "Reading $PathToSettingsTemplate"    
				
    $settings = Get-Content -Path $PathToSettingsTemplate -Raw | ConvertFrom-Json

    Write-Host -Object "Populating settings with values: SqlIp=$SqlIp;SqlPort=$SqlPort;databaseName=$SqlDatabaseName;"
    Write-Host -Object "Populating settings with values: user=$SqlUser;password=$SqlPassword;"
    Write-Host -Object "Populating settings with values: index=$ElasticsearchIndex;schedule=$Schedule;"

    $settings.jdbc.url = "jdbc:sqlserver://" + $SqlIp + ":" + $SqlPort+ ";databaseName=$SqlDatabaseName"
    $settings.jdbc.user = $SqlUser
    $settings.jdbc.password = $SqlPassword    
    $settings.jdbc.index = $ElasticsearchIndex
    $settings.jdbc.schedule = $Schedule
    				
    $json = $settings | ConvertTo-Json -Depth 999				
    
    $pathToSettings = "$PathToJdbc\bin\settings.json" 

    Write-Host -Object "Saving settings to $pathToSettings"

    [System.IO.File]::WriteAllLines($pathToSettings, $json)
    
    Write-Host -Object "Creating execute.bat"
    
    $executeCommand = "java -cp `"$PathToJdbc\lib\*`" -Dlog4j.configurationFile=`"$PathToJdbc\bin\log4j2.xml`" org.xbib.tools.Runner org.xbib.tools.JDBCImporter `"$PathToJdbc\bin\settings.json`""
        
    New-Item -Path "$PathToJdbc\bin" -Name "execute.bat" -Type "file" -Value $executeCommand -Force
        
    Write-Host -Object "Starting elasticsearch-jdbc as a service"

    try
    {
        Set-NssmService -ServiceName "jdbc-2.3.2.0" -Path "$PathToJdbc\bin\execute.bat"`
            -Status "Running"`
            -NssmPath $PathToNssm `
            -ServiceDisplayName "JDBC importer for Elasticsearch"`
            -ServiceDescription "The Java Database Connection (JDBC) importer allows to fetch data from JDBC sources for indexing into Elasticsearch."

        Write-Host -Object "Succesfully configured Jdbc." -ForegroundColor Green
    }
    catch [Exception] {
        Write-Host $_.Exception.Message -ForegroundColor Red        
        Write-Host -Object "Failed to configure Jdbc." -ForegroundColor Red
    }
}
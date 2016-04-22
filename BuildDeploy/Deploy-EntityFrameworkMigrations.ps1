function Deploy-EntityFrameworkMigrations {
    
    [CmdletBinding()]
    [OutputType([void])]
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $MigrateAssembly,

        [Parameter(Mandatory=$true)]
        [string] 
        $ConnectionString,

        [Parameter(Mandatory=$true)]
        [string] 
        $PackagePath
    )
	
    Write-Host "Deploying Entity Framework migrations from package '$PackagePath' using connectionString '$ConnectionString'"
        
	$scriptPath = "$PackagePath\migrate.exe"
    
    if (!(Test-Path -LiteralPath $scriptPath)) {
        throw "No migrate.exe in '$PackagePath')"
    }

    Write-Host "Running $scriptPath with following parameters $MigrateAssembly /connectionString=$ConnectionString /connectionProviderName=System.Data.SqlClient"
	
	& $scriptPath "$MigrateAssembly" /connectionString="$ConnectionString" /connectionProviderName=System.Data.SqlClient  

	if($LASTEXITCODE -ne 0)
	{
		throw "Migrate assembly failed with exit code: $LASTEXITCODE"
	}

    Write-Host "Deploying Entity Framework migrations to '$ConnectionString' completed successfully." -ForegroundColor Green
}
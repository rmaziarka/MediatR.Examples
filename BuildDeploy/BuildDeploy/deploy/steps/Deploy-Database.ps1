function Deploy-Database {
    param ($NodeName, $Environment, $Tokens, $ConnectionParams)

    $databaseName = $Tokens.DatabaseConfig.DatabaseName
    $connectionString = $Tokens.DatabaseConfig.ConnectionString

    if ($Tokens.DatabaseConfig.DropDatabase) { 
        Remove-SqlDatabase -DatabaseName $databaseName -ConnectionString $connectionString
    }

    Deploy-EntityFrameworkMigratePackage -PackageName 'KnightFrank.Antares.Dal' -ConnectionString $connectionString -MigrateAssembly 'KnightFrank.Antares.Dal.dll'

    $defaultAppPoolUserName = "IIS AppPool\$($Tokens.WebAPIConfig.AppPoolName)"
    Update-SqlLogin -ConnectionString $connectionString -Username $defaultAppPoolUserName -WindowsAuthentication
    Update-SqlUser -ConnectionString $connectionString -DatabaseName $databaseName -Username $defaultAppPoolUserName -DbRole 'db_datareader'
}
function Build-Database {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )

    $params = @{
        PackageName = 'KnightFrank.Antares.Dal'
        ProjectPath = 'KnightFrank.Antares.Dal\KnightFrank.Antares.Dal.csproj'
        MigrationsDir = 'KnightFrank.Antares.Dal\bin\Release'
        EntityFrameworkDir = 'packages\EntityFramework.6.1.3' 
    }

    Build-EntityFrameworkMigratePackage @params
}

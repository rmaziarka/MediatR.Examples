function Build-Backend {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )
		
	Build-WebPackage -ProjectPath 'KnightFrank.Antares.Backend.sln' -PackageName 'KnightFrank.Antares.Api' -RestoreNuGet -Version $Version	
}

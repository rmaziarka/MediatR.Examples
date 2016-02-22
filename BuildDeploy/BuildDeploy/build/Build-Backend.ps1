function Build-Backend {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )
		
	Build-WebPackage -ProjectPath 'KnightFrank.Antares.Backend.sln' -RestoreNuGet $true -Version $Version	
}

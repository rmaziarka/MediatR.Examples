function Build-Local {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )
		
	Invoke-MsBuild -ProjectPath 'KnightFrank.Antares.Backend.sln' -RestoreNuGet $true -Version $Version
}

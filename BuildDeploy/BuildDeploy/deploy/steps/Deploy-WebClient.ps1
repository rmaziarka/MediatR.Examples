function Deploy-WebClient {
    param ($NodeName, $Environment, $Tokens, $ConnectionParams)

	$packageZipPath = Join-Path -Path ((Get-ConfigurationPaths).PackagesPath) -ChildPath 'KnightFrank.Antares.WebClient\KnightFrank.Antares.WebClient.zip'     
	Expand-With7Zip -ArchiveFile $packageZipPath -OutputDirectory $Tokens.WebClientConfig.WebsitePhysicalPath
}
function Invoke-SqlQuery {
    
    [CmdletBinding()]
    [OutputType([void])]
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $InputFile,

        [Parameter(Mandatory=$true)]
        [string] 
        $DatabaseName,

		[Parameter(Mandatory=$true)]
        [string[]] 
        $SqlVariables,

		[Parameter(Mandatory=$false)]
        [string] 
        $ServerInstance = "localhost"		
    )
	
    Write-Host "Invoking sql '$InputFile' against database '$DatabaseName' on '$ServerInstance'"
	[void](Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $DatabaseName -InputFile $InputFile -Variable $SqlVariables)
}

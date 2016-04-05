function Invoke-SqlQuery {
    
    [CmdletBinding()]
    [OutputType([void])]
    param(
        [Parameter(Mandatory=$true)]
        [string] 
        $InputFile,

        [Parameter(Mandatory=$false)]
        [string] 
        $DatabaseName = "",

		[Parameter(Mandatory=$true)]
        [string[]] 
        $SqlVariables,

		[Parameter(Mandatory=$false)]
        [string] 
        $ServerInstance = "localhost"		
    )

    if($DatabaseName)
    {
        Write-Host "Invoking sql '$InputFile' on '$ServerInstance'"
	    [void](Invoke-Sqlcmd -ServerInstance $ServerInstance -InputFile $InputFile -Variable $SqlVariables)
    }
    else
    {
        Write-Host "Invoking sql '$InputFile' against database '$DatabaseName' on '$ServerInstance'"
	    [void](Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $DatabaseName -InputFile $InputFile -Variable $SqlVariables)
    }
}

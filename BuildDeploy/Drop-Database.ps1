function Drop-Database {
    
    [CmdletBinding()]
    [OutputType([void])]
    param(
        [Parameter(Mandatory=$false)]
        [string] 
        $SqlServer ='.',

        [Parameter(Mandatory=$false)]
        [string] 
        $SqlUsername = "",	

        [Parameter(Mandatory=$false)]
        [string] 
        $SqlPassword = "",	

        [Parameter(Mandatory=$true)]
        [string] 
        $SqlDatabase	
    )

    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null

    Write-Host -Object "Dropping database '$SqlDatabase'"

    try
    {    
        $server = new-object ('Microsoft.SqlServer.Management.Smo.Server') $SqlServer
    
        if ($SqlUsername -eq "" -and $SqlPassword -eq "")
        {
            $server.ConnectionContext.LoginSecure = $true
        } else {
            $server.ConnectionContext.LoginSecure = $false
            $server.ConnectionContext.set_Login($SqlUsername)
            $server.ConnectionContext.set_Password($SqlPassword)      
        }

	    if ($server.databases[$SqlDatabase] -ne $null)
	    {
    	    $server.killallprocesses($SqlDatabase)
    	    $server.databases[$SqlDatabase].drop()
	    }

        Write-Host -Object "Dropping database '$SqlDatabase' completed successfully."  -ForegroundColor Green
    }
    catch
    {    
        $error[0] | format-list -force
        Exit 1
    }
}

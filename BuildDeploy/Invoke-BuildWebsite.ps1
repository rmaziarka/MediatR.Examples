function Invoke-BuildWebsite
{
     param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectSrcWwwrootPath
    )

    Write-Host -Object "Start building website ..."

    # ===========================================================
    # gulp build-local
    # ===========================================================	
    Push-Location -Path $ProjectSrcWwwrootPath
    try 
    {
        Write-Host -Object "Executing : gulp build-local"
        Invoke-Expression -Command "gulp build-local"    
        if ($LASTEXITCODE -ne 0) 
        {
            throw "Failed gulp build-local"
        }
    }     
    finally 
    {
        Pop-Location
    }
}

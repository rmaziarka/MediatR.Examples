function Invoke-InstallNpm
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectSrcWwwrootPath
    )
       
    Write-Host -Object "Start installing npm modules ..."

    # ===========================================================
    # npm install
    # ===========================================================	    
    Push-Location -Path $ProjectSrcWwwrootPath
    try 
    {
        Write-Host -Object "Executing : npm install"
        Invoke-Expression -Command "npm install"
        if ($LASTEXITCODE -ne 0) 
        {
            throw "Failed npm install"
        }
    }     
    finally 
    {
        Pop-Location
    }
}

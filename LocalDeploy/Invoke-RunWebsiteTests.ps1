function Invoke-RunWebsiteTests
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectSrcWwwrootPath
    )
    
   Write-Host -Object "Start running website tests ..."

    $phantomjs = ".\phantomjs.exe"
    $runJasmine = "run-jasmine.js"
    $specRunner = "specRunner.html"
	
    Push-Location -Path $ProjectSrcWwwrootPath
    try 
    {

        Write-Host -Object "Executing : $phantomjs $runJasmine $specRunner"    
        & $phantomjs $runJasmine $specRunner    
        if ($LASTEXITCODE -ne 0) 
        {
            throw "Failed jasmine tests"
        }
    }     
    finally 
    {
        Pop-Location
    }
}

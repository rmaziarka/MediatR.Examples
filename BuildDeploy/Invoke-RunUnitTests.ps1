function Invoke-RunUnitTests
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectSrcPath,

        [Parameter(Mandatory=$true)]
        [string]
        $XUnit
    )
    
    Write-Host -Object "Start running unit tests ..."

    # ===========================================================
    # Check if xUnit exist
    # ===========================================================
    if (!(Test-Path -Path $XUnit)) 
    {
        Write-Host -Object "XUnit $XUnit not exist"
        exit 1
    }    

    # ===========================================================
    # Filter project with UnitTests
    # ===========================================================
    $binDirectory = "bin\\Release"
    $unitTestDllMask = "*UnitTests.dll"
    $testDlls = Get-Files -Path $ProjectSrcPath -Filter $unitTestDllMask -ExcludeFolder "wwwroot" -Recurse | Where-Object {$_ -match ".*\\$binDirectory\\.*"}
    
    $projects = @()
    foreach($testDll in $testDlls) 
    {
        $project = (Get-Item -Path $testDll).Directory.FullName
        $projects += $project
    }

    # ===========================================================
    # Run XUnit on each project
    # ===========================================================
    foreach($project in $projects) 
    {
        $assemblyDlls = Get-ChildItem -Path $project -Filter $unitTestDllMask | Select-Object -ExpandProperty FullName

        $assemblies = @()
        foreach ($assemblyDll in $assemblyDlls) 
        {
            $assemblies += $assemblyDll
        }
             
        & $XUnit $assemblies -parallel 'all'
        if ($LASTEXITCODE -ne 0) 
        {
            throw "Failed finish tests for project $project"
        }
    }

}

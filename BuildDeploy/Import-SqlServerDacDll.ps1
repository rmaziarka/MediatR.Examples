function Import-SqlServerDacDll
{
    [CmdletBinding()]
    [OutputType([void])]
    param(
        [Parameter(Mandatory=$false)]
        [string]
        [ValidateSet($null, '2012', '2014')]
        $SqlServerVersion
    )	
    
    Write-Host -Object "Searching for Microsoft.SqlServer.Dac.dll."

    $sqlServerPath = Join-Path -Path ${env:ProgramFiles(x86)} -ChildPath 'Microsoft SQL Server'    
    if (Test-Path -LiteralPath $sqlServerPath) 
    {    
        if ($SqlServerVersion) 
        { 
            $sqlServerVersionMap = @{ 
                '2012' = '110'
                '2014' = '120'
            }

            $potentialDacDllPathsVer = Get-ChildItem -Path $sqlServerPath -Filter $sqlServerVersionMap[$SqlServerVersion] -Directory                      
        } 
        else 
        { 
            $potentialDacDllPathsVer = Get-ChildItem -Path $sqlServerPath -Directory | Where-Object { $_.Name -match "\d+"} | Sort-Object {[convert]::ToInt32($_.Name)} -Descending             
        }
    }
    else 
    {
        throw "Cannot find neither '$sqlServerPath'. Please ensure you have SSDT or Data-Tier Application Framework installed."
    }

    if (!$potentialDacDllPathsVer) 
    {
         throw "Cannot find any DAC version directory under directory: $sqlServerPath. Please ensure you have SSDT or Data-Tier Application Framework installed."
    }
    
    foreach ($potentialDacDllPath in $potentialDacDllPathsVer) 
    {
        $path = Join-Path -Path $potentialDacDllPath.FullName -ChildPath 'Dac\bin\Microsoft.SqlServer.Dac.dll'
        if (Test-Path -LiteralPath $path) 
        {
            $dacDllPath = $path
            break
        }
        $path = Join-Path -Path $potentialDacDllPath.FullName -ChildPath 'Microsoft.SqlServer.Dac.dll'
        if (Test-Path -LiteralPath $path) 
        {
            $dacDllPath = $path
            break
        }
    }

    if (!$dacDllPath) 
    {
       throw "Cannot find Microsoft.SqlServer.Dac.dll under any of following directories: $($potentialDacDllPathsVer.FullName -join ', '). Please ensure you have SSDT or Data-Tier Application Framework installed."
    }

    Write-Host -Object "Found at '$dacDllPath' - importing."
    Add-Type -Path $dacDllPath
}

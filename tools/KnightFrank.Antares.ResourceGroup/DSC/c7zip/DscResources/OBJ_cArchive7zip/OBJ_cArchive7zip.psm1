<#
The MIT License (MIT)

Copyright (c) 2015 Objectivity Bespoke Software Specialists

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
#>

function Get-TargetResource {
    param(    
        [parameter(Mandatory=$true)] 
        [ValidateNotNullOrEmpty()]
        [string] 
        $DestinationPath,

        [parameter(Mandatory = $true)]
        [string]
        $SourcePath, 

        [parameter(Mandatory = $false)]
        [string]
        $Path7zip
    )

    $result = @{ 
        DestinationPath = $DestinationPath;
        Exists = (Test-Path -LiteralPath $DestinationPath)
    }
    return $result
}

function Test-TargetResource {
    param(    
        [parameter(Mandatory=$true)] 
        [ValidateNotNullOrEmpty()]
        [string] 
        $DestinationPath,

        [parameter(Mandatory = $true)]
        [string]
        $SourcePath, 

        [parameter(Mandatory = $false)]
        [string]
        $Path7zip
    )

    $currentSettings = Get-TargetResource @PSBoundParameters
    return $currentSettings.Exists
}


function Set-TargetResource {
    param(    
        [parameter(Mandatory=$true)] 
        [ValidateNotNullOrEmpty()]
        [string] 
        $DestinationPath,

        [parameter(Mandatory = $true)]
        [string]
        $SourcePath, 

        [parameter(Mandatory = $false)]
        [string]
        $Path7zip
    )

    if (!$Path7zip) {
        $Path7zip = Get-PathTo7Zip -FailIfNotFound
    } else { 
        $Path7zip = Join-Path -Path $Path7zip -ChildPath '7z.exe'
    }
    if (!(Test-Path -LiteralPath $Path7zip)) {
        throw "7zip does not exist at '$Path7zip'"
    }

    $args = "x `"$SourcePath`" -o`"$DestinationPath`" -y"
    Write-Verbose -Message "Running $Path7zip $args"
    & "$Path7zip" x "`"$SourcePath`"" "-o`"$DestinationPath`"" "-y"
    if ($lastexitcode) {
        throw "7-Zip failed with exit code $lastexitcode"
    }
}

function Get-PathTo7Zip {
    <#
    .SYNOPSIS
    Returns path to 7-zip. Returns $null or throws an error if it has not been installed.

    .PARAMETER FailIfNotFound
    If on and 7-zip is not installed, an exception will be thrown.
    Otherwise $null will be returned.

    .EXAMPLE
    Get-PathTo7Zip
    #>

    [CmdletBinding()]
    [OutputType([string])]
    param(       
        [Parameter(Mandatory=$false)]
        [switch]
        $FailIfNotFound
    )

    $regEntry = "Registry::HKLM\SOFTWARE\7-Zip"
    # note - registry check will fail if running Powershell x86 on x64 machine
    if (Test-Path -LiteralPath $regEntry) {
        $7zipPath = (Get-ItemProperty -Path $regEntry).Path
        if (!(Test-Path -LiteralPath $7zipPath)) {
            if ($FailIfNotFound) { 
                throw "7zip directory not found at '$7zipPath'."
            } else {
                return $null
            }
        }
    } else { 
        # note - 'Program Files' is hardcoded here as $env:ProgramFiles on Powershell x86 is 'Program Files (x86)'
        $7zipPath = "C:\Program Files\7-Zip"
        if (!(Test-Path -LiteralPath $7zipPath)) {
            $7zipPath = Join-Path -Path $env:ProgramFiles -ChildPath '7-Zip'
        }
        if (!(Test-Path -LiteralPath $7zipPath)) {
            if ($FailIfNotFound) { 
                throw "Cannot find neither 7-zip registry entry at '$regEntry' nor 7-zip directory at '$7zipPath'. Please ensure 7-zip has been installed."
            } else {
                return $null
            }
        }       
    }
    $7zipPath = Join-Path -Path $7zipPath -ChildPath "7z.exe"
    if (!(Test-Path -LiteralPath $7zipPath)) {
        if ($FailIfNotFound) { 
            throw "7z.exe not found at '$7zipPath'"
        } else {
            return $null
        }
    }
    return $7zipPath
}


Export-ModuleMember -Function *-TargetResource

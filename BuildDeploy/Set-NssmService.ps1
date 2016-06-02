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

function Set-NssmService {
    <#
    .SYNOPSIS
    Creates and configures Windows Service using [nssm](https://nssm.cc).

    .DESCRIPTION
    It copies bundled nssm files to Program Files and executes it to create a new service, reconfigure it or to remove a service.
    Note if service is currently running it will be stopped if it needs to be reconfigured.

    .PARAMETER ServiceName
    Windows Service name to create / remove.

    .PARAMETER Path
    Path to the executable that will be run by service. Mandatory if creating a service.

    .PARAMETER Arguments
    Additional arguments for the executable specified in $Path.

    .PARAMETER ServiceDisplayName
    Service display name. If not specified and service does not exist, it will be the same as $ServiceName.

    .PARAMETER ServiceDescription
    Service description. If not specified and service does not exist, it will be empty.

    .PARAMETER StartupType
    Service startup type.

    .PARAMETER Credential
    Credential to use for starting the service. If not specified and service does not exist, LOCAL SYSTEM will be used.

    .PARAMETER Status
    Final status of the service to set.

    .PARAMETER NssmPath
    Path to nssm.exe. If not specified, nssm.exe bundled with PSCI will be copied to Program Files and run from there.

    .PARAMETER Remove
    If $true, the service will be removed.

    .PARAMETER AdditionalParameters
    Additional parameters that will be passed to nssm.exe (nssm set <key> <value>) - see [usage](https://nssm.cc/usage).

    .OUTPUTS
    $true if any change was made, $false otherwise.

    .EXAMPLE
    Set-NssmService -ServiceName 'MyService' -Path 'c:\MyService\MyService.bat' -Arguments '80' -Status Running

    #>
    [CmdletBinding()]
    [OutputType([bool])]
    param (
        [parameter(Mandatory=$true)]
        [string]
        $ServiceName,
   
        [parameter(Mandatory=$false)]
        [string]
        $Path,

        [parameter(Mandatory=$false)]
        [string[]]
        $Arguments,

        [parameter(Mandatory=$false)]
        [string]
        $ServiceDisplayName,

        [parameter(Mandatory=$false)]
        [string]
        $ServiceDescription,

        [parameter(Mandatory=$false)]
        [string]
        [ValidateSet($null, 'Automatic', 'Delayed', 'Manual', 'Disabled')]
        $StartupType = 'Automatic',

        [parameter(Mandatory=$false)]
        [PSCredential]
        $Credential,

        [parameter(Mandatory=$false)]
        [string]
        [ValidateSet($null, 'Running', 'Stopped', 'Paused')]
        $Status = 'Stopped',

        [parameter(Mandatory=$false)]
        [string]
        $NssmPath,

        [parameter(Mandatory=$false)]
        [switch]
        $Remove,

        [parameter(Mandatory=$false)]
        [hashtable]
        $AdditionalParameters
    )

    Write-Log -Info "Configuring service '$ServiceName'"

    if (!$NssmPath) {
        $NssmSrcPath = Get-PathToExternalLib -ModulePath 'nssm'
        $NssmPath = Join-Path -Path $env:ProgramFiles -ChildPath 'nssm'
    } 

    $NssmExePath = Join-Path -Path $NssmPath -ChildPath 'win64\nssm.exe' 

    $serviceChanged = $false
    if (!(Test-Path -Path $NssmExePath)) {
        if (!(Test-Path -Path $NssmSrcPath)) {
            throw "Cannot find nssm at '$NssmSrcPath'."
        }
        Write-Log -Info "Copying nssm from '$NssmSrcPath' to '$NssmPath'"
        Copy-Directory -Path $NssmSrcPath -Destination $NssmPath
    }

    if (!(Test-Path -Path $NssmExePath)) {
        throw "Cannot find nssm at '$NssmExePath'."
    }

    $currentService = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if ($Remove) {
        if ($currentService) {
            Write-Log -Info "Stopping service '$ServiceName'"
            Stop-Service -Name $ServiceName
            Write-Log -Info "Removing service '$ServiceName'"
            [void](Start-ExternalProcess -Command $NssmExePath -ArgumentList @('remove', "`"$ServiceName`"", 'confirm') -Quiet)
            return $true
        } else {
            Write-Log -Info "Service '$ServiceName' already does not exist."
            return $false
        }
    }

    $serviceStopped = !$currentService
    if (!$currentService) {
        Write-Log -Info "Creating service '$ServiceName' with command line '$Path'"
        [void](Start-ExternalProcess -Command $NssmExePath -ArgumentList @('install', "`"$ServiceName`"", "`"$Path`"") -Quiet)
        $serviceStopped = $true
        $serviceChanged = $true
    }

    $appParams = @{}

    if ($Path) { 
        $appParams.Application = $Path
    }

    if ($ServiceDisplayName) {
        $appParams.DisplayName = $ServiceDisplayName
    }

    if ($ServiceDescription) {
        $appParams.Description = $ServiceDescription
    }

    if ($Credential) {
        $appParams.ObjectName = $Credential.UserName
    }

    if ($Arguments) { 
        $appParams.AppParameters = $Arguments -join ' '
    }

    switch ($StartupType) {
        'Automatic' { $appParams.Start = 'SERVICE_AUTO_START' }
        'Delayed' { $appParams.Start = 'SERVICE_DELAYED_START' }
        'Manual' { $appParams.Start = 'SERVICE_DEMAND_START' }
        'Disabled' { $appParams.Start = 'SERVICE_DISABLED' }
    }

    if ($AdditionalParameters) {
        $appParams += $AdditionalParameters
    }

    foreach ($appParam in $appParams.GetEnumerator()) {
        $nssmOutput = ''
        [void](Start-ExternalProcess -Command $NssmExePath -ArgumentList @('get', "`"$ServiceName`"", $appParam.Key) -Quiet -Output ([ref]$nssmOutput))
        $value = $appParam.Value
        if ($nssmOutput -ne $appParam.Value) {
            if (!$serviceStopped) {
                Write-Log -Info "Stopping service '$ServiceName'"
                Stop-Service -Name $ServiceName
                $serviceStopped = $true
            }
            if ($appParam.Key -eq 'ObjectName') {
                
                $value = "`"{0}`" {1}" -f $Credential.UserName, $Credential.GetNetworkCredential().Password
                Write-Log -Info "Setting service '$ServiceName' parameter '$($appParam.Key)' to '$($Credential.UserName) <password>'"
            } else {
                $value ="`"$value`""
                Write-Log -Info "Setting service '$ServiceName' parameter '$($appParam.Key)' to '$value)'"
            }
            
            $argumentList = @('set', "`"$ServiceName`"", $appParam.Key, $value)
            [void](Start-ExternalProcess -Command $NssmExePath -ArgumentList $argumentList -Quiet)
            $serviceChanged = $true
        } else {
            Write-Log -_Debug "Service '$ServiceName' parameter '$($appParam.Key)' is already '$($appParam.Value)'"
        }
    }
    
    $currentService = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if ($currentService.Status -ne $Status) {
        Write-Log -Info "Setting service status from $($currentService.Status) to '$Status'"
        Set-Service -Name $ServiceName -Status $Status
        $serviceChanged = $true
    }

    return $serviceChanged
}
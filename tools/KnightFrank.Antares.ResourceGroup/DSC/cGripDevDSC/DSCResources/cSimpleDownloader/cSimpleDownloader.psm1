function Get-TargetResource
{
  [CmdletBinding()]
  [OutputType([System.Collections.Hashtable])]
  param
  (
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $RemoteFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $DestinationPath
  )

  Write-Verbose "Start Get-TargetResource"

  CheckChocoInstalled

  #Needs to return a hashtable that returns the current
  #status of the configuration component
  $Configuration = @{
    RemoteFileLocation = $RemoteFileLocation
    DestinationPath = $DestinationPath
  }

  return $Configuration
}

function Set-TargetResource
{
  [CmdletBinding()]
  param
  (
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $RemoteFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $DestinationPath,
    [System.String]
    $CookieName,
    [System.String]
    $CookieValue,
    [System.String]
    $CookieDomain
    
  )
  
  Write-Verbose "Start Set-TargetResource"
  Write-Verbose "Downloading  $RemoteFileLocation to $DestinationPath"


  $session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
  
  if ($CookieName)
  {
    $cookie = New-Object System.Net.Cookie  
    $cookie.Name = $CookieName
    $cookie.Value = $CookieValue
    $cookie.Domain = $CookieDomain
    $session.Cookies.Add($cookie);
  }
  
  $containingFile = Split-Path $DestinationPath 
  
  if (-not (Test-Path $containingFile))
  {
    New-Item -ItemType directory -Force -Path $containingFile
  }

  Invoke-WebRequest $RemoteFileLocation -WebSession $session -TimeoutSec 900 -OutFile $DestinationPath
}

function Test-TargetResource
{
  [CmdletBinding()]
  [OutputType([System.Boolean])]
  param
  (
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $RemoteFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $DestinationPath,
    [System.String]
    $CookieName,
    [System.String]
    $CookieValue,
    [System.String]
    $CookieDomain
  )

  Write-Verbose "Start Test-TargetResource"
  
  #Quick check that the file exists and is not tiny (possibly indicates error) 
  #Todo: Replace with CheckSum validation
  if ((Test-Path $DestinationPath) -and ((Get-Item $DestinationPath).length -gt 50kb))
  {
    Write-Verbose "File already exists, returning TRUE"
    
    return $true
  }
  else
  {
    Write-Verbose "File not present, returning FALSE"
    
    return $false
  }
  
}


Export-ModuleMember -Function *-TargetResource

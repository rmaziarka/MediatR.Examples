function Get-TargetResource
{
  [CmdletBinding()]
  [OutputType([System.Collections.Hashtable])]
  param
  (
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ExeFolder,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ExeOrBatName,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $NssmFolder, 
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ServiceName
  )

  Write-Verbose "Start Get-TargetResource"

  #Needs to return a hashtable that returns the current
  #status of the configuration component
  $Configuration = @{
    ExeFolder = $ExeFolder
    ExeOrBatName = $ExeOrBatName
    NssmFolder = $NssmFolder
    ServiceName = $ServiceName
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
    $ExeFolder,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ExeOrBatName,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $NssmFolder, 
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ServiceName
  )

  Write-Verbose "Start Set-TargetResource"

  #Find the bat or exe to run as a service
  $serviceBatLoction = Get-ChildItem -Path $ExeFolder -Filter $ExeOrBatName -Recurse | Select -first 1

  #Get the non-sucky-service-manager exe
  $nssmExeLoction = Get-ChildItem -Path $NssmFolder -Filter nssm.exe -Recurse | ?{ $_.Directory.Name-eq "win64"}

  if ($nssmExeLoction.Count -ne 1)
  {
    throw "Failed to find NSSM.exe has likely changed its install method or Zip structure"
  }

  #Check we're not already installed
  $serviceObject = get-service | ?{$_.Name -like "*$ServiceName*"}
  if ($serviceObject)
  {
    #Stop the currently installed service
    $serviceObject.Stop()

    #Remove if we are
    $logRemoveFilePath = Join-Path $ExeFolder "RemoveLog.txt"
    $removeArgs = "remove $ServiceName confirm"
    Start-Process $nssmExeLoction.FullName $removeArgs -RedirectStandardOutput $logRemoveFilePath -Wait

    #Wait for intall as service manager can be nice and laggy
    Start-Sleep -s 2
  }



  #Create a service, using nssm, to host the exe or bat
  $logFilePath = Join-Path $ExeFolder "InstallLog.txt"
  $installArgs = "install $ServiceName $($serviceBatLoction.FullName)"
  Start-Process $nssmExeLoction.FullName $installArgs -RedirectStandardOutput $logFilePath -Wait


  #Wait for intall
  Start-Sleep -s 2

  $serviceObject = get-service | ?{$_.Name -eq $ServiceName}

  #Check the service appeared
  if ($serviceObject.Count -ne 1)
  {
    throw "Service failed to install correctly"
  }
  else
  {
    Write-Verbose "Service Present on machine (may have already been installed)"
    Write-Verbose "$($serviceObject | Format-List | Out-String)"
  }

  #Set it up to start automatically and Start it, if needed
  if ($serviceObject.Status -eq "Stopped")
  {
    $serviceObject.Start()
  }

  $serviceObject | Set-Service -StartupType Automatic
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
    $ExeFolder,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ExeOrBatName,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $NssmFolder, 
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ServiceName
  )

  Write-Verbose "Start Test-TargetResource"

  Write-Verbose "Looking for service with name $ServiceName"

  $serviceObject = get-service | ?{$_.Name -eq $ServiceName}

  if ($serviceObject.Count -ne 1)
  {
    Write-Verbose "Service not present on machine"
    Return $false
  }
  else
  {
    Write-Verbose "Service Present on machine (may have already been installed)"
    Write-Verbose "$($serviceObject | Format-List | Out-String)"

    #Check if we need to start it again
    if ($serviceObject.Status -eq "Stopped")
    {
      $serviceObject.Start()
    }
  }

  Return $true
}


Export-ModuleMember -Function *-TargetResource

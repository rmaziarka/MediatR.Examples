function Get-TargetResource
{
  [CmdletBinding()]
  [OutputType([System.Collections.Hashtable])]
  param
  (
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $ZipFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $UnzipFolder

  )

  Write-Verbose "Start Get-TargetResource"

  CheckChocoInstalled

  #Needs to return a hashtable that returns the current
  #status of the configuration component
  $Configuration = @{
    UnzipFolder = $UnzipFolder
    ZipFileLocation = $ZipFileLocation
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
    $ZipFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $UnzipFolder
  )
  Write-Verbose "Start Set-TargetResource"
  Write-Verbose "Unzipping $ZipFileLocation to $UnzipFolder"

  Unzip $ZipFileLocation $UnzipFolder
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
    $ZipFileLocation,
    [parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $UnzipFolder
  )

  Write-Verbose "Start Test-TargetResource"

  if (-not (Test-Path $ZipFileLocation))
  {
    throw "Zip file $ZipFileLocation not present, check file path for ZipFileLocation parameter"
  }

  if (-not (Test-Path $UnzipFolder))
  {
    Write-Verbose "Unzip Location $UnzipFolder not present suggesting not unzipped, Test returning false"
    Return $false
  }
  Write-Verbose "Unzip Location present $UnzipFolder, returning true"

  Return $true
}

#Inspired by https://gist.github.com/xpando/8a896d903ceb7cc31192
Function Unzip($path,$to)
{
  $7z = "$env:TEMP\7z"
  if (!(test-path $7z) -or !(test-path "$7z\7za.exe"))
  {
    if (!(test-path $7z)) { md $7z | out-null }
    push-location $7z
    try
    {
      Write-Verbose "Downloading 7zip"
      $wc = new-object system.net.webClient
      $wc.headers.add('user-agent', [Microsoft.PowerShell.Commands.PSUserAgent]::FireFox)
      $wc.downloadFile("http://softlayer-dal.dl.sourceforge.net/project/sevenzip/7-Zip/9.20/7za920.zip","$7z\7z.zip")

      add-type -assembly "system.io.compression.filesystem"
      [io.compression.zipfile]::extracttodirectory("$7z\7z.zip","$7z")
      del .\7z.zip
    }
    catch { throw "Failed to download 7zip to temp location, aborting"}
    finally { pop-location }
  }

  $logFilePath = Join-Path $to "UnzipLog.txt"
  Write-Verbose "Unzip output logged to $logFilePath"

  if ($path.endswith('.tar.gz') -or $path.endswith('.tgz'))
  {
    #Pipe output of gz unzip and unzip remaining tar files.
    $cmdline = "cmd"
    $arguments = "/C `"^`"$7z\7za.exe^`" x ^`"$path^`" -so | ^`"$7z\7za.exe^`" x -y -si -ttar -o^`"$to^`""
    $proc = start-process $cmdline $arguments -RedirectStandardOutput $logFilePath -LoadUserProfile -Wait -PassThru
    if ($proc.ExitCode -ne 0)
    {
      throw "Error when unzipping with 7zip"
    }
  }
  else
  {
    $proc = start-process "$7z\7za.exe" "x $path -y -o$to" -RedirectStandardOutput $logFilePath -LoadUserProfile -Wait -PassThru
    if ($proc.ExitCode -ne 0)
    {
      throw "Error when unzipping with 7zip"
    }
  }
}


Export-ModuleMember -Function *-TargetResource

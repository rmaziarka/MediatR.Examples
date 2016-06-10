$RemoteFileLocation = "http://download.oracle.com/otn-pub/java/jdk/8u45-b15/server-jre-8u45-windows-x64.tar.gz"
  			$DownloadFolder = "c:\temp\"
  			$FileName = "jre.tar.gz"

  
  $downloadToPath = Join-Path $DownloadFolder $FileName
  
    Write-Host "Attempting with Cookie"
    #$response = Invoke-WebRequest $RemoteFileLocation -OutFile $downloadToPath -Headers @{"Cookie" = $Cookie} -SessionVariable session
    $session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
    
  $cookie = New-Object System.Net.Cookie 
    
  $cookie.Name = "oraclelicense"
  $cookie.Value = "accept-securebackup-cookie"
  $cookie.Domain = ".oracle.com"

  $session.Cookies.Add([System.Net.Cookie]$cookie);

  Invoke-WebRequest $RemoteFileLocation -WebSession $session -TimeoutSec 900 -OutFile $downloadToPath




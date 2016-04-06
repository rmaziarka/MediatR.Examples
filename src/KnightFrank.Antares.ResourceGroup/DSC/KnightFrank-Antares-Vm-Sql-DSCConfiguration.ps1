Configuration SetupSQLVm
{

Param ( [string] $nodeName,
		
		[Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $PackagePath,
 
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $WinSources )

Import-DscResource -ModuleName PSDesiredStateConfiguration

Node $nodeName
{
    Log ParamLog
    {
        Message = "Running SQLInstall. PackagePath = $PackagePath"
    }
 
    WindowsFeature NetFramework35Core
    {
        Name = "NET-Framework-Core"
        Ensure = "Present"
        Source = $WinSources
    }
 
    WindowsFeature NetFramework45Core
    {
        Name = "NET-Framework-45-Core"
        Ensure = "Present"
        Source = $WinSources
    }
 
    # copy the sqlserver iso
    File SQLServerIso
    {
        SourcePath = "$PackagePath\en_sql_server_2014_developer_edition_x86_x64_dvd_813280.iso"
        DestinationPath = "c:\temp\SQLServer.iso"
        Type = "File"
        Ensure = "Present"
    }
 
    # copy the ini file to the temp folder
    File SQLServerIniFile
    {
        SourcePath = "$PackagePath\ConfigurationFile.ini"
        DestinationPath = "c:\temp"
        Type = "File"
        Ensure = "Present"
        DependsOn = "[File]SQLServerIso"
    }
 
    #
    # Install SqlServer using ini file
    #
    Script InstallSQLServer
    {
        GetScript =
        {
            $sqlInstances = gwmi win32_service -computerName localhost | ? { $_.Name -match "mssql*" -and $_.PathName -match "sqlservr.exe" } | % { $_.Caption }
            $res = $sqlInstances -ne $null -and $sqlInstances -gt 0
            $vals = @{
                Installed = $res;
                InstanceCount = $sqlInstances.count
            }
            $vals
        }
        SetScript =
        {
            # mount the iso
            $setupDriveLetter = (Mount-DiskImage -ImagePath c:\temp\SQLServer.iso -PassThru | Get-Volume).DriveLetter + ":"
            if ($setupDriveLetter -eq $null) {
                throw "Could not mount SQL install iso"
            }
            Write-Verbose "Drive letter for iso is: $setupDriveLetter"
                 
            # run the installer using the ini file
            $cmd = "$setupDriveLetter\Setup.exe /ConfigurationFile=c:\temp\ConfigurationFile.ini /SQLSVCPASSWORD=P2ssw0rd /AGTSVCPASSWORD=P2ssw0rd /SAPWD=P2ssw0rd"
            Write-Verbose "Running SQL Install - check %programfiles%\Microsoft SQL Server\120\Setup Bootstrap\Log\ for logs..."
            Invoke-Expression $cmd | Write-Verbose
        }
        TestScript =
        {
            $sqlInstances = gwmi win32_service -computerName localhost | ? { $_.Name -match "mssql*" -and $_.PathName -match "sqlservr.exe" } | % { $_.Caption }
            $res = $sqlInstances -ne $null -and $sqlInstances -gt 0
            if ($res) {
                Write-Verbose "SQL Server is already installed"
            } else {
                Write-Verbose "SQL Server is not installed"
            }
            $res
        }
    }
}
}
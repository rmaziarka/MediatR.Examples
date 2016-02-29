Configuration Main
{

Param ( [string] $nodeName, [string] $dbUserName, [string] $dbPassword )

Import-DscResource -ModuleName PSDesiredStateConfiguration
Import-DscResource -ModuleName xNetworking
Import-DscResource -ModuleName xSQLServer

$securePassword = ConvertTo-SecureString -String $dbPassword -AsPlainText -Force
$loginCredential = New-Object System.Management.Automation.PSCredential ($dbUserName, $SecurePassword)

Node $nodeName
  {
   xFirewall WebFirewallRule 
        { 
            Direction = "Inbound" 
            Name = "SQL-TCP-In" 
            DisplayName = "Sql Server (TCP In)" 
            Description = "Allow incoming sql traffic." 
            DisplayGroup = "SQL" 
            State = "Enabled" 
            Access = "Allow" 
            Protocol = "TCP" 
            LocalPort = "1433" 
            Ensure = "Present" 
        }

	xSQLServerLogin CreateLogin
		{
			LoginType = "SQL"
			LoginCredential = $loginCredential
			Ensure = "Present"
		}
  }
}
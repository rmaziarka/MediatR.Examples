configuration cNssmConfig
{
	Import-DSCResource -Module cGripDevDSC
	
    node("localhost")
    {
        LocalConfigurationManager
        {
            DebugMode = 'ForceModuleImport'
        }
		
		cNssm InstallKibana
		{
			ExeFolder = 'C:\elkInstall\kibana\kibana-4.0.1-windows\bin'
            NssmFolder = 'C:\elkInstall\nssmkibana'
            ServiceName = 'KibanaTest'
            ExeOrBatName = 'kibana.bat'
		}
	}
}

cNssmConfig

Start-DSCConfiguration .\cNssmConfig -wait -verbose -force
function Build-All {
    param(
        [Parameter(Mandatory=$false)]
        [string]
        $Version
    )

    <# Get-ConfigurationPaths returns an object with the following properties:
       ProjectRootPath         - base directory of the project, relative to the directory where this script resides (it is used as a base directory for other directories)
       PackagesPath            - path to directory with packages
       PackagesContainDeployScripts - $true if $PackagesPath exists and contains DeployScripts / PSCI
       DeployConfigurationPath - path to directory with configuration files
       DeployScriptsPath       - path to directory with deploy.ps1
    #>
	
    Build-Backend @PSBoundParameters
    Run-Tests @PSBoundParameters
    Build-Database @PSBoundParameters
	Build-Frontend @PSBoundParameters

    # Package configuration scripts and PSCI itself
    Build-DeploymentScriptsPackage 
}

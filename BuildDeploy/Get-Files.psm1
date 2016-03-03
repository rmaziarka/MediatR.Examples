function Get-Files
{
    param
    (        
        [Parameter(Mandatory=$true)]
        [string]
        $Path,

        [Parameter(Mandatory=$true)]
        [string]
        $Filter,
        
        [Parameter(Mandatory=$false)]
        [string]
        $ExcludeFolder,

        [Parameter(Mandatory=$false)]
        [switch]
        $Recurse
    )    

    $result = @()    

    if($Recurse)
    {    
        $directories = Get-ChildItem -Path $Path | Where {$_.PSIsContainer -and ($ExcludeFolder -eq $null -or $_.Name -ne $ExcludeFolder)} | Select-Object -ExpandProperty FullName

        foreach($directory in $directories) 
        {
            if($ExcludeFolder -ne $null)
            {            
                $result += Get-Files -Path $directory -Filter $Filter -ExcludeFolder $ExcludeFolder -Recurse
            }
            else 
            {
                $result += Get-Files -Path $directory -Filter $Filter -Recurse
            }
        }
    }

    $result += Get-ChildItem -Path $Path -Filter $Filter | Select-Object -ExpandProperty FullName
    return $result;
}

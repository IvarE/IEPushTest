param (
	[string[]]
	$hosts,
	[string]
	$BranchVersion,
	[string]
	$ReleaseId

)

$PathSuffix = "_" + $BranchVersion + "_" + $ReleaseId

if (-not $hosts)
{
    Write-Host "Usage: " $script:MyInvocation.MyCommand.Name "-hosts @("host1", "host2")"
}
else 
{
    $credential = New-Object System.Management.Automation.PSCredential -ArgumentList "D1\CrmAdmin", (ConvertTo-SecureString -String “uSEme2!nstal1” -AsPlainText -Force)
	Write-Host "BranchVersion:" + $BranchVersion + " ReleaseId:" + $ReleaseId 
    foreach ($hostname in $hosts)
    {
		Invoke-Command -ComputerName $hostname -ScriptBlock { param($PathSuffix) D:\PowerShell\NewCGIXrmServices.ps1 -PathSuffix $PathSuffix } -ArgumentList $PathSuffix -Credential $credential -Verbose
        #Invoke-Command -ComputerName $hostname -ScriptBlock { D:\PowerShell\NewCGIXrmServices.ps1 } -credential $credential -Verbose
        #Invoke-Command -ComputerName $hostname -ScriptBlock { D:\PowerShell\NewCrmEAIConnectorService.ps1 } -credential $credential -Verbose
        #Invoke-Command -ComputerName $hostname -ScriptBlock { D:\PowerShell\NewCrmExtConnetorService.ps1 } -credential $credential -Verbose
        #Invoke-Command -ComputerName $hostname -ScriptBlock { D:\PowerShell\NewCRMExtIntegrationPortal.ps1 } -credential $credential -Verbose
        #Invoke-Command -ComputerName $hostname -ScriptBlock { D:\PowerShell\NewCRMPortalService.ps1 } -credential $credential -Verbose
    }
}
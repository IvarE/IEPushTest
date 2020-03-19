param (
	[string]
	$environment
)

$credential = New-Object System.Management.Automation.PSCredential -ArgumentList "D1\CrmAdmin", (ConvertTo-SecureString -String “uSEme2!nstal1” -AsPlainText -Force)
$hostname = "v-dkvs-utv"

Invoke-Command -ComputerName $hostname -ScriptBlock { param($env) C:\PowerShell\DeploySolution.ps1 -environment $env } -ArgumentList $environment -credential $credential -Verbose
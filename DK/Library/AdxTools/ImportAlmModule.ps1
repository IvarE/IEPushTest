<#
.SYNOPSIS 
Imports the ALM Toolkit modules.

.NOTES
On Windows 7, install PowerShell V3: http://social.technet.microsoft.com/wiki/contents/articles/4725.powershell-v3-guide.aspx
- alternatively, enable PowerShell V2 .NET Framework 4.0 support by configuring the powershell.exe.config file.

Requires Microsoft® Windows PowerShell Extensions for Microsoft® SQL Server® 2012
- Prerequisites: Microsoft® SQL Server® 2012 Shared Management Objects, Microsoft® System CLR Types for Microsoft® SQL Server® 2012
- Help: http://msdn.microsoft.com/en-us/library/hh245198.aspx
- Download: http://www.microsoft.com/en-us/download/details.aspx?id=29065

.PARAMETER IncludeSqlPs
Imports the SQLPS module.

.EXAMPLE
PS C:\> .\ImportAlmModule.ps1 -IncludeSqlPs
#>

param (
	[Switch]
	$includeSqlPs,

	$modulePaths = (
		".\Adxstudio.Xrm.PowerShell",
		".\bin\Adxstudio.Xrm.PowerShell",
		".\bin\ALM Toolkit\Adxstudio.Xrm.PowerShell",
		"..\Adxstudio.Xrm.PowerShell",
		"..\bin\Adxstudio.Xrm.PowerShell",
		"..\bin\ALM Toolkit\Adxstudio.Xrm.PowerShell"
	)
)

if ($PSVersionTable.CLRVersion -lt "4.0")
{
	Write-Host ("Incompatible CLR Version: {0}" -f $PSVersionTable.CLRVersion)
	Write-Host "Install PowerShell V3: http://social.technet.microsoft.com/wiki/contents/articles/4725.powershell-v3-guide.aspx"
	return
}

function Get-ScriptDirectory
{
	if ($script:MyInvocation.MyCommand.Path) { Split-Path $script:MyInvocation.MyCommand.Path } else { $pwd }
}

$scriptPath = Get-ScriptDirectory
$modulePath = $modulePaths | ? { Test-Path (Join-Path $scriptPath $_) } | select -f 1 | % { Join-Path $scriptPath $_ -Resolve }

Write-Host "Loading modules..."

Import-Module $modulePath -DisableNameChecking

if ($includeSqlPs)
{
	$originalLocation = Get-Location
	Import-Module "sqlps" -DisableNameChecking

	if ($pwd.Path -eq "SQLSERVER:\")
	{
		Set-Location $originalLocation
	}
}

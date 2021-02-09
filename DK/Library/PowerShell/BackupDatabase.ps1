param (
	[string]
	$dbhost,

	[string]
	$dbname
)

$dt = get-date -format yyyyMMddHHmmss
$credential = New-Object System.Management.Automation.PSCredential -ArgumentList "backupadmin", (ConvertTo-SecureString -String “uSEme2!nstal1” -AsPlainText -Force)
$backupfolder = "\\v-dksql3\sqlbkp$\MSSQL11.CLCMSDB-SQL1\MSSQL\Backup\$($dbhost)\$($dbname)\FULL"
#$backupfile = "\\v-dksql3\sqlbkp$\MSSQL11.CLCMSDB-SQL1\MSSQL\Backup\$($dbhost)\$($dbname)\FULL\$($dbname)_db_$($dt)_$($Env:BUILD_BUILDNUMBER).bak"
$backupfile = "$($backupfolder)\$($dbname)_db_$($dt)_$($Env:BUILD_BUILDNUMBER).bak"

# kör katalogskapadet / kontroll som crmadmin
$pscredential = New-Object System.Management.Automation.PSCredential -ArgumentList "D1\CrmAdmin", (ConvertTo-SecureString -String “uSEme2!nstal1” -AsPlainText -Force)
New-PSDrive -Name SqlBkp -PSProvider FileSystem -Root \\v-dksql3\sqlbkp$ -Credential $pscredential
$psdfolder = $backupfolder.Replace("\\v-dksql3\sqlbkp$","SqlBkp:")

Write-Host $psdfolder
# skapa katalogen
if ((Test-Path($psdfolder)) -eq 0)
{
	New-Item -ItemType Directory  -Force -Path $psdfolder
}

Remove-PSDrive SqlBkp

Backup-SqlDatabase -ServerInstance $dbhost -Database $dbname -BackupFile $backupfile -Credential $credential
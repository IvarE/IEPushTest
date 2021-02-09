param (
	[string]
	$PathSuffix
)

[Reflection.Assembly]::LoadFile('C:\Windows\System32\inetsrv\Microsoft.Web.Administration.dll') 



$WebSiteName = "CGIXrmServices"
$AppPoolName = "CGIXrmServices"
$BasePath = "D:\Crm"
$Port = 4500
$FolderPath = $WebSiteName + $PathSuffix

Write-Host "FolderPath:" + $FolderPath

Try {
    Remove-WebSite -Name $WebSiteName
}
Catch
{
    Write-Host "Site " $WebSiteName " did not exist!"
}

Try {
    Remove-WebAppPool -Name $AppPoolName
}
Catch
{
    Write-Host "AppPool " $AppPoolName " did not exist!"
}

# If path does not exist New-Website will fail
#if ((Test-Path $BasePath\$WebSiteName) -And (Test-Path $BasePath\$WebSiteName\CGIXrmGetOrdersService) -And (Test-Path $BasePath\$WebSiteName\CGIXrmCreateCaseService) -And (Test-Path $BasePath\$WebSiteName\CGIXrmTravelcardService))
if ((Test-Path $BasePath\$FolderPath) -And (Test-Path $BasePath\$FolderPath\CGIXrmGetOrdersService) -And (Test-Path $BasePath\$FolderPath\CGIXrmCreateCaseService) -And (Test-Path $BasePath\$FolderPath\CGIXrmTravelcardService))

{
    New-WebAppPool -Name $AppPoolName
    $appPool = Get-Item "IIS:\AppPools\$AppPoolName"
    $appPool.processModel.identityType = 4
    $appPool.managedRuntimeVersion = "v4.0"
    $appPool.managedPipeLineMode = "Integrated"
    $appPool | Set-Item

    New-Website -Name $WebSiteName -Port $Port -ApplicationPool $AppPoolName -PhysicalPath $BasePath\$FolderPath

    New-WebApplication -Site $WebSiteName -ApplicationPool $AppPoolName -PhysicalPath D:\Crm\$FolderPath\CGIXrmGetOrdersService -Name CGIXrmGetOrdersService
    New-WebApplication -Site $WebSiteName -ApplicationPool $AppPoolName -PhysicalPath D:\Crm\$FolderPath\CGIXrmCreateCaseService -Name CGIXrmCreateCaseService
    New-WebApplication -Site $WebSiteName -ApplicationPool $AppPoolName -PhysicalPath D:\Crm\$FolderPath\CGIXrmTravelcardService -Name CGIXrmTravelcardService
}
[Reflection.Assembly]::LoadFile('C:\Windows\System32\inetsrv\Microsoft.Web.Administration.dll') 

$WebSiteName = "CRMPortalService"
$AppPoolName = "CRMPortalService"
$BasePath = "D:\Crm"
$Port = 4002

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
if (Test-Path $BasePath\$WebSiteName)
{
    New-WebAppPool -Name $AppPoolName
    $appPool = Get-Item "IIS:\AppPools\$AppPoolName"
    $appPool.processModel.identityType = 4
    $appPool.managedRuntimeVersion = "v4.0"
    $appPool.managedPipeLineMode = "Integrated"
    $appPool | Set-Item

    New-Website -Name $WebSiteName -Port $Port -ApplicationPool $AppPoolName -PhysicalPath $BasePath\$WebSiteName
}
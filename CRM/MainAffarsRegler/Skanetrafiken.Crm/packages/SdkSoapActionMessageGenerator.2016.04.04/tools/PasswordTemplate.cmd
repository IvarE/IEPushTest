@echo off

set discoveryUri=https://crm.endeavor.se:1444/EndeavorDevProject/XRMServices/2011/Discovery.svc
set organizationUri=https://crm.endeavor.se:1444/EndeavorDevProject/XRMServices/2011/Organization.svc
set organizationName=EndeavorLicense
set minOutputFolder=Messages\min
set docOutputFolder=Messages\vsdoc
set endpointType=ActiveDirectory
set domain=Endeavor
set username=hsteen

@echo Generating Entities from %organizationUri% to %minOutputFolder% and %docOutputFolder%.

powershell -Command $pword = read-host "Enter password for %domain%\%username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

Sdk.SoapActionMessageGenerator.exe ^
/Endeavor.DiscoveryUri:"%discoveryUri%" ^
/Endeavor.OrganizationUri:"%organizationUri%" ^
/Endeavor.OrganizationName:"%organizationName%" ^
/Endeavor.EndpointType:"%endpointType%" ^
/Endeavor.MinOutputFolder:"%minOutputFolder%" ^
/Endeavor.DocOutputFolder:"%docOutputFolder%" ^
/Endeavor.Username:"%username%" ^
/Endeavor.Password:"%password%" ^
/Endeavor.Domain:"%domain%" ^
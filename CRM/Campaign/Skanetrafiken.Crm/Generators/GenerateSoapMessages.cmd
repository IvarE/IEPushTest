@echo off

set discoveryUri=https://crm.endeavor.se:1444/XRMServices/2011/Discovery.svc
set organizationUri=https://crm.endeavor.se:1444/SkanetrafikenDev/XRMServices/2011/Organization.svc
set organizationName=SkanetrafikenDev
set endpointType=ActiveDirectory
set minOutputFolder=..\Webresource\script
set docOutputFolder=..\Webresource\script\vsdoc
set username=endeavor\jandersson

@echo Generating Entities from %organizationUri% to %minOutputFolder% and %docOutputFolder%.

powershell -Command $pword = read-host "Enter password for %username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

"..\packages\SdkSoapActionMessageGenerator.2016.04.04\tools\Sdk.SoapActionMessageGenerator.exe" ^
/Endeavor.DiscoveryUri:"%discoveryUri%" ^
/Endeavor.OrganizationUri:"%organizationUri%" ^
/Endeavor.OrganizationName:"%organizationName%" ^
/Endeavor.EndpointType:"%endpointType%" ^
/Endeavor.MinOutputFolder:"%minOutputFolder%" ^
/Endeavor.DocOutputFolder:"%docOutputFolder%" ^
/Endeavor.Username:"%username%" ^
/Endeavor.Password:"%password%" ^
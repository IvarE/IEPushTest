@echo off

set discoveryUri=https://disco.crm4.dynamics.com/XRMServices/2011/Discovery.svc
set organizationUri=https://sodraskogverifiering.crm4.dynamics.com/XRMServices/2011/Organization.svc
set organizationName=org1049b9a9
set endpointType=OnlineFederation
set minOutputFolder=Messages\min
set docOutputFolder=Messages\vsdoc
set username=johan.andersson@sodra.com

@echo Generating Entities from %organizationUri% to %minOutputFolder% and %docOutputFolder%.

powershell -Command $pword = read-host "Enter password for %username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

Sdk.SoapActionMessageGenerator.exe ^
/Endeavor.DiscoveryUri:"%discoveryUri%" ^
/Endeavor.OrganizationUri:"%organizationUri%" ^
/Endeavor.OrganizationName:"%organizationName%" ^
/Endeavor.EndpointType:"%endpointType%" ^
/Endeavor.MinOutputFolder:"%minOutputFolder%" ^
/Endeavor.DocOutputFolder:"%docOutputFolder%" ^
/Endeavor.Username:"%username%" ^
/Endeavor.Password:"%password%" ^
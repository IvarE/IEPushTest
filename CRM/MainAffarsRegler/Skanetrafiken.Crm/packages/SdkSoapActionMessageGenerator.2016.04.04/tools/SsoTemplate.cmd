@echo off

set discoveryUri=https://crm.endeavor.se:1444/EndeavorDevProject/XRMServices/2011/Discovery.svc
set organizationUri=https://crm.endeavor.se:1444/EndeavorDevProject/XRMServices/2011/Organization.svc
set organizationName=EndeavorLicense
set minOutputFolder=Messages\min
set docOutputFolder=Messages\vsdoc
set endpointType=ActiveDirectory
set useDefaultCredentials=true

@echo Generating Entities from %organizationUri% to %minOutputFolder% and %docOutputFolder%.

Sdk.SoapActionMessageGenerator.exe ^
/Endeavor.DiscoveryUri:"%discoveryUri%" ^
/Endeavor.OrganizationUri:"%organizationUri%" ^
/Endeavor.OrganizationName:"%organizationName%" ^
/Endeavor.EndpointType:"%endpointType%" ^
/Endeavor.MinOutputFolder:"%minOutputFolder%" ^
/Endeavor.DocOutputFolder:"%docOutputFolder%" ^
/Endeavor.UseDefaultCredentials:"%useDefaultCredentials%" ^
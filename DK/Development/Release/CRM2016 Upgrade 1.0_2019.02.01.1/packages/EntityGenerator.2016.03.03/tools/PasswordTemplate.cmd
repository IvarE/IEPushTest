@echo off

set url=https://crm.endeavor.se/EndeavorDevProject/XRMServices/2011/Organization.svc
set domain=Endeavor
set username=hsteen
set out=GeneratedEntities.cs
set filter=FilterTemplate.xml

@echo Generating Entities from %url% to %out% filtered by %filter%.

powershell -Command $pword = read-host "Enter password for %domain%\%username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

crmsvcutil.exe ^
/url:%url% ^
/out:%out% ^
/namespace:Endeavor.Crm.Schema.Generated ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/domain:%domain% ^
/username:%username% ^
/password:%password% ^
/Endeavor.FilterFile:%filter%

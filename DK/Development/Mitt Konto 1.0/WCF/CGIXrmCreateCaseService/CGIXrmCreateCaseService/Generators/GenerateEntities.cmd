@echo off
rem powershell -Command $pword = read-host "Skånetrafiken ED-miljö, Enter password for account ???" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt


"..\..\..\..\packages\EntityGenerator.2016.03.03\tools\CrmSvcUtil.exe" ^
/url:https://crm.endeavor.se:1444/SkanetrafikenDev/XRMServices/2011/Organization.svc ^
/out:..\GeneratedEntities.cs ^
/namespace:CGIXrmCreateCaseService.Schema.Generated ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/Endeavor.FilterFile:GenerateEntities.xml


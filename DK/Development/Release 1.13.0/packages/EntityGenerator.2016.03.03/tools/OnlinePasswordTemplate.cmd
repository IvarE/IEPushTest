@echo off

set url=https://sodraskogverifiering.crm4.dynamics.com/XRMServices/2011/Organization.svc
set username=johan.andersson@sodra.com
set out=GeneratedEntities_Online.cs
set filter=FilterTemplate.xml

@echo Generating Entities from %url% to %out% filtered by %filter%.

powershell -Command $pword = read-host "Enter password for %username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

CrmSvcUtil.exe ^
/url:%url% ^
/out:"%out%" ^
/username:"%username%" ^
/password:"%password%" ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/Endeavor.FilterFile:%filter%

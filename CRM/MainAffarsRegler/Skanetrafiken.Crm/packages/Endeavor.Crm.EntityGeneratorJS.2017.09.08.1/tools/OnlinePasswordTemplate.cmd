@echo off

set url=https://endeavorincident.crm4.dynamics.com
set username=endeavor@endeavorincident.onmicrosoft.com
set authtype=Office365
set filter=FilterTemplateRegion.xml

@echo Generating Entities from %url% to filtered by %filter%.

powershell -Command $pword = read-host "Enter password for %username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

Endeavor.Crm.JavaScriptGenerator.exe ^
/connectionString:"Url=%url%; Username=%username%; Password=%password%; authtype=%authtype%" ^
/Endeavor.FilterFile:%filter%

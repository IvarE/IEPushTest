@echo off

set url=https://crm.endeavor.se:1444/EndeavorDevScheduleEngine
set username=endeavor\hsteen
set authtype=AD
set filter=FilterTemplateRegion.xml

@echo Generating Entities from %url% to %out% filtered by %filter%.

powershell -Command $pword = read-host "Enter password for %username%" -AsSecureString ; $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword) ; [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR) > .tmp.txt & set /p password=<.tmp.txt & del .tmp.txt

Endeavor.Crm.JavaScriptGenerator.exe ^
/connectionString:"Url=%url%; Username=%username%; Password=%password%; authtype=%authtype%" ^
/Endeavor.FilterFile:%filter%

@echo off

set url=https://crm.endeavor.se:1444/EndeavorDevScheduleEngine
set authtype=AD
set filter=FilterTemplateRegion.xml

@echo Generating Entities from %url% to filtered by %filter% with user %USERDOMAIN%\%USERNAME%.

Endeavor.Crm.JavaScriptGenerator.exe ^
/connectionString:"Url=%url%; authtype=%authtype%" ^
/Endeavor.FilterFile:%filter%

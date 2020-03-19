@echo off

set url=https://crm.endeavor.se:1444/SkanetrafikenDevCampaign
set authtype=AD
set filter=GenerateJavaScriptEntities.xml

@echo Generating Entities from %url% to filtered by %filter% with user %USERDOMAIN%\%USERNAME%.

..\packages\Endeavor.Crm.EntityGeneratorJS.2017.09.08.1\tools\Endeavor.Crm.JavaScriptGenerator.exe ^
/connectionString:"Url=%url%; authtype=%authtype%" ^
/Endeavor.FilterFile:%filter%

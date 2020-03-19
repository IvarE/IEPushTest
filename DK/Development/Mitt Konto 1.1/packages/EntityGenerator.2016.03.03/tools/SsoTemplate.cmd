@echo off

set url=https://crm.endeavor.se:1444/EndeavorDevProject/XRMServices/2011/Organization.svc
set domain=Endeavor
set username=hsteen
set out=GeneratedEntities.cs
set filter=FilterTemplate.xml

@echo Generating Entities from %url% to %out% filtered by %filter% with user %USERDOMAIN%\%USERNAME%.

crmsvcutil.exe ^
/url:%url% ^
/out:"%out%" ^
/namespace:Endeavor.Crm.Schema.Generated ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/Endeavor.FilterFile:%filter%

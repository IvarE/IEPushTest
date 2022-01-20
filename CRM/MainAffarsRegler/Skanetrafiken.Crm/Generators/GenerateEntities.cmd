@echo off

"..\packages\EntityGenerator.2016.11.18\tools\CrmSvcUtil.exe" ^
/url:https://sekundacc.skanetrafiken.se/DKCRMACC/XRMServices/2011/Organization.svc ^
/out:..\GeneratedEntities.cs ^
/namespace:Skanetrafiken.Crm.Schema.Generated ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/serviceContextName:"CrmContext" ^
/username:"D1\CRMAdmin" ^
/password:"uSEme2!nstal1" ^
/Endeavor.FilterFile:GenerateEntities.xml

pause;
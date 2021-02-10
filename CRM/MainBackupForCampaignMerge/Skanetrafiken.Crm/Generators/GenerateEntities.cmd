@echo off

"..\packages\EntityGenerator.2016.11.18\tools\CrmSvcUtil.exe" ^
/url:https://crm.endeavor.se:1444/SkanetrafikenDev/XRMServices/2011/Organization.svc ^
/out:..\GeneratedEntities.cs ^
/namespace:Skanetrafiken.Crm.Schema.Generated ^
/codewriterfilter:Endeavor.Crm.CodeWriterFilter,Endeavor.Crm.EntityGenerator ^
/codecustomization:"Endeavor.Crm.CodeCustomizationService,Endeavor.Crm.EntityGenerator" ^
/Endeavor.FilterFile:GenerateEntities.xml
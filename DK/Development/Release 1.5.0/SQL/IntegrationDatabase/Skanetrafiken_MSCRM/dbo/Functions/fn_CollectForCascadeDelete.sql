CREATE function fn_CollectForCascadeDelete
(
@x CascadeCollectionTable READONLY,
@f int,
@u int
)
returns @t table
(
o uniqueidentifier NOT NULL,
t int NOT NULL,
d int NOT NULL,
s int NOT NULL,
PRIMARY KEY CLUSTERED(t,o,d)
)as begin
insert into @t(o,t,d,s)select o,t,c,0 from @x
declare @c int=-1
declare @n int=0
declare @l int=1
declare @y int
while(0<@l)begin
set @c=@n
set @n=@n+1
DECLARE yc CURSOR LOCAL FAST_FORWARD
FOR SELECT DISTINCT t FROM @t where d=@c
OPEN yc
FETCH NEXT FROM yc INTO @y
while(0=@@FETCH_STATUS)begin
if(@y=9333)begin
if(exists(select*from SavedQueryVisualizationAsIfPublished o,@t c where c.t=9333 and c.o=o.WebResourceId and c.d=@c))goto b
goto a end
if(@y=1030)begin
if(@u=1)insert into @t(o,t,d,s)select o.ProcessTriggerId,4712,@n,ComponentState from ProcessTriggerLogicalAsIfPublished o,@t c where c.t=1030 and c.o=o.FormId and c.d=@c
 else insert into @t(o,t,d,s)select o.ProcessTriggerId,4712,@n,0 from ProcessTriggerAsIfPublished o,@t c where c.t=1030 and c.o=o.FormId and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.SocialInsightsConfigurationId,1300,@n,0 from SocialInsightsConfiguration o,@t c where c.t=1030 and c.o=o.FormId and o.FormTypeCode=1030 and c.d=@c
goto a end
if(@y=4616)begin
if(exists(select*from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4616 and c.o=o.SdkMessageProcessingStepSecureConfigId and c.d=@c))goto b
goto a end
if(@y=92)begin
insert into @t(o,t,d,s)select o.TeamId,9,@n,0 from Team o,@t c where c.t=92 and c.o=o.TeamTemplateId and c.d=@c
goto a end
if(@y=9910)begin
insert into @t(o,t,d,s)select o.MultiEntitySearchEntityId,9911,@n,0 from MultiEntitySearchEntities o,@t c where c.t=9910 and c.o=o.MultiEntitySearchId and c.d=@c
goto a end
if(@y=1200 and @f=0)begin
insert into @t(o,t,d,s)select o.SystemUserProfileId,1202,@n,0 from SystemUserProfiles o,@t c where c.t=1200 and c.o=o.FieldSecurityProfileId and c.d=@c
insert into @t(o,t,d,s)select o.TeamProfileId,1203,@n,0 from TeamProfiles o,@t c where c.t=1200 and c.o=o.FieldSecurityProfileId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.FieldPermissionId,1201,@n,ComponentState from FieldPermissionLogicalAsIfPublished o,@t c where c.t=1200 and c.o=o.FieldSecurityProfileId and c.d=@c
 else insert into @t(o,t,d,s)select o.FieldPermissionId,1201,@n,0 from FieldPermissionAsIfPublished o,@t c where c.t=1200 and c.o=o.FieldSecurityProfileId and c.d=@c
goto a end
if(@y=1002)begin
if(exists(select*from ActivityAttachmentAsIfPublished o,@t c where c.t=1002 and c.o=o.AttachmentId and c.d=@c))goto b
goto a end
if(@y=4618)begin
if(@u=1)insert into @t(o,t,d,s)select o.SdkMessageProcessingStepId,4608,@n,ComponentState from SdkMessageProcessingStepLogicalAsIfPublished o,@t c where c.t=4618 and c.o=o.EventHandler and o.EventHandlerTypeCode=4618 and c.d=@c
 else insert into @t(o,t,d,s)select o.SdkMessageProcessingStepId,4608,@n,0 from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4618 and c.o=o.EventHandler and o.EventHandlerTypeCode=4618 and c.d=@c
goto a end
if(@y=1120)begin
if(@u=1)insert into @t(o,t,d,s)select o.RibbonRuleId,1117,@n,ComponentState from RibbonRuleLogicalAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
 else insert into @t(o,t,d,s)select o.RibbonRuleId,1117,@n,0 from RibbonRuleAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.RibbonDiffId,1130,@n,ComponentState from RibbonDiffLogicalAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
 else insert into @t(o,t,d,s)select o.RibbonDiffId,1130,@n,0 from RibbonDiffAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.RibbonContextGroupId,1115,@n,ComponentState from RibbonContextGroupLogicalAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
 else insert into @t(o,t,d,s)select o.RibbonContextGroupId,1115,@n,0 from RibbonContextGroupAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.RibbonCommandId,1116,@n,ComponentState from RibbonCommandLogicalAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
 else insert into @t(o,t,d,s)select o.RibbonCommandId,1116,@n,0 from RibbonCommandAsIfPublished o,@t c where c.t=1120 and c.o=o.RibbonCustomizationId and c.d=@c
goto a end
if(@y=1130)begin
if(@u=1)insert into @t(o,t,d,s)select o.RibbonTabToCommandMapId,1113,@n,ComponentState from RibbonTabToCommandMapLogicalAsIfPublished o,@t c where c.t=1130 and c.o=o.RibbonDiffId and c.d=@c
 else insert into @t(o,t,d,s)select o.RibbonTabToCommandMapId,1113,@n,0 from RibbonTabToCommandMapAsIfPublished o,@t c where c.t=1130 and c.o=o.RibbonDiffId and c.d=@c
goto a end
if(@y=1023)begin
insert into @t(o,t,d,s)select o.PrivilegeObjectTypeCodeId,31,@n,0 from PrivilegeObjectTypeCodes o,@t c where c.t=1023 and c.o=o.PrivilegeId and c.d=@c
insert into @t(o,t,d,s)select o.RolePrivilegeId,12,@n,0 from RolePrivilegesAsIfPublished o,@t c where c.t=1023 and c.o=o.PrivilegeId and c.d=@c
goto a end
if(@y=9603)begin
if(exists(select*from Goal o,@t c where c.t=9603 and c.o=o.MetricId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.RollupFieldId,9604,@n,0 from RollupField o,@t c where c.t=9603 and c.o=o.MetricId and c.d=@c
goto a end
if(@y=4605)begin
if(@u=1)insert into @t(o,t,d,s)select o.PluginTypeId,4602,@n,ComponentState from PluginTypeLogicalAsIfPublished o,@t c where c.t=4605 and c.o=o.PluginAssemblyId and c.d=@c
 else insert into @t(o,t,d,s)select o.PluginTypeId,4602,@n,0 from PluginTypeAsIfPublished o,@t c where c.t=4605 and c.o=o.PluginAssemblyId and c.d=@c
goto a end
if(@y=7100)begin
insert into @t(o,t,d,s)select o.SolutionComponentId,7103,@n,0 from SolutionComponent o,@t c where c.t=7100 and c.o=o.SolutionId and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.DependencyNodeId,7106,@n,0 from DependencyNode o,@t c where c.t=7100 and c.o=o.BaseSolutionId and c.d=@c
goto a end
if(@y=7106 and @f=0)begin
insert into @t(o,t,d,s)select o.DependencyId,7105,@n,0 from Dependency o,@t c where c.t=7106 and c.o=o.DependentComponentNodeId and c.d=@c and o.DependencyId not in(select o from @t where t=7105)
insert into @t(o,t,d,s)select o.DependencyId,7105,@n,0 from Dependency o,@t c where c.t=7106 and c.o=o.RequiredComponentNodeId and c.d=@c and o.DependencyId not in(select o from @t where t=7105)
goto a end
if(@y=4600)begin
if(@u=1)insert into @t(o,t,d,s)select o.AttributeMapId,4601,@n,ComponentState from AttributeMapLogicalAsIfPublished o,@t c where c.t=4600 and c.o=o.EntityMapId and c.d=@c
 else insert into @t(o,t,d,s)select o.AttributeMapId,4601,@n,0 from AttributeMapAsIfPublished o,@t c where c.t=4600 and c.o=o.EntityMapId and c.d=@c
goto a end
if(@y=4810)begin
insert into @t(o,t,d,s)select o.TimeZoneRuleId,4811,@n,0 from TimeZoneRule o,@t c where c.t=4810 and c.o=o.TimeZoneDefinitionId and c.d=@c
insert into @t(o,t,d,s)select o.TimeZoneLocalizedNameId,4812,@n,0 from TimeZoneLocalizedName o,@t c where c.t=4810 and c.o=o.TimeZoneDefinitionId and c.d=@c
goto a end
if(@y=10001 and @f=0)begin
insert into @t(o,t,d,s)select o.msdyn_wallsavedqueryId,10003,@n,0 from msdyn_wallsavedquery o,@t c where c.t=10001 and c.o=o.msdyn_postconfigurationid and c.d=@c
insert into @t(o,t,d,s)select o.msdyn_PostRuleConfigId,10002,@n,0 from msdyn_PostRuleConfig o,@t c where c.t=10001 and c.o=o.msdyn_PostConfigId and c.d=@c
goto a end
if(@y=10003 and @f=0)begin
insert into @t(o,t,d,s)select o.msdyn_wallsavedqueryusersettingsId,10004,@n,0 from msdyn_wallsavedqueryusersettings o,@t c where c.t=10003 and c.o=o.msdyn_wallsavedqueryid and c.d=@c
goto a end
if(@y=4800)begin
insert into @t(o,t,d,s)select o.WizardPageId,4802,@n,0 from WizardPage o,@t c where c.t=4800 and c.o=o.WebWizardId and c.d=@c
insert into @t(o,t,d,s)select o.WizardAccessPrivilegeId,4803,@n,0 from WizardAccessPrivilege o,@t c where c.t=4800 and c.o=o.WebWizardId and c.d=@c
goto a end
if(@y=3231)begin
insert into @t(o,t,d,s)select o.ConnectionRoleObjectTypeCodeId,3233,@n,0 from ConnectionRoleObjectTypeCode o,@t c where c.t=3231 and c.o=o.ConnectionRoleId and c.d=@c
insert into @t(o,t,d,s)select o.ConnectionRoleAssociationId,3232,@n,0 from ConnectionRoleAssociation o,@t c where c.t=3231 and c.o=o.ConnectionRoleId and c.d=@c and o.ConnectionRoleAssociationId not in(select o from @t where t=3232)
insert into @t(o,t,d,s)select o.ConnectionRoleAssociationId,3232,@n,0 from ConnectionRoleAssociation o,@t c where c.t=3231 and c.o=o.AssociatedConnectionRoleId and c.d=@c and o.ConnectionRoleAssociationId not in(select o from @t where t=3232)
goto a end
if(@y=4411 and @f=0)begin
insert into @t(o,t,d,s)select o.ImportEntityMappingId,4428,@n,0 from ImportEntityMapping o,@t c where c.t=4411 and c.o=o.ImportMapId and c.d=@c
insert into @t(o,t,d,s)select o.OwnerMappingId,4420,@n,0 from OwnerMapping o,@t c where c.t=4411 and c.o=o.ImportMapId and c.d=@c
insert into @t(o,t,d,s)select o.ColumnMappingId,4417,@n,0 from ColumnMapping o,@t c where c.t=4411 and c.o=o.ImportMapId and c.d=@c
insert into @t(o,t,d,s)select o.TransformationMappingId,4426,@n,0 from TransformationMapping o,@t c where c.t=4411 and c.o=o.ImportMapId and c.d=@c
goto a end
if(@y=4417 and @f=0)begin
insert into @t(o,t,d,s)select o.PickListMappingId,4418,@n,0 from PickListMapping o,@t c where c.t=4417 and c.o=o.ColumnMappingId and c.d=@c
insert into @t(o,t,d,s)select o.LookUpMappingId,4419,@n,0 from LookUpMapping o,@t c where c.t=4417 and c.o=o.ColumnMappingId and c.d=@c
goto a end
if(@y=1031 and @f=0)begin
insert into @t(o,t,d,s)select o.SocialInsightsConfigurationId,1300,@n,0 from SocialInsightsConfiguration o,@t c where c.t=1031 and c.o=o.FormId and o.FormTypeCode=1031 and c.d=@c
goto a end
if(@y=4410 and @f=0)begin
insert into @t(o,t,d,s)select o.ImportFileId,4412,@n,0 from ImportFile o,@t c where c.t=4410 and c.o=o.ImportId and c.d=@c
goto a end
if(@y=4009)begin
if(exists(select*from SystemUser o,@t c where c.t=4009 and c.o=o.SiteId and c.d=@c))goto b
if(exists(select*from Equipment o,@t c where c.t=4009 and c.o=o.SiteId and c.d=@c))goto b
if(exists(select*from ServiceAppointment o,@t c where c.t=4009 and c.o=o.SiteId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.InternalAddressId,1003,@n,0 from InternalAddress o,@t c where c.t=4009 and c.o=o.ParentId and o.ObjectTypeCode=4009 and c.d=@c
goto a end
if(@y=4412 and @f=0)begin
insert into @t(o,t,d,s)select o.ImportLogId,4423,@n,0 from ImportLog o,@t c where c.t=4412 and c.o=o.ImportFileId and c.d=@c
insert into @t(o,t,d,s)select o.ImportDataId,4413,@n,0 from ImportData o,@t c where c.t=4412 and c.o=o.ImportFileId and c.d=@c
goto a end
if(@y=1080)begin
if(exists(select*from ProductPriceLevel o,@t c where c.t=1080 and c.o=o.DiscountTypeId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.DiscountId,1013,@n,0 from Discount o,@t c where c.t=1080 and c.o=o.DiscountTypeId and c.d=@c
goto a end
if(@y=8181 and @f=0)begin
if(@u=1)insert into @t(o,t,d,s)select o.RoutingRuleItemId,8199,@n,ComponentState from RoutingRuleItemLogicalAsIfPublished o,@t c where c.t=8181 and c.o=o.RoutingRuleId and c.d=@c
 else insert into @t(o,t,d,s)select o.RoutingRuleItemId,8199,@n,0 from RoutingRuleItemAsIfPublished o,@t c where c.t=8181 and c.o=o.RoutingRuleId and c.d=@c
goto a end
if(@y=4426 and @f=0)begin
insert into @t(o,t,d,s)select o.TransformationParameterMappingId,4427,@n,0 from TransformationParameterMapping o,@t c where c.t=4426 and c.o=o.TransformationMappingId and c.d=@c
goto a end
if(@y=4427 and @f=0)begin
insert into @t(o,t,d,s)select o.LookUpMappingId,4419,@n,0 from LookUpMapping o,@t c where c.t=4427 and c.o=o.TransformationParameterMappingId and c.d=@c
goto a end
if(@y=9602)begin
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryActualMoneyId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryActualDecimalId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryCustomIntegerId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryInprogressDecimalId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryInprogressIntegerId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollupQueryActualIntegerId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryCustomMoneyId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryInprogressMoneyId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9602 and c.o=o.RollUpQueryCustomDecimalId and c.d=@c))goto b
goto a end
if(@y=1056)begin
if(exists(select*from Product o,@t c where c.t=1056 and c.o=o.DefaultUoMScheduleId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.UoMId,1055,@n,0 from UoM o,@t c where c.t=1056 and c.o=o.UoMScheduleId and c.d=@c
goto a end
if(@y=4606)begin
if(exists(select*from SdkMessageFilter o,@t c where c.t=4606 and c.o=o.SdkMessageId and c.d=@c))goto b
if(exists(select*from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4606 and c.o=o.SdkMessageId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.SdkMessagePairId,4613,@n,0 from SdkMessagePair o,@t c where c.t=4606 and c.o=o.SdkMessageId and c.d=@c
if(@f=0)if(exists(select*from WorkflowDependency o,@t c where c.t=4606 and c.o=o.SdkMessageId and c.d=@c))goto b
goto a end
if(@y=4613)begin
insert into @t(o,t,d,s)select o.SdkMessageRequestId,4609,@n,0 from SdkMessageRequest o,@t c where c.t=4613 and c.o=o.SdkMessagePairId and c.d=@c
goto a end
if(@y=4607)begin
if(exists(select*from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4607 and c.o=o.SdkMessageFilterId and c.d=@c))goto b
goto a end
if(@y=4601)begin
if(@u=1)insert into @t(o,t,d,s)select o.AttributeMapId,4601,@n,ComponentState from AttributeMapLogicalAsIfPublished o,@t c where c.t=4601 and c.o=o.ParentAttributeMapId and c.d=@c and o.AttributeMapId not in(select o from @t where t=4601)
 else insert into @t(o,t,d,s)select o.AttributeMapId,4601,@n,0 from AttributeMapAsIfPublished o,@t c where c.t=4601 and c.o=o.ParentAttributeMapId and c.d=@c and o.AttributeMapId not in(select o from @t where t=4601)
goto a end
if(@y=4500)begin
if(exists(select*from CustomerRelationship o,@t c where c.t=4500 and c.o=o.CustomerRoleId and c.d=@c))goto b
if(exists(select*from CustomerRelationship o,@t c where c.t=4500 and c.o=o.PartnerRoleId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.RelationshipRoleMapId,4501,@n,0 from RelationshipRoleMap o,@t c where c.t=4500 and c.o=o.RelationshipRoleId and c.d=@c
goto a end
if(@y=1022)begin
if(exists(select*from ProductPriceLevel o,@t c where c.t=1022 and c.o=o.PriceLevelId and c.d=@c))goto b
if(exists(select*from Quote o,@t c where c.t=1022 and c.o=o.PriceLevelId and c.d=@c))goto b
if(exists(select*from Invoice o,@t c where c.t=1022 and c.o=o.PriceLevelId and c.d=@c))goto b
if(exists(select*from Product o,@t c where c.t=1022 and c.o=o.PriceLevelId and c.d=@c))goto b
if(exists(select*from Account o,@t c where c.t=1022 and c.o=o.DefaultPriceLevelId and c.d=@c))goto b
if(exists(select*from SalesOrder o,@t c where c.t=1022 and c.o=o.PriceLevelId and c.d=@c))goto b
if(exists(select*from Contact o,@t c where c.t=1022 and c.o=o.DefaultPriceLevelId and c.d=@c))goto b
goto a end
if(@y=1016)begin
if(exists(select*from KbArticle o,@t c where c.t=1016 and c.o=o.KbArticleTemplateId and c.d=@c))goto b
goto a end
if(@y=129)begin
if(exists(select*from Product o,@t c where c.t=129 and c.o=o.SubjectId and c.d=@c))goto b
if(exists(select*from KbArticle o,@t c where c.t=129 and c.o=o.SubjectId and c.d=@c))goto b
if(exists(select*from SalesLiterature o,@t c where c.t=129 and c.o=o.SubjectId and c.d=@c))goto b
if(exists(select*from DocumentIndex o,@t c where c.t=129 and c.o=o.SubjectId and c.d=@c))goto b
if(exists(select*from Incident o,@t c where c.t=129 and c.o=o.SubjectId and c.d=@c))goto b
goto a end
if(@y=2010)begin
if(@u=1)insert into @t(o,t,d,s)select o.ActivityMimeAttachmentId,1001,@n,ComponentState from ActivityAttachmentLogicalAsIfPublished o,@t c where c.t=2010 and c.o=o.ObjectId and c.d=@c
 else insert into @t(o,t,d,s)select o.ActivityMimeAttachmentId,1001,@n,0 from ActivityAttachmentAsIfPublished o,@t c where c.t=2010 and c.o=o.ObjectId and c.d=@c
if(@f=0)if(exists(select*from ConvertRuleAsIfPublished o,@t c where c.t=2010 and c.o=o.ResponseTemplateId and c.d=@c))goto b
goto a end
if(@y=4602)begin
if(exists(select*from Service o,@t c where c.t=4602 and c.o=o.StrategyId and c.d=@c))goto b
if(exists(select*from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4602 and c.o=o.PluginTypeId and c.d=@c))goto b
if(@u=1)insert into @t(o,t,d,s)select o.SdkMessageProcessingStepId,4608,@n,ComponentState from SdkMessageProcessingStepLogicalAsIfPublished o,@t c where c.t=4602 and c.o=o.EventHandler and o.EventHandlerTypeCode=4602 and c.d=@c
 else insert into @t(o,t,d,s)select o.SdkMessageProcessingStepId,4608,@n,0 from SdkMessageProcessingStepAsIfPublished o,@t c where c.t=4602 and c.o=o.EventHandler and o.EventHandlerTypeCode=4602 and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.PluginTypeStatisticId,4603,@n,0 from PluginTypeStatistic o,@t c where c.t=4602 and c.o=o.PluginTypeId and c.d=@c
goto a end
if(@y=4608)begin
if(@u=1)insert into @t(o,t,d,s)select o.SdkMessageProcessingStepImageId,4615,@n,ComponentState from SdkMessageProcessingStepImageLogicalAsIfPublished o,@t c where c.t=4608 and c.o=o.SdkMessageProcessingStepId and c.d=@c
 else insert into @t(o,t,d,s)select o.SdkMessageProcessingStepImageId,4615,@n,0 from SdkMessageProcessingStepImageAsIfPublished o,@t c where c.t=4608 and c.o=o.SdkMessageProcessingStepId and c.d=@c
goto a end
if(@y=4609)begin
insert into @t(o,t,d,s)select o.SdkMessageRequestFieldId,4614,@n,0 from SdkMessageRequestField o,@t c where c.t=4609 and c.o=o.SdkMessageRequestId and c.d=@c
insert into @t(o,t,d,s)select o.SdkMessageResponseId,4610,@n,0 from SdkMessageResponse o,@t c where c.t=4609 and c.o=o.SdkMessageRequestId and c.d=@c
goto a end
if(@y=1055)begin
if(exists(select*from OpportunityProduct o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from ContractDetail o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from SalesOrderDetail o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from ProductPriceLevel o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from QuoteDetail o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from InvoiceDetail o,@t c where c.t=1055 and c.o=o.UoMId and c.d=@c))goto b
if(exists(select*from Product o,@t c where c.t=1055 and c.o=o.DefaultUoMId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.UoMId,1055,@n,0 from UoM o,@t c where c.t=1055 and c.o=o.BaseUoM and c.d=@c and o.UoMId not in(select o from @t where t=1055)
goto a end
if(@y=2011)begin
if(exists(select*from Contract o,@t c where c.t=2011 and c.o=o.ContractTemplateId and c.d=@c))goto b
goto a end
if(@y=1007)begin
insert into @t(o,t,d,s)select o.ContractId,1010,@n,0 from Contract o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.PublisherId,7101,@n,0 from Publisher o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.ConnectionId,3234,@n,0 from Connection o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.TransactionCurrencyId,9105,@n,0 from TransactionCurrency o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.InvoiceId,1090,@n,0 from Invoice o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.AccountId,1,@n,0 from Account o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.SystemUserId,8,@n,0 from SystemUser o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.TerritoryId,2013,@n,0 from Territory o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.KbArticleId,127,@n,0 from KbArticle o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.GoalId,9600,@n,0 from Goal o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.CampaignId,4400,@n,0 from Campaign o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.SalesOrderId,1088,@n,0 from SalesOrder o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.ContactId,2,@n,0 from Contact o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.ResourceId,4002,@n,0 from Resource o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.SalesLiteratureId,1038,@n,0 from SalesLiterature o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.OpportunityProductId,1083,@n,0 from OpportunityProduct o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.LeadId,4,@n,0 from Lead o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.QueueId,2020,@n,0 from Queue o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.OrganizationId,1019,@n,0 from Organization o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.MailboxId,9606,@n,0 from Mailbox o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.ProductId,1024,@n,0 from Product o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
insert into @t(o,t,d,s)select o.CompetitorId,123,@n,0 from Competitor o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.EmailServerProfileId,9605,@n,0 from EmailServerProfile o,@t c where c.t=1007 and c.o=o.EntityImageId and c.d=@c
goto a end
if(@y=123)begin
insert into @t(o,t,d,s)select o.CompetitorSalesLiteratureId,26,@n,0 from CompetitorSalesLiterature o,@t c where c.t=123 and c.o=o.CompetitorId and c.d=@c
insert into @t(o,t,d,s)select o.OpportunityCompetitorId,25,@n,0 from OpportunityCompetitors o,@t c where c.t=123 and c.o=o.CompetitorId and c.d=@c
insert into @t(o,t,d,s)select o.CompetitorAddressId,1004,@n,0 from CompetitorAddress o,@t c where c.t=123 and c.o=o.ParentId and c.d=@c
insert into @t(o,t,d,s)select o.LeadCompetitorId,24,@n,0 from LeadCompetitors o,@t c where c.t=123 and c.o=o.CompetitorId and c.d=@c
insert into @t(o,t,d,s)select o.CompetitorProductId,1006,@n,0 from CompetitorProduct o,@t c where c.t=123 and c.o=o.CompetitorId and c.d=@c
goto a end
if(@y=1024)begin
if(exists(select*from QuoteDetail o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
if(exists(select*from ContractDetail o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
if(exists(select*from SalesOrderDetail o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
if(exists(select*from Incident o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
if(exists(select*from InvoiceDetail o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
if(exists(select*from OpportunityProduct o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ProductSubstituteId,1028,@n,0 from ProductSubstitute o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c and o.ProductSubstituteId not in(select o from @t where t=1028)
insert into @t(o,t,d,s)select o.EntitlementProductId,6363,@n,0 from EntitlementProducts o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
insert into @t(o,t,d,s)select o.ProductSubstituteId,1028,@n,0 from ProductSubstitute o,@t c where c.t=1024 and c.o=o.SubstitutedProductId and c.d=@c and o.ProductSubstituteId not in(select o from @t where t=1028)
insert into @t(o,t,d,s)select o.CompetitorProductId,1006,@n,0 from CompetitorProduct o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
insert into @t(o,t,d,s)select o.ProductSalesLiteratureId,21,@n,0 from ProductSalesLiterature o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
insert into @t(o,t,d,s)select o.LeadProductId,27,@n,0 from LeadProduct o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
insert into @t(o,t,d,s)select o.EntitlementTemplateProductId,4545,@n,0 from EntitlementTemplateProducts o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
insert into @t(o,t,d,s)select o.ProductAssociationId,1025,@n,0 from ProductAssociation o,@t c where c.t=1024 and c.o=o.AssociatedProduct and c.d=@c and o.ProductAssociationId not in(select o from @t where t=1025)
insert into @t(o,t,d,s)select o.ProductAssociationId,1025,@n,0 from ProductAssociation o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c and o.ProductAssociationId not in(select o from @t where t=1025)
insert into @t(o,t,d,s)select o.ProductPriceLevelId,1026,@n,0 from ProductPriceLevel o,@t c where c.t=1024 and c.o=o.ProductId and c.d=@c
goto a end
if(@y=2020)begin
insert into @t(o,t,d,s)select o.QueueItemId,2029,@n,0 from QueueItem o,@t c where c.t=2020 and c.o=o.QueueId and c.d=@c
insert into @t(o,t,d,s)select o.QueueMembershipId,1213,@n,0 from QueueMembership o,@t c where c.t=2020 and c.o=o.QueueId and c.d=@c
insert into @t(o,t,d,s)select o.MailboxId,9606,@n,0 from Mailbox o,@t c where c.t=2020 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=2020 and c.d=@c
if(@f=0)begin
insert into @t(o,t,d,s)select o.ConvertRuleId,9300,@n,0 from ConvertRuleAsIfPublished o,@t c where c.t=2020 and c.o=o.QueueId and c.d=@c
insert into @t(o,t,d,s)select o.ConvertRuleItemId,9301,@n,0 from ConvertRuleItemAsIfPublished o,@t c where c.t=2020 and c.o=o.QueueId and c.d=@c
insert into @t(o,t,d,s)select o.RoutingRuleItemId,8199,@n,0 from RoutingRuleItemAsIfPublished o,@t c where c.t=2020 and c.o=o.RoutedQueueId and c.d=@c
end goto a end
if(@y=9606 and @f=0)begin
insert into @t(o,t,d,s)select o.TraceRegardingId,8052,@n,0 from TraceRegarding o,@t c where c.t=9606 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=9606 and c.d=@c
goto a end
if(@y=2013)begin
if(exists(select*from SystemUser o,@t c where c.t=2013 and c.o=o.TerritoryId and c.d=@c))goto b
if(exists(select*from Account o,@t c where c.t=2013 and c.o=o.TerritoryId and c.d=@c))goto b
goto a end
if(@y=7101)begin
if(exists(select*from Solution o,@t c where c.t=7101 and c.o=o.PublisherId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.PublisherAddressId,7102,@n,0 from PublisherAddress o,@t c where c.t=7101 and c.o=o.ParentId and c.d=@c
goto a end
if(@y=9750 and @f=0)begin
if(@u=1)insert into @t(o,t,d,s)select o.SLAItemId,9751,@n,ComponentState from SLAItemLogicalAsIfPublished o,@t c where c.t=9750 and c.o=o.SLAId and c.d=@c
 else insert into @t(o,t,d,s)select o.SLAItemId,9751,@n,0 from SLAItemAsIfPublished o,@t c where c.t=9750 and c.o=o.SLAId and c.d=@c
goto a end
if(@y=4703)begin
if(exists(select*from WorkflowAsIfPublished o,@t c where c.t=4703 and c.o=o.ActiveWorkflowId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ProcessStageId,4724,@n,0 from ProcessStage o,@t c where c.t=4703 and c.o=o.ProcessId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.ProcessTriggerId,4712,@n,ComponentState from ProcessTriggerLogicalAsIfPublished o,@t c where c.t=4703 and c.o=o.ProcessId and c.d=@c
 else insert into @t(o,t,d,s)select o.ProcessTriggerId,4712,@n,0 from ProcessTriggerAsIfPublished o,@t c where c.t=4703 and c.o=o.ProcessId and c.d=@c
if(@f=0)begin
insert into @t(o,t,d,s)select o.WorkflowDependencyId,4704,@n,0 from WorkflowDependency o,@t c where c.t=4703 and c.o=o.WorkflowId and c.d=@c
insert into @t(o,t,d,s)select o.ProcessSessionId,4710,@n,0 from ProcessSession o,@t c where c.t=4703 and c.o=o.ProcessId and c.d=@c
end goto a end
if(@y=4710 and @f=0)begin
insert into @t(o,t,d,s)select o.WorkflowLogId,4706,@n,0 from WorkflowLog o,@t c where c.t=4710 and c.o=o.AsyncOperationId and o.ObjectTypeCode=4710 and c.d=@c
goto a end
if(@y=127)begin
if(exists(select*from Incident o,@t c where c.t=127 and c.o=o.KbArticleId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.KbArticleCommentId,1082,@n,0 from KbArticleComment o,@t c where c.t=127 and c.o=o.KbArticleId and c.d=@c
insert into @t(o,t,d,s)select o.DocumentIndexId,126,@n,0 from DocumentIndex o,@t c where c.t=127 and c.o=o.DocumentId and c.d=@c
goto a end
if(@y=9702)begin
insert into @t(o,t,d,s)select o.EntitlementTemplateProductId,4545,@n,0 from EntitlementTemplateProducts o,@t c where c.t=9702 and c.o=o.EntitlementTemplateId and c.d=@c
insert into @t(o,t,d,s)select o.EntitlementTemplateChannelId,9703,@n,0 from EntitlementTemplateChannel o,@t c where c.t=9702 and c.o=o.EntitlementTemplateId and c.d=@c
goto a end
if(@y=9605 and @f=0)begin
insert into @t(o,t,d,s)select o.TraceRegardingId,8052,@n,0 from TraceRegarding o,@t c where c.t=9605 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=9605 and c.d=@c
goto a end
if(@y=8052 and @f=0)begin
insert into @t(o,t,d,s)select o.TraceLogId,8050,@n,0 from TraceLog o,@t c where c.t=8052 and c.o=o.TraceRegardingId and c.d=@c
goto a end
if(@y=8050 and @f=0)begin
insert into @t(o,t,d,s)select o.TraceAssociationId,8051,@n,0 from TraceAssociation o,@t c where c.t=8050 and c.o=o.TraceLogId and c.d=@c
insert into @t(o,t,d,s)select o.TraceLogId,8050,@n,0 from TraceLog o,@t c where c.t=8050 and c.o=o.ParentTraceLogId and c.d=@c and o.TraceLogId not in(select o from @t where t=8050)
goto a end
if(@y=9100)begin
if(@u=1)insert into @t(o,t,d,s)select o.ReportEntityId,9101,@n,ComponentState from ReportEntityLogicalAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
 else insert into @t(o,t,d,s)select o.ReportEntityId,9101,@n,0 from ReportEntityAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
insert into @t(o,t,d,s)select o.ReportLinkId,9104,@n,0 from ReportLink o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.ReportCategoryId,9102,@n,ComponentState from ReportCategoryLogicalAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
 else insert into @t(o,t,d,s)select o.ReportCategoryId,9102,@n,0 from ReportCategoryAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.ReportVisibilityId,9103,@n,ComponentState from ReportVisibilityLogicalAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
 else insert into @t(o,t,d,s)select o.ReportVisibilityId,9103,@n,0 from ReportVisibilityAsIfPublished o,@t c where c.t=9100 and c.o=o.ReportId and c.d=@c
goto a end
if(@y=1038)begin
insert into @t(o,t,d,s)select o.CompetitorSalesLiteratureId,26,@n,0 from CompetitorSalesLiterature o,@t c where c.t=1038 and c.o=o.SalesLiteratureId and c.d=@c
insert into @t(o,t,d,s)select o.ProductSalesLiteratureId,21,@n,0 from ProductSalesLiterature o,@t c where c.t=1038 and c.o=o.SalesLiteratureId and c.d=@c
insert into @t(o,t,d,s)select o.SalesLiteratureItemId,1070,@n,0 from SalesLiteratureItem o,@t c where c.t=1038 and c.o=o.SalesLiteratureId and c.d=@c
insert into @t(o,t,d,s)select o.CampaignActivityItemId,4404,@n,0 from CampaignActivityItem o,@t c where c.t=1038 and c.o=o.ItemId and o.ItemObjectTypeCode=1038 and c.d=@c
goto a end
if(@y=4300)begin
insert into @t(o,t,d,s)select o.ActivityId,4406,@n,0 from BulkOperation o,@t c where c.t=4300 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=4300 and c.d=@c
insert into @t(o,t,d,s)select o.CampaignActivityItemId,4404,@n,0 from CampaignActivityItem o,@t c where c.t=4300 and c.o=o.ItemId and o.ItemObjectTypeCode=4300 and c.d=@c
insert into @t(o,t,d,s)select o.ListMemberId,4301,@n,0 from ListMember o,@t c where c.t=4300 and c.o=o.ListId and c.d=@c
goto a end
if(@y=10)begin
if(exists(select*from Team o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c))goto b
if(exists(select*from UserSettings o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c))goto b
if(exists(select*from BusinessUnit o,@t c where c.t=10 and c.o=o.ParentBusinessUnitId and c.d=@c))goto b
if(exists(select*from SystemUser o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.InternalAddressId,1003,@n,0 from InternalAddress o,@t c where c.t=10 and c.o=o.ParentId and o.ObjectTypeCode=10 and c.d=@c
insert into @t(o,t,d,s)select o.ResourceId,4002,@n,0 from Resource o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.RoleId,1036,@n,0 from RoleAsIfPublished o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.CalendarId,4003,@n,0 from Calendar o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.ConstraintBasedGroupId,4007,@n,0 from ConstraintBasedGroup o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.ResourceSpecId,4006,@n,0 from ResourceSpec o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.BusinessUnitMapId,6,@n,0 from BusinessUnitMap o,@t c where c.t=10 and c.o=o.BusinessId and c.d=@c and o.BusinessUnitMapId not in(select o from @t where t=6)
insert into @t(o,t,d,s)select o.BusinessUnitMapId,6,@n,0 from BusinessUnitMap o,@t c where c.t=10 and c.o=o.SubBusinessId and c.d=@c and o.BusinessUnitMapId not in(select o from @t where t=6)
insert into @t(o,t,d,s)select o.SystemUserBusinessUnitEntityMapId,42,@n,0 from SystemUserBusinessUnitEntityMap o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.ResourceGroupId,4005,@n,0 from ResourceGroup o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
insert into @t(o,t,d,s)select o.EquipmentId,4000,@n,0 from Equipment o,@t c where c.t=10 and c.o=o.BusinessUnitId and c.d=@c
goto a end
if(@y=4006)begin
if(exists(select*from Service o,@t c where c.t=4006 and c.o=o.ResourceSpecId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ResourceId,4002,@n,0 from Resource o,@t c where c.t=4006 and c.o=o.ResourceId and o.ObjectTypeCode=4006 and c.d=@c
goto a end
if(@y=4007)begin
if(exists(select*from BusinessUnitMap o,@t c where c.t=4007 and c.o=o.SubBusinessId and c.d=@c))goto b
if(exists(select*from ResourceSpec o,@t c where c.t=4007 and c.o=o.GroupObjectId and o.ObjectTypeCode=4007 and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ResourceGroupId,4005,@n,0 from ResourceGroup o,@t c where c.t=4007 and c.o=o.ResourceGroupId and o.ObjectTypeCode=4007 and c.d=@c
goto a end
if(@y=4003)begin
if(exists(select*from SystemUser o,@t c where c.t=4003 and c.o=o.CalendarId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.EquipmentId,4000,@n,0 from Equipment o,@t c where c.t=4003 and c.o=o.CalendarId and c.d=@c
insert into @t(o,t,d,s)select o.CalendarRuleId,4004,@n,0 from CalendarRule o,@t c where c.t=4003 and c.o=o.CalendarId and c.d=@c
goto a end
if(@y=4000)begin
if(exists(select*from BusinessUnitMap o,@t c where c.t=4000 and c.o=o.SubBusinessId and c.d=@c))goto b
if(exists(select*from ActivityParty o,@t c where c.t=4000 and c.o=o.PartyId and o.PartyObjectTypeCode=4000 and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ResourceId,4002,@n,0 from Resource o,@t c where c.t=4000 and c.o=o.ResourceId and o.ObjectTypeCode=4000 and c.d=@c
goto a end
if(@y=1036)begin
if(@u=1)insert into @t(o,t,d,s)select o.RolePrivilegeId,12,@n,ComponentState from RolePrivilegesLogicalAsIfPublished o,@t c where c.t=1036 and c.o=o.RoleId and c.d=@c
 else insert into @t(o,t,d,s)select o.RolePrivilegeId,12,@n,0 from RolePrivilegesAsIfPublished o,@t c where c.t=1036 and c.o=o.RoleId and c.d=@c
if(@u=1)insert into @t(o,t,d,s)select o.RoleId,1036,@n,ComponentState from RoleLogicalAsIfPublished o,@t c where c.t=1036 and c.o=o.ParentRootRoleId and c.d=@c and o.RoleId not in(select o from @t where t=1036)
 else insert into @t(o,t,d,s)select o.RoleId,1036,@n,0 from RoleAsIfPublished o,@t c where c.t=1036 and c.o=o.ParentRootRoleId and c.d=@c and o.RoleId not in(select o from @t where t=1036)
insert into @t(o,t,d,s)select o.TeamRoleId,40,@n,0 from TeamRoles o,@t c where c.t=1036 and c.o=o.RoleId and c.d=@c
insert into @t(o,t,d,s)select o.SystemUserRoleId,15,@n,0 from SystemUserRoles o,@t c where c.t=1036 and c.o=o.RoleId and c.d=@c
goto a end
if(@y=9700)begin
insert into @t(o,t,d,s)select o.EntitlementContactId,7272,@n,0 from EntitlementContacts o,@t c where c.t=9700 and c.o=o.EntitlementId and c.d=@c
insert into @t(o,t,d,s)select o.EntitlementProductId,6363,@n,0 from EntitlementProducts o,@t c where c.t=9700 and c.o=o.EntitlementId and c.d=@c
insert into @t(o,t,d,s)select o.EntitlementChannelId,9701,@n,0 from EntitlementChannel o,@t c where c.t=9700 and c.o=o.EntitlementId and c.d=@c
goto a end
if(@y=4610)begin
insert into @t(o,t,d,s)select o.SdkMessageResponseFieldId,4611,@n,0 from SdkMessageResponseField o,@t c where c.t=4610 and c.o=o.SdkMessageResponseId and c.d=@c
goto a end
if(@y=9502 and @f=0)begin
insert into @t(o,t,d,s)select o.SharePointSiteId,9502,@n,0 from SharePointSite o,@t c where c.t=9502 and c.o=o.ParentSite and o.ParentSiteObjectTypeCode=9502 and c.d=@c and o.SharePointSiteId not in(select o from @t where t=9502)
insert into @t(o,t,d,s)select o.SharePointDocumentLocationId,9508,@n,0 from SharePointDocumentLocation o,@t c where c.t=9502 and c.o=o.ParentSiteOrLocation and o.ParentSiteOrLocationTypeCode=9502 and c.d=@c
goto a end
if(@y=4700 and @f=0)begin
insert into @t(o,t,d,s)select o.WorkflowLogId,4706,@n,0 from WorkflowLog o,@t c where c.t=4700 and c.o=o.AsyncOperationId and o.ObjectTypeCode=4700 and c.d=@c
insert into @t(o,t,d,s)select o.BulkDeleteOperationId,4424,@n,0 from BulkDeleteOperation o,@t c where c.t=4700 and c.o=o.AsyncOperationId and c.d=@c
insert into @t(o,t,d,s)select o.WorkflowWaitSubscriptionId,4702,@n,0 from WorkflowWaitSubscription o,@t c where c.t=4700 and c.o=o.AsyncOperationId and c.d=@c
insert into @t(o,t,d,s)select o.DuplicateId,4415,@n,0 from DuplicateRecord o,@t c where c.t=4700 and c.o=o.AsyncOperationId and c.d=@c
goto a end
if(@y=4424 and @f=0)begin
insert into @t(o,t,d,s)select o.BulkDeleteFailureId,4425,@n,0 from BulkDeleteFailure o,@t c where c.t=4424 and c.o=o.BulkDeleteOperationId and c.d=@c
goto a end
if(@y=4200)begin
insert into @t(o,t,d,s)select o.ActivityMimeAttachmentId,1001,@n,0 from ActivityAttachmentAsIfPublished o,@t c where c.t=4200 and c.o=o.ObjectId and c.d=@c
insert into @t(o,t,d,s)select o.EmailHashId,4023,@n,0 from EmailHash o,@t c where c.t=4200 and c.o=o.ActivityId and c.d=@c
insert into @t(o,t,d,s)select o.ActivityPartyId,135,@n,0 from ActivityParty o,@t c where c.t=4200 and c.o=o.ActivityId and c.d=@c
insert into @t(o,t,d,s)select o.RuleId,4250,@n,0 from RecurrenceRule o,@t c where c.t=4200 and c.o=o.ObjectId and o.ObjectTypeCode=4251 and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4401,@n,0 from CampaignResponse o,@t c where c.t=4200 and c.o=o.OriginatingActivityId and c.d=@c
insert into @t(o,t,d,s)select o.CampaignActivityItemId,4404,@n,0 from CampaignActivityItem o,@t c where c.t=4200 and c.o=o.CampaignActivityId and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.BulkOperationLogId,4405,@n,0 from BulkOperationLog o,@t c where c.t=4200 and c.o=o.CreatedObjectId and c.d=@c
goto a end
if(@y=7)begin
if(exists(select*from TemplateAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from ReportAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from WorkflowAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from MailMergeTemplateAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(@f=0)begin
if(exists(select*from DuplicateRule o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from SLAAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from ConvertRuleAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
if(exists(select*from RoutingRuleAsIfPublished o,@t c where c.t=7 and c.o=o.OwnerId and c.d=@c))goto b
end goto a end
if(@y=4414 and @f=0)begin
insert into @t(o,t,d,s)select o.DuplicateId,4415,@n,0 from DuplicateRecord o,@t c where c.t=4414 and c.o=o.DuplicateRuleId and c.d=@c
insert into @t(o,t,d,s)select o.DuplicateRuleConditionId,4416,@n,0 from DuplicateRuleCondition o,@t c where c.t=4414 and c.o=o.RegardingObjectId and c.d=@c
goto a end
if(@y=10042 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_reimbursementform_cgi_refundresponsibleId,10043,@n,0 from cgi_reimbursementform_cgi_refundresponsible o,@t c where c.t=10042 and c.o=o.cgi_refundresponsibleid and c.d=@c
goto a end
if(@y=10025 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_segment_accountId,10034,@n,0 from cgi_cgi_segment_account o,@t c where c.t=10025 and c.o=o.cgi_segmentid and c.d=@c
goto a end
if(@y=10021 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_reimbursementform_cgi_refundproductId,10033,@n,0 from cgi_cgi_reimbursementform_cgi_refundproduct o,@t c where c.t=10021 and c.o=o.cgi_refundproductid and c.d=@c
goto a end
if(@y=10020 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_reimbursementform_cgi_refundaccountId,10032,@n,0 from cgi_cgi_reimbursementform_cgi_refundaccount o,@t c where c.t=10020 and c.o=o.cgi_refundaccountid and c.d=@c
goto a end
if(@y=10023 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_refundtype_cgi_reimbursementformId,10031,@n,0 from cgi_cgi_refundtype_cgi_reimbursementform o,@t c where c.t=10023 and c.o=o.cgi_reimbursementformid and c.d=@c
insert into @t(o,t,d,s)select o.cgi_cgi_reimbursementform_cgi_refundaccountId,10032,@n,0 from cgi_cgi_reimbursementform_cgi_refundaccount o,@t c where c.t=10023 and c.o=o.cgi_reimbursementformid and c.d=@c
insert into @t(o,t,d,s)select o.cgi_cgi_reimbursementform_cgi_refundproductId,10033,@n,0 from cgi_cgi_reimbursementform_cgi_refundproduct o,@t c where c.t=10023 and c.o=o.cgi_reimbursementformid and c.d=@c
insert into @t(o,t,d,s)select o.cgi_reimbursementform_cgi_refundresponsibleId,10043,@n,0 from cgi_reimbursementform_cgi_refundresponsible o,@t c where c.t=10023 and c.o=o.cgi_reimbursementformid and c.d=@c
goto a end
if(@y=10022 and @f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_refundtype_cgi_reimbursementformId,10031,@n,0 from cgi_cgi_refundtype_cgi_reimbursementform o,@t c where c.t=10022 and c.o=o.cgi_refundtypeid and c.d=@c
goto a end
if(@y=9105)begin
if(exists(select*from cgi_callguidechat o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_callguidefacebook o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Letter o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Incident o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Appointment o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SalesOrder o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Product o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Connection o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Fax o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Territory o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from QuoteDetail o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from UserSettings o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from List o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Contact o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Campaign o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from BusinessUnit o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from CampaignActivity o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ServiceAppointment o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Team o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Goal o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Contract o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from DiscountType o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from PhoneCall o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from RecurringAppointmentMaster o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ContractDetail o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from KbArticle o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SalesOrderDetail o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from EntitlementTemplateChannel o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Lead o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Invoice o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from CampaignResponse o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from OpportunityClose o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ProductPriceLevel o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Account o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from OpportunityProduct o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from InvoiceDetail o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from EntitlementChannel o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Opportunity o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Discount o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from MailMergeTemplateAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Task o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Queue o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from PriceLevel o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ActivityPointer o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ReportCategoryAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SystemUser o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SalesLiterature o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from LeadAddress o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from QueueItem o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from CustomerAddress o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Competitor o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Organization o,@t c where c.t=9105 and c.o=o.BaseCurrencyId and c.d=@c))goto b
if(exists(select*from Equipment o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Quote o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Email o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(@f=0)begin
if(exists(select*from ConvertRuleItemAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SocialProfile o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from ConvertRuleAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from RoutingRuleItemAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from EntitlementTemplate o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from UserFiscalCalendar o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SharePointDocumentLocation o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SLAAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from Entitlement o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_setting o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SharePointSite o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_refund o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SocialActivity o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SharePointDocument o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_amounttranslation o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from SLAItemAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_travelcard o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from RoutingRuleAsIfPublished o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
if(exists(select*from cgi_travelcardtransaction o,@t c where c.t=9105 and c.o=o.TransactionCurrencyId and c.d=@c))goto b
end goto a end
if(@y=4001)begin
if(exists(select*from cgi_callguidechat o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from cgi_callguidefacebook o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from Appointment o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from Letter o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from QuoteClose o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from ServiceAppointment o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from OpportunityClose o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from Email o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from OrderClose o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from Task o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from RecurringAppointmentMaster o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from IncidentResolution o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from Fax o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from PhoneCall o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(exists(select*from CalendarRule o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
if(@f=0)if(exists(select*from SocialActivity o,@t c where c.t=4001 and c.o=o.ServiceId and c.d=@c))goto b
goto a end
if(@y=4400)begin
insert into @t(o,t,d,s)select o.CampaignItemId,4403,@n,0 from CampaignItem o,@t c where c.t=4400 and c.o=o.CampaignId and c.d=@c and o.CampaignItemId not in(select o from @t where t=4403)
insert into @t(o,t,d,s)select o.ActivityId,4402,@n,0 from CampaignActivity o,@t c where c.t=4400 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=4400 and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4401,@n,0 from CampaignResponse o,@t c where c.t=4400 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=4400 and c.d=@c
goto a end
if(@y=4402)begin
insert into @t(o,t,d,s)select o.ActivityId,4406,@n,0 from BulkOperation o,@t c where c.t=4402 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=4402 and c.d=@c
insert into @t(o,t,d,s)select o.CampaignActivityItemId,4404,@n,0 from CampaignActivityItem o,@t c where c.t=4402 and c.o=o.CampaignActivityId and c.d=@c
goto a end
if(@y=4406)begin
insert into @t(o,t,d,s)select o.ActivityId,4401,@n,0 from CampaignResponse o,@t c where c.t=4406 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=4406 and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.BulkOperationLogId,4405,@n,0 from BulkOperationLog o,@t c where c.t=4406 and c.o=o.BulkOperationId and c.d=@c
goto a end
if(@y=1090)begin
insert into @t(o,t,d,s)select o.ContactInvoiceId,17,@n,0 from ContactInvoices o,@t c where c.t=1090 and c.o=o.InvoiceId and c.d=@c
insert into @t(o,t,d,s)select o.InvoiceDetailId,1091,@n,0 from InvoiceDetail o,@t c where c.t=1090 and c.o=o.InvoiceId and c.d=@c
goto a end
if(@y=1088)begin
if(exists(select*from Invoice o,@t c where c.t=1088 and c.o=o.SalesOrderId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ContactOrderId,19,@n,0 from ContactOrders o,@t c where c.t=1088 and c.o=o.SalesOrderId and c.d=@c
insert into @t(o,t,d,s)select o.SalesOrderDetailId,1089,@n,0 from SalesOrderDetail o,@t c where c.t=1088 and c.o=o.SalesOrderId and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4209,@n,0 from OrderClose o,@t c where c.t=1088 and c.o=o.SalesOrderId and c.d=@c
goto a end
if(@y=1)begin
if(exists(select*from SalesOrder o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c))goto b
if(exists(select*from Contract o,@t c where c.t=1 and c.o=o.BillingCustomerId and o.BillingCustomerIdType=1 and c.d=@c))goto b
if(exists(select*from Invoice o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c))goto b
insert into @t(o,t,d,s)select o.cgi_account_contactId,10038,@n,0 from cgi_account_contact o,@t c where c.t=1 and c.o=o.accountid and c.d=@c
insert into @t(o,t,d,s)select o.CustomerRelationshipId,4502,@n,0 from CustomerRelationship o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c and o.CustomerRelationshipId not in(select o from @t where t=4502)
insert into @t(o,t,d,s)select o.CustomerOpportunityRoleId,4503,@n,0 from CustomerOpportunityRole o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.QuoteId,1084,@n,0 from Quote o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.ContactId,2,@n,0 from Contact o,@t c where c.t=1 and c.o=o.ParentCustomerId and o.ParentCustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.CustomerAddressId,1071,@n,0 from CustomerAddress o,@t c where c.t=1 and c.o=o.ParentId and o.ObjectTypeCode=1 and c.d=@c
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.AccountLeadId,16,@n,0 from AccountLeads o,@t c where c.t=1 and c.o=o.AccountId and c.d=@c
insert into @t(o,t,d,s)select o.ContractDetailId,1011,@n,0 from ContractDetail o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.ContractId,1010,@n,0 from Contract o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.LeadId,4,@n,0 from Lead o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.OpportunityId,3,@n,0 from Opportunity o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
insert into @t(o,t,d,s)select o.CustomerRelationshipId,4502,@n,0 from CustomerRelationship o,@t c where c.t=1 and c.o=o.PartnerId and o.PartnerIdType=1 and c.d=@c and o.CustomerRelationshipId not in(select o from @t where t=4502)
if(@f=0)begin
insert into @t(o,t,d,s)select o.cgi_cgi_segment_accountId,10034,@n,0 from cgi_cgi_segment_account o,@t c where c.t=1 and c.o=o.accountid and c.d=@c
insert into @t(o,t,d,s)select o.SocialProfileId,99,@n,0 from SocialProfile o,@t c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1 and c.d=@c
end goto a end
if(@y=2)begin
if(exists(select*from SalesOrder o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c))goto b
if(exists(select*from Invoice o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c))goto b
if(exists(select*from Contract o,@t c where c.t=2 and c.o=o.BillingCustomerId and o.BillingCustomerIdType=2 and c.d=@c))goto b
insert into @t(o,t,d,s)select o.cgi_account_contactId,10038,@n,0 from cgi_account_contact o,@t c where c.t=2 and c.o=o.contactid and c.d=@c
insert into @t(o,t,d,s)select o.ContractId,1010,@n,0 from Contract o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.OpportunityId,3,@n,0 from Opportunity o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.LeadId,4,@n,0 from Lead o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.CustomerOpportunityRoleId,4503,@n,0 from CustomerOpportunityRole o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.ContactLeadId,22,@n,0 from ContactLeads o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.ContactOrderId,19,@n,0 from ContactOrders o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.ContactQuoteId,18,@n,0 from ContactQuotes o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.ServiceContractContactId,20,@n,0 from ServiceContractContacts o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.QuoteId,1084,@n,0 from Quote o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.ContractDetailId,1011,@n,0 from ContractDetail o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
insert into @t(o,t,d,s)select o.EntitlementContactId,7272,@n,0 from EntitlementContacts o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.CustomerRelationshipId,4502,@n,0 from CustomerRelationship o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c and o.CustomerRelationshipId not in(select o from @t where t=4502)
insert into @t(o,t,d,s)select o.ContactInvoiceId,17,@n,0 from ContactInvoices o,@t c where c.t=2 and c.o=o.ContactId and c.d=@c
insert into @t(o,t,d,s)select o.CustomerRelationshipId,4502,@n,0 from CustomerRelationship o,@t c where c.t=2 and c.o=o.PartnerId and o.PartnerIdType=2 and c.d=@c and o.CustomerRelationshipId not in(select o from @t where t=4502)
insert into @t(o,t,d,s)select o.CustomerAddressId,1071,@n,0 from CustomerAddress o,@t c where c.t=2 and c.o=o.ParentId and o.ObjectTypeCode=2 and c.d=@c
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
if(@f=0)begin
insert into @t(o,t,d,s)select o.SubscriptionManuallyTrackedObjectId,37,@n,0 from SubscriptionManuallyTrackedObject o,@t c where c.t=2 and c.o=o.ObjectId and o.ObjectTypeCode=2 and c.d=@c
insert into @t(o,t,d,s)select o.SocialProfileId,99,@n,0 from SocialProfile o,@t c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2 and c.d=@c
end goto a end
if(@y=99 and @f=0)begin
insert into @t(o,t,d,s)select o.ActivityId,4216,@n,0 from SocialActivity o,@t c where c.t=99 and c.o=o.PostFromProfileId and c.d=@c
goto a end
if(@y=1071)begin
if(exists(select*from ContractDetail o,@t c where c.t=1071 and c.o=o.ServiceAddress and c.d=@c))goto b
if(exists(select*from Contract o,@t c where c.t=1071 and c.o=o.ServiceAddress and c.d=@c))goto b
if(exists(select*from Contract o,@t c where c.t=1071 and c.o=o.BillToAddress and c.d=@c))goto b
goto a end
if(@y=4)begin
insert into @t(o,t,d,s)select o.LeadAddressId,1017,@n,0 from LeadAddress o,@t c where c.t=4 and c.o=o.ParentId and c.d=@c
insert into @t(o,t,d,s)select o.ContactLeadId,22,@n,0 from ContactLeads o,@t c where c.t=4 and c.o=o.LeadId and c.d=@c
insert into @t(o,t,d,s)select o.LeadCompetitorId,24,@n,0 from LeadCompetitors o,@t c where c.t=4 and c.o=o.LeadId and c.d=@c
insert into @t(o,t,d,s)select o.AccountLeadId,16,@n,0 from AccountLeads o,@t c where c.t=4 and c.o=o.LeadId and c.d=@c
insert into @t(o,t,d,s)select o.LeadProductId,27,@n,0 from LeadProduct o,@t c where c.t=4 and c.o=o.LeadId and c.d=@c
goto a end
if(@y=3)begin
if(exists(select*from Invoice o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c))goto b
if(exists(select*from SalesOrder o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.OpportunityProductId,1083,@n,0 from OpportunityProduct o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c
insert into @t(o,t,d,s)select o.TeamId,9,@n,0 from Team o,@t c where c.t=3 and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=3 and c.d=@c
insert into @t(o,t,d,s)select o.CustomerOpportunityRoleId,4503,@n,0 from CustomerOpportunityRole o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c
insert into @t(o,t,d,s)select o.OpportunityCompetitorId,25,@n,0 from OpportunityCompetitors o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4208,@n,0 from OpportunityClose o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c
insert into @t(o,t,d,s)select o.QuoteId,1084,@n,0 from Quote o,@t c where c.t=3 and c.o=o.OpportunityId and c.d=@c
goto a end
if(@y=1084)begin
if(exists(select*from SalesOrder o,@t c where c.t=1084 and c.o=o.QuoteId and c.d=@c))goto b
insert into @t(o,t,d,s)select o.QuoteDetailId,1085,@n,0 from QuoteDetail o,@t c where c.t=1084 and c.o=o.QuoteId and c.d=@c
insert into @t(o,t,d,s)select o.ContactQuoteId,18,@n,0 from ContactQuotes o,@t c where c.t=1084 and c.o=o.QuoteId and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4211,@n,0 from QuoteClose o,@t c where c.t=1084 and c.o=o.QuoteId and c.d=@c
goto a end
if(@y=9508 and @f=0)begin
insert into @t(o,t,d,s)select o.SharePointDocumentLocationId,9508,@n,0 from SharePointDocumentLocation o,@t c where c.t=9508 and c.o=o.ParentSiteOrLocation and o.ParentSiteOrLocationTypeCode=9508 and c.d=@c and o.SharePointDocumentLocationId not in(select o from @t where t=9508)
goto a end
if(@y=9)begin
if(exists(select*from ResourceSpec o,@t c where c.t=9 and c.o=o.GroupObjectId and o.ObjectTypeCode=9 and c.d=@c))goto b
insert into @t(o,t,d,s)select o.PrincipalObjectAttributeAccessId,44,@n,0 from PrincipalObjectAttributeAccess o,@t c where c.t=9 and c.o=o.PrincipalId and o.PrincipalIdType=9 and c.d=@c and o.PrincipalObjectAttributeAccessId not in(select o from @t where t=44)
insert into @t(o,t,d,s)select o.TeamMembershipId,23,@n,0 from TeamMembership o,@t c where c.t=9 and c.o=o.TeamId and c.d=@c
insert into @t(o,t,d,s)select o.TeamRoleId,40,@n,0 from TeamRoles o,@t c where c.t=9 and c.o=o.TeamId and c.d=@c
insert into @t(o,t,d,s)select o.ResourceGroupId,4005,@n,0 from ResourceGroup o,@t c where c.t=9 and c.o=o.ResourceGroupId and o.ObjectTypeCode=9 and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.TeamProfileId,1203,@n,0 from TeamProfiles o,@t c where c.t=9 and c.o=o.TeamId and c.d=@c
goto a end
if(@y=1010)begin
if(exists(select*from Contract o,@t c where c.t=1010 and c.o=o.OriginatingContract and c.d=@c))goto b
insert into @t(o,t,d,s)select o.ContractDetailId,1011,@n,0 from ContractDetail o,@t c where c.t=1010 and c.o=o.ContractId and c.d=@c
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=1010 and c.o=o.ContractId and c.d=@c
insert into @t(o,t,d,s)select o.ServiceContractContactId,20,@n,0 from ServiceContractContacts o,@t c where c.t=1010 and c.o=o.ContractId and c.d=@c
goto a end
if(@y=1011)begin
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=1011 and c.o=o.ContractDetailId and c.d=@c
goto a end
if(@y=112)begin
insert into @t(o,t,d,s)select o.ActivityId,4206,@n,0 from IncidentResolution o,@t c where c.t=112 and c.o=o.IncidentId and c.d=@c
insert into @t(o,t,d,s)select o.IncidentId,112,@n,0 from Incident o,@t c where c.t=112 and c.o=o.ParentCaseId and c.d=@c and o.IncidentId not in(select o from @t where t=112)
if(@f=0)begin
insert into @t(o,t,d,s)select o.cgi_filelinkId,10055,@n,0 from cgi_filelink o,@t c where c.t=112 and c.o=o.cgi_IncidentId and c.d=@c
insert into @t(o,t,d,s)select o.cgi_passtravelinformationId,10037,@n,0 from cgi_passtravelinformation o,@t c where c.t=112 and c.o=o.cgi_IncidentId and c.d=@c
end goto a end
if(@y=4202)begin
insert into @t(o,t,d,s)select o.ActivityMimeAttachmentId,1001,@n,0 from ActivityAttachmentAsIfPublished o,@t c where c.t=4202 and c.o=o.ObjectId and c.d=@c
insert into @t(o,t,d,s)select o.EmailHashId,4023,@n,0 from EmailHash o,@t c where c.t=4202 and c.o=o.ActivityId and c.d=@c
goto a end
if(@y=4212 and @f=0)begin
insert into @t(o,t,d,s)select o.SubscriptionManuallyTrackedObjectId,37,@n,0 from SubscriptionManuallyTrackedObject o,@t c where c.t=4212 and c.o=o.ObjectId and o.ObjectTypeCode=4212 and c.d=@c
goto a end
if(@y=4251)begin
insert into @t(o,t,d,s)select o.ActivityId,4201,@n,0 from Appointment o,@t c where c.t=4251 and c.o=o.SeriesId and c.d=@c
goto a end
if(@y=8002 and @f=0)begin
insert into @t(o,t,d,s)select o.PostId,8000,@n,0 from Post o,@t c where c.t=8002 and c.o=o.PostRegardingId and c.d=@c
goto a end
if(@y=8000 and @f=0)begin
insert into @t(o,t,d,s)select o.PostRoleId,8001,@n,0 from PostRole o,@t c where c.t=8000 and c.o=o.PostId and c.d=@c
insert into @t(o,t,d,s)select o.PostCommentId,8005,@n,0 from PostComment o,@t c where c.t=8000 and c.o=o.PostId and c.d=@c
insert into @t(o,t,d,s)select o.PostLikeId,8006,@n,0 from PostLike o,@t c where c.t=8000 and c.o=o.PostId and c.d=@c
goto a end
if(@y=9300 and @f=0)begin
if(@u=1)insert into @t(o,t,d,s)select o.ConvertRuleItemId,9301,@n,ComponentState from ConvertRuleItemLogicalAsIfPublished o,@t c where c.t=9300 and c.o=o.ConvertRuleId and c.d=@c
 else insert into @t(o,t,d,s)select o.ConvertRuleItemId,9301,@n,0 from ConvertRuleItemAsIfPublished o,@t c where c.t=9300 and c.o=o.ConvertRuleId and c.d=@c
goto a end
a:
if(@y in(1,2,3,4,5,9,10,112,123,127,129,132,1001,1010,1011,1013,1016,1022,1023,1024,1026,1030,1031,1036,1038,1039,1055,1056,1070,1071,1080,1082,1083,1084,1085,1088,1089,1090,1091,2000,2001,2002,2003,2004,2010,2011,2013,2020,2029,4000,4001,4002,4003,4005,4006,4007,4009,4102,4200,4201,4202,4204,4206,4207,4208,4209,4210,4211,4212,4214,4216,4230,4251,4300,4400,4401,4402,4405,4406,4410,4411,4412,4413,4423,4500,4501,4502,4503,4600,4601,4705,8000,8181,8199,9605,9700,9701,9702,9750,10000,10001,10002,10003,10004,10005,10006,10007,10008,10009,10010,10011,10012,10013,10014,10015,10016,10017,10018,10019,10020,10021,10022,10023,10024,10025,10026,10027,10028,10029,10030,10035,10037,10039,10040,10041,10042,10044,10046,10047,10055,10056,10057,10058,10059,10063)and @f=0)
insert into @t(o,t,d,s)select o.BulkDeleteFailureId,4425,@n,0 from BulkDeleteFailure o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4,112,123,127,1010,1011,1024,1084,1088,1090,4000,4001,4003,4006,4201,4202,4204,4206,4207,4208,4209,4210,4211,4212,4214,4215,4216,4251,4300,4400,4401,4402,4414,4703,8181,8199,9300,9507,9600,9605,9606,9700,9701,9702,9750,10000,10008,10009,10019,10025,10027,10058,10059,10063))
insert into @t(o,t,d,s)select o.AnnotationId,5,@n,0 from Annotation o,@t c where c.t=@y and c.o=o.ObjectId and o.ObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4,9,10,99,112,123,127,1010,1011,1022,1024,1038,1071,1083,1084,1085,1088,1089,1090,1091,2013,2020,2029,3234,4000,4201,4202,4204,4207,4210,4212,4214,4216,4251,4300,4400,4401,4402,9102,9106,9502,9507,9508,9600,9700,9702,10000,10001,10002,10003,10004,10005,10006,10007,10008,10009,10010,10011,10012,10013,10014,10015,10016,10017,10018,10019,10020,10021,10022,10023,10024,10025,10026,10027,10028,10029,10030,10035,10037,10039,10040,10041,10042,10044,10046,10047,10055,10056,10057,10058,10059,10063))
insert into @t(o,t,d,s)select o.PrincipalObjectAttributeAccessId,44,@n,0 from PrincipalObjectAttributeAccess o,@t c where c.t=@y and c.o=o.ObjectId and o.ObjectTypeCode=@y and c.d=@c and o.PrincipalObjectAttributeAccessId not in(select o from @t where t=44)
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,4402,4406,10000,10014,10025,10027,10058,10059,10063))begin
insert into @t(o,t,d,s)select o.ActivityId,10008,@n,0 from cgi_callguidechat o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,10009,@n,0 from cgi_callguidefacebook o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
end
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,4402,4406,9700,9702,10000,10014,10025,10027,10058,10059,10063))begin
insert into @t(o,t,d,s)select o.ActivityId,4204,@n,0 from Fax o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4251,@n,0 from RecurringAppointmentMaster o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4201,@n,0 from Appointment o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4207,@n,0 from Letter o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.ActivityId,4210,@n,0 from PhoneCall o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
end
if(@y in(1,2,4))begin
insert into @t(o,t,d,s)select o.ListMemberId,4301,@n,0 from ListMember o,@t c where c.t=@y and c.o=o.EntityId and o.EntityType=@y and c.d=@c
if(@f=0)insert into @t(o,t,d,s)select o.BulkOperationLogId,4405,@n,0 from BulkOperationLog o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectIdTypeCode=@y and c.d=@c and o.BulkOperationLogId not in(select o from @t where t=4405)
end
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,4402,4406,4700,9700,9702,10000,10014,10025,10027,10058,10059,10063))
insert into @t(o,t,d,s)select o.ActivityId,4202,@n,0 from Email o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4,9,99,112,123,1010,1084,1088,1090,4000,4005,4007,4200,4201,4202,4204,4207,4210,4212,4214,4216,4251,4300,4400,4402,4710,9600,9700,10000,10008,10009,10058,10059,10063))begin
insert into @t(o,t,d,s)select o.ConnectionId,3234,@n,0 from Connection o,@t c where c.t=@y and c.o=o.Record1Id and o.Record1IdObjectTypeCode=@y and c.d=@c and o.ConnectionId not in(select o from @t where t=3234)
insert into @t(o,t,d,s)select o.ConnectionId,3234,@n,0 from Connection o,@t c where c.t=@y and c.o=o.Record2Id and o.Record2IdObjectTypeCode=@y and c.d=@c and o.ConnectionId not in(select o from @t where t=3234)
end
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,4402,9700,9702,10000,10014,10025,10027,10058,10059,10063))
insert into @t(o,t,d,s)select o.ActivityId,4212,@n,0 from Task o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4,9,99,112,123,127,1010,1084,2013,2020,4000,4001,4005,4201,4202,4204,4207,4210,4212,4216,4251,4300,4400,4401,9502,9508,9605,9700,9701,9702,10000,10006,10007,10008,10009,10010,10011,10012,10013,10014,10015,10016,10017,10018,10019,10020,10021,10022,10023,10024,10025,10026,10027,10028,10029,10030,10035,10037,10039,10040,10041,10042,10044,10047,10058,10059,10063)and @f=0)begin
insert into @t(o,t,d,s)select o.DuplicateId,4415,@n,0 from DuplicateRecord o,@t c where c.t=@y and c.o=o.BaseRecordId and o.BaseRecordIdTypeCode=@y and c.d=@c and o.DuplicateId not in(select o from @t where t=4415)
insert into @t(o,t,d,s)select o.DuplicateId,4415,@n,0 from DuplicateRecord o,@t c where c.t=@y and c.o=o.DuplicateRecordId and o.DuplicateRecordIdTypeCode=@y and c.d=@c and o.DuplicateId not in(select o from @t where t=4415)
end
if(@y in(1,2,3,4,112,123,2020,4201,4210,4212,4251,4710)and @f=0)begin
insert into @t(o,t,d,s)select o.PostFollowId,8003,@n,0 from PostFollow o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.PostRoleId,8001,@n,0 from PostRole o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
end
if(@y in(1,2,3,4,9,112,123,2020,4201,4210,4212,4251,4710)and @f=0)
insert into @t(o,t,d,s)select o.PostRegardingId,8002,@n,0 from PostRegarding o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4406,4700,9700,9702,10000,10014,10025,10027,10058,10059,10063)and @f=0)
insert into @t(o,t,d,s)select o.ActivityId,4216,@n,0 from SocialActivity o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1,2,3,4)and @f=0)
insert into @t(o,t,d,s)select o.BulkOperationLogId,4405,@n,0 from BulkOperationLog o,@t c where c.t=@y and c.o=o.CreatedObjectId and o.CreatedObjectIdTypeCode=@y and c.d=@c and o.BulkOperationLogId not in(select o from @t where t=4405)
if(@y in(1,3,4,127,1024,1038,1084)and @f=0)begin
insert into @t(o,t,d,s)select o.SharePointDocumentLocationId,9508,@n,0 from SharePointDocumentLocation o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
insert into @t(o,t,d,s)select o.SharePointDocumentId,9507,@n,0 from SharePointDocument o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
end
if(@y in(112,127,4200,4201,4202,4204,4207,4210,4212,4214,4216,4251,4401,4402,4406,10008,10009))
insert into @t(o,t,d,s)select o.QueueItemId,2029,@n,0 from QueueItem o,@t c where c.t=@y and c.o=o.ObjectId and o.ObjectIdTypeCode=@y and c.d=@c
if(@y in(4201,4202,4204,4207,4210,4251,10008,10009))
insert into @t(o,t,d,s)select o.ActivityId,4401,@n,0 from CampaignResponse o,@t c where c.t=@y and c.o=o.OriginatingActivityId and o.OriginatingActivityIdTypeCode=@y and c.d=@c
if(@y in(9702,10000,10014,10025,10027,10058,10059,10063))
insert into @t(o,t,d,s)select o.ActivityId,4214,@n,0 from ServiceAppointment o,@t c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y and c.d=@c
if(@y in(1024,1038,4300,4400))
insert into @t(o,t,d,s)select o.CampaignItemId,4403,@n,0 from CampaignItem o,@t c where c.t=@y and c.o=o.EntityId and o.EntityType=@y and c.d=@c and o.CampaignItemId not in(select o from @t where t=4403)
FETCH NEXT FROM yc INTO @y
end
CLOSE yc
DEALLOCATE yc
select @l=COUNT(*)FROM @t where d=@n
end
insert into @t(o,t,d,s)select o.UserEntityInstanceDataId,2501,@n+1,0 from UserEntityInstanceData o,@t c where c.o=o.ObjectId and o.ObjectTypeCode=c.t
return

b:
delete @t
insert into @t(o,t,d,s)values(N'00000000-0000-0000-0000-000000000000',-1,-1,0)
return
end
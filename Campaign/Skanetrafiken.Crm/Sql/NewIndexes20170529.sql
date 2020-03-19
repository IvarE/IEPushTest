CREATE INDEX [IX_ED_cdi_emaileventBase_type_emailsendid] ON [DKCRM_MSCRM].[dbo].[cdi_emaileventBase] ([cdi_type], [cdi_emailsendid])
CREATE INDEX [IX_ED_cdi_sentemailBase_emailsendid_openscount] ON [DKCRM_MSCRM].[dbo].[cdi_sentemailBase] ([cdi_emailsendid],[cdi_openscount])
CREATE INDEX [IX_ED_cdi_sentemailBase_emailsendid_deliveriescount] ON [DKCRM_MSCRM].[dbo].[cdi_sentemailBase] ([cdi_emailsendid],[cdi_deliveriescount])
CREATE INDEX [IX_ED_ProcessSessionBase_RegObjectId_RegObjectTypeCode] ON [DKCRM_MSCRM].[dbo].[ProcessSessionBase] ([RegardingObjectId], [RegardingObjectTypeCode])
CREATE INDEX [IX_ED_cdi_emaileventBase_emailsendid_type] ON [DKCRM_MSCRM].[dbo].[cdi_emaileventBase] ([cdi_emailsendid],[cdi_type])
CREATE INDEX [IX_ED_WorkflowDependencyBase_WorkflowId] ON [DKCRM_MSCRM].[dbo].[WorkflowDependencyBase] ([WorkflowId])
CREATE INDEX [IX_ED_WorkflowDependencyBase_SdkMessageId] ON [DKCRM_MSCRM].[dbo].[WorkflowDependencyBase] ([SdkMessageId]) 
	INCLUDE ([WorkflowId])

create index ed_contactid_include_fullname on contactbase (contactid) include(fullname, YomiFullName)

-- Index i andra databaser
CREATE INDEX [IX_ED_OrderGroupId] ON [skanetrafiken_commerce_Prod].[dbo].[OrderGroupNote] ([OrderGroupId])
CREATE INDEX [IX_ED_PromotionUsage_PromotionId] ON [skanetrafiken_commerce_Prod].[dbo].[PromotionUsage] ([PromotionId]) 
	INCLUDE ([PromotionUsageId], [OrderGroupId], [CustomerId], [Version], [Status], [LastUpdated])

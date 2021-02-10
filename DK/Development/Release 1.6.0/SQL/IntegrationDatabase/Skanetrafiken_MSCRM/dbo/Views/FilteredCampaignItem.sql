

--
-- report view for campaignitem
--
create view dbo.[FilteredCampaignItem] (
    [campaignid],
    [campaignitemid],
    [entityid],
    [entitytype],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [versionnumber]
) with view_metadata as
select
    [CampaignItem].[CampaignId],
    [CampaignItem].[CampaignItemId],
    [CampaignItem].[EntityId],
    [CampaignItem].[EntityType],
    [CampaignItem].[OwnerId],
    [CampaignItem].[OwnerIdType],
    [CampaignItem].[OwningBusinessUnit],
    [CampaignItem].[OwningUser],
    [CampaignItem].[VersionNumber]
from CampaignItem

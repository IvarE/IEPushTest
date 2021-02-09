


--
-- base view for st_discountcode
--
create view dbo.[st_discountcode]
 (
    -- logical attributes
    [CreatedByName],
    [CreatedByYomiName],
    [CreatedOnBehalfByName],
    [CreatedOnBehalfByYomiName],
    [ModifiedByName],
    [ModifiedByYomiName],
    [ModifiedOnBehalfByName],
    [ModifiedOnBehalfByYomiName],
    [st_LeadName],
    [st_TravelCardName],
    [st_ContactYomiName],
    [st_CampaignName],
    [st_ContactName],
    [st_LeadYomiName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [st_discountcodeId],
    [CreatedOn],
    [CreatedBy],
    [ModifiedOn],
    [ModifiedBy],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [OwningBusinessUnit],
    [statecode],
    [statuscode],
    [VersionNumber],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [TimeZoneRuleVersionNumber],
    [UTCConversionTimeZoneCode],
    [st_name],
    [st_Contact],
    [st_Lead],
    [st_Campaign],
    [st_TravelCard],
    [st_DiscountCode],
    [st_ValidFrom],
    [st_ValidTo],
    [st_CardNo]
) with view_metadata as
select
    -- logical attributes
    [lk_st_discountcode_createdby].[FullName],
    [lk_st_discountcode_createdby].[YomiFullName],
    [lk_st_discountcode_createdonbehalfby].[FullName],
    [lk_st_discountcode_createdonbehalfby].[YomiFullName],
    [lk_st_discountcode_modifiedby].[FullName],
    [lk_st_discountcode_modifiedby].[YomiFullName],
    [lk_st_discountcode_modifiedonbehalfby].[FullName],
    [lk_st_discountcode_modifiedonbehalfby].[YomiFullName],
    [st_lead_st_discountcode_Lead].[FullName],
    [st_cgi_travelcard_st_discountcode_TravelCard].[cgi_travelcardnumber],
    [st_contact_st_discountcode_Contact].[YomiFullName],
    [st_campaign_st_discountcode_Campaign].[Name],
    [st_contact_st_discountcode_Contact].[FullName],
    [st_lead_st_discountcode_Lead].[YomiFullName],

    -- ownership entries
    OwnerId = [st_discountcodeBase].OwnerId,
    OwnerName = XXowner.Name,
    OwnerYomiName =  XXowner.YomiName,
    OwnerDsc = 0, -- DSC is removed, stub it to 0
    OwnerIdType = XXowner.OwnerIdType,
    OwningUser = case 
 		when XXowner.OwnerIdType= 8 then XXowner.OwnerId
		else null
		end,
    OwningTeam = case 
 		when XXowner.OwnerIdType= 9 then XXowner.OwnerId
		else null
		end,

    -- physical attribute
    [st_discountcodeBase].[st_discountcodeId],
    [st_discountcodeBase].[CreatedOn],
    [st_discountcodeBase].[CreatedBy],
    [st_discountcodeBase].[ModifiedOn],
    [st_discountcodeBase].[ModifiedBy],
    [st_discountcodeBase].[CreatedOnBehalfBy],
    [st_discountcodeBase].[ModifiedOnBehalfBy],
    [st_discountcodeBase].[OwningBusinessUnit],
    [st_discountcodeBase].[statecode],
    [st_discountcodeBase].[statuscode],
    [st_discountcodeBase].[VersionNumber],
    [st_discountcodeBase].[ImportSequenceNumber],
    [st_discountcodeBase].[OverriddenCreatedOn],
    [st_discountcodeBase].[TimeZoneRuleVersionNumber],
    [st_discountcodeBase].[UTCConversionTimeZoneCode],
    [st_discountcodeBase].[st_name],
    [st_discountcodeBase].[st_Contact],
    [st_discountcodeBase].[st_Lead],
    [st_discountcodeBase].[st_Campaign],
    [st_discountcodeBase].[st_TravelCard],
    [st_discountcodeBase].[st_DiscountCode],
    [st_discountcodeBase].[st_ValidFrom],
    [st_discountcodeBase].[st_ValidTo],
    [st_discountcodeBase].[st_CardNo]
from [st_discountcodeBase] 
    left join [SystemUserBase] [lk_st_discountcode_createdby] with(nolock) on ([st_discountcodeBase].[CreatedBy] = [lk_st_discountcode_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_st_discountcode_createdonbehalfby] with(nolock) on ([st_discountcodeBase].[CreatedOnBehalfBy] = [lk_st_discountcode_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_st_discountcode_modifiedby] with(nolock) on ([st_discountcodeBase].[ModifiedBy] = [lk_st_discountcode_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_st_discountcode_modifiedonbehalfby] with(nolock) on ([st_discountcodeBase].[ModifiedOnBehalfBy] = [lk_st_discountcode_modifiedonbehalfby].[SystemUserId])
    left join [CampaignBase] [st_campaign_st_discountcode_Campaign] on ([st_discountcodeBase].[st_Campaign] = [st_campaign_st_discountcode_Campaign].[CampaignId])
    left join [cgi_travelcardBase] [st_cgi_travelcard_st_discountcode_TravelCard] on ([st_discountcodeBase].[st_TravelCard] = [st_cgi_travelcard_st_discountcode_TravelCard].[cgi_travelcardId])
    left join [ContactBase] [st_contact_st_discountcode_Contact] on ([st_discountcodeBase].[st_Contact] = [st_contact_st_discountcode_Contact].[ContactId])
    left join [LeadBase] [st_lead_st_discountcode_Lead] on ([st_discountcodeBase].[st_Lead] = [st_lead_st_discountcode_Lead].[LeadId])
    left join OwnerBase XXowner with(nolock) on ([st_discountcodeBase].OwnerId = XXowner.OwnerId)

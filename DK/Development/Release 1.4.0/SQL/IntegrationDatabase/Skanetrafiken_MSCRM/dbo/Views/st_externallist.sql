


--
-- base view for st_externallist
--
create view dbo.[st_externallist]
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
    [st_MarketingListName],
    [st_ContactLookupYomiName],
    [st_ContactLookupName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [st_externallistId],
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
    [st_ContactLookup],
    [st_MarketingList]
) with view_metadata as
select
    -- logical attributes
    [lk_st_externallist_createdby].[FullName],
    [lk_st_externallist_createdby].[YomiFullName],
    [lk_st_externallist_createdonbehalfby].[FullName],
    [lk_st_externallist_createdonbehalfby].[YomiFullName],
    [lk_st_externallist_modifiedby].[FullName],
    [lk_st_externallist_modifiedby].[YomiFullName],
    [lk_st_externallist_modifiedonbehalfby].[FullName],
    [lk_st_externallist_modifiedonbehalfby].[YomiFullName],
    [st_list_st_externallist_MarketingList].[ListName],
    [st_contact_st_externallist_ContactLookup].[YomiFullName],
    [st_contact_st_externallist_ContactLookup].[FullName],

    -- ownership entries
    OwnerId = [st_externallistBase].OwnerId,
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
    [st_externallistBase].[st_externallistId],
    [st_externallistBase].[CreatedOn],
    [st_externallistBase].[CreatedBy],
    [st_externallistBase].[ModifiedOn],
    [st_externallistBase].[ModifiedBy],
    [st_externallistBase].[CreatedOnBehalfBy],
    [st_externallistBase].[ModifiedOnBehalfBy],
    [st_externallistBase].[OwningBusinessUnit],
    [st_externallistBase].[statecode],
    [st_externallistBase].[statuscode],
    [st_externallistBase].[VersionNumber],
    [st_externallistBase].[ImportSequenceNumber],
    [st_externallistBase].[OverriddenCreatedOn],
    [st_externallistBase].[TimeZoneRuleVersionNumber],
    [st_externallistBase].[UTCConversionTimeZoneCode],
    [st_externallistBase].[st_name],
    [st_externallistBase].[st_ContactLookup],
    [st_externallistBase].[st_MarketingList]
from [st_externallistBase] 
    left join [SystemUserBase] [lk_st_externallist_createdby] with(nolock) on ([st_externallistBase].[CreatedBy] = [lk_st_externallist_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_st_externallist_createdonbehalfby] with(nolock) on ([st_externallistBase].[CreatedOnBehalfBy] = [lk_st_externallist_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_st_externallist_modifiedby] with(nolock) on ([st_externallistBase].[ModifiedBy] = [lk_st_externallist_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_st_externallist_modifiedonbehalfby] with(nolock) on ([st_externallistBase].[ModifiedOnBehalfBy] = [lk_st_externallist_modifiedonbehalfby].[SystemUserId])
    left join [ContactBase] [st_contact_st_externallist_ContactLookup] on ([st_externallistBase].[st_ContactLookup] = [st_contact_st_externallist_ContactLookup].[ContactId])
    left join [ListBase] [st_list_st_externallist_MarketingList] on ([st_externallistBase].[st_MarketingList] = [st_list_st_externallist_MarketingList].[ListId])
    left join OwnerBase XXowner with(nolock) on ([st_externallistBase].OwnerId = XXowner.OwnerId)




--
-- base view for st_external_list
--
create view dbo.[st_external_list]
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
    [st_ContactName],
    [st_MarketingListIdName],
    [st_ContactYomiName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [st_external_listId],
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
    [st_MarketingListId],
    [st_Contact]
) with view_metadata as
select
    -- logical attributes
    [lk_st_external_list_createdby].[FullName],
    [lk_st_external_list_createdby].[YomiFullName],
    [lk_st_external_list_createdonbehalfby].[FullName],
    [lk_st_external_list_createdonbehalfby].[YomiFullName],
    [lk_st_external_list_modifiedby].[FullName],
    [lk_st_external_list_modifiedby].[YomiFullName],
    [lk_st_external_list_modifiedonbehalfby].[FullName],
    [lk_st_external_list_modifiedonbehalfby].[YomiFullName],
    [st_contact_st_external_list_Contact].[FullName],
    [st_list_st_external_list].[ListName],
    [st_contact_st_external_list_Contact].[YomiFullName],

    -- ownership entries
    OwnerId = [st_external_listBase].OwnerId,
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
    [st_external_listBase].[st_external_listId],
    [st_external_listBase].[CreatedOn],
    [st_external_listBase].[CreatedBy],
    [st_external_listBase].[ModifiedOn],
    [st_external_listBase].[ModifiedBy],
    [st_external_listBase].[CreatedOnBehalfBy],
    [st_external_listBase].[ModifiedOnBehalfBy],
    [st_external_listBase].[OwningBusinessUnit],
    [st_external_listBase].[statecode],
    [st_external_listBase].[statuscode],
    [st_external_listBase].[VersionNumber],
    [st_external_listBase].[ImportSequenceNumber],
    [st_external_listBase].[OverriddenCreatedOn],
    [st_external_listBase].[TimeZoneRuleVersionNumber],
    [st_external_listBase].[UTCConversionTimeZoneCode],
    [st_external_listBase].[st_name],
    [st_external_listBase].[st_MarketingListId],
    [st_external_listBase].[st_Contact]
from [st_external_listBase] 
    left join [SystemUserBase] [lk_st_external_list_createdby] with(nolock) on ([st_external_listBase].[CreatedBy] = [lk_st_external_list_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_st_external_list_createdonbehalfby] with(nolock) on ([st_external_listBase].[CreatedOnBehalfBy] = [lk_st_external_list_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_st_external_list_modifiedby] with(nolock) on ([st_external_listBase].[ModifiedBy] = [lk_st_external_list_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_st_external_list_modifiedonbehalfby] with(nolock) on ([st_external_listBase].[ModifiedOnBehalfBy] = [lk_st_external_list_modifiedonbehalfby].[SystemUserId])
    left join [ContactBase] [st_contact_st_external_list_Contact] on ([st_external_listBase].[st_Contact] = [st_contact_st_external_list_Contact].[ContactId])
    left join [ListBase] [st_list_st_external_list] on ([st_external_listBase].[st_MarketingListId] = [st_list_st_external_list].[ListId])
    left join OwnerBase XXowner with(nolock) on ([st_external_listBase].OwnerId = XXowner.OwnerId)

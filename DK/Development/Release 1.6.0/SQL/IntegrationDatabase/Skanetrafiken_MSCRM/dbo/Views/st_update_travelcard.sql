


--
-- base view for st_update_travelcard
--
create view dbo.[st_update_travelcard]
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
    [st_CardNumName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [st_update_travelcardId],
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
    [st_CardNumber],
    [st_ReplyDate],
    [st_CardNum],
    [st_OP_Offer],
    [st_OP_Offer_Date],
    [st_OP_Offer_Code]
) with view_metadata as
select
    -- logical attributes
    [lk_st_update_travelcard_createdby].[FullName],
    [lk_st_update_travelcard_createdby].[YomiFullName],
    [lk_st_update_travelcard_createdonbehalfby].[FullName],
    [lk_st_update_travelcard_createdonbehalfby].[YomiFullName],
    [lk_st_update_travelcard_modifiedby].[FullName],
    [lk_st_update_travelcard_modifiedby].[YomiFullName],
    [lk_st_update_travelcard_modifiedonbehalfby].[FullName],
    [lk_st_update_travelcard_modifiedonbehalfby].[YomiFullName],
    [st_cgi_travelcard_st_update_travelcard_CardNum].[cgi_travelcardnumber],

    -- ownership entries
    OwnerId = [st_update_travelcardBase].OwnerId,
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
    [st_update_travelcardBase].[st_update_travelcardId],
    [st_update_travelcardBase].[CreatedOn],
    [st_update_travelcardBase].[CreatedBy],
    [st_update_travelcardBase].[ModifiedOn],
    [st_update_travelcardBase].[ModifiedBy],
    [st_update_travelcardBase].[CreatedOnBehalfBy],
    [st_update_travelcardBase].[ModifiedOnBehalfBy],
    [st_update_travelcardBase].[OwningBusinessUnit],
    [st_update_travelcardBase].[statecode],
    [st_update_travelcardBase].[statuscode],
    [st_update_travelcardBase].[VersionNumber],
    [st_update_travelcardBase].[ImportSequenceNumber],
    [st_update_travelcardBase].[OverriddenCreatedOn],
    [st_update_travelcardBase].[TimeZoneRuleVersionNumber],
    [st_update_travelcardBase].[UTCConversionTimeZoneCode],
    [st_update_travelcardBase].[st_name],
    [st_update_travelcardBase].[st_CardNumber],
    [st_update_travelcardBase].[st_ReplyDate],
    [st_update_travelcardBase].[st_CardNum],
    [st_update_travelcardBase].[st_OP_Offer],
    [st_update_travelcardBase].[st_OP_Offer_Date],
    [st_update_travelcardBase].[st_OP_Offer_Code]
from [st_update_travelcardBase] 
    left join [SystemUserBase] [lk_st_update_travelcard_createdby] with(nolock) on ([st_update_travelcardBase].[CreatedBy] = [lk_st_update_travelcard_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_st_update_travelcard_createdonbehalfby] with(nolock) on ([st_update_travelcardBase].[CreatedOnBehalfBy] = [lk_st_update_travelcard_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_st_update_travelcard_modifiedby] with(nolock) on ([st_update_travelcardBase].[ModifiedBy] = [lk_st_update_travelcard_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_st_update_travelcard_modifiedonbehalfby] with(nolock) on ([st_update_travelcardBase].[ModifiedOnBehalfBy] = [lk_st_update_travelcard_modifiedonbehalfby].[SystemUserId])
    left join [cgi_travelcardBase] [st_cgi_travelcard_st_update_travelcard_CardNum] on ([st_update_travelcardBase].[st_CardNum] = [st_cgi_travelcard_st_update_travelcard_CardNum].[cgi_travelcardId])
    left join OwnerBase XXowner with(nolock) on ([st_update_travelcardBase].OwnerId = XXowner.OwnerId)

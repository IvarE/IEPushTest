


--
-- base view for cgi_bank
--
create view dbo.[cgi_bank]
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

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_bankId],
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
    [cgi_bankname]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_bank_createdby].[FullName],
    [lk_cgi_bank_createdby].[YomiFullName],
    [lk_cgi_bank_createdonbehalfby].[FullName],
    [lk_cgi_bank_createdonbehalfby].[YomiFullName],
    [lk_cgi_bank_modifiedby].[FullName],
    [lk_cgi_bank_modifiedby].[YomiFullName],
    [lk_cgi_bank_modifiedonbehalfby].[FullName],
    [lk_cgi_bank_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_bankBase].OwnerId,
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
    [cgi_bankBase].[cgi_bankId],
    [cgi_bankBase].[CreatedOn],
    [cgi_bankBase].[CreatedBy],
    [cgi_bankBase].[ModifiedOn],
    [cgi_bankBase].[ModifiedBy],
    [cgi_bankBase].[CreatedOnBehalfBy],
    [cgi_bankBase].[ModifiedOnBehalfBy],
    [cgi_bankBase].[OwningBusinessUnit],
    [cgi_bankBase].[statecode],
    [cgi_bankBase].[statuscode],
    [cgi_bankBase].[VersionNumber],
    [cgi_bankBase].[ImportSequenceNumber],
    [cgi_bankBase].[OverriddenCreatedOn],
    [cgi_bankBase].[TimeZoneRuleVersionNumber],
    [cgi_bankBase].[UTCConversionTimeZoneCode],
    [cgi_bankBase].[cgi_bankname]
from [cgi_bankBase] 
    left join [SystemUserBase] [lk_cgi_bank_createdby] with(nolock) on ([cgi_bankBase].[CreatedBy] = [lk_cgi_bank_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_bank_createdonbehalfby] with(nolock) on ([cgi_bankBase].[CreatedOnBehalfBy] = [lk_cgi_bank_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_bank_modifiedby] with(nolock) on ([cgi_bankBase].[ModifiedBy] = [lk_cgi_bank_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_bank_modifiedonbehalfby] with(nolock) on ([cgi_bankBase].[ModifiedOnBehalfBy] = [lk_cgi_bank_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_bankBase].OwnerId = XXowner.OwnerId)




--
-- base view for cgi_travelcardtype
--
create view dbo.[cgi_travelcardtype]
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
    [cgi_travelcardtypeId],
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
    [cgi_travelcardtypename]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_travelcardtype_createdby].[FullName],
    [lk_cgi_travelcardtype_createdby].[YomiFullName],
    [lk_cgi_travelcardtype_createdonbehalfby].[FullName],
    [lk_cgi_travelcardtype_createdonbehalfby].[YomiFullName],
    [lk_cgi_travelcardtype_modifiedby].[FullName],
    [lk_cgi_travelcardtype_modifiedby].[YomiFullName],
    [lk_cgi_travelcardtype_modifiedonbehalfby].[FullName],
    [lk_cgi_travelcardtype_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_travelcardtypeBase].OwnerId,
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
    [cgi_travelcardtypeBase].[cgi_travelcardtypeId],
    [cgi_travelcardtypeBase].[CreatedOn],
    [cgi_travelcardtypeBase].[CreatedBy],
    [cgi_travelcardtypeBase].[ModifiedOn],
    [cgi_travelcardtypeBase].[ModifiedBy],
    [cgi_travelcardtypeBase].[CreatedOnBehalfBy],
    [cgi_travelcardtypeBase].[ModifiedOnBehalfBy],
    [cgi_travelcardtypeBase].[OwningBusinessUnit],
    [cgi_travelcardtypeBase].[statecode],
    [cgi_travelcardtypeBase].[statuscode],
    [cgi_travelcardtypeBase].[VersionNumber],
    [cgi_travelcardtypeBase].[ImportSequenceNumber],
    [cgi_travelcardtypeBase].[OverriddenCreatedOn],
    [cgi_travelcardtypeBase].[TimeZoneRuleVersionNumber],
    [cgi_travelcardtypeBase].[UTCConversionTimeZoneCode],
    [cgi_travelcardtypeBase].[cgi_travelcardtypename]
from [cgi_travelcardtypeBase] 
    left join [SystemUserBase] [lk_cgi_travelcardtype_createdby] with(nolock) on ([cgi_travelcardtypeBase].[CreatedBy] = [lk_cgi_travelcardtype_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtype_createdonbehalfby] with(nolock) on ([cgi_travelcardtypeBase].[CreatedOnBehalfBy] = [lk_cgi_travelcardtype_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtype_modifiedby] with(nolock) on ([cgi_travelcardtypeBase].[ModifiedBy] = [lk_cgi_travelcardtype_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtype_modifiedonbehalfby] with(nolock) on ([cgi_travelcardtypeBase].[ModifiedOnBehalfBy] = [lk_cgi_travelcardtype_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_travelcardtypeBase].OwnerId = XXowner.OwnerId)

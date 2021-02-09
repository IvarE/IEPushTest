


--
-- base view for cgi_zonename
--
create view dbo.[cgi_zonename]
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
    [cgi_zonenameId],
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
    [cgi_name],
    [cgi_ZoneID]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_zonename_createdby].[FullName],
    [lk_cgi_zonename_createdby].[YomiFullName],
    [lk_cgi_zonename_createdonbehalfby].[FullName],
    [lk_cgi_zonename_createdonbehalfby].[YomiFullName],
    [lk_cgi_zonename_modifiedby].[FullName],
    [lk_cgi_zonename_modifiedby].[YomiFullName],
    [lk_cgi_zonename_modifiedonbehalfby].[FullName],
    [lk_cgi_zonename_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_zonenameBase].OwnerId,
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
    [cgi_zonenameBase].[cgi_zonenameId],
    [cgi_zonenameBase].[CreatedOn],
    [cgi_zonenameBase].[CreatedBy],
    [cgi_zonenameBase].[ModifiedOn],
    [cgi_zonenameBase].[ModifiedBy],
    [cgi_zonenameBase].[CreatedOnBehalfBy],
    [cgi_zonenameBase].[ModifiedOnBehalfBy],
    [cgi_zonenameBase].[OwningBusinessUnit],
    [cgi_zonenameBase].[statecode],
    [cgi_zonenameBase].[statuscode],
    [cgi_zonenameBase].[VersionNumber],
    [cgi_zonenameBase].[ImportSequenceNumber],
    [cgi_zonenameBase].[OverriddenCreatedOn],
    [cgi_zonenameBase].[TimeZoneRuleVersionNumber],
    [cgi_zonenameBase].[UTCConversionTimeZoneCode],
    [cgi_zonenameBase].[cgi_name],
    [cgi_zonenameBase].[cgi_ZoneID]
from [cgi_zonenameBase] 
    left join [SystemUserBase] [lk_cgi_zonename_createdby] with(nolock) on ([cgi_zonenameBase].[CreatedBy] = [lk_cgi_zonename_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_zonename_createdonbehalfby] with(nolock) on ([cgi_zonenameBase].[CreatedOnBehalfBy] = [lk_cgi_zonename_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_zonename_modifiedby] with(nolock) on ([cgi_zonenameBase].[ModifiedBy] = [lk_cgi_zonename_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_zonename_modifiedonbehalfby] with(nolock) on ([cgi_zonenameBase].[ModifiedOnBehalfBy] = [lk_cgi_zonename_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_zonenameBase].OwnerId = XXowner.OwnerId)




--
-- base view for cgi_transportcompany
--
create view dbo.[cgi_transportcompany]
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
    [cgi_transportcompanyId],
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
    [cgi_name]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_transportcompany_createdby].[FullName],
    [lk_cgi_transportcompany_createdby].[YomiFullName],
    [lk_cgi_transportcompany_createdonbehalfby].[FullName],
    [lk_cgi_transportcompany_createdonbehalfby].[YomiFullName],
    [lk_cgi_transportcompany_modifiedby].[FullName],
    [lk_cgi_transportcompany_modifiedby].[YomiFullName],
    [lk_cgi_transportcompany_modifiedonbehalfby].[FullName],
    [lk_cgi_transportcompany_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_transportcompanyBase].OwnerId,
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
    [cgi_transportcompanyBase].[cgi_transportcompanyId],
    [cgi_transportcompanyBase].[CreatedOn],
    [cgi_transportcompanyBase].[CreatedBy],
    [cgi_transportcompanyBase].[ModifiedOn],
    [cgi_transportcompanyBase].[ModifiedBy],
    [cgi_transportcompanyBase].[CreatedOnBehalfBy],
    [cgi_transportcompanyBase].[ModifiedOnBehalfBy],
    [cgi_transportcompanyBase].[OwningBusinessUnit],
    [cgi_transportcompanyBase].[statecode],
    [cgi_transportcompanyBase].[statuscode],
    [cgi_transportcompanyBase].[VersionNumber],
    [cgi_transportcompanyBase].[ImportSequenceNumber],
    [cgi_transportcompanyBase].[OverriddenCreatedOn],
    [cgi_transportcompanyBase].[TimeZoneRuleVersionNumber],
    [cgi_transportcompanyBase].[UTCConversionTimeZoneCode],
    [cgi_transportcompanyBase].[cgi_name]
from [cgi_transportcompanyBase] 
    left join [SystemUserBase] [lk_cgi_transportcompany_createdby] with(nolock) on ([cgi_transportcompanyBase].[CreatedBy] = [lk_cgi_transportcompany_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_transportcompany_createdonbehalfby] with(nolock) on ([cgi_transportcompanyBase].[CreatedOnBehalfBy] = [lk_cgi_transportcompany_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_transportcompany_modifiedby] with(nolock) on ([cgi_transportcompanyBase].[ModifiedBy] = [lk_cgi_transportcompany_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_transportcompany_modifiedonbehalfby] with(nolock) on ([cgi_transportcompanyBase].[ModifiedOnBehalfBy] = [lk_cgi_transportcompany_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_transportcompanyBase].OwnerId = XXowner.OwnerId)

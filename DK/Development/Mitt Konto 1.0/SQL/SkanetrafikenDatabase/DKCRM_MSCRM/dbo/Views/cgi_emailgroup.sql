


--
-- base view for cgi_emailgroup
--
create view dbo.[cgi_emailgroup]
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
    [cgi_emailgroupId],
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
    [cgi_emailgroupname]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_emailgroup_createdby].[FullName],
    [lk_cgi_emailgroup_createdby].[YomiFullName],
    [lk_cgi_emailgroup_createdonbehalfby].[FullName],
    [lk_cgi_emailgroup_createdonbehalfby].[YomiFullName],
    [lk_cgi_emailgroup_modifiedby].[FullName],
    [lk_cgi_emailgroup_modifiedby].[YomiFullName],
    [lk_cgi_emailgroup_modifiedonbehalfby].[FullName],
    [lk_cgi_emailgroup_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_emailgroupBase].OwnerId,
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
    [cgi_emailgroupBase].[cgi_emailgroupId],
    [cgi_emailgroupBase].[CreatedOn],
    [cgi_emailgroupBase].[CreatedBy],
    [cgi_emailgroupBase].[ModifiedOn],
    [cgi_emailgroupBase].[ModifiedBy],
    [cgi_emailgroupBase].[CreatedOnBehalfBy],
    [cgi_emailgroupBase].[ModifiedOnBehalfBy],
    [cgi_emailgroupBase].[OwningBusinessUnit],
    [cgi_emailgroupBase].[statecode],
    [cgi_emailgroupBase].[statuscode],
    [cgi_emailgroupBase].[VersionNumber],
    [cgi_emailgroupBase].[ImportSequenceNumber],
    [cgi_emailgroupBase].[OverriddenCreatedOn],
    [cgi_emailgroupBase].[TimeZoneRuleVersionNumber],
    [cgi_emailgroupBase].[UTCConversionTimeZoneCode],
    [cgi_emailgroupBase].[cgi_emailgroupname]
from [cgi_emailgroupBase] 
    left join [SystemUserBase] [lk_cgi_emailgroup_createdby] with(nolock) on ([cgi_emailgroupBase].[CreatedBy] = [lk_cgi_emailgroup_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailgroup_createdonbehalfby] with(nolock) on ([cgi_emailgroupBase].[CreatedOnBehalfBy] = [lk_cgi_emailgroup_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailgroup_modifiedby] with(nolock) on ([cgi_emailgroupBase].[ModifiedBy] = [lk_cgi_emailgroup_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailgroup_modifiedonbehalfby] with(nolock) on ([cgi_emailgroupBase].[ModifiedOnBehalfBy] = [lk_cgi_emailgroup_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_emailgroupBase].OwnerId = XXowner.OwnerId)

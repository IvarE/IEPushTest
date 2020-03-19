


--
-- base view for cgi_emailrecipient
--
create view dbo.[cgi_emailrecipient]
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
    [cgi_EmailGroupidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_emailrecipientId],
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
    [cgi_emailrecipientname],
    [cgi_EmailAddress],
    [cgi_Role],
    [cgi_EmailGroupid]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_emailrecipient_createdby].[FullName],
    [lk_cgi_emailrecipient_createdby].[YomiFullName],
    [lk_cgi_emailrecipient_createdonbehalfby].[FullName],
    [lk_cgi_emailrecipient_createdonbehalfby].[YomiFullName],
    [lk_cgi_emailrecipient_modifiedby].[FullName],
    [lk_cgi_emailrecipient_modifiedby].[YomiFullName],
    [lk_cgi_emailrecipient_modifiedonbehalfby].[FullName],
    [lk_cgi_emailrecipient_modifiedonbehalfby].[YomiFullName],
    [cgi_cgi_emailgroup_cgi_emailrecipient_EmailGroupid].[cgi_emailgroupname],

    -- ownership entries
    OwnerId = [cgi_emailrecipientBase].OwnerId,
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
    [cgi_emailrecipientBase].[cgi_emailrecipientId],
    [cgi_emailrecipientBase].[CreatedOn],
    [cgi_emailrecipientBase].[CreatedBy],
    [cgi_emailrecipientBase].[ModifiedOn],
    [cgi_emailrecipientBase].[ModifiedBy],
    [cgi_emailrecipientBase].[CreatedOnBehalfBy],
    [cgi_emailrecipientBase].[ModifiedOnBehalfBy],
    [cgi_emailrecipientBase].[OwningBusinessUnit],
    [cgi_emailrecipientBase].[statecode],
    [cgi_emailrecipientBase].[statuscode],
    [cgi_emailrecipientBase].[VersionNumber],
    [cgi_emailrecipientBase].[ImportSequenceNumber],
    [cgi_emailrecipientBase].[OverriddenCreatedOn],
    [cgi_emailrecipientBase].[TimeZoneRuleVersionNumber],
    [cgi_emailrecipientBase].[UTCConversionTimeZoneCode],
    [cgi_emailrecipientBase].[cgi_emailrecipientname],
    [cgi_emailrecipientBase].[cgi_EmailAddress],
    [cgi_emailrecipientBase].[cgi_Role],
    [cgi_emailrecipientBase].[cgi_EmailGroupid]
from [cgi_emailrecipientBase] 
    left join [cgi_emailgroupBase] [cgi_cgi_emailgroup_cgi_emailrecipient_EmailGroupid] on ([cgi_emailrecipientBase].[cgi_EmailGroupid] = [cgi_cgi_emailgroup_cgi_emailrecipient_EmailGroupid].[cgi_emailgroupId])
    left join [SystemUserBase] [lk_cgi_emailrecipient_createdby] with(nolock) on ([cgi_emailrecipientBase].[CreatedBy] = [lk_cgi_emailrecipient_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailrecipient_createdonbehalfby] with(nolock) on ([cgi_emailrecipientBase].[CreatedOnBehalfBy] = [lk_cgi_emailrecipient_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailrecipient_modifiedby] with(nolock) on ([cgi_emailrecipientBase].[ModifiedBy] = [lk_cgi_emailrecipient_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_emailrecipient_modifiedonbehalfby] with(nolock) on ([cgi_emailrecipientBase].[ModifiedOnBehalfBy] = [lk_cgi_emailrecipient_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_emailrecipientBase].OwnerId = XXowner.OwnerId)

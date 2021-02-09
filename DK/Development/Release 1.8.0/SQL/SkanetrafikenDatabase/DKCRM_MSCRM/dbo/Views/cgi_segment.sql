


--
-- base view for cgi_segment
--
create view dbo.[cgi_segment]
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
    [cgi_segmentId],
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
    [cgi_segmentname]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_segment_createdby].[FullName],
    [lk_cgi_segment_createdby].[YomiFullName],
    [lk_cgi_segment_createdonbehalfby].[FullName],
    [lk_cgi_segment_createdonbehalfby].[YomiFullName],
    [lk_cgi_segment_modifiedby].[FullName],
    [lk_cgi_segment_modifiedby].[YomiFullName],
    [lk_cgi_segment_modifiedonbehalfby].[FullName],
    [lk_cgi_segment_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_segmentBase].OwnerId,
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
    [cgi_segmentBase].[cgi_segmentId],
    [cgi_segmentBase].[CreatedOn],
    [cgi_segmentBase].[CreatedBy],
    [cgi_segmentBase].[ModifiedOn],
    [cgi_segmentBase].[ModifiedBy],
    [cgi_segmentBase].[CreatedOnBehalfBy],
    [cgi_segmentBase].[ModifiedOnBehalfBy],
    [cgi_segmentBase].[OwningBusinessUnit],
    [cgi_segmentBase].[statecode],
    [cgi_segmentBase].[statuscode],
    [cgi_segmentBase].[VersionNumber],
    [cgi_segmentBase].[ImportSequenceNumber],
    [cgi_segmentBase].[OverriddenCreatedOn],
    [cgi_segmentBase].[TimeZoneRuleVersionNumber],
    [cgi_segmentBase].[UTCConversionTimeZoneCode],
    [cgi_segmentBase].[cgi_segmentname]
from [cgi_segmentBase] 
    left join [SystemUserBase] [lk_cgi_segment_createdby] with(nolock) on ([cgi_segmentBase].[CreatedBy] = [lk_cgi_segment_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_segment_createdonbehalfby] with(nolock) on ([cgi_segmentBase].[CreatedOnBehalfBy] = [lk_cgi_segment_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_segment_modifiedby] with(nolock) on ([cgi_segmentBase].[ModifiedBy] = [lk_cgi_segment_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_segment_modifiedonbehalfby] with(nolock) on ([cgi_segmentBase].[ModifiedOnBehalfBy] = [lk_cgi_segment_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_segmentBase].OwnerId = XXowner.OwnerId)

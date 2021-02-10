


--
-- base view for cgi_filelink
--
create view dbo.[cgi_filelink]
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
    [cgi_IncidentIdName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_filelinkId],
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
    [cgi_filelinkname],
    [cgi_IncidentId],
    [cgi_URL]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_filelink_createdby].[FullName],
    [lk_cgi_filelink_createdby].[YomiFullName],
    [lk_cgi_filelink_createdonbehalfby].[FullName],
    [lk_cgi_filelink_createdonbehalfby].[YomiFullName],
    [lk_cgi_filelink_modifiedby].[FullName],
    [lk_cgi_filelink_modifiedby].[YomiFullName],
    [lk_cgi_filelink_modifiedonbehalfby].[FullName],
    [lk_cgi_filelink_modifiedonbehalfby].[YomiFullName],
    [cgi_incident_cgi_filelink].[Title],

    -- ownership entries
    OwnerId = [cgi_filelinkBase].OwnerId,
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
    [cgi_filelinkBase].[cgi_filelinkId],
    [cgi_filelinkBase].[CreatedOn],
    [cgi_filelinkBase].[CreatedBy],
    [cgi_filelinkBase].[ModifiedOn],
    [cgi_filelinkBase].[ModifiedBy],
    [cgi_filelinkBase].[CreatedOnBehalfBy],
    [cgi_filelinkBase].[ModifiedOnBehalfBy],
    [cgi_filelinkBase].[OwningBusinessUnit],
    [cgi_filelinkBase].[statecode],
    [cgi_filelinkBase].[statuscode],
    [cgi_filelinkBase].[VersionNumber],
    [cgi_filelinkBase].[ImportSequenceNumber],
    [cgi_filelinkBase].[OverriddenCreatedOn],
    [cgi_filelinkBase].[TimeZoneRuleVersionNumber],
    [cgi_filelinkBase].[UTCConversionTimeZoneCode],
    [cgi_filelinkBase].[cgi_filelinkname],
    [cgi_filelinkBase].[cgi_IncidentId],
    [cgi_filelinkBase].[cgi_URL]
from [cgi_filelinkBase] 
    left join [IncidentBase] [cgi_incident_cgi_filelink] on ([cgi_filelinkBase].[cgi_IncidentId] = [cgi_incident_cgi_filelink].[IncidentId])
    left join [SystemUserBase] [lk_cgi_filelink_createdby] with(nolock) on ([cgi_filelinkBase].[CreatedBy] = [lk_cgi_filelink_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_filelink_createdonbehalfby] with(nolock) on ([cgi_filelinkBase].[CreatedOnBehalfBy] = [lk_cgi_filelink_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_filelink_modifiedby] with(nolock) on ([cgi_filelinkBase].[ModifiedBy] = [lk_cgi_filelink_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_filelink_modifiedonbehalfby] with(nolock) on ([cgi_filelinkBase].[ModifiedOnBehalfBy] = [lk_cgi_filelink_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_filelinkBase].OwnerId = XXowner.OwnerId)

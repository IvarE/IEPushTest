


--
-- base view for cgi_passtravelinformation
--
create view dbo.[cgi_passtravelinformation]
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
    [cgi_passtravelinformationId],
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
    [cgi_passtravelinformation],
    [cgi_sTCN],
    [cgi_sTCID],
    [cgi_iTLID],
    [cgi_sTLN],
    [cgi_sTRN],
    [cgi_iTJID],
    [cgi_iTFID],
    [cgi_sTFN],
    [cgi_sTFD],
    [cgi_sTFT],
    [cgi_iTTID],
    [cgi_sTTN],
    [cgi_sTTD],
    [cgi_sTTT],
    [cgi_IncidentId]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_passtravelinformation_createdby].[FullName],
    [lk_cgi_passtravelinformation_createdby].[YomiFullName],
    [lk_cgi_passtravelinformation_createdonbehalfby].[FullName],
    [lk_cgi_passtravelinformation_createdonbehalfby].[YomiFullName],
    [lk_cgi_passtravelinformation_modifiedby].[FullName],
    [lk_cgi_passtravelinformation_modifiedby].[YomiFullName],
    [lk_cgi_passtravelinformation_modifiedonbehalfby].[FullName],
    [lk_cgi_passtravelinformation_modifiedonbehalfby].[YomiFullName],
    [cgi_incident_cgi_passtravelinformation_IncidentId].[Title],

    -- ownership entries
    OwnerId = [cgi_passtravelinformationBase].OwnerId,
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
    [cgi_passtravelinformationBase].[cgi_passtravelinformationId],
    [cgi_passtravelinformationBase].[CreatedOn],
    [cgi_passtravelinformationBase].[CreatedBy],
    [cgi_passtravelinformationBase].[ModifiedOn],
    [cgi_passtravelinformationBase].[ModifiedBy],
    [cgi_passtravelinformationBase].[CreatedOnBehalfBy],
    [cgi_passtravelinformationBase].[ModifiedOnBehalfBy],
    [cgi_passtravelinformationBase].[OwningBusinessUnit],
    [cgi_passtravelinformationBase].[statecode],
    [cgi_passtravelinformationBase].[statuscode],
    [cgi_passtravelinformationBase].[VersionNumber],
    [cgi_passtravelinformationBase].[ImportSequenceNumber],
    [cgi_passtravelinformationBase].[OverriddenCreatedOn],
    [cgi_passtravelinformationBase].[TimeZoneRuleVersionNumber],
    [cgi_passtravelinformationBase].[UTCConversionTimeZoneCode],
    [cgi_passtravelinformationBase].[cgi_passtravelinformation],
    [cgi_passtravelinformationBase].[cgi_sTCN],
    [cgi_passtravelinformationBase].[cgi_sTCID],
    [cgi_passtravelinformationBase].[cgi_iTLID],
    [cgi_passtravelinformationBase].[cgi_sTLN],
    [cgi_passtravelinformationBase].[cgi_sTRN],
    [cgi_passtravelinformationBase].[cgi_iTJID],
    [cgi_passtravelinformationBase].[cgi_iTFID],
    [cgi_passtravelinformationBase].[cgi_sTFN],
    [cgi_passtravelinformationBase].[cgi_sTFD],
    [cgi_passtravelinformationBase].[cgi_sTFT],
    [cgi_passtravelinformationBase].[cgi_iTTID],
    [cgi_passtravelinformationBase].[cgi_sTTN],
    [cgi_passtravelinformationBase].[cgi_sTTD],
    [cgi_passtravelinformationBase].[cgi_sTTT],
    [cgi_passtravelinformationBase].[cgi_IncidentId]
from [cgi_passtravelinformationBase] 
    left join [IncidentBase] [cgi_incident_cgi_passtravelinformation_IncidentId] on ([cgi_passtravelinformationBase].[cgi_IncidentId] = [cgi_incident_cgi_passtravelinformation_IncidentId].[IncidentId])
    left join [SystemUserBase] [lk_cgi_passtravelinformation_createdby] with(nolock) on ([cgi_passtravelinformationBase].[CreatedBy] = [lk_cgi_passtravelinformation_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_passtravelinformation_createdonbehalfby] with(nolock) on ([cgi_passtravelinformationBase].[CreatedOnBehalfBy] = [lk_cgi_passtravelinformation_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_passtravelinformation_modifiedby] with(nolock) on ([cgi_passtravelinformationBase].[ModifiedBy] = [lk_cgi_passtravelinformation_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_passtravelinformation_modifiedonbehalfby] with(nolock) on ([cgi_passtravelinformationBase].[ModifiedOnBehalfBy] = [lk_cgi_passtravelinformation_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_passtravelinformationBase].OwnerId = XXowner.OwnerId)




--
-- base view for cgi_travelinformation
--
create view dbo.[cgi_travelinformation]
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
    [cgi_CaseidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_travelinformationId],
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
    [cgi_travelinformation],
    [cgi_ArivalActual],
    [cgi_ArivalPlanned],
    [cgi_City],
    [cgi_Contractor],
    [cgi_DirectionText],
    [cgi_Line],
    [cgi_StartActual],
    [cgi_StartPlanned],
    [cgi_Stop],
    [cgi_Tour],
    [cgi_Transport],
    [cgi_Caseid],
    [cgi_Start],
    [cgi_Deviationmessage],
    [cgi_DisplayText],
    [cgi_JourneyNumber],
    [cgi_LineType],
    [cgi_TrainNumber]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_travelinformation_createdby].[FullName],
    [lk_cgi_travelinformation_createdby].[YomiFullName],
    [lk_cgi_travelinformation_createdonbehalfby].[FullName],
    [lk_cgi_travelinformation_createdonbehalfby].[YomiFullName],
    [lk_cgi_travelinformation_modifiedby].[FullName],
    [lk_cgi_travelinformation_modifiedby].[YomiFullName],
    [lk_cgi_travelinformation_modifiedonbehalfby].[FullName],
    [lk_cgi_travelinformation_modifiedonbehalfby].[YomiFullName],
    [cgi_incident_cgi_travelinformation_Caseid].[Title],

    -- ownership entries
    OwnerId = [cgi_travelinformationBase].OwnerId,
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
    [cgi_travelinformationBase].[cgi_travelinformationId],
    [cgi_travelinformationBase].[CreatedOn],
    [cgi_travelinformationBase].[CreatedBy],
    [cgi_travelinformationBase].[ModifiedOn],
    [cgi_travelinformationBase].[ModifiedBy],
    [cgi_travelinformationBase].[CreatedOnBehalfBy],
    [cgi_travelinformationBase].[ModifiedOnBehalfBy],
    [cgi_travelinformationBase].[OwningBusinessUnit],
    [cgi_travelinformationBase].[statecode],
    [cgi_travelinformationBase].[statuscode],
    [cgi_travelinformationBase].[VersionNumber],
    [cgi_travelinformationBase].[ImportSequenceNumber],
    [cgi_travelinformationBase].[OverriddenCreatedOn],
    [cgi_travelinformationBase].[TimeZoneRuleVersionNumber],
    [cgi_travelinformationBase].[UTCConversionTimeZoneCode],
    [cgi_travelinformationBase].[cgi_travelinformation],
    [cgi_travelinformationBase].[cgi_ArivalActual],
    [cgi_travelinformationBase].[cgi_ArivalPlanned],
    [cgi_travelinformationBase].[cgi_City],
    [cgi_travelinformationBase].[cgi_Contractor],
    [cgi_travelinformationBase].[cgi_DirectionText],
    [cgi_travelinformationBase].[cgi_Line],
    [cgi_travelinformationBase].[cgi_StartActual],
    [cgi_travelinformationBase].[cgi_StartPlanned],
    [cgi_travelinformationBase].[cgi_Stop],
    [cgi_travelinformationBase].[cgi_Tour],
    [cgi_travelinformationBase].[cgi_Transport],
    [cgi_travelinformationBase].[cgi_Caseid],
    [cgi_travelinformationBase].[cgi_Start],
    [cgi_travelinformationBase].[cgi_Deviationmessage],
    [cgi_travelinformationBase].[cgi_DisplayText],
    [cgi_travelinformationBase].[cgi_JourneyNumber],
    [cgi_travelinformationBase].[cgi_LineType],
    [cgi_travelinformationBase].[cgi_TrainNumber]
from [cgi_travelinformationBase] 
    left join [IncidentBase] [cgi_incident_cgi_travelinformation_Caseid] on ([cgi_travelinformationBase].[cgi_Caseid] = [cgi_incident_cgi_travelinformation_Caseid].[IncidentId])
    left join [SystemUserBase] [lk_cgi_travelinformation_createdby] with(nolock) on ([cgi_travelinformationBase].[CreatedBy] = [lk_cgi_travelinformation_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelinformation_createdonbehalfby] with(nolock) on ([cgi_travelinformationBase].[CreatedOnBehalfBy] = [lk_cgi_travelinformation_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelinformation_modifiedby] with(nolock) on ([cgi_travelinformationBase].[ModifiedBy] = [lk_cgi_travelinformation_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelinformation_modifiedonbehalfby] with(nolock) on ([cgi_travelinformationBase].[ModifiedOnBehalfBy] = [lk_cgi_travelinformation_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_travelinformationBase].OwnerId = XXowner.OwnerId)

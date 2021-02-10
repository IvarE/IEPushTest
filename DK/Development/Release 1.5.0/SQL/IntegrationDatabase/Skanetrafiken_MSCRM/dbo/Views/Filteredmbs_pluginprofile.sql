

--
-- report view for mbs_pluginprofile
--
create view dbo.[Filteredmbs_pluginprofile] (
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [importsequencenumber],
    [mbs_businessunitid],
    [mbs_businessunitidname],
    [mbs_configuration],
    [mbs_correlationid],
    [mbs_depth],
    [mbs_initiatinguserid],
    [mbs_initiatinguseridname],
    [mbs_initiatinguseridyominame],
    [mbs_label],
    [mbs_messagename],
    [mbs_mode],
    [mbs_modename],
    [mbs_operationtype],
    [mbs_operationtypename],
    [mbs_performanceconstructorduration],
    [mbs_performanceconstructorstarttime],
    [mbs_performanceconstructorstarttimeutc],
    [mbs_performanceexecutionduration],
    [mbs_performanceexecutionstarttime],
    [mbs_performanceexecutionstarttimeutc],
    [mbs_persistencekey],
    [mbs_pluginprofileid],
    [mbs_primaryentity],
    [mbs_profile],
    [mbs_requestid],
    [mbs_secureconfiguration],
    [mbs_workflowstepid],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [organizationid],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [mbs_pluginprofile].[CreatedBy],
    [mbs_pluginprofile].[CreatedByName],
    [mbs_pluginprofile].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([mbs_pluginprofile].[CreatedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [mbs_pluginprofile].[CreatedOn],
    [mbs_pluginprofile].[CreatedOnBehalfBy],
    [mbs_pluginprofile].[CreatedOnBehalfByName],
    [mbs_pluginprofile].[CreatedOnBehalfByYomiName],
    [mbs_pluginprofile].[ImportSequenceNumber],
    [mbs_pluginprofile].[mbs_BusinessUnitId],
    [mbs_pluginprofile].[mbs_BusinessUnitIdName],
    [mbs_pluginprofile].[mbs_Configuration],
    [mbs_pluginprofile].[mbs_CorrelationId],
    [mbs_pluginprofile].[mbs_Depth],
    [mbs_pluginprofile].[mbs_InitiatingUserId],
    [mbs_pluginprofile].[mbs_InitiatingUserIdName],
    [mbs_pluginprofile].[mbs_InitiatingUserIdYomiName],
    [mbs_pluginprofile].[mbs_label],
    [mbs_pluginprofile].[mbs_MessageName],
    [mbs_pluginprofile].[mbs_mode],
    mbs_modePLTable.Value,
    [mbs_pluginprofile].[mbs_OperationType],
    mbs_OperationTypePLTable.Value,
    [mbs_pluginprofile].[mbs_PerformanceConstructorDuration],
    dbo.fn_UTCToTzSpecificLocalTime([mbs_pluginprofile].[mbs_PerformanceConstructorStartTime], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [mbs_pluginprofile].[mbs_PerformanceConstructorStartTime],
    [mbs_pluginprofile].[mbs_PerformanceExecutionDuration],
    dbo.fn_UTCToTzSpecificLocalTime([mbs_pluginprofile].[mbs_PerformanceExecutionStartTime], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [mbs_pluginprofile].[mbs_PerformanceExecutionStartTime],
    [mbs_pluginprofile].[mbs_PersistenceKey],
    [mbs_pluginprofile].[mbs_pluginprofileId],
    [mbs_pluginprofile].[mbs_PrimaryEntity],
    [mbs_pluginprofile].[mbs_Profile],
    [mbs_pluginprofile].[mbs_RequestId],
    [mbs_pluginprofile].[mbs_SecureConfiguration],
    [mbs_pluginprofile].[mbs_WorkflowStepId],
    [mbs_pluginprofile].[ModifiedBy],
    [mbs_pluginprofile].[ModifiedByName],
    [mbs_pluginprofile].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([mbs_pluginprofile].[ModifiedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [mbs_pluginprofile].[ModifiedOn],
    [mbs_pluginprofile].[ModifiedOnBehalfBy],
    [mbs_pluginprofile].[ModifiedOnBehalfByName],
    [mbs_pluginprofile].[ModifiedOnBehalfByYomiName],
    [mbs_pluginprofile].[OrganizationId],
    [mbs_pluginprofile].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([mbs_pluginprofile].[OverriddenCreatedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [mbs_pluginprofile].[OverriddenCreatedOn],
    [mbs_pluginprofile].[statecode],
    statecodePLTable.Value,
    [mbs_pluginprofile].[statuscode],
    statuscodePLTable.Value,
    [mbs_pluginprofile].[TimeZoneRuleVersionNumber],
    [mbs_pluginprofile].[UTCConversionTimeZoneCode],
    [mbs_pluginprofile].[VersionNumber]
from mbs_pluginprofile
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [mbs_modePLTable] on 
		([mbs_modePLTable].AttributeName = 'mbs_mode'
		and [mbs_modePLTable].ObjectTypeCode = 10057
		and [mbs_modePLTable].AttributeValue = [mbs_pluginprofile].[mbs_mode]
		and [mbs_modePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [mbs_OperationTypePLTable] on 
		([mbs_OperationTypePLTable].AttributeName = 'mbs_operationtype'
		and [mbs_OperationTypePLTable].ObjectTypeCode = 10057
		and [mbs_OperationTypePLTable].AttributeValue = [mbs_pluginprofile].[mbs_OperationType]
		and [mbs_OperationTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10057
		and [statecodePLTable].AttributeValue = [mbs_pluginprofile].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10057
		and [statuscodePLTable].AttributeValue = [mbs_pluginprofile].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10057) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [mbs_pluginprofile].OrganizationId = u.OrganizationId
)

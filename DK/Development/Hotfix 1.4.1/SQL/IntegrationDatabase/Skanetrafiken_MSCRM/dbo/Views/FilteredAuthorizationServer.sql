

--
-- report view for authorizationserver
--
create view dbo.[FilteredAuthorizationServer] (
    [authorizationserverid],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [metadata],
    [metadatarefreshedon],
    [metadatarefreshedonutc],
    [metadataurl],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [organizationidname],
    [principalid],
    [realm],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [tenantid],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode]
) with view_metadata as
select
    [AuthorizationServer].[AuthorizationServerId],
    [AuthorizationServer].[CreatedBy],
    [AuthorizationServer].[CreatedByName],
    [AuthorizationServer].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([AuthorizationServer].[CreatedOn], 
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
        [AuthorizationServer].[CreatedOn],
    [AuthorizationServer].[CreatedOnBehalfBy],
    [AuthorizationServer].[CreatedOnBehalfByName],
    [AuthorizationServer].[CreatedOnBehalfByYomiName],
    [AuthorizationServer].[Metadata],
    dbo.fn_UTCToTzSpecificLocalTime([AuthorizationServer].[MetadataRefreshedOn], 
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
        [AuthorizationServer].[MetadataRefreshedOn],
    [AuthorizationServer].[MetadataUrl],
    [AuthorizationServer].[ModifiedBy],
    [AuthorizationServer].[ModifiedByName],
    [AuthorizationServer].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([AuthorizationServer].[ModifiedOn], 
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
        [AuthorizationServer].[ModifiedOn],
    [AuthorizationServer].[ModifiedOnBehalfBy],
    [AuthorizationServer].[ModifiedOnBehalfByName],
    [AuthorizationServer].[ModifiedOnBehalfByYomiName],
    [AuthorizationServer].[Name],
    [AuthorizationServer].[OrganizationId],
    [AuthorizationServer].[OrganizationIdName],
    [AuthorizationServer].[PrincipalId],
    [AuthorizationServer].[Realm],
    [AuthorizationServer].[StateCode],
    StateCodePLTable.Value,
    [AuthorizationServer].[StatusCode],
    StatusCodePLTable.Value,
    [AuthorizationServer].[TenantId],
    [AuthorizationServer].[TimeZoneRuleVersionNumber],
    [AuthorizationServer].[UTCConversionTimeZoneCode]
from AuthorizationServer
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1094
		and [StateCodePLTable].AttributeValue = [AuthorizationServer].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1094
		and [StatusCodePLTable].AttributeValue = [AuthorizationServer].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1094) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [AuthorizationServer].OrganizationId = u.OrganizationId
)

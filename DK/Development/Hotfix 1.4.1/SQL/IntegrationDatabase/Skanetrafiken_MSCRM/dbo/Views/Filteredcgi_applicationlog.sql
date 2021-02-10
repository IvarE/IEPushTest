

--
-- report view for cgi_applicationlog
--
create view dbo.[Filteredcgi_applicationlog] (
    [cgi_applicationlogid],
    [cgi_applicationname],
    [cgi_logmessage],
    [cgi_logtype],
    [cgi_logtypename],
    [cgi_method],
    [cgi_systemexception],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [importsequencenumber],
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
    [cgi_applicationlog].[cgi_applicationlogId],
    [cgi_applicationlog].[cgi_applicationname],
    [cgi_applicationlog].[cgi_LogMessage],
    [cgi_applicationlog].[cgi_LogType],
    cgi_LogTypePLTable.Value,
    [cgi_applicationlog].[cgi_Method],
    [cgi_applicationlog].[cgi_SystemException],
    [cgi_applicationlog].[CreatedBy],
    [cgi_applicationlog].[CreatedByName],
    [cgi_applicationlog].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_applicationlog].[CreatedOn], 
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
        [cgi_applicationlog].[CreatedOn],
    [cgi_applicationlog].[CreatedOnBehalfBy],
    [cgi_applicationlog].[CreatedOnBehalfByName],
    [cgi_applicationlog].[CreatedOnBehalfByYomiName],
    [cgi_applicationlog].[ImportSequenceNumber],
    [cgi_applicationlog].[ModifiedBy],
    [cgi_applicationlog].[ModifiedByName],
    [cgi_applicationlog].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_applicationlog].[ModifiedOn], 
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
        [cgi_applicationlog].[ModifiedOn],
    [cgi_applicationlog].[ModifiedOnBehalfBy],
    [cgi_applicationlog].[ModifiedOnBehalfByName],
    [cgi_applicationlog].[ModifiedOnBehalfByYomiName],
    [cgi_applicationlog].[OrganizationId],
    [cgi_applicationlog].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_applicationlog].[OverriddenCreatedOn], 
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
        [cgi_applicationlog].[OverriddenCreatedOn],
    [cgi_applicationlog].[statecode],
    statecodePLTable.Value,
    [cgi_applicationlog].[statuscode],
    statuscodePLTable.Value,
    [cgi_applicationlog].[TimeZoneRuleVersionNumber],
    [cgi_applicationlog].[UTCConversionTimeZoneCode],
    [cgi_applicationlog].[VersionNumber]
from cgi_applicationlog
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_LogTypePLTable] on 
		([cgi_LogTypePLTable].AttributeName = 'cgi_logtype'
		and [cgi_LogTypePLTable].ObjectTypeCode = 10005
		and [cgi_LogTypePLTable].AttributeValue = [cgi_applicationlog].[cgi_LogType]
		and [cgi_LogTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10005
		and [statecodePLTable].AttributeValue = [cgi_applicationlog].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10005
		and [statuscodePLTable].AttributeValue = [cgi_applicationlog].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10005) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_applicationlog].OrganizationId = u.OrganizationId
)

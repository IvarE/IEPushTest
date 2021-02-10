

--
-- report view for cgi_localizedlabelgroup
--
create view dbo.[Filteredcgi_localizedlabelgroup] (
    [cgi_localizedlabelgroupid],
    [cgi_localizedlabelgroupname],
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
    [cgi_localizedlabelgroup].[cgi_localizedlabelgroupId],
    [cgi_localizedlabelgroup].[cgi_localizedlabelgroupname],
    [cgi_localizedlabelgroup].[CreatedBy],
    [cgi_localizedlabelgroup].[CreatedByName],
    [cgi_localizedlabelgroup].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabelgroup].[CreatedOn], 
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
        [cgi_localizedlabelgroup].[CreatedOn],
    [cgi_localizedlabelgroup].[CreatedOnBehalfBy],
    [cgi_localizedlabelgroup].[CreatedOnBehalfByName],
    [cgi_localizedlabelgroup].[CreatedOnBehalfByYomiName],
    [cgi_localizedlabelgroup].[ImportSequenceNumber],
    [cgi_localizedlabelgroup].[ModifiedBy],
    [cgi_localizedlabelgroup].[ModifiedByName],
    [cgi_localizedlabelgroup].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabelgroup].[ModifiedOn], 
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
        [cgi_localizedlabelgroup].[ModifiedOn],
    [cgi_localizedlabelgroup].[ModifiedOnBehalfBy],
    [cgi_localizedlabelgroup].[ModifiedOnBehalfByName],
    [cgi_localizedlabelgroup].[ModifiedOnBehalfByYomiName],
    [cgi_localizedlabelgroup].[OrganizationId],
    [cgi_localizedlabelgroup].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabelgroup].[OverriddenCreatedOn], 
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
        [cgi_localizedlabelgroup].[OverriddenCreatedOn],
    [cgi_localizedlabelgroup].[statecode],
    statecodePLTable.Value,
    [cgi_localizedlabelgroup].[statuscode],
    statuscodePLTable.Value,
    [cgi_localizedlabelgroup].[TimeZoneRuleVersionNumber],
    [cgi_localizedlabelgroup].[UTCConversionTimeZoneCode],
    [cgi_localizedlabelgroup].[VersionNumber]
from cgi_localizedlabelgroup
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10018
		and [statecodePLTable].AttributeValue = [cgi_localizedlabelgroup].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10018
		and [statuscodePLTable].AttributeValue = [cgi_localizedlabelgroup].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10018) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_localizedlabelgroup].OrganizationId = u.OrganizationId
)

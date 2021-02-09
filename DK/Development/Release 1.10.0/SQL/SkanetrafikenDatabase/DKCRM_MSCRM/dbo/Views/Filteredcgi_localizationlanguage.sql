

--
-- report view for cgi_localizationlanguage
--
create view dbo.[Filteredcgi_localizationlanguage] (
    [cgi_localizationlanguageid],
    [cgi_localizationlanguagename],
    [cgi_localizationlanguagenumber],
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
    [cgi_localizationlanguage].[cgi_localizationlanguageId],
    [cgi_localizationlanguage].[cgi_localizationlanguagename],
    [cgi_localizationlanguage].[cgi_LocalizationLanguageNumber],
    [cgi_localizationlanguage].[CreatedBy],
    [cgi_localizationlanguage].[CreatedByName],
    [cgi_localizationlanguage].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizationlanguage].[CreatedOn], 
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
        [cgi_localizationlanguage].[CreatedOn],
    [cgi_localizationlanguage].[CreatedOnBehalfBy],
    [cgi_localizationlanguage].[CreatedOnBehalfByName],
    [cgi_localizationlanguage].[CreatedOnBehalfByYomiName],
    [cgi_localizationlanguage].[ImportSequenceNumber],
    [cgi_localizationlanguage].[ModifiedBy],
    [cgi_localizationlanguage].[ModifiedByName],
    [cgi_localizationlanguage].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizationlanguage].[ModifiedOn], 
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
        [cgi_localizationlanguage].[ModifiedOn],
    [cgi_localizationlanguage].[ModifiedOnBehalfBy],
    [cgi_localizationlanguage].[ModifiedOnBehalfByName],
    [cgi_localizationlanguage].[ModifiedOnBehalfByYomiName],
    [cgi_localizationlanguage].[OrganizationId],
    [cgi_localizationlanguage].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizationlanguage].[OverriddenCreatedOn], 
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
        [cgi_localizationlanguage].[OverriddenCreatedOn],
    [cgi_localizationlanguage].[statecode],
    statecodePLTable.Value,
    [cgi_localizationlanguage].[statuscode],
    statuscodePLTable.Value,
    [cgi_localizationlanguage].[TimeZoneRuleVersionNumber],
    [cgi_localizationlanguage].[UTCConversionTimeZoneCode],
    [cgi_localizationlanguage].[VersionNumber]
from cgi_localizationlanguage
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10016
		and [statecodePLTable].AttributeValue = [cgi_localizationlanguage].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10016
		and [statuscodePLTable].AttributeValue = [cgi_localizationlanguage].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10016) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_localizationlanguage].OrganizationId = u.OrganizationId
)



--
-- report view for cgi_localizedlabel
--
create view dbo.[Filteredcgi_localizedlabel] (
    [cgi_localizationlanguageid],
    [cgi_localizationlanguageidname],
    [cgi_localizedcontrolid],
    [cgi_localizedlabelgroupid],
    [cgi_localizedlabelgroupidname],
    [cgi_localizedlabelid],
    [cgi_localizedlabelname],
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
    [cgi_localizedlabel].[cgi_LocalizationLanguageid],
    [cgi_localizedlabel].[cgi_LocalizationLanguageidName],
    [cgi_localizedlabel].[cgi_LocalizedControlId],
    [cgi_localizedlabel].[cgi_LocalizedLabelGroupid],
    [cgi_localizedlabel].[cgi_LocalizedLabelGroupidName],
    [cgi_localizedlabel].[cgi_localizedlabelId],
    [cgi_localizedlabel].[cgi_localizedlabelname],
    [cgi_localizedlabel].[CreatedBy],
    [cgi_localizedlabel].[CreatedByName],
    [cgi_localizedlabel].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabel].[CreatedOn], 
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
        [cgi_localizedlabel].[CreatedOn],
    [cgi_localizedlabel].[CreatedOnBehalfBy],
    [cgi_localizedlabel].[CreatedOnBehalfByName],
    [cgi_localizedlabel].[CreatedOnBehalfByYomiName],
    [cgi_localizedlabel].[ImportSequenceNumber],
    [cgi_localizedlabel].[ModifiedBy],
    [cgi_localizedlabel].[ModifiedByName],
    [cgi_localizedlabel].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabel].[ModifiedOn], 
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
        [cgi_localizedlabel].[ModifiedOn],
    [cgi_localizedlabel].[ModifiedOnBehalfBy],
    [cgi_localizedlabel].[ModifiedOnBehalfByName],
    [cgi_localizedlabel].[ModifiedOnBehalfByYomiName],
    [cgi_localizedlabel].[OrganizationId],
    [cgi_localizedlabel].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_localizedlabel].[OverriddenCreatedOn], 
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
        [cgi_localizedlabel].[OverriddenCreatedOn],
    [cgi_localizedlabel].[statecode],
    statecodePLTable.Value,
    [cgi_localizedlabel].[statuscode],
    statuscodePLTable.Value,
    [cgi_localizedlabel].[TimeZoneRuleVersionNumber],
    [cgi_localizedlabel].[UTCConversionTimeZoneCode],
    [cgi_localizedlabel].[VersionNumber]
from cgi_localizedlabel
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10017
		and [statecodePLTable].AttributeValue = [cgi_localizedlabel].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10017
		and [statuscodePLTable].AttributeValue = [cgi_localizedlabel].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10017) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_localizedlabel].OrganizationId = u.OrganizationId
)

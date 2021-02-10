

--
-- report view for cgi_autonumber
--
create view dbo.[Filteredcgi_autonumber] (
    [cgi_autonumberid],
    [cgi_entity],
    [cgi_lastused],
    [cgi_lockfield],
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
    [cgi_autonumber].[cgi_autonumberId],
    [cgi_autonumber].[cgi_entity],
    [cgi_autonumber].[cgi_LastUsed],
    [cgi_autonumber].[cgi_LockField],
    [cgi_autonumber].[CreatedBy],
    [cgi_autonumber].[CreatedByName],
    [cgi_autonumber].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_autonumber].[CreatedOn], 
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
        [cgi_autonumber].[CreatedOn],
    [cgi_autonumber].[CreatedOnBehalfBy],
    [cgi_autonumber].[CreatedOnBehalfByName],
    [cgi_autonumber].[CreatedOnBehalfByYomiName],
    [cgi_autonumber].[ImportSequenceNumber],
    [cgi_autonumber].[ModifiedBy],
    [cgi_autonumber].[ModifiedByName],
    [cgi_autonumber].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_autonumber].[ModifiedOn], 
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
        [cgi_autonumber].[ModifiedOn],
    [cgi_autonumber].[ModifiedOnBehalfBy],
    [cgi_autonumber].[ModifiedOnBehalfByName],
    [cgi_autonumber].[ModifiedOnBehalfByYomiName],
    [cgi_autonumber].[OrganizationId],
    [cgi_autonumber].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_autonumber].[OverriddenCreatedOn], 
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
        [cgi_autonumber].[OverriddenCreatedOn],
    [cgi_autonumber].[statecode],
    statecodePLTable.Value,
    [cgi_autonumber].[statuscode],
    statuscodePLTable.Value,
    [cgi_autonumber].[TimeZoneRuleVersionNumber],
    [cgi_autonumber].[UTCConversionTimeZoneCode],
    [cgi_autonumber].[VersionNumber]
from cgi_autonumber
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10006
		and [statecodePLTable].AttributeValue = [cgi_autonumber].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10006
		and [statuscodePLTable].AttributeValue = [cgi_autonumber].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10006) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_autonumber].OrganizationId = u.OrganizationId
)

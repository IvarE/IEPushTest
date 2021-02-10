

--
-- report view for equipment
--
create view dbo.[FilteredEquipment] (
    [businessunitid],
    [businessunitiddsc],
    [businessunitidname],
    [calendarid],
    [createdby],
    [createdbydsc],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [displayinserviceviews],
    [displayinserviceviewsname],
    [emailaddress],
    [equipmentid],
    [exchangerate],
    [importsequencenumber],
    [isdisabled],
    [isdisabledname],
    [modifiedby],
    [modifiedbydsc],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [siteid],
    [siteiddsc],
    [siteidname],
    [skills],
    [timezonecode],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [Equipment].[BusinessUnitId],
    --[Equipment].[BusinessUnitIdDsc]
    0,
    [Equipment].[BusinessUnitIdName],
    [Equipment].[CalendarId],
    [Equipment].[CreatedBy],
    --[Equipment].[CreatedByDsc]
    0,
    [Equipment].[CreatedByName],
    [Equipment].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Equipment].[CreatedOn], 
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
        [Equipment].[CreatedOn],
    [Equipment].[CreatedOnBehalfBy],
    --[Equipment].[CreatedOnBehalfByDsc]
    0,
    [Equipment].[CreatedOnBehalfByName],
    [Equipment].[CreatedOnBehalfByYomiName],
    [Equipment].[Description],
    [Equipment].[DisplayInServiceViews],
    DisplayInServiceViewsPLTable.Value,
    [Equipment].[EMailAddress],
    [Equipment].[EquipmentId],
    [Equipment].[ExchangeRate],
    [Equipment].[ImportSequenceNumber],
    [Equipment].[IsDisabled],
    IsDisabledPLTable.Value,
    [Equipment].[ModifiedBy],
    --[Equipment].[ModifiedByDsc]
    0,
    [Equipment].[ModifiedByName],
    [Equipment].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Equipment].[ModifiedOn], 
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
        [Equipment].[ModifiedOn],
    [Equipment].[ModifiedOnBehalfBy],
    --[Equipment].[ModifiedOnBehalfByDsc]
    0,
    [Equipment].[ModifiedOnBehalfByName],
    [Equipment].[ModifiedOnBehalfByYomiName],
    [Equipment].[Name],
    [Equipment].[OrganizationId],
    --[Equipment].[OrganizationIdDsc]
    0,
    [Equipment].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([Equipment].[OverriddenCreatedOn], 
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
        [Equipment].[OverriddenCreatedOn],
    [Equipment].[SiteId],
    --[Equipment].[SiteIdDsc]
    0,
    [Equipment].[SiteIdName],
    [Equipment].[Skills],
    [Equipment].[TimeZoneCode],
    [Equipment].[TimeZoneRuleVersionNumber],
    [Equipment].[TransactionCurrencyId],
    [Equipment].[TransactionCurrencyIdName],
    [Equipment].[UTCConversionTimeZoneCode],
    [Equipment].[VersionNumber]
from Equipment
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [DisplayInServiceViewsPLTable] on 
		([DisplayInServiceViewsPLTable].AttributeName = 'displayinserviceviews'
		and [DisplayInServiceViewsPLTable].ObjectTypeCode = 4000
		and [DisplayInServiceViewsPLTable].AttributeValue = [Equipment].[DisplayInServiceViews]
		and [DisplayInServiceViewsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsDisabledPLTable] on 
		([IsDisabledPLTable].AttributeName = 'isdisabled'
		and [IsDisabledPLTable].ObjectTypeCode = 4000
		and [IsDisabledPLTable].AttributeValue = [Equipment].[IsDisabled]
		and [IsDisabledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4000) pdm
where
(
    
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[Equipment].[BusinessUnitId] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4000)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Equipment].[BusinessUnitId] is not null 
	) 
)

)

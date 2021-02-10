

--
-- report view for calendarrule
--
create view dbo.[FilteredCalendarRule] (
    [businessunitid],
    [calendarid],
    [calendarruleid],
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
    [duration],
    [effectiveintervalend],
    [effectiveintervalendutc],
    [effectiveintervalstart],
    [effectiveintervalstartutc],
    [effort],
    [endtime],
    [endtimeutc],
    [extentcode],
    [groupdesignator],
    [innercalendarid],
    [ismodified],
    [ismodifiedname],
    [isselected],
    [isselectedname],
    [issimple],
    [issimplename],
    [isvaried],
    [isvariedname],
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
    [offset],
    [organizationid],
    [pattern],
    [rank],
    [serviceid],
    [serviceiddsc],
    [serviceidname],
    [starttime],
    [starttimeutc],
    [subcode],
    [timecode],
    [timezonecode],
    [versionnumber]
) with view_metadata as
select
    [CalendarRule].[BusinessUnitId],
    [CalendarRule].[CalendarId],
    [CalendarRule].[CalendarRuleId],
    [CalendarRule].[CreatedBy],
    --[CalendarRule].[CreatedByDsc]
    0,
    [CalendarRule].[CreatedByName],
    [CalendarRule].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[CreatedOn], 
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
        [CalendarRule].[CreatedOn],
    [CalendarRule].[CreatedOnBehalfBy],
    --[CalendarRule].[CreatedOnBehalfByDsc]
    0,
    [CalendarRule].[CreatedOnBehalfByName],
    [CalendarRule].[CreatedOnBehalfByYomiName],
    [CalendarRule].[Description],
    [CalendarRule].[Duration],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[EffectiveIntervalEnd], 
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
        [CalendarRule].[EffectiveIntervalEnd],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[EffectiveIntervalStart], 
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
        [CalendarRule].[EffectiveIntervalStart],
    [CalendarRule].[Effort],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[EndTime], 
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
        [CalendarRule].[EndTime],
    [CalendarRule].[ExtentCode],
    [CalendarRule].[GroupDesignator],
    [CalendarRule].[InnerCalendarId],
    [CalendarRule].[IsModified],
    IsModifiedPLTable.Value,
    [CalendarRule].[IsSelected],
    IsSelectedPLTable.Value,
    [CalendarRule].[IsSimple],
    IsSimplePLTable.Value,
    [CalendarRule].[IsVaried],
    IsVariedPLTable.Value,
    [CalendarRule].[ModifiedBy],
    --[CalendarRule].[ModifiedByDsc]
    0,
    [CalendarRule].[ModifiedByName],
    [CalendarRule].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[ModifiedOn], 
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
        [CalendarRule].[ModifiedOn],
    [CalendarRule].[ModifiedOnBehalfBy],
    --[CalendarRule].[ModifiedOnBehalfByDsc]
    0,
    [CalendarRule].[ModifiedOnBehalfByName],
    [CalendarRule].[ModifiedOnBehalfByYomiName],
    [CalendarRule].[Name],
    [CalendarRule].[Offset],
    [CalendarRule].[OrganizationId],
    [CalendarRule].[Pattern],
    [CalendarRule].[Rank],
    [CalendarRule].[ServiceId],
    --[CalendarRule].[ServiceIdDsc]
    0,
    [CalendarRule].[ServiceIdName],
    dbo.fn_UTCToTzSpecificLocalTime([CalendarRule].[StartTime], 
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
        [CalendarRule].[StartTime],
    [CalendarRule].[SubCode],
    [CalendarRule].[TimeCode],
    [CalendarRule].[TimeZoneCode],
    [CalendarRule].[VersionNumber]
from CalendarRule
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsModifiedPLTable] on 
		([IsModifiedPLTable].AttributeName = 'ismodified'
		and [IsModifiedPLTable].ObjectTypeCode = 4004
		and [IsModifiedPLTable].AttributeValue = [CalendarRule].[IsModified]
		and [IsModifiedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsSelectedPLTable] on 
		([IsSelectedPLTable].AttributeName = 'isselected'
		and [IsSelectedPLTable].ObjectTypeCode = 4004
		and [IsSelectedPLTable].AttributeValue = [CalendarRule].[IsSelected]
		and [IsSelectedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsSimplePLTable] on 
		([IsSimplePLTable].AttributeName = 'issimple'
		and [IsSimplePLTable].ObjectTypeCode = 4004
		and [IsSimplePLTable].AttributeValue = [CalendarRule].[IsSimple]
		and [IsSimplePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsVariedPLTable] on 
		([IsVariedPLTable].AttributeName = 'isvaried'
		and [IsVariedPLTable].ObjectTypeCode = 4004
		and [IsVariedPLTable].AttributeValue = [CalendarRule].[IsVaried]
		and [IsVariedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4003) pdm
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
		[CalendarRule].[BusinessUnitId] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4003)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[CalendarRule].[BusinessUnitId] is not null 
	) 
)

)

﻿

--
-- report view for rollupfield
--
create view dbo.[FilteredRollupFieldItem] (
    [createdby],
    [createdbyname],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [dateattribute],
    [entityfordateattribute],
    [entityfordateattributename],
    [goalattribute],
    [importsequencenumber],
    [isstateparententityattribute],
    [metricid],
    [metricidname],
    [modifiedby],
    [modifiedbyname],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [organizationid],
    [rollupfieldid],
    [sourceattribute],
    [sourceentity],
    [sourceentityname],
    [sourcestate],
    [sourcestatus],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [RollupField].[CreatedBy],
    [RollupField].[CreatedByName],
    dbo.fn_UTCToTzSpecificLocalTime([RollupField].[CreatedOn], 
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
        [RollupField].[CreatedOn],
    [RollupField].[CreatedOnBehalfBy],
    --[RollupField].[CreatedOnBehalfByDsc]
    0,
    [RollupField].[CreatedOnBehalfByName],
    [RollupField].[CreatedOnBehalfByYomiName],
    [RollupField].[DateAttribute],
    [RollupField].[EntityForDateAttribute],
    EntityForDateAttributePLTable.Value,
    [RollupField].[GoalAttribute],
    [RollupField].[ImportSequenceNumber],
    [RollupField].[IsStateParentEntityAttribute],
    [RollupField].[MetricId],
    [RollupField].[MetricIdName],
    [RollupField].[ModifiedBy],
    [RollupField].[ModifiedByName],
    dbo.fn_UTCToTzSpecificLocalTime([RollupField].[ModifiedOn], 
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
        [RollupField].[ModifiedOn],
    [RollupField].[ModifiedOnBehalfBy],
    --[RollupField].[ModifiedOnBehalfByDsc]
    0,
    [RollupField].[ModifiedOnBehalfByName],
    [RollupField].[ModifiedOnBehalfByYomiName],
    [RollupField].[OrganizationId],
    [RollupField].[RollupFieldId],
    [RollupField].[SourceAttribute],
    [RollupField].[SourceEntity],
    SourceEntityPLTable.Value,
    [RollupField].[SourceState],
    [RollupField].[SourceStatus],
    [RollupField].[TimeZoneRuleVersionNumber],
    [RollupField].[UTCConversionTimeZoneCode],
    [RollupField].[VersionNumber]
from RollupField
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [EntityForDateAttributePLTable] on 
		([EntityForDateAttributePLTable].AttributeName = 'entityfordateattribute'
		and [EntityForDateAttributePLTable].ObjectTypeCode = 9604
		and [EntityForDateAttributePLTable].AttributeValue = [RollupField].[EntityForDateAttribute]
		and [EntityForDateAttributePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SourceEntityPLTable] on 
		([SourceEntityPLTable].AttributeName = 'sourceentity'
		and [SourceEntityPLTable].ObjectTypeCode = 9604
		and [SourceEntityPLTable].AttributeValue = [RollupField].[SourceEntity]
		and [SourceEntityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9603) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RollupField].OrganizationId = u.OrganizationId
)

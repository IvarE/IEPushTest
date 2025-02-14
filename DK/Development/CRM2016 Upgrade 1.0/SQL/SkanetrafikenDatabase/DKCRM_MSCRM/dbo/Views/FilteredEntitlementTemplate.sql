﻿

--
-- report view for entitlementtemplate
--
create view dbo.[FilteredEntitlementTemplate] (
    [allocationtypecode],
    [allocationtypecodename],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [decreaseremainingon],
    [decreaseremainingonname],
    [description],
    [enddate],
    [enddateutc],
    [entitlementtemplateid],
    [exchangerate],
    [importsequencenumber],
    [kbaccesslevel],
    [kbaccesslevelname],
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
    [overriddencreatedon],
    [overriddencreatedonutc],
    [restrictcasecreation],
    [restrictcasecreationname],
    [slaid],
    [slaidname],
    [startdate],
    [startdateutc],
    [timezoneruleversionnumber],
    [totalterms],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [EntitlementTemplate].[AllocationTypeCode],
    AllocationTypeCodePLTable.Value,
    [EntitlementTemplate].[CreatedBy],
    [EntitlementTemplate].[CreatedByName],
    [EntitlementTemplate].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([EntitlementTemplate].[CreatedOn], 
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
        [EntitlementTemplate].[CreatedOn],
    [EntitlementTemplate].[CreatedOnBehalfBy],
    [EntitlementTemplate].[CreatedOnBehalfByName],
    [EntitlementTemplate].[CreatedOnBehalfByYomiName],
    [EntitlementTemplate].[DecreaseRemainingOn],
    DecreaseRemainingOnPLTable.Value,
    [EntitlementTemplate].[Description],
    dbo.fn_UTCToTzSpecificLocalTime([EntitlementTemplate].[EndDate], 
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
        [EntitlementTemplate].[EndDate],
    [EntitlementTemplate].[EntitlementTemplateId],
    [EntitlementTemplate].[ExchangeRate],
    [EntitlementTemplate].[ImportSequenceNumber],
    [EntitlementTemplate].[KbAccessLevel],
    KbAccessLevelPLTable.Value,
    [EntitlementTemplate].[ModifiedBy],
    [EntitlementTemplate].[ModifiedByName],
    [EntitlementTemplate].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([EntitlementTemplate].[ModifiedOn], 
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
        [EntitlementTemplate].[ModifiedOn],
    [EntitlementTemplate].[ModifiedOnBehalfBy],
    [EntitlementTemplate].[ModifiedOnBehalfByName],
    [EntitlementTemplate].[ModifiedOnBehalfByYomiName],
    [EntitlementTemplate].[Name],
    [EntitlementTemplate].[OrganizationId],
    [EntitlementTemplate].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([EntitlementTemplate].[OverriddenCreatedOn], 
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
        [EntitlementTemplate].[OverriddenCreatedOn],
    [EntitlementTemplate].[RestrictCaseCreation],
    RestrictCaseCreationPLTable.Value,
    [EntitlementTemplate].[SLAId],
    [EntitlementTemplate].[SLAIdName],
    dbo.fn_UTCToTzSpecificLocalTime([EntitlementTemplate].[StartDate], 
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
        [EntitlementTemplate].[StartDate],
    [EntitlementTemplate].[TimeZoneRuleVersionNumber],
    [EntitlementTemplate].[TotalTerms],
    [EntitlementTemplate].[TransactionCurrencyId],
    [EntitlementTemplate].[TransactionCurrencyIdName],
    [EntitlementTemplate].[UTCConversionTimeZoneCode],
    [EntitlementTemplate].[VersionNumber]
from EntitlementTemplate
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [AllocationTypeCodePLTable] on 
		([AllocationTypeCodePLTable].AttributeName = 'allocationtypecode'
		and [AllocationTypeCodePLTable].ObjectTypeCode = 9702
		and [AllocationTypeCodePLTable].AttributeValue = [EntitlementTemplate].[AllocationTypeCode]
		and [AllocationTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DecreaseRemainingOnPLTable] on 
		([DecreaseRemainingOnPLTable].AttributeName = 'decreaseremainingon'
		and [DecreaseRemainingOnPLTable].ObjectTypeCode = 9702
		and [DecreaseRemainingOnPLTable].AttributeValue = [EntitlementTemplate].[DecreaseRemainingOn]
		and [DecreaseRemainingOnPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [KbAccessLevelPLTable] on 
		([KbAccessLevelPLTable].AttributeName = 'kbaccesslevel'
		and [KbAccessLevelPLTable].ObjectTypeCode = 9702
		and [KbAccessLevelPLTable].AttributeValue = [EntitlementTemplate].[KbAccessLevel]
		and [KbAccessLevelPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [RestrictCaseCreationPLTable] on 
		([RestrictCaseCreationPLTable].AttributeName = 'restrictcasecreation'
		and [RestrictCaseCreationPLTable].ObjectTypeCode = 9702
		and [RestrictCaseCreationPLTable].AttributeValue = [EntitlementTemplate].[RestrictCaseCreation]
		and [RestrictCaseCreationPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9702) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [EntitlementTemplate].OrganizationId = u.OrganizationId
)

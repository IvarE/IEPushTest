﻿

--
-- report view for discounttype
--
create view dbo.[FilteredDiscountType] (
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
    [discounttypeid],
    [importsequencenumber],
    [isamounttype],
    [isamounttypename],
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
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [versionnumber]
) with view_metadata as
select
    [DiscountType].[CreatedBy],
    --[DiscountType].[CreatedByDsc]
    0,
    [DiscountType].[CreatedByName],
    [DiscountType].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([DiscountType].[CreatedOn], 
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
        [DiscountType].[CreatedOn],
    [DiscountType].[CreatedOnBehalfBy],
    --[DiscountType].[CreatedOnBehalfByDsc]
    0,
    [DiscountType].[CreatedOnBehalfByName],
    [DiscountType].[CreatedOnBehalfByYomiName],
    [DiscountType].[Description],
    [DiscountType].[DiscountTypeId],
    [DiscountType].[ImportSequenceNumber],
    [DiscountType].[IsAmountType],
    IsAmountTypePLTable.Value,
    [DiscountType].[ModifiedBy],
    --[DiscountType].[ModifiedByDsc]
    0,
    [DiscountType].[ModifiedByName],
    [DiscountType].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([DiscountType].[ModifiedOn], 
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
        [DiscountType].[ModifiedOn],
    [DiscountType].[ModifiedOnBehalfBy],
    --[DiscountType].[ModifiedOnBehalfByDsc]
    0,
    [DiscountType].[ModifiedOnBehalfByName],
    [DiscountType].[ModifiedOnBehalfByYomiName],
    [DiscountType].[Name],
    [DiscountType].[OrganizationId],
    --[DiscountType].[OrganizationIdDsc]
    0,
    [DiscountType].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([DiscountType].[OverriddenCreatedOn], 
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
        [DiscountType].[OverriddenCreatedOn],
    [DiscountType].[StateCode],
    StateCodePLTable.Value,
    [DiscountType].[StatusCode],
    StatusCodePLTable.Value,
    [DiscountType].[TransactionCurrencyId],
    --[DiscountType].[TransactionCurrencyIdDsc]
    0,
    [DiscountType].[TransactionCurrencyIdName],
    [DiscountType].[VersionNumber]
from DiscountType
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsAmountTypePLTable] on 
		([IsAmountTypePLTable].AttributeName = 'isamounttype'
		and [IsAmountTypePLTable].ObjectTypeCode = 1080
		and [IsAmountTypePLTable].AttributeValue = [DiscountType].[IsAmountType]
		and [IsAmountTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1080
		and [StateCodePLTable].AttributeValue = [DiscountType].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1080
		and [StatusCodePLTable].AttributeValue = [DiscountType].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1080) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [DiscountType].OrganizationId = u.OrganizationId
)

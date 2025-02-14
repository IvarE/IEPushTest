﻿

--
-- report view for pricelevel
--
create view dbo.[FilteredPriceLevel] (
    [begindate],
    [begindateutc],
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
    [enddate],
    [enddateutc],
    [exchangerate],
    [freighttermscode],
    [freighttermscodename],
    [importsequencenumber],
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
    [paymentmethodcode],
    [paymentmethodcodename],
    [pricelevelid],
    [shippingmethodcode],
    [shippingmethodcodename],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    dbo.fn_UTCToTzSpecificLocalTime([PriceLevel].[BeginDate], 
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
        [PriceLevel].[BeginDate],
    [PriceLevel].[CreatedBy],
    --[PriceLevel].[CreatedByDsc]
    0,
    [PriceLevel].[CreatedByName],
    [PriceLevel].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([PriceLevel].[CreatedOn], 
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
        [PriceLevel].[CreatedOn],
    [PriceLevel].[CreatedOnBehalfBy],
    --[PriceLevel].[CreatedOnBehalfByDsc]
    0,
    [PriceLevel].[CreatedOnBehalfByName],
    [PriceLevel].[CreatedOnBehalfByYomiName],
    [PriceLevel].[Description],
    dbo.fn_UTCToTzSpecificLocalTime([PriceLevel].[EndDate], 
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
        [PriceLevel].[EndDate],
    [PriceLevel].[ExchangeRate],
    [PriceLevel].[FreightTermsCode],
    FreightTermsCodePLTable.Value,
    [PriceLevel].[ImportSequenceNumber],
    [PriceLevel].[ModifiedBy],
    --[PriceLevel].[ModifiedByDsc]
    0,
    [PriceLevel].[ModifiedByName],
    [PriceLevel].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([PriceLevel].[ModifiedOn], 
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
        [PriceLevel].[ModifiedOn],
    [PriceLevel].[ModifiedOnBehalfBy],
    --[PriceLevel].[ModifiedOnBehalfByDsc]
    0,
    [PriceLevel].[ModifiedOnBehalfByName],
    [PriceLevel].[ModifiedOnBehalfByYomiName],
    [PriceLevel].[Name],
    [PriceLevel].[OrganizationId],
    --[PriceLevel].[OrganizationIdDsc]
    0,
    [PriceLevel].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([PriceLevel].[OverriddenCreatedOn], 
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
        [PriceLevel].[OverriddenCreatedOn],
    [PriceLevel].[PaymentMethodCode],
    PaymentMethodCodePLTable.Value,
    [PriceLevel].[PriceLevelId],
    [PriceLevel].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [PriceLevel].[StateCode],
    StateCodePLTable.Value,
    [PriceLevel].[StatusCode],
    StatusCodePLTable.Value,
    [PriceLevel].[TimeZoneRuleVersionNumber],
    [PriceLevel].[TransactionCurrencyId],
    --[PriceLevel].[TransactionCurrencyIdDsc]
    0,
    [PriceLevel].[TransactionCurrencyIdName],
    [PriceLevel].[UTCConversionTimeZoneCode],
    [PriceLevel].[VersionNumber]
from PriceLevel
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [FreightTermsCodePLTable] on 
		([FreightTermsCodePLTable].AttributeName = 'freighttermscode'
		and [FreightTermsCodePLTable].ObjectTypeCode = 1022
		and [FreightTermsCodePLTable].AttributeValue = [PriceLevel].[FreightTermsCode]
		and [FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PaymentMethodCodePLTable] on 
		([PaymentMethodCodePLTable].AttributeName = 'paymentmethodcode'
		and [PaymentMethodCodePLTable].ObjectTypeCode = 1022
		and [PaymentMethodCodePLTable].AttributeValue = [PriceLevel].[PaymentMethodCode]
		and [PaymentMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 1022
		and [ShippingMethodCodePLTable].AttributeValue = [PriceLevel].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1022
		and [StateCodePLTable].AttributeValue = [PriceLevel].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1022
		and [StatusCodePLTable].AttributeValue = [PriceLevel].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1022) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [PriceLevel].OrganizationId = u.OrganizationId
)

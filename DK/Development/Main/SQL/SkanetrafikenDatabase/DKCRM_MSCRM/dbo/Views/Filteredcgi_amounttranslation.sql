

--
-- report view for cgi_amounttranslation
--
create view dbo.[Filteredcgi_amounttranslation] (
    [cgi_amount],
    [cgi_amounttranslationid],
    [cgi_amount_base],
    [cgi_name],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [exchangerate],
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
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [cgi_amounttranslation].[cgi_Amount],
    [cgi_amounttranslation].[cgi_amounttranslationId],
    [cgi_amounttranslation].[cgi_amount_Base],
    [cgi_amounttranslation].[cgi_name],
    [cgi_amounttranslation].[CreatedBy],
    [cgi_amounttranslation].[CreatedByName],
    [cgi_amounttranslation].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_amounttranslation].[CreatedOn], 
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
        [cgi_amounttranslation].[CreatedOn],
    [cgi_amounttranslation].[CreatedOnBehalfBy],
    [cgi_amounttranslation].[CreatedOnBehalfByName],
    [cgi_amounttranslation].[CreatedOnBehalfByYomiName],
    [cgi_amounttranslation].[ExchangeRate],
    [cgi_amounttranslation].[ImportSequenceNumber],
    [cgi_amounttranslation].[ModifiedBy],
    [cgi_amounttranslation].[ModifiedByName],
    [cgi_amounttranslation].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_amounttranslation].[ModifiedOn], 
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
        [cgi_amounttranslation].[ModifiedOn],
    [cgi_amounttranslation].[ModifiedOnBehalfBy],
    [cgi_amounttranslation].[ModifiedOnBehalfByName],
    [cgi_amounttranslation].[ModifiedOnBehalfByYomiName],
    [cgi_amounttranslation].[OrganizationId],
    [cgi_amounttranslation].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_amounttranslation].[OverriddenCreatedOn], 
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
        [cgi_amounttranslation].[OverriddenCreatedOn],
    [cgi_amounttranslation].[statecode],
    statecodePLTable.Value,
    [cgi_amounttranslation].[statuscode],
    statuscodePLTable.Value,
    [cgi_amounttranslation].[TimeZoneRuleVersionNumber],
    [cgi_amounttranslation].[TransactionCurrencyId],
    [cgi_amounttranslation].[TransactionCurrencyIdName],
    [cgi_amounttranslation].[UTCConversionTimeZoneCode],
    [cgi_amounttranslation].[VersionNumber],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from cgi_amounttranslation
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [cgi_amounttranslation].TransactionCurrencyId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10046
		and [statecodePLTable].AttributeValue = [cgi_amounttranslation].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10046
		and [statuscodePLTable].AttributeValue = [cgi_amounttranslation].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10046) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [cgi_amounttranslation].OrganizationId = u.OrganizationId
)

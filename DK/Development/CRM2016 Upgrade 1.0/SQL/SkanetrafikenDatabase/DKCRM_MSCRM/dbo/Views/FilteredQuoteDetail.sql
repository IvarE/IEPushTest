﻿

--
-- report view for quotedetail
--
create view dbo.[FilteredQuoteDetail] (
    [baseamount],
    [baseamount_base],
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
    [exchangerate],
    [extendedamount],
    [extendedamount_base],
    [importsequencenumber],
    [ispriceoverridden],
    [ispriceoverriddenname],
    [isproductoverridden],
    [isproductoverriddenname],
    [lineitemnumber],
    [manualdiscountamount],
    [manualdiscountamount_base],
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
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [priceperunit],
    [priceperunit_base],
    [pricingerrorcode],
    [pricingerrorcodename],
    [productdescription],
    [productid],
    [productiddsc],
    [productidname],
    [quantity],
    [quotedetailid],
    [quoteid],
    [quotestatecode],
    [quotestatecodename],
    [requestdeliveryby],
    [requestdeliverybyutc],
    [salesrepid],
    [salesrepiddsc],
    [salesrepidname],
    [salesrepidyominame],
    [sequencenumber],
    [shipto_addressid],
    [shipto_city],
    [shipto_contactname],
    [shipto_country],
    [shipto_fax],
    [shipto_freighttermscode],
    [shipto_freighttermscodename],
    [shipto_line1],
    [shipto_line2],
    [shipto_line3],
    [shipto_name],
    [shipto_postalcode],
    [shipto_stateorprovince],
    [shipto_telephone],
    [tax],
    [tax_base],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [uomid],
    [uomiddsc],
    [uomidname],
    [utcconversiontimezonecode],
    [versionnumber],
    [volumediscountamount],
    [volumediscountamount_base],
    [willcall],
    [willcallname],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [QuoteDetail].[BaseAmount],
    [QuoteDetail].[BaseAmount_Base],
    [QuoteDetail].[CreatedBy],
    --[QuoteDetail].[CreatedByDsc]
    0,
    [QuoteDetail].[CreatedByName],
    [QuoteDetail].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QuoteDetail].[CreatedOn], 
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
        [QuoteDetail].[CreatedOn],
    [QuoteDetail].[CreatedOnBehalfBy],
    --[QuoteDetail].[CreatedOnBehalfByDsc]
    0,
    [QuoteDetail].[CreatedOnBehalfByName],
    [QuoteDetail].[CreatedOnBehalfByYomiName],
    [QuoteDetail].[Description],
    [QuoteDetail].[ExchangeRate],
    [QuoteDetail].[ExtendedAmount],
    [QuoteDetail].[ExtendedAmount_Base],
    [QuoteDetail].[ImportSequenceNumber],
    [QuoteDetail].[IsPriceOverridden],
    IsPriceOverriddenPLTable.Value,
    [QuoteDetail].[IsProductOverridden],
    IsProductOverriddenPLTable.Value,
    [QuoteDetail].[LineItemNumber],
    [QuoteDetail].[ManualDiscountAmount],
    [QuoteDetail].[ManualDiscountAmount_Base],
    [QuoteDetail].[ModifiedBy],
    --[QuoteDetail].[ModifiedByDsc]
    0,
    [QuoteDetail].[ModifiedByName],
    [QuoteDetail].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QuoteDetail].[ModifiedOn], 
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
        [QuoteDetail].[ModifiedOn],
    [QuoteDetail].[ModifiedOnBehalfBy],
    --[QuoteDetail].[ModifiedOnBehalfByDsc]
    0,
    [QuoteDetail].[ModifiedOnBehalfByName],
    [QuoteDetail].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QuoteDetail].[OverriddenCreatedOn], 
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
        [QuoteDetail].[OverriddenCreatedOn],
    [QuoteDetail].[OwnerId],
    [QuoteDetail].[OwnerIdType],
    [QuoteDetail].[OwningBusinessUnit],
    [QuoteDetail].[OwningUser],
    [QuoteDetail].[PricePerUnit],
    [QuoteDetail].[PricePerUnit_Base],
    [QuoteDetail].[PricingErrorCode],
    PricingErrorCodePLTable.Value,
    [QuoteDetail].[ProductDescription],
    [QuoteDetail].[ProductId],
    --[QuoteDetail].[ProductIdDsc]
    0,
    [QuoteDetail].[ProductIdName],
    [QuoteDetail].[Quantity],
    [QuoteDetail].[QuoteDetailId],
    [QuoteDetail].[QuoteId],
    [QuoteDetail].[QuoteStateCode],
    QuoteStateCodePLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([QuoteDetail].[RequestDeliveryBy], 
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
        [QuoteDetail].[RequestDeliveryBy],
    [QuoteDetail].[SalesRepId],
    --[QuoteDetail].[SalesRepIdDsc]
    0,
    [QuoteDetail].[SalesRepIdName],
    [QuoteDetail].[SalesRepIdYomiName],
    [QuoteDetail].[SequenceNumber],
    [QuoteDetail].[ShipTo_AddressId],
    [QuoteDetail].[ShipTo_City],
    [QuoteDetail].[ShipTo_ContactName],
    [QuoteDetail].[ShipTo_Country],
    [QuoteDetail].[ShipTo_Fax],
    [QuoteDetail].[ShipTo_FreightTermsCode],
    ShipTo_FreightTermsCodePLTable.Value,
    [QuoteDetail].[ShipTo_Line1],
    [QuoteDetail].[ShipTo_Line2],
    [QuoteDetail].[ShipTo_Line3],
    [QuoteDetail].[ShipTo_Name],
    [QuoteDetail].[ShipTo_PostalCode],
    [QuoteDetail].[ShipTo_StateOrProvince],
    [QuoteDetail].[ShipTo_Telephone],
    [QuoteDetail].[Tax],
    [QuoteDetail].[Tax_Base],
    [QuoteDetail].[TimeZoneRuleVersionNumber],
    [QuoteDetail].[TransactionCurrencyId],
    --[QuoteDetail].[TransactionCurrencyIdDsc]
    0,
    [QuoteDetail].[TransactionCurrencyIdName],
    [QuoteDetail].[UoMId],
    --[QuoteDetail].[UoMIdDsc]
    0,
    [QuoteDetail].[UoMIdName],
    [QuoteDetail].[UTCConversionTimeZoneCode],
    [QuoteDetail].[VersionNumber],
    [QuoteDetail].[VolumeDiscountAmount],
    [QuoteDetail].[VolumeDiscountAmount_Base],
    [QuoteDetail].[WillCall],
    WillCallPLTable.Value,
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from QuoteDetail
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [QuoteDetail].TransactionCurrencyId
    left outer join StringMap [IsPriceOverriddenPLTable] on 
		([IsPriceOverriddenPLTable].AttributeName = 'ispriceoverridden'
		and [IsPriceOverriddenPLTable].ObjectTypeCode = 1085
		and [IsPriceOverriddenPLTable].AttributeValue = [QuoteDetail].[IsPriceOverridden]
		and [IsPriceOverriddenPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsProductOverriddenPLTable] on 
		([IsProductOverriddenPLTable].AttributeName = 'isproductoverridden'
		and [IsProductOverriddenPLTable].ObjectTypeCode = 1085
		and [IsProductOverriddenPLTable].AttributeValue = [QuoteDetail].[IsProductOverridden]
		and [IsProductOverriddenPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PricingErrorCodePLTable] on 
		([PricingErrorCodePLTable].AttributeName = 'pricingerrorcode'
		and [PricingErrorCodePLTable].ObjectTypeCode = 1085
		and [PricingErrorCodePLTable].AttributeValue = [QuoteDetail].[PricingErrorCode]
		and [PricingErrorCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [QuoteStateCodePLTable] on 
		([QuoteStateCodePLTable].AttributeName = 'quotestatecode'
		and [QuoteStateCodePLTable].ObjectTypeCode = 1085
		and [QuoteStateCodePLTable].AttributeValue = [QuoteDetail].[QuoteStateCode]
		and [QuoteStateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShipTo_FreightTermsCodePLTable] on 
		([ShipTo_FreightTermsCodePLTable].AttributeName = 'shipto_freighttermscode'
		and [ShipTo_FreightTermsCodePLTable].ObjectTypeCode = 1085
		and [ShipTo_FreightTermsCodePLTable].AttributeValue = [QuoteDetail].[ShipTo_FreightTermsCode]
		and [ShipTo_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [WillCallPLTable] on 
		([WillCallPLTable].AttributeName = 'willcall'
		and [WillCallPLTable].ObjectTypeCode = 1085
		and [WillCallPLTable].AttributeValue = [QuoteDetail].[WillCall]
		and [WillCallPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1084) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[QuoteDetail].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 1084
	)	

		
	-- role based access
	or 
	
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[QuoteDetail].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 1084)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[QuoteDetail].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[QuoteDetail].[QuoteId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 1084 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

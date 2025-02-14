﻿

--
-- report view for invoice
--
create view dbo.[FilteredInvoice] (
    [accountid],
    [accountiddsc],
    [accountidname],
    [accountidyominame],
    [billto_city],
    [billto_composite],
    [billto_country],
    [billto_fax],
    [billto_line1],
    [billto_line2],
    [billto_line3],
    [billto_name],
    [billto_postalcode],
    [billto_stateorprovince],
    [billto_telephone],
    [contactid],
    [contactiddsc],
    [contactidname],
    [contactidyominame],
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
    [customerid],
    [customeriddsc],
    [customeridname],
    [customeridtype],
    [customeridyominame],
    [datedelivered],
    [datedeliveredutc],
    [description],
    [discountamount],
    [discountamount_base],
    [discountpercentage],
    [duedate],
    [duedateutc],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [freightamount],
    [freightamount_base],
    [importsequencenumber],
    [invoiceid],
    [invoicenumber],
    [ispricelocked],
    [ispricelockedname],
    [lastbackofficesubmit],
    [lastbackofficesubmitutc],
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
    [opportunityid],
    [opportunityiddsc],
    [opportunityidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [paymenttermscode],
    [paymenttermscodename],
    [pricelevelid],
    [priceleveliddsc],
    [pricelevelidname],
    [pricingerrorcode],
    [pricingerrorcodename],
    [prioritycode],
    [prioritycodename],
    [processid],
    [salesorderid],
    [salesorderiddsc],
    [salesorderidname],
    [shippingmethodcode],
    [shippingmethodcodename],
    [shipto_city],
    [shipto_composite],
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
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [totalamount],
    [totalamountlessfreight],
    [totalamountlessfreight_base],
    [totalamount_base],
    [totaldiscountamount],
    [totaldiscountamount_base],
    [totallineitemamount],
    [totallineitemamount_base],
    [totallineitemdiscountamount],
    [totallineitemdiscountamount_base],
    [totaltax],
    [totaltax_base],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    [willcall],
    [willcallname],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [Invoice].[AccountId],
    --[Invoice].[AccountIdDsc]
    0,
    [Invoice].[AccountIdName],
    [Invoice].[AccountIdYomiName],
    [Invoice].[BillTo_City],
    [Invoice].[BillTo_Composite],
    [Invoice].[BillTo_Country],
    [Invoice].[BillTo_Fax],
    [Invoice].[BillTo_Line1],
    [Invoice].[BillTo_Line2],
    [Invoice].[BillTo_Line3],
    [Invoice].[BillTo_Name],
    [Invoice].[BillTo_PostalCode],
    [Invoice].[BillTo_StateOrProvince],
    [Invoice].[BillTo_Telephone],
    [Invoice].[ContactId],
    --[Invoice].[ContactIdDsc]
    0,
    [Invoice].[ContactIdName],
    [Invoice].[ContactIdYomiName],
    [Invoice].[CreatedBy],
    --[Invoice].[CreatedByDsc]
    0,
    [Invoice].[CreatedByName],
    [Invoice].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[CreatedOn], 
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
        [Invoice].[CreatedOn],
    [Invoice].[CreatedOnBehalfBy],
    --[Invoice].[CreatedOnBehalfByDsc]
    0,
    [Invoice].[CreatedOnBehalfByName],
    [Invoice].[CreatedOnBehalfByYomiName],
    [Invoice].[CustomerId],
    --[Invoice].[CustomerIdDsc]
    0,
    [Invoice].[CustomerIdName],
    [Invoice].[CustomerIdType],
    [Invoice].[CustomerIdYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[DateDelivered], 
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
        [Invoice].[DateDelivered],
    [Invoice].[Description],
    [Invoice].[DiscountAmount],
    [Invoice].[DiscountAmount_Base],
    [Invoice].[DiscountPercentage],
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[DueDate], 
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
        [Invoice].[DueDate],
    --[Invoice].[EntityImage]
    cast(null as varbinary),
    [Invoice].[EntityImageId],
    [Invoice].[EntityImage_Timestamp],
    [Invoice].[EntityImage_URL],
    [Invoice].[ExchangeRate],
    [Invoice].[FreightAmount],
    [Invoice].[FreightAmount_Base],
    [Invoice].[ImportSequenceNumber],
    [Invoice].[InvoiceId],
    [Invoice].[InvoiceNumber],
    [Invoice].[IsPriceLocked],
    IsPriceLockedPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[LastBackofficeSubmit], 
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
        [Invoice].[LastBackofficeSubmit],
    [Invoice].[ModifiedBy],
    --[Invoice].[ModifiedByDsc]
    0,
    [Invoice].[ModifiedByName],
    [Invoice].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[ModifiedOn], 
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
        [Invoice].[ModifiedOn],
    [Invoice].[ModifiedOnBehalfBy],
    --[Invoice].[ModifiedOnBehalfByDsc]
    0,
    [Invoice].[ModifiedOnBehalfByName],
    [Invoice].[ModifiedOnBehalfByYomiName],
    [Invoice].[Name],
    [Invoice].[OpportunityId],
    --[Invoice].[OpportunityIdDsc]
    0,
    [Invoice].[OpportunityIdName],
    dbo.fn_UTCToTzSpecificLocalTime([Invoice].[OverriddenCreatedOn], 
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
        [Invoice].[OverriddenCreatedOn],
    [Invoice].[OwnerId],
    --[Invoice].[OwnerIdDsc]
    0,
    [Invoice].[OwnerIdName],
    [Invoice].[OwnerIdType],
    [Invoice].[OwnerIdYomiName],
    [Invoice].[OwningBusinessUnit],
    [Invoice].[OwningTeam],
    [Invoice].[OwningUser],
    [Invoice].[PaymentTermsCode],
    PaymentTermsCodePLTable.Value,
    [Invoice].[PriceLevelId],
    --[Invoice].[PriceLevelIdDsc]
    0,
    [Invoice].[PriceLevelIdName],
    [Invoice].[PricingErrorCode],
    PricingErrorCodePLTable.Value,
    [Invoice].[PriorityCode],
    PriorityCodePLTable.Value,
    [Invoice].[ProcessId],
    [Invoice].[SalesOrderId],
    --[Invoice].[SalesOrderIdDsc]
    0,
    [Invoice].[SalesOrderIdName],
    [Invoice].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [Invoice].[ShipTo_City],
    [Invoice].[ShipTo_Composite],
    [Invoice].[ShipTo_Country],
    [Invoice].[ShipTo_Fax],
    [Invoice].[ShipTo_FreightTermsCode],
    ShipTo_FreightTermsCodePLTable.Value,
    [Invoice].[ShipTo_Line1],
    [Invoice].[ShipTo_Line2],
    [Invoice].[ShipTo_Line3],
    [Invoice].[ShipTo_Name],
    [Invoice].[ShipTo_PostalCode],
    [Invoice].[ShipTo_StateOrProvince],
    [Invoice].[ShipTo_Telephone],
    [Invoice].[StageId],
    [Invoice].[StateCode],
    StateCodePLTable.Value,
    [Invoice].[StatusCode],
    StatusCodePLTable.Value,
    [Invoice].[TimeZoneRuleVersionNumber],
    [Invoice].[TotalAmount],
    [Invoice].[TotalAmountLessFreight],
    [Invoice].[TotalAmountLessFreight_Base],
    [Invoice].[TotalAmount_Base],
    [Invoice].[TotalDiscountAmount],
    [Invoice].[TotalDiscountAmount_Base],
    [Invoice].[TotalLineItemAmount],
    [Invoice].[TotalLineItemAmount_Base],
    [Invoice].[TotalLineItemDiscountAmount],
    [Invoice].[TotalLineItemDiscountAmount_Base],
    [Invoice].[TotalTax],
    [Invoice].[TotalTax_Base],
    [Invoice].[TransactionCurrencyId],
    --[Invoice].[TransactionCurrencyIdDsc]
    0,
    [Invoice].[TransactionCurrencyIdName],
    [Invoice].[UTCConversionTimeZoneCode],
    [Invoice].[VersionNumber],
    [Invoice].[WillCall],
    WillCallPLTable.Value,
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from Invoice
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [Invoice].TransactionCurrencyId
    left outer join StringMap [IsPriceLockedPLTable] on 
		([IsPriceLockedPLTable].AttributeName = 'ispricelocked'
		and [IsPriceLockedPLTable].ObjectTypeCode = 1090
		and [IsPriceLockedPLTable].AttributeValue = [Invoice].[IsPriceLocked]
		and [IsPriceLockedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PaymentTermsCodePLTable] on 
		([PaymentTermsCodePLTable].AttributeName = 'paymenttermscode'
		and [PaymentTermsCodePLTable].ObjectTypeCode = 1090
		and [PaymentTermsCodePLTable].AttributeValue = [Invoice].[PaymentTermsCode]
		and [PaymentTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PricingErrorCodePLTable] on 
		([PricingErrorCodePLTable].AttributeName = 'pricingerrorcode'
		and [PricingErrorCodePLTable].ObjectTypeCode = 1090
		and [PricingErrorCodePLTable].AttributeValue = [Invoice].[PricingErrorCode]
		and [PricingErrorCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 1090
		and [PriorityCodePLTable].AttributeValue = [Invoice].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 1090
		and [ShippingMethodCodePLTable].AttributeValue = [Invoice].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShipTo_FreightTermsCodePLTable] on 
		([ShipTo_FreightTermsCodePLTable].AttributeName = 'shipto_freighttermscode'
		and [ShipTo_FreightTermsCodePLTable].ObjectTypeCode = 1090
		and [ShipTo_FreightTermsCodePLTable].AttributeValue = [Invoice].[ShipTo_FreightTermsCode]
		and [ShipTo_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1090
		and [StateCodePLTable].AttributeValue = [Invoice].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1090
		and [StatusCodePLTable].AttributeValue = [Invoice].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [WillCallPLTable] on 
		([WillCallPLTable].AttributeName = 'willcall'
		and [WillCallPLTable].ObjectTypeCode = 1090
		and [WillCallPLTable].AttributeValue = [Invoice].[WillCall]
		and [WillCallPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1090) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Invoice].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 1090
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
		[Invoice].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 1090)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Invoice].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Invoice].[InvoiceId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 1090 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

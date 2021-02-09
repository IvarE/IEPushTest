

--
-- report view for cgi_travelcard
--
create view dbo.[Filteredcgi_travelcard] (
    [cgi_accountid],
    [cgi_accountidname],
    [cgi_accountidyominame],
    [cgi_autoloadconnectiondate],
    [cgi_autoloadconnectiondateutc],
    [cgi_autoloaddisconnectiondate],
    [cgi_autoloaddisconnectiondateutc],
    [cgi_autoloadstatus],
    [cgi_blocked],
    [cgi_blockedname],
    [cgi_cardcategory],
    [cgi_cardtypeid],
    [cgi_cardtypeidname],
    [cgi_contactid],
    [cgi_contactidname],
    [cgi_contactidyominame],
    [cgi_creditcardmask],
    [cgi_currency],
    [cgi_failedattemptstochargemoney],
    [cgi_importid],
    [cgi_latestautoloadamount],
    [cgi_latestautoloadamount_base],
    [cgi_latestchargedate],
    [cgi_latestchargedateutc],
    [cgi_latestfailedattempt],
    [cgi_latestfailedattemptutc],
    [cgi_numberofzones],
    [cgi_periodcardtypeid],
    [cgi_periodic_card_type],
    [cgi_travelcardcvc],
    [cgi_travelcardid],
    [cgi_travelcardname],
    [cgi_travelcardnumber],
    [cgi_validfrom],
    [cgi_validfromutc],
    [cgi_validto],
    [cgi_validtoutc],
    [cgi_valuecardtypeid],
    [cgi_value_card_type],
    [cgi_verifyid],
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
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [st_op_offer],
    [st_op_offername],
    [st_op_offer_code],
    [st_op_offer_date],
    [st_op_offer_dateutc],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [cgi_travelcard].[cgi_Accountid],
    [cgi_travelcard].[cgi_AccountidName],
    [cgi_travelcard].[cgi_AccountidYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_AutoloadConnectionDate], 
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
        [cgi_travelcard].[cgi_AutoloadConnectionDate],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_AutoloadDisconnectionDate], 
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
        [cgi_travelcard].[cgi_AutoloadDisconnectionDate],
    [cgi_travelcard].[cgi_AutoloadStatus],
    [cgi_travelcard].[cgi_Blocked],
    cgi_BlockedPLTable.Value,
    [cgi_travelcard].[cgi_CardCategory],
    [cgi_travelcard].[cgi_CardTypeid],
    [cgi_travelcard].[cgi_CardTypeidName],
    [cgi_travelcard].[cgi_Contactid],
    [cgi_travelcard].[cgi_ContactidName],
    [cgi_travelcard].[cgi_ContactidYomiName],
    [cgi_travelcard].[cgi_CreditCardMask],
    [cgi_travelcard].[cgi_Currency],
    [cgi_travelcard].[cgi_FailedAttemptsToChargeMoney],
    [cgi_travelcard].[cgi_ImportID],
    [cgi_travelcard].[cgi_LatestAutoloadAmount],
    [cgi_travelcard].[cgi_latestautoloadamount_Base],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_LatestChargeDate], 
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
        [cgi_travelcard].[cgi_LatestChargeDate],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_LatestFailedAttempt], 
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
        [cgi_travelcard].[cgi_LatestFailedAttempt],
    [cgi_travelcard].[cgi_NumberofZones],
    [cgi_travelcard].[cgi_PeriodCardTypeId],
    [cgi_travelcard].[cgi_periodic_card_type],
    [cgi_travelcard].[cgi_TravelCardCVC],
    [cgi_travelcard].[cgi_travelcardId],
    [cgi_travelcard].[cgi_TravelCardName],
    [cgi_travelcard].[cgi_travelcardnumber],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_ValidFrom], 
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
        [cgi_travelcard].[cgi_ValidFrom],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[cgi_ValidTo], 
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
        [cgi_travelcard].[cgi_ValidTo],
    [cgi_travelcard].[cgi_ValueCardTypeId],
    [cgi_travelcard].[cgi_value_card_type],
    [cgi_travelcard].[cgi_VerifyId],
    [cgi_travelcard].[CreatedBy],
    [cgi_travelcard].[CreatedByName],
    [cgi_travelcard].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[CreatedOn], 
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
        [cgi_travelcard].[CreatedOn],
    [cgi_travelcard].[CreatedOnBehalfBy],
    [cgi_travelcard].[CreatedOnBehalfByName],
    [cgi_travelcard].[CreatedOnBehalfByYomiName],
    [cgi_travelcard].[ExchangeRate],
    [cgi_travelcard].[ImportSequenceNumber],
    [cgi_travelcard].[ModifiedBy],
    [cgi_travelcard].[ModifiedByName],
    [cgi_travelcard].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[ModifiedOn], 
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
        [cgi_travelcard].[ModifiedOn],
    [cgi_travelcard].[ModifiedOnBehalfBy],
    [cgi_travelcard].[ModifiedOnBehalfByName],
    [cgi_travelcard].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[OverriddenCreatedOn], 
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
        [cgi_travelcard].[OverriddenCreatedOn],
    [cgi_travelcard].[OwnerId],
    --[cgi_travelcard].[OwnerIdDsc]
    0,
    [cgi_travelcard].[OwnerIdName],
    [cgi_travelcard].[OwnerIdType],
    [cgi_travelcard].[OwnerIdYomiName],
    [cgi_travelcard].[OwningBusinessUnit],
    [cgi_travelcard].[OwningTeam],
    [cgi_travelcard].[OwningUser],
    [cgi_travelcard].[statecode],
    statecodePLTable.Value,
    [cgi_travelcard].[statuscode],
    statuscodePLTable.Value,
    case when ([paamst_op_offer].ReadAccess = 4 or [poaast_op_offer].ReadAccess = 1) then [cgi_travelcard].[st_OP_Offer] else null end,
    case when ([paamst_op_offer].ReadAccess = 4 or [poaast_op_offer].ReadAccess = 1) then st_OP_OfferPLTable.Value else null end,
    [cgi_travelcard].[st_OP_Offer_Code],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcard].[st_OP_Offer_Date], 
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
        [cgi_travelcard].[st_OP_Offer_Date],
    [cgi_travelcard].[TimeZoneRuleVersionNumber],
    [cgi_travelcard].[TransactionCurrencyId],
    [cgi_travelcard].[TransactionCurrencyIdName],
    [cgi_travelcard].[UTCConversionTimeZoneCode],
    [cgi_travelcard].[VersionNumber],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from cgi_travelcard
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [cgi_travelcard].TransactionCurrencyId
    left outer join StringMap [cgi_BlockedPLTable] on 
		([cgi_BlockedPLTable].AttributeName = 'cgi_blocked'
		and [cgi_BlockedPLTable].ObjectTypeCode = 10027
		and [cgi_BlockedPLTable].AttributeValue = [cgi_travelcard].[cgi_Blocked]
		and [cgi_BlockedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10027
		and [statecodePLTable].AttributeValue = [cgi_travelcard].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10027
		and [statuscodePLTable].AttributeValue = [cgi_travelcard].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [st_OP_OfferPLTable] on 
		([st_OP_OfferPLTable].AttributeName = 'st_op_offer'
		and [st_OP_OfferPLTable].ObjectTypeCode = 10027
		and [st_OP_OfferPLTable].AttributeValue = [cgi_travelcard].[st_OP_Offer]
		and [st_OP_OfferPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join PrincipalAttributeAccessMap [paamst_op_offer] on 
		([paamst_op_offer].AttributeId = '2b61f6a4-7bd5-e511-80e4-0050569010ad' -- st_op_offer
		and [paamst_op_offer].PrincipalId = u.SystemUserId)
	left outer join dbo.fn_UserSharedAttributeAccess(dbo.fn_FindUserGuid(),
		'2b61f6a4-7bd5-e511-80e4-0050569010ad', -- st_op_offer
		10027) [poaast_op_offer] on
		([poaast_op_offer].ObjectId = [cgi_travelcard].[cgi_travelcardId])

    cross join dbo.fn_GetMaxPrivilegeDepthMask(10027) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_travelcard].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10027
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
		[cgi_travelcard].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10027)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_travelcard].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_travelcard].[cgi_travelcardId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10027 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

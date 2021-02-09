

--
-- report view for account
--
create view dbo.[FilteredAccount] (
    [accountcategorycode],
    [accountcategorycodename],
    [accountclassificationcode],
    [accountclassificationcodename],
    [accountid],
    [accountnumber],
    [accountratingcode],
    [accountratingcodename],
    [address1_addressid],
    [address1_addresstypecode],
    [address1_addresstypecodename],
    [address1_city],
    [address1_composite],
    [address1_country],
    [address1_county],
    [address1_fax],
    [address1_freighttermscode],
    [address1_freighttermscodename],
    [address1_latitude],
    [address1_line1],
    [address1_line2],
    [address1_line3],
    [address1_longitude],
    [address1_name],
    [address1_postalcode],
    [address1_postofficebox],
    [address1_primarycontactname],
    [address1_shippingmethodcode],
    [address1_shippingmethodcodename],
    [address1_stateorprovince],
    [address1_telephone1],
    [address1_telephone2],
    [address1_telephone3],
    [address1_upszone],
    [address1_utcoffset],
    [address2_addressid],
    [address2_addresstypecode],
    [address2_addresstypecodename],
    [address2_city],
    [address2_composite],
    [address2_country],
    [address2_county],
    [address2_fax],
    [address2_freighttermscode],
    [address2_freighttermscodename],
    [address2_latitude],
    [address2_line1],
    [address2_line2],
    [address2_line3],
    [address2_longitude],
    [address2_name],
    [address2_postalcode],
    [address2_postofficebox],
    [address2_primarycontactname],
    [address2_shippingmethodcode],
    [address2_shippingmethodcodename],
    [address2_stateorprovince],
    [address2_telephone1],
    [address2_telephone2],
    [address2_telephone3],
    [address2_upszone],
    [address2_utcoffset],
    [aging30],
    [aging30_base],
    [aging60],
    [aging60_base],
    [aging90],
    [aging90_base],
    [businesstypecode],
    [businesstypecodename],
    [cgi_activated],
    [cgi_activatedname],
    [cgi_addressid],
    [cgi_allow_autoload],
    [cgi_allow_autoloadname],
    [cgi_counterpart],
    [cgi_customertype],
    [cgi_customertypename],
    [cgi_debtcollection],
    [cgi_debtcollectionname],
    [cgi_firstname],
    [cgi_hasepiserveraccount],
    [cgi_hasepiserveraccountname],
    [cgi_importid],
    [cgi_lastname],
    [cgi_max_cards_autoload],
    [cgi_membership],
    [cgi_membershipname],
    [cgi_myaccount],
    [cgi_myaccountname],
    [cgi_myaccount_lastlogin],
    [cgi_myaccount_lastloginutc],
    [cgi_newsletter],
    [cgi_newslettername],
    [cgi_organizational_number],
    [cgi_organization_sub_number],
    [cgi_responsibility],
    [cgi_rsid],
    [cgi_socialsecuritynumber],
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
    [creditlimit],
    [creditlimit_base],
    [creditonhold],
    [creditonholdname],
    [customersizecode],
    [customersizecodename],
    [customertypecode],
    [customertypecodename],
    [defaultpricelevelid],
    [defaultpriceleveliddsc],
    [defaultpricelevelidname],
    [description],
    [donotbulkemail],
    [donotbulkemailname],
    [donotbulkpostalmail],
    [donotbulkpostalmailname],
    [donotemail],
    [donotemailname],
    [donotfax],
    [donotfaxname],
    [donotphone],
    [donotphonename],
    [donotpostalmail],
    [donotpostalmailname],
    [donotsendmarketingmaterialname],
    [donotsendmm],
    [emailaddress1],
    [emailaddress2],
    [emailaddress3],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [fax],
    [ftpsiteurl],
    [importsequencenumber],
    [industrycode],
    [industrycodename],
    [isprivatename],
    [lastusedincampaign],
    [lastusedincampaignutc],
    [marketcap],
    [marketcap_base],
    [masteraccountiddsc],
    [masteraccountidname],
    [masteraccountidyominame],
    [masterid],
    [merged],
    [mergedname],
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
    [numberofemployees],
    [originatingleadid],
    [originatingleadiddsc],
    [originatingleadidname],
    [originatingleadidyominame],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [ownershipcode],
    [ownershipcodename],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [parentaccountid],
    [parentaccountiddsc],
    [parentaccountidname],
    [parentaccountidyominame],
    [participatesinworkflow],
    [participatesinworkflowname],
    [paymenttermscode],
    [paymenttermscodename],
    [preferredappointmentdaycode],
    [preferredappointmentdaycodename],
    [preferredappointmenttimecode],
    [preferredappointmenttimecodename],
    [preferredcontactmethodcode],
    [preferredcontactmethodcodename],
    [preferredequipmentid],
    [preferredequipmentiddsc],
    [preferredequipmentidname],
    [preferredserviceid],
    [preferredserviceiddsc],
    [preferredserviceidname],
    [preferredsystemuserid],
    [preferredsystemuseriddsc],
    [preferredsystemuseridname],
    [preferredsystemuseridyominame],
    [primarycontactid],
    [primarycontactiddsc],
    [primarycontactidname],
    [primarycontactidyominame],
    [processid],
    [revenue],
    [revenue_base],
    [sharesoutstanding],
    [shippingmethodcode],
    [shippingmethodcodename],
    [sic],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [stockexchange],
    [telephone1],
    [telephone2],
    [telephone3],
    [territorycode],
    [territorycodename],
    [territoryid],
    [territoryiddsc],
    [territoryidname],
    [tickersymbol],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    [websiteurl],
    [yominame],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [Account].[AccountCategoryCode],
    AccountCategoryCodePLTable.Value,
    [Account].[AccountClassificationCode],
    AccountClassificationCodePLTable.Value,
    [Account].[AccountId],
    [Account].[AccountNumber],
    [Account].[AccountRatingCode],
    AccountRatingCodePLTable.Value,
    [Account].[Address1_AddressId],
    [Account].[Address1_AddressTypeCode],
    Address1_AddressTypeCodePLTable.Value,
    [Account].[Address1_City],
    [Account].[Address1_Composite],
    [Account].[Address1_Country],
    [Account].[Address1_County],
    [Account].[Address1_Fax],
    [Account].[Address1_FreightTermsCode],
    Address1_FreightTermsCodePLTable.Value,
    [Account].[Address1_Latitude],
    [Account].[Address1_Line1],
    [Account].[Address1_Line2],
    [Account].[Address1_Line3],
    [Account].[Address1_Longitude],
    [Account].[Address1_Name],
    [Account].[Address1_PostalCode],
    [Account].[Address1_PostOfficeBox],
    [Account].[Address1_PrimaryContactName],
    [Account].[Address1_ShippingMethodCode],
    Address1_ShippingMethodCodePLTable.Value,
    [Account].[Address1_StateOrProvince],
    [Account].[Address1_Telephone1],
    [Account].[Address1_Telephone2],
    [Account].[Address1_Telephone3],
    [Account].[Address1_UPSZone],
    [Account].[Address1_UTCOffset],
    [Account].[Address2_AddressId],
    [Account].[Address2_AddressTypeCode],
    Address2_AddressTypeCodePLTable.Value,
    [Account].[Address2_City],
    [Account].[Address2_Composite],
    [Account].[Address2_Country],
    [Account].[Address2_County],
    [Account].[Address2_Fax],
    [Account].[Address2_FreightTermsCode],
    Address2_FreightTermsCodePLTable.Value,
    [Account].[Address2_Latitude],
    [Account].[Address2_Line1],
    [Account].[Address2_Line2],
    [Account].[Address2_Line3],
    [Account].[Address2_Longitude],
    [Account].[Address2_Name],
    [Account].[Address2_PostalCode],
    [Account].[Address2_PostOfficeBox],
    [Account].[Address2_PrimaryContactName],
    [Account].[Address2_ShippingMethodCode],
    Address2_ShippingMethodCodePLTable.Value,
    [Account].[Address2_StateOrProvince],
    [Account].[Address2_Telephone1],
    [Account].[Address2_Telephone2],
    [Account].[Address2_Telephone3],
    [Account].[Address2_UPSZone],
    [Account].[Address2_UTCOffset],
    [Account].[Aging30],
    [Account].[Aging30_Base],
    [Account].[Aging60],
    [Account].[Aging60_Base],
    [Account].[Aging90],
    [Account].[Aging90_Base],
    [Account].[BusinessTypeCode],
    BusinessTypeCodePLTable.Value,
    [Account].[cgi_activated],
    cgi_activatedPLTable.Value,
    [Account].[cgi_addressid],
    [Account].[cgi_allow_autoload],
    cgi_allow_autoloadPLTable.Value,
    [Account].[cgi_counterpart],
    [Account].[cgi_CustomerType],
    cgi_CustomerTypePLTable.Value,
    [Account].[cgi_DebtCollection],
    cgi_DebtCollectionPLTable.Value,
    [Account].[cgi_FirstName],
    [Account].[cgi_hasepiserveraccount],
    cgi_hasepiserveraccountPLTable.Value,
    [Account].[cgi_ImportID],
    [Account].[cgi_LastName],
    [Account].[cgi_max_cards_autoload],
    [Account].[cgi_Membership],
    cgi_MembershipPLTable.Value,
    [Account].[cgi_MyAccount],
    cgi_MyAccountPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Account].[cgi_MyAccount_LastLogin], 
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
        [Account].[cgi_MyAccount_LastLogin],
    [Account].[cgi_NewsLetter],
    cgi_NewsLetterPLTable.Value,
    [Account].[cgi_organizational_number],
    [Account].[cgi_organization_sub_number],
    [Account].[cgi_responsibility],
    [Account].[cgi_rsid],
    case when ([paamcgi_socialsecuritynumber].ReadAccess = 4 or [poaacgi_socialsecuritynumber].ReadAccess = 1) then [Account].[cgi_SocialSecurityNumber] else null end,
    [Account].[CreatedBy],
    --[Account].[CreatedByDsc]
    0,
    [Account].[CreatedByName],
    [Account].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Account].[CreatedOn], 
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
        [Account].[CreatedOn],
    [Account].[CreatedOnBehalfBy],
    --[Account].[CreatedOnBehalfByDsc]
    0,
    [Account].[CreatedOnBehalfByName],
    [Account].[CreatedOnBehalfByYomiName],
    [Account].[CreditLimit],
    [Account].[CreditLimit_Base],
    [Account].[CreditOnHold],
    CreditOnHoldPLTable.Value,
    [Account].[CustomerSizeCode],
    CustomerSizeCodePLTable.Value,
    [Account].[CustomerTypeCode],
    CustomerTypeCodePLTable.Value,
    [Account].[DefaultPriceLevelId],
    --[Account].[DefaultPriceLevelIdDsc]
    0,
    [Account].[DefaultPriceLevelIdName],
    [Account].[Description],
    [Account].[DoNotBulkEMail],
    DoNotBulkEMailPLTable.Value,
    [Account].[DoNotBulkPostalMail],
    DoNotBulkPostalMailPLTable.Value,
    [Account].[DoNotEMail],
    DoNotEMailPLTable.Value,
    [Account].[DoNotFax],
    DoNotFaxPLTable.Value,
    [Account].[DoNotPhone],
    DoNotPhonePLTable.Value,
    [Account].[DoNotPostalMail],
    DoNotPostalMailPLTable.Value,
    DoNotSendMMPLTable.Value,
    [Account].[DoNotSendMM],
    [Account].[EMailAddress1],
    [Account].[EMailAddress2],
    [Account].[EMailAddress3],
    --[Account].[EntityImage]
    cast(null as varbinary),
    [Account].[EntityImageId],
    [Account].[EntityImage_Timestamp],
    [Account].[EntityImage_URL],
    [Account].[ExchangeRate],
    [Account].[Fax],
    [Account].[FtpSiteURL],
    [Account].[ImportSequenceNumber],
    [Account].[IndustryCode],
    IndustryCodePLTable.Value,
    IsPrivatePLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Account].[LastUsedInCampaign], 
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
        [Account].[LastUsedInCampaign],
    [Account].[MarketCap],
    [Account].[MarketCap_Base],
    --[Account].[MasterAccountIdDsc]
    0,
    [Account].[MasterAccountIdName],
    [Account].[MasterAccountIdYomiName],
    [Account].[MasterId],
    [Account].[Merged],
    MergedPLTable.Value,
    [Account].[ModifiedBy],
    --[Account].[ModifiedByDsc]
    0,
    [Account].[ModifiedByName],
    [Account].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Account].[ModifiedOn], 
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
        [Account].[ModifiedOn],
    [Account].[ModifiedOnBehalfBy],
    --[Account].[ModifiedOnBehalfByDsc]
    0,
    [Account].[ModifiedOnBehalfByName],
    [Account].[ModifiedOnBehalfByYomiName],
    [Account].[Name],
    [Account].[NumberOfEmployees],
    [Account].[OriginatingLeadId],
    --[Account].[OriginatingLeadIdDsc]
    0,
    [Account].[OriginatingLeadIdName],
    [Account].[OriginatingLeadIdYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Account].[OverriddenCreatedOn], 
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
        [Account].[OverriddenCreatedOn],
    [Account].[OwnerId],
    --[Account].[OwnerIdDsc]
    0,
    [Account].[OwnerIdName],
    [Account].[OwnerIdType],
    [Account].[OwnerIdYomiName],
    [Account].[OwnershipCode],
    OwnershipCodePLTable.Value,
    [Account].[OwningBusinessUnit],
    [Account].[OwningTeam],
    [Account].[OwningUser],
    [Account].[ParentAccountId],
    --[Account].[ParentAccountIdDsc]
    0,
    [Account].[ParentAccountIdName],
    [Account].[ParentAccountIdYomiName],
    [Account].[ParticipatesInWorkflow],
    ParticipatesInWorkflowPLTable.Value,
    [Account].[PaymentTermsCode],
    PaymentTermsCodePLTable.Value,
    [Account].[PreferredAppointmentDayCode],
    PreferredAppointmentDayCodePLTable.Value,
    [Account].[PreferredAppointmentTimeCode],
    PreferredAppointmentTimeCodePLTable.Value,
    [Account].[PreferredContactMethodCode],
    PreferredContactMethodCodePLTable.Value,
    [Account].[PreferredEquipmentId],
    --[Account].[PreferredEquipmentIdDsc]
    0,
    [Account].[PreferredEquipmentIdName],
    [Account].[PreferredServiceId],
    --[Account].[PreferredServiceIdDsc]
    0,
    [Account].[PreferredServiceIdName],
    [Account].[PreferredSystemUserId],
    --[Account].[PreferredSystemUserIdDsc]
    0,
    [Account].[PreferredSystemUserIdName],
    [Account].[PreferredSystemUserIdYomiName],
    [Account].[PrimaryContactId],
    --[Account].[PrimaryContactIdDsc]
    0,
    [Account].[PrimaryContactIdName],
    [Account].[PrimaryContactIdYomiName],
    [Account].[ProcessId],
    [Account].[Revenue],
    [Account].[Revenue_Base],
    [Account].[SharesOutstanding],
    [Account].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [Account].[SIC],
    [Account].[StageId],
    [Account].[StateCode],
    StateCodePLTable.Value,
    [Account].[StatusCode],
    StatusCodePLTable.Value,
    [Account].[StockExchange],
    [Account].[Telephone1],
    [Account].[Telephone2],
    [Account].[Telephone3],
    [Account].[TerritoryCode],
    TerritoryCodePLTable.Value,
    [Account].[TerritoryId],
    --[Account].[TerritoryIdDsc]
    0,
    [Account].[TerritoryIdName],
    [Account].[TickerSymbol],
    [Account].[TimeZoneRuleVersionNumber],
    [Account].[TransactionCurrencyId],
    --[Account].[TransactionCurrencyIdDsc]
    0,
    [Account].[TransactionCurrencyIdName],
    [Account].[UTCConversionTimeZoneCode],
    [Account].[VersionNumber],
    [Account].[WebSiteURL],
    [Account].[YomiName],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from Account
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [Account].TransactionCurrencyId
    left outer join StringMap [AccountCategoryCodePLTable] on 
		([AccountCategoryCodePLTable].AttributeName = 'accountcategorycode'
		and [AccountCategoryCodePLTable].ObjectTypeCode = 1
		and [AccountCategoryCodePLTable].AttributeValue = [Account].[AccountCategoryCode]
		and [AccountCategoryCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [AccountClassificationCodePLTable] on 
		([AccountClassificationCodePLTable].AttributeName = 'accountclassificationcode'
		and [AccountClassificationCodePLTable].ObjectTypeCode = 1
		and [AccountClassificationCodePLTable].AttributeValue = [Account].[AccountClassificationCode]
		and [AccountClassificationCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [AccountRatingCodePLTable] on 
		([AccountRatingCodePLTable].AttributeName = 'accountratingcode'
		and [AccountRatingCodePLTable].ObjectTypeCode = 1
		and [AccountRatingCodePLTable].AttributeValue = [Account].[AccountRatingCode]
		and [AccountRatingCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_AddressTypeCodePLTable] on 
		([Address1_AddressTypeCodePLTable].AttributeName = 'address1_addresstypecode'
		and [Address1_AddressTypeCodePLTable].ObjectTypeCode = 1
		and [Address1_AddressTypeCodePLTable].AttributeValue = [Account].[Address1_AddressTypeCode]
		and [Address1_AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_FreightTermsCodePLTable] on 
		([Address1_FreightTermsCodePLTable].AttributeName = 'address1_freighttermscode'
		and [Address1_FreightTermsCodePLTable].ObjectTypeCode = 1
		and [Address1_FreightTermsCodePLTable].AttributeValue = [Account].[Address1_FreightTermsCode]
		and [Address1_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_ShippingMethodCodePLTable] on 
		([Address1_ShippingMethodCodePLTable].AttributeName = 'address1_shippingmethodcode'
		and [Address1_ShippingMethodCodePLTable].ObjectTypeCode = 1
		and [Address1_ShippingMethodCodePLTable].AttributeValue = [Account].[Address1_ShippingMethodCode]
		and [Address1_ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_AddressTypeCodePLTable] on 
		([Address2_AddressTypeCodePLTable].AttributeName = 'address2_addresstypecode'
		and [Address2_AddressTypeCodePLTable].ObjectTypeCode = 1
		and [Address2_AddressTypeCodePLTable].AttributeValue = [Account].[Address2_AddressTypeCode]
		and [Address2_AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_FreightTermsCodePLTable] on 
		([Address2_FreightTermsCodePLTable].AttributeName = 'address2_freighttermscode'
		and [Address2_FreightTermsCodePLTable].ObjectTypeCode = 1
		and [Address2_FreightTermsCodePLTable].AttributeValue = [Account].[Address2_FreightTermsCode]
		and [Address2_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_ShippingMethodCodePLTable] on 
		([Address2_ShippingMethodCodePLTable].AttributeName = 'address2_shippingmethodcode'
		and [Address2_ShippingMethodCodePLTable].ObjectTypeCode = 1
		and [Address2_ShippingMethodCodePLTable].AttributeValue = [Account].[Address2_ShippingMethodCode]
		and [Address2_ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [BusinessTypeCodePLTable] on 
		([BusinessTypeCodePLTable].AttributeName = 'businesstypecode'
		and [BusinessTypeCodePLTable].ObjectTypeCode = 1
		and [BusinessTypeCodePLTable].AttributeValue = [Account].[BusinessTypeCode]
		and [BusinessTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_activatedPLTable] on 
		([cgi_activatedPLTable].AttributeName = 'cgi_activated'
		and [cgi_activatedPLTable].ObjectTypeCode = 1
		and [cgi_activatedPLTable].AttributeValue = [Account].[cgi_activated]
		and [cgi_activatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_allow_autoloadPLTable] on 
		([cgi_allow_autoloadPLTable].AttributeName = 'cgi_allow_autoload'
		and [cgi_allow_autoloadPLTable].ObjectTypeCode = 1
		and [cgi_allow_autoloadPLTable].AttributeValue = [Account].[cgi_allow_autoload]
		and [cgi_allow_autoloadPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_CustomerTypePLTable] on 
		([cgi_CustomerTypePLTable].AttributeName = 'cgi_customertype'
		and [cgi_CustomerTypePLTable].ObjectTypeCode = 1
		and [cgi_CustomerTypePLTable].AttributeValue = [Account].[cgi_CustomerType]
		and [cgi_CustomerTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_DebtCollectionPLTable] on 
		([cgi_DebtCollectionPLTable].AttributeName = 'cgi_debtcollection'
		and [cgi_DebtCollectionPLTable].ObjectTypeCode = 1
		and [cgi_DebtCollectionPLTable].AttributeValue = [Account].[cgi_DebtCollection]
		and [cgi_DebtCollectionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_hasepiserveraccountPLTable] on 
		([cgi_hasepiserveraccountPLTable].AttributeName = 'cgi_hasepiserveraccount'
		and [cgi_hasepiserveraccountPLTable].ObjectTypeCode = 1
		and [cgi_hasepiserveraccountPLTable].AttributeValue = [Account].[cgi_hasepiserveraccount]
		and [cgi_hasepiserveraccountPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_MembershipPLTable] on 
		([cgi_MembershipPLTable].AttributeName = 'cgi_membership'
		and [cgi_MembershipPLTable].ObjectTypeCode = 1
		and [cgi_MembershipPLTable].AttributeValue = [Account].[cgi_Membership]
		and [cgi_MembershipPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_MyAccountPLTable] on 
		([cgi_MyAccountPLTable].AttributeName = 'cgi_myaccount'
		and [cgi_MyAccountPLTable].ObjectTypeCode = 1
		and [cgi_MyAccountPLTable].AttributeValue = [Account].[cgi_MyAccount]
		and [cgi_MyAccountPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_NewsLetterPLTable] on 
		([cgi_NewsLetterPLTable].AttributeName = 'cgi_newsletter'
		and [cgi_NewsLetterPLTable].ObjectTypeCode = 1
		and [cgi_NewsLetterPLTable].AttributeValue = [Account].[cgi_NewsLetter]
		and [cgi_NewsLetterPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CreditOnHoldPLTable] on 
		([CreditOnHoldPLTable].AttributeName = 'creditonhold'
		and [CreditOnHoldPLTable].ObjectTypeCode = 1
		and [CreditOnHoldPLTable].AttributeValue = [Account].[CreditOnHold]
		and [CreditOnHoldPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CustomerSizeCodePLTable] on 
		([CustomerSizeCodePLTable].AttributeName = 'customersizecode'
		and [CustomerSizeCodePLTable].ObjectTypeCode = 1
		and [CustomerSizeCodePLTable].AttributeValue = [Account].[CustomerSizeCode]
		and [CustomerSizeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CustomerTypeCodePLTable] on 
		([CustomerTypeCodePLTable].AttributeName = 'customertypecode'
		and [CustomerTypeCodePLTable].ObjectTypeCode = 1
		and [CustomerTypeCodePLTable].AttributeValue = [Account].[CustomerTypeCode]
		and [CustomerTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotBulkEMailPLTable] on 
		([DoNotBulkEMailPLTable].AttributeName = 'donotbulkemail'
		and [DoNotBulkEMailPLTable].ObjectTypeCode = 1
		and [DoNotBulkEMailPLTable].AttributeValue = [Account].[DoNotBulkEMail]
		and [DoNotBulkEMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotBulkPostalMailPLTable] on 
		([DoNotBulkPostalMailPLTable].AttributeName = 'donotbulkpostalmail'
		and [DoNotBulkPostalMailPLTable].ObjectTypeCode = 1
		and [DoNotBulkPostalMailPLTable].AttributeValue = [Account].[DoNotBulkPostalMail]
		and [DoNotBulkPostalMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotEMailPLTable] on 
		([DoNotEMailPLTable].AttributeName = 'donotemail'
		and [DoNotEMailPLTable].ObjectTypeCode = 1
		and [DoNotEMailPLTable].AttributeValue = [Account].[DoNotEMail]
		and [DoNotEMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotFaxPLTable] on 
		([DoNotFaxPLTable].AttributeName = 'donotfax'
		and [DoNotFaxPLTable].ObjectTypeCode = 1
		and [DoNotFaxPLTable].AttributeValue = [Account].[DoNotFax]
		and [DoNotFaxPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotPhonePLTable] on 
		([DoNotPhonePLTable].AttributeName = 'donotphone'
		and [DoNotPhonePLTable].ObjectTypeCode = 1
		and [DoNotPhonePLTable].AttributeValue = [Account].[DoNotPhone]
		and [DoNotPhonePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotPostalMailPLTable] on 
		([DoNotPostalMailPLTable].AttributeName = 'donotpostalmail'
		and [DoNotPostalMailPLTable].ObjectTypeCode = 1
		and [DoNotPostalMailPLTable].AttributeValue = [Account].[DoNotPostalMail]
		and [DoNotPostalMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotSendMMPLTable] on 
		([DoNotSendMMPLTable].AttributeName = 'donotsendmm'
		and [DoNotSendMMPLTable].ObjectTypeCode = 1
		and [DoNotSendMMPLTable].AttributeValue = [Account].[DoNotSendMM]
		and [DoNotSendMMPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IndustryCodePLTable] on 
		([IndustryCodePLTable].AttributeName = 'industrycode'
		and [IndustryCodePLTable].ObjectTypeCode = 1
		and [IndustryCodePLTable].AttributeValue = [Account].[IndustryCode]
		and [IndustryCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsPrivatePLTable] on 
		([IsPrivatePLTable].AttributeName = 'isprivate'
		and [IsPrivatePLTable].ObjectTypeCode = 1
		and [IsPrivatePLTable].AttributeValue = [Account].[IsPrivate]
		and [IsPrivatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [MergedPLTable] on 
		([MergedPLTable].AttributeName = 'merged'
		and [MergedPLTable].ObjectTypeCode = 1
		and [MergedPLTable].AttributeValue = [Account].[Merged]
		and [MergedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [OwnershipCodePLTable] on 
		([OwnershipCodePLTable].AttributeName = 'ownershipcode'
		and [OwnershipCodePLTable].ObjectTypeCode = 1
		and [OwnershipCodePLTable].AttributeValue = [Account].[OwnershipCode]
		and [OwnershipCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ParticipatesInWorkflowPLTable] on 
		([ParticipatesInWorkflowPLTable].AttributeName = 'participatesinworkflow'
		and [ParticipatesInWorkflowPLTable].ObjectTypeCode = 1
		and [ParticipatesInWorkflowPLTable].AttributeValue = [Account].[ParticipatesInWorkflow]
		and [ParticipatesInWorkflowPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PaymentTermsCodePLTable] on 
		([PaymentTermsCodePLTable].AttributeName = 'paymenttermscode'
		and [PaymentTermsCodePLTable].ObjectTypeCode = 1
		and [PaymentTermsCodePLTable].AttributeValue = [Account].[PaymentTermsCode]
		and [PaymentTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredAppointmentDayCodePLTable] on 
		([PreferredAppointmentDayCodePLTable].AttributeName = 'preferredappointmentdaycode'
		and [PreferredAppointmentDayCodePLTable].ObjectTypeCode = 1
		and [PreferredAppointmentDayCodePLTable].AttributeValue = [Account].[PreferredAppointmentDayCode]
		and [PreferredAppointmentDayCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredAppointmentTimeCodePLTable] on 
		([PreferredAppointmentTimeCodePLTable].AttributeName = 'preferredappointmenttimecode'
		and [PreferredAppointmentTimeCodePLTable].ObjectTypeCode = 1
		and [PreferredAppointmentTimeCodePLTable].AttributeValue = [Account].[PreferredAppointmentTimeCode]
		and [PreferredAppointmentTimeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredContactMethodCodePLTable] on 
		([PreferredContactMethodCodePLTable].AttributeName = 'preferredcontactmethodcode'
		and [PreferredContactMethodCodePLTable].ObjectTypeCode = 1
		and [PreferredContactMethodCodePLTable].AttributeValue = [Account].[PreferredContactMethodCode]
		and [PreferredContactMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 1
		and [ShippingMethodCodePLTable].AttributeValue = [Account].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1
		and [StateCodePLTable].AttributeValue = [Account].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1
		and [StatusCodePLTable].AttributeValue = [Account].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TerritoryCodePLTable] on 
		([TerritoryCodePLTable].AttributeName = 'territorycode'
		and [TerritoryCodePLTable].ObjectTypeCode = 1
		and [TerritoryCodePLTable].AttributeValue = [Account].[TerritoryCode]
		and [TerritoryCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join PrincipalAttributeAccessMap [paamcgi_socialsecuritynumber] on 
		([paamcgi_socialsecuritynumber].AttributeId = '9fc44322-74cb-44c0-899f-f8e458927962' -- cgi_socialsecuritynumber
		and [paamcgi_socialsecuritynumber].PrincipalId = u.SystemUserId)
	left outer join dbo.fn_UserSharedAttributeAccess(dbo.fn_FindUserGuid(),
		'9fc44322-74cb-44c0-899f-f8e458927962', -- cgi_socialsecuritynumber
		1) [poaacgi_socialsecuritynumber] on
		([poaacgi_socialsecuritynumber].ObjectId = [Account].[AccountId])

    cross join dbo.fn_GetMaxPrivilegeDepthMask(1) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Account].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 1
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
		[Account].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 1)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Account].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Account].[AccountId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 1 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

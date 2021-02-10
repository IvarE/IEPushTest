

--
-- report view for contact
--
create view dbo.[FilteredContact] (
    [accountid],
    [accountiddsc],
    [accountidname],
    [accountidyominame],
    [accountrolecode],
    [accountrolecodename],
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
    [anniversary],
    [anniversaryutc],
    [annualincome],
    [annualincome_base],
    [assistantname],
    [assistantphone],
    [birthdate],
    [birthdateutc],
    [cgi_activated],
    [cgi_activatedname],
    [cgi_addressid],
    [cgi_allow_autoload],
    [cgi_allow_autoloadname],
    [cgi_contactnumber],
    [cgi_counterpart],
    [cgi_debtcollection],
    [cgi_debtcollectionname],
    [cgi_hasepiserveraccount],
    [cgi_hasepiserveraccountname],
    [cgi_importid],
    [cgi_isprimaryaddress],
    [cgi_isprimaryaddressname],
    [cgi_max_cards_autoload],
    [cgi_membership],
    [cgi_membershipname],
    [cgi_myaccount],
    [cgi_myaccountname],
    [cgi_myaccount_lastlogin],
    [cgi_myaccount_lastloginutc],
    [cgi_newsletter],
    [cgi_newslettername],
    [cgi_representativeid],
    [cgi_representativeidname],
    [cgi_responsibility],
    [cgi_rsid],
    [cgi_socialsecuritynumber],
    [childrensnames],
    [contactid],
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
    [department],
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
    [educationcode],
    [educationcodename],
    [emailaddress1],
    [emailaddress2],
    [emailaddress3],
    [employeeid],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [externaluseridentifier],
    [familystatuscode],
    [familystatuscodename],
    [fax],
    [firstname],
    [ftpsiteurl],
    [fullname],
    [gendercode],
    [gendercodename],
    [governmentid],
    [haschildrencode],
    [haschildrencodename],
    [importsequencenumber],
    [isbackofficecustomer],
    [isbackofficecustomername],
    [isprivatename],
    [jobtitle],
    [lastname],
    [lastusedincampaign],
    [lastusedincampaignutc],
    [leadsourcecode],
    [leadsourcecodename],
    [managername],
    [managerphone],
    [mastercontactiddsc],
    [mastercontactidname],
    [mastercontactidyominame],
    [masterid],
    [merged],
    [mergedname],
    [middlename],
    [mobilephone],
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
    [nickname],
    [numberofchildren],
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
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [pager],
    [parentcontactid],
    [parentcontactiddsc],
    [parentcontactidname],
    [parentcontactidyominame],
    [parentcustomerid],
    [parentcustomeriddsc],
    [parentcustomeridname],
    [parentcustomeridtype],
    [parentcustomeridyominame],
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
    [processid],
    [salutation],
    [shippingmethodcode],
    [shippingmethodcodename],
    [spousesname],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [st_myaccount_username],
    [suffix],
    [telephone1],
    [telephone2],
    [telephone3],
    [territorycode],
    [territorycodename],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    [websiteurl],
    [yomifirstname],
    [yomifullname],
    [yomilastname],
    [yomimiddlename],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [Contact].[AccountId],
    --[Contact].[AccountIdDsc]
    0,
    [Contact].[AccountIdName],
    [Contact].[AccountIdYomiName],
    [Contact].[AccountRoleCode],
    AccountRoleCodePLTable.Value,
    [Contact].[Address1_AddressId],
    [Contact].[Address1_AddressTypeCode],
    Address1_AddressTypeCodePLTable.Value,
    [Contact].[Address1_City],
    [Contact].[Address1_Composite],
    [Contact].[Address1_Country],
    [Contact].[Address1_County],
    [Contact].[Address1_Fax],
    [Contact].[Address1_FreightTermsCode],
    Address1_FreightTermsCodePLTable.Value,
    [Contact].[Address1_Latitude],
    [Contact].[Address1_Line1],
    [Contact].[Address1_Line2],
    [Contact].[Address1_Line3],
    [Contact].[Address1_Longitude],
    [Contact].[Address1_Name],
    [Contact].[Address1_PostalCode],
    [Contact].[Address1_PostOfficeBox],
    [Contact].[Address1_PrimaryContactName],
    [Contact].[Address1_ShippingMethodCode],
    Address1_ShippingMethodCodePLTable.Value,
    [Contact].[Address1_StateOrProvince],
    [Contact].[Address1_Telephone1],
    [Contact].[Address1_Telephone2],
    [Contact].[Address1_Telephone3],
    [Contact].[Address1_UPSZone],
    [Contact].[Address1_UTCOffset],
    [Contact].[Address2_AddressId],
    [Contact].[Address2_AddressTypeCode],
    Address2_AddressTypeCodePLTable.Value,
    [Contact].[Address2_City],
    [Contact].[Address2_Composite],
    [Contact].[Address2_Country],
    [Contact].[Address2_County],
    [Contact].[Address2_Fax],
    [Contact].[Address2_FreightTermsCode],
    Address2_FreightTermsCodePLTable.Value,
    [Contact].[Address2_Latitude],
    [Contact].[Address2_Line1],
    [Contact].[Address2_Line2],
    [Contact].[Address2_Line3],
    [Contact].[Address2_Longitude],
    [Contact].[Address2_Name],
    [Contact].[Address2_PostalCode],
    [Contact].[Address2_PostOfficeBox],
    [Contact].[Address2_PrimaryContactName],
    [Contact].[Address2_ShippingMethodCode],
    Address2_ShippingMethodCodePLTable.Value,
    [Contact].[Address2_StateOrProvince],
    [Contact].[Address2_Telephone1],
    [Contact].[Address2_Telephone2],
    [Contact].[Address2_Telephone3],
    [Contact].[Address2_UPSZone],
    [Contact].[Address2_UTCOffset],
    [Contact].[Aging30],
    [Contact].[Aging30_Base],
    [Contact].[Aging60],
    [Contact].[Aging60_Base],
    [Contact].[Aging90],
    [Contact].[Aging90_Base],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[Anniversary], 
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
        [Contact].[Anniversary],
    [Contact].[AnnualIncome],
    [Contact].[AnnualIncome_Base],
    [Contact].[AssistantName],
    [Contact].[AssistantPhone],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[BirthDate], 
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
        [Contact].[BirthDate],
    [Contact].[cgi_activated],
    cgi_activatedPLTable.Value,
    [Contact].[cgi_addressid],
    [Contact].[cgi_allow_autoload],
    cgi_allow_autoloadPLTable.Value,
    [Contact].[cgi_ContactNumber],
    [Contact].[cgi_counterpart],
    [Contact].[cgi_DebtCollection],
    cgi_DebtCollectionPLTable.Value,
    [Contact].[cgi_HasEpiserverAccount],
    cgi_HasEpiserverAccountPLTable.Value,
    [Contact].[cgi_importid],
    [Contact].[cgi_IsPrimaryAddress],
    cgi_IsPrimaryAddressPLTable.Value,
    [Contact].[cgi_Max_Cards_AutoLoad],
    [Contact].[cgi_Membership],
    cgi_MembershipPLTable.Value,
    [Contact].[cgi_MyAccount],
    cgi_MyAccountPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[cgi_MyAccount_LastLogin], 
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
        [Contact].[cgi_MyAccount_LastLogin],
    [Contact].[cgi_Newsletter],
    cgi_NewsletterPLTable.Value,
    [Contact].[cgi_Representativeid],
    [Contact].[cgi_RepresentativeidName],
    [Contact].[cgi_Responsibility],
    [Contact].[cgi_rsid],
    [Contact].[cgi_socialsecuritynumber],
    [Contact].[ChildrensNames],
    [Contact].[ContactId],
    [Contact].[CreatedBy],
    --[Contact].[CreatedByDsc]
    0,
    [Contact].[CreatedByName],
    [Contact].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[CreatedOn], 
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
        [Contact].[CreatedOn],
    [Contact].[CreatedOnBehalfBy],
    --[Contact].[CreatedOnBehalfByDsc]
    0,
    [Contact].[CreatedOnBehalfByName],
    [Contact].[CreatedOnBehalfByYomiName],
    [Contact].[CreditLimit],
    [Contact].[CreditLimit_Base],
    [Contact].[CreditOnHold],
    CreditOnHoldPLTable.Value,
    [Contact].[CustomerSizeCode],
    CustomerSizeCodePLTable.Value,
    [Contact].[CustomerTypeCode],
    CustomerTypeCodePLTable.Value,
    [Contact].[DefaultPriceLevelId],
    --[Contact].[DefaultPriceLevelIdDsc]
    0,
    [Contact].[DefaultPriceLevelIdName],
    [Contact].[Department],
    [Contact].[Description],
    [Contact].[DoNotBulkEMail],
    DoNotBulkEMailPLTable.Value,
    [Contact].[DoNotBulkPostalMail],
    DoNotBulkPostalMailPLTable.Value,
    [Contact].[DoNotEMail],
    DoNotEMailPLTable.Value,
    [Contact].[DoNotFax],
    DoNotFaxPLTable.Value,
    [Contact].[DoNotPhone],
    DoNotPhonePLTable.Value,
    [Contact].[DoNotPostalMail],
    DoNotPostalMailPLTable.Value,
    DoNotSendMMPLTable.Value,
    [Contact].[DoNotSendMM],
    [Contact].[EducationCode],
    EducationCodePLTable.Value,
    [Contact].[EMailAddress1],
    [Contact].[EMailAddress2],
    [Contact].[EMailAddress3],
    [Contact].[EmployeeId],
    --[Contact].[EntityImage]
    cast(null as varbinary),
    [Contact].[EntityImageId],
    [Contact].[EntityImage_Timestamp],
    [Contact].[EntityImage_URL],
    [Contact].[ExchangeRate],
    [Contact].[ExternalUserIdentifier],
    [Contact].[FamilyStatusCode],
    FamilyStatusCodePLTable.Value,
    [Contact].[Fax],
    [Contact].[FirstName],
    [Contact].[FtpSiteUrl],
    [Contact].[FullName],
    [Contact].[GenderCode],
    GenderCodePLTable.Value,
    [Contact].[GovernmentId],
    [Contact].[HasChildrenCode],
    HasChildrenCodePLTable.Value,
    [Contact].[ImportSequenceNumber],
    [Contact].[IsBackofficeCustomer],
    IsBackofficeCustomerPLTable.Value,
    IsPrivatePLTable.Value,
    [Contact].[JobTitle],
    [Contact].[LastName],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[LastUsedInCampaign], 
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
        [Contact].[LastUsedInCampaign],
    [Contact].[LeadSourceCode],
    LeadSourceCodePLTable.Value,
    [Contact].[ManagerName],
    [Contact].[ManagerPhone],
    --[Contact].[MasterContactIdDsc]
    0,
    [Contact].[MasterContactIdName],
    [Contact].[MasterContactIdYomiName],
    [Contact].[MasterId],
    [Contact].[Merged],
    MergedPLTable.Value,
    [Contact].[MiddleName],
    [Contact].[MobilePhone],
    [Contact].[ModifiedBy],
    --[Contact].[ModifiedByDsc]
    0,
    [Contact].[ModifiedByName],
    [Contact].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[ModifiedOn], 
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
        [Contact].[ModifiedOn],
    [Contact].[ModifiedOnBehalfBy],
    --[Contact].[ModifiedOnBehalfByDsc]
    0,
    [Contact].[ModifiedOnBehalfByName],
    [Contact].[ModifiedOnBehalfByYomiName],
    [Contact].[NickName],
    [Contact].[NumberOfChildren],
    [Contact].[OriginatingLeadId],
    --[Contact].[OriginatingLeadIdDsc]
    0,
    [Contact].[OriginatingLeadIdName],
    [Contact].[OriginatingLeadIdYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Contact].[OverriddenCreatedOn], 
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
        [Contact].[OverriddenCreatedOn],
    [Contact].[OwnerId],
    --[Contact].[OwnerIdDsc]
    0,
    [Contact].[OwnerIdName],
    [Contact].[OwnerIdType],
    [Contact].[OwnerIdYomiName],
    [Contact].[OwningBusinessUnit],
    [Contact].[OwningTeam],
    [Contact].[OwningUser],
    [Contact].[Pager],
    [Contact].[ParentContactId],
    --[Contact].[ParentContactIdDsc]
    0,
    [Contact].[ParentContactIdName],
    [Contact].[ParentContactIdYomiName],
    [Contact].[ParentCustomerId],
    --[Contact].[ParentCustomerIdDsc]
    0,
    [Contact].[ParentCustomerIdName],
    [Contact].[ParentCustomerIdType],
    [Contact].[ParentCustomerIdYomiName],
    [Contact].[ParticipatesInWorkflow],
    ParticipatesInWorkflowPLTable.Value,
    [Contact].[PaymentTermsCode],
    PaymentTermsCodePLTable.Value,
    [Contact].[PreferredAppointmentDayCode],
    PreferredAppointmentDayCodePLTable.Value,
    [Contact].[PreferredAppointmentTimeCode],
    PreferredAppointmentTimeCodePLTable.Value,
    [Contact].[PreferredContactMethodCode],
    PreferredContactMethodCodePLTable.Value,
    [Contact].[PreferredEquipmentId],
    --[Contact].[PreferredEquipmentIdDsc]
    0,
    [Contact].[PreferredEquipmentIdName],
    [Contact].[PreferredServiceId],
    --[Contact].[PreferredServiceIdDsc]
    0,
    [Contact].[PreferredServiceIdName],
    [Contact].[PreferredSystemUserId],
    --[Contact].[PreferredSystemUserIdDsc]
    0,
    [Contact].[PreferredSystemUserIdName],
    [Contact].[PreferredSystemUserIdYomiName],
    [Contact].[ProcessId],
    [Contact].[Salutation],
    [Contact].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [Contact].[SpousesName],
    [Contact].[StageId],
    [Contact].[StateCode],
    StateCodePLTable.Value,
    [Contact].[StatusCode],
    StatusCodePLTable.Value,
    [Contact].[st_MyAccount_UserName],
    [Contact].[Suffix],
    [Contact].[Telephone1],
    [Contact].[Telephone2],
    [Contact].[Telephone3],
    [Contact].[TerritoryCode],
    TerritoryCodePLTable.Value,
    [Contact].[TimeZoneRuleVersionNumber],
    [Contact].[TransactionCurrencyId],
    --[Contact].[TransactionCurrencyIdDsc]
    0,
    [Contact].[TransactionCurrencyIdName],
    [Contact].[UTCConversionTimeZoneCode],
    [Contact].[VersionNumber],
    [Contact].[WebSiteUrl],
    [Contact].[YomiFirstName],
    [Contact].[YomiFullName],
    [Contact].[YomiLastName],
    [Contact].[YomiMiddleName],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from Contact
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [Contact].TransactionCurrencyId
    left outer join StringMap [AccountRoleCodePLTable] on 
		([AccountRoleCodePLTable].AttributeName = 'accountrolecode'
		and [AccountRoleCodePLTable].ObjectTypeCode = 2
		and [AccountRoleCodePLTable].AttributeValue = [Contact].[AccountRoleCode]
		and [AccountRoleCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_AddressTypeCodePLTable] on 
		([Address1_AddressTypeCodePLTable].AttributeName = 'address1_addresstypecode'
		and [Address1_AddressTypeCodePLTable].ObjectTypeCode = 2
		and [Address1_AddressTypeCodePLTable].AttributeValue = [Contact].[Address1_AddressTypeCode]
		and [Address1_AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_FreightTermsCodePLTable] on 
		([Address1_FreightTermsCodePLTable].AttributeName = 'address1_freighttermscode'
		and [Address1_FreightTermsCodePLTable].ObjectTypeCode = 2
		and [Address1_FreightTermsCodePLTable].AttributeValue = [Contact].[Address1_FreightTermsCode]
		and [Address1_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address1_ShippingMethodCodePLTable] on 
		([Address1_ShippingMethodCodePLTable].AttributeName = 'address1_shippingmethodcode'
		and [Address1_ShippingMethodCodePLTable].ObjectTypeCode = 2
		and [Address1_ShippingMethodCodePLTable].AttributeValue = [Contact].[Address1_ShippingMethodCode]
		and [Address1_ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_AddressTypeCodePLTable] on 
		([Address2_AddressTypeCodePLTable].AttributeName = 'address2_addresstypecode'
		and [Address2_AddressTypeCodePLTable].ObjectTypeCode = 2
		and [Address2_AddressTypeCodePLTable].AttributeValue = [Contact].[Address2_AddressTypeCode]
		and [Address2_AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_FreightTermsCodePLTable] on 
		([Address2_FreightTermsCodePLTable].AttributeName = 'address2_freighttermscode'
		and [Address2_FreightTermsCodePLTable].ObjectTypeCode = 2
		and [Address2_FreightTermsCodePLTable].AttributeValue = [Contact].[Address2_FreightTermsCode]
		and [Address2_FreightTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Address2_ShippingMethodCodePLTable] on 
		([Address2_ShippingMethodCodePLTable].AttributeName = 'address2_shippingmethodcode'
		and [Address2_ShippingMethodCodePLTable].ObjectTypeCode = 2
		and [Address2_ShippingMethodCodePLTable].AttributeValue = [Contact].[Address2_ShippingMethodCode]
		and [Address2_ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_activatedPLTable] on 
		([cgi_activatedPLTable].AttributeName = 'cgi_activated'
		and [cgi_activatedPLTable].ObjectTypeCode = 2
		and [cgi_activatedPLTable].AttributeValue = [Contact].[cgi_activated]
		and [cgi_activatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_allow_autoloadPLTable] on 
		([cgi_allow_autoloadPLTable].AttributeName = 'cgi_allow_autoload'
		and [cgi_allow_autoloadPLTable].ObjectTypeCode = 2
		and [cgi_allow_autoloadPLTable].AttributeValue = [Contact].[cgi_allow_autoload]
		and [cgi_allow_autoloadPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_DebtCollectionPLTable] on 
		([cgi_DebtCollectionPLTable].AttributeName = 'cgi_debtcollection'
		and [cgi_DebtCollectionPLTable].ObjectTypeCode = 2
		and [cgi_DebtCollectionPLTable].AttributeValue = [Contact].[cgi_DebtCollection]
		and [cgi_DebtCollectionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_HasEpiserverAccountPLTable] on 
		([cgi_HasEpiserverAccountPLTable].AttributeName = 'cgi_hasepiserveraccount'
		and [cgi_HasEpiserverAccountPLTable].ObjectTypeCode = 2
		and [cgi_HasEpiserverAccountPLTable].AttributeValue = [Contact].[cgi_HasEpiserverAccount]
		and [cgi_HasEpiserverAccountPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_IsPrimaryAddressPLTable] on 
		([cgi_IsPrimaryAddressPLTable].AttributeName = 'cgi_isprimaryaddress'
		and [cgi_IsPrimaryAddressPLTable].ObjectTypeCode = 2
		and [cgi_IsPrimaryAddressPLTable].AttributeValue = [Contact].[cgi_IsPrimaryAddress]
		and [cgi_IsPrimaryAddressPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_MembershipPLTable] on 
		([cgi_MembershipPLTable].AttributeName = 'cgi_membership'
		and [cgi_MembershipPLTable].ObjectTypeCode = 2
		and [cgi_MembershipPLTable].AttributeValue = [Contact].[cgi_Membership]
		and [cgi_MembershipPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_MyAccountPLTable] on 
		([cgi_MyAccountPLTable].AttributeName = 'cgi_myaccount'
		and [cgi_MyAccountPLTable].ObjectTypeCode = 2
		and [cgi_MyAccountPLTable].AttributeValue = [Contact].[cgi_MyAccount]
		and [cgi_MyAccountPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_NewsletterPLTable] on 
		([cgi_NewsletterPLTable].AttributeName = 'cgi_newsletter'
		and [cgi_NewsletterPLTable].ObjectTypeCode = 2
		and [cgi_NewsletterPLTable].AttributeValue = [Contact].[cgi_Newsletter]
		and [cgi_NewsletterPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CreditOnHoldPLTable] on 
		([CreditOnHoldPLTable].AttributeName = 'creditonhold'
		and [CreditOnHoldPLTable].ObjectTypeCode = 2
		and [CreditOnHoldPLTable].AttributeValue = [Contact].[CreditOnHold]
		and [CreditOnHoldPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CustomerSizeCodePLTable] on 
		([CustomerSizeCodePLTable].AttributeName = 'customersizecode'
		and [CustomerSizeCodePLTable].ObjectTypeCode = 2
		and [CustomerSizeCodePLTable].AttributeValue = [Contact].[CustomerSizeCode]
		and [CustomerSizeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CustomerTypeCodePLTable] on 
		([CustomerTypeCodePLTable].AttributeName = 'customertypecode'
		and [CustomerTypeCodePLTable].ObjectTypeCode = 2
		and [CustomerTypeCodePLTable].AttributeValue = [Contact].[CustomerTypeCode]
		and [CustomerTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotBulkEMailPLTable] on 
		([DoNotBulkEMailPLTable].AttributeName = 'donotbulkemail'
		and [DoNotBulkEMailPLTable].ObjectTypeCode = 2
		and [DoNotBulkEMailPLTable].AttributeValue = [Contact].[DoNotBulkEMail]
		and [DoNotBulkEMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotBulkPostalMailPLTable] on 
		([DoNotBulkPostalMailPLTable].AttributeName = 'donotbulkpostalmail'
		and [DoNotBulkPostalMailPLTable].ObjectTypeCode = 2
		and [DoNotBulkPostalMailPLTable].AttributeValue = [Contact].[DoNotBulkPostalMail]
		and [DoNotBulkPostalMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotEMailPLTable] on 
		([DoNotEMailPLTable].AttributeName = 'donotemail'
		and [DoNotEMailPLTable].ObjectTypeCode = 2
		and [DoNotEMailPLTable].AttributeValue = [Contact].[DoNotEMail]
		and [DoNotEMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotFaxPLTable] on 
		([DoNotFaxPLTable].AttributeName = 'donotfax'
		and [DoNotFaxPLTable].ObjectTypeCode = 2
		and [DoNotFaxPLTable].AttributeValue = [Contact].[DoNotFax]
		and [DoNotFaxPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotPhonePLTable] on 
		([DoNotPhonePLTable].AttributeName = 'donotphone'
		and [DoNotPhonePLTable].ObjectTypeCode = 2
		and [DoNotPhonePLTable].AttributeValue = [Contact].[DoNotPhone]
		and [DoNotPhonePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotPostalMailPLTable] on 
		([DoNotPostalMailPLTable].AttributeName = 'donotpostalmail'
		and [DoNotPostalMailPLTable].ObjectTypeCode = 2
		and [DoNotPostalMailPLTable].AttributeValue = [Contact].[DoNotPostalMail]
		and [DoNotPostalMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotSendMMPLTable] on 
		([DoNotSendMMPLTable].AttributeName = 'donotsendmm'
		and [DoNotSendMMPLTable].ObjectTypeCode = 2
		and [DoNotSendMMPLTable].AttributeValue = [Contact].[DoNotSendMM]
		and [DoNotSendMMPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [EducationCodePLTable] on 
		([EducationCodePLTable].AttributeName = 'educationcode'
		and [EducationCodePLTable].ObjectTypeCode = 2
		and [EducationCodePLTable].AttributeValue = [Contact].[EducationCode]
		and [EducationCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [FamilyStatusCodePLTable] on 
		([FamilyStatusCodePLTable].AttributeName = 'familystatuscode'
		and [FamilyStatusCodePLTable].ObjectTypeCode = 2
		and [FamilyStatusCodePLTable].AttributeValue = [Contact].[FamilyStatusCode]
		and [FamilyStatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [GenderCodePLTable] on 
		([GenderCodePLTable].AttributeName = 'gendercode'
		and [GenderCodePLTable].ObjectTypeCode = 2
		and [GenderCodePLTable].AttributeValue = [Contact].[GenderCode]
		and [GenderCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [HasChildrenCodePLTable] on 
		([HasChildrenCodePLTable].AttributeName = 'haschildrencode'
		and [HasChildrenCodePLTable].ObjectTypeCode = 2
		and [HasChildrenCodePLTable].AttributeValue = [Contact].[HasChildrenCode]
		and [HasChildrenCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBackofficeCustomerPLTable] on 
		([IsBackofficeCustomerPLTable].AttributeName = 'isbackofficecustomer'
		and [IsBackofficeCustomerPLTable].ObjectTypeCode = 2
		and [IsBackofficeCustomerPLTable].AttributeValue = [Contact].[IsBackofficeCustomer]
		and [IsBackofficeCustomerPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsPrivatePLTable] on 
		([IsPrivatePLTable].AttributeName = 'isprivate'
		and [IsPrivatePLTable].ObjectTypeCode = 2
		and [IsPrivatePLTable].AttributeValue = [Contact].[IsPrivate]
		and [IsPrivatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [LeadSourceCodePLTable] on 
		([LeadSourceCodePLTable].AttributeName = 'leadsourcecode'
		and [LeadSourceCodePLTable].ObjectTypeCode = 2
		and [LeadSourceCodePLTable].AttributeValue = [Contact].[LeadSourceCode]
		and [LeadSourceCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [MergedPLTable] on 
		([MergedPLTable].AttributeName = 'merged'
		and [MergedPLTable].ObjectTypeCode = 2
		and [MergedPLTable].AttributeValue = [Contact].[Merged]
		and [MergedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ParticipatesInWorkflowPLTable] on 
		([ParticipatesInWorkflowPLTable].AttributeName = 'participatesinworkflow'
		and [ParticipatesInWorkflowPLTable].ObjectTypeCode = 2
		and [ParticipatesInWorkflowPLTable].AttributeValue = [Contact].[ParticipatesInWorkflow]
		and [ParticipatesInWorkflowPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PaymentTermsCodePLTable] on 
		([PaymentTermsCodePLTable].AttributeName = 'paymenttermscode'
		and [PaymentTermsCodePLTable].ObjectTypeCode = 2
		and [PaymentTermsCodePLTable].AttributeValue = [Contact].[PaymentTermsCode]
		and [PaymentTermsCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredAppointmentDayCodePLTable] on 
		([PreferredAppointmentDayCodePLTable].AttributeName = 'preferredappointmentdaycode'
		and [PreferredAppointmentDayCodePLTable].ObjectTypeCode = 2
		and [PreferredAppointmentDayCodePLTable].AttributeValue = [Contact].[PreferredAppointmentDayCode]
		and [PreferredAppointmentDayCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredAppointmentTimeCodePLTable] on 
		([PreferredAppointmentTimeCodePLTable].AttributeName = 'preferredappointmenttimecode'
		and [PreferredAppointmentTimeCodePLTable].ObjectTypeCode = 2
		and [PreferredAppointmentTimeCodePLTable].AttributeValue = [Contact].[PreferredAppointmentTimeCode]
		and [PreferredAppointmentTimeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PreferredContactMethodCodePLTable] on 
		([PreferredContactMethodCodePLTable].AttributeName = 'preferredcontactmethodcode'
		and [PreferredContactMethodCodePLTable].ObjectTypeCode = 2
		and [PreferredContactMethodCodePLTable].AttributeValue = [Contact].[PreferredContactMethodCode]
		and [PreferredContactMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 2
		and [ShippingMethodCodePLTable].AttributeValue = [Contact].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 2
		and [StateCodePLTable].AttributeValue = [Contact].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 2
		and [StatusCodePLTable].AttributeValue = [Contact].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TerritoryCodePLTable] on 
		([TerritoryCodePLTable].AttributeName = 'territorycode'
		and [TerritoryCodePLTable].ObjectTypeCode = 2
		and [TerritoryCodePLTable].AttributeValue = [Contact].[TerritoryCode]
		and [TerritoryCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(2) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Contact].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 2
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
		[Contact].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 2)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Contact].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Contact].[ContactId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 2 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)



--
-- report view for incident
--
create view dbo.[FilteredIncident] (
    [accountid],
    [accountiddsc],
    [accountidname],
    [accountidyominame],
    [activitiescomplete],
    [activitiescompletename],
    [actualserviceunits],
    [billedserviceunits],
    [blockedprofile],
    [blockedprofilename],
    [caseorigincode],
    [caseorigincodename],
    [casetypecode],
    [casetypecodename],
    [cgi_accountid],
    [cgi_accountidname],
    [cgi_accountidyominame],
    [cgi_accountnumber],
    [cgi_actiondate],
    [cgi_actiondateutc],
    [cgi_arrival_date],
    [cgi_arrival_dateutc],
    [cgi_bic],
    [cgi_bombmobilenumber],
    [cgi_callguideinfo],
    [cgi_callguideinfoname],
    [cgi_casdet_row1_cat1id],
    [cgi_casdet_row1_cat1idname],
    [cgi_casdet_row1_cat2id],
    [cgi_casdet_row1_cat2idname],
    [cgi_casdet_row1_cat3id],
    [cgi_casdet_row1_cat3idname],
    [cgi_casdet_row2_cat1id],
    [cgi_casdet_row2_cat1idname],
    [cgi_casdet_row2_cat2id],
    [cgi_casdet_row2_cat2idname],
    [cgi_casdet_row2_cat3id],
    [cgi_casdet_row2_cat3idname],
    [cgi_casdet_row3_cat1id],
    [cgi_casdet_row3_cat1idname],
    [cgi_casdet_row3_cat2id],
    [cgi_casdet_row3_cat2idname],
    [cgi_casdet_row3_cat3id],
    [cgi_casdet_row3_cat3idname],
    [cgi_casdet_row4_cat1id],
    [cgi_casdet_row4_cat1idname],
    [cgi_casdet_row4_cat2id],
    [cgi_casdet_row4_cat2idname],
    [cgi_casdet_row4_cat3id],
    [cgi_casdet_row4_cat3idname],
    [cgi_casesolved],
    [cgi_case_category_selected],
    [cgi_case_remittance],
    [cgi_case_remittancename],
    [cgi_case_reopen],
    [cgi_chatconversation],
    [cgi_chatid],
    [cgi_chatidname],
    [cgi_circulationnameinpass1],
    [cgi_circulationnameinpass2],
    [cgi_city],
    [cgi_clearingnr],
    [cgi_compensationclaimfromrgol],
    [cgi_compensationclaimfromrgol_base],
    [cgi_contactcustomer],
    [cgi_contactcustomername],
    [cgi_contactid],
    [cgi_contactidname],
    [cgi_contactidyominame],
    [cgi_controlfeeno],
    [cgi_country],
    [cgi_county],
    [cgi_customernumber],
    [cgi_customerpayout],
    [cgi_customerpayout_base],
    [cgi_customers_category],
    [cgi_customers_subcategory],
    [cgi_customer_demand],
    [cgi_customer_email],
    [cgi_customer_number],
    [cgi_customer_telephonenumber],
    [cgi_customer_telephonenumber_mobile],
    [cgi_customer_telephonenumber_work],
    [cgi_departuredatetime],
    [cgi_departuredatetimeutc],
    [cgi_description2],
    [cgi_emailaddress],
    [cgi_emailcount],
    [cgi_experienceddelay],
    [cgi_facebookpostid],
    [cgi_facebookpostidname],
    [cgi_full_name],
    [cgi_gironumber],
    [cgi_handelsedatum],
    [cgi_iban],
    [cgi_ibid],
    [cgi_idc],
    [cgi_invoiceno],
    [cgi_iscompleted],
    [cgi_iscompletedname],
    [cgi_letter_body],
    [cgi_letter_templateid],
    [cgi_letter_templateidname],
    [cgi_letter_title],
    [cgi_line],
    [cgi_milagefrom],
    [cgi_milagekilometers],
    [cgi_milagelicenseplatenumber],
    [cgi_milageto],
    [cgi_notravelinfo],
    [cgi_notravelinfoname],
    [cgi_operatorpass1],
    [cgi_operatorpass2],
    [cgi_originalcallguidecategory],
    [cgi_originalcallguidecategoryname],
    [cgi_passhasbeenopenedatleastonce],
    [cgi_passhasbeenopenedatleastoncename],
    [cgi_paymenttype],
    [cgi_paymenttypename],
    [cgi_postaladdress],
    [cgi_refundaccountno],
    [cgi_refundamount],
    [cgi_refundamount_base],
    [cgi_refundapprovaltype],
    [cgi_refundcalculatedamount],
    [cgi_refundcheckno],
    [cgi_refundcomments],
    [cgi_refundlastvalid],
    [cgi_refundlastvalidutc],
    [cgi_refundmilagecompensation],
    [cgi_refundmilagekm],
    [cgi_refundmobilenumber],
    [cgi_refundquantity],
    [cgi_refundreimbursementform],
    [cgi_refundreimbursementformname],
    [cgi_refundtransportcompanyid],
    [cgi_refundtransportcompanyidname],
    [cgi_refundtransportcompanyidyominame],
    [cgi_refundtravelcardno],
    [cgi_refundtypes],
    [cgi_refundtypesname],
    [cgi_refundvaluecode],
    [cgi_remiss_emailreminders],
    [cgi_representativid],
    [cgi_representatividname],
    [cgi_reqreceipt],
    [cgi_reqreceiptname],
    [cgi_rgolcaselog],
    [cgi_rgolissueid],
    [cgi_rgol_address1_city],
    [cgi_rgol_address1_country],
    [cgi_rgol_address1_line1],
    [cgi_rgol_address1_line2],
    [cgi_rgol_address1_postalcode],
    [cgi_rgol_delivery_email],
    [cgi_rgol_fullname],
    [cgi_rgol_socialsecuritynumber],
    [cgi_rgol_telephonenumber],
    [cgi_runpriorityworkflow],
    [cgi_runpriorityworkflowname],
    [cgi_sa],
    [cgi_sem],
    [cgi_sendmailaction],
    [cgi_sendtoqueue],
    [cgi_sendtoqueuename],
    [cgi_set_anonymous_customer],
    [cgi_sfn],
    [cgi_sln],
    [cgi_soc_sec_number],
    [cgi_sop1],
    [cgi_sop2],
    [cgi_spa],
    [cgi_spf],
    [cgi_sph],
    [cgi_spm],
    [cgi_spr],
    [cgi_spw],
    [cgi_sssn],
    [cgi_streetaddress],
    [cgi_sttj],
    [cgi_taxiclaimedamount],
    [cgi_taxiclaimedamount_base],
    [cgi_taxifrom],
    [cgi_taxito],
    [cgi_telephonenumber],
    [cgi_thirdpartynameid],
    [cgi_thirdpartynameidname],
    [cgi_thirdpartynameidyominame],
    [cgi_ticketnumber1],
    [cgi_ticketnumber2],
    [cgi_tickettype_1],
    [cgi_tickettype_2],
    [cgi_track_token],
    [cgi_track_token_customer],
    [cgi_track_token_remiss_reminder],
    [cgi_train],
    [cgi_travelcardid],
    [cgi_travelcardidname],
    [cgi_travelcardno],
    [cgi_travelinformation],
    [cgi_travelinformationarrivalactual],
    [cgi_travelinformationarrivalplanned],
    [cgi_travelinformationarrivalplannedutc],
    [cgi_travelinformationcity],
    [cgi_travelinformationcompany],
    [cgi_travelinformationdeviationmessage],
    [cgi_travelinformationdirectiontext],
    [cgi_travelinformationdisplaytext],
    [cgi_travelinformationline],
    [cgi_travelinformationlookup],
    [cgi_travelinformationlookupname],
    [cgi_travelinformationstart],
    [cgi_travelinformationstartactual],
    [cgi_travelinformationstartplanned],
    [cgi_travelinformationstartplannedutc],
    [cgi_travelinformationstop],
    [cgi_travelinformationtour],
    [cgi_travelinformationtransport],
    [cgi_underordnaderendenid],
    [cgi_underordnaderendenidname],
    [cgi_unregisterdtravelcard],
    [cgi_way_of_transport],
    [checkemail],
    [checkemailname],
    [contactid],
    [contactiddsc],
    [contactidname],
    [contactidyominame],
    [contractdetailid],
    [contractdetailiddsc],
    [contractdetailidname],
    [contractid],
    [contractiddsc],
    [contractidname],
    [contractservicelevelcode],
    [contractservicelevelcodename],
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
    [customercontacted],
    [customerid],
    [customeriddsc],
    [customeridname],
    [customeridtype],
    [customeridyominame],
    [customersatisfactioncode],
    [customersatisfactioncodename],
    [description],
    [entitlementid],
    [entitlementiddsc],
    [entitlementidname],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [escalatedon],
    [escalatedonutc],
    [exchangerate],
    [existingcase],
    [firstresponsesent],
    [firstresponsesentname],
    [firstresponseslastatus],
    [firstresponseslastatusname],
    [followupby],
    [followupbyutc],
    [followuptaskcreated],
    [followuptaskcreatedname],
    [importsequencenumber],
    [incidentid],
    [incidentstagecode],
    [incidentstagecodename],
    [influencescore],
    [isdecrementing],
    [isdecrementingname],
    [isescalated],
    [isescalatedname],
    [kbarticleid],
    [kbarticleiddsc],
    [kbarticleidname],
    [masterid],
    [masteriddsc],
    [masteridname],
    [merged],
    [mergedname],
    [messagetypecode],
    [messagetypecodename],
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
    [numberofchildincidents],
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
    [parentcaseid],
    [parentcaseiddsc],
    [parentcaseidname],
    [primarycontactid],
    [primarycontactiddsc],
    [primarycontactidname],
    [primarycontactidyominame],
    [prioritycode],
    [prioritycodename],
    [processid],
    [productid],
    [productiddsc],
    [productidname],
    [productserialnumber],
    [resolveby],
    [resolvebyutc],
    [resolvebyslastatus],
    [resolvebyslastatusname],
    [responseby],
    [responsebyutc],
    [responsiblecontactid],
    [responsiblecontactiddsc],
    [responsiblecontactidname],
    [responsiblecontactidyominame],
    [routecase],
    [sentimentvalue],
    [servicestage],
    [servicestagename],
    [severitycode],
    [severitycodename],
    [slainvokedid],
    [slainvokedidname],
    [socialprofileid],
    [socialprofileidname],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subjectid],
    [subjectiddsc],
    [subjectidname],
    [ticketnumber],
    [timezoneruleversionnumber],
    [title],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    [Incident].[AccountId],
    --[Incident].[AccountIdDsc]
    0,
    [Incident].[AccountIdName],
    [Incident].[AccountIdYomiName],
    [Incident].[ActivitiesComplete],
    ActivitiesCompletePLTable.Value,
    [Incident].[ActualServiceUnits],
    [Incident].[BilledServiceUnits],
    [Incident].[BlockedProfile],
    BlockedProfilePLTable.Value,
    [Incident].[CaseOriginCode],
    CaseOriginCodePLTable.Value,
    [Incident].[CaseTypeCode],
    CaseTypeCodePLTable.Value,
    [Incident].[cgi_Accountid],
    [Incident].[cgi_AccountidName],
    [Incident].[cgi_AccountidYomiName],
    [Incident].[cgi_AccountNumber],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_ActionDate], 
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
        [Incident].[cgi_ActionDate],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_arrival_date], 
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
        [Incident].[cgi_arrival_date],
    [Incident].[cgi_bic],
    [Incident].[cgi_BOMBMobileNumber],
    [Incident].[cgi_CallGuideInfo],
    [Incident].[cgi_CallGuideInfoName],
    [Incident].[cgi_casdet_row1_cat1Id],
    [Incident].[cgi_casdet_row1_cat1IdName],
    [Incident].[cgi_casdet_row1_cat2Id],
    [Incident].[cgi_casdet_row1_cat2IdName],
    [Incident].[cgi_casdet_row1_cat3Id],
    [Incident].[cgi_casdet_row1_cat3IdName],
    [Incident].[cgi_casdet_row2_cat1Id],
    [Incident].[cgi_casdet_row2_cat1IdName],
    [Incident].[cgi_casdet_row2_cat2Id],
    [Incident].[cgi_casdet_row2_cat2IdName],
    [Incident].[cgi_casdet_row2_cat3Id],
    [Incident].[cgi_casdet_row2_cat3IdName],
    [Incident].[cgi_casdet_row3_cat1Id],
    [Incident].[cgi_casdet_row3_cat1IdName],
    [Incident].[cgi_casdet_row3_cat2Id],
    [Incident].[cgi_casdet_row3_cat2IdName],
    [Incident].[cgi_casdet_row3_cat3Id],
    [Incident].[cgi_casdet_row3_cat3IdName],
    [Incident].[cgi_casdet_row4_cat1Id],
    [Incident].[cgi_casdet_row4_cat1IdName],
    [Incident].[cgi_casdet_row4_cat2Id],
    [Incident].[cgi_casdet_row4_cat2IdName],
    [Incident].[cgi_casdet_row4_cat3Id],
    [Incident].[cgi_casdet_row4_cat3IdName],
    [Incident].[cgi_CaseSolved],
    [Incident].[cgi_case_category_selected],
    [Incident].[cgi_case_remittance],
    cgi_case_remittancePLTable.Value,
    [Incident].[cgi_case_reopen],
    [Incident].[cgi_ChatConversation],
    [Incident].[cgi_Chatid],
    [Incident].[cgi_ChatidName],
    [Incident].[cgi_CirculationNameInPass1],
    [Incident].[cgi_CirculationNameInPass2],
    [Incident].[cgi_City],
    [Incident].[cgi_clearingnr],
    [Incident].[cgi_compensationclaimfromrgol],
    [Incident].[cgi_compensationclaimfromrgol_Base],
    [Incident].[cgi_ContactCustomer],
    cgi_ContactCustomerPLTable.Value,
    [Incident].[cgi_Contactid],
    [Incident].[cgi_ContactidName],
    [Incident].[cgi_ContactidYomiName],
    [Incident].[cgi_controlfeeno],
    [Incident].[cgi_Country],
    [Incident].[cgi_county],
    [Incident].[cgi_CustomerNumber],
    [Incident].[cgi_CustomerPayout],
    [Incident].[cgi_customerpayout_Base],
    [Incident].[cgi_Customers_Category],
    [Incident].[cgi_Customers_SubCategory],
    [Incident].[cgi_customer_demand],
    [Incident].[cgi_customer_email],
    [Incident].[cgi_customer_number],
    [Incident].[cgi_customer_telephonenumber],
    [Incident].[cgi_customer_telephonenumber_mobile],
    [Incident].[cgi_customer_telephonenumber_work],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_DepartureDateTime], 
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
        [Incident].[cgi_DepartureDateTime],
    [Incident].[cgi_description2],
    [Incident].[cgi_EmailAddress],
    [Incident].[cgi_emailcount],
    [Incident].[cgi_experienceddelay],
    [Incident].[cgi_FacebookPostid],
    [Incident].[cgi_FacebookPostidName],
    [Incident].[cgi_full_name],
    [Incident].[cgi_GiroNumber],
    [Incident].[cgi_Handelsedatum],
    [Incident].[cgi_iban],
    [Incident].[cgi_iBID],
    [Incident].[cgi_iDC],
    [Incident].[cgi_invoiceno],
    [Incident].[cgi_iscompleted],
    cgi_iscompletedPLTable.Value,
    [Incident].[cgi_letter_body],
    [Incident].[cgi_letter_templateId],
    [Incident].[cgi_letter_templateIdName],
    [Incident].[cgi_letter_title],
    [Incident].[cgi_line],
    [Incident].[cgi_milagefrom],
    [Incident].[cgi_milagekilometers],
    [Incident].[cgi_milagelicenseplatenumber],
    [Incident].[cgi_milageto],
    [Incident].[cgi_notravelinfo],
    cgi_notravelinfoPLTable.Value,
    [Incident].[cgi_OperatorPass1],
    [Incident].[cgi_OperatorPass2],
    [Incident].[cgi_OriginalCallguideCategory],
    [Incident].[cgi_OriginalCallguideCategoryName],
    [Incident].[cgi_passhasbeenopenedatleastonce],
    cgi_passhasbeenopenedatleastoncePLTable.Value,
    [Incident].[cgi_PaymentType],
    cgi_PaymentTypePLTable.Value,
    [Incident].[cgi_Postaladdress],
    [Incident].[cgi_RefundAccountNo],
    [Incident].[cgi_RefundAmount],
    [Incident].[cgi_refundamount_Base],
    [Incident].[cgi_refundapprovaltype],
    [Incident].[cgi_RefundCalculatedAmount],
    [Incident].[cgi_refundcheckno],
    [Incident].[cgi_refundcomments],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_RefundLastValid], 
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
        [Incident].[cgi_RefundLastValid],
    [Incident].[cgi_RefundMilageCompensation],
    [Incident].[cgi_RefundMilagekm],
    [Incident].[cgi_refundmobilenumber],
    [Incident].[cgi_RefundQuantity],
    [Incident].[cgi_RefundReimbursementForm],
    [Incident].[cgi_RefundReimbursementFormName],
    [Incident].[cgi_refundtransportcompanyid],
    [Incident].[cgi_refundtransportcompanyidName],
    [Incident].[cgi_refundtransportcompanyidYomiName],
    [Incident].[cgi_RefundTravelCardNo],
    [Incident].[cgi_RefundTypes],
    [Incident].[cgi_RefundTypesName],
    [Incident].[cgi_RefundValueCode],
    [Incident].[cgi_remiss_emailreminders],
    [Incident].[cgi_Representativid],
    [Incident].[cgi_RepresentatividName],
    [Incident].[cgi_reqreceipt],
    cgi_reqreceiptPLTable.Value,
    [Incident].[cgi_rgolcaselog],
    [Incident].[cgi_RGOLIssueId],
    [Incident].[cgi_rgol_address1_city],
    [Incident].[cgi_rgol_address1_country],
    [Incident].[cgi_rgol_address1_line1],
    [Incident].[cgi_rgol_address1_line2],
    [Incident].[cgi_rgol_address1_postalcode],
    [Incident].[cgi_rgol_delivery_email],
    [Incident].[cgi_rgol_fullname],
    [Incident].[cgi_rgol_socialsecuritynumber],
    [Incident].[cgi_rgol_telephonenumber],
    [Incident].[cgi_RunPriorityWorkflow],
    cgi_RunPriorityWorkflowPLTable.Value,
    [Incident].[cgi_sA],
    [Incident].[cgi_sEM],
    [Incident].[cgi_SendMailAction],
    [Incident].[cgi_sendtoqueue],
    cgi_sendtoqueuePLTable.Value,
    [Incident].[cgi_set_anonymous_customer],
    [Incident].[cgi_sFN],
    [Incident].[cgi_sLN],
    [Incident].[cgi_soc_sec_number],
    [Incident].[cgi_sOP1],
    [Incident].[cgi_sOP2],
    [Incident].[cgi_sPA],
    [Incident].[cgi_sPF],
    [Incident].[cgi_sPH],
    [Incident].[cgi_sPM],
    [Incident].[cgi_sPR],
    [Incident].[cgi_sPW],
    [Incident].[cgi_sSSN],
    [Incident].[cgi_StreetAddress],
    [Incident].[cgi_sTTJ],
    [Incident].[cgi_taxiclaimedamount],
    [Incident].[cgi_taxiclaimedamount_Base],
    [Incident].[cgi_taxifrom],
    [Incident].[cgi_taxito],
    [Incident].[cgi_TelephoneNumber],
    [Incident].[cgi_ThirdpartyNameid],
    [Incident].[cgi_ThirdpartyNameidName],
    [Incident].[cgi_ThirdpartyNameidYomiName],
    [Incident].[cgi_ticketnumber1],
    [Incident].[cgi_ticketnumber2],
    [Incident].[cgi_tickettype_1],
    [Incident].[cgi_tickettype_2],
    [Incident].[cgi_track_token],
    [Incident].[cgi_track_token_customer],
    [Incident].[cgi_track_token_remiss_reminder],
    [Incident].[cgi_train],
    [Incident].[cgi_TravelCardid],
    [Incident].[cgi_TravelCardidName],
    [Incident].[cgi_TravelCardNo],
    [Incident].[cgi_TravelInformation],
    [Incident].[cgi_TravelInformationArrivalActual],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_TravelInformationArrivalPlanned], 
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
        [Incident].[cgi_TravelInformationArrivalPlanned],
    [Incident].[cgi_TravelInformationCity],
    [Incident].[cgi_TravelInformationCompany],
    [Incident].[cgi_TravelInformationDeviationMessage],
    [Incident].[cgi_TravelInformationDirectionText],
    [Incident].[cgi_TravelInformationDisplayText],
    [Incident].[cgi_TravelInformationLine],
    [Incident].[cgi_TravelInformationLookup],
    [Incident].[cgi_TravelInformationLookupName],
    [Incident].[cgi_TravelInformationStart],
    [Incident].[cgi_TravelInformationStartActual],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[cgi_TravelInformationStartPlanned], 
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
        [Incident].[cgi_TravelInformationStartPlanned],
    [Incident].[cgi_TravelInformationStop],
    [Incident].[cgi_TravelInformationTour],
    [Incident].[cgi_TravelInformationTransport],
    [Incident].[cgi_UnderordnaderendenId],
    [Incident].[cgi_UnderordnaderendenIdName],
    [Incident].[cgi_UnregisterdTravelCard],
    [Incident].[cgi_way_of_transport],
    [Incident].[CheckEmail],
    CheckEmailPLTable.Value,
    [Incident].[ContactId],
    --[Incident].[ContactIdDsc]
    0,
    [Incident].[ContactIdName],
    [Incident].[ContactIdYomiName],
    [Incident].[ContractDetailId],
    --[Incident].[ContractDetailIdDsc]
    0,
    [Incident].[ContractDetailIdName],
    [Incident].[ContractId],
    --[Incident].[ContractIdDsc]
    0,
    [Incident].[ContractIdName],
    [Incident].[ContractServiceLevelCode],
    ContractServiceLevelCodePLTable.Value,
    [Incident].[CreatedBy],
    --[Incident].[CreatedByDsc]
    0,
    [Incident].[CreatedByName],
    [Incident].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[CreatedOn], 
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
        [Incident].[CreatedOn],
    [Incident].[CreatedOnBehalfBy],
    --[Incident].[CreatedOnBehalfByDsc]
    0,
    [Incident].[CreatedOnBehalfByName],
    [Incident].[CreatedOnBehalfByYomiName],
    [Incident].[CustomerContacted],
    [Incident].[CustomerId],
    --[Incident].[CustomerIdDsc]
    0,
    [Incident].[CustomerIdName],
    [Incident].[CustomerIdType],
    [Incident].[CustomerIdYomiName],
    [Incident].[CustomerSatisfactionCode],
    CustomerSatisfactionCodePLTable.Value,
    [Incident].[Description],
    [Incident].[EntitlementId],
    --[Incident].[EntitlementIdDsc]
    0,
    [Incident].[EntitlementIdName],
    --[Incident].[EntityImage]
    cast(null as varbinary),
    [Incident].[EntityImageId],
    [Incident].[EntityImage_Timestamp],
    [Incident].[EntityImage_URL],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[EscalatedOn], 
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
        [Incident].[EscalatedOn],
    [Incident].[ExchangeRate],
    [Incident].[ExistingCase],
    [Incident].[FirstResponseSent],
    FirstResponseSentPLTable.Value,
    [Incident].[FirstResponseSLAStatus],
    FirstResponseSLAStatusPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[FollowupBy], 
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
        [Incident].[FollowupBy],
    [Incident].[FollowUpTaskCreated],
    FollowUpTaskCreatedPLTable.Value,
    [Incident].[ImportSequenceNumber],
    [Incident].[IncidentId],
    [Incident].[IncidentStageCode],
    IncidentStageCodePLTable.Value,
    [Incident].[InfluenceScore],
    [Incident].[IsDecrementing],
    IsDecrementingPLTable.Value,
    [Incident].[IsEscalated],
    IsEscalatedPLTable.Value,
    [Incident].[KbArticleId],
    --[Incident].[KbArticleIdDsc]
    0,
    [Incident].[KbArticleIdName],
    [Incident].[MasterId],
    --[Incident].[MasterIdDsc]
    0,
    [Incident].[MasterIdName],
    [Incident].[Merged],
    MergedPLTable.Value,
    [Incident].[MessageTypeCode],
    MessageTypeCodePLTable.Value,
    [Incident].[ModifiedBy],
    --[Incident].[ModifiedByDsc]
    0,
    [Incident].[ModifiedByName],
    [Incident].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[ModifiedOn], 
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
        [Incident].[ModifiedOn],
    [Incident].[ModifiedOnBehalfBy],
    --[Incident].[ModifiedOnBehalfByDsc]
    0,
    [Incident].[ModifiedOnBehalfByName],
    [Incident].[ModifiedOnBehalfByYomiName],
    [Incident].[NumberOfChildIncidents],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[OverriddenCreatedOn], 
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
        [Incident].[OverriddenCreatedOn],
    [Incident].[OwnerId],
    --[Incident].[OwnerIdDsc]
    0,
    [Incident].[OwnerIdName],
    [Incident].[OwnerIdType],
    [Incident].[OwnerIdYomiName],
    [Incident].[OwningBusinessUnit],
    [Incident].[OwningTeam],
    [Incident].[OwningUser],
    [Incident].[ParentCaseId],
    --[Incident].[ParentCaseIdDsc]
    0,
    [Incident].[ParentCaseIdName],
    [Incident].[PrimaryContactId],
    --[Incident].[PrimaryContactIdDsc]
    0,
    [Incident].[PrimaryContactIdName],
    [Incident].[PrimaryContactIdYomiName],
    [Incident].[PriorityCode],
    PriorityCodePLTable.Value,
    [Incident].[ProcessId],
    [Incident].[ProductId],
    --[Incident].[ProductIdDsc]
    0,
    [Incident].[ProductIdName],
    [Incident].[ProductSerialNumber],
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[ResolveBy], 
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
        [Incident].[ResolveBy],
    [Incident].[ResolveBySLAStatus],
    ResolveBySLAStatusPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Incident].[ResponseBy], 
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
        [Incident].[ResponseBy],
    [Incident].[ResponsibleContactId],
    --[Incident].[ResponsibleContactIdDsc]
    0,
    [Incident].[ResponsibleContactIdName],
    [Incident].[ResponsibleContactIdYomiName],
    [Incident].[RouteCase],
    [Incident].[SentimentValue],
    [Incident].[ServiceStage],
    ServiceStagePLTable.Value,
    [Incident].[SeverityCode],
    SeverityCodePLTable.Value,
    [Incident].[SLAInvokedId],
    [Incident].[SLAInvokedIdName],
    [Incident].[SocialProfileId],
    [Incident].[SocialProfileIdName],
    [Incident].[StageId],
    [Incident].[StateCode],
    StateCodePLTable.Value,
    [Incident].[StatusCode],
    StatusCodePLTable.Value,
    [Incident].[SubjectId],
    --[Incident].[SubjectIdDsc]
    0,
    [Incident].[SubjectIdName],
    [Incident].[TicketNumber],
    [Incident].[TimeZoneRuleVersionNumber],
    [Incident].[Title],
    [Incident].[TransactionCurrencyId],
    [Incident].[TransactionCurrencyIdName],
    [Incident].[UTCConversionTimeZoneCode],
    [Incident].[VersionNumber],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from Incident
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [Incident].TransactionCurrencyId
    left outer join StringMap [ActivitiesCompletePLTable] on 
		([ActivitiesCompletePLTable].AttributeName = 'activitiescomplete'
		and [ActivitiesCompletePLTable].ObjectTypeCode = 112
		and [ActivitiesCompletePLTable].AttributeValue = [Incident].[ActivitiesComplete]
		and [ActivitiesCompletePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [BlockedProfilePLTable] on 
		([BlockedProfilePLTable].AttributeName = 'blockedprofile'
		and [BlockedProfilePLTable].ObjectTypeCode = 112
		and [BlockedProfilePLTable].AttributeValue = [Incident].[BlockedProfile]
		and [BlockedProfilePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CaseOriginCodePLTable] on 
		([CaseOriginCodePLTable].AttributeName = 'caseorigincode'
		and [CaseOriginCodePLTable].ObjectTypeCode = 112
		and [CaseOriginCodePLTable].AttributeValue = [Incident].[CaseOriginCode]
		and [CaseOriginCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CaseTypeCodePLTable] on 
		([CaseTypeCodePLTable].AttributeName = 'casetypecode'
		and [CaseTypeCodePLTable].ObjectTypeCode = 112
		and [CaseTypeCodePLTable].AttributeValue = [Incident].[CaseTypeCode]
		and [CaseTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_case_remittancePLTable] on 
		([cgi_case_remittancePLTable].AttributeName = 'cgi_case_remittance'
		and [cgi_case_remittancePLTable].ObjectTypeCode = 112
		and [cgi_case_remittancePLTable].AttributeValue = [Incident].[cgi_case_remittance]
		and [cgi_case_remittancePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_ContactCustomerPLTable] on 
		([cgi_ContactCustomerPLTable].AttributeName = 'cgi_contactcustomer'
		and [cgi_ContactCustomerPLTable].ObjectTypeCode = 112
		and [cgi_ContactCustomerPLTable].AttributeValue = [Incident].[cgi_ContactCustomer]
		and [cgi_ContactCustomerPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_iscompletedPLTable] on 
		([cgi_iscompletedPLTable].AttributeName = 'cgi_iscompleted'
		and [cgi_iscompletedPLTable].ObjectTypeCode = 112
		and [cgi_iscompletedPLTable].AttributeValue = [Incident].[cgi_iscompleted]
		and [cgi_iscompletedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_notravelinfoPLTable] on 
		([cgi_notravelinfoPLTable].AttributeName = 'cgi_notravelinfo'
		and [cgi_notravelinfoPLTable].ObjectTypeCode = 112
		and [cgi_notravelinfoPLTable].AttributeValue = [Incident].[cgi_notravelinfo]
		and [cgi_notravelinfoPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_passhasbeenopenedatleastoncePLTable] on 
		([cgi_passhasbeenopenedatleastoncePLTable].AttributeName = 'cgi_passhasbeenopenedatleastonce'
		and [cgi_passhasbeenopenedatleastoncePLTable].ObjectTypeCode = 112
		and [cgi_passhasbeenopenedatleastoncePLTable].AttributeValue = [Incident].[cgi_passhasbeenopenedatleastonce]
		and [cgi_passhasbeenopenedatleastoncePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_PaymentTypePLTable] on 
		([cgi_PaymentTypePLTable].AttributeName = 'cgi_paymenttype'
		and [cgi_PaymentTypePLTable].ObjectTypeCode = 112
		and [cgi_PaymentTypePLTable].AttributeValue = [Incident].[cgi_PaymentType]
		and [cgi_PaymentTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_reqreceiptPLTable] on 
		([cgi_reqreceiptPLTable].AttributeName = 'cgi_reqreceipt'
		and [cgi_reqreceiptPLTable].ObjectTypeCode = 112
		and [cgi_reqreceiptPLTable].AttributeValue = [Incident].[cgi_reqreceipt]
		and [cgi_reqreceiptPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_RunPriorityWorkflowPLTable] on 
		([cgi_RunPriorityWorkflowPLTable].AttributeName = 'cgi_runpriorityworkflow'
		and [cgi_RunPriorityWorkflowPLTable].ObjectTypeCode = 112
		and [cgi_RunPriorityWorkflowPLTable].AttributeValue = [Incident].[cgi_RunPriorityWorkflow]
		and [cgi_RunPriorityWorkflowPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_sendtoqueuePLTable] on 
		([cgi_sendtoqueuePLTable].AttributeName = 'cgi_sendtoqueue'
		and [cgi_sendtoqueuePLTable].ObjectTypeCode = 112
		and [cgi_sendtoqueuePLTable].AttributeValue = [Incident].[cgi_sendtoqueue]
		and [cgi_sendtoqueuePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CheckEmailPLTable] on 
		([CheckEmailPLTable].AttributeName = 'checkemail'
		and [CheckEmailPLTable].ObjectTypeCode = 112
		and [CheckEmailPLTable].AttributeValue = [Incident].[CheckEmail]
		and [CheckEmailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ContractServiceLevelCodePLTable] on 
		([ContractServiceLevelCodePLTable].AttributeName = 'contractservicelevelcode'
		and [ContractServiceLevelCodePLTable].ObjectTypeCode = 112
		and [ContractServiceLevelCodePLTable].AttributeValue = [Incident].[ContractServiceLevelCode]
		and [ContractServiceLevelCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CustomerSatisfactionCodePLTable] on 
		([CustomerSatisfactionCodePLTable].AttributeName = 'customersatisfactioncode'
		and [CustomerSatisfactionCodePLTable].ObjectTypeCode = 112
		and [CustomerSatisfactionCodePLTable].AttributeValue = [Incident].[CustomerSatisfactionCode]
		and [CustomerSatisfactionCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [FirstResponseSentPLTable] on 
		([FirstResponseSentPLTable].AttributeName = 'firstresponsesent'
		and [FirstResponseSentPLTable].ObjectTypeCode = 112
		and [FirstResponseSentPLTable].AttributeValue = [Incident].[FirstResponseSent]
		and [FirstResponseSentPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [FirstResponseSLAStatusPLTable] on 
		([FirstResponseSLAStatusPLTable].AttributeName = 'firstresponseslastatus'
		and [FirstResponseSLAStatusPLTable].ObjectTypeCode = 112
		and [FirstResponseSLAStatusPLTable].AttributeValue = [Incident].[FirstResponseSLAStatus]
		and [FirstResponseSLAStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [FollowUpTaskCreatedPLTable] on 
		([FollowUpTaskCreatedPLTable].AttributeName = 'followuptaskcreated'
		and [FollowUpTaskCreatedPLTable].ObjectTypeCode = 112
		and [FollowUpTaskCreatedPLTable].AttributeValue = [Incident].[FollowUpTaskCreated]
		and [FollowUpTaskCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IncidentStageCodePLTable] on 
		([IncidentStageCodePLTable].AttributeName = 'incidentstagecode'
		and [IncidentStageCodePLTable].ObjectTypeCode = 112
		and [IncidentStageCodePLTable].AttributeValue = [Incident].[IncidentStageCode]
		and [IncidentStageCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsDecrementingPLTable] on 
		([IsDecrementingPLTable].AttributeName = 'isdecrementing'
		and [IsDecrementingPLTable].ObjectTypeCode = 112
		and [IsDecrementingPLTable].AttributeValue = [Incident].[IsDecrementing]
		and [IsDecrementingPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsEscalatedPLTable] on 
		([IsEscalatedPLTable].AttributeName = 'isescalated'
		and [IsEscalatedPLTable].ObjectTypeCode = 112
		and [IsEscalatedPLTable].AttributeValue = [Incident].[IsEscalated]
		and [IsEscalatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [MergedPLTable] on 
		([MergedPLTable].AttributeName = 'merged'
		and [MergedPLTable].ObjectTypeCode = 112
		and [MergedPLTable].AttributeValue = [Incident].[Merged]
		and [MergedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [MessageTypeCodePLTable] on 
		([MessageTypeCodePLTable].AttributeName = 'messagetypecode'
		and [MessageTypeCodePLTable].ObjectTypeCode = 112
		and [MessageTypeCodePLTable].AttributeValue = [Incident].[MessageTypeCode]
		and [MessageTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 112
		and [PriorityCodePLTable].AttributeValue = [Incident].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ResolveBySLAStatusPLTable] on 
		([ResolveBySLAStatusPLTable].AttributeName = 'resolvebyslastatus'
		and [ResolveBySLAStatusPLTable].ObjectTypeCode = 112
		and [ResolveBySLAStatusPLTable].AttributeValue = [Incident].[ResolveBySLAStatus]
		and [ResolveBySLAStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ServiceStagePLTable] on 
		([ServiceStagePLTable].AttributeName = 'servicestage'
		and [ServiceStagePLTable].ObjectTypeCode = 112
		and [ServiceStagePLTable].AttributeValue = [Incident].[ServiceStage]
		and [ServiceStagePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SeverityCodePLTable] on 
		([SeverityCodePLTable].AttributeName = 'severitycode'
		and [SeverityCodePLTable].ObjectTypeCode = 112
		and [SeverityCodePLTable].AttributeValue = [Incident].[SeverityCode]
		and [SeverityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 112
		and [StateCodePLTable].AttributeValue = [Incident].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 112
		and [StatusCodePLTable].AttributeValue = [Incident].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(112) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Incident].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 112
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
		[Incident].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 112)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Incident].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Incident].[IncidentId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 112 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

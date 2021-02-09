

--
-- report view for campaign
--
create view dbo.[FilteredCampaign] (
    [actualend],
    [actualendutc],
    [actualstart],
    [actualstartutc],
    [budgetedcost],
    [budgetedcost_base],
    [campaignid],
    [cgi_adr],
    [cgi_adrname],
    [cgi_advertising_di],
    [cgi_advertising_diname],
    [cgi_advertising_pr],
    [cgi_advertising_prname],
    [cgi_assignment_agency],
    [cgi_cinema],
    [cgi_cinemaname],
    [cgi_communication_objective],
    [cgi_customercenter],
    [cgi_customercentername],
    [cgi_evaluation],
    [cgi_event],
    [cgi_eventname],
    [cgi_infotainment],
    [cgi_infotainmentname],
    [cgi_marketarea],
    [cgi_marketareaname],
    [cgi_newsletter],
    [cgi_newslettername],
    [cgi_objective],
    [cgi_objective_meet],
    [cgi_objective_meetname],
    [cgi_odr],
    [cgi_odrname],
    [cgi_outcome_budget],
    [cgi_outcome_budget_base],
    [cgi_outdoor_advertising],
    [cgi_outdoor_advertisingname],
    [cgi_radio],
    [cgi_radioname],
    [cgi_recommendations],
    [cgi_segment],
    [cgi_segmentname],
    [cgi_social_media],
    [cgi_social_medianame],
    [cgi_station_advertising],
    [cgi_station_advertisingname],
    [cgi_strategy],
    [cgi_strategyname],
    [cgi_targetgroup],
    [cgi_trafficcontractid],
    [cgi_trafficcontractidname],
    [cgi_tv],
    [cgi_tvname],
    [cgi_vehicle_advertising],
    [cgi_vehicle_advertisingname],
    [codename],
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
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [expectedresponse],
    [expectedrevenue],
    [expectedrevenue_base],
    [importsequencenumber],
    [istemplate],
    [istemplatename],
    [message],
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
    [objective],
    [othercost],
    [othercost_base],
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
    [pricelistdsc],
    [pricelistid],
    [pricelistname],
    [processid],
    [promotioncodename],
    [proposedend],
    [proposedendutc],
    [proposedstart],
    [proposedstartutc],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [totalactualcost],
    [totalactualcost_base],
    [totalcampaignactivityactualcost],
    [totalcampaignactivityactualcost_base],
    [transactioncurrencyid],
    [transactioncurrencyiddsc],
    [transactioncurrencyidname],
    [typecode],
    [typecodename],
    [utcconversiontimezonecode],
    [versionnumber],
    crm_moneyformatstring,
    crm_priceformatstring
) with view_metadata as
select
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[ActualEnd], 
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
        [Campaign].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[ActualStart], 
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
        [Campaign].[ActualStart],
    [Campaign].[BudgetedCost],
    [Campaign].[BudgetedCost_Base],
    [Campaign].[CampaignId],
    [Campaign].[cgi_adr],
    cgi_adrPLTable.Value,
    [Campaign].[cgi_advertising_di],
    cgi_advertising_diPLTable.Value,
    [Campaign].[cgi_advertising_pr],
    cgi_advertising_prPLTable.Value,
    [Campaign].[cgi_assignment_agency],
    [Campaign].[cgi_cinema],
    cgi_cinemaPLTable.Value,
    [Campaign].[cgi_communication_objective],
    [Campaign].[cgi_customercenter],
    cgi_customercenterPLTable.Value,
    [Campaign].[cgi_evaluation],
    [Campaign].[cgi_event],
    cgi_eventPLTable.Value,
    [Campaign].[cgi_infotainment],
    cgi_infotainmentPLTable.Value,
    [Campaign].[cgi_marketarea],
    cgi_marketareaPLTable.Value,
    [Campaign].[cgi_newsletter],
    cgi_newsletterPLTable.Value,
    [Campaign].[cgi_objective],
    [Campaign].[cgi_objective_meet],
    cgi_objective_meetPLTable.Value,
    [Campaign].[cgi_odr],
    cgi_odrPLTable.Value,
    [Campaign].[cgi_outcome_budget],
    [Campaign].[cgi_outcome_budget_Base],
    [Campaign].[cgi_outdoor_advertising],
    cgi_outdoor_advertisingPLTable.Value,
    [Campaign].[cgi_radio],
    cgi_radioPLTable.Value,
    [Campaign].[cgi_recommendations],
    [Campaign].[cgi_segment],
    cgi_segmentPLTable.Value,
    [Campaign].[cgi_social_media],
    cgi_social_mediaPLTable.Value,
    [Campaign].[cgi_station_advertising],
    cgi_station_advertisingPLTable.Value,
    [Campaign].[cgi_strategy],
    cgi_strategyPLTable.Value,
    [Campaign].[cgi_targetgroup],
    [Campaign].[cgi_trafficcontractId],
    [Campaign].[cgi_trafficcontractIdName],
    [Campaign].[cgi_tv],
    cgi_tvPLTable.Value,
    [Campaign].[cgi_vehicle_advertising],
    cgi_vehicle_advertisingPLTable.Value,
    [Campaign].[CodeName],
    [Campaign].[CreatedBy],
    --[Campaign].[CreatedByDsc]
    0,
    [Campaign].[CreatedByName],
    [Campaign].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[CreatedOn], 
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
        [Campaign].[CreatedOn],
    [Campaign].[CreatedOnBehalfBy],
    --[Campaign].[CreatedOnBehalfByDsc]
    0,
    [Campaign].[CreatedOnBehalfByName],
    [Campaign].[CreatedOnBehalfByYomiName],
    [Campaign].[Description],
    --[Campaign].[EntityImage]
    cast(null as varbinary),
    [Campaign].[EntityImageId],
    [Campaign].[EntityImage_Timestamp],
    [Campaign].[EntityImage_URL],
    [Campaign].[ExchangeRate],
    [Campaign].[ExpectedResponse],
    [Campaign].[ExpectedRevenue],
    [Campaign].[ExpectedRevenue_Base],
    [Campaign].[ImportSequenceNumber],
    [Campaign].[IsTemplate],
    IsTemplatePLTable.Value,
    [Campaign].[Message],
    [Campaign].[ModifiedBy],
    --[Campaign].[ModifiedByDsc]
    0,
    [Campaign].[ModifiedByName],
    [Campaign].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[ModifiedOn], 
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
        [Campaign].[ModifiedOn],
    [Campaign].[ModifiedOnBehalfBy],
    --[Campaign].[ModifiedOnBehalfByDsc]
    0,
    [Campaign].[ModifiedOnBehalfByName],
    [Campaign].[ModifiedOnBehalfByYomiName],
    [Campaign].[Name],
    [Campaign].[Objective],
    [Campaign].[OtherCost],
    [Campaign].[OtherCost_Base],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[OverriddenCreatedOn], 
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
        [Campaign].[OverriddenCreatedOn],
    [Campaign].[OwnerId],
    --[Campaign].[OwnerIdDsc]
    0,
    [Campaign].[OwnerIdName],
    [Campaign].[OwnerIdType],
    [Campaign].[OwnerIdYomiName],
    [Campaign].[OwningBusinessUnit],
    [Campaign].[OwningTeam],
    [Campaign].[OwningUser],
    --[Campaign].[PriceListDsc]
    0,
    [Campaign].[PriceListId],
    [Campaign].[PriceListName],
    [Campaign].[ProcessId],
    [Campaign].[PromotionCodeName],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[ProposedEnd], 
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
        [Campaign].[ProposedEnd],
    dbo.fn_UTCToTzSpecificLocalTime([Campaign].[ProposedStart], 
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
        [Campaign].[ProposedStart],
    [Campaign].[StageId],
    [Campaign].[StateCode],
    StateCodePLTable.Value,
    [Campaign].[StatusCode],
    StatusCodePLTable.Value,
    [Campaign].[TimeZoneRuleVersionNumber],
    [Campaign].[TotalActualCost],
    [Campaign].[TotalActualCost_Base],
    [Campaign].[TotalCampaignActivityActualCost],
    [Campaign].[TotalCampaignActivityActualCost_Base],
    [Campaign].[TransactionCurrencyId],
    --[Campaign].[TransactionCurrencyIdDsc]
    0,
    [Campaign].[TransactionCurrencyIdName],
    [Campaign].[TypeCode],
    TypeCodePLTable.Value,
    [Campaign].[UTCConversionTimeZoneCode],
    [Campaign].[VersionNumber],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from Campaign
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [Campaign].TransactionCurrencyId
    left outer join StringMap [cgi_adrPLTable] on 
		([cgi_adrPLTable].AttributeName = 'cgi_adr'
		and [cgi_adrPLTable].ObjectTypeCode = 4400
		and [cgi_adrPLTable].AttributeValue = [Campaign].[cgi_adr]
		and [cgi_adrPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_advertising_diPLTable] on 
		([cgi_advertising_diPLTable].AttributeName = 'cgi_advertising_di'
		and [cgi_advertising_diPLTable].ObjectTypeCode = 4400
		and [cgi_advertising_diPLTable].AttributeValue = [Campaign].[cgi_advertising_di]
		and [cgi_advertising_diPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_advertising_prPLTable] on 
		([cgi_advertising_prPLTable].AttributeName = 'cgi_advertising_pr'
		and [cgi_advertising_prPLTable].ObjectTypeCode = 4400
		and [cgi_advertising_prPLTable].AttributeValue = [Campaign].[cgi_advertising_pr]
		and [cgi_advertising_prPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_cinemaPLTable] on 
		([cgi_cinemaPLTable].AttributeName = 'cgi_cinema'
		and [cgi_cinemaPLTable].ObjectTypeCode = 4400
		and [cgi_cinemaPLTable].AttributeValue = [Campaign].[cgi_cinema]
		and [cgi_cinemaPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_customercenterPLTable] on 
		([cgi_customercenterPLTable].AttributeName = 'cgi_customercenter'
		and [cgi_customercenterPLTable].ObjectTypeCode = 4400
		and [cgi_customercenterPLTable].AttributeValue = [Campaign].[cgi_customercenter]
		and [cgi_customercenterPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_eventPLTable] on 
		([cgi_eventPLTable].AttributeName = 'cgi_event'
		and [cgi_eventPLTable].ObjectTypeCode = 4400
		and [cgi_eventPLTable].AttributeValue = [Campaign].[cgi_event]
		and [cgi_eventPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_infotainmentPLTable] on 
		([cgi_infotainmentPLTable].AttributeName = 'cgi_infotainment'
		and [cgi_infotainmentPLTable].ObjectTypeCode = 4400
		and [cgi_infotainmentPLTable].AttributeValue = [Campaign].[cgi_infotainment]
		and [cgi_infotainmentPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_marketareaPLTable] on 
		([cgi_marketareaPLTable].AttributeName = 'cgi_marketarea'
		and [cgi_marketareaPLTable].ObjectTypeCode = 4400
		and [cgi_marketareaPLTable].AttributeValue = [Campaign].[cgi_marketarea]
		and [cgi_marketareaPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_newsletterPLTable] on 
		([cgi_newsletterPLTable].AttributeName = 'cgi_newsletter'
		and [cgi_newsletterPLTable].ObjectTypeCode = 4400
		and [cgi_newsletterPLTable].AttributeValue = [Campaign].[cgi_newsletter]
		and [cgi_newsletterPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_objective_meetPLTable] on 
		([cgi_objective_meetPLTable].AttributeName = 'cgi_objective_meet'
		and [cgi_objective_meetPLTable].ObjectTypeCode = 4400
		and [cgi_objective_meetPLTable].AttributeValue = [Campaign].[cgi_objective_meet]
		and [cgi_objective_meetPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_odrPLTable] on 
		([cgi_odrPLTable].AttributeName = 'cgi_odr'
		and [cgi_odrPLTable].ObjectTypeCode = 4400
		and [cgi_odrPLTable].AttributeValue = [Campaign].[cgi_odr]
		and [cgi_odrPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_outdoor_advertisingPLTable] on 
		([cgi_outdoor_advertisingPLTable].AttributeName = 'cgi_outdoor_advertising'
		and [cgi_outdoor_advertisingPLTable].ObjectTypeCode = 4400
		and [cgi_outdoor_advertisingPLTable].AttributeValue = [Campaign].[cgi_outdoor_advertising]
		and [cgi_outdoor_advertisingPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_radioPLTable] on 
		([cgi_radioPLTable].AttributeName = 'cgi_radio'
		and [cgi_radioPLTable].ObjectTypeCode = 4400
		and [cgi_radioPLTable].AttributeValue = [Campaign].[cgi_radio]
		and [cgi_radioPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_segmentPLTable] on 
		([cgi_segmentPLTable].AttributeName = 'cgi_segment'
		and [cgi_segmentPLTable].ObjectTypeCode = 4400
		and [cgi_segmentPLTable].AttributeValue = [Campaign].[cgi_segment]
		and [cgi_segmentPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_social_mediaPLTable] on 
		([cgi_social_mediaPLTable].AttributeName = 'cgi_social_media'
		and [cgi_social_mediaPLTable].ObjectTypeCode = 4400
		and [cgi_social_mediaPLTable].AttributeValue = [Campaign].[cgi_social_media]
		and [cgi_social_mediaPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_station_advertisingPLTable] on 
		([cgi_station_advertisingPLTable].AttributeName = 'cgi_station_advertising'
		and [cgi_station_advertisingPLTable].ObjectTypeCode = 4400
		and [cgi_station_advertisingPLTable].AttributeValue = [Campaign].[cgi_station_advertising]
		and [cgi_station_advertisingPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_strategyPLTable] on 
		([cgi_strategyPLTable].AttributeName = 'cgi_strategy'
		and [cgi_strategyPLTable].ObjectTypeCode = 4400
		and [cgi_strategyPLTable].AttributeValue = [Campaign].[cgi_strategy]
		and [cgi_strategyPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_tvPLTable] on 
		([cgi_tvPLTable].AttributeName = 'cgi_tv'
		and [cgi_tvPLTable].ObjectTypeCode = 4400
		and [cgi_tvPLTable].AttributeValue = [Campaign].[cgi_tv]
		and [cgi_tvPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_vehicle_advertisingPLTable] on 
		([cgi_vehicle_advertisingPLTable].AttributeName = 'cgi_vehicle_advertising'
		and [cgi_vehicle_advertisingPLTable].ObjectTypeCode = 4400
		and [cgi_vehicle_advertisingPLTable].AttributeValue = [Campaign].[cgi_vehicle_advertising]
		and [cgi_vehicle_advertisingPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsTemplatePLTable] on 
		([IsTemplatePLTable].AttributeName = 'istemplate'
		and [IsTemplatePLTable].ObjectTypeCode = 4400
		and [IsTemplatePLTable].AttributeValue = [Campaign].[IsTemplate]
		and [IsTemplatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4400
		and [StateCodePLTable].AttributeValue = [Campaign].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4400
		and [StatusCodePLTable].AttributeValue = [Campaign].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TypeCodePLTable] on 
		([TypeCodePLTable].AttributeName = 'typecode'
		and [TypeCodePLTable].ObjectTypeCode = 4400
		and [TypeCodePLTable].AttributeValue = [Campaign].[TypeCode]
		and [TypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4400) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Campaign].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4400
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
		[Campaign].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4400)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Campaign].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Campaign].[CampaignId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4400 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

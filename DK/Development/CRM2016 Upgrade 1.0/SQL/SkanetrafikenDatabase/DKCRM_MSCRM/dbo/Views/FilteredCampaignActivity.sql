﻿

--
-- report view for campaignactivity
--
create view dbo.[FilteredCampaignActivity] (
    [activityid],
    [activitytypecode],
    [activitytypecodename],
    [actualcost],
    [actualcost_base],
    [actualdurationminutes],
    [actualend],
    [actualendutc],
    [actualstart],
    [actualstartutc],
    [budgetedcost],
    [budgetedcost_base],
    [category],
    [channeltypecode],
    [channeltypecodename],
    [checkfordonotsendmmonlistmembersname],
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
    [donotsendonoptout],
    [exchangerate],
    [excludeifcontactedinxdays],
    [ignoreinactivelistmembers],
    [ignoreinactivelistmembersname],
    [importsequencenumber],
    [isbilled],
    [isbilledname],
    [isregularactivity],
    [isregularactivityname],
    [isworkflowcreated],
    [isworkflowcreatedname],
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
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [prioritycode],
    [prioritycodename],
    [processid],
    [regardingobjectid],
    [regardingobjectiddsc],
    [regardingobjectidname],
    [regardingobjectidyominame],
    [regardingobjecttypecode],
    [scheduleddurationminutes],
    [scheduledend],
    [scheduledendutc],
    [scheduledstart],
    [scheduledstartutc],
    [serviceid],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subcategory],
    [subject],
    [timezoneruleversionnumber],
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
    [CampaignActivity].[ActivityId],
    [CampaignActivity].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [CampaignActivity].[ActualCost],
    [CampaignActivity].[ActualCost_Base],
    [CampaignActivity].[ActualDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[ActualEnd], 
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
        [CampaignActivity].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[ActualStart], 
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
        [CampaignActivity].[ActualStart],
    [CampaignActivity].[BudgetedCost],
    [CampaignActivity].[BudgetedCost_Base],
    [CampaignActivity].[Category],
    [CampaignActivity].[ChannelTypeCode],
    ChannelTypeCodePLTable.Value,
    DoNotSendOnOptOutPLTable.Value,
    [CampaignActivity].[CreatedBy],
    --[CampaignActivity].[CreatedByDsc]
    0,
    [CampaignActivity].[CreatedByName],
    [CampaignActivity].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[CreatedOn], 
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
        [CampaignActivity].[CreatedOn],
    [CampaignActivity].[CreatedOnBehalfBy],
    --[CampaignActivity].[CreatedOnBehalfByDsc]
    0,
    [CampaignActivity].[CreatedOnBehalfByName],
    [CampaignActivity].[CreatedOnBehalfByYomiName],
    [CampaignActivity].[Description],
    [CampaignActivity].[DoNotSendOnOptOut],
    [CampaignActivity].[ExchangeRate],
    [CampaignActivity].[ExcludeIfContactedInXDays],
    [CampaignActivity].[IgnoreInactiveListMembers],
    IgnoreInactiveListMembersPLTable.Value,
    [CampaignActivity].[ImportSequenceNumber],
    [CampaignActivity].[IsBilled],
    IsBilledPLTable.Value,
    [CampaignActivity].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [CampaignActivity].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    [CampaignActivity].[ModifiedBy],
    --[CampaignActivity].[ModifiedByDsc]
    0,
    [CampaignActivity].[ModifiedByName],
    [CampaignActivity].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[ModifiedOn], 
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
        [CampaignActivity].[ModifiedOn],
    [CampaignActivity].[ModifiedOnBehalfBy],
    --[CampaignActivity].[ModifiedOnBehalfByDsc]
    0,
    [CampaignActivity].[ModifiedOnBehalfByName],
    [CampaignActivity].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[OverriddenCreatedOn], 
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
        [CampaignActivity].[OverriddenCreatedOn],
    [CampaignActivity].[OwnerId],
    --[CampaignActivity].[OwnerIdDsc]
    0,
    [CampaignActivity].[OwnerIdName],
    [CampaignActivity].[OwnerIdType],
    [CampaignActivity].[OwnerIdYomiName],
    [CampaignActivity].[OwningBusinessUnit],
    [CampaignActivity].[OwningTeam],
    [CampaignActivity].[OwningUser],
    [CampaignActivity].[PriorityCode],
    PriorityCodePLTable.Value,
    [CampaignActivity].[ProcessId],
    [CampaignActivity].[RegardingObjectId],
    --[CampaignActivity].[RegardingObjectIdDsc]
    0,
    [CampaignActivity].[RegardingObjectIdName],
    [CampaignActivity].[RegardingObjectIdYomiName],
    [CampaignActivity].[RegardingObjectTypeCode],
    [CampaignActivity].[ScheduledDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[ScheduledEnd], 
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
        [CampaignActivity].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([CampaignActivity].[ScheduledStart], 
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
        [CampaignActivity].[ScheduledStart],
    [CampaignActivity].[ServiceId],
    [CampaignActivity].[StageId],
    [CampaignActivity].[StateCode],
    StateCodePLTable.Value,
    [CampaignActivity].[StatusCode],
    StatusCodePLTable.Value,
    [CampaignActivity].[Subcategory],
    [CampaignActivity].[Subject],
    [CampaignActivity].[TimeZoneRuleVersionNumber],
    [CampaignActivity].[TransactionCurrencyId],
    --[CampaignActivity].[TransactionCurrencyIdDsc]
    0,
    [CampaignActivity].[TransactionCurrencyIdName],
    [CampaignActivity].[TypeCode],
    TypeCodePLTable.Value,
    [CampaignActivity].[UTCConversionTimeZoneCode],
    [CampaignActivity].[VersionNumber],
   dbo.fn_GetNumberFormatString(t.CurrencyPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode),
   dbo.fn_GetNumberFormatString(o.PricingDecimalPrecision, us.NumberGroupFormat, us.NegativeCurrencyFormatCode, 1, case o.CurrencyDisplayOption when 0 then t.CurrencySymbol when 1 then t.ISOCurrencyCode end, us.CurrencyFormatCode)
from CampaignActivity
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left join TransactionCurrencyBase t on t.TransactionCurrencyId = [CampaignActivity].TransactionCurrencyId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 4402
		and [ActivityTypeCodePLTable].AttributeValue = [CampaignActivity].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ChannelTypeCodePLTable] on 
		([ChannelTypeCodePLTable].AttributeName = 'channeltypecode'
		and [ChannelTypeCodePLTable].ObjectTypeCode = 4402
		and [ChannelTypeCodePLTable].AttributeValue = [CampaignActivity].[ChannelTypeCode]
		and [ChannelTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DoNotSendOnOptOutPLTable] on 
		([DoNotSendOnOptOutPLTable].AttributeName = 'donotsendonoptout'
		and [DoNotSendOnOptOutPLTable].ObjectTypeCode = 4402
		and [DoNotSendOnOptOutPLTable].AttributeValue = [CampaignActivity].[DoNotSendOnOptOut]
		and [DoNotSendOnOptOutPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IgnoreInactiveListMembersPLTable] on 
		([IgnoreInactiveListMembersPLTable].AttributeName = 'ignoreinactivelistmembers'
		and [IgnoreInactiveListMembersPLTable].ObjectTypeCode = 4402
		and [IgnoreInactiveListMembersPLTable].AttributeValue = [CampaignActivity].[IgnoreInactiveListMembers]
		and [IgnoreInactiveListMembersPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 4402
		and [IsBilledPLTable].AttributeValue = [CampaignActivity].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 4402
		and [IsRegularActivityPLTable].AttributeValue = [CampaignActivity].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 4402
		and [IsWorkflowCreatedPLTable].AttributeValue = [CampaignActivity].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 4402
		and [PriorityCodePLTable].AttributeValue = [CampaignActivity].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4402
		and [StateCodePLTable].AttributeValue = [CampaignActivity].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4402
		and [StatusCodePLTable].AttributeValue = [CampaignActivity].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TypeCodePLTable] on 
		([TypeCodePLTable].AttributeName = 'typecode'
		and [TypeCodePLTable].ObjectTypeCode = 4402
		and [TypeCodePLTable].AttributeValue = [CampaignActivity].[TypeCode]
		and [TypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4200) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[CampaignActivity].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4200
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
		[CampaignActivity].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[CampaignActivity].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[CampaignActivity].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

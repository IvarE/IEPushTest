

--
-- report view for cgi_callguidechat
--
create view dbo.[Filteredcgi_callguidechat] (
    [activityid],
    [activitytypecode],
    [activitytypecodename],
    [actualdurationminutes],
    [actualend],
    [actualendutc],
    [actualstart],
    [actualstartutc],
    [cgi_callguideinfoid],
    [cgi_callguideinfoidname],
    [cgi_chatconversation],
    [cgi_createcase],
    [cgi_createcasename],
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
    [deliverylastattemptedon],
    [deliverylastattemptedonutc],
    [deliveryprioritycode],
    [deliveryprioritycodename],
    [description],
    [exchangerate],
    [importsequencenumber],
    [instancetypecode],
    [instancetypecodename],
    [isbilled],
    [isbilledname],
    [ismapiprivate],
    [ismapiprivatename],
    [isregularactivity],
    [isregularactivityname],
    [isworkflowcreated],
    [isworkflowcreatedname],
    [leftvoicemail],
    [leftvoicemailname],
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
    [postponeactivityprocessinguntil],
    [postponeactivityprocessinguntilutc],
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
    [sendermailboxid],
    [sendermailboxidname],
    [senton],
    [sentonutc],
    [seriesid],
    [serviceid],
    [serviceiddsc],
    [serviceidname],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subject],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [cgi_callguidechat].[ActivityId],
    [cgi_callguidechat].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [cgi_callguidechat].[ActualDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[ActualEnd], 
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
        [cgi_callguidechat].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[ActualStart], 
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
        [cgi_callguidechat].[ActualStart],
    [cgi_callguidechat].[cgi_CallguideInfoid],
    [cgi_callguidechat].[cgi_CallguideInfoidName],
    [cgi_callguidechat].[cgi_ChatConversation],
    [cgi_callguidechat].[cgi_CreateCase],
    cgi_CreateCasePLTable.Value,
    [cgi_callguidechat].[CreatedBy],
    --[cgi_callguidechat].[CreatedByDsc]
    0,
    [cgi_callguidechat].[CreatedByName],
    [cgi_callguidechat].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[CreatedOn], 
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
        [cgi_callguidechat].[CreatedOn],
    [cgi_callguidechat].[CreatedOnBehalfBy],
    --[cgi_callguidechat].[CreatedOnBehalfByDsc]
    0,
    [cgi_callguidechat].[CreatedOnBehalfByName],
    [cgi_callguidechat].[CreatedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[DeliveryLastAttemptedOn], 
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
        [cgi_callguidechat].[DeliveryLastAttemptedOn],
    [cgi_callguidechat].[DeliveryPriorityCode],
    DeliveryPriorityCodePLTable.Value,
    [cgi_callguidechat].[Description],
    [cgi_callguidechat].[ExchangeRate],
    [cgi_callguidechat].[ImportSequenceNumber],
    [cgi_callguidechat].[InstanceTypeCode],
    InstanceTypeCodePLTable.Value,
    [cgi_callguidechat].[IsBilled],
    IsBilledPLTable.Value,
    [cgi_callguidechat].[IsMapiPrivate],
    IsMapiPrivatePLTable.Value,
    [cgi_callguidechat].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [cgi_callguidechat].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    [cgi_callguidechat].[LeftVoiceMail],
    LeftVoiceMailPLTable.Value,
    [cgi_callguidechat].[ModifiedBy],
    --[cgi_callguidechat].[ModifiedByDsc]
    0,
    [cgi_callguidechat].[ModifiedByName],
    [cgi_callguidechat].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[ModifiedOn], 
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
        [cgi_callguidechat].[ModifiedOn],
    [cgi_callguidechat].[ModifiedOnBehalfBy],
    --[cgi_callguidechat].[ModifiedOnBehalfByDsc]
    0,
    [cgi_callguidechat].[ModifiedOnBehalfByName],
    [cgi_callguidechat].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[OverriddenCreatedOn], 
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
        [cgi_callguidechat].[OverriddenCreatedOn],
    [cgi_callguidechat].[OwnerId],
    --[cgi_callguidechat].[OwnerIdDsc]
    0,
    [cgi_callguidechat].[OwnerIdName],
    [cgi_callguidechat].[OwnerIdType],
    [cgi_callguidechat].[OwnerIdYomiName],
    [cgi_callguidechat].[OwningBusinessUnit],
    [cgi_callguidechat].[OwningTeam],
    [cgi_callguidechat].[OwningUser],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[PostponeActivityProcessingUntil], 
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
        [cgi_callguidechat].[PostponeActivityProcessingUntil],
    [cgi_callguidechat].[PriorityCode],
    PriorityCodePLTable.Value,
    [cgi_callguidechat].[ProcessId],
    [cgi_callguidechat].[RegardingObjectId],
    --[cgi_callguidechat].[RegardingObjectIdDsc]
    0,
    [cgi_callguidechat].[RegardingObjectIdName],
    [cgi_callguidechat].[RegardingObjectIdYomiName],
    [cgi_callguidechat].[RegardingObjectTypeCode],
    [cgi_callguidechat].[ScheduledDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[ScheduledEnd], 
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
        [cgi_callguidechat].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[ScheduledStart], 
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
        [cgi_callguidechat].[ScheduledStart],
    [cgi_callguidechat].[SenderMailboxId],
    [cgi_callguidechat].[SenderMailboxIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidechat].[SentOn], 
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
        [cgi_callguidechat].[SentOn],
    [cgi_callguidechat].[SeriesId],
    [cgi_callguidechat].[ServiceId],
    --[cgi_callguidechat].[ServiceIdDsc]
    0,
    [cgi_callguidechat].[ServiceIdName],
    [cgi_callguidechat].[StageId],
    [cgi_callguidechat].[StateCode],
    StateCodePLTable.Value,
    [cgi_callguidechat].[StatusCode],
    StatusCodePLTable.Value,
    [cgi_callguidechat].[Subject],
    [cgi_callguidechat].[TimeZoneRuleVersionNumber],
    [cgi_callguidechat].[TransactionCurrencyId],
    [cgi_callguidechat].[TransactionCurrencyIdName],
    [cgi_callguidechat].[UTCConversionTimeZoneCode],
    [cgi_callguidechat].[VersionNumber]
from cgi_callguidechat
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 10008
		and [ActivityTypeCodePLTable].AttributeValue = [cgi_callguidechat].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_CreateCasePLTable] on 
		([cgi_CreateCasePLTable].AttributeName = 'cgi_createcase'
		and [cgi_CreateCasePLTable].ObjectTypeCode = 10008
		and [cgi_CreateCasePLTable].AttributeValue = [cgi_callguidechat].[cgi_CreateCase]
		and [cgi_CreateCasePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DeliveryPriorityCodePLTable] on 
		([DeliveryPriorityCodePLTable].AttributeName = 'deliveryprioritycode'
		and [DeliveryPriorityCodePLTable].ObjectTypeCode = 10008
		and [DeliveryPriorityCodePLTable].AttributeValue = [cgi_callguidechat].[DeliveryPriorityCode]
		and [DeliveryPriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [InstanceTypeCodePLTable] on 
		([InstanceTypeCodePLTable].AttributeName = 'instancetypecode'
		and [InstanceTypeCodePLTable].ObjectTypeCode = 10008
		and [InstanceTypeCodePLTable].AttributeValue = [cgi_callguidechat].[InstanceTypeCode]
		and [InstanceTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 10008
		and [IsBilledPLTable].AttributeValue = [cgi_callguidechat].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsMapiPrivatePLTable] on 
		([IsMapiPrivatePLTable].AttributeName = 'ismapiprivate'
		and [IsMapiPrivatePLTable].ObjectTypeCode = 10008
		and [IsMapiPrivatePLTable].AttributeValue = [cgi_callguidechat].[IsMapiPrivate]
		and [IsMapiPrivatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 10008
		and [IsRegularActivityPLTable].AttributeValue = [cgi_callguidechat].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 10008
		and [IsWorkflowCreatedPLTable].AttributeValue = [cgi_callguidechat].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [LeftVoiceMailPLTable] on 
		([LeftVoiceMailPLTable].AttributeName = 'leftvoicemail'
		and [LeftVoiceMailPLTable].ObjectTypeCode = 10008
		and [LeftVoiceMailPLTable].AttributeValue = [cgi_callguidechat].[LeftVoiceMail]
		and [LeftVoiceMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 10008
		and [PriorityCodePLTable].AttributeValue = [cgi_callguidechat].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 10008
		and [StateCodePLTable].AttributeValue = [cgi_callguidechat].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 10008
		and [StatusCodePLTable].AttributeValue = [cgi_callguidechat].[StatusCode]
		and [StatusCodePLTable].LangId = 
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
	[cgi_callguidechat].OwnerId in 
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
		[cgi_callguidechat].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_callguidechat].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_callguidechat].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

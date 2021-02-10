

--
-- report view for cgi_callguidefacebook
--
create view dbo.[Filteredcgi_callguidefacebook] (
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
    [cgi_createcase],
    [cgi_createcasename],
    [cgi_facebookpost],
    [cgi_facebookurl],
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
    [cgi_callguidefacebook].[ActivityId],
    [cgi_callguidefacebook].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [cgi_callguidefacebook].[ActualDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[ActualEnd], 
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
        [cgi_callguidefacebook].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[ActualStart], 
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
        [cgi_callguidefacebook].[ActualStart],
    [cgi_callguidefacebook].[cgi_CallguideInfoid],
    [cgi_callguidefacebook].[cgi_CallguideInfoidName],
    [cgi_callguidefacebook].[cgi_CreateCase],
    cgi_CreateCasePLTable.Value,
    [cgi_callguidefacebook].[cgi_FaceBookPost],
    [cgi_callguidefacebook].[cgi_FacebookUrl],
    [cgi_callguidefacebook].[CreatedBy],
    --[cgi_callguidefacebook].[CreatedByDsc]
    0,
    [cgi_callguidefacebook].[CreatedByName],
    [cgi_callguidefacebook].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[CreatedOn], 
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
        [cgi_callguidefacebook].[CreatedOn],
    [cgi_callguidefacebook].[CreatedOnBehalfBy],
    --[cgi_callguidefacebook].[CreatedOnBehalfByDsc]
    0,
    [cgi_callguidefacebook].[CreatedOnBehalfByName],
    [cgi_callguidefacebook].[CreatedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[DeliveryLastAttemptedOn], 
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
        [cgi_callguidefacebook].[DeliveryLastAttemptedOn],
    [cgi_callguidefacebook].[DeliveryPriorityCode],
    DeliveryPriorityCodePLTable.Value,
    [cgi_callguidefacebook].[Description],
    [cgi_callguidefacebook].[ExchangeRate],
    [cgi_callguidefacebook].[ImportSequenceNumber],
    [cgi_callguidefacebook].[InstanceTypeCode],
    InstanceTypeCodePLTable.Value,
    [cgi_callguidefacebook].[IsBilled],
    IsBilledPLTable.Value,
    [cgi_callguidefacebook].[IsMapiPrivate],
    IsMapiPrivatePLTable.Value,
    [cgi_callguidefacebook].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [cgi_callguidefacebook].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    [cgi_callguidefacebook].[LeftVoiceMail],
    LeftVoiceMailPLTable.Value,
    [cgi_callguidefacebook].[ModifiedBy],
    --[cgi_callguidefacebook].[ModifiedByDsc]
    0,
    [cgi_callguidefacebook].[ModifiedByName],
    [cgi_callguidefacebook].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[ModifiedOn], 
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
        [cgi_callguidefacebook].[ModifiedOn],
    [cgi_callguidefacebook].[ModifiedOnBehalfBy],
    --[cgi_callguidefacebook].[ModifiedOnBehalfByDsc]
    0,
    [cgi_callguidefacebook].[ModifiedOnBehalfByName],
    [cgi_callguidefacebook].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[OverriddenCreatedOn], 
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
        [cgi_callguidefacebook].[OverriddenCreatedOn],
    [cgi_callguidefacebook].[OwnerId],
    --[cgi_callguidefacebook].[OwnerIdDsc]
    0,
    [cgi_callguidefacebook].[OwnerIdName],
    [cgi_callguidefacebook].[OwnerIdType],
    [cgi_callguidefacebook].[OwnerIdYomiName],
    [cgi_callguidefacebook].[OwningBusinessUnit],
    [cgi_callguidefacebook].[OwningTeam],
    [cgi_callguidefacebook].[OwningUser],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[PostponeActivityProcessingUntil], 
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
        [cgi_callguidefacebook].[PostponeActivityProcessingUntil],
    [cgi_callguidefacebook].[PriorityCode],
    PriorityCodePLTable.Value,
    [cgi_callguidefacebook].[ProcessId],
    [cgi_callguidefacebook].[RegardingObjectId],
    --[cgi_callguidefacebook].[RegardingObjectIdDsc]
    0,
    [cgi_callguidefacebook].[RegardingObjectIdName],
    [cgi_callguidefacebook].[RegardingObjectIdYomiName],
    [cgi_callguidefacebook].[RegardingObjectTypeCode],
    [cgi_callguidefacebook].[ScheduledDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[ScheduledEnd], 
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
        [cgi_callguidefacebook].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[ScheduledStart], 
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
        [cgi_callguidefacebook].[ScheduledStart],
    [cgi_callguidefacebook].[SenderMailboxId],
    [cgi_callguidefacebook].[SenderMailboxIdName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguidefacebook].[SentOn], 
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
        [cgi_callguidefacebook].[SentOn],
    [cgi_callguidefacebook].[SeriesId],
    [cgi_callguidefacebook].[ServiceId],
    --[cgi_callguidefacebook].[ServiceIdDsc]
    0,
    [cgi_callguidefacebook].[ServiceIdName],
    [cgi_callguidefacebook].[StageId],
    [cgi_callguidefacebook].[StateCode],
    StateCodePLTable.Value,
    [cgi_callguidefacebook].[StatusCode],
    StatusCodePLTable.Value,
    [cgi_callguidefacebook].[Subject],
    [cgi_callguidefacebook].[TimeZoneRuleVersionNumber],
    [cgi_callguidefacebook].[TransactionCurrencyId],
    [cgi_callguidefacebook].[TransactionCurrencyIdName],
    [cgi_callguidefacebook].[UTCConversionTimeZoneCode],
    [cgi_callguidefacebook].[VersionNumber]
from cgi_callguidefacebook
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 10009
		and [ActivityTypeCodePLTable].AttributeValue = [cgi_callguidefacebook].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_CreateCasePLTable] on 
		([cgi_CreateCasePLTable].AttributeName = 'cgi_createcase'
		and [cgi_CreateCasePLTable].ObjectTypeCode = 10009
		and [cgi_CreateCasePLTable].AttributeValue = [cgi_callguidefacebook].[cgi_CreateCase]
		and [cgi_CreateCasePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DeliveryPriorityCodePLTable] on 
		([DeliveryPriorityCodePLTable].AttributeName = 'deliveryprioritycode'
		and [DeliveryPriorityCodePLTable].ObjectTypeCode = 10009
		and [DeliveryPriorityCodePLTable].AttributeValue = [cgi_callguidefacebook].[DeliveryPriorityCode]
		and [DeliveryPriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [InstanceTypeCodePLTable] on 
		([InstanceTypeCodePLTable].AttributeName = 'instancetypecode'
		and [InstanceTypeCodePLTable].ObjectTypeCode = 10009
		and [InstanceTypeCodePLTable].AttributeValue = [cgi_callguidefacebook].[InstanceTypeCode]
		and [InstanceTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 10009
		and [IsBilledPLTable].AttributeValue = [cgi_callguidefacebook].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsMapiPrivatePLTable] on 
		([IsMapiPrivatePLTable].AttributeName = 'ismapiprivate'
		and [IsMapiPrivatePLTable].ObjectTypeCode = 10009
		and [IsMapiPrivatePLTable].AttributeValue = [cgi_callguidefacebook].[IsMapiPrivate]
		and [IsMapiPrivatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 10009
		and [IsRegularActivityPLTable].AttributeValue = [cgi_callguidefacebook].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 10009
		and [IsWorkflowCreatedPLTable].AttributeValue = [cgi_callguidefacebook].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [LeftVoiceMailPLTable] on 
		([LeftVoiceMailPLTable].AttributeName = 'leftvoicemail'
		and [LeftVoiceMailPLTable].ObjectTypeCode = 10009
		and [LeftVoiceMailPLTable].AttributeValue = [cgi_callguidefacebook].[LeftVoiceMail]
		and [LeftVoiceMailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 10009
		and [PriorityCodePLTable].AttributeValue = [cgi_callguidefacebook].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 10009
		and [StateCodePLTable].AttributeValue = [cgi_callguidefacebook].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 10009
		and [StatusCodePLTable].AttributeValue = [cgi_callguidefacebook].[StatusCode]
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
	[cgi_callguidefacebook].OwnerId in 
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
		[cgi_callguidefacebook].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_callguidefacebook].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_callguidefacebook].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

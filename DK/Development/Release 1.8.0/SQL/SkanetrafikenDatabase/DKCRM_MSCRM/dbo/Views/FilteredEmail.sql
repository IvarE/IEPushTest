

--
-- report view for email
--
create view dbo.[FilteredEmail] (
    [activityid],
    [activitytypecode],
    [activitytypecodename],
    [actualdurationminutes],
    [actualend],
    [actualendutc],
    [actualstart],
    [actualstartutc],
    [attachmentcount],
    [category],
    [cgi_ask_customer],
    [cgi_ask_customername],
    [cgi_attention],
    [cgi_attentionname],
    [cgi_bcc_emailrecipient],
    [cgi_bcc_emailrecipientname],
    [cgi_button_customer],
    [cgi_cc_emailrecipient],
    [cgi_cc_emailrecipientname],
    [cgi_email_recipient_id],
    [cgi_email_recipient_idname],
    [cgi_getfilelink],
    [cgi_getfilelinkname],
    [cgi_remittance],
    [cgi_remittancename],
    [cgi_sourcetype],
    [cgi_sourcetypename],
    [cgi_type],
    [compressed],
    [compressedname],
    [conversationindex],
    [correlationmethod],
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
    [deliveryattempts],
    [deliveryprioritycode],
    [deliveryprioritycodename],
    [deliveryreceiptrequested],
    [deliveryreceiptrequestedname],
    [description],
    [directioncode],
    [directioncodename],
    [emailsender],
    [emailsendername],
    [emailsenderobjecttypecode],
    [emailsenderyominame],
    [exchangerate],
    [importsequencenumber],
    [inreplyto],
    [isbilled],
    [isbilledname],
    [isregularactivity],
    [isregularactivityname],
    [isworkflowcreated],
    [isworkflowcreatedname],
    [messageid],
    [mimetype],
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
    [notifications],
    [notificationsname],
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
    [parentactivityid],
    [parentactivityidname],
    [postponeemailprocessinguntil],
    [postponeemailprocessinguntilutc],
    [prioritycode],
    [prioritycodename],
    [processid],
    [readreceiptrequested],
    [readreceiptrequestedname],
    [regardingobjectid],
    [regardingobjectiddsc],
    [regardingobjectidname],
    [regardingobjectidyominame],
    [regardingobjecttypecode],
    [safedescription],
    [scheduleddurationminutes],
    [scheduledend],
    [scheduledendutc],
    [scheduledstart],
    [scheduledstartutc],
    [sender],
    [sendermailboxid],
    [sendermailboxidname],
    [sendersaccount],
    [sendersaccountname],
    [sendersaccountobjecttypecode],
    [sendersaccountyominame],
    [senton],
    [sentonutc],
    [serviceid],
    [stageid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subcategory],
    [subject],
    [submittedby],
    [timezoneruleversionnumber],
    [torecipients],
    [trackingtoken],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [Email].[ActivityId],
    [Email].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [Email].[ActualDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[ActualEnd], 
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
        [Email].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[ActualStart], 
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
        [Email].[ActualStart],
    [Email].[AttachmentCount],
    [Email].[Category],
    [Email].[cgi_ask_customer],
    cgi_ask_customerPLTable.Value,
    [Email].[cgi_Attention],
    cgi_AttentionPLTable.Value,
    [Email].[cgi_bcc_emailrecipient],
    [Email].[cgi_bcc_emailrecipientName],
    [Email].[cgi_button_customer],
    [Email].[cgi_cc_emailrecipient],
    [Email].[cgi_cc_emailrecipientName],
    [Email].[cgi_email_recipient_Id],
    [Email].[cgi_email_recipient_IdName],
    [Email].[cgi_getfilelink],
    cgi_getfilelinkPLTable.Value,
    [Email].[cgi_Remittance],
    cgi_RemittancePLTable.Value,
    [Email].[cgi_SourceType],
    cgi_SourceTypePLTable.Value,
    [Email].[cgi_Type],
    [Email].[Compressed],
    CompressedPLTable.Value,
    [Email].[ConversationIndex],
    [Email].[CorrelationMethod],
    [Email].[CreatedBy],
    --[Email].[CreatedByDsc]
    0,
    [Email].[CreatedByName],
    [Email].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[CreatedOn], 
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
        [Email].[CreatedOn],
    [Email].[CreatedOnBehalfBy],
    --[Email].[CreatedOnBehalfByDsc]
    0,
    [Email].[CreatedOnBehalfByName],
    [Email].[CreatedOnBehalfByYomiName],
    [Email].[DeliveryAttempts],
    [Email].[DeliveryPriorityCode],
    DeliveryPriorityCodePLTable.Value,
    [Email].[DeliveryReceiptRequested],
    DeliveryReceiptRequestedPLTable.Value,
    [Email].[Description],
    [Email].[DirectionCode],
    DirectionCodePLTable.Value,
    [Email].[EmailSender],
    [Email].[EmailSenderName],
    [Email].[EmailSenderObjectTypeCode],
    [Email].[EmailSenderYomiName],
    [Email].[ExchangeRate],
    [Email].[ImportSequenceNumber],
    [Email].[InReplyTo],
    [Email].[IsBilled],
    IsBilledPLTable.Value,
    [Email].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [Email].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    [Email].[MessageId],
    [Email].[MimeType],
    [Email].[ModifiedBy],
    --[Email].[ModifiedByDsc]
    0,
    [Email].[ModifiedByName],
    [Email].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[ModifiedOn], 
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
        [Email].[ModifiedOn],
    [Email].[ModifiedOnBehalfBy],
    --[Email].[ModifiedOnBehalfByDsc]
    0,
    [Email].[ModifiedOnBehalfByName],
    [Email].[ModifiedOnBehalfByYomiName],
    [Email].[Notifications],
    NotificationsPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Email].[OverriddenCreatedOn], 
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
        [Email].[OverriddenCreatedOn],
    [Email].[OwnerId],
    --[Email].[OwnerIdDsc]
    0,
    [Email].[OwnerIdName],
    [Email].[OwnerIdType],
    [Email].[OwnerIdYomiName],
    [Email].[OwningBusinessUnit],
    [Email].[OwningTeam],
    [Email].[OwningUser],
    [Email].[ParentActivityId],
    [Email].[ParentActivityIdName],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[PostponeEmailProcessingUntil], 
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
        [Email].[PostponeEmailProcessingUntil],
    [Email].[PriorityCode],
    PriorityCodePLTable.Value,
    [Email].[ProcessId],
    [Email].[ReadReceiptRequested],
    ReadReceiptRequestedPLTable.Value,
    [Email].[RegardingObjectId],
    --[Email].[RegardingObjectIdDsc]
    0,
    [Email].[RegardingObjectIdName],
    [Email].[RegardingObjectIdYomiName],
    [Email].[RegardingObjectTypeCode],
    --[Email].[SafeDescription]
    cast(null as nvarchar),
    [Email].[ScheduledDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[ScheduledEnd], 
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
        [Email].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[ScheduledStart], 
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
        [Email].[ScheduledStart],
    [Email].[Sender],
    [Email].[SenderMailboxId],
    [Email].[SenderMailboxIdName],
    [Email].[SendersAccount],
    [Email].[SendersAccountName],
    [Email].[SendersAccountObjectTypeCode],
    [Email].[SendersAccountYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Email].[SentOn], 
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
        [Email].[SentOn],
    [Email].[ServiceId],
    [Email].[StageId],
    [Email].[StateCode],
    StateCodePLTable.Value,
    [Email].[StatusCode],
    StatusCodePLTable.Value,
    [Email].[Subcategory],
    [Email].[Subject],
    [Email].[SubmittedBy],
    [Email].[TimeZoneRuleVersionNumber],
    [Email].[ToRecipients],
    [Email].[TrackingToken],
    [Email].[TransactionCurrencyId],
    [Email].[TransactionCurrencyIdName],
    [Email].[UTCConversionTimeZoneCode],
    [Email].[VersionNumber]
from Email
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 4202
		and [ActivityTypeCodePLTable].AttributeValue = [Email].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_ask_customerPLTable] on 
		([cgi_ask_customerPLTable].AttributeName = 'cgi_ask_customer'
		and [cgi_ask_customerPLTable].ObjectTypeCode = 4202
		and [cgi_ask_customerPLTable].AttributeValue = [Email].[cgi_ask_customer]
		and [cgi_ask_customerPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_AttentionPLTable] on 
		([cgi_AttentionPLTable].AttributeName = 'cgi_attention'
		and [cgi_AttentionPLTable].ObjectTypeCode = 4202
		and [cgi_AttentionPLTable].AttributeValue = [Email].[cgi_Attention]
		and [cgi_AttentionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_getfilelinkPLTable] on 
		([cgi_getfilelinkPLTable].AttributeName = 'cgi_getfilelink'
		and [cgi_getfilelinkPLTable].ObjectTypeCode = 4202
		and [cgi_getfilelinkPLTable].AttributeValue = [Email].[cgi_getfilelink]
		and [cgi_getfilelinkPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_RemittancePLTable] on 
		([cgi_RemittancePLTable].AttributeName = 'cgi_remittance'
		and [cgi_RemittancePLTable].ObjectTypeCode = 4202
		and [cgi_RemittancePLTable].AttributeValue = [Email].[cgi_Remittance]
		and [cgi_RemittancePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_SourceTypePLTable] on 
		([cgi_SourceTypePLTable].AttributeName = 'cgi_sourcetype'
		and [cgi_SourceTypePLTable].ObjectTypeCode = 4202
		and [cgi_SourceTypePLTable].AttributeValue = [Email].[cgi_SourceType]
		and [cgi_SourceTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CompressedPLTable] on 
		([CompressedPLTable].AttributeName = 'compressed'
		and [CompressedPLTable].ObjectTypeCode = 4202
		and [CompressedPLTable].AttributeValue = [Email].[Compressed]
		and [CompressedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DeliveryPriorityCodePLTable] on 
		([DeliveryPriorityCodePLTable].AttributeName = 'deliveryprioritycode'
		and [DeliveryPriorityCodePLTable].ObjectTypeCode = 4202
		and [DeliveryPriorityCodePLTable].AttributeValue = [Email].[DeliveryPriorityCode]
		and [DeliveryPriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DeliveryReceiptRequestedPLTable] on 
		([DeliveryReceiptRequestedPLTable].AttributeName = 'deliveryreceiptrequested'
		and [DeliveryReceiptRequestedPLTable].ObjectTypeCode = 4202
		and [DeliveryReceiptRequestedPLTable].AttributeValue = [Email].[DeliveryReceiptRequested]
		and [DeliveryReceiptRequestedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DirectionCodePLTable] on 
		([DirectionCodePLTable].AttributeName = 'directioncode'
		and [DirectionCodePLTable].ObjectTypeCode = 4202
		and [DirectionCodePLTable].AttributeValue = [Email].[DirectionCode]
		and [DirectionCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 4202
		and [IsBilledPLTable].AttributeValue = [Email].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 4202
		and [IsRegularActivityPLTable].AttributeValue = [Email].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 4202
		and [IsWorkflowCreatedPLTable].AttributeValue = [Email].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [NotificationsPLTable] on 
		([NotificationsPLTable].AttributeName = 'notifications'
		and [NotificationsPLTable].ObjectTypeCode = 4202
		and [NotificationsPLTable].AttributeValue = [Email].[Notifications]
		and [NotificationsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 4202
		and [PriorityCodePLTable].AttributeValue = [Email].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ReadReceiptRequestedPLTable] on 
		([ReadReceiptRequestedPLTable].AttributeName = 'readreceiptrequested'
		and [ReadReceiptRequestedPLTable].ObjectTypeCode = 4202
		and [ReadReceiptRequestedPLTable].AttributeValue = [Email].[ReadReceiptRequested]
		and [ReadReceiptRequestedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4202
		and [StateCodePLTable].AttributeValue = [Email].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4202
		and [StatusCodePLTable].AttributeValue = [Email].[StatusCode]
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
	[Email].OwnerId in 
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
		[Email].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Email].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Email].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

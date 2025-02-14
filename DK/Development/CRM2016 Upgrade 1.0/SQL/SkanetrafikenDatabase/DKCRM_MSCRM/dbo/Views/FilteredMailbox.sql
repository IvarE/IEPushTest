﻿

--
-- report view for mailbox
--
create view dbo.[FilteredMailbox] (
    [actdeliverymethod],
    [actdeliverymethodname],
    [actstatus],
    [actstatusname],
    [allowemailconnectortousecredentials],
    [allowemailconnectortousecredentialsname],
    [averagetotalduration],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [emailaddress],
    [emailrouteraccessapproval],
    [emailrouteraccessapprovalname],
    [emailserverprofile],
    [emailserverprofilename],
    [enabledforact],
    [enabledforactname],
    [enabledforincomingemail],
    [enabledforincomingemailname],
    [enabledforoutgoingemail],
    [enabledforoutgoingemailname],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [ewsurl],
    [exchangesyncstatexml],
    [hostid],
    [incomingemaildeliverymethod],
    [incomingemaildeliverymethodname],
    [incomingemailstatus],
    [incomingemailstatusname],
    [isactsyncorgflagset],
    [isemailaddressapprovedbyo365admin],
    [isforwardmailbox],
    [isforwardmailboxname],
    [ispasswordset],
    [itemsfailedforlastsync],
    [itemsprocessedforlastsync],
    [lastautodiscoveredon],
    [lastautodiscoveredonutc],
    [lastduration],
    [lastsuccessfulsynccompletedon],
    [lastsuccessfulsynccompletedonutc],
    [lastsyncerror],
    [lastsyncerrorcode],
    [lastsyncerrorcount],
    [lastsyncerrormachinename],
    [lastsyncerroroccurredon],
    [lastsyncerroroccurredonutc],
    [lastsyncstartedon],
    [lastsyncstartedonutc],
    [mailboxid],
    [mailboxprocessingcontext],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [noactcount],
    [noemailcount],
    [organizationid],
    [organizationidname],
    [orgmarkedasprimaryforexchangesync],
    [outgoingemaildeliverymethod],
    [outgoingemaildeliverymethodname],
    [outgoingemailstatus],
    [outgoingemailstatusname],
    [ownerid],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningbusinessunitname],
    [owningteam],
    [owninguser],
    [postponemailboxprocessinguntil],
    [postponemailboxprocessinguntilutc],
    [postponesendinguntil],
    [postponesendinguntilutc],
    [postponetestemailconfigurationuntil],
    [postponetestemailconfigurationuntilutc],
    [processanddeleteemails],
    [processanddeleteemailsname],
    [processedtimes],
    [processemailreceivedafter],
    [processemailreceivedafterutc],
    [receivingpostponeduntil],
    [receivingpostponeduntilutc],
    [receivingpostponeduntilforact],
    [receivingpostponeduntilforactutc],
    [regardingobjectid],
    [regardingobjectidname],
    [regardingobjecttypecode],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [testemailconfigurationretrycount],
    [testemailconfigurationscheduled],
    [testemailconfigurationscheduledname],
    [testmailboxaccesscompletedon],
    [testmailboxaccesscompletedonutc],
    [timezoneruleversionnumber],
    [transientfailurecount],
    [undeliverablefolder],
    [username],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [Mailbox].[ACTDeliveryMethod],
    ACTDeliveryMethodPLTable.Value,
    [Mailbox].[ACTStatus],
    ACTStatusPLTable.Value,
    [Mailbox].[AllowEmailConnectorToUseCredentials],
    AllowEmailConnectorToUseCredentialsPLTable.Value,
    [Mailbox].[AverageTotalDuration],
    [Mailbox].[CreatedBy],
    [Mailbox].[CreatedByName],
    [Mailbox].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[CreatedOn], 
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
        [Mailbox].[CreatedOn],
    [Mailbox].[CreatedOnBehalfBy],
    [Mailbox].[CreatedOnBehalfByName],
    [Mailbox].[CreatedOnBehalfByYomiName],
    [Mailbox].[EmailAddress],
    [Mailbox].[EmailRouterAccessApproval],
    EmailRouterAccessApprovalPLTable.Value,
    [Mailbox].[EmailServerProfile],
    [Mailbox].[EmailServerProfileName],
    [Mailbox].[EnabledForACT],
    EnabledForACTPLTable.Value,
    [Mailbox].[EnabledForIncomingEmail],
    EnabledForIncomingEmailPLTable.Value,
    [Mailbox].[EnabledForOutgoingEmail],
    EnabledForOutgoingEmailPLTable.Value,
    --[Mailbox].[EntityImage]
    cast(null as varbinary),
    [Mailbox].[EntityImageId],
    [Mailbox].[EntityImage_Timestamp],
    [Mailbox].[EntityImage_URL],
    [Mailbox].[EWSURL],
    [Mailbox].[ExchangeSyncStateXml],
    [Mailbox].[HostId],
    [Mailbox].[IncomingEmailDeliveryMethod],
    IncomingEmailDeliveryMethodPLTable.Value,
    [Mailbox].[IncomingEmailStatus],
    IncomingEmailStatusPLTable.Value,
    [Mailbox].[IsACTSyncOrgFlagSet],
    [Mailbox].[IsEmailAddressApprovedByO365Admin],
    [Mailbox].[IsForwardMailbox],
    IsForwardMailboxPLTable.Value,
    [Mailbox].[IsPasswordSet],
    [Mailbox].[ItemsFailedForLastSync],
    [Mailbox].[ItemsProcessedForLastSync],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[LastAutoDiscoveredOn], 
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
        [Mailbox].[LastAutoDiscoveredOn],
    [Mailbox].[LastDuration],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[LastSuccessfulSyncCompletedOn], 
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
        [Mailbox].[LastSuccessfulSyncCompletedOn],
    [Mailbox].[LastSyncError],
    [Mailbox].[LastSyncErrorCode],
    [Mailbox].[LastSyncErrorCount],
    [Mailbox].[LastSyncErrorMachineName],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[LastSyncErrorOccurredOn], 
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
        [Mailbox].[LastSyncErrorOccurredOn],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[LastSyncStartedOn], 
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
        [Mailbox].[LastSyncStartedOn],
    [Mailbox].[MailboxId],
    [Mailbox].[MailboxProcessingContext],
    [Mailbox].[ModifiedBy],
    [Mailbox].[ModifiedByName],
    [Mailbox].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[ModifiedOn], 
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
        [Mailbox].[ModifiedOn],
    [Mailbox].[ModifiedOnBehalfBy],
    [Mailbox].[ModifiedOnBehalfByName],
    [Mailbox].[ModifiedOnBehalfByYomiName],
    [Mailbox].[Name],
    [Mailbox].[NoACTCount],
    [Mailbox].[NoEmailCount],
    [Mailbox].[OrganizationId],
    [Mailbox].[OrganizationIdName],
    [Mailbox].[OrgMarkedAsPrimaryForExchangeSync],
    [Mailbox].[OutgoingEmailDeliveryMethod],
    OutgoingEmailDeliveryMethodPLTable.Value,
    [Mailbox].[OutgoingEmailStatus],
    OutgoingEmailStatusPLTable.Value,
    [Mailbox].[OwnerId],
    [Mailbox].[OwnerIdName],
    [Mailbox].[OwnerIdType],
    [Mailbox].[OwnerIdYomiName],
    [Mailbox].[OwningBusinessUnit],
    [Mailbox].[OwningBusinessUnitName],
    [Mailbox].[OwningTeam],
    [Mailbox].[OwningUser],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[PostponeMailboxProcessingUntil], 
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
        [Mailbox].[PostponeMailboxProcessingUntil],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[PostponeSendingUntil], 
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
        [Mailbox].[PostponeSendingUntil],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[PostponeTestEmailConfigurationUntil], 
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
        [Mailbox].[PostponeTestEmailConfigurationUntil],
    [Mailbox].[ProcessAndDeleteEmails],
    ProcessAndDeleteEmailsPLTable.Value,
    [Mailbox].[ProcessedTimes],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[ProcessEmailReceivedAfter], 
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
        [Mailbox].[ProcessEmailReceivedAfter],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[ReceivingPostponedUntil], 
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
        [Mailbox].[ReceivingPostponedUntil],
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[ReceivingPostponedUntilForACT], 
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
        [Mailbox].[ReceivingPostponedUntilForACT],
    [Mailbox].[RegardingObjectId],
    [Mailbox].[RegardingObjectIdName],
    [Mailbox].[RegardingObjectTypeCode],
    [Mailbox].[StateCode],
    StateCodePLTable.Value,
    [Mailbox].[StatusCode],
    StatusCodePLTable.Value,
    [Mailbox].[TestEmailConfigurationRetryCount],
    [Mailbox].[TestEmailConfigurationScheduled],
    TestEmailConfigurationScheduledPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Mailbox].[TestMailboxAccessCompletedOn], 
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
        [Mailbox].[TestMailboxAccessCompletedOn],
    [Mailbox].[TimeZoneRuleVersionNumber],
    [Mailbox].[TransientFailureCount],
    [Mailbox].[UndeliverableFolder],
    [Mailbox].[Username],
    [Mailbox].[UTCConversionTimeZoneCode],
    [Mailbox].[VersionNumber]
from Mailbox
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ACTDeliveryMethodPLTable] on 
		([ACTDeliveryMethodPLTable].AttributeName = 'actdeliverymethod'
		and [ACTDeliveryMethodPLTable].ObjectTypeCode = 9606
		and [ACTDeliveryMethodPLTable].AttributeValue = [Mailbox].[ACTDeliveryMethod]
		and [ACTDeliveryMethodPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ACTStatusPLTable] on 
		([ACTStatusPLTable].AttributeName = 'actstatus'
		and [ACTStatusPLTable].ObjectTypeCode = 9606
		and [ACTStatusPLTable].AttributeValue = [Mailbox].[ACTStatus]
		and [ACTStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [AllowEmailConnectorToUseCredentialsPLTable] on 
		([AllowEmailConnectorToUseCredentialsPLTable].AttributeName = 'allowemailconnectortousecredentials'
		and [AllowEmailConnectorToUseCredentialsPLTable].ObjectTypeCode = 9606
		and [AllowEmailConnectorToUseCredentialsPLTable].AttributeValue = [Mailbox].[AllowEmailConnectorToUseCredentials]
		and [AllowEmailConnectorToUseCredentialsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [EmailRouterAccessApprovalPLTable] on 
		([EmailRouterAccessApprovalPLTable].AttributeName = 'emailrouteraccessapproval'
		and [EmailRouterAccessApprovalPLTable].ObjectTypeCode = 9606
		and [EmailRouterAccessApprovalPLTable].AttributeValue = [Mailbox].[EmailRouterAccessApproval]
		and [EmailRouterAccessApprovalPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [EnabledForACTPLTable] on 
		([EnabledForACTPLTable].AttributeName = 'enabledforact'
		and [EnabledForACTPLTable].ObjectTypeCode = 9606
		and [EnabledForACTPLTable].AttributeValue = [Mailbox].[EnabledForACT]
		and [EnabledForACTPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [EnabledForIncomingEmailPLTable] on 
		([EnabledForIncomingEmailPLTable].AttributeName = 'enabledforincomingemail'
		and [EnabledForIncomingEmailPLTable].ObjectTypeCode = 9606
		and [EnabledForIncomingEmailPLTable].AttributeValue = [Mailbox].[EnabledForIncomingEmail]
		and [EnabledForIncomingEmailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [EnabledForOutgoingEmailPLTable] on 
		([EnabledForOutgoingEmailPLTable].AttributeName = 'enabledforoutgoingemail'
		and [EnabledForOutgoingEmailPLTable].ObjectTypeCode = 9606
		and [EnabledForOutgoingEmailPLTable].AttributeValue = [Mailbox].[EnabledForOutgoingEmail]
		and [EnabledForOutgoingEmailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IncomingEmailDeliveryMethodPLTable] on 
		([IncomingEmailDeliveryMethodPLTable].AttributeName = 'incomingemaildeliverymethod'
		and [IncomingEmailDeliveryMethodPLTable].ObjectTypeCode = 9606
		and [IncomingEmailDeliveryMethodPLTable].AttributeValue = [Mailbox].[IncomingEmailDeliveryMethod]
		and [IncomingEmailDeliveryMethodPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IncomingEmailStatusPLTable] on 
		([IncomingEmailStatusPLTable].AttributeName = 'incomingemailstatus'
		and [IncomingEmailStatusPLTable].ObjectTypeCode = 9606
		and [IncomingEmailStatusPLTable].AttributeValue = [Mailbox].[IncomingEmailStatus]
		and [IncomingEmailStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsForwardMailboxPLTable] on 
		([IsForwardMailboxPLTable].AttributeName = 'isforwardmailbox'
		and [IsForwardMailboxPLTable].ObjectTypeCode = 9606
		and [IsForwardMailboxPLTable].AttributeValue = [Mailbox].[IsForwardMailbox]
		and [IsForwardMailboxPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [OutgoingEmailDeliveryMethodPLTable] on 
		([OutgoingEmailDeliveryMethodPLTable].AttributeName = 'outgoingemaildeliverymethod'
		and [OutgoingEmailDeliveryMethodPLTable].ObjectTypeCode = 9606
		and [OutgoingEmailDeliveryMethodPLTable].AttributeValue = [Mailbox].[OutgoingEmailDeliveryMethod]
		and [OutgoingEmailDeliveryMethodPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [OutgoingEmailStatusPLTable] on 
		([OutgoingEmailStatusPLTable].AttributeName = 'outgoingemailstatus'
		and [OutgoingEmailStatusPLTable].ObjectTypeCode = 9606
		and [OutgoingEmailStatusPLTable].AttributeValue = [Mailbox].[OutgoingEmailStatus]
		and [OutgoingEmailStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ProcessAndDeleteEmailsPLTable] on 
		([ProcessAndDeleteEmailsPLTable].AttributeName = 'processanddeleteemails'
		and [ProcessAndDeleteEmailsPLTable].ObjectTypeCode = 9606
		and [ProcessAndDeleteEmailsPLTable].AttributeValue = [Mailbox].[ProcessAndDeleteEmails]
		and [ProcessAndDeleteEmailsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 9606
		and [StateCodePLTable].AttributeValue = [Mailbox].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 9606
		and [StatusCodePLTable].AttributeValue = [Mailbox].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TestEmailConfigurationScheduledPLTable] on 
		([TestEmailConfigurationScheduledPLTable].AttributeName = 'testemailconfigurationscheduled'
		and [TestEmailConfigurationScheduledPLTable].ObjectTypeCode = 9606
		and [TestEmailConfigurationScheduledPLTable].AttributeValue = [Mailbox].[TestEmailConfigurationScheduled]
		and [TestEmailConfigurationScheduledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9606) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Mailbox].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 9606
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
		[Mailbox].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9606)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Mailbox].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Mailbox].[MailboxId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 9606 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

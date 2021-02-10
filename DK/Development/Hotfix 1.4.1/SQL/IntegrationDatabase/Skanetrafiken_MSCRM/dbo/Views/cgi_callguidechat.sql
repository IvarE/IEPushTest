


--
-- base view for cgi_callguidechat
--
create view dbo.[cgi_callguidechat]
 (
    -- logical attributes
    [CreatedOnBehalfByYomiName],
    [CreatedByName],
    [ModifiedOnBehalfByName],
    [SenderMailboxIdName],
    [ModifiedByYomiName],
    [ModifiedOnBehalfByYomiName],
    [ServiceIdName],
    [CreatedOnBehalfByName],
    [CreatedByYomiName],
    [TransactionCurrencyIdName],
    [ModifiedByName],
    [cgi_CallguideInfoidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [ActivityId],
    [ActivityTypeCode],
    [ActualDurationMinutes],
    [ActualEnd],
    [ActualStart],
    [CreatedBy],
    [CreatedOn],
    [CreatedOnBehalfBy],
    [DeliveryLastAttemptedOn],
    [DeliveryPriorityCode],
    [Description],
    [ExchangeRate],
    [InstanceTypeCode],
    [IsBilled],
    [IsMapiPrivate],
    [IsRegularActivity],
    [IsWorkflowCreated],
    [LeftVoiceMail],
    [ModifiedBy],
    [ModifiedOn],
    [ModifiedOnBehalfBy],
    [OwningBusinessUnit],
    [PostponeActivityProcessingUntil],
    [PriorityCode],
    [ProcessId],
    [RegardingObjectId],
    [ScheduledDurationMinutes],
    [ScheduledEnd],
    [ScheduledStart],
    [SenderMailboxId],
    [SentOn],
    [SeriesId],
    [ServiceId],
    [StageId],
    [StateCode],
    [StatusCode],
    [Subject],
    [TimeZoneRuleVersionNumber],
    [TransactionCurrencyId],
    [UTCConversionTimeZoneCode],
    [VersionNumber],
    [RegardingObjectIdName],
    [RegardingObjectIdYomiName],
    [RegardingObjectTypeCode],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [cgi_ChatConversation],
    [cgi_CreateCase],
    [cgi_CallguideInfoid]
) with view_metadata as
select
    -- logical attributes
    [cgi_callguidechat_systemuser_createdonbehalfby].[YomiFullName],
    [cgi_callguidechat_systemuser_createdby].[FullName],
    [cgi_callguidechat_systemuser_modifiedonbehalfby].[FullName],
    [cgi_callguidechat_mailbox_sendermailboxid].[Name],
    [cgi_callguidechat_systemuser_modifiedby].[YomiFullName],
    [cgi_callguidechat_systemuser_modifiedonbehalfby].[YomiFullName],
    [cgi_callguidechat_service_serviceid].[Name],
    [cgi_callguidechat_systemuser_createdonbehalfby].[FullName],
    [cgi_callguidechat_systemuser_createdby].[YomiFullName],
    [cgi_callguidechat_transactioncurrency_transactioncurrencyid].[CurrencyName],
    [cgi_callguidechat_systemuser_modifiedby].[FullName],
    [cgi_cgi_callguideinfo_cgi_callguidechat_CallguideInfoid].[cgi_callguideinfoname],

    -- ownership entries
    OwnerId = [ActivityPointerBase].OwnerId,
    OwnerName = XXowner.Name,
    OwnerYomiName =  XXowner.YomiName,
    OwnerDsc = 0, -- DSC is removed, stub it to 0
    OwnerIdType = XXowner.OwnerIdType,
    OwningUser = case 
 		when XXowner.OwnerIdType= 8 then XXowner.OwnerId
		else null
		end,
    OwningTeam = case 
 		when XXowner.OwnerIdType= 9 then XXowner.OwnerId
		else null
		end,

    -- physical attribute
    [ActivityPointerBase].[ActivityId],
    [ActivityPointerBase].[ActivityTypeCode],
    [ActivityPointerBase].[ActualDurationMinutes],
    [ActivityPointerBase].[ActualEnd],
    [ActivityPointerBase].[ActualStart],
    [ActivityPointerBase].[CreatedBy],
    [ActivityPointerBase].[CreatedOn],
    [ActivityPointerBase].[CreatedOnBehalfBy],
    [ActivityPointerBase].[DeliveryLastAttemptedOn],
    [ActivityPointerBase].[DeliveryPriorityCode],
    [ActivityPointerBase].[Description],
    [ActivityPointerBase].[ExchangeRate],
    [ActivityPointerBase].[InstanceTypeCode],
    [ActivityPointerBase].[IsBilled],
    [ActivityPointerBase].[IsMapiPrivate],
    [ActivityPointerBase].[IsRegularActivity],
    [ActivityPointerBase].[IsWorkflowCreated],
    [ActivityPointerBase].[LeftVoiceMail],
    [ActivityPointerBase].[ModifiedBy],
    [ActivityPointerBase].[ModifiedOn],
    [ActivityPointerBase].[ModifiedOnBehalfBy],
    [ActivityPointerBase].[OwningBusinessUnit],
    [ActivityPointerBase].[PostponeActivityProcessingUntil],
    [ActivityPointerBase].[PriorityCode],
    [ActivityPointerBase].[ProcessId],
    [ActivityPointerBase].[RegardingObjectId],
    [ActivityPointerBase].[ScheduledDurationMinutes],
    [ActivityPointerBase].[ScheduledEnd],
    [ActivityPointerBase].[ScheduledStart],
    [ActivityPointerBase].[SenderMailboxId],
    [ActivityPointerBase].[SentOn],
    [ActivityPointerBase].[SeriesId],
    [ActivityPointerBase].[ServiceId],
    [ActivityPointerBase].[StageId],
    [ActivityPointerBase].[StateCode],
    [ActivityPointerBase].[StatusCode],
    [ActivityPointerBase].[Subject],
    [ActivityPointerBase].[TimeZoneRuleVersionNumber],
    [ActivityPointerBase].[TransactionCurrencyId],
    [ActivityPointerBase].[UTCConversionTimeZoneCode],
    [ActivityPointerBase].[VersionNumber],
    [ActivityPointerBase].[RegardingObjectIdName],
    [ActivityPointerBase].[RegardingObjectIdYomiName],
    [ActivityPointerBase].[RegardingObjectTypeCode],
    [ActivityPointerBase].[ImportSequenceNumber_10008],
    [ActivityPointerBase].[OverriddenCreatedOn_10008],
    [cgi_callguidechatBase].[cgi_ChatConversation],
    [cgi_callguidechatBase].[cgi_CreateCase],
    [cgi_callguidechatBase].[cgi_CallguideInfoid]
from [ActivityPointerBase] 
    inner join [cgi_callguidechatBase] on ([ActivityPointerBase].[ActivityId] = [cgi_callguidechatBase].[ActivityId] and [ActivityPointerBase].[ActivityTypeCode] = 10008)
    left join [MailboxBase] [cgi_callguidechat_mailbox_sendermailboxid] on ([ActivityPointerBase].[SenderMailboxId] = [cgi_callguidechat_mailbox_sendermailboxid].[MailboxId])
    left join [ServiceBase] [cgi_callguidechat_service_serviceid] on ([ActivityPointerBase].[ServiceId] = [cgi_callguidechat_service_serviceid].[ServiceId])
    left join [SystemUserBase] [cgi_callguidechat_systemuser_createdby] with(nolock) on ([ActivityPointerBase].[CreatedBy] = [cgi_callguidechat_systemuser_createdby].[SystemUserId])
    left join [SystemUserBase] [cgi_callguidechat_systemuser_createdonbehalfby] with(nolock) on ([ActivityPointerBase].[CreatedOnBehalfBy] = [cgi_callguidechat_systemuser_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [cgi_callguidechat_systemuser_modifiedby] with(nolock) on ([ActivityPointerBase].[ModifiedBy] = [cgi_callguidechat_systemuser_modifiedby].[SystemUserId])
    left join [SystemUserBase] [cgi_callguidechat_systemuser_modifiedonbehalfby] with(nolock) on ([ActivityPointerBase].[ModifiedOnBehalfBy] = [cgi_callguidechat_systemuser_modifiedonbehalfby].[SystemUserId])
    left join [TransactionCurrencyBase] [cgi_callguidechat_transactioncurrency_transactioncurrencyid] on ([ActivityPointerBase].[TransactionCurrencyId] = [cgi_callguidechat_transactioncurrency_transactioncurrencyid].[TransactionCurrencyId])
    left join [cgi_callguideinfoBase] [cgi_cgi_callguideinfo_cgi_callguidechat_CallguideInfoid] on ([cgi_callguidechatBase].[cgi_CallguideInfoid] = [cgi_cgi_callguideinfo_cgi_callguidechat_CallguideInfoid].[cgi_callguideinfoId])
    left join OwnerBase XXowner with(nolock) on ([ActivityPointerBase].OwnerId = XXowner.OwnerId)
where [ActivityPointerBase].[ActivityTypeCode] = 10008
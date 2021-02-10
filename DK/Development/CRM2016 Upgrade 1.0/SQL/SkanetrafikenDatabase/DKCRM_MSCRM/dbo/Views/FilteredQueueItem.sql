

--
-- report view for queueitem
--
create view dbo.[FilteredQueueItem] (
    [cgi_action_date],
    [cgi_arrival_date],
    [cgi_arrival_dateutc],
    [cgi_casdet_row1_cat3],
    [cgi_caseorigincode],
    [cgi_case_remittance],
    [cgi_case_type],
    [cgi_customer],
    [cgi_customer_email],
    [cgi_customer_number],
    [cgi_customer_telephonenumber],
    [cgi_customer_telephonenumber_mobile],
    [cgi_customer_telephonenumber_work],
    [cgi_incidentstagecode],
    [cgi_priority],
    [cgi_refund_approvaltype],
    [cgi_refund_status],
    [cgi_refund_type],
    [cgi_refund_typename],
    [cgi_reimbursement_name],
    [cgi_resolve_by],
    [cgi_soc_sec_number],
    [cgi_ticketnumber],
    [cgi_travelinformationline],
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
    [enteredon],
    [enteredonutc],
    [exchangerate],
    [importsequencenumber],
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
    [objectid],
    [objectidname],
    [objectidtypecode],
    [objecttypecode],
    [objecttypecodename],
    [organizationid],
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [priority],
    [queueid],
    [queueidname],
    [queueitemid],
    [sender],
    [state],
    [statecode],
    [statecodename],
    [status],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [title],
    [torecipients],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber],
    [workerid],
    [workeridmodifiedon],
    [workeridmodifiedonutc],
    [workeridname],
    [workeridtype],
    [workeridyominame]
) with view_metadata as
select
    [QueueItem].[cgi_action_date],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[cgi_arrival_date], 
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
        [QueueItem].[cgi_arrival_date],
    [QueueItem].[cgi_casdet_row1_cat3],
    [QueueItem].[cgi_caseorigincode],
    [QueueItem].[cgi_case_remittance],
    [QueueItem].[cgi_case_type],
    [QueueItem].[cgi_customer],
    [QueueItem].[cgi_customer_email],
    [QueueItem].[cgi_customer_number],
    [QueueItem].[cgi_customer_telephonenumber],
    [QueueItem].[cgi_customer_telephonenumber_mobile],
    [QueueItem].[cgi_customer_telephonenumber_work],
    [QueueItem].[cgi_incidentstagecode],
    [QueueItem].[cgi_priority],
    [QueueItem].[cgi_refund_approvaltype],
    [QueueItem].[cgi_refund_status],
    [QueueItem].[cgi_refund_type],
    [QueueItem].[cgi_refund_typename],
    [QueueItem].[cgi_reimbursement_name],
    [QueueItem].[cgi_resolve_by],
    [QueueItem].[cgi_soc_sec_number],
    [QueueItem].[cgi_ticketnumber],
    [QueueItem].[cgi_travelinformationline],
    [QueueItem].[CreatedBy],
    --[QueueItem].[CreatedByDsc]
    0,
    [QueueItem].[CreatedByName],
    [QueueItem].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[CreatedOn], 
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
        [QueueItem].[CreatedOn],
    [QueueItem].[CreatedOnBehalfBy],
    --[QueueItem].[CreatedOnBehalfByDsc]
    0,
    [QueueItem].[CreatedOnBehalfByName],
    [QueueItem].[CreatedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[EnteredOn], 
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
        [QueueItem].[EnteredOn],
    [QueueItem].[ExchangeRate],
    [QueueItem].[ImportSequenceNumber],
    [QueueItem].[ModifiedBy],
    --[QueueItem].[ModifiedByDsc]
    0,
    [QueueItem].[ModifiedByName],
    [QueueItem].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[ModifiedOn], 
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
        [QueueItem].[ModifiedOn],
    [QueueItem].[ModifiedOnBehalfBy],
    --[QueueItem].[ModifiedOnBehalfByDsc]
    0,
    [QueueItem].[ModifiedOnBehalfByName],
    [QueueItem].[ModifiedOnBehalfByYomiName],
    [QueueItem].[ObjectId],
    [QueueItem].[ObjectIdName],
    [QueueItem].[ObjectIdTypeCode],
    [QueueItem].[ObjectTypeCode],
    ObjectTypeCodePLTable.Value,
    [QueueItem].[OrganizationId],
    --[QueueItem].[OrganizationIdDsc]
    0,
    [QueueItem].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[OverriddenCreatedOn], 
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
        [QueueItem].[OverriddenCreatedOn],
    [QueueItem].[OwnerId],
    [QueueItem].[OwnerIdType],
    [QueueItem].[OwningBusinessUnit],
    [QueueItem].[OwningUser],
    [QueueItem].[Priority],
    [QueueItem].[QueueId],
    [QueueItem].[QueueIdName],
    [QueueItem].[QueueItemId],
    [QueueItem].[Sender],
    [QueueItem].[State],
    [QueueItem].[StateCode],
    StateCodePLTable.Value,
    [QueueItem].[Status],
    [QueueItem].[StatusCode],
    StatusCodePLTable.Value,
    [QueueItem].[TimeZoneRuleVersionNumber],
    [QueueItem].[Title],
    [QueueItem].[ToRecipients],
    [QueueItem].[TransactionCurrencyId],
    [QueueItem].[TransactionCurrencyIdName],
    [QueueItem].[UTCConversionTimeZoneCode],
    [QueueItem].[VersionNumber],
    [QueueItem].[WorkerId],
    dbo.fn_UTCToTzSpecificLocalTime([QueueItem].[WorkerIdModifiedOn], 
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
        [QueueItem].[WorkerIdModifiedOn],
    [QueueItem].[WorkerIdName],
    [QueueItem].[WorkerIdType],
    [QueueItem].[WorkerIdYomiName]
from QueueItem
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ObjectTypeCodePLTable] on 
		([ObjectTypeCodePLTable].AttributeName = 'objecttypecode'
		and [ObjectTypeCodePLTable].ObjectTypeCode = 2029
		and [ObjectTypeCodePLTable].AttributeValue = [QueueItem].[ObjectTypeCode]
		and [ObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 2029
		and [StateCodePLTable].AttributeValue = [QueueItem].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 2029
		and [StatusCodePLTable].AttributeValue = [QueueItem].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(2020) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[QueueItem].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 2020
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
		[QueueItem].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 2020)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[QueueItem].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[QueueItem].[QueueId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 2020 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

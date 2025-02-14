﻿

--
-- report view for bulkoperation
--
create view dbo.[FilteredBulkOperation] (
    [activityid],
    [activitytypecode],
    [activitytypecodename],
    [actualdurationminutes],
    [actualend],
    [actualendutc],
    [actualstart],
    [actualstartutc],
    [bulkoperationnumber],
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
    [createdrecordtypecode],
    [createdrecordtypecodename],
    [description],
    [errornumber],
    [failurecount],
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
    [operationtypecode],
    [operationtypecodename],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [parameters],
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
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subject],
    [successcount],
    [targetedrecordtypecode],
    [targetedrecordtypecodename],
    [targetmemberscount],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [BulkOperation].[ActivityId],
    [BulkOperation].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [BulkOperation].[ActualDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[ActualEnd], 
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
        [BulkOperation].[ActualEnd],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[ActualStart], 
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
        [BulkOperation].[ActualStart],
    [BulkOperation].[BulkOperationNumber],
    [BulkOperation].[CreatedBy],
    --[BulkOperation].[CreatedByDsc]
    0,
    [BulkOperation].[CreatedByName],
    [BulkOperation].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[CreatedOn], 
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
        [BulkOperation].[CreatedOn],
    [BulkOperation].[CreatedOnBehalfBy],
    --[BulkOperation].[CreatedOnBehalfByDsc]
    0,
    [BulkOperation].[CreatedOnBehalfByName],
    [BulkOperation].[CreatedOnBehalfByYomiName],
    [BulkOperation].[CreatedRecordTypeCode],
    CreatedRecordTypeCodePLTable.Value,
    [BulkOperation].[Description],
    [BulkOperation].[ErrorNumber],
    [BulkOperation].[FailureCount],
    [BulkOperation].[IsBilled],
    IsBilledPLTable.Value,
    [BulkOperation].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [BulkOperation].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    [BulkOperation].[ModifiedBy],
    --[BulkOperation].[ModifiedByDsc]
    0,
    [BulkOperation].[ModifiedByName],
    [BulkOperation].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[ModifiedOn], 
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
        [BulkOperation].[ModifiedOn],
    [BulkOperation].[ModifiedOnBehalfBy],
    --[BulkOperation].[ModifiedOnBehalfByDsc]
    0,
    [BulkOperation].[ModifiedOnBehalfByName],
    [BulkOperation].[ModifiedOnBehalfByYomiName],
    [BulkOperation].[OperationTypeCode],
    OperationTypeCodePLTable.Value,
    [BulkOperation].[OwnerId],
    --[BulkOperation].[OwnerIdDsc]
    0,
    [BulkOperation].[OwnerIdName],
    [BulkOperation].[OwnerIdType],
    [BulkOperation].[OwnerIdYomiName],
    [BulkOperation].[OwningBusinessUnit],
    [BulkOperation].[OwningTeam],
    [BulkOperation].[OwningUser],
    [BulkOperation].[Parameters],
    [BulkOperation].[RegardingObjectId],
    --[BulkOperation].[RegardingObjectIdDsc]
    0,
    [BulkOperation].[RegardingObjectIdName],
    [BulkOperation].[RegardingObjectIdYomiName],
    [BulkOperation].[RegardingObjectTypeCode],
    [BulkOperation].[ScheduledDurationMinutes],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[ScheduledEnd], 
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
        [BulkOperation].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([BulkOperation].[ScheduledStart], 
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
        [BulkOperation].[ScheduledStart],
    [BulkOperation].[ServiceId],
    [BulkOperation].[StateCode],
    StateCodePLTable.Value,
    [BulkOperation].[StatusCode],
    StatusCodePLTable.Value,
    [BulkOperation].[Subject],
    [BulkOperation].[SuccessCount],
    [BulkOperation].[TargetedRecordTypeCode],
    TargetedRecordTypeCodePLTable.Value,
    [BulkOperation].[TargetMembersCount],
    [BulkOperation].[TimeZoneRuleVersionNumber],
    [BulkOperation].[UTCConversionTimeZoneCode],
    [BulkOperation].[VersionNumber]
from BulkOperation
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 4406
		and [ActivityTypeCodePLTable].AttributeValue = [BulkOperation].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CreatedRecordTypeCodePLTable] on 
		([CreatedRecordTypeCodePLTable].AttributeName = 'createdrecordtypecode'
		and [CreatedRecordTypeCodePLTable].ObjectTypeCode = 4406
		and [CreatedRecordTypeCodePLTable].AttributeValue = [BulkOperation].[CreatedRecordTypeCode]
		and [CreatedRecordTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 4406
		and [IsBilledPLTable].AttributeValue = [BulkOperation].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 4406
		and [IsRegularActivityPLTable].AttributeValue = [BulkOperation].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 4406
		and [IsWorkflowCreatedPLTable].AttributeValue = [BulkOperation].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [OperationTypeCodePLTable] on 
		([OperationTypeCodePLTable].AttributeName = 'operationtypecode'
		and [OperationTypeCodePLTable].ObjectTypeCode = 4406
		and [OperationTypeCodePLTable].AttributeValue = [BulkOperation].[OperationTypeCode]
		and [OperationTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4406
		and [StateCodePLTable].AttributeValue = [BulkOperation].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4406
		and [StatusCodePLTable].AttributeValue = [BulkOperation].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TargetedRecordTypeCodePLTable] on 
		([TargetedRecordTypeCodePLTable].AttributeName = 'targetedrecordtypecode'
		and [TargetedRecordTypeCodePLTable].ObjectTypeCode = 4406
		and [TargetedRecordTypeCodePLTable].AttributeValue = [BulkOperation].[TargetedRecordTypeCode]
		and [TargetedRecordTypeCodePLTable].LangId = 
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
	[BulkOperation].OwnerId in 
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
		[BulkOperation].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[BulkOperation].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[BulkOperation].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

﻿

--
-- report view for bulkdeleteoperation
--
create view dbo.[FilteredBulkDeleteOperation] (
    [asyncoperationid],
    [bulkdeleteoperationid],
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
    [failurecount],
    [isrecurring],
    [isrecurringname],
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
    [nextrun],
    [nextrunutc],
    [orderedquerysetxml],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [processingqeindex],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [successcount],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode]
) with view_metadata as
select
    [BulkDeleteOperation].[AsyncOperationId],
    [BulkDeleteOperation].[BulkDeleteOperationId],
    [BulkDeleteOperation].[CreatedBy],
    --[BulkDeleteOperation].[CreatedByDsc]
    0,
    [BulkDeleteOperation].[CreatedByName],
    [BulkDeleteOperation].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([BulkDeleteOperation].[CreatedOn], 
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
        [BulkDeleteOperation].[CreatedOn],
    [BulkDeleteOperation].[CreatedOnBehalfBy],
    --[BulkDeleteOperation].[CreatedOnBehalfByDsc]
    0,
    [BulkDeleteOperation].[CreatedOnBehalfByName],
    [BulkDeleteOperation].[CreatedOnBehalfByYomiName],
    [BulkDeleteOperation].[FailureCount],
    [BulkDeleteOperation].[IsRecurring],
    IsRecurringPLTable.Value,
    [BulkDeleteOperation].[ModifiedBy],
    --[BulkDeleteOperation].[ModifiedByDsc]
    0,
    [BulkDeleteOperation].[ModifiedByName],
    [BulkDeleteOperation].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([BulkDeleteOperation].[ModifiedOn], 
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
        [BulkDeleteOperation].[ModifiedOn],
    [BulkDeleteOperation].[ModifiedOnBehalfBy],
    --[BulkDeleteOperation].[ModifiedOnBehalfByDsc]
    0,
    [BulkDeleteOperation].[ModifiedOnBehalfByName],
    [BulkDeleteOperation].[ModifiedOnBehalfByYomiName],
    [BulkDeleteOperation].[Name],
    dbo.fn_UTCToTzSpecificLocalTime([BulkDeleteOperation].[NextRun], 
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
        [BulkDeleteOperation].[NextRun],
    [BulkDeleteOperation].[OrderedQuerySetXml],
    [BulkDeleteOperation].[OwnerId],
    [BulkDeleteOperation].[OwnerIdType],
    [BulkDeleteOperation].[OwningBusinessUnit],
    [BulkDeleteOperation].[OwningUser],
    [BulkDeleteOperation].[ProcessingQEIndex],
    [BulkDeleteOperation].[StateCode],
    StateCodePLTable.Value,
    [BulkDeleteOperation].[StatusCode],
    StatusCodePLTable.Value,
    [BulkDeleteOperation].[SuccessCount],
    [BulkDeleteOperation].[TimeZoneRuleVersionNumber],
    [BulkDeleteOperation].[UTCConversionTimeZoneCode]
from BulkDeleteOperation
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsRecurringPLTable] on 
		([IsRecurringPLTable].AttributeName = 'isrecurring'
		and [IsRecurringPLTable].ObjectTypeCode = 4424
		and [IsRecurringPLTable].AttributeValue = [BulkDeleteOperation].[IsRecurring]
		and [IsRecurringPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4424
		and [StateCodePLTable].AttributeValue = [BulkDeleteOperation].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4424
		and [StatusCodePLTable].AttributeValue = [BulkDeleteOperation].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4700) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[BulkDeleteOperation].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4700
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
		[BulkDeleteOperation].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4700)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[BulkDeleteOperation].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[BulkDeleteOperation].[AsyncOperationId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4700 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

﻿

--
-- report view for importlog
--
create view dbo.[FilteredImportLog] (
    [additionalinfo],
    [columnvalue],
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
    [errordescription],
    [errornumber],
    [headercolumn],
    [importdataid],
    [importdataiddsc],
    [importdataidname],
    [importfileid],
    [importfileiddsc],
    [importfileidname],
    [importlogid],
    [linenumber],
    [logphasecode],
    [logphasecodename],
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
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [sequencenumber],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename]
) with view_metadata as
select
    [ImportLog].[AdditionalInfo],
    [ImportLog].[ColumnValue],
    [ImportLog].[CreatedBy],
    --[ImportLog].[CreatedByDsc]
    0,
    [ImportLog].[CreatedByName],
    [ImportLog].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ImportLog].[CreatedOn], 
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
        [ImportLog].[CreatedOn],
    [ImportLog].[CreatedOnBehalfBy],
    --[ImportLog].[CreatedOnBehalfByDsc]
    0,
    [ImportLog].[CreatedOnBehalfByName],
    [ImportLog].[CreatedOnBehalfByYomiName],
    [ImportLog].[ErrorDescription],
    [ImportLog].[ErrorNumber],
    [ImportLog].[HeaderColumn],
    [ImportLog].[ImportDataId],
    --[ImportLog].[ImportDataIdDsc]
    0,
    [ImportLog].[ImportDataIdName],
    [ImportLog].[ImportFileId],
    --[ImportLog].[ImportFileIdDsc]
    0,
    [ImportLog].[ImportFileIdName],
    [ImportLog].[ImportLogId],
    [ImportLog].[LineNumber],
    [ImportLog].[LogPhaseCode],
    LogPhaseCodePLTable.Value,
    [ImportLog].[ModifiedBy],
    --[ImportLog].[ModifiedByDsc]
    0,
    [ImportLog].[ModifiedByName],
    [ImportLog].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ImportLog].[ModifiedOn], 
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
        [ImportLog].[ModifiedOn],
    [ImportLog].[ModifiedOnBehalfBy],
    --[ImportLog].[ModifiedOnBehalfByDsc]
    0,
    [ImportLog].[ModifiedOnBehalfByName],
    [ImportLog].[ModifiedOnBehalfByYomiName],
    [ImportLog].[OwnerId],
    --[ImportLog].[OwnerIdDsc]
    0,
    [ImportLog].[OwnerIdName],
    [ImportLog].[OwnerIdType],
    [ImportLog].[OwnerIdYomiName],
    [ImportLog].[OwningBusinessUnit],
    [ImportLog].[OwningTeam],
    [ImportLog].[OwningUser],
    [ImportLog].[SequenceNumber],
    [ImportLog].[StateCode],
    StateCodePLTable.Value,
    [ImportLog].[StatusCode],
    StatusCodePLTable.Value
from ImportLog
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [LogPhaseCodePLTable] on 
		([LogPhaseCodePLTable].AttributeName = 'logphasecode'
		and [LogPhaseCodePLTable].ObjectTypeCode = 4423
		and [LogPhaseCodePLTable].AttributeValue = [ImportLog].[LogPhaseCode]
		and [LogPhaseCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4423
		and [StateCodePLTable].AttributeValue = [ImportLog].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4423
		and [StatusCodePLTable].AttributeValue = [ImportLog].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4423) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[ImportLog].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4423
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
		[ImportLog].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4423)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ImportLog].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ImportLog].[ImportLogId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4423 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

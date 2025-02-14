﻿

--
-- report view for import
--
create view dbo.[FilteredImport] (
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
    [emailaddress],
    [importid],
    [isimport],
    [isimportname],
    [modecode],
    [modecodename],
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
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [sendnotification],
    [sendnotificationname],
    [sequence],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename]
) with view_metadata as
select
    [Import].[CreatedBy],
    --[Import].[CreatedByDsc]
    0,
    [Import].[CreatedByName],
    [Import].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Import].[CreatedOn], 
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
        [Import].[CreatedOn],
    [Import].[CreatedOnBehalfBy],
    --[Import].[CreatedOnBehalfByDsc]
    0,
    [Import].[CreatedOnBehalfByName],
    [Import].[CreatedOnBehalfByYomiName],
    [Import].[EMailAddress],
    [Import].[ImportId],
    [Import].[IsImport],
    IsImportPLTable.Value,
    [Import].[ModeCode],
    ModeCodePLTable.Value,
    [Import].[ModifiedBy],
    --[Import].[ModifiedByDsc]
    0,
    [Import].[ModifiedByName],
    [Import].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Import].[ModifiedOn], 
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
        [Import].[ModifiedOn],
    [Import].[ModifiedOnBehalfBy],
    --[Import].[ModifiedOnBehalfByDsc]
    0,
    [Import].[ModifiedOnBehalfByName],
    [Import].[ModifiedOnBehalfByYomiName],
    [Import].[Name],
    [Import].[OwnerId],
    --[Import].[OwnerIdDsc]
    0,
    [Import].[OwnerIdName],
    [Import].[OwnerIdType],
    [Import].[OwnerIdYomiName],
    [Import].[OwningBusinessUnit],
    [Import].[OwningTeam],
    [Import].[OwningUser],
    [Import].[SendNotification],
    SendNotificationPLTable.Value,
    [Import].[Sequence],
    [Import].[StateCode],
    StateCodePLTable.Value,
    [Import].[StatusCode],
    StatusCodePLTable.Value
from Import
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsImportPLTable] on 
		([IsImportPLTable].AttributeName = 'isimport'
		and [IsImportPLTable].ObjectTypeCode = 4410
		and [IsImportPLTable].AttributeValue = [Import].[IsImport]
		and [IsImportPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ModeCodePLTable] on 
		([ModeCodePLTable].AttributeName = 'modecode'
		and [ModeCodePLTable].ObjectTypeCode = 4410
		and [ModeCodePLTable].AttributeValue = [Import].[ModeCode]
		and [ModeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SendNotificationPLTable] on 
		([SendNotificationPLTable].AttributeName = 'sendnotification'
		and [SendNotificationPLTable].ObjectTypeCode = 4410
		and [SendNotificationPLTable].AttributeValue = [Import].[SendNotification]
		and [SendNotificationPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4410
		and [StateCodePLTable].AttributeValue = [Import].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4410
		and [StatusCodePLTable].AttributeValue = [Import].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4410) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Import].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4410
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
		[Import].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4410)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Import].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Import].[ImportId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4410 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

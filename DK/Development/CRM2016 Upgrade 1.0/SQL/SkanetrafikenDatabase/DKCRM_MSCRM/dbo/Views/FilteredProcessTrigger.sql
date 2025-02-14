﻿

--
-- report view for processtrigger
--
create view dbo.[FilteredProcessTrigger] (
    [componentstate],
    [controlname],
    [controltype],
    [controltypename],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [event],
    [formid],
    [formidname],
    [iscustomizable],
    [ismanaged],
    [ismanagedname],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [overwritetime],
    [overwritetimeutc],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [primaryentitytypecode],
    [primaryentitytypecodename],
    [processid],
    [processidname],
    [processtriggerid],
    [processtriggeridunique],
    [solutionid],
    [versionnumber]
) with view_metadata as
select
    [ProcessTrigger].[ComponentState],
    [ProcessTrigger].[ControlName],
    [ProcessTrigger].[ControlType],
    ControlTypePLTable.Value,
    [ProcessTrigger].[CreatedBy],
    [ProcessTrigger].[CreatedByName],
    [ProcessTrigger].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ProcessTrigger].[CreatedOn], 
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
        [ProcessTrigger].[CreatedOn],
    [ProcessTrigger].[CreatedOnBehalfBy],
    [ProcessTrigger].[CreatedOnBehalfByName],
    [ProcessTrigger].[CreatedOnBehalfByYomiName],
    [ProcessTrigger].[Event],
    [ProcessTrigger].[FormId],
    [ProcessTrigger].[FormIdName],
    [ProcessTrigger].[IsCustomizable],
    [ProcessTrigger].[IsManaged],
    IsManagedPLTable.Value,
    [ProcessTrigger].[ModifiedBy],
    [ProcessTrigger].[ModifiedByName],
    [ProcessTrigger].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ProcessTrigger].[ModifiedOn], 
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
        [ProcessTrigger].[ModifiedOn],
    [ProcessTrigger].[ModifiedOnBehalfBy],
    [ProcessTrigger].[ModifiedOnBehalfByName],
    [ProcessTrigger].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ProcessTrigger].[OverwriteTime], 
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
        [ProcessTrigger].[OverwriteTime],
    [ProcessTrigger].[OwnerId],
    [ProcessTrigger].[OwnerIdType],
    [ProcessTrigger].[OwningBusinessUnit],
    [ProcessTrigger].[OwningUser],
    [ProcessTrigger].[PrimaryEntityTypeCode],
    PrimaryEntityTypeCodePLTable.Value,
    [ProcessTrigger].[ProcessId],
    [ProcessTrigger].[ProcessIdName],
    [ProcessTrigger].[ProcessTriggerId],
    [ProcessTrigger].[ProcessTriggerIdUnique],
    [ProcessTrigger].[SolutionId],
    [ProcessTrigger].[VersionNumber]
from ProcessTrigger
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ControlTypePLTable] on 
		([ControlTypePLTable].AttributeName = 'controltype'
		and [ControlTypePLTable].ObjectTypeCode = 4712
		and [ControlTypePLTable].AttributeValue = [ProcessTrigger].[ControlType]
		and [ControlTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 4712
		and [IsManagedPLTable].AttributeValue = [ProcessTrigger].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PrimaryEntityTypeCodePLTable] on 
		([PrimaryEntityTypeCodePLTable].AttributeName = 'primaryentitytypecode'
		and [PrimaryEntityTypeCodePLTable].ObjectTypeCode = 4712
		and [PrimaryEntityTypeCodePLTable].AttributeValue = [ProcessTrigger].[PrimaryEntityTypeCode]
		and [PrimaryEntityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4703) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[ProcessTrigger].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4703
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
		[ProcessTrigger].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4703)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ProcessTrigger].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ProcessTrigger].[ProcessId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4703 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

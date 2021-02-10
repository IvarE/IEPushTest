

--
-- report view for cgi_callguideinfo
--
create view dbo.[Filteredcgi_callguideinfo] (
    [cgi_agentname],
    [cgi_aphonenumber],
    [cgi_bphonenumber],
    [cgi_callguideinfoid],
    [cgi_callguideinfoname],
    [cgi_callguidesessionid],
    [cgi_chatcustomeralias],
    [cgi_cid],
    [cgi_contactsourcetype],
    [cgi_duration],
    [cgi_errandtasktype],
    [cgi_queuetime],
    [cgi_screenpopchoice],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [importsequencenumber],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
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
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [cgi_callguideinfo].[cgi_AgentName],
    [cgi_callguideinfo].[cgi_APhoneNumber],
    [cgi_callguideinfo].[cgi_BPhoneNumber],
    [cgi_callguideinfo].[cgi_callguideinfoId],
    [cgi_callguideinfo].[cgi_callguideinfoname],
    [cgi_callguideinfo].[cgi_CallguideSessionID],
    [cgi_callguideinfo].[cgi_chatcustomeralias],
    [cgi_callguideinfo].[cgi_Cid],
    [cgi_callguideinfo].[cgi_ContactSourceType],
    [cgi_callguideinfo].[cgi_Duration],
    [cgi_callguideinfo].[cgi_ErrandTaskType],
    [cgi_callguideinfo].[cgi_QueueTime],
    [cgi_callguideinfo].[cgi_ScreenPopChoice],
    [cgi_callguideinfo].[CreatedBy],
    [cgi_callguideinfo].[CreatedByName],
    [cgi_callguideinfo].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguideinfo].[CreatedOn], 
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
        [cgi_callguideinfo].[CreatedOn],
    [cgi_callguideinfo].[CreatedOnBehalfBy],
    [cgi_callguideinfo].[CreatedOnBehalfByName],
    [cgi_callguideinfo].[CreatedOnBehalfByYomiName],
    [cgi_callguideinfo].[ImportSequenceNumber],
    [cgi_callguideinfo].[ModifiedBy],
    [cgi_callguideinfo].[ModifiedByName],
    [cgi_callguideinfo].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguideinfo].[ModifiedOn], 
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
        [cgi_callguideinfo].[ModifiedOn],
    [cgi_callguideinfo].[ModifiedOnBehalfBy],
    [cgi_callguideinfo].[ModifiedOnBehalfByName],
    [cgi_callguideinfo].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_callguideinfo].[OverriddenCreatedOn], 
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
        [cgi_callguideinfo].[OverriddenCreatedOn],
    [cgi_callguideinfo].[OwnerId],
    --[cgi_callguideinfo].[OwnerIdDsc]
    0,
    [cgi_callguideinfo].[OwnerIdName],
    [cgi_callguideinfo].[OwnerIdType],
    [cgi_callguideinfo].[OwnerIdYomiName],
    [cgi_callguideinfo].[OwningBusinessUnit],
    [cgi_callguideinfo].[OwningTeam],
    [cgi_callguideinfo].[OwningUser],
    [cgi_callguideinfo].[statecode],
    statecodePLTable.Value,
    [cgi_callguideinfo].[statuscode],
    statuscodePLTable.Value,
    [cgi_callguideinfo].[TimeZoneRuleVersionNumber],
    [cgi_callguideinfo].[UTCConversionTimeZoneCode],
    [cgi_callguideinfo].[VersionNumber]
from cgi_callguideinfo
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10010
		and [statecodePLTable].AttributeValue = [cgi_callguideinfo].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10010
		and [statuscodePLTable].AttributeValue = [cgi_callguideinfo].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10010) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_callguideinfo].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10010
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
		[cgi_callguideinfo].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10010)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_callguideinfo].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_callguideinfo].[cgi_callguideinfoId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10010 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

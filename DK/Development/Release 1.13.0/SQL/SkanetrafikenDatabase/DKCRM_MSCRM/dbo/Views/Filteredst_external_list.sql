

--
-- report view for st_external_list
--
create view dbo.[Filteredst_external_list] (
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
    [st_contact],
    [st_contactname],
    [st_contactyominame],
    [st_external_listid],
    [st_marketinglistid],
    [st_marketinglistidname],
    [st_name],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [st_external_list].[CreatedBy],
    [st_external_list].[CreatedByName],
    [st_external_list].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_external_list].[CreatedOn], 
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
        [st_external_list].[CreatedOn],
    [st_external_list].[CreatedOnBehalfBy],
    [st_external_list].[CreatedOnBehalfByName],
    [st_external_list].[CreatedOnBehalfByYomiName],
    [st_external_list].[ImportSequenceNumber],
    [st_external_list].[ModifiedBy],
    [st_external_list].[ModifiedByName],
    [st_external_list].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_external_list].[ModifiedOn], 
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
        [st_external_list].[ModifiedOn],
    [st_external_list].[ModifiedOnBehalfBy],
    [st_external_list].[ModifiedOnBehalfByName],
    [st_external_list].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_external_list].[OverriddenCreatedOn], 
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
        [st_external_list].[OverriddenCreatedOn],
    [st_external_list].[OwnerId],
    --[st_external_list].[OwnerIdDsc]
    0,
    [st_external_list].[OwnerIdName],
    [st_external_list].[OwnerIdType],
    [st_external_list].[OwnerIdYomiName],
    [st_external_list].[OwningBusinessUnit],
    [st_external_list].[OwningTeam],
    [st_external_list].[OwningUser],
    [st_external_list].[statecode],
    statecodePLTable.Value,
    [st_external_list].[statuscode],
    statuscodePLTable.Value,
    [st_external_list].[st_Contact],
    [st_external_list].[st_ContactName],
    [st_external_list].[st_ContactYomiName],
    [st_external_list].[st_external_listId],
    [st_external_list].[st_MarketingListId],
    [st_external_list].[st_MarketingListIdName],
    [st_external_list].[st_name],
    [st_external_list].[TimeZoneRuleVersionNumber],
    [st_external_list].[UTCConversionTimeZoneCode],
    [st_external_list].[VersionNumber]
from st_external_list
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10064
		and [statecodePLTable].AttributeValue = [st_external_list].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10064
		and [statuscodePLTable].AttributeValue = [st_external_list].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10064) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[st_external_list].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10064
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
		[st_external_list].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10064)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[st_external_list].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[st_external_list].[st_external_listId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10064 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

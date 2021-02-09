

--
-- report view for st_externallist
--
create view dbo.[Filteredst_externallist] (
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
    [st_contactlookup],
    [st_contactlookupname],
    [st_contactlookupyominame],
    [st_externallistid],
    [st_marketinglist],
    [st_marketinglistname],
    [st_name],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [st_externallist].[CreatedBy],
    [st_externallist].[CreatedByName],
    [st_externallist].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_externallist].[CreatedOn], 
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
        [st_externallist].[CreatedOn],
    [st_externallist].[CreatedOnBehalfBy],
    [st_externallist].[CreatedOnBehalfByName],
    [st_externallist].[CreatedOnBehalfByYomiName],
    [st_externallist].[ImportSequenceNumber],
    [st_externallist].[ModifiedBy],
    [st_externallist].[ModifiedByName],
    [st_externallist].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_externallist].[ModifiedOn], 
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
        [st_externallist].[ModifiedOn],
    [st_externallist].[ModifiedOnBehalfBy],
    [st_externallist].[ModifiedOnBehalfByName],
    [st_externallist].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_externallist].[OverriddenCreatedOn], 
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
        [st_externallist].[OverriddenCreatedOn],
    [st_externallist].[OwnerId],
    --[st_externallist].[OwnerIdDsc]
    0,
    [st_externallist].[OwnerIdName],
    [st_externallist].[OwnerIdType],
    [st_externallist].[OwnerIdYomiName],
    [st_externallist].[OwningBusinessUnit],
    [st_externallist].[OwningTeam],
    [st_externallist].[OwningUser],
    [st_externallist].[statecode],
    statecodePLTable.Value,
    [st_externallist].[statuscode],
    statuscodePLTable.Value,
    [st_externallist].[st_ContactLookup],
    [st_externallist].[st_ContactLookupName],
    [st_externallist].[st_ContactLookupYomiName],
    [st_externallist].[st_externallistId],
    [st_externallist].[st_MarketingList],
    [st_externallist].[st_MarketingListName],
    [st_externallist].[st_name],
    [st_externallist].[TimeZoneRuleVersionNumber],
    [st_externallist].[UTCConversionTimeZoneCode],
    [st_externallist].[VersionNumber]
from st_externallist
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10059
		and [statecodePLTable].AttributeValue = [st_externallist].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10059
		and [statuscodePLTable].AttributeValue = [st_externallist].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10059) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[st_externallist].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10059
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
		[st_externallist].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10059)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[st_externallist].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[st_externallist].[st_externallistId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10059 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

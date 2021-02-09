

--
-- report view for cgi_routingaction
--
create view dbo.[Filteredcgi_routingaction] (
    [cgi_action],
    [cgi_actionname],
    [cgi_routingactionid],
    [cgi_routingactionname],
    [cgi_routingactionnumber],
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
    [cgi_routingaction].[cgi_Action],
    cgi_ActionPLTable.Value,
    [cgi_routingaction].[cgi_routingactionId],
    [cgi_routingaction].[cgi_routingactionname],
    [cgi_routingaction].[cgi_RoutingActionNumber],
    [cgi_routingaction].[CreatedBy],
    [cgi_routingaction].[CreatedByName],
    [cgi_routingaction].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_routingaction].[CreatedOn], 
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
        [cgi_routingaction].[CreatedOn],
    [cgi_routingaction].[CreatedOnBehalfBy],
    [cgi_routingaction].[CreatedOnBehalfByName],
    [cgi_routingaction].[CreatedOnBehalfByYomiName],
    [cgi_routingaction].[ImportSequenceNumber],
    [cgi_routingaction].[ModifiedBy],
    [cgi_routingaction].[ModifiedByName],
    [cgi_routingaction].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_routingaction].[ModifiedOn], 
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
        [cgi_routingaction].[ModifiedOn],
    [cgi_routingaction].[ModifiedOnBehalfBy],
    [cgi_routingaction].[ModifiedOnBehalfByName],
    [cgi_routingaction].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_routingaction].[OverriddenCreatedOn], 
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
        [cgi_routingaction].[OverriddenCreatedOn],
    [cgi_routingaction].[OwnerId],
    --[cgi_routingaction].[OwnerIdDsc]
    0,
    [cgi_routingaction].[OwnerIdName],
    [cgi_routingaction].[OwnerIdType],
    [cgi_routingaction].[OwnerIdYomiName],
    [cgi_routingaction].[OwningBusinessUnit],
    [cgi_routingaction].[OwningTeam],
    [cgi_routingaction].[OwningUser],
    [cgi_routingaction].[statecode],
    statecodePLTable.Value,
    [cgi_routingaction].[statuscode],
    statuscodePLTable.Value,
    [cgi_routingaction].[TimeZoneRuleVersionNumber],
    [cgi_routingaction].[UTCConversionTimeZoneCode],
    [cgi_routingaction].[VersionNumber]
from cgi_routingaction
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_ActionPLTable] on 
		([cgi_ActionPLTable].AttributeName = 'cgi_action'
		and [cgi_ActionPLTable].ObjectTypeCode = 10024
		and [cgi_ActionPLTable].AttributeValue = [cgi_routingaction].[cgi_Action]
		and [cgi_ActionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10024
		and [statecodePLTable].AttributeValue = [cgi_routingaction].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10024
		and [statuscodePLTable].AttributeValue = [cgi_routingaction].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10024) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_routingaction].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10024
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
		[cgi_routingaction].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10024)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_routingaction].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_routingaction].[cgi_routingactionId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10024 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)



--
-- report view for cgi_emailrecipient
--
create view dbo.[Filteredcgi_emailrecipient] (
    [cgi_emailaddress],
    [cgi_emailgroupid],
    [cgi_emailgroupidname],
    [cgi_emailrecipientid],
    [cgi_emailrecipientname],
    [cgi_role],
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
    [cgi_emailrecipient].[cgi_EmailAddress],
    [cgi_emailrecipient].[cgi_EmailGroupid],
    [cgi_emailrecipient].[cgi_EmailGroupidName],
    [cgi_emailrecipient].[cgi_emailrecipientId],
    [cgi_emailrecipient].[cgi_emailrecipientname],
    [cgi_emailrecipient].[cgi_Role],
    [cgi_emailrecipient].[CreatedBy],
    [cgi_emailrecipient].[CreatedByName],
    [cgi_emailrecipient].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_emailrecipient].[CreatedOn], 
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
        [cgi_emailrecipient].[CreatedOn],
    [cgi_emailrecipient].[CreatedOnBehalfBy],
    [cgi_emailrecipient].[CreatedOnBehalfByName],
    [cgi_emailrecipient].[CreatedOnBehalfByYomiName],
    [cgi_emailrecipient].[ImportSequenceNumber],
    [cgi_emailrecipient].[ModifiedBy],
    [cgi_emailrecipient].[ModifiedByName],
    [cgi_emailrecipient].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_emailrecipient].[ModifiedOn], 
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
        [cgi_emailrecipient].[ModifiedOn],
    [cgi_emailrecipient].[ModifiedOnBehalfBy],
    [cgi_emailrecipient].[ModifiedOnBehalfByName],
    [cgi_emailrecipient].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_emailrecipient].[OverriddenCreatedOn], 
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
        [cgi_emailrecipient].[OverriddenCreatedOn],
    [cgi_emailrecipient].[OwnerId],
    --[cgi_emailrecipient].[OwnerIdDsc]
    0,
    [cgi_emailrecipient].[OwnerIdName],
    [cgi_emailrecipient].[OwnerIdType],
    [cgi_emailrecipient].[OwnerIdYomiName],
    [cgi_emailrecipient].[OwningBusinessUnit],
    [cgi_emailrecipient].[OwningTeam],
    [cgi_emailrecipient].[OwningUser],
    [cgi_emailrecipient].[statecode],
    statecodePLTable.Value,
    [cgi_emailrecipient].[statuscode],
    statuscodePLTable.Value,
    [cgi_emailrecipient].[TimeZoneRuleVersionNumber],
    [cgi_emailrecipient].[UTCConversionTimeZoneCode],
    [cgi_emailrecipient].[VersionNumber]
from cgi_emailrecipient
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10014
		and [statecodePLTable].AttributeValue = [cgi_emailrecipient].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10014
		and [statuscodePLTable].AttributeValue = [cgi_emailrecipient].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10014) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_emailrecipient].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10014
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
		[cgi_emailrecipient].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10014)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_emailrecipient].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_emailrecipient].[cgi_emailrecipientId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10014 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

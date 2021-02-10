

--
-- report view for cgi_letter_template
--
create view dbo.[Filteredcgi_letter_template] (
    [cgi_letter_templateid],
    [cgi_name],
    [cgi_template_body],
    [cgi_title],
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
    [cgi_letter_template].[cgi_letter_templateId],
    [cgi_letter_template].[cgi_name],
    [cgi_letter_template].[cgi_template_body],
    [cgi_letter_template].[cgi_title],
    [cgi_letter_template].[CreatedBy],
    [cgi_letter_template].[CreatedByName],
    [cgi_letter_template].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_letter_template].[CreatedOn], 
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
        [cgi_letter_template].[CreatedOn],
    [cgi_letter_template].[CreatedOnBehalfBy],
    [cgi_letter_template].[CreatedOnBehalfByName],
    [cgi_letter_template].[CreatedOnBehalfByYomiName],
    [cgi_letter_template].[ImportSequenceNumber],
    [cgi_letter_template].[ModifiedBy],
    [cgi_letter_template].[ModifiedByName],
    [cgi_letter_template].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_letter_template].[ModifiedOn], 
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
        [cgi_letter_template].[ModifiedOn],
    [cgi_letter_template].[ModifiedOnBehalfBy],
    [cgi_letter_template].[ModifiedOnBehalfByName],
    [cgi_letter_template].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_letter_template].[OverriddenCreatedOn], 
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
        [cgi_letter_template].[OverriddenCreatedOn],
    [cgi_letter_template].[OwnerId],
    --[cgi_letter_template].[OwnerIdDsc]
    0,
    [cgi_letter_template].[OwnerIdName],
    [cgi_letter_template].[OwnerIdType],
    [cgi_letter_template].[OwnerIdYomiName],
    [cgi_letter_template].[OwningBusinessUnit],
    [cgi_letter_template].[OwningTeam],
    [cgi_letter_template].[OwningUser],
    [cgi_letter_template].[statecode],
    statecodePLTable.Value,
    [cgi_letter_template].[statuscode],
    statuscodePLTable.Value,
    [cgi_letter_template].[TimeZoneRuleVersionNumber],
    [cgi_letter_template].[UTCConversionTimeZoneCode],
    [cgi_letter_template].[VersionNumber]
from cgi_letter_template
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10047
		and [statecodePLTable].AttributeValue = [cgi_letter_template].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10047
		and [statuscodePLTable].AttributeValue = [cgi_letter_template].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10047) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_letter_template].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10047
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
		[cgi_letter_template].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10047)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_letter_template].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_letter_template].[cgi_letter_templateId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10047 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)



--
-- report view for cgi_rgolsetting
--
create view dbo.[Filteredcgi_rgolsetting] (
    [cgi_name],
    [cgi_refundtypeid],
    [cgi_refundtypeidname],
    [cgi_reimbursementformid],
    [cgi_reimbursementformidname],
    [cgi_rgolsettingid],
    [cgi_rgolsettingno],
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
    [cgi_rgolsetting].[cgi_name],
    [cgi_rgolsetting].[cgi_RefundTypeid],
    [cgi_rgolsetting].[cgi_RefundTypeidName],
    [cgi_rgolsetting].[cgi_ReimbursementFormid],
    [cgi_rgolsetting].[cgi_ReimbursementFormidName],
    [cgi_rgolsetting].[cgi_rgolsettingId],
    [cgi_rgolsetting].[cgi_rgolsettingno],
    [cgi_rgolsetting].[CreatedBy],
    [cgi_rgolsetting].[CreatedByName],
    [cgi_rgolsetting].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_rgolsetting].[CreatedOn], 
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
        [cgi_rgolsetting].[CreatedOn],
    [cgi_rgolsetting].[CreatedOnBehalfBy],
    [cgi_rgolsetting].[CreatedOnBehalfByName],
    [cgi_rgolsetting].[CreatedOnBehalfByYomiName],
    [cgi_rgolsetting].[ImportSequenceNumber],
    [cgi_rgolsetting].[ModifiedBy],
    [cgi_rgolsetting].[ModifiedByName],
    [cgi_rgolsetting].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_rgolsetting].[ModifiedOn], 
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
        [cgi_rgolsetting].[ModifiedOn],
    [cgi_rgolsetting].[ModifiedOnBehalfBy],
    [cgi_rgolsetting].[ModifiedOnBehalfByName],
    [cgi_rgolsetting].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_rgolsetting].[OverriddenCreatedOn], 
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
        [cgi_rgolsetting].[OverriddenCreatedOn],
    [cgi_rgolsetting].[OwnerId],
    --[cgi_rgolsetting].[OwnerIdDsc]
    0,
    [cgi_rgolsetting].[OwnerIdName],
    [cgi_rgolsetting].[OwnerIdType],
    [cgi_rgolsetting].[OwnerIdYomiName],
    [cgi_rgolsetting].[OwningBusinessUnit],
    [cgi_rgolsetting].[OwningTeam],
    [cgi_rgolsetting].[OwningUser],
    [cgi_rgolsetting].[statecode],
    statecodePLTable.Value,
    [cgi_rgolsetting].[statuscode],
    statuscodePLTable.Value,
    [cgi_rgolsetting].[TimeZoneRuleVersionNumber],
    [cgi_rgolsetting].[UTCConversionTimeZoneCode],
    [cgi_rgolsetting].[VersionNumber]
from cgi_rgolsetting
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10040
		and [statecodePLTable].AttributeValue = [cgi_rgolsetting].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10040
		and [statuscodePLTable].AttributeValue = [cgi_rgolsetting].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10040) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_rgolsetting].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10040
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
		[cgi_rgolsetting].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10040)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_rgolsetting].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_rgolsetting].[cgi_rgolsettingId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10040 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

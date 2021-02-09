

--
-- report view for cgi_categorydetail
--
create view dbo.[Filteredcgi_categorydetail] (
    [cgi_callguidecategory],
    [cgi_categorydetailid],
    [cgi_categorydetailname],
    [cgi_color],
    [cgi_colorname],
    [cgi_level],
    [cgi_parentid],
    [cgi_parentid2],
    [cgi_parentid2name],
    [cgi_parentidname],
    [cgi_requirestravelinfo],
    [cgi_requirestravelinfoname],
    [cgi_sortorder],
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
    [cgi_categorydetail].[cgi_CallguideCategory],
    [cgi_categorydetail].[cgi_categorydetailId],
    [cgi_categorydetail].[cgi_categorydetailname],
    [cgi_categorydetail].[cgi_Color],
    cgi_ColorPLTable.Value,
    [cgi_categorydetail].[cgi_Level],
    [cgi_categorydetail].[cgi_Parentid],
    [cgi_categorydetail].[cgi_parentid2],
    [cgi_categorydetail].[cgi_parentid2Name],
    [cgi_categorydetail].[cgi_ParentidName],
    [cgi_categorydetail].[cgi_requirestravelinfo],
    cgi_requirestravelinfoPLTable.Value,
    [cgi_categorydetail].[cgi_Sortorder],
    [cgi_categorydetail].[CreatedBy],
    [cgi_categorydetail].[CreatedByName],
    [cgi_categorydetail].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_categorydetail].[CreatedOn], 
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
        [cgi_categorydetail].[CreatedOn],
    [cgi_categorydetail].[CreatedOnBehalfBy],
    [cgi_categorydetail].[CreatedOnBehalfByName],
    [cgi_categorydetail].[CreatedOnBehalfByYomiName],
    [cgi_categorydetail].[ImportSequenceNumber],
    [cgi_categorydetail].[ModifiedBy],
    [cgi_categorydetail].[ModifiedByName],
    [cgi_categorydetail].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_categorydetail].[ModifiedOn], 
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
        [cgi_categorydetail].[ModifiedOn],
    [cgi_categorydetail].[ModifiedOnBehalfBy],
    [cgi_categorydetail].[ModifiedOnBehalfByName],
    [cgi_categorydetail].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_categorydetail].[OverriddenCreatedOn], 
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
        [cgi_categorydetail].[OverriddenCreatedOn],
    [cgi_categorydetail].[OwnerId],
    --[cgi_categorydetail].[OwnerIdDsc]
    0,
    [cgi_categorydetail].[OwnerIdName],
    [cgi_categorydetail].[OwnerIdType],
    [cgi_categorydetail].[OwnerIdYomiName],
    [cgi_categorydetail].[OwningBusinessUnit],
    [cgi_categorydetail].[OwningTeam],
    [cgi_categorydetail].[OwningUser],
    [cgi_categorydetail].[statecode],
    statecodePLTable.Value,
    [cgi_categorydetail].[statuscode],
    statuscodePLTable.Value,
    [cgi_categorydetail].[TimeZoneRuleVersionNumber],
    [cgi_categorydetail].[UTCConversionTimeZoneCode],
    [cgi_categorydetail].[VersionNumber]
from cgi_categorydetail
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_ColorPLTable] on 
		([cgi_ColorPLTable].AttributeName = 'cgi_color'
		and [cgi_ColorPLTable].ObjectTypeCode = 10012
		and [cgi_ColorPLTable].AttributeValue = [cgi_categorydetail].[cgi_Color]
		and [cgi_ColorPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_requirestravelinfoPLTable] on 
		([cgi_requirestravelinfoPLTable].AttributeName = 'cgi_requirestravelinfo'
		and [cgi_requirestravelinfoPLTable].ObjectTypeCode = 10012
		and [cgi_requirestravelinfoPLTable].AttributeValue = [cgi_categorydetail].[cgi_requirestravelinfo]
		and [cgi_requirestravelinfoPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10012
		and [statecodePLTable].AttributeValue = [cgi_categorydetail].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10012
		and [statuscodePLTable].AttributeValue = [cgi_categorydetail].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10012) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_categorydetail].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10012
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
		[cgi_categorydetail].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10012)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_categorydetail].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_categorydetail].[cgi_categorydetailId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10012 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

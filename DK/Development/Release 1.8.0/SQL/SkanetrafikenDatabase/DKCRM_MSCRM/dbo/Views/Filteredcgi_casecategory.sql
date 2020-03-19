

--
-- report view for cgi_casecategory
--
create view dbo.[Filteredcgi_casecategory] (
    [cgi_casecategoryid],
    [cgi_casecategoryname],
    [cgi_caseid],
    [cgi_caseidname],
    [cgi_category1id],
    [cgi_category1idname],
    [cgi_category2id],
    [cgi_category2idname],
    [cgi_category3id],
    [cgi_category3idname],
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
    [cgi_casecategory].[cgi_casecategoryId],
    [cgi_casecategory].[cgi_casecategoryname],
    [cgi_casecategory].[cgi_Caseid],
    [cgi_casecategory].[cgi_CaseidName],
    [cgi_casecategory].[cgi_Category1id],
    [cgi_casecategory].[cgi_Category1idName],
    [cgi_casecategory].[cgi_Category2id],
    [cgi_casecategory].[cgi_Category2idName],
    [cgi_casecategory].[cgi_Category3id],
    [cgi_casecategory].[cgi_Category3idName],
    [cgi_casecategory].[CreatedBy],
    [cgi_casecategory].[CreatedByName],
    [cgi_casecategory].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_casecategory].[CreatedOn], 
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
        [cgi_casecategory].[CreatedOn],
    [cgi_casecategory].[CreatedOnBehalfBy],
    [cgi_casecategory].[CreatedOnBehalfByName],
    [cgi_casecategory].[CreatedOnBehalfByYomiName],
    [cgi_casecategory].[ImportSequenceNumber],
    [cgi_casecategory].[ModifiedBy],
    [cgi_casecategory].[ModifiedByName],
    [cgi_casecategory].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_casecategory].[ModifiedOn], 
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
        [cgi_casecategory].[ModifiedOn],
    [cgi_casecategory].[ModifiedOnBehalfBy],
    [cgi_casecategory].[ModifiedOnBehalfByName],
    [cgi_casecategory].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_casecategory].[OverriddenCreatedOn], 
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
        [cgi_casecategory].[OverriddenCreatedOn],
    [cgi_casecategory].[OwnerId],
    --[cgi_casecategory].[OwnerIdDsc]
    0,
    [cgi_casecategory].[OwnerIdName],
    [cgi_casecategory].[OwnerIdType],
    [cgi_casecategory].[OwnerIdYomiName],
    [cgi_casecategory].[OwningBusinessUnit],
    [cgi_casecategory].[OwningTeam],
    [cgi_casecategory].[OwningUser],
    [cgi_casecategory].[statecode],
    statecodePLTable.Value,
    [cgi_casecategory].[statuscode],
    statuscodePLTable.Value,
    [cgi_casecategory].[TimeZoneRuleVersionNumber],
    [cgi_casecategory].[UTCConversionTimeZoneCode],
    [cgi_casecategory].[VersionNumber]
from cgi_casecategory
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10011
		and [statecodePLTable].AttributeValue = [cgi_casecategory].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10011
		and [statuscodePLTable].AttributeValue = [cgi_casecategory].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10011) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_casecategory].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10011
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
		[cgi_casecategory].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10011)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_casecategory].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_casecategory].[cgi_casecategoryId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10011 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

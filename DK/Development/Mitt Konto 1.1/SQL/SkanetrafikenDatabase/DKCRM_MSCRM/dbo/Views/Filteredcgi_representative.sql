

--
-- report view for cgi_representative
--
create view dbo.[Filteredcgi_representative] (
    [cgi_city],
    [cgi_coaddress],
    [cgi_country],
    [cgi_email],
    [cgi_firstname],
    [cgi_lastname],
    [cgi_mainphone],
    [cgi_name],
    [cgi_otherphone],
    [cgi_representativeid],
    [cgi_streetaddress],
    [cgi_telephone3],
    [cgi_zippostalcode],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [emailaddress],
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
    [cgi_representative].[cgi_City],
    [cgi_representative].[cgi_COaddress],
    [cgi_representative].[cgi_country],
    [cgi_representative].[cgi_Email],
    [cgi_representative].[cgi_FirstName],
    [cgi_representative].[cgi_LastName],
    [cgi_representative].[cgi_Mainphone],
    [cgi_representative].[cgi_name],
    [cgi_representative].[cgi_OtherPhone],
    [cgi_representative].[cgi_representativeId],
    [cgi_representative].[cgi_StreetAddress],
    [cgi_representative].[cgi_Telephone3],
    [cgi_representative].[cgi_ZIPPostalCode],
    [cgi_representative].[CreatedBy],
    [cgi_representative].[CreatedByName],
    [cgi_representative].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_representative].[CreatedOn], 
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
        [cgi_representative].[CreatedOn],
    [cgi_representative].[CreatedOnBehalfBy],
    [cgi_representative].[CreatedOnBehalfByName],
    [cgi_representative].[CreatedOnBehalfByYomiName],
    [cgi_representative].[EmailAddress],
    [cgi_representative].[ImportSequenceNumber],
    [cgi_representative].[ModifiedBy],
    [cgi_representative].[ModifiedByName],
    [cgi_representative].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_representative].[ModifiedOn], 
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
        [cgi_representative].[ModifiedOn],
    [cgi_representative].[ModifiedOnBehalfBy],
    [cgi_representative].[ModifiedOnBehalfByName],
    [cgi_representative].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_representative].[OverriddenCreatedOn], 
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
        [cgi_representative].[OverriddenCreatedOn],
    [cgi_representative].[OwnerId],
    --[cgi_representative].[OwnerIdDsc]
    0,
    [cgi_representative].[OwnerIdName],
    [cgi_representative].[OwnerIdType],
    [cgi_representative].[OwnerIdYomiName],
    [cgi_representative].[OwningBusinessUnit],
    [cgi_representative].[OwningTeam],
    [cgi_representative].[OwningUser],
    [cgi_representative].[statecode],
    statecodePLTable.Value,
    [cgi_representative].[statuscode],
    statuscodePLTable.Value,
    [cgi_representative].[TimeZoneRuleVersionNumber],
    [cgi_representative].[UTCConversionTimeZoneCode],
    [cgi_representative].[VersionNumber]
from cgi_representative
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10041
		and [statecodePLTable].AttributeValue = [cgi_representative].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10041
		and [statuscodePLTable].AttributeValue = [cgi_representative].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10041) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_representative].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10041
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
		[cgi_representative].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10041)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_representative].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_representative].[cgi_representativeId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10041 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

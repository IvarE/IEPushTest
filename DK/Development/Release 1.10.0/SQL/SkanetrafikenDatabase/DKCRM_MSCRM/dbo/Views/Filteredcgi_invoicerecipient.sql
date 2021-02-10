

--
-- report view for cgi_invoicerecipient
--
create view dbo.[Filteredcgi_invoicerecipient] (
    [cgi_address1],
    [cgi_customer_no],
    [cgi_invoicerecipientid],
    [cgi_invoicerecipientname],
    [cgi_inv_reference],
    [cgi_postalcode],
    [cgi_postal_city],
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
    [cgi_invoicerecipient].[cgi_address1],
    [cgi_invoicerecipient].[cgi_customer_no],
    [cgi_invoicerecipient].[cgi_invoicerecipientId],
    [cgi_invoicerecipient].[cgi_invoicerecipientname],
    [cgi_invoicerecipient].[cgi_inv_reference],
    [cgi_invoicerecipient].[cgi_postalcode],
    [cgi_invoicerecipient].[cgi_postal_city],
    [cgi_invoicerecipient].[CreatedBy],
    [cgi_invoicerecipient].[CreatedByName],
    [cgi_invoicerecipient].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_invoicerecipient].[CreatedOn], 
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
        [cgi_invoicerecipient].[CreatedOn],
    [cgi_invoicerecipient].[CreatedOnBehalfBy],
    [cgi_invoicerecipient].[CreatedOnBehalfByName],
    [cgi_invoicerecipient].[CreatedOnBehalfByYomiName],
    [cgi_invoicerecipient].[EmailAddress],
    [cgi_invoicerecipient].[ImportSequenceNumber],
    [cgi_invoicerecipient].[ModifiedBy],
    [cgi_invoicerecipient].[ModifiedByName],
    [cgi_invoicerecipient].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_invoicerecipient].[ModifiedOn], 
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
        [cgi_invoicerecipient].[ModifiedOn],
    [cgi_invoicerecipient].[ModifiedOnBehalfBy],
    [cgi_invoicerecipient].[ModifiedOnBehalfByName],
    [cgi_invoicerecipient].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_invoicerecipient].[OverriddenCreatedOn], 
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
        [cgi_invoicerecipient].[OverriddenCreatedOn],
    [cgi_invoicerecipient].[OwnerId],
    --[cgi_invoicerecipient].[OwnerIdDsc]
    0,
    [cgi_invoicerecipient].[OwnerIdName],
    [cgi_invoicerecipient].[OwnerIdType],
    [cgi_invoicerecipient].[OwnerIdYomiName],
    [cgi_invoicerecipient].[OwningBusinessUnit],
    [cgi_invoicerecipient].[OwningTeam],
    [cgi_invoicerecipient].[OwningUser],
    [cgi_invoicerecipient].[statecode],
    statecodePLTable.Value,
    [cgi_invoicerecipient].[statuscode],
    statuscodePLTable.Value,
    [cgi_invoicerecipient].[TimeZoneRuleVersionNumber],
    [cgi_invoicerecipient].[UTCConversionTimeZoneCode],
    [cgi_invoicerecipient].[VersionNumber]
from cgi_invoicerecipient
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10015
		and [statecodePLTable].AttributeValue = [cgi_invoicerecipient].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10015
		and [statuscodePLTable].AttributeValue = [cgi_invoicerecipient].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10015) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_invoicerecipient].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10015
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
		[cgi_invoicerecipient].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10015)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_invoicerecipient].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_invoicerecipient].[cgi_invoicerecipientId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10015 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)



--
-- report view for st_discountcode
--
create view dbo.[Filteredst_discountcode] (
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
    [st_campaign],
    [st_campaignname],
    [st_cardno],
    [st_contact],
    [st_contactname],
    [st_contactyominame],
    [st_discountcode],
    [st_discountcodeid],
    [st_lead],
    [st_leadname],
    [st_leadyominame],
    [st_name],
    [st_travelcard],
    [st_travelcardname],
    [st_validfrom],
    [st_validfromutc],
    [st_validto],
    [st_validtoutc],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [st_discountcode].[CreatedBy],
    [st_discountcode].[CreatedByName],
    [st_discountcode].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_discountcode].[CreatedOn], 
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
        [st_discountcode].[CreatedOn],
    [st_discountcode].[CreatedOnBehalfBy],
    [st_discountcode].[CreatedOnBehalfByName],
    [st_discountcode].[CreatedOnBehalfByYomiName],
    [st_discountcode].[ImportSequenceNumber],
    [st_discountcode].[ModifiedBy],
    [st_discountcode].[ModifiedByName],
    [st_discountcode].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_discountcode].[ModifiedOn], 
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
        [st_discountcode].[ModifiedOn],
    [st_discountcode].[ModifiedOnBehalfBy],
    [st_discountcode].[ModifiedOnBehalfByName],
    [st_discountcode].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_discountcode].[OverriddenCreatedOn], 
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
        [st_discountcode].[OverriddenCreatedOn],
    [st_discountcode].[OwnerId],
    --[st_discountcode].[OwnerIdDsc]
    0,
    [st_discountcode].[OwnerIdName],
    [st_discountcode].[OwnerIdType],
    [st_discountcode].[OwnerIdYomiName],
    [st_discountcode].[OwningBusinessUnit],
    [st_discountcode].[OwningTeam],
    [st_discountcode].[OwningUser],
    [st_discountcode].[statecode],
    statecodePLTable.Value,
    [st_discountcode].[statuscode],
    statuscodePLTable.Value,
    [st_discountcode].[st_Campaign],
    [st_discountcode].[st_CampaignName],
    [st_discountcode].[st_CardNo],
    [st_discountcode].[st_Contact],
    [st_discountcode].[st_ContactName],
    [st_discountcode].[st_ContactYomiName],
    [st_discountcode].[st_DiscountCode],
    [st_discountcode].[st_discountcodeId],
    [st_discountcode].[st_Lead],
    [st_discountcode].[st_LeadName],
    [st_discountcode].[st_LeadYomiName],
    [st_discountcode].[st_name],
    [st_discountcode].[st_TravelCard],
    [st_discountcode].[st_TravelCardName],
    dbo.fn_UTCToTzSpecificLocalTime([st_discountcode].[st_ValidFrom], 
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
        [st_discountcode].[st_ValidFrom],
    dbo.fn_UTCToTzSpecificLocalTime([st_discountcode].[st_ValidTo], 
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
        [st_discountcode].[st_ValidTo],
    [st_discountcode].[TimeZoneRuleVersionNumber],
    [st_discountcode].[UTCConversionTimeZoneCode],
    [st_discountcode].[VersionNumber]
from st_discountcode
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10063
		and [statecodePLTable].AttributeValue = [st_discountcode].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10063
		and [statuscodePLTable].AttributeValue = [st_discountcode].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10063) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[st_discountcode].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10063
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
		[st_discountcode].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10063)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[st_discountcode].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[st_discountcode].[st_discountcodeId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10063 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

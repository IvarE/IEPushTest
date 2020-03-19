

--
-- report view for cgi_creditorderrow
--
create view dbo.[Filteredcgi_creditorderrow] (
    [cgi_accountid],
    [cgi_accountidname],
    [cgi_accountidyominame],
    [cgi_contactid],
    [cgi_contactidname],
    [cgi_contactidyominame],
    [cgi_createdby],
    [cgi_creditorderrowid],
    [cgi_date],
    [cgi_message],
    [cgi_name],
    [cgi_ordernumber],
    [cgi_productnumber],
    [cgi_reason],
    [cgi_referencenumber],
    [cgi_success],
    [cgi_sum],
    [cgi_time],
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
    [cgi_creditorderrow].[cgi_Accountid],
    [cgi_creditorderrow].[cgi_AccountidName],
    [cgi_creditorderrow].[cgi_AccountidYomiName],
    [cgi_creditorderrow].[cgi_Contactid],
    [cgi_creditorderrow].[cgi_ContactidName],
    [cgi_creditorderrow].[cgi_ContactidYomiName],
    [cgi_creditorderrow].[cgi_CreatedBy],
    [cgi_creditorderrow].[cgi_creditorderrowId],
    [cgi_creditorderrow].[cgi_Date],
    [cgi_creditorderrow].[cgi_Message],
    [cgi_creditorderrow].[cgi_name],
    [cgi_creditorderrow].[cgi_OrderNumber],
    [cgi_creditorderrow].[cgi_ProductNumber],
    [cgi_creditorderrow].[cgi_Reason],
    [cgi_creditorderrow].[cgi_ReferenceNumber],
    [cgi_creditorderrow].[cgi_Success],
    [cgi_creditorderrow].[cgi_Sum],
    [cgi_creditorderrow].[cgi_Time],
    [cgi_creditorderrow].[CreatedBy],
    [cgi_creditorderrow].[CreatedByName],
    [cgi_creditorderrow].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_creditorderrow].[CreatedOn], 
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
        [cgi_creditorderrow].[CreatedOn],
    [cgi_creditorderrow].[CreatedOnBehalfBy],
    [cgi_creditorderrow].[CreatedOnBehalfByName],
    [cgi_creditorderrow].[CreatedOnBehalfByYomiName],
    [cgi_creditorderrow].[ImportSequenceNumber],
    [cgi_creditorderrow].[ModifiedBy],
    [cgi_creditorderrow].[ModifiedByName],
    [cgi_creditorderrow].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_creditorderrow].[ModifiedOn], 
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
        [cgi_creditorderrow].[ModifiedOn],
    [cgi_creditorderrow].[ModifiedOnBehalfBy],
    [cgi_creditorderrow].[ModifiedOnBehalfByName],
    [cgi_creditorderrow].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_creditorderrow].[OverriddenCreatedOn], 
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
        [cgi_creditorderrow].[OverriddenCreatedOn],
    [cgi_creditorderrow].[OwnerId],
    --[cgi_creditorderrow].[OwnerIdDsc]
    0,
    [cgi_creditorderrow].[OwnerIdName],
    [cgi_creditorderrow].[OwnerIdType],
    [cgi_creditorderrow].[OwnerIdYomiName],
    [cgi_creditorderrow].[OwningBusinessUnit],
    [cgi_creditorderrow].[OwningTeam],
    [cgi_creditorderrow].[OwningUser],
    [cgi_creditorderrow].[statecode],
    statecodePLTable.Value,
    [cgi_creditorderrow].[statuscode],
    statuscodePLTable.Value,
    [cgi_creditorderrow].[TimeZoneRuleVersionNumber],
    [cgi_creditorderrow].[UTCConversionTimeZoneCode],
    [cgi_creditorderrow].[VersionNumber]
from cgi_creditorderrow
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10044
		and [statecodePLTable].AttributeValue = [cgi_creditorderrow].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10044
		and [statuscodePLTable].AttributeValue = [cgi_creditorderrow].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10044) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_creditorderrow].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10044
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
		[cgi_creditorderrow].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10044)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_creditorderrow].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_creditorderrow].[cgi_creditorderrowId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10044 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

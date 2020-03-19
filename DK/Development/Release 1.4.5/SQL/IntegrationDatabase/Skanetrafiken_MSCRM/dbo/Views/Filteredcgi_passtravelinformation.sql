

--
-- report view for cgi_passtravelinformation
--
create view dbo.[Filteredcgi_passtravelinformation] (
    [cgi_incidentid],
    [cgi_incidentidname],
    [cgi_itfid],
    [cgi_itjid],
    [cgi_itlid],
    [cgi_ittid],
    [cgi_passtravelinformation],
    [cgi_passtravelinformationid],
    [cgi_stcid],
    [cgi_stcn],
    [cgi_stfd],
    [cgi_stfn],
    [cgi_stft],
    [cgi_stln],
    [cgi_strn],
    [cgi_sttd],
    [cgi_sttn],
    [cgi_sttt],
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
    [cgi_passtravelinformation].[cgi_IncidentId],
    [cgi_passtravelinformation].[cgi_IncidentIdName],
    [cgi_passtravelinformation].[cgi_iTFID],
    [cgi_passtravelinformation].[cgi_iTJID],
    [cgi_passtravelinformation].[cgi_iTLID],
    [cgi_passtravelinformation].[cgi_iTTID],
    [cgi_passtravelinformation].[cgi_passtravelinformation],
    [cgi_passtravelinformation].[cgi_passtravelinformationId],
    [cgi_passtravelinformation].[cgi_sTCID],
    [cgi_passtravelinformation].[cgi_sTCN],
    [cgi_passtravelinformation].[cgi_sTFD],
    [cgi_passtravelinformation].[cgi_sTFN],
    [cgi_passtravelinformation].[cgi_sTFT],
    [cgi_passtravelinformation].[cgi_sTLN],
    [cgi_passtravelinformation].[cgi_sTRN],
    [cgi_passtravelinformation].[cgi_sTTD],
    [cgi_passtravelinformation].[cgi_sTTN],
    [cgi_passtravelinformation].[cgi_sTTT],
    [cgi_passtravelinformation].[CreatedBy],
    [cgi_passtravelinformation].[CreatedByName],
    [cgi_passtravelinformation].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_passtravelinformation].[CreatedOn], 
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
        [cgi_passtravelinformation].[CreatedOn],
    [cgi_passtravelinformation].[CreatedOnBehalfBy],
    [cgi_passtravelinformation].[CreatedOnBehalfByName],
    [cgi_passtravelinformation].[CreatedOnBehalfByYomiName],
    [cgi_passtravelinformation].[ImportSequenceNumber],
    [cgi_passtravelinformation].[ModifiedBy],
    [cgi_passtravelinformation].[ModifiedByName],
    [cgi_passtravelinformation].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_passtravelinformation].[ModifiedOn], 
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
        [cgi_passtravelinformation].[ModifiedOn],
    [cgi_passtravelinformation].[ModifiedOnBehalfBy],
    [cgi_passtravelinformation].[ModifiedOnBehalfByName],
    [cgi_passtravelinformation].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_passtravelinformation].[OverriddenCreatedOn], 
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
        [cgi_passtravelinformation].[OverriddenCreatedOn],
    [cgi_passtravelinformation].[OwnerId],
    --[cgi_passtravelinformation].[OwnerIdDsc]
    0,
    [cgi_passtravelinformation].[OwnerIdName],
    [cgi_passtravelinformation].[OwnerIdType],
    [cgi_passtravelinformation].[OwnerIdYomiName],
    [cgi_passtravelinformation].[OwningBusinessUnit],
    [cgi_passtravelinformation].[OwningTeam],
    [cgi_passtravelinformation].[OwningUser],
    [cgi_passtravelinformation].[statecode],
    statecodePLTable.Value,
    [cgi_passtravelinformation].[statuscode],
    statuscodePLTable.Value,
    [cgi_passtravelinformation].[TimeZoneRuleVersionNumber],
    [cgi_passtravelinformation].[UTCConversionTimeZoneCode],
    [cgi_passtravelinformation].[VersionNumber]
from cgi_passtravelinformation
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10037
		and [statecodePLTable].AttributeValue = [cgi_passtravelinformation].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10037
		and [statuscodePLTable].AttributeValue = [cgi_passtravelinformation].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10037) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_passtravelinformation].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10037
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
		[cgi_passtravelinformation].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10037)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_passtravelinformation].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_passtravelinformation].[cgi_passtravelinformationId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10037 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

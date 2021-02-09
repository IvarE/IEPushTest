

--
-- report view for cgi_travelinformation
--
create view dbo.[Filteredcgi_travelinformation] (
    [cgi_arivalactual],
    [cgi_arivalplanned],
    [cgi_arivalplannedutc],
    [cgi_caseid],
    [cgi_caseidname],
    [cgi_city],
    [cgi_contractor],
    [cgi_deviationmessage],
    [cgi_directiontext],
    [cgi_displaytext],
    [cgi_journeynumber],
    [cgi_line],
    [cgi_linetype],
    [cgi_start],
    [cgi_startactual],
    [cgi_startplanned],
    [cgi_startplannedutc],
    [cgi_stop],
    [cgi_tour],
    [cgi_trainnumber],
    [cgi_transport],
    [cgi_travelinformation],
    [cgi_travelinformationid],
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
    [cgi_travelinformation].[cgi_ArivalActual],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelinformation].[cgi_ArivalPlanned], 
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
        [cgi_travelinformation].[cgi_ArivalPlanned],
    [cgi_travelinformation].[cgi_Caseid],
    [cgi_travelinformation].[cgi_CaseidName],
    [cgi_travelinformation].[cgi_City],
    [cgi_travelinformation].[cgi_Contractor],
    [cgi_travelinformation].[cgi_Deviationmessage],
    [cgi_travelinformation].[cgi_DirectionText],
    [cgi_travelinformation].[cgi_DisplayText],
    [cgi_travelinformation].[cgi_JourneyNumber],
    [cgi_travelinformation].[cgi_Line],
    [cgi_travelinformation].[cgi_LineType],
    [cgi_travelinformation].[cgi_Start],
    [cgi_travelinformation].[cgi_StartActual],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelinformation].[cgi_StartPlanned], 
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
        [cgi_travelinformation].[cgi_StartPlanned],
    [cgi_travelinformation].[cgi_Stop],
    [cgi_travelinformation].[cgi_Tour],
    [cgi_travelinformation].[cgi_TrainNumber],
    [cgi_travelinformation].[cgi_Transport],
    [cgi_travelinformation].[cgi_travelinformation],
    [cgi_travelinformation].[cgi_travelinformationId],
    [cgi_travelinformation].[CreatedBy],
    [cgi_travelinformation].[CreatedByName],
    [cgi_travelinformation].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelinformation].[CreatedOn], 
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
        [cgi_travelinformation].[CreatedOn],
    [cgi_travelinformation].[CreatedOnBehalfBy],
    [cgi_travelinformation].[CreatedOnBehalfByName],
    [cgi_travelinformation].[CreatedOnBehalfByYomiName],
    [cgi_travelinformation].[ImportSequenceNumber],
    [cgi_travelinformation].[ModifiedBy],
    [cgi_travelinformation].[ModifiedByName],
    [cgi_travelinformation].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelinformation].[ModifiedOn], 
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
        [cgi_travelinformation].[ModifiedOn],
    [cgi_travelinformation].[ModifiedOnBehalfBy],
    [cgi_travelinformation].[ModifiedOnBehalfByName],
    [cgi_travelinformation].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelinformation].[OverriddenCreatedOn], 
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
        [cgi_travelinformation].[OverriddenCreatedOn],
    [cgi_travelinformation].[OwnerId],
    --[cgi_travelinformation].[OwnerIdDsc]
    0,
    [cgi_travelinformation].[OwnerIdName],
    [cgi_travelinformation].[OwnerIdType],
    [cgi_travelinformation].[OwnerIdYomiName],
    [cgi_travelinformation].[OwningBusinessUnit],
    [cgi_travelinformation].[OwningTeam],
    [cgi_travelinformation].[OwningUser],
    [cgi_travelinformation].[statecode],
    statecodePLTable.Value,
    [cgi_travelinformation].[statuscode],
    statuscodePLTable.Value,
    [cgi_travelinformation].[TimeZoneRuleVersionNumber],
    [cgi_travelinformation].[UTCConversionTimeZoneCode],
    [cgi_travelinformation].[VersionNumber]
from cgi_travelinformation
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10030
		and [statecodePLTable].AttributeValue = [cgi_travelinformation].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10030
		and [statuscodePLTable].AttributeValue = [cgi_travelinformation].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10030) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_travelinformation].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10030
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
		[cgi_travelinformation].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10030)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_travelinformation].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_travelinformation].[cgi_travelinformationId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10030 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

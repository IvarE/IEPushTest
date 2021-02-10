

--
-- report view for st_update_travelcard
--
create view dbo.[Filteredst_update_travelcard] (
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
    [st_cardnum],
    [st_cardnumber],
    [st_cardnumname],
    [st_name],
    [st_op_offer],
    [st_op_offername],
    [st_op_offer_code],
    [st_op_offer_date],
    [st_op_offer_dateutc],
    [st_replydate],
    [st_replydateutc],
    [st_update_travelcardid],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [st_update_travelcard].[CreatedBy],
    [st_update_travelcard].[CreatedByName],
    [st_update_travelcard].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_update_travelcard].[CreatedOn], 
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
        [st_update_travelcard].[CreatedOn],
    [st_update_travelcard].[CreatedOnBehalfBy],
    [st_update_travelcard].[CreatedOnBehalfByName],
    [st_update_travelcard].[CreatedOnBehalfByYomiName],
    [st_update_travelcard].[ImportSequenceNumber],
    [st_update_travelcard].[ModifiedBy],
    [st_update_travelcard].[ModifiedByName],
    [st_update_travelcard].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_update_travelcard].[ModifiedOn], 
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
        [st_update_travelcard].[ModifiedOn],
    [st_update_travelcard].[ModifiedOnBehalfBy],
    [st_update_travelcard].[ModifiedOnBehalfByName],
    [st_update_travelcard].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([st_update_travelcard].[OverriddenCreatedOn], 
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
        [st_update_travelcard].[OverriddenCreatedOn],
    [st_update_travelcard].[OwnerId],
    --[st_update_travelcard].[OwnerIdDsc]
    0,
    [st_update_travelcard].[OwnerIdName],
    [st_update_travelcard].[OwnerIdType],
    [st_update_travelcard].[OwnerIdYomiName],
    [st_update_travelcard].[OwningBusinessUnit],
    [st_update_travelcard].[OwningTeam],
    [st_update_travelcard].[OwningUser],
    [st_update_travelcard].[statecode],
    statecodePLTable.Value,
    [st_update_travelcard].[statuscode],
    statuscodePLTable.Value,
    [st_update_travelcard].[st_CardNum],
    [st_update_travelcard].[st_CardNumber],
    [st_update_travelcard].[st_CardNumName],
    [st_update_travelcard].[st_name],
    [st_update_travelcard].[st_OP_Offer],
    st_OP_OfferPLTable.Value,
    [st_update_travelcard].[st_OP_Offer_Code],
    dbo.fn_UTCToTzSpecificLocalTime([st_update_travelcard].[st_OP_Offer_Date], 
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
        [st_update_travelcard].[st_OP_Offer_Date],
    dbo.fn_UTCToTzSpecificLocalTime([st_update_travelcard].[st_ReplyDate], 
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
        [st_update_travelcard].[st_ReplyDate],
    [st_update_travelcard].[st_update_travelcardId],
    [st_update_travelcard].[TimeZoneRuleVersionNumber],
    [st_update_travelcard].[UTCConversionTimeZoneCode],
    [st_update_travelcard].[VersionNumber]
from st_update_travelcard
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10058
		and [statecodePLTable].AttributeValue = [st_update_travelcard].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10058
		and [statuscodePLTable].AttributeValue = [st_update_travelcard].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [st_OP_OfferPLTable] on 
		([st_OP_OfferPLTable].AttributeName = 'st_op_offer'
		and [st_OP_OfferPLTable].ObjectTypeCode = 10058
		and [st_OP_OfferPLTable].AttributeValue = [st_update_travelcard].[st_OP_Offer]
		and [st_OP_OfferPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10058) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[st_update_travelcard].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10058
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
		[st_update_travelcard].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10058)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[st_update_travelcard].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[st_update_travelcard].[st_update_travelcardId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10058 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

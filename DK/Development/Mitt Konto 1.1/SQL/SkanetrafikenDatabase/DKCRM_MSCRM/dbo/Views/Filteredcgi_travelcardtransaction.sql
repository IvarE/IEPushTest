

--
-- report view for cgi_travelcardtransaction
--
create view dbo.[Filteredcgi_travelcardtransaction] (
    [cgi_amount],
    [cgi_cardsect],
    [cgi_caseid],
    [cgi_caseidname],
    [cgi_currency],
    [cgi_date],
    [cgi_deviceid],
    [cgi_origzone],
    [cgi_origzonename],
    [cgi_rectype],
    [cgi_route],
    [cgi_time],
    [cgi_travelcard],
    [cgi_travelcardid],
    [cgi_travelcardidname],
    [cgi_travelcardtransaction],
    [cgi_travelcardtransactionid],
    [cgi_txnnum],
    [cgi_txntype],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [exchangerate],
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
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [cgi_travelcardtransaction].[cgi_Amount],
    [cgi_travelcardtransaction].[cgi_cardsect],
    [cgi_travelcardtransaction].[cgi_caseId],
    [cgi_travelcardtransaction].[cgi_caseIdName],
    [cgi_travelcardtransaction].[cgi_currency],
    [cgi_travelcardtransaction].[cgi_date],
    [cgi_travelcardtransaction].[cgi_deviceid],
    [cgi_travelcardtransaction].[cgi_origzone],
    [cgi_travelcardtransaction].[cgi_OrigZoneName],
    [cgi_travelcardtransaction].[cgi_rectype],
    [cgi_travelcardtransaction].[cgi_route],
    [cgi_travelcardtransaction].[cgi_time],
    [cgi_travelcardtransaction].[cgi_TravelCard],
    [cgi_travelcardtransaction].[cgi_TravelCardid],
    [cgi_travelcardtransaction].[cgi_TravelCardidName],
    [cgi_travelcardtransaction].[cgi_travelcardtransaction],
    [cgi_travelcardtransaction].[cgi_travelcardtransactionId],
    [cgi_travelcardtransaction].[cgi_txnnum],
    [cgi_travelcardtransaction].[cgi_txntype],
    [cgi_travelcardtransaction].[CreatedBy],
    [cgi_travelcardtransaction].[CreatedByName],
    [cgi_travelcardtransaction].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcardtransaction].[CreatedOn], 
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
        [cgi_travelcardtransaction].[CreatedOn],
    [cgi_travelcardtransaction].[CreatedOnBehalfBy],
    [cgi_travelcardtransaction].[CreatedOnBehalfByName],
    [cgi_travelcardtransaction].[CreatedOnBehalfByYomiName],
    [cgi_travelcardtransaction].[ExchangeRate],
    [cgi_travelcardtransaction].[ImportSequenceNumber],
    [cgi_travelcardtransaction].[ModifiedBy],
    [cgi_travelcardtransaction].[ModifiedByName],
    [cgi_travelcardtransaction].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcardtransaction].[ModifiedOn], 
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
        [cgi_travelcardtransaction].[ModifiedOn],
    [cgi_travelcardtransaction].[ModifiedOnBehalfBy],
    [cgi_travelcardtransaction].[ModifiedOnBehalfByName],
    [cgi_travelcardtransaction].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_travelcardtransaction].[OverriddenCreatedOn], 
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
        [cgi_travelcardtransaction].[OverriddenCreatedOn],
    [cgi_travelcardtransaction].[OwnerId],
    --[cgi_travelcardtransaction].[OwnerIdDsc]
    0,
    [cgi_travelcardtransaction].[OwnerIdName],
    [cgi_travelcardtransaction].[OwnerIdType],
    [cgi_travelcardtransaction].[OwnerIdYomiName],
    [cgi_travelcardtransaction].[OwningBusinessUnit],
    [cgi_travelcardtransaction].[OwningTeam],
    [cgi_travelcardtransaction].[OwningUser],
    [cgi_travelcardtransaction].[statecode],
    statecodePLTable.Value,
    [cgi_travelcardtransaction].[statuscode],
    statuscodePLTable.Value,
    [cgi_travelcardtransaction].[TimeZoneRuleVersionNumber],
    [cgi_travelcardtransaction].[TransactionCurrencyId],
    [cgi_travelcardtransaction].[TransactionCurrencyIdName],
    [cgi_travelcardtransaction].[UTCConversionTimeZoneCode],
    [cgi_travelcardtransaction].[VersionNumber]
from cgi_travelcardtransaction
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10028
		and [statecodePLTable].AttributeValue = [cgi_travelcardtransaction].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10028
		and [statuscodePLTable].AttributeValue = [cgi_travelcardtransaction].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10028) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_travelcardtransaction].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10028
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
		[cgi_travelcardtransaction].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10028)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_travelcardtransaction].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_travelcardtransaction].[cgi_travelcardtransactionId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10028 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

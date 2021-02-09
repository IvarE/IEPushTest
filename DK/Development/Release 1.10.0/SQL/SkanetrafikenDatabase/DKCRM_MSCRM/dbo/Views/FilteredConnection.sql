

--
-- report view for connection
--
create view dbo.[FilteredConnection] (
    [connectionid],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [effectiveend],
    [effectiveendutc],
    [effectivestart],
    [effectivestartutc],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [importsequencenumber],
    [ismaster],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [record1id],
    [record1idname],
    [record1idobjecttypecode],
    [record1objecttypecode],
    [record1objecttypecodename],
    [record1roleid],
    [record1roleidname],
    [record2id],
    [record2idname],
    [record2idobjecttypecode],
    [record2objecttypecode],
    [record2objecttypecodename],
    [record2roleid],
    [record2roleidname],
    [relatedconnectionid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [versionnumber]
) with view_metadata as
select
    [Connection].[ConnectionId],
    [Connection].[CreatedBy],
    [Connection].[CreatedByName],
    [Connection].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Connection].[CreatedOn], 
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
        [Connection].[CreatedOn],
    [Connection].[CreatedOnBehalfBy],
    --[Connection].[CreatedOnBehalfByDsc]
    0,
    [Connection].[CreatedOnBehalfByName],
    [Connection].[CreatedOnBehalfByYomiName],
    [Connection].[Description],
    dbo.fn_UTCToTzSpecificLocalTime([Connection].[EffectiveEnd], 
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
        [Connection].[EffectiveEnd],
    dbo.fn_UTCToTzSpecificLocalTime([Connection].[EffectiveStart], 
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
        [Connection].[EffectiveStart],
    --[Connection].[EntityImage]
    cast(null as varbinary),
    [Connection].[EntityImageId],
    [Connection].[EntityImage_Timestamp],
    [Connection].[EntityImage_URL],
    [Connection].[ExchangeRate],
    [Connection].[ImportSequenceNumber],
    [Connection].[IsMaster],
    [Connection].[ModifiedBy],
    [Connection].[ModifiedByName],
    [Connection].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Connection].[ModifiedOn], 
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
        [Connection].[ModifiedOn],
    [Connection].[ModifiedOnBehalfBy],
    --[Connection].[ModifiedOnBehalfByDsc]
    0,
    [Connection].[ModifiedOnBehalfByName],
    [Connection].[ModifiedOnBehalfByYomiName],
    [Connection].[Name],
    dbo.fn_UTCToTzSpecificLocalTime([Connection].[OverriddenCreatedOn], 
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
        [Connection].[OverriddenCreatedOn],
    [Connection].[OwnerId],
    [Connection].[OwnerIdName],
    [Connection].[OwnerIdType],
    [Connection].[OwnerIdYomiName],
    [Connection].[OwningBusinessUnit],
    [Connection].[OwningTeam],
    [Connection].[OwningUser],
    [Connection].[Record1Id],
    [Connection].[Record1IdName],
    [Connection].[Record1IdObjectTypeCode],
    [Connection].[Record1ObjectTypeCode],
    Record1ObjectTypeCodePLTable.Value,
    [Connection].[Record1RoleId],
    [Connection].[Record1RoleIdName],
    [Connection].[Record2Id],
    [Connection].[Record2IdName],
    [Connection].[Record2IdObjectTypeCode],
    [Connection].[Record2ObjectTypeCode],
    Record2ObjectTypeCodePLTable.Value,
    [Connection].[Record2RoleId],
    [Connection].[Record2RoleIdName],
    [Connection].[RelatedConnectionId],
    [Connection].[StateCode],
    StateCodePLTable.Value,
    [Connection].[StatusCode],
    StatusCodePLTable.Value,
    [Connection].[TransactionCurrencyId],
    [Connection].[TransactionCurrencyIdName],
    [Connection].[VersionNumber]
from Connection
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [Record1ObjectTypeCodePLTable] on 
		([Record1ObjectTypeCodePLTable].AttributeName = 'record1objecttypecode'
		and [Record1ObjectTypeCodePLTable].ObjectTypeCode = 3234
		and [Record1ObjectTypeCodePLTable].AttributeValue = [Connection].[Record1ObjectTypeCode]
		and [Record1ObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [Record2ObjectTypeCodePLTable] on 
		([Record2ObjectTypeCodePLTable].AttributeName = 'record2objecttypecode'
		and [Record2ObjectTypeCodePLTable].ObjectTypeCode = 3234
		and [Record2ObjectTypeCodePLTable].AttributeValue = [Connection].[Record2ObjectTypeCode]
		and [Record2ObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 3234
		and [StateCodePLTable].AttributeValue = [Connection].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 3234
		and [StatusCodePLTable].AttributeValue = [Connection].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(3234) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Connection].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 3234
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
		[Connection].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 3234)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Connection].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Connection].[ConnectionId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 3234 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

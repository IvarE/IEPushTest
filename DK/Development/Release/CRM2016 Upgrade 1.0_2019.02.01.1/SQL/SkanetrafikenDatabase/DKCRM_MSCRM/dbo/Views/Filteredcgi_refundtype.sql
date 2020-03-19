

--
-- report view for cgi_refundtype
--
create view dbo.[Filteredcgi_refundtype] (
    [cgi_financialtransaction],
    [cgi_financialtransactionname],
    [cgi_printtext],
    [cgi_refundaccountid],
    [cgi_refundaccountidname],
    [cgi_refundoption],
    [cgi_refundoptionname],
    [cgi_refundproductid],
    [cgi_refundproductidname],
    [cgi_refundresponsibleid],
    [cgi_refundresponsibleidname],
    [cgi_refundtypeid],
    [cgi_refundtypename],
    [cgi_reinvoice],
    [cgi_reinvoicename],
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
    [cgi_refundtype].[cgi_FinancialTransaction],
    cgi_FinancialTransactionPLTable.Value,
    [cgi_refundtype].[cgi_PrintText],
    [cgi_refundtype].[cgi_refundaccountid],
    [cgi_refundtype].[cgi_refundaccountidName],
    [cgi_refundtype].[cgi_RefundOption],
    cgi_RefundOptionPLTable.Value,
    [cgi_refundtype].[cgi_refundproductid],
    [cgi_refundtype].[cgi_refundproductidName],
    [cgi_refundtype].[cgi_refundresponsibleId],
    [cgi_refundtype].[cgi_refundresponsibleIdName],
    [cgi_refundtype].[cgi_refundtypeId],
    [cgi_refundtype].[cgi_refundtypename],
    [cgi_refundtype].[cgi_reinvoice],
    cgi_reinvoicePLTable.Value,
    [cgi_refundtype].[CreatedBy],
    [cgi_refundtype].[CreatedByName],
    [cgi_refundtype].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_refundtype].[CreatedOn], 
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
        [cgi_refundtype].[CreatedOn],
    [cgi_refundtype].[CreatedOnBehalfBy],
    [cgi_refundtype].[CreatedOnBehalfByName],
    [cgi_refundtype].[CreatedOnBehalfByYomiName],
    [cgi_refundtype].[ImportSequenceNumber],
    [cgi_refundtype].[ModifiedBy],
    [cgi_refundtype].[ModifiedByName],
    [cgi_refundtype].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_refundtype].[ModifiedOn], 
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
        [cgi_refundtype].[ModifiedOn],
    [cgi_refundtype].[ModifiedOnBehalfBy],
    [cgi_refundtype].[ModifiedOnBehalfByName],
    [cgi_refundtype].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_refundtype].[OverriddenCreatedOn], 
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
        [cgi_refundtype].[OverriddenCreatedOn],
    [cgi_refundtype].[OwnerId],
    --[cgi_refundtype].[OwnerIdDsc]
    0,
    [cgi_refundtype].[OwnerIdName],
    [cgi_refundtype].[OwnerIdType],
    [cgi_refundtype].[OwnerIdYomiName],
    [cgi_refundtype].[OwningBusinessUnit],
    [cgi_refundtype].[OwningTeam],
    [cgi_refundtype].[OwningUser],
    [cgi_refundtype].[statecode],
    statecodePLTable.Value,
    [cgi_refundtype].[statuscode],
    statuscodePLTable.Value,
    [cgi_refundtype].[TimeZoneRuleVersionNumber],
    [cgi_refundtype].[UTCConversionTimeZoneCode],
    [cgi_refundtype].[VersionNumber]
from cgi_refundtype
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_FinancialTransactionPLTable] on 
		([cgi_FinancialTransactionPLTable].AttributeName = 'cgi_financialtransaction'
		and [cgi_FinancialTransactionPLTable].ObjectTypeCode = 10022
		and [cgi_FinancialTransactionPLTable].AttributeValue = [cgi_refundtype].[cgi_FinancialTransaction]
		and [cgi_FinancialTransactionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_RefundOptionPLTable] on 
		([cgi_RefundOptionPLTable].AttributeName = 'cgi_refundoption'
		and [cgi_RefundOptionPLTable].ObjectTypeCode = 10022
		and [cgi_RefundOptionPLTable].AttributeValue = [cgi_refundtype].[cgi_RefundOption]
		and [cgi_RefundOptionPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_reinvoicePLTable] on 
		([cgi_reinvoicePLTable].AttributeName = 'cgi_reinvoice'
		and [cgi_reinvoicePLTable].ObjectTypeCode = 10022
		and [cgi_reinvoicePLTable].AttributeValue = [cgi_refundtype].[cgi_reinvoice]
		and [cgi_reinvoicePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10022
		and [statecodePLTable].AttributeValue = [cgi_refundtype].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10022
		and [statuscodePLTable].AttributeValue = [cgi_refundtype].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10022) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_refundtype].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10022
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
		[cgi_refundtype].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10022)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_refundtype].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_refundtype].[cgi_refundtypeId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10022 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)



--
-- report view for cgi_reimbursementform
--
create view dbo.[Filteredcgi_reimbursementform] (
    [cgi_attestation],
    [cgi_attestationname],
    [cgi_couponemail],
    [cgi_couponemailname],
    [cgi_couponsms],
    [cgi_couponsmsname],
    [cgi_giftcard],
    [cgi_giftcardname],
    [cgi_loadcard],
    [cgi_loadcardname],
    [cgi_payment],
    [cgi_paymentname],
    [cgi_payment_abroad],
    [cgi_payment_abroadname],
    [cgi_print],
    [cgi_printname],
    [cgi_reimbursementformid],
    [cgi_reimbursementname],
    [cgi_reinvoicing],
    [cgi_reinvoicingname],
    [cgi_sendtostralfors],
    [cgi_sendtostralforsname],
    [cgi_time_valid],
    [cgi_time_validname],
    [cgi_useaccount],
    [cgi_useaccountname],
    [cgi_useproduct],
    [cgi_useproductname],
    [cgi_useresponsible],
    [cgi_useresponsiblename],
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
    [cgi_reimbursementform].[cgi_attestation],
    cgi_attestationPLTable.Value,
    [cgi_reimbursementform].[cgi_couponemail],
    cgi_couponemailPLTable.Value,
    [cgi_reimbursementform].[cgi_couponsms],
    cgi_couponsmsPLTable.Value,
    [cgi_reimbursementform].[cgi_giftcard],
    cgi_giftcardPLTable.Value,
    [cgi_reimbursementform].[cgi_loadcard],
    cgi_loadcardPLTable.Value,
    [cgi_reimbursementform].[cgi_payment],
    cgi_paymentPLTable.Value,
    [cgi_reimbursementform].[cgi_payment_abroad],
    cgi_payment_abroadPLTable.Value,
    [cgi_reimbursementform].[cgi_Print],
    cgi_PrintPLTable.Value,
    [cgi_reimbursementform].[cgi_reimbursementformId],
    [cgi_reimbursementform].[cgi_reimbursementname],
    [cgi_reimbursementform].[cgi_ReInvoicing],
    cgi_ReInvoicingPLTable.Value,
    [cgi_reimbursementform].[cgi_sendtostralfors],
    cgi_sendtostralforsPLTable.Value,
    [cgi_reimbursementform].[cgi_time_valid],
    cgi_time_validPLTable.Value,
    [cgi_reimbursementform].[cgi_UseAccount],
    cgi_UseAccountPLTable.Value,
    [cgi_reimbursementform].[cgi_UseProduct],
    cgi_UseProductPLTable.Value,
    [cgi_reimbursementform].[cgi_useresponsible],
    cgi_useresponsiblePLTable.Value,
    [cgi_reimbursementform].[CreatedBy],
    [cgi_reimbursementform].[CreatedByName],
    [cgi_reimbursementform].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_reimbursementform].[CreatedOn], 
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
        [cgi_reimbursementform].[CreatedOn],
    [cgi_reimbursementform].[CreatedOnBehalfBy],
    [cgi_reimbursementform].[CreatedOnBehalfByName],
    [cgi_reimbursementform].[CreatedOnBehalfByYomiName],
    [cgi_reimbursementform].[ImportSequenceNumber],
    [cgi_reimbursementform].[ModifiedBy],
    [cgi_reimbursementform].[ModifiedByName],
    [cgi_reimbursementform].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_reimbursementform].[ModifiedOn], 
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
        [cgi_reimbursementform].[ModifiedOn],
    [cgi_reimbursementform].[ModifiedOnBehalfBy],
    [cgi_reimbursementform].[ModifiedOnBehalfByName],
    [cgi_reimbursementform].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([cgi_reimbursementform].[OverriddenCreatedOn], 
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
        [cgi_reimbursementform].[OverriddenCreatedOn],
    [cgi_reimbursementform].[OwnerId],
    --[cgi_reimbursementform].[OwnerIdDsc]
    0,
    [cgi_reimbursementform].[OwnerIdName],
    [cgi_reimbursementform].[OwnerIdType],
    [cgi_reimbursementform].[OwnerIdYomiName],
    [cgi_reimbursementform].[OwningBusinessUnit],
    [cgi_reimbursementform].[OwningTeam],
    [cgi_reimbursementform].[OwningUser],
    [cgi_reimbursementform].[statecode],
    statecodePLTable.Value,
    [cgi_reimbursementform].[statuscode],
    statuscodePLTable.Value,
    [cgi_reimbursementform].[TimeZoneRuleVersionNumber],
    [cgi_reimbursementform].[UTCConversionTimeZoneCode],
    [cgi_reimbursementform].[VersionNumber]
from cgi_reimbursementform
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_attestationPLTable] on 
		([cgi_attestationPLTable].AttributeName = 'cgi_attestation'
		and [cgi_attestationPLTable].ObjectTypeCode = 10023
		and [cgi_attestationPLTable].AttributeValue = [cgi_reimbursementform].[cgi_attestation]
		and [cgi_attestationPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_couponemailPLTable] on 
		([cgi_couponemailPLTable].AttributeName = 'cgi_couponemail'
		and [cgi_couponemailPLTable].ObjectTypeCode = 10023
		and [cgi_couponemailPLTable].AttributeValue = [cgi_reimbursementform].[cgi_couponemail]
		and [cgi_couponemailPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_couponsmsPLTable] on 
		([cgi_couponsmsPLTable].AttributeName = 'cgi_couponsms'
		and [cgi_couponsmsPLTable].ObjectTypeCode = 10023
		and [cgi_couponsmsPLTable].AttributeValue = [cgi_reimbursementform].[cgi_couponsms]
		and [cgi_couponsmsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_giftcardPLTable] on 
		([cgi_giftcardPLTable].AttributeName = 'cgi_giftcard'
		and [cgi_giftcardPLTable].ObjectTypeCode = 10023
		and [cgi_giftcardPLTable].AttributeValue = [cgi_reimbursementform].[cgi_giftcard]
		and [cgi_giftcardPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_loadcardPLTable] on 
		([cgi_loadcardPLTable].AttributeName = 'cgi_loadcard'
		and [cgi_loadcardPLTable].ObjectTypeCode = 10023
		and [cgi_loadcardPLTable].AttributeValue = [cgi_reimbursementform].[cgi_loadcard]
		and [cgi_loadcardPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_paymentPLTable] on 
		([cgi_paymentPLTable].AttributeName = 'cgi_payment'
		and [cgi_paymentPLTable].ObjectTypeCode = 10023
		and [cgi_paymentPLTable].AttributeValue = [cgi_reimbursementform].[cgi_payment]
		and [cgi_paymentPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_payment_abroadPLTable] on 
		([cgi_payment_abroadPLTable].AttributeName = 'cgi_payment_abroad'
		and [cgi_payment_abroadPLTable].ObjectTypeCode = 10023
		and [cgi_payment_abroadPLTable].AttributeValue = [cgi_reimbursementform].[cgi_payment_abroad]
		and [cgi_payment_abroadPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_PrintPLTable] on 
		([cgi_PrintPLTable].AttributeName = 'cgi_print'
		and [cgi_PrintPLTable].ObjectTypeCode = 10023
		and [cgi_PrintPLTable].AttributeValue = [cgi_reimbursementform].[cgi_Print]
		and [cgi_PrintPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_ReInvoicingPLTable] on 
		([cgi_ReInvoicingPLTable].AttributeName = 'cgi_reinvoicing'
		and [cgi_ReInvoicingPLTable].ObjectTypeCode = 10023
		and [cgi_ReInvoicingPLTable].AttributeValue = [cgi_reimbursementform].[cgi_ReInvoicing]
		and [cgi_ReInvoicingPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_sendtostralforsPLTable] on 
		([cgi_sendtostralforsPLTable].AttributeName = 'cgi_sendtostralfors'
		and [cgi_sendtostralforsPLTable].ObjectTypeCode = 10023
		and [cgi_sendtostralforsPLTable].AttributeValue = [cgi_reimbursementform].[cgi_sendtostralfors]
		and [cgi_sendtostralforsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_time_validPLTable] on 
		([cgi_time_validPLTable].AttributeName = 'cgi_time_valid'
		and [cgi_time_validPLTable].ObjectTypeCode = 10023
		and [cgi_time_validPLTable].AttributeValue = [cgi_reimbursementform].[cgi_time_valid]
		and [cgi_time_validPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_UseAccountPLTable] on 
		([cgi_UseAccountPLTable].AttributeName = 'cgi_useaccount'
		and [cgi_UseAccountPLTable].ObjectTypeCode = 10023
		and [cgi_UseAccountPLTable].AttributeValue = [cgi_reimbursementform].[cgi_UseAccount]
		and [cgi_UseAccountPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_UseProductPLTable] on 
		([cgi_UseProductPLTable].AttributeName = 'cgi_useproduct'
		and [cgi_UseProductPLTable].ObjectTypeCode = 10023
		and [cgi_UseProductPLTable].AttributeValue = [cgi_reimbursementform].[cgi_UseProduct]
		and [cgi_UseProductPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_useresponsiblePLTable] on 
		([cgi_useresponsiblePLTable].AttributeName = 'cgi_useresponsible'
		and [cgi_useresponsiblePLTable].ObjectTypeCode = 10023
		and [cgi_useresponsiblePLTable].AttributeValue = [cgi_reimbursementform].[cgi_useresponsible]
		and [cgi_useresponsiblePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statecodePLTable] on 
		([statecodePLTable].AttributeName = 'statecode'
		and [statecodePLTable].ObjectTypeCode = 10023
		and [statecodePLTable].AttributeValue = [cgi_reimbursementform].[statecode]
		and [statecodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [statuscodePLTable] on 
		([statuscodePLTable].AttributeName = 'statuscode'
		and [statuscodePLTable].ObjectTypeCode = 10023
		and [statuscodePLTable].AttributeValue = [cgi_reimbursementform].[statuscode]
		and [statuscodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(10023) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[cgi_reimbursementform].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 10023
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
		[cgi_reimbursementform].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 10023)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[cgi_reimbursementform].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[cgi_reimbursementform].[cgi_reimbursementformId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 10023 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

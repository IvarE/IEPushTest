﻿

--
-- report view for reportcategory
--
create view dbo.[FilteredReportCategory] (
    [categorycode],
    [categorycodename],
    [componentstate],
    [createdby],
    [createdbydsc],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [exchangerate],
    [importsequencenumber],
    [iscustomizable],
    [ismanaged],
    [ismanagedname],
    [modifiedby],
    [modifiedbydsc],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [overwritetime],
    [overwritetimeutc],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [reportcategoryid],
    [reportcategoryidunique],
    [reportid],
    [reportiddsc],
    [reportidname],
    [solutionid],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [ReportCategory].[CategoryCode],
    CategoryCodePLTable.Value,
    [ReportCategory].[ComponentState],
    [ReportCategory].[CreatedBy],
    --[ReportCategory].[CreatedByDsc]
    0,
    [ReportCategory].[CreatedByName],
    [ReportCategory].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ReportCategory].[CreatedOn], 
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
        [ReportCategory].[CreatedOn],
    [ReportCategory].[CreatedOnBehalfBy],
    --[ReportCategory].[CreatedOnBehalfByDsc]
    0,
    [ReportCategory].[CreatedOnBehalfByName],
    [ReportCategory].[CreatedOnBehalfByYomiName],
    [ReportCategory].[ExchangeRate],
    [ReportCategory].[ImportSequenceNumber],
    [ReportCategory].[IsCustomizable],
    [ReportCategory].[IsManaged],
    IsManagedPLTable.Value,
    [ReportCategory].[ModifiedBy],
    --[ReportCategory].[ModifiedByDsc]
    0,
    [ReportCategory].[ModifiedByName],
    [ReportCategory].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ReportCategory].[ModifiedOn], 
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
        [ReportCategory].[ModifiedOn],
    [ReportCategory].[ModifiedOnBehalfBy],
    --[ReportCategory].[ModifiedOnBehalfByDsc]
    0,
    [ReportCategory].[ModifiedOnBehalfByName],
    [ReportCategory].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ReportCategory].[OverwriteTime], 
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
        [ReportCategory].[OverwriteTime],
    [ReportCategory].[OwnerId],
    [ReportCategory].[OwnerIdType],
    [ReportCategory].[OwningBusinessUnit],
    [ReportCategory].[OwningUser],
    [ReportCategory].[ReportCategoryId],
    [ReportCategory].[ReportCategoryIdUnique],
    [ReportCategory].[ReportId],
    --[ReportCategory].[ReportIdDsc]
    0,
    [ReportCategory].[ReportIdName],
    [ReportCategory].[SolutionId],
    [ReportCategory].[TimeZoneRuleVersionNumber],
    [ReportCategory].[TransactionCurrencyId],
    [ReportCategory].[TransactionCurrencyIdName],
    [ReportCategory].[UTCConversionTimeZoneCode],
    [ReportCategory].[VersionNumber]
from ReportCategory
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [CategoryCodePLTable] on 
		([CategoryCodePLTable].AttributeName = 'categorycode'
		and [CategoryCodePLTable].ObjectTypeCode = 9102
		and [CategoryCodePLTable].AttributeValue = [ReportCategory].[CategoryCode]
		and [CategoryCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 9102
		and [IsManagedPLTable].AttributeValue = [ReportCategory].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9100) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[ReportCategory].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 9100
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
		[ReportCategory].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9100)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ReportCategory].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ReportCategory].[ReportId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 9100 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

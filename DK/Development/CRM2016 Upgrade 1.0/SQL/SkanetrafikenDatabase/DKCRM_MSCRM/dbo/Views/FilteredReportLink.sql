﻿

--
-- report view for reportlink
--
create view dbo.[FilteredReportLink] (
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
    [importsequencenumber],
    [linkedreportid],
    [linkedreportname],
    [linktypecode],
    [linktypecodename],
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
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [reportid],
    [reportlinkid],
    [reportlinkidunique],
    [versionnumber]
) with view_metadata as
select
    [ReportLink].[CreatedBy],
    --[ReportLink].[CreatedByDsc]
    0,
    [ReportLink].[CreatedByName],
    [ReportLink].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ReportLink].[CreatedOn], 
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
        [ReportLink].[CreatedOn],
    [ReportLink].[CreatedOnBehalfBy],
    --[ReportLink].[CreatedOnBehalfByDsc]
    0,
    [ReportLink].[CreatedOnBehalfByName],
    [ReportLink].[CreatedOnBehalfByYomiName],
    [ReportLink].[ImportSequenceNumber],
    [ReportLink].[LinkedReportId],
    [ReportLink].[LinkedReportName],
    [ReportLink].[LinkTypeCode],
    LinkTypeCodePLTable.Value,
    [ReportLink].[ModifiedBy],
    --[ReportLink].[ModifiedByDsc]
    0,
    [ReportLink].[ModifiedByName],
    [ReportLink].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ReportLink].[ModifiedOn], 
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
        [ReportLink].[ModifiedOn],
    [ReportLink].[ModifiedOnBehalfBy],
    --[ReportLink].[ModifiedOnBehalfByDsc]
    0,
    [ReportLink].[ModifiedOnBehalfByName],
    [ReportLink].[ModifiedOnBehalfByYomiName],
    [ReportLink].[OwnerId],
    [ReportLink].[OwnerIdType],
    [ReportLink].[OwningBusinessUnit],
    [ReportLink].[OwningUser],
    [ReportLink].[ReportId],
    [ReportLink].[ReportLinkId],
    [ReportLink].[ReportLinkIdUnique],
    [ReportLink].[VersionNumber]
from ReportLink
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [LinkTypeCodePLTable] on 
		([LinkTypeCodePLTable].AttributeName = 'linktypecode'
		and [LinkTypeCodePLTable].ObjectTypeCode = 9104
		and [LinkTypeCodePLTable].AttributeValue = [ReportLink].[LinkTypeCode]
		and [LinkTypeCodePLTable].LangId = 
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
	[ReportLink].OwnerId in 
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
		[ReportLink].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9100)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ReportLink].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ReportLink].[ReportId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 9100 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

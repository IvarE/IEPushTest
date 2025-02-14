﻿

--
-- report view for report
--
create view dbo.[FilteredReport] (
    [bodybinary],
    [bodytext],
    [bodyurl],
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
    [customreportxml],
    [defaultfilter],
    [description],
    [filename],
    [filesize],
    [introducedversion],
    [iscustomizable],
    [iscustomreport],
    [iscustomreportname],
    [ismanaged],
    [ismanagedname],
    [ispersonal],
    [ispersonalname],
    [isscheduledreport],
    [isscheduledreportname],
    [languagecode],
    [mimetype],
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
    [name],
    [originalbodytext],
    [overwritetime],
    [overwritetimeutc],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [parentreportid],
    [parentreportiddsc],
    [parentreportidname],
    [queryinfo],
    [reportid],
    [reportidunique],
    [reportnameonsrs],
    [reporttypecode],
    [reporttypecodename],
    [schedulexml],
    [signaturedate],
    [signaturedateutc],
    [signatureid],
    [signaturelcid],
    [signaturemajorversion],
    [signatureminorversion],
    [solutionid],
    [timezoneruleversionnumber],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [Report].[BodyBinary],
    [Report].[BodyText],
    [Report].[BodyUrl],
    [Report].[ComponentState],
    [Report].[CreatedBy],
    --[Report].[CreatedByDsc]
    0,
    [Report].[CreatedByName],
    [Report].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Report].[CreatedOn], 
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
        [Report].[CreatedOn],
    [Report].[CreatedOnBehalfBy],
    --[Report].[CreatedOnBehalfByDsc]
    0,
    [Report].[CreatedOnBehalfByName],
    [Report].[CreatedOnBehalfByYomiName],
    [Report].[CustomReportXml],
    [Report].[DefaultFilter],
    [Report].[Description],
    [Report].[FileName],
    [Report].[FileSize],
    [Report].[IntroducedVersion],
    [Report].[IsCustomizable],
    [Report].[IsCustomReport],
    IsCustomReportPLTable.Value,
    [Report].[IsManaged],
    IsManagedPLTable.Value,
    [Report].[IsPersonal],
    IsPersonalPLTable.Value,
    [Report].[IsScheduledReport],
    IsScheduledReportPLTable.Value,
    [Report].[LanguageCode],
    [Report].[MimeType],
    [Report].[ModifiedBy],
    --[Report].[ModifiedByDsc]
    0,
    [Report].[ModifiedByName],
    [Report].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Report].[ModifiedOn], 
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
        [Report].[ModifiedOn],
    [Report].[ModifiedOnBehalfBy],
    --[Report].[ModifiedOnBehalfByDsc]
    0,
    [Report].[ModifiedOnBehalfByName],
    [Report].[ModifiedOnBehalfByYomiName],
    [Report].[Name],
    [Report].[OriginalBodyText],
    dbo.fn_UTCToTzSpecificLocalTime([Report].[OverwriteTime], 
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
        [Report].[OverwriteTime],
    [Report].[OwnerId],
    --[Report].[OwnerIdDsc]
    0,
    [Report].[OwnerIdName],
    [Report].[OwnerIdType],
    [Report].[OwnerIdYomiName],
    [Report].[OwningBusinessUnit],
    [Report].[OwningTeam],
    [Report].[OwningUser],
    [Report].[ParentReportId],
    --[Report].[ParentReportIdDsc]
    0,
    [Report].[ParentReportIdName],
    [Report].[QueryInfo],
    [Report].[ReportId],
    [Report].[ReportIdUnique],
    [Report].[ReportNameOnSRS],
    [Report].[ReportTypeCode],
    ReportTypeCodePLTable.Value,
    [Report].[ScheduleXml],
    dbo.fn_UTCToTzSpecificLocalTime([Report].[SignatureDate], 
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
        [Report].[SignatureDate],
    [Report].[SignatureId],
    [Report].[SignatureLcid],
    [Report].[SignatureMajorVersion],
    [Report].[SignatureMinorVersion],
    [Report].[SolutionId],
    [Report].[TimeZoneRuleVersionNumber],
    [Report].[UTCConversionTimeZoneCode],
    [Report].[VersionNumber]
from Report
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsCustomReportPLTable] on 
		([IsCustomReportPLTable].AttributeName = 'iscustomreport'
		and [IsCustomReportPLTable].ObjectTypeCode = 9100
		and [IsCustomReportPLTable].AttributeValue = [Report].[IsCustomReport]
		and [IsCustomReportPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 9100
		and [IsManagedPLTable].AttributeValue = [Report].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsPersonalPLTable] on 
		([IsPersonalPLTable].AttributeName = 'ispersonal'
		and [IsPersonalPLTable].ObjectTypeCode = 9100
		and [IsPersonalPLTable].AttributeValue = [Report].[IsPersonal]
		and [IsPersonalPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsScheduledReportPLTable] on 
		([IsScheduledReportPLTable].AttributeName = 'isscheduledreport'
		and [IsScheduledReportPLTable].ObjectTypeCode = 9100
		and [IsScheduledReportPLTable].AttributeValue = [Report].[IsScheduledReport]
		and [IsScheduledReportPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ReportTypeCodePLTable] on 
		([ReportTypeCodePLTable].AttributeName = 'reporttypecode'
		and [ReportTypeCodePLTable].ObjectTypeCode = 9100
		and [ReportTypeCodePLTable].AttributeValue = [Report].[ReportTypeCode]
		and [ReportTypeCodePLTable].LangId = 
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
	[Report].OwnerId in 
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
		[Report].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9100)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Report].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Report].[ReportId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 9100 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

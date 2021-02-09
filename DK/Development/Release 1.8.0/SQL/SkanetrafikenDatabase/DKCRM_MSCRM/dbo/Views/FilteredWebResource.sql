

--
-- report view for webresource
--
create view dbo.[FilteredWebResource] (
    [canbedeleted],
    [componentstate],
    [content],
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [displayname],
    [introducedversion],
    [iscustomizable],
    [ishidden],
    [ismanaged],
    [ismanagedname],
    [languagecode],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [organizationidname],
    [overwritetime],
    [overwritetimeutc],
    [silverlightversion],
    [solutionid],
    [versionnumber],
    [webresourceid],
    [webresourceidunique],
    [webresourcetype],
    [webresourcetypename]
) with view_metadata as
select
    [WebResource].[CanBeDeleted],
    [WebResource].[ComponentState],
    [WebResource].[Content],
    [WebResource].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([WebResource].[CreatedOn], 
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
        [WebResource].[CreatedOn],
    [WebResource].[CreatedOnBehalfBy],
    --[WebResource].[CreatedOnBehalfByDsc]
    0,
    [WebResource].[CreatedOnBehalfByName],
    [WebResource].[CreatedOnBehalfByYomiName],
    coalesce(dbo.fn_GetLocalizedLabel([WebResource].[WebResourceId], 'description', 16, us.UILanguageId), [WebResource].[Description]),
    coalesce(dbo.fn_GetLocalizedLabel([WebResource].[WebResourceId], 'displayname', 16, us.UILanguageId), [WebResource].[DisplayName]),
    [WebResource].[IntroducedVersion],
    [WebResource].[IsCustomizable],
    [WebResource].[IsHidden],
    [WebResource].[IsManaged],
    IsManagedPLTable.Value,
    [WebResource].[LanguageCode],
    [WebResource].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([WebResource].[ModifiedOn], 
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
        [WebResource].[ModifiedOn],
    [WebResource].[ModifiedOnBehalfBy],
    --[WebResource].[ModifiedOnBehalfByDsc]
    0,
    [WebResource].[ModifiedOnBehalfByName],
    [WebResource].[ModifiedOnBehalfByYomiName],
    [WebResource].[Name],
    [WebResource].[OrganizationId],
    [WebResource].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([WebResource].[OverwriteTime], 
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
        [WebResource].[OverwriteTime],
    coalesce(dbo.fn_GetLocalizedLabel([WebResource].[WebResourceId], 'silverlightversion', 16, us.UILanguageId), [WebResource].[SilverlightVersion]),
    [WebResource].[SolutionId],
    [WebResource].[VersionNumber],
    [WebResource].[WebResourceId],
    [WebResource].[WebResourceIdUnique],
    [WebResource].[WebResourceType],
    WebResourceTypePLTable.Value
from WebResource
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 9333
		and [IsManagedPLTable].AttributeValue = [WebResource].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [WebResourceTypePLTable] on 
		([WebResourceTypePLTable].AttributeName = 'webresourcetype'
		and [WebResourceTypePLTable].ObjectTypeCode = 9333
		and [WebResourceTypePLTable].AttributeValue = [WebResource].[WebResourceType]
		and [WebResourceTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9333) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [WebResource].OrganizationId = u.OrganizationId
)



--
-- report view for ribboncontextgroup
--
create view dbo.[FilteredRibbonContextGroup] (
    [componentstate],
    [contextgroupid],
    [contextgroupxml],
    [entity],
    [ismanaged],
    [ismanagedname],
    [organizationid],
    [overwritetime],
    [overwritetimeutc],
    [ribboncontextgroupid],
    [ribboncontextgroupuniqueid],
    [ribboncustomizationid],
    [solutionid],
    [versionnumber]
) with view_metadata as
select
    [RibbonContextGroup].[ComponentState],
    [RibbonContextGroup].[ContextGroupId],
    [RibbonContextGroup].[ContextGroupXml],
    [RibbonContextGroup].[Entity],
    [RibbonContextGroup].[IsManaged],
    IsManagedPLTable.Value,
    [RibbonContextGroup].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([RibbonContextGroup].[OverwriteTime], 
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
        [RibbonContextGroup].[OverwriteTime],
    [RibbonContextGroup].[RibbonContextGroupId],
    [RibbonContextGroup].[RibbonContextGroupUniqueId],
    [RibbonContextGroup].[RibbonCustomizationId],
    [RibbonContextGroup].[SolutionId],
    [RibbonContextGroup].[VersionNumber]
from RibbonContextGroup
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1115
		and [IsManagedPLTable].AttributeValue = [RibbonContextGroup].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1115) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RibbonContextGroup].OrganizationId = u.OrganizationId
)

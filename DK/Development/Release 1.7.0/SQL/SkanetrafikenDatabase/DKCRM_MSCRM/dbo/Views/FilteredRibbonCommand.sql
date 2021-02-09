

--
-- report view for ribboncommand
--
create view dbo.[FilteredRibbonCommand] (
    [command],
    [commanddefinition],
    [componentstate],
    [entity],
    [ismanaged],
    [ismanagedname],
    [organizationid],
    [overwritetime],
    [overwritetimeutc],
    [ribboncommandid],
    [ribboncommanduniqueid],
    [ribboncustomizationid],
    [solutionid],
    [versionnumber]
) with view_metadata as
select
    [RibbonCommand].[Command],
    [RibbonCommand].[CommandDefinition],
    [RibbonCommand].[ComponentState],
    [RibbonCommand].[Entity],
    [RibbonCommand].[IsManaged],
    IsManagedPLTable.Value,
    [RibbonCommand].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([RibbonCommand].[OverwriteTime], 
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
        [RibbonCommand].[OverwriteTime],
    [RibbonCommand].[RibbonCommandId],
    [RibbonCommand].[RibbonCommandUniqueId],
    [RibbonCommand].[RibbonCustomizationId],
    [RibbonCommand].[SolutionId],
    [RibbonCommand].[VersionNumber]
from RibbonCommand
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1116
		and [IsManagedPLTable].AttributeValue = [RibbonCommand].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1116) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RibbonCommand].OrganizationId = u.OrganizationId
)

﻿

--
-- report view for ribbontabtocommandmap
--
create view dbo.[FilteredRibbonTabToCommandMap] (
    [command],
    [componentstate],
    [controlid],
    [entity],
    [ismanaged],
    [ismanagedname],
    [organizationid],
    [overwritetime],
    [overwritetimeutc],
    [ribbondiffid],
    [ribbontabtocommandmapid],
    [ribbontabtocommandmapuniqueid],
    [solutionid],
    [tabid],
    [versionnumber]
) with view_metadata as
select
    [RibbonTabToCommandMap].[Command],
    [RibbonTabToCommandMap].[ComponentState],
    [RibbonTabToCommandMap].[ControlId],
    [RibbonTabToCommandMap].[Entity],
    [RibbonTabToCommandMap].[IsManaged],
    IsManagedPLTable.Value,
    [RibbonTabToCommandMap].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([RibbonTabToCommandMap].[OverwriteTime], 
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
        [RibbonTabToCommandMap].[OverwriteTime],
    [RibbonTabToCommandMap].[RibbonDiffId],
    [RibbonTabToCommandMap].[RibbonTabToCommandMapId],
    [RibbonTabToCommandMap].[RibbonTabToCommandMapUniqueId],
    [RibbonTabToCommandMap].[SolutionId],
    [RibbonTabToCommandMap].[TabId],
    [RibbonTabToCommandMap].[VersionNumber]
from RibbonTabToCommandMap
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1113
		and [IsManagedPLTable].AttributeValue = [RibbonTabToCommandMap].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1113) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RibbonTabToCommandMap].OrganizationId = u.OrganizationId
)

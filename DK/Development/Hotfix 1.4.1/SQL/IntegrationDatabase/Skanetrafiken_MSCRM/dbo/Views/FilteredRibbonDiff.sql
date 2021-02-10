

--
-- report view for ribbondiff
--
create view dbo.[FilteredRibbonDiff] (
    [componentstate],
    [contextgroupid],
    [diffid],
    [difftype],
    [entity],
    [ismanaged],
    [ismanagedname],
    [organizationid],
    [overwritetime],
    [overwritetimeutc],
    [rdx],
    [ribboncustomizationid],
    [ribbondiffid],
    [ribbondiffuniqueid],
    [sequence],
    [solutionid],
    [tabid],
    [versionnumber]
) with view_metadata as
select
    [RibbonDiff].[ComponentState],
    [RibbonDiff].[ContextGroupId],
    [RibbonDiff].[DiffId],
    [RibbonDiff].[DiffType],
    [RibbonDiff].[Entity],
    [RibbonDiff].[IsManaged],
    IsManagedPLTable.Value,
    [RibbonDiff].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([RibbonDiff].[OverwriteTime], 
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
        [RibbonDiff].[OverwriteTime],
    [RibbonDiff].[RDX],
    [RibbonDiff].[RibbonCustomizationId],
    [RibbonDiff].[RibbonDiffId],
    [RibbonDiff].[RibbonDiffUniqueId],
    [RibbonDiff].[Sequence],
    [RibbonDiff].[SolutionId],
    [RibbonDiff].[TabId],
    [RibbonDiff].[VersionNumber]
from RibbonDiff
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1130
		and [IsManagedPLTable].AttributeValue = [RibbonDiff].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1130) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RibbonDiff].OrganizationId = u.OrganizationId
)

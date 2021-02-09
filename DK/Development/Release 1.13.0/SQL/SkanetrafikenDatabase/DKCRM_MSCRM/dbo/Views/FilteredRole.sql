

--
-- report view for role
--
create view dbo.[FilteredRole] (
    [businessunitid],
    [businessunitiddsc],
    [businessunitidname],
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
    [name],
    [organizationid],
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [overwritetime],
    [overwritetimeutc],
    [parentroleid],
    [parentroleiddsc],
    [parentroleidname],
    [parentrootroleid],
    [parentrootroleidname],
    [roleid],
    [roleidunique],
    [roletemplateid],
    [solutionid],
    [versionnumber]
) with view_metadata as
select
    [Role].[BusinessUnitId],
    --[Role].[BusinessUnitIdDsc]
    0,
    [Role].[BusinessUnitIdName],
    [Role].[ComponentState],
    [Role].[CreatedBy],
    --[Role].[CreatedByDsc]
    0,
    [Role].[CreatedByName],
    [Role].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Role].[CreatedOn], 
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
        [Role].[CreatedOn],
    [Role].[CreatedOnBehalfBy],
    --[Role].[CreatedOnBehalfByDsc]
    0,
    [Role].[CreatedOnBehalfByName],
    [Role].[CreatedOnBehalfByYomiName],
    [Role].[ImportSequenceNumber],
    [Role].[IsCustomizable],
    [Role].[IsManaged],
    IsManagedPLTable.Value,
    [Role].[ModifiedBy],
    --[Role].[ModifiedByDsc]
    0,
    [Role].[ModifiedByName],
    [Role].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Role].[ModifiedOn], 
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
        [Role].[ModifiedOn],
    [Role].[ModifiedOnBehalfBy],
    --[Role].[ModifiedOnBehalfByDsc]
    0,
    [Role].[ModifiedOnBehalfByName],
    [Role].[ModifiedOnBehalfByYomiName],
    [Role].[Name],
    [Role].[OrganizationId],
    --[Role].[OrganizationIdDsc]
    0,
    [Role].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([Role].[OverriddenCreatedOn], 
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
        [Role].[OverriddenCreatedOn],
    dbo.fn_UTCToTzSpecificLocalTime([Role].[OverwriteTime], 
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
        [Role].[OverwriteTime],
    [Role].[ParentRoleId],
    --[Role].[ParentRoleIdDsc]
    0,
    [Role].[ParentRoleIdName],
    [Role].[ParentRootRoleId],
    [Role].[ParentRootRoleIdName],
    [Role].[RoleId],
    [Role].[RoleIdUnique],
    [Role].[RoleTemplateId],
    [Role].[SolutionId],
    [Role].[VersionNumber]
from Role
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1036
		and [IsManagedPLTable].AttributeValue = [Role].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1036) pdm
where
(
    
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[Role].[BusinessUnitId] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 1036)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Role].[BusinessUnitId] is not null 
	) 
)

)

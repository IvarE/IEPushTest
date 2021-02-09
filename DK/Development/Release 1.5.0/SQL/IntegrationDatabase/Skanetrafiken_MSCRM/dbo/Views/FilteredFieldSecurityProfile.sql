

--
-- report view for fieldsecurityprofile
--
create view dbo.[FilteredFieldSecurityProfile] (
    [componentstate],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [fieldsecurityprofileid],
    [fieldsecurityprofileidunique],
    [ismanaged],
    [ismanagedname],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [organizationidname],
    [overwritetime],
    [overwritetimeutc],
    [solutionid],
    [versionnumber]
) with view_metadata as
select
    [FieldSecurityProfile].[ComponentState],
    [FieldSecurityProfile].[CreatedBy],
    [FieldSecurityProfile].[CreatedByName],
    [FieldSecurityProfile].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([FieldSecurityProfile].[CreatedOn], 
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
        [FieldSecurityProfile].[CreatedOn],
    [FieldSecurityProfile].[CreatedOnBehalfBy],
    [FieldSecurityProfile].[CreatedOnBehalfByName],
    [FieldSecurityProfile].[CreatedOnBehalfByYomiName],
    [FieldSecurityProfile].[Description],
    [FieldSecurityProfile].[FieldSecurityProfileId],
    [FieldSecurityProfile].[FieldSecurityProfileIdUnique],
    [FieldSecurityProfile].[IsManaged],
    IsManagedPLTable.Value,
    [FieldSecurityProfile].[ModifiedBy],
    [FieldSecurityProfile].[ModifiedByName],
    [FieldSecurityProfile].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([FieldSecurityProfile].[ModifiedOn], 
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
        [FieldSecurityProfile].[ModifiedOn],
    [FieldSecurityProfile].[ModifiedOnBehalfBy],
    [FieldSecurityProfile].[ModifiedOnBehalfByName],
    [FieldSecurityProfile].[ModifiedOnBehalfByYomiName],
    [FieldSecurityProfile].[Name],
    [FieldSecurityProfile].[OrganizationId],
    [FieldSecurityProfile].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([FieldSecurityProfile].[OverwriteTime], 
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
        [FieldSecurityProfile].[OverwriteTime],
    [FieldSecurityProfile].[SolutionId],
    [FieldSecurityProfile].[VersionNumber]
from FieldSecurityProfile
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1200
		and [IsManagedPLTable].AttributeValue = [FieldSecurityProfile].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1200) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [FieldSecurityProfile].OrganizationId = u.OrganizationId
)

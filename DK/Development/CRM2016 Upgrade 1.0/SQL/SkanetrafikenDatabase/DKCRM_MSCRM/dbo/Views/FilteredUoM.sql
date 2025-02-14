﻿

--
-- report view for uom
--
create view dbo.[FilteredUoM] (
    [baseuom],
    [baseuomname],
    [baseuomnamedsc],
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
    [isschedulebaseuom],
    [isschedulebaseuomname],
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
    [overriddencreatedon],
    [overriddencreatedonutc],
    [quantity],
    [uomid],
    [uomscheduleid],
    [versionnumber]
) with view_metadata as
select
    [UoM].[BaseUoM],
    [UoM].[BaseUoMName],
    --[UoM].[BaseUoMNameDsc]
    0,
    [UoM].[CreatedBy],
    --[UoM].[CreatedByDsc]
    0,
    [UoM].[CreatedByName],
    [UoM].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([UoM].[CreatedOn], 
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
        [UoM].[CreatedOn],
    [UoM].[CreatedOnBehalfBy],
    --[UoM].[CreatedOnBehalfByDsc]
    0,
    [UoM].[CreatedOnBehalfByName],
    [UoM].[CreatedOnBehalfByYomiName],
    [UoM].[ImportSequenceNumber],
    [UoM].[IsScheduleBaseUoM],
    IsScheduleBaseUoMPLTable.Value,
    [UoM].[ModifiedBy],
    --[UoM].[ModifiedByDsc]
    0,
    [UoM].[ModifiedByName],
    [UoM].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([UoM].[ModifiedOn], 
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
        [UoM].[ModifiedOn],
    [UoM].[ModifiedOnBehalfBy],
    --[UoM].[ModifiedOnBehalfByDsc]
    0,
    [UoM].[ModifiedOnBehalfByName],
    [UoM].[ModifiedOnBehalfByYomiName],
    [UoM].[Name],
    [UoM].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([UoM].[OverriddenCreatedOn], 
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
        [UoM].[OverriddenCreatedOn],
    [UoM].[Quantity],
    [UoM].[UoMId],
    [UoM].[UoMScheduleId],
    [UoM].[VersionNumber]
from UoM
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsScheduleBaseUoMPLTable] on 
		([IsScheduleBaseUoMPLTable].AttributeName = 'isschedulebaseuom'
		and [IsScheduleBaseUoMPLTable].ObjectTypeCode = 1055
		and [IsScheduleBaseUoMPLTable].AttributeValue = [UoM].[IsScheduleBaseUoM]
		and [IsScheduleBaseUoMPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1056) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [UoM].OrganizationId = u.OrganizationId
)

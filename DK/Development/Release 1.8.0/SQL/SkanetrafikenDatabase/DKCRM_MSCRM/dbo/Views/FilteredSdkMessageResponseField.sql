

--
-- report view for sdkmessageresponsefield
--
create view dbo.[FilteredSdkMessageResponseField] (
    [clrformatter],
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [customizationlevel],
    [formatter],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [position],
    [publicname],
    [sdkmessageresponsefieldid],
    [sdkmessageresponsefieldidunique],
    [sdkmessageresponseid],
    [value],
    [versionnumber]
) with view_metadata as
select
    [SdkMessageResponseField].[ClrFormatter],
    [SdkMessageResponseField].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageResponseField].[CreatedOn], 
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
        [SdkMessageResponseField].[CreatedOn],
    [SdkMessageResponseField].[CreatedOnBehalfBy],
    --[SdkMessageResponseField].[CreatedOnBehalfByDsc]
    0,
    [SdkMessageResponseField].[CreatedOnBehalfByName],
    [SdkMessageResponseField].[CreatedOnBehalfByYomiName],
    [SdkMessageResponseField].[CustomizationLevel],
    [SdkMessageResponseField].[Formatter],
    [SdkMessageResponseField].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageResponseField].[ModifiedOn], 
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
        [SdkMessageResponseField].[ModifiedOn],
    [SdkMessageResponseField].[ModifiedOnBehalfBy],
    --[SdkMessageResponseField].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessageResponseField].[ModifiedOnBehalfByName],
    [SdkMessageResponseField].[ModifiedOnBehalfByYomiName],
    [SdkMessageResponseField].[Name],
    [SdkMessageResponseField].[OrganizationId],
    [SdkMessageResponseField].[Position],
    [SdkMessageResponseField].[PublicName],
    [SdkMessageResponseField].[SdkMessageResponseFieldId],
    [SdkMessageResponseField].[SdkMessageResponseFieldIdUnique],
    [SdkMessageResponseField].[SdkMessageResponseId],
    [SdkMessageResponseField].[Value],
    [SdkMessageResponseField].[VersionNumber]
from SdkMessageResponseField
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4611) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessageResponseField].OrganizationId = u.OrganizationId
)

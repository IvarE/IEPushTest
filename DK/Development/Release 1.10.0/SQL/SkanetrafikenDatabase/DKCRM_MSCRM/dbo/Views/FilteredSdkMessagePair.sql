

--
-- report view for sdkmessagepair
--
create view dbo.[FilteredSdkMessagePair] (
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [customizationlevel],
    [endpoint],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [namespace],
    [organizationid],
    [sdkmessageid],
    [sdkmessagepairid],
    [sdkmessagepairidunique],
    [versionnumber]
) with view_metadata as
select
    [SdkMessagePair].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessagePair].[CreatedOn], 
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
        [SdkMessagePair].[CreatedOn],
    [SdkMessagePair].[CreatedOnBehalfBy],
    --[SdkMessagePair].[CreatedOnBehalfByDsc]
    0,
    [SdkMessagePair].[CreatedOnBehalfByName],
    [SdkMessagePair].[CreatedOnBehalfByYomiName],
    [SdkMessagePair].[CustomizationLevel],
    [SdkMessagePair].[Endpoint],
    [SdkMessagePair].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessagePair].[ModifiedOn], 
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
        [SdkMessagePair].[ModifiedOn],
    [SdkMessagePair].[ModifiedOnBehalfBy],
    --[SdkMessagePair].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessagePair].[ModifiedOnBehalfByName],
    [SdkMessagePair].[ModifiedOnBehalfByYomiName],
    [SdkMessagePair].[Namespace],
    [SdkMessagePair].[OrganizationId],
    [SdkMessagePair].[SdkMessageId],
    [SdkMessagePair].[SdkMessagePairId],
    [SdkMessagePair].[SdkMessagePairIdUnique],
    [SdkMessagePair].[VersionNumber]
from SdkMessagePair
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4613) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessagePair].OrganizationId = u.OrganizationId
)

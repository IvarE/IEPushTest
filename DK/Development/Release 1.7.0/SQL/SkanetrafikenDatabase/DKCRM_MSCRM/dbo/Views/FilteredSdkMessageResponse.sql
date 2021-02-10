

--
-- report view for sdkmessageresponse
--
create view dbo.[FilteredSdkMessageResponse] (
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [customizationlevel],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [organizationid],
    [sdkmessagerequestid],
    [sdkmessageresponseid],
    [sdkmessageresponseidunique],
    [versionnumber]
) with view_metadata as
select
    [SdkMessageResponse].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageResponse].[CreatedOn], 
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
        [SdkMessageResponse].[CreatedOn],
    [SdkMessageResponse].[CreatedOnBehalfBy],
    --[SdkMessageResponse].[CreatedOnBehalfByDsc]
    0,
    [SdkMessageResponse].[CreatedOnBehalfByName],
    [SdkMessageResponse].[CreatedOnBehalfByYomiName],
    [SdkMessageResponse].[CustomizationLevel],
    [SdkMessageResponse].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageResponse].[ModifiedOn], 
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
        [SdkMessageResponse].[ModifiedOn],
    [SdkMessageResponse].[ModifiedOnBehalfBy],
    --[SdkMessageResponse].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessageResponse].[ModifiedOnBehalfByName],
    [SdkMessageResponse].[ModifiedOnBehalfByYomiName],
    [SdkMessageResponse].[OrganizationId],
    [SdkMessageResponse].[SdkMessageRequestId],
    [SdkMessageResponse].[SdkMessageResponseId],
    [SdkMessageResponse].[SdkMessageResponseIdUnique],
    [SdkMessageResponse].[VersionNumber]
from SdkMessageResponse
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4610) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessageResponse].OrganizationId = u.OrganizationId
)

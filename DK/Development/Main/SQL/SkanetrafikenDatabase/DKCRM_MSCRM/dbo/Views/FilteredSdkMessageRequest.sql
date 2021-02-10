

--
-- report view for sdkmessagerequest
--
create view dbo.[FilteredSdkMessageRequest] (
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
    [name],
    [organizationid],
    [primaryobjecttypecode],
    [sdkmessagepairid],
    [sdkmessagerequestid],
    [sdkmessagerequestidunique],
    [versionnumber]
) with view_metadata as
select
    [SdkMessageRequest].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageRequest].[CreatedOn], 
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
        [SdkMessageRequest].[CreatedOn],
    [SdkMessageRequest].[CreatedOnBehalfBy],
    --[SdkMessageRequest].[CreatedOnBehalfByDsc]
    0,
    [SdkMessageRequest].[CreatedOnBehalfByName],
    [SdkMessageRequest].[CreatedOnBehalfByYomiName],
    [SdkMessageRequest].[CustomizationLevel],
    [SdkMessageRequest].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageRequest].[ModifiedOn], 
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
        [SdkMessageRequest].[ModifiedOn],
    [SdkMessageRequest].[ModifiedOnBehalfBy],
    --[SdkMessageRequest].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessageRequest].[ModifiedOnBehalfByName],
    [SdkMessageRequest].[ModifiedOnBehalfByYomiName],
    [SdkMessageRequest].[Name],
    [SdkMessageRequest].[OrganizationId],
    [SdkMessageRequest].[PrimaryObjectTypeCode],
    [SdkMessageRequest].[SdkMessagePairId],
    [SdkMessageRequest].[SdkMessageRequestId],
    [SdkMessageRequest].[SdkMessageRequestIdUnique],
    [SdkMessageRequest].[VersionNumber]
from SdkMessageRequest
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4609) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessageRequest].OrganizationId = u.OrganizationId
)

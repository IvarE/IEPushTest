

--
-- report view for sdkmessagerequestfield
--
create view dbo.[FilteredSdkMessageRequestField] (
    [clrparser],
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [customizationlevel],
    [fieldmask],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [optional],
    [organizationid],
    [parser],
    [position],
    [publicname],
    [sdkmessagerequestfieldid],
    [sdkmessagerequestfieldidunique],
    [sdkmessagerequestid],
    [versionnumber]
) with view_metadata as
select
    [SdkMessageRequestField].[ClrParser],
    [SdkMessageRequestField].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageRequestField].[CreatedOn], 
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
        [SdkMessageRequestField].[CreatedOn],
    [SdkMessageRequestField].[CreatedOnBehalfBy],
    --[SdkMessageRequestField].[CreatedOnBehalfByDsc]
    0,
    [SdkMessageRequestField].[CreatedOnBehalfByName],
    [SdkMessageRequestField].[CreatedOnBehalfByYomiName],
    [SdkMessageRequestField].[CustomizationLevel],
    [SdkMessageRequestField].[FieldMask],
    [SdkMessageRequestField].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageRequestField].[ModifiedOn], 
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
        [SdkMessageRequestField].[ModifiedOn],
    [SdkMessageRequestField].[ModifiedOnBehalfBy],
    --[SdkMessageRequestField].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessageRequestField].[ModifiedOnBehalfByName],
    [SdkMessageRequestField].[ModifiedOnBehalfByYomiName],
    [SdkMessageRequestField].[Name],
    [SdkMessageRequestField].[Optional],
    [SdkMessageRequestField].[OrganizationId],
    [SdkMessageRequestField].[Parser],
    [SdkMessageRequestField].[Position],
    [SdkMessageRequestField].[PublicName],
    [SdkMessageRequestField].[SdkMessageRequestFieldId],
    [SdkMessageRequestField].[SdkMessageRequestFieldIdUnique],
    [SdkMessageRequestField].[SdkMessageRequestId],
    [SdkMessageRequestField].[VersionNumber]
from SdkMessageRequestField
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4614) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessageRequestField].OrganizationId = u.OrganizationId
)

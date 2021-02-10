

--
-- report view for sdkmessagefilter
--
create view dbo.[FilteredSdkMessageFilter] (
    [availability],
    [createdby],
    [createdbyname],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [customizationlevel],
    [iscustomprocessingstepallowed],
    [iscustomprocessingstepallowedname],
    [isvisible],
    [modifiedby],
    [modifiedbyname],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [organizationid],
    [primaryobjecttypecode],
    [primaryobjecttypecodename],
    [sdkmessagefilterid],
    [sdkmessagefilteridunique],
    [sdkmessageid],
    [sdkmessageidname],
    [secondaryobjecttypecode],
    [secondaryobjecttypecodename],
    [versionnumber]
) with view_metadata as
select
    [SdkMessageFilter].[Availability],
    [SdkMessageFilter].[CreatedBy],
    [SdkMessageFilter].[CreatedByName],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageFilter].[CreatedOn], 
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
        [SdkMessageFilter].[CreatedOn],
    [SdkMessageFilter].[CreatedOnBehalfBy],
    --[SdkMessageFilter].[CreatedOnBehalfByDsc]
    0,
    [SdkMessageFilter].[CreatedOnBehalfByName],
    [SdkMessageFilter].[CreatedOnBehalfByYomiName],
    [SdkMessageFilter].[CustomizationLevel],
    [SdkMessageFilter].[IsCustomProcessingStepAllowed],
    IsCustomProcessingStepAllowedPLTable.Value,
    [SdkMessageFilter].[IsVisible],
    [SdkMessageFilter].[ModifiedBy],
    [SdkMessageFilter].[ModifiedByName],
    dbo.fn_UTCToTzSpecificLocalTime([SdkMessageFilter].[ModifiedOn], 
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
        [SdkMessageFilter].[ModifiedOn],
    [SdkMessageFilter].[ModifiedOnBehalfBy],
    --[SdkMessageFilter].[ModifiedOnBehalfByDsc]
    0,
    [SdkMessageFilter].[ModifiedOnBehalfByName],
    [SdkMessageFilter].[ModifiedOnBehalfByYomiName],
    [SdkMessageFilter].[OrganizationId],
    [SdkMessageFilter].[PrimaryObjectTypeCode],
    PrimaryObjectTypeCodePLTable.Value,
    [SdkMessageFilter].[SdkMessageFilterId],
    [SdkMessageFilter].[SdkMessageFilterIdUnique],
    [SdkMessageFilter].[SdkMessageId],
    [SdkMessageFilter].[SdkMessageIdName],
    [SdkMessageFilter].[SecondaryObjectTypeCode],
    SecondaryObjectTypeCodePLTable.Value,
    [SdkMessageFilter].[VersionNumber]
from SdkMessageFilter
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsCustomProcessingStepAllowedPLTable] on 
		([IsCustomProcessingStepAllowedPLTable].AttributeName = 'iscustomprocessingstepallowed'
		and [IsCustomProcessingStepAllowedPLTable].ObjectTypeCode = 4607
		and [IsCustomProcessingStepAllowedPLTable].AttributeValue = [SdkMessageFilter].[IsCustomProcessingStepAllowed]
		and [IsCustomProcessingStepAllowedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PrimaryObjectTypeCodePLTable] on 
		([PrimaryObjectTypeCodePLTable].AttributeName = 'primaryobjecttypecode'
		and [PrimaryObjectTypeCodePLTable].ObjectTypeCode = 4607
		and [PrimaryObjectTypeCodePLTable].AttributeValue = [SdkMessageFilter].[PrimaryObjectTypeCode]
		and [PrimaryObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SecondaryObjectTypeCodePLTable] on 
		([SecondaryObjectTypeCodePLTable].AttributeName = 'secondaryobjecttypecode'
		and [SecondaryObjectTypeCodePLTable].ObjectTypeCode = 4607
		and [SecondaryObjectTypeCodePLTable].AttributeValue = [SdkMessageFilter].[SecondaryObjectTypeCode]
		and [SecondaryObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4607) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SdkMessageFilter].OrganizationId = u.OrganizationId
)

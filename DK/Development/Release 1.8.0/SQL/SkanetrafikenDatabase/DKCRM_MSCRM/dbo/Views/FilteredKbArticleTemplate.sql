

--
-- report view for kbarticletemplate
--
create view dbo.[FilteredKbArticleTemplate] (
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
    [description],
    [formatxml],
    [importsequencenumber],
    [introducedversion],
    [isactive],
    [isactivename],
    [iscustomizable],
    [ismanaged],
    [ismanagedname],
    [kbarticletemplateid],
    [kbarticletemplateidunique],
    [languagecode],
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
    [organizationid],
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [overwritetime],
    [overwritetimeutc],
    [solutionid],
    [structurexml],
    [title],
    [versionnumber]
) with view_metadata as
select
    [KbArticleTemplate].[ComponentState],
    [KbArticleTemplate].[CreatedBy],
    --[KbArticleTemplate].[CreatedByDsc]
    0,
    [KbArticleTemplate].[CreatedByName],
    [KbArticleTemplate].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticleTemplate].[CreatedOn], 
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
        [KbArticleTemplate].[CreatedOn],
    [KbArticleTemplate].[CreatedOnBehalfBy],
    --[KbArticleTemplate].[CreatedOnBehalfByDsc]
    0,
    [KbArticleTemplate].[CreatedOnBehalfByName],
    [KbArticleTemplate].[CreatedOnBehalfByYomiName],
    [KbArticleTemplate].[Description],
    [KbArticleTemplate].[FormatXml],
    [KbArticleTemplate].[ImportSequenceNumber],
    [KbArticleTemplate].[IntroducedVersion],
    [KbArticleTemplate].[IsActive],
    IsActivePLTable.Value,
    [KbArticleTemplate].[IsCustomizable],
    [KbArticleTemplate].[IsManaged],
    IsManagedPLTable.Value,
    [KbArticleTemplate].[KbArticleTemplateId],
    [KbArticleTemplate].[KbArticleTemplateIdUnique],
    [KbArticleTemplate].[LanguageCode],
    [KbArticleTemplate].[ModifiedBy],
    --[KbArticleTemplate].[ModifiedByDsc]
    0,
    [KbArticleTemplate].[ModifiedByName],
    [KbArticleTemplate].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticleTemplate].[ModifiedOn], 
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
        [KbArticleTemplate].[ModifiedOn],
    [KbArticleTemplate].[ModifiedOnBehalfBy],
    --[KbArticleTemplate].[ModifiedOnBehalfByDsc]
    0,
    [KbArticleTemplate].[ModifiedOnBehalfByName],
    [KbArticleTemplate].[ModifiedOnBehalfByYomiName],
    [KbArticleTemplate].[OrganizationId],
    --[KbArticleTemplate].[OrganizationIdDsc]
    0,
    [KbArticleTemplate].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticleTemplate].[OverriddenCreatedOn], 
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
        [KbArticleTemplate].[OverriddenCreatedOn],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticleTemplate].[OverwriteTime], 
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
        [KbArticleTemplate].[OverwriteTime],
    [KbArticleTemplate].[SolutionId],
    [KbArticleTemplate].[StructureXml],
    [KbArticleTemplate].[Title],
    [KbArticleTemplate].[VersionNumber]
from KbArticleTemplate
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsActivePLTable] on 
		([IsActivePLTable].AttributeName = 'isactive'
		and [IsActivePLTable].ObjectTypeCode = 1016
		and [IsActivePLTable].AttributeValue = [KbArticleTemplate].[IsActive]
		and [IsActivePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 1016
		and [IsManagedPLTable].AttributeValue = [KbArticleTemplate].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1016) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [KbArticleTemplate].OrganizationId = u.OrganizationId
)

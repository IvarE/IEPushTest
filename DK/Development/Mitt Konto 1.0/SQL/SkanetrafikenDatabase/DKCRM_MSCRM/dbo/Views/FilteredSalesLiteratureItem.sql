

--
-- report view for salesliteratureitem
--
create view dbo.[FilteredSalesLiteratureItem] (
    [abstract],
    [attacheddocumenturl],
    [authorname],
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
    [documentbody],
    [filename],
    [filesize],
    [filetypecode],
    [filetypecodename],
    [importsequencenumber],
    [iscustomerviewable],
    [iscustomerviewablename],
    [keywords],
    [mimetype],
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
    [overriddencreatedon],
    [overriddencreatedonutc],
    [salesliteratureid],
    [salesliteratureitemid],
    [title],
    [versionnumber]
) with view_metadata as
select
    [SalesLiteratureItem].[Abstract],
    [SalesLiteratureItem].[AttachedDocumentUrl],
    [SalesLiteratureItem].[AuthorName],
    [SalesLiteratureItem].[CreatedBy],
    --[SalesLiteratureItem].[CreatedByDsc]
    0,
    [SalesLiteratureItem].[CreatedByName],
    [SalesLiteratureItem].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiteratureItem].[CreatedOn], 
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
        [SalesLiteratureItem].[CreatedOn],
    [SalesLiteratureItem].[CreatedOnBehalfBy],
    --[SalesLiteratureItem].[CreatedOnBehalfByDsc]
    0,
    [SalesLiteratureItem].[CreatedOnBehalfByName],
    [SalesLiteratureItem].[CreatedOnBehalfByYomiName],
    [SalesLiteratureItem].[DocumentBody],
    [SalesLiteratureItem].[FileName],
    [SalesLiteratureItem].[FileSize],
    [SalesLiteratureItem].[FileTypeCode],
    FileTypeCodePLTable.Value,
    [SalesLiteratureItem].[ImportSequenceNumber],
    [SalesLiteratureItem].[IsCustomerViewable],
    IsCustomerViewablePLTable.Value,
    [SalesLiteratureItem].[KeyWords],
    [SalesLiteratureItem].[MimeType],
    [SalesLiteratureItem].[ModifiedBy],
    --[SalesLiteratureItem].[ModifiedByDsc]
    0,
    [SalesLiteratureItem].[ModifiedByName],
    [SalesLiteratureItem].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiteratureItem].[ModifiedOn], 
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
        [SalesLiteratureItem].[ModifiedOn],
    [SalesLiteratureItem].[ModifiedOnBehalfBy],
    --[SalesLiteratureItem].[ModifiedOnBehalfByDsc]
    0,
    [SalesLiteratureItem].[ModifiedOnBehalfByName],
    [SalesLiteratureItem].[ModifiedOnBehalfByYomiName],
    [SalesLiteratureItem].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiteratureItem].[OverriddenCreatedOn], 
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
        [SalesLiteratureItem].[OverriddenCreatedOn],
    [SalesLiteratureItem].[SalesLiteratureId],
    [SalesLiteratureItem].[SalesLiteratureItemId],
    [SalesLiteratureItem].[Title],
    [SalesLiteratureItem].[VersionNumber]
from SalesLiteratureItem
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [FileTypeCodePLTable] on 
		([FileTypeCodePLTable].AttributeName = 'filetypecode'
		and [FileTypeCodePLTable].ObjectTypeCode = 1070
		and [FileTypeCodePLTable].AttributeValue = [SalesLiteratureItem].[FileTypeCode]
		and [FileTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsCustomerViewablePLTable] on 
		([IsCustomerViewablePLTable].AttributeName = 'iscustomerviewable'
		and [IsCustomerViewablePLTable].ObjectTypeCode = 1070
		and [IsCustomerViewablePLTable].AttributeValue = [SalesLiteratureItem].[IsCustomerViewable]
		and [IsCustomerViewablePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1038) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SalesLiteratureItem].OrganizationId = u.OrganizationId
)

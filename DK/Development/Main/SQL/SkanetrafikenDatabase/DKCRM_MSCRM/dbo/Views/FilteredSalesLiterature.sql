

--
-- report view for salesliterature
--
create view dbo.[FilteredSalesLiterature] (
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
    [employeecontactid],
    [employeecontactiddsc],
    [employeecontactidname],
    [employeecontactidyominame],
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [expirationdate],
    [expirationdateutc],
    [hasattachments],
    [hasattachmentsname],
    [importsequencenumber],
    [iscustomerviewable],
    [iscustomerviewablename],
    [keywords],
    [literaturetypecode],
    [literaturetypecodename],
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
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [processid],
    [salesliteratureid],
    [stageid],
    [subjectid],
    [subjectiddsc],
    [subjectidname],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [SalesLiterature].[CreatedBy],
    --[SalesLiterature].[CreatedByDsc]
    0,
    [SalesLiterature].[CreatedByName],
    [SalesLiterature].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiterature].[CreatedOn], 
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
        [SalesLiterature].[CreatedOn],
    [SalesLiterature].[CreatedOnBehalfBy],
    --[SalesLiterature].[CreatedOnBehalfByDsc]
    0,
    [SalesLiterature].[CreatedOnBehalfByName],
    [SalesLiterature].[CreatedOnBehalfByYomiName],
    [SalesLiterature].[Description],
    [SalesLiterature].[EmployeeContactId],
    --[SalesLiterature].[EmployeeContactIdDsc]
    0,
    [SalesLiterature].[EmployeeContactIdName],
    [SalesLiterature].[EmployeeContactIdYomiName],
    --[SalesLiterature].[EntityImage]
    cast(null as varbinary),
    [SalesLiterature].[EntityImageId],
    [SalesLiterature].[EntityImage_Timestamp],
    [SalesLiterature].[EntityImage_URL],
    [SalesLiterature].[ExchangeRate],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiterature].[ExpirationDate], 
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
        [SalesLiterature].[ExpirationDate],
    [SalesLiterature].[HasAttachments],
    HasAttachmentsPLTable.Value,
    [SalesLiterature].[ImportSequenceNumber],
    [SalesLiterature].[IsCustomerViewable],
    IsCustomerViewablePLTable.Value,
    [SalesLiterature].[KeyWords],
    [SalesLiterature].[LiteratureTypeCode],
    LiteratureTypeCodePLTable.Value,
    [SalesLiterature].[ModifiedBy],
    --[SalesLiterature].[ModifiedByDsc]
    0,
    [SalesLiterature].[ModifiedByName],
    [SalesLiterature].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiterature].[ModifiedOn], 
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
        [SalesLiterature].[ModifiedOn],
    [SalesLiterature].[ModifiedOnBehalfBy],
    --[SalesLiterature].[ModifiedOnBehalfByDsc]
    0,
    [SalesLiterature].[ModifiedOnBehalfByName],
    [SalesLiterature].[ModifiedOnBehalfByYomiName],
    [SalesLiterature].[Name],
    [SalesLiterature].[OrganizationId],
    --[SalesLiterature].[OrganizationIdDsc]
    0,
    [SalesLiterature].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([SalesLiterature].[OverriddenCreatedOn], 
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
        [SalesLiterature].[OverriddenCreatedOn],
    [SalesLiterature].[ProcessId],
    [SalesLiterature].[SalesLiteratureId],
    [SalesLiterature].[StageId],
    [SalesLiterature].[SubjectId],
    --[SalesLiterature].[SubjectIdDsc]
    0,
    [SalesLiterature].[SubjectIdName],
    [SalesLiterature].[TimeZoneRuleVersionNumber],
    [SalesLiterature].[TransactionCurrencyId],
    [SalesLiterature].[TransactionCurrencyIdName],
    [SalesLiterature].[UTCConversionTimeZoneCode],
    [SalesLiterature].[VersionNumber]
from SalesLiterature
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [HasAttachmentsPLTable] on 
		([HasAttachmentsPLTable].AttributeName = 'hasattachments'
		and [HasAttachmentsPLTable].ObjectTypeCode = 1038
		and [HasAttachmentsPLTable].AttributeValue = [SalesLiterature].[HasAttachments]
		and [HasAttachmentsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsCustomerViewablePLTable] on 
		([IsCustomerViewablePLTable].AttributeName = 'iscustomerviewable'
		and [IsCustomerViewablePLTable].ObjectTypeCode = 1038
		and [IsCustomerViewablePLTable].AttributeValue = [SalesLiterature].[IsCustomerViewable]
		and [IsCustomerViewablePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [LiteratureTypeCodePLTable] on 
		([LiteratureTypeCodePLTable].AttributeName = 'literaturetypecode'
		and [LiteratureTypeCodePLTable].ObjectTypeCode = 1038
		and [LiteratureTypeCodePLTable].AttributeValue = [SalesLiterature].[LiteratureTypeCode]
		and [LiteratureTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1038) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [SalesLiterature].OrganizationId = u.OrganizationId
)

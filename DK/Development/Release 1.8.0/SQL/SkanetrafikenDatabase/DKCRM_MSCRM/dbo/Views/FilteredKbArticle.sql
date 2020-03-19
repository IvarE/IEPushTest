

--
-- report view for kbarticle
--
create view dbo.[FilteredKbArticle] (
    [articlexml],
    [cgi_appoval],
    [cgi_appovalname],
    [cgi_devlogfield],
    [cgi_externaltext],
    [cgi_externalwebindex],
    [cgi_intranetindex],
    [cgi_publishonweb],
    [cgi_publishonwebname],
    [comments],
    [content],
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
    [entityimage],
    [entityimageid],
    [entityimage_timestamp],
    [entityimage_url],
    [exchangerate],
    [importsequencenumber],
    [kbarticleid],
    [kbarticletemplateid],
    [kbarticletemplateiddsc],
    [kbarticletemplateidtitle],
    [keywords],
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
    [number],
    [organizationid],
    [organizationiddsc],
    [organizationidname],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subjectid],
    [subjectiddsc],
    [subjectidname],
    [title],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [versionnumber]
) with view_metadata as
select
    [KbArticle].[ArticleXml],
    [KbArticle].[cgi_Appoval],
    cgi_AppovalPLTable.Value,
    [KbArticle].[cgi_devLogField],
    [KbArticle].[cgi_ExternalText],
    [KbArticle].[cgi_ExternalWebIndex],
    [KbArticle].[cgi_IntranetIndex],
    [KbArticle].[cgi_PublishonWeb],
    cgi_PublishonWebPLTable.Value,
    [KbArticle].[Comments],
    [KbArticle].[Content],
    [KbArticle].[CreatedBy],
    --[KbArticle].[CreatedByDsc]
    0,
    [KbArticle].[CreatedByName],
    [KbArticle].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticle].[CreatedOn], 
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
        [KbArticle].[CreatedOn],
    [KbArticle].[CreatedOnBehalfBy],
    --[KbArticle].[CreatedOnBehalfByDsc]
    0,
    [KbArticle].[CreatedOnBehalfByName],
    [KbArticle].[CreatedOnBehalfByYomiName],
    [KbArticle].[Description],
    --[KbArticle].[EntityImage]
    cast(null as varbinary),
    [KbArticle].[EntityImageId],
    [KbArticle].[EntityImage_Timestamp],
    [KbArticle].[EntityImage_URL],
    [KbArticle].[ExchangeRate],
    [KbArticle].[ImportSequenceNumber],
    [KbArticle].[KbArticleId],
    [KbArticle].[KbArticleTemplateId],
    --[KbArticle].[KbArticleTemplateIdDsc]
    0,
    [KbArticle].[KbArticleTemplateIdTitle],
    [KbArticle].[KeyWords],
    [KbArticle].[LanguageCode],
    [KbArticle].[ModifiedBy],
    --[KbArticle].[ModifiedByDsc]
    0,
    [KbArticle].[ModifiedByName],
    [KbArticle].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticle].[ModifiedOn], 
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
        [KbArticle].[ModifiedOn],
    [KbArticle].[ModifiedOnBehalfBy],
    --[KbArticle].[ModifiedOnBehalfByDsc]
    0,
    [KbArticle].[ModifiedOnBehalfByName],
    [KbArticle].[ModifiedOnBehalfByYomiName],
    [KbArticle].[Number],
    [KbArticle].[OrganizationId],
    --[KbArticle].[OrganizationIdDsc]
    0,
    [KbArticle].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([KbArticle].[OverriddenCreatedOn], 
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
        [KbArticle].[OverriddenCreatedOn],
    [KbArticle].[StateCode],
    StateCodePLTable.Value,
    [KbArticle].[StatusCode],
    StatusCodePLTable.Value,
    [KbArticle].[SubjectId],
    --[KbArticle].[SubjectIdDsc]
    0,
    [KbArticle].[SubjectIdName],
    [KbArticle].[Title],
    [KbArticle].[TransactionCurrencyId],
    [KbArticle].[TransactionCurrencyIdName],
    [KbArticle].[VersionNumber]
from KbArticle
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [cgi_AppovalPLTable] on 
		([cgi_AppovalPLTable].AttributeName = 'cgi_appoval'
		and [cgi_AppovalPLTable].ObjectTypeCode = 127
		and [cgi_AppovalPLTable].AttributeValue = [KbArticle].[cgi_Appoval]
		and [cgi_AppovalPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [cgi_PublishonWebPLTable] on 
		([cgi_PublishonWebPLTable].AttributeName = 'cgi_publishonweb'
		and [cgi_PublishonWebPLTable].ObjectTypeCode = 127
		and [cgi_PublishonWebPLTable].AttributeValue = [KbArticle].[cgi_PublishonWeb]
		and [cgi_PublishonWebPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 127
		and [StateCodePLTable].AttributeValue = [KbArticle].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 127
		and [StatusCodePLTable].AttributeValue = [KbArticle].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(127) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [KbArticle].OrganizationId = u.OrganizationId
)

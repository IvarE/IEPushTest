﻿

--
-- report view for partnerapplication
--
create view dbo.[FilteredPartnerApplication] (
    [applicationrole],
    [applicationrolename],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [metadataurl],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [organizationidname],
    [partnerapplicationid],
    [principalid],
    [realm],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [tenantid],
    [timezoneruleversionnumber],
    [useauthorizationserver],
    [useauthorizationservername],
    [utcconversiontimezonecode]
) with view_metadata as
select
    [PartnerApplication].[ApplicationRole],
    ApplicationRolePLTable.Value,
    [PartnerApplication].[CreatedBy],
    [PartnerApplication].[CreatedByName],
    [PartnerApplication].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([PartnerApplication].[CreatedOn], 
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
        [PartnerApplication].[CreatedOn],
    [PartnerApplication].[CreatedOnBehalfBy],
    [PartnerApplication].[CreatedOnBehalfByName],
    [PartnerApplication].[CreatedOnBehalfByYomiName],
    [PartnerApplication].[MetadataUrl],
    [PartnerApplication].[ModifiedBy],
    [PartnerApplication].[ModifiedByName],
    [PartnerApplication].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([PartnerApplication].[ModifiedOn], 
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
        [PartnerApplication].[ModifiedOn],
    [PartnerApplication].[ModifiedOnBehalfBy],
    [PartnerApplication].[ModifiedOnBehalfByName],
    [PartnerApplication].[ModifiedOnBehalfByYomiName],
    [PartnerApplication].[Name],
    [PartnerApplication].[OrganizationId],
    [PartnerApplication].[OrganizationIdName],
    [PartnerApplication].[PartnerApplicationId],
    [PartnerApplication].[PrincipalId],
    [PartnerApplication].[Realm],
    [PartnerApplication].[StateCode],
    StateCodePLTable.Value,
    [PartnerApplication].[StatusCode],
    StatusCodePLTable.Value,
    [PartnerApplication].[TenantId],
    [PartnerApplication].[TimeZoneRuleVersionNumber],
    [PartnerApplication].[UseAuthorizationServer],
    UseAuthorizationServerPLTable.Value,
    [PartnerApplication].[UTCConversionTimeZoneCode]
from PartnerApplication
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ApplicationRolePLTable] on 
		([ApplicationRolePLTable].AttributeName = 'applicationrole'
		and [ApplicationRolePLTable].ObjectTypeCode = 1095
		and [ApplicationRolePLTable].AttributeValue = [PartnerApplication].[ApplicationRole]
		and [ApplicationRolePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 1095
		and [StateCodePLTable].AttributeValue = [PartnerApplication].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 1095
		and [StatusCodePLTable].AttributeValue = [PartnerApplication].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [UseAuthorizationServerPLTable] on 
		([UseAuthorizationServerPLTable].AttributeName = 'useauthorizationserver'
		and [UseAuthorizationServerPLTable].ObjectTypeCode = 1095
		and [UseAuthorizationServerPLTable].AttributeValue = [PartnerApplication].[UseAuthorizationServer]
		and [UseAuthorizationServerPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(1095) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [PartnerApplication].OrganizationId = u.OrganizationId
)

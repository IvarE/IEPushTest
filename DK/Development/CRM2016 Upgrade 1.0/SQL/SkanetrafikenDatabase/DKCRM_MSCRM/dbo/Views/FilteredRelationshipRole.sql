﻿

--
-- report view for relationshiprole
--
create view dbo.[FilteredRelationshipRole] (
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
    [importsequencenumber],
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
    [relationshiproleid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [versionnumber]
) with view_metadata as
select
    [RelationshipRole].[CreatedBy],
    --[RelationshipRole].[CreatedByDsc]
    0,
    [RelationshipRole].[CreatedByName],
    [RelationshipRole].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RelationshipRole].[CreatedOn], 
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
        [RelationshipRole].[CreatedOn],
    [RelationshipRole].[CreatedOnBehalfBy],
    --[RelationshipRole].[CreatedOnBehalfByDsc]
    0,
    [RelationshipRole].[CreatedOnBehalfByName],
    [RelationshipRole].[CreatedOnBehalfByYomiName],
    [RelationshipRole].[Description],
    [RelationshipRole].[ImportSequenceNumber],
    [RelationshipRole].[ModifiedBy],
    --[RelationshipRole].[ModifiedByDsc]
    0,
    [RelationshipRole].[ModifiedByName],
    [RelationshipRole].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RelationshipRole].[ModifiedOn], 
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
        [RelationshipRole].[ModifiedOn],
    [RelationshipRole].[ModifiedOnBehalfBy],
    --[RelationshipRole].[ModifiedOnBehalfByDsc]
    0,
    [RelationshipRole].[ModifiedOnBehalfByName],
    [RelationshipRole].[ModifiedOnBehalfByYomiName],
    [RelationshipRole].[Name],
    [RelationshipRole].[OrganizationId],
    --[RelationshipRole].[OrganizationIdDsc]
    0,
    [RelationshipRole].[OrganizationIdName],
    [RelationshipRole].[RelationshipRoleId],
    [RelationshipRole].[StateCode],
    StateCodePLTable.Value,
    [RelationshipRole].[StatusCode],
    StatusCodePLTable.Value,
    [RelationshipRole].[VersionNumber]
from RelationshipRole
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4500
		and [StateCodePLTable].AttributeValue = [RelationshipRole].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4500
		and [StatusCodePLTable].AttributeValue = [RelationshipRole].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4500) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RelationshipRole].OrganizationId = u.OrganizationId
)

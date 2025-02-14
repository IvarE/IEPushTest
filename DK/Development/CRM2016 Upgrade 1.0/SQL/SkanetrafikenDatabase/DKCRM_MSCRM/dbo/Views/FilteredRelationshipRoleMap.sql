﻿

--
-- report view for relationshiprolemap
--
create view dbo.[FilteredRelationshipRoleMap] (
    [associateobjecttypecode],
    [associateobjecttypecodename],
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
    [primaryobjecttypecode],
    [primaryobjecttypecodename],
    [relationshiproleid],
    [relationshiproleiddsc],
    [relationshiproleidname],
    [relationshiprolemapid],
    [versionnumber]
) with view_metadata as
select
    [RelationshipRoleMap].[AssociateObjectTypeCode],
    AssociateObjectTypeCodePLTable.Value,
    [RelationshipRoleMap].[CreatedBy],
    --[RelationshipRoleMap].[CreatedByDsc]
    0,
    [RelationshipRoleMap].[CreatedByName],
    [RelationshipRoleMap].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RelationshipRoleMap].[CreatedOn], 
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
        [RelationshipRoleMap].[CreatedOn],
    [RelationshipRoleMap].[CreatedOnBehalfBy],
    --[RelationshipRoleMap].[CreatedOnBehalfByDsc]
    0,
    [RelationshipRoleMap].[CreatedOnBehalfByName],
    [RelationshipRoleMap].[CreatedOnBehalfByYomiName],
    [RelationshipRoleMap].[ModifiedBy],
    --[RelationshipRoleMap].[ModifiedByDsc]
    0,
    [RelationshipRoleMap].[ModifiedByName],
    [RelationshipRoleMap].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RelationshipRoleMap].[ModifiedOn], 
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
        [RelationshipRoleMap].[ModifiedOn],
    [RelationshipRoleMap].[ModifiedOnBehalfBy],
    --[RelationshipRoleMap].[ModifiedOnBehalfByDsc]
    0,
    [RelationshipRoleMap].[ModifiedOnBehalfByName],
    [RelationshipRoleMap].[ModifiedOnBehalfByYomiName],
    [RelationshipRoleMap].[OrganizationId],
    [RelationshipRoleMap].[PrimaryObjectTypeCode],
    PrimaryObjectTypeCodePLTable.Value,
    [RelationshipRoleMap].[RelationshipRoleId],
    --[RelationshipRoleMap].[RelationshipRoleIdDsc]
    0,
    [RelationshipRoleMap].[RelationshipRoleIdName],
    [RelationshipRoleMap].[RelationshipRoleMapId],
    [RelationshipRoleMap].[VersionNumber]
from RelationshipRoleMap
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [AssociateObjectTypeCodePLTable] on 
		([AssociateObjectTypeCodePLTable].AttributeName = 'associateobjecttypecode'
		and [AssociateObjectTypeCodePLTable].ObjectTypeCode = 4501
		and [AssociateObjectTypeCodePLTable].AttributeValue = [RelationshipRoleMap].[AssociateObjectTypeCode]
		and [AssociateObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PrimaryObjectTypeCodePLTable] on 
		([PrimaryObjectTypeCodePLTable].AttributeName = 'primaryobjecttypecode'
		and [PrimaryObjectTypeCodePLTable].ObjectTypeCode = 4501
		and [PrimaryObjectTypeCodePLTable].AttributeValue = [RelationshipRoleMap].[PrimaryObjectTypeCode]
		and [PrimaryObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4500) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [RelationshipRoleMap].OrganizationId = u.OrganizationId
)

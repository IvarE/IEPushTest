﻿

--
-- report view for customerrelationship
--
create view dbo.[FilteredCustomerRelationship] (
    [converserelationshipid],
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
    [customerid],
    [customeriddsc],
    [customeridname],
    [customeridtype],
    [customeridyominame],
    [customerrelationshipid],
    [customerroledescription],
    [customerroleid],
    [customerroleiddsc],
    [customerroleidname],
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
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [partnerid],
    [partneriddsc],
    [partneridname],
    [partneridtype],
    [partneridyominame],
    [partnerroledescription],
    [partnerroleid],
    [partnerroleiddsc],
    [partnerroleidname],
    [versionnumber]
) with view_metadata as
select
    [CustomerRelationship].[ConverseRelationshipId],
    [CustomerRelationship].[CreatedBy],
    --[CustomerRelationship].[CreatedByDsc]
    0,
    [CustomerRelationship].[CreatedByName],
    [CustomerRelationship].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerRelationship].[CreatedOn], 
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
        [CustomerRelationship].[CreatedOn],
    [CustomerRelationship].[CreatedOnBehalfBy],
    --[CustomerRelationship].[CreatedOnBehalfByDsc]
    0,
    [CustomerRelationship].[CreatedOnBehalfByName],
    [CustomerRelationship].[CreatedOnBehalfByYomiName],
    [CustomerRelationship].[CustomerId],
    --[CustomerRelationship].[CustomerIdDsc]
    0,
    [CustomerRelationship].[CustomerIdName],
    [CustomerRelationship].[CustomerIdType],
    [CustomerRelationship].[CustomerIdYomiName],
    [CustomerRelationship].[CustomerRelationshipId],
    [CustomerRelationship].[CustomerRoleDescription],
    [CustomerRelationship].[CustomerRoleId],
    --[CustomerRelationship].[CustomerRoleIdDsc]
    0,
    [CustomerRelationship].[CustomerRoleIdName],
    [CustomerRelationship].[ImportSequenceNumber],
    [CustomerRelationship].[ModifiedBy],
    --[CustomerRelationship].[ModifiedByDsc]
    0,
    [CustomerRelationship].[ModifiedByName],
    [CustomerRelationship].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerRelationship].[ModifiedOn], 
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
        [CustomerRelationship].[ModifiedOn],
    [CustomerRelationship].[ModifiedOnBehalfBy],
    --[CustomerRelationship].[ModifiedOnBehalfByDsc]
    0,
    [CustomerRelationship].[ModifiedOnBehalfByName],
    [CustomerRelationship].[ModifiedOnBehalfByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerRelationship].[OverriddenCreatedOn], 
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
        [CustomerRelationship].[OverriddenCreatedOn],
    [CustomerRelationship].[OwnerId],
    --[CustomerRelationship].[OwnerIdDsc]
    0,
    [CustomerRelationship].[OwnerIdName],
    [CustomerRelationship].[OwnerIdType],
    [CustomerRelationship].[OwnerIdYomiName],
    [CustomerRelationship].[OwningBusinessUnit],
    [CustomerRelationship].[OwningTeam],
    [CustomerRelationship].[OwningUser],
    [CustomerRelationship].[PartnerId],
    --[CustomerRelationship].[PartnerIdDsc]
    0,
    [CustomerRelationship].[PartnerIdName],
    [CustomerRelationship].[PartnerIdType],
    [CustomerRelationship].[PartnerIdYomiName],
    [CustomerRelationship].[PartnerRoleDescription],
    [CustomerRelationship].[PartnerRoleId],
    --[CustomerRelationship].[PartnerRoleIdDsc]
    0,
    [CustomerRelationship].[PartnerRoleIdName],
    [CustomerRelationship].[VersionNumber]
from CustomerRelationship
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4502) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[CustomerRelationship].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4502
	)	

		
	-- role based access
	or 
	
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[CustomerRelationship].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4502)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[CustomerRelationship].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[CustomerRelationship].[CustomerRelationshipId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4502 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

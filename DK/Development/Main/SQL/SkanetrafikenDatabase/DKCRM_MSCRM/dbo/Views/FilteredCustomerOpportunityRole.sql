

--
-- report view for customeropportunityrole
--
create view dbo.[FilteredCustomerOpportunityRole] (
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
    [customeropportunityroleid],
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
    [opportunityid],
    [opportunityiddsc],
    [opportunityidname],
    [opportunityroleid],
    [opportunityroleiddsc],
    [opportunityroleidname],
    [opportunitystatecode],
    [opportunitystatuscode],
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
    [versionnumber]
) with view_metadata as
select
    [CustomerOpportunityRole].[CreatedBy],
    --[CustomerOpportunityRole].[CreatedByDsc]
    0,
    [CustomerOpportunityRole].[CreatedByName],
    [CustomerOpportunityRole].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerOpportunityRole].[CreatedOn], 
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
        [CustomerOpportunityRole].[CreatedOn],
    [CustomerOpportunityRole].[CreatedOnBehalfBy],
    --[CustomerOpportunityRole].[CreatedOnBehalfByDsc]
    0,
    [CustomerOpportunityRole].[CreatedOnBehalfByName],
    [CustomerOpportunityRole].[CreatedOnBehalfByYomiName],
    [CustomerOpportunityRole].[CustomerId],
    --[CustomerOpportunityRole].[CustomerIdDsc]
    0,
    [CustomerOpportunityRole].[CustomerIdName],
    [CustomerOpportunityRole].[CustomerIdType],
    [CustomerOpportunityRole].[CustomerIdYomiName],
    [CustomerOpportunityRole].[CustomerOpportunityRoleId],
    [CustomerOpportunityRole].[Description],
    [CustomerOpportunityRole].[ImportSequenceNumber],
    [CustomerOpportunityRole].[ModifiedBy],
    --[CustomerOpportunityRole].[ModifiedByDsc]
    0,
    [CustomerOpportunityRole].[ModifiedByName],
    [CustomerOpportunityRole].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerOpportunityRole].[ModifiedOn], 
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
        [CustomerOpportunityRole].[ModifiedOn],
    [CustomerOpportunityRole].[ModifiedOnBehalfBy],
    --[CustomerOpportunityRole].[ModifiedOnBehalfByDsc]
    0,
    [CustomerOpportunityRole].[ModifiedOnBehalfByName],
    [CustomerOpportunityRole].[ModifiedOnBehalfByYomiName],
    [CustomerOpportunityRole].[OpportunityId],
    --[CustomerOpportunityRole].[OpportunityIdDsc]
    0,
    [CustomerOpportunityRole].[OpportunityIdName],
    [CustomerOpportunityRole].[OpportunityRoleId],
    --[CustomerOpportunityRole].[OpportunityRoleIdDsc]
    0,
    [CustomerOpportunityRole].[OpportunityRoleIdName],
    [CustomerOpportunityRole].[OpportunityStateCode],
    [CustomerOpportunityRole].[OpportunityStatusCode],
    dbo.fn_UTCToTzSpecificLocalTime([CustomerOpportunityRole].[OverriddenCreatedOn], 
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
        [CustomerOpportunityRole].[OverriddenCreatedOn],
    [CustomerOpportunityRole].[OwnerId],
    --[CustomerOpportunityRole].[OwnerIdDsc]
    0,
    [CustomerOpportunityRole].[OwnerIdName],
    [CustomerOpportunityRole].[OwnerIdType],
    [CustomerOpportunityRole].[OwnerIdYomiName],
    [CustomerOpportunityRole].[OwningBusinessUnit],
    [CustomerOpportunityRole].[OwningTeam],
    [CustomerOpportunityRole].[OwningUser],
    [CustomerOpportunityRole].[VersionNumber]
from CustomerOpportunityRole
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4503) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[CustomerOpportunityRole].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4503
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
		[CustomerOpportunityRole].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4503)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[CustomerOpportunityRole].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[CustomerOpportunityRole].[CustomerOpportunityRoleId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4503 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

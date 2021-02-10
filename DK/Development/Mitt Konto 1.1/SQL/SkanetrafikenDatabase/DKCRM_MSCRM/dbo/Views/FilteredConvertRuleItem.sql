

--
-- report view for convertruleitem
--
create view dbo.[FilteredConvertRuleItem] (
    [componentstate],
    [conditionid],
    [conditionxml],
    [convertruleid],
    [convertruleidname],
    [convertruleitemid],
    [convertruleitemidunique],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [exchangerate],
    [ismanaged],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [overwritetime],
    [overwritetimeutc],
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [owninguser],
    [propertiesxml],
    [queueid],
    [queueidname],
    [sequencenumber],
    [solutionid],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [versionnumber]
) with view_metadata as
select
    [ConvertRuleItem].[ComponentState],
    [ConvertRuleItem].[ConditionId],
    [ConvertRuleItem].[ConditionXml],
    [ConvertRuleItem].[ConvertRuleId],
    [ConvertRuleItem].[ConvertRuleIdName],
    [ConvertRuleItem].[ConvertRuleItemId],
    [ConvertRuleItem].[ConvertRuleItemIdUnique],
    [ConvertRuleItem].[CreatedBy],
    [ConvertRuleItem].[CreatedByName],
    [ConvertRuleItem].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ConvertRuleItem].[CreatedOn], 
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
        [ConvertRuleItem].[CreatedOn],
    [ConvertRuleItem].[CreatedOnBehalfBy],
    [ConvertRuleItem].[CreatedOnBehalfByName],
    [ConvertRuleItem].[CreatedOnBehalfByYomiName],
    [ConvertRuleItem].[Description],
    [ConvertRuleItem].[ExchangeRate],
    [ConvertRuleItem].[IsManaged],
    [ConvertRuleItem].[ModifiedBy],
    [ConvertRuleItem].[ModifiedByName],
    [ConvertRuleItem].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([ConvertRuleItem].[ModifiedOn], 
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
        [ConvertRuleItem].[ModifiedOn],
    [ConvertRuleItem].[ModifiedOnBehalfBy],
    [ConvertRuleItem].[ModifiedOnBehalfByName],
    [ConvertRuleItem].[ModifiedOnBehalfByYomiName],
    [ConvertRuleItem].[Name],
    dbo.fn_UTCToTzSpecificLocalTime([ConvertRuleItem].[OverwriteTime], 
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
        [ConvertRuleItem].[OverwriteTime],
    [ConvertRuleItem].[OwnerId],
    [ConvertRuleItem].[OwnerIdType],
    [ConvertRuleItem].[OwningBusinessUnit],
    [ConvertRuleItem].[OwningUser],
    [ConvertRuleItem].[PropertiesXml],
    [ConvertRuleItem].[QueueId],
    [ConvertRuleItem].[QueueIdName],
    [ConvertRuleItem].[SequenceNumber],
    [ConvertRuleItem].[SolutionId],
    [ConvertRuleItem].[TransactionCurrencyId],
    [ConvertRuleItem].[TransactionCurrencyIdName],
    [ConvertRuleItem].[VersionNumber]
from ConvertRuleItem
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9300) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[ConvertRuleItem].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 9300
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
		[ConvertRuleItem].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9300)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ConvertRuleItem].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ConvertRuleItem].[ConvertRuleId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 9300 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

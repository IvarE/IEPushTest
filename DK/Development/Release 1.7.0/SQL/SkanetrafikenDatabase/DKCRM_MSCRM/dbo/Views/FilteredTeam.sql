

--
-- report view for team
--
create view dbo.[FilteredTeam] (
    [administratorid],
    [administratoridname],
    [administratoridyominame],
    [businessunitid],
    [businessunitiddsc],
    [businessunitidname],
    [cgi_printdetails],
    [cgi_teamcontactdetails],
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
    [emailaddress],
    [exchangerate],
    [importsequencenumber],
    [isdefault],
    [isdefaultname],
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
    [queueid],
    [queueidname],
    [regardingobjectid],
    [regardingobjecttypecode],
    [stageid],
    [systemmanaged],
    [systemmanagedname],
    [teamid],
    [teamtemplateid],
    [teamtype],
    [teamtypename],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [versionnumber],
    [yominame]
) with view_metadata as
select
    [Team].[AdministratorId],
    [Team].[AdministratorIdName],
    [Team].[AdministratorIdYomiName],
    [Team].[BusinessUnitId],
    --[Team].[BusinessUnitIdDsc]
    0,
    [Team].[BusinessUnitIdName],
    [Team].[cgi_PrintDetails],
    [Team].[cgi_TeamContactDetails],
    [Team].[CreatedBy],
    --[Team].[CreatedByDsc]
    0,
    [Team].[CreatedByName],
    [Team].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Team].[CreatedOn], 
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
        [Team].[CreatedOn],
    [Team].[CreatedOnBehalfBy],
    --[Team].[CreatedOnBehalfByDsc]
    0,
    [Team].[CreatedOnBehalfByName],
    [Team].[CreatedOnBehalfByYomiName],
    [Team].[Description],
    [Team].[EMailAddress],
    [Team].[ExchangeRate],
    [Team].[ImportSequenceNumber],
    [Team].[IsDefault],
    IsDefaultPLTable.Value,
    [Team].[ModifiedBy],
    --[Team].[ModifiedByDsc]
    0,
    [Team].[ModifiedByName],
    [Team].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Team].[ModifiedOn], 
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
        [Team].[ModifiedOn],
    [Team].[ModifiedOnBehalfBy],
    --[Team].[ModifiedOnBehalfByDsc]
    0,
    [Team].[ModifiedOnBehalfByName],
    [Team].[ModifiedOnBehalfByYomiName],
    [Team].[Name],
    [Team].[OrganizationId],
    --[Team].[OrganizationIdDsc]
    0,
    [Team].[OrganizationIdName],
    dbo.fn_UTCToTzSpecificLocalTime([Team].[OverriddenCreatedOn], 
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
        [Team].[OverriddenCreatedOn],
    [Team].[ProcessId],
    [Team].[QueueId],
    [Team].[QueueIdName],
    [Team].[RegardingObjectId],
    [Team].[RegardingObjectTypeCode],
    [Team].[StageId],
    [Team].[SystemManaged],
    SystemManagedPLTable.Value,
    [Team].[TeamId],
    [Team].[TeamTemplateId],
    [Team].[TeamType],
    TeamTypePLTable.Value,
    [Team].[TransactionCurrencyId],
    [Team].[TransactionCurrencyIdName],
    [Team].[VersionNumber],
    [Team].[YomiName]
from Team
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [IsDefaultPLTable] on 
		([IsDefaultPLTable].AttributeName = 'isdefault'
		and [IsDefaultPLTable].ObjectTypeCode = 9
		and [IsDefaultPLTable].AttributeValue = [Team].[IsDefault]
		and [IsDefaultPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SystemManagedPLTable] on 
		([SystemManagedPLTable].AttributeName = 'systemmanaged'
		and [SystemManagedPLTable].ObjectTypeCode = 9
		and [SystemManagedPLTable].AttributeValue = [Team].[SystemManaged]
		and [SystemManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TeamTypePLTable] on 
		([TeamTypePLTable].AttributeName = 'teamtype'
		and [TeamTypePLTable].ObjectTypeCode = 9
		and [TeamTypePLTable].AttributeValue = [Team].[TeamType]
		and [TeamTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(9) pdm
where
(
    
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[Team].[BusinessUnitId] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 9)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Team].[BusinessUnitId] is not null 
	) 
)

)



--
-- report view for serviceendpoint
--
create view dbo.[FilteredServiceEndpoint] (
    [componentstate],
    [connectionmode],
    [connectionmodename],
    [contract],
    [contractname],
    [createdby],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [description],
    [introducedversion],
    [iscustomizable],
    [ismanaged],
    [ismanagedname],
    [modifiedby],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [organizationid],
    [overwritetime],
    [overwritetimeutc],
    [path],
    [serviceendpointid],
    [serviceendpointidunique],
    [solutionid],
    [solutionnamespace],
    [userclaim],
    [userclaimname]
) with view_metadata as
select
    [ServiceEndpoint].[ComponentState],
    [ServiceEndpoint].[ConnectionMode],
    ConnectionModePLTable.Value,
    [ServiceEndpoint].[Contract],
    ContractPLTable.Value,
    [ServiceEndpoint].[CreatedBy],
    dbo.fn_UTCToTzSpecificLocalTime([ServiceEndpoint].[CreatedOn], 
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
        [ServiceEndpoint].[CreatedOn],
    [ServiceEndpoint].[CreatedOnBehalfBy],
    --[ServiceEndpoint].[CreatedOnBehalfByDsc]
    0,
    [ServiceEndpoint].[CreatedOnBehalfByName],
    [ServiceEndpoint].[CreatedOnBehalfByYomiName],
    [ServiceEndpoint].[Description],
    [ServiceEndpoint].[IntroducedVersion],
    [ServiceEndpoint].[IsCustomizable],
    [ServiceEndpoint].[IsManaged],
    IsManagedPLTable.Value,
    [ServiceEndpoint].[ModifiedBy],
    dbo.fn_UTCToTzSpecificLocalTime([ServiceEndpoint].[ModifiedOn], 
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
        [ServiceEndpoint].[ModifiedOn],
    [ServiceEndpoint].[ModifiedOnBehalfBy],
    --[ServiceEndpoint].[ModifiedOnBehalfByDsc]
    0,
    [ServiceEndpoint].[ModifiedOnBehalfByName],
    [ServiceEndpoint].[ModifiedOnBehalfByYomiName],
    [ServiceEndpoint].[Name],
    [ServiceEndpoint].[OrganizationId],
    dbo.fn_UTCToTzSpecificLocalTime([ServiceEndpoint].[OverwriteTime], 
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
        [ServiceEndpoint].[OverwriteTime],
    [ServiceEndpoint].[Path],
    [ServiceEndpoint].[ServiceEndpointId],
    [ServiceEndpoint].[ServiceEndpointIdUnique],
    [ServiceEndpoint].[SolutionId],
    [ServiceEndpoint].[SolutionNamespace],
    [ServiceEndpoint].[UserClaim],
    UserClaimPLTable.Value
from ServiceEndpoint
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ConnectionModePLTable] on 
		([ConnectionModePLTable].AttributeName = 'connectionmode'
		and [ConnectionModePLTable].ObjectTypeCode = 4618
		and [ConnectionModePLTable].AttributeValue = [ServiceEndpoint].[ConnectionMode]
		and [ConnectionModePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ContractPLTable] on 
		([ContractPLTable].AttributeName = 'contract'
		and [ContractPLTable].ObjectTypeCode = 4618
		and [ContractPLTable].AttributeValue = [ServiceEndpoint].[Contract]
		and [ContractPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 4618
		and [IsManagedPLTable].AttributeValue = [ServiceEndpoint].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [UserClaimPLTable] on 
		([UserClaimPLTable].AttributeName = 'userclaim'
		and [UserClaimPLTable].ObjectTypeCode = 4618
		and [UserClaimPLTable].AttributeValue = [ServiceEndpoint].[UserClaim]
		and [UserClaimPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4618) pdm
where
(
    -- privilege check
    pdm.PrivilegeDepthMask is not null and
    [ServiceEndpoint].OrganizationId = u.OrganizationId
)

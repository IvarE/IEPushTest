﻿

--
-- report view for workflow
--
create view dbo.[FilteredWorkflow] (
    [activeworkflowid],
    [activeworkflowiddsc],
    [activeworkflowidname],
    [activities],
    [asyncautodelete],
    [asyncautodeletename],
    [category],
    [categoryname],
    [clientdata],
    [componentstate],
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
    [createstage],
    [createstagename],
    [deletestage],
    [deletestagename],
    [description],
    [inputparameters],
    [introducedversion],
    [iscrmuiworkflow],
    [iscustomizable],
    [ismanaged],
    [ismanagedname],
    [istransacted],
    [istransactedname],
    [languagecode],
    [mode],
    [modename],
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
    [ondemand],
    [ondemandname],
    [overwritetime],
    [overwritetimeutc],
    [ownerid],
    [owneriddsc],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningbusinessunitdsc],
    [owningbusinessunitname],
    [owningteam],
    [owninguser],
    [parentworkflowid],
    [parentworkflowiddsc],
    [parentworkflowidname],
    [plugintypeid],
    [primaryentity],
    [primaryentityname],
    [processorder],
    [processroleassignment],
    [rank],
    [rendererobjecttypecode],
    [rules],
    [runas],
    [runasname],
    [scope],
    [scopename],
    [sdkmessageid],
    [solutionid],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subprocess],
    [subprocessname],
    [syncworkflowlogonfailure],
    [syncworkflowlogonfailurename],
    [triggeroncreate],
    [triggeroncreatename],
    [triggerondelete],
    [triggerondeletename],
    [triggeronupdateattributelist],
    [type],
    [typename],
    [uidata],
    [uniquename],
    [updatestage],
    [updatestagename],
    [versionnumber],
    [workflowid],
    [workflowidunique],
    [xaml]
) with view_metadata as
select
    [Workflow].[ActiveWorkflowId],
    --[Workflow].[ActiveWorkflowIdDsc]
    0,
    [Workflow].[ActiveWorkflowIdName],
    [Workflow].[Activities],
    [Workflow].[AsyncAutoDelete],
    AsyncAutoDeletePLTable.Value,
    [Workflow].[Category],
    CategoryPLTable.Value,
    [Workflow].[ClientData],
    [Workflow].[ComponentState],
    [Workflow].[CreatedBy],
    --[Workflow].[CreatedByDsc]
    0,
    [Workflow].[CreatedByName],
    [Workflow].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Workflow].[CreatedOn], 
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
        [Workflow].[CreatedOn],
    [Workflow].[CreatedOnBehalfBy],
    --[Workflow].[CreatedOnBehalfByDsc]
    0,
    [Workflow].[CreatedOnBehalfByName],
    [Workflow].[CreatedOnBehalfByYomiName],
    [Workflow].[CreateStage],
    CreateStagePLTable.Value,
    [Workflow].[DeleteStage],
    DeleteStagePLTable.Value,
    coalesce(dbo.fn_GetLocalizedLabel([Workflow].[WorkflowId], 'description', 19, us.UILanguageId), [Workflow].[Description]),
    [Workflow].[InputParameters],
    [Workflow].[IntroducedVersion],
    [Workflow].[IsCrmUIWorkflow],
    [Workflow].[IsCustomizable],
    [Workflow].[IsManaged],
    IsManagedPLTable.Value,
    [Workflow].[IsTransacted],
    IsTransactedPLTable.Value,
    [Workflow].[LanguageCode],
    [Workflow].[Mode],
    ModePLTable.Value,
    [Workflow].[ModifiedBy],
    --[Workflow].[ModifiedByDsc]
    0,
    [Workflow].[ModifiedByName],
    [Workflow].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([Workflow].[ModifiedOn], 
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
        [Workflow].[ModifiedOn],
    [Workflow].[ModifiedOnBehalfBy],
    --[Workflow].[ModifiedOnBehalfByDsc]
    0,
    [Workflow].[ModifiedOnBehalfByName],
    [Workflow].[ModifiedOnBehalfByYomiName],
    coalesce(dbo.fn_GetLocalizedLabel([Workflow].[WorkflowId], 'name', 19, us.UILanguageId), [Workflow].[Name]),
    [Workflow].[OnDemand],
    OnDemandPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([Workflow].[OverwriteTime], 
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
        [Workflow].[OverwriteTime],
    [Workflow].[OwnerId],
    --[Workflow].[OwnerIdDsc]
    0,
    [Workflow].[OwnerIdName],
    [Workflow].[OwnerIdType],
    [Workflow].[OwnerIdYomiName],
    [Workflow].[OwningBusinessUnit],
    --[Workflow].[OwningBusinessUnitDsc]
    0,
    [Workflow].[OwningBusinessUnitName],
    [Workflow].[OwningTeam],
    [Workflow].[OwningUser],
    [Workflow].[ParentWorkflowId],
    --[Workflow].[ParentWorkflowIdDsc]
    0,
    [Workflow].[ParentWorkflowIdName],
    [Workflow].[PluginTypeId],
    [Workflow].[PrimaryEntity],
    PrimaryEntityPLTable.Value,
    [Workflow].[ProcessOrder],
    [Workflow].[ProcessRoleAssignment],
    [Workflow].[Rank],
    [Workflow].[RendererObjectTypeCode],
    [Workflow].[Rules],
    [Workflow].[RunAs],
    RunAsPLTable.Value,
    [Workflow].[Scope],
    ScopePLTable.Value,
    [Workflow].[SdkMessageId],
    [Workflow].[SolutionId],
    [Workflow].[StateCode],
    StateCodePLTable.Value,
    [Workflow].[StatusCode],
    StatusCodePLTable.Value,
    [Workflow].[Subprocess],
    SubprocessPLTable.Value,
    [Workflow].[SyncWorkflowLogOnFailure],
    SyncWorkflowLogOnFailurePLTable.Value,
    [Workflow].[TriggerOnCreate],
    TriggerOnCreatePLTable.Value,
    [Workflow].[TriggerOnDelete],
    TriggerOnDeletePLTable.Value,
    [Workflow].[TriggerOnUpdateAttributeList],
    [Workflow].[Type],
    TypePLTable.Value,
    [Workflow].[UIData],
    [Workflow].[UniqueName],
    [Workflow].[UpdateStage],
    UpdateStagePLTable.Value,
    [Workflow].[VersionNumber],
    [Workflow].[WorkflowId],
    [Workflow].[WorkflowIdUnique],
    [Workflow].[Xaml]
from Workflow
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [AsyncAutoDeletePLTable] on 
		([AsyncAutoDeletePLTable].AttributeName = 'asyncautodelete'
		and [AsyncAutoDeletePLTable].ObjectTypeCode = 4703
		and [AsyncAutoDeletePLTable].AttributeValue = [Workflow].[AsyncAutoDelete]
		and [AsyncAutoDeletePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CategoryPLTable] on 
		([CategoryPLTable].AttributeName = 'category'
		and [CategoryPLTable].ObjectTypeCode = 4703
		and [CategoryPLTable].AttributeValue = [Workflow].[Category]
		and [CategoryPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [CreateStagePLTable] on 
		([CreateStagePLTable].AttributeName = 'createstage'
		and [CreateStagePLTable].ObjectTypeCode = 4703
		and [CreateStagePLTable].AttributeValue = [Workflow].[CreateStage]
		and [CreateStagePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [DeleteStagePLTable] on 
		([DeleteStagePLTable].AttributeName = 'deletestage'
		and [DeleteStagePLTable].ObjectTypeCode = 4703
		and [DeleteStagePLTable].AttributeValue = [Workflow].[DeleteStage]
		and [DeleteStagePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsManagedPLTable] on 
		([IsManagedPLTable].AttributeName = 'ismanaged'
		and [IsManagedPLTable].ObjectTypeCode = 4703
		and [IsManagedPLTable].AttributeValue = [Workflow].[IsManaged]
		and [IsManagedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsTransactedPLTable] on 
		([IsTransactedPLTable].AttributeName = 'istransacted'
		and [IsTransactedPLTable].ObjectTypeCode = 4703
		and [IsTransactedPLTable].AttributeValue = [Workflow].[IsTransacted]
		and [IsTransactedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ModePLTable] on 
		([ModePLTable].AttributeName = 'mode'
		and [ModePLTable].ObjectTypeCode = 4703
		and [ModePLTable].AttributeValue = [Workflow].[Mode]
		and [ModePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [OnDemandPLTable] on 
		([OnDemandPLTable].AttributeName = 'ondemand'
		and [OnDemandPLTable].ObjectTypeCode = 4703
		and [OnDemandPLTable].AttributeValue = [Workflow].[OnDemand]
		and [OnDemandPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PrimaryEntityPLTable] on 
		([PrimaryEntityPLTable].AttributeName = 'primaryentity'
		and [PrimaryEntityPLTable].ObjectTypeCode = 4703
		and [PrimaryEntityPLTable].AttributeValue = [Workflow].[PrimaryEntity]
		and [PrimaryEntityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [RunAsPLTable] on 
		([RunAsPLTable].AttributeName = 'runas'
		and [RunAsPLTable].ObjectTypeCode = 4703
		and [RunAsPLTable].AttributeValue = [Workflow].[RunAs]
		and [RunAsPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ScopePLTable] on 
		([ScopePLTable].AttributeName = 'scope'
		and [ScopePLTable].ObjectTypeCode = 4703
		and [ScopePLTable].AttributeValue = [Workflow].[Scope]
		and [ScopePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4703
		and [StateCodePLTable].AttributeValue = [Workflow].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4703
		and [StatusCodePLTable].AttributeValue = [Workflow].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SubprocessPLTable] on 
		([SubprocessPLTable].AttributeName = 'subprocess'
		and [SubprocessPLTable].ObjectTypeCode = 4703
		and [SubprocessPLTable].AttributeValue = [Workflow].[Subprocess]
		and [SubprocessPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SyncWorkflowLogOnFailurePLTable] on 
		([SyncWorkflowLogOnFailurePLTable].AttributeName = 'syncworkflowlogonfailure'
		and [SyncWorkflowLogOnFailurePLTable].ObjectTypeCode = 4703
		and [SyncWorkflowLogOnFailurePLTable].AttributeValue = [Workflow].[SyncWorkflowLogOnFailure]
		and [SyncWorkflowLogOnFailurePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TriggerOnCreatePLTable] on 
		([TriggerOnCreatePLTable].AttributeName = 'triggeroncreate'
		and [TriggerOnCreatePLTable].ObjectTypeCode = 4703
		and [TriggerOnCreatePLTable].AttributeValue = [Workflow].[TriggerOnCreate]
		and [TriggerOnCreatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TriggerOnDeletePLTable] on 
		([TriggerOnDeletePLTable].AttributeName = 'triggerondelete'
		and [TriggerOnDeletePLTable].ObjectTypeCode = 4703
		and [TriggerOnDeletePLTable].AttributeValue = [Workflow].[TriggerOnDelete]
		and [TriggerOnDeletePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [TypePLTable] on 
		([TypePLTable].AttributeName = 'type'
		and [TypePLTable].ObjectTypeCode = 4703
		and [TypePLTable].AttributeValue = [Workflow].[Type]
		and [TypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [UpdateStagePLTable] on 
		([UpdateStagePLTable].AttributeName = 'updatestage'
		and [UpdateStagePLTable].ObjectTypeCode = 4703
		and [UpdateStagePLTable].AttributeValue = [Workflow].[UpdateStage]
		and [UpdateStagePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4703) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[Workflow].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4703
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
		[Workflow].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4703)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[Workflow].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[Workflow].[WorkflowId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4703 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

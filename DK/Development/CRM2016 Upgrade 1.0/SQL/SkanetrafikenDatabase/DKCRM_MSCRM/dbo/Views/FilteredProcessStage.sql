﻿

--
-- report view for processstage
--
create view dbo.[FilteredProcessStage] (
    [ownerid],
    [owneridtype],
    [owningbusinessunit],
    [primaryentitytypecode],
    [primaryentitytypecodename],
    [processid],
    [processidname],
    [processstageid],
    [stagecategory],
    [stagecategoryname],
    [stagename],
    [versionnumber]
) with view_metadata as
select
    [ProcessStage].[OwnerId],
    [ProcessStage].[OwnerIdType],
    [ProcessStage].[OwningBusinessUnit],
    [ProcessStage].[PrimaryEntityTypeCode],
    PrimaryEntityTypeCodePLTable.Value,
    [ProcessStage].[ProcessId],
    [ProcessStage].[ProcessIdName],
    [ProcessStage].[ProcessStageId],
    [ProcessStage].[StageCategory],
    StageCategoryPLTable.Value,
    [ProcessStage].[StageName],
    [ProcessStage].[VersionNumber]
from ProcessStage
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [PrimaryEntityTypeCodePLTable] on 
		([PrimaryEntityTypeCodePLTable].AttributeName = 'primaryentitytypecode'
		and [PrimaryEntityTypeCodePLTable].ObjectTypeCode = 4724
		and [PrimaryEntityTypeCodePLTable].AttributeValue = [ProcessStage].[PrimaryEntityTypeCode]
		and [PrimaryEntityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StageCategoryPLTable] on 
		([StageCategoryPLTable].AttributeName = 'stagecategory'
		and [StageCategoryPLTable].ObjectTypeCode = 4724
		and [StageCategoryPLTable].AttributeValue = [ProcessStage].[StageCategory]
		and [StageCategoryPLTable].LangId = 
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
	[ProcessStage].OwnerId in 
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
		[ProcessStage].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4703)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[ProcessStage].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[ProcessStage].[ProcessId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4703 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)

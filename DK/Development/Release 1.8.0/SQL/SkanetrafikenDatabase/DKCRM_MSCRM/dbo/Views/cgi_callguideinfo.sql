


--
-- base view for cgi_callguideinfo
--
create view dbo.[cgi_callguideinfo]
 (
    -- logical attributes
    [CreatedByName],
    [CreatedByYomiName],
    [CreatedOnBehalfByName],
    [CreatedOnBehalfByYomiName],
    [ModifiedByName],
    [ModifiedByYomiName],
    [ModifiedOnBehalfByName],
    [ModifiedOnBehalfByYomiName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_callguideinfoId],
    [CreatedOn],
    [CreatedBy],
    [ModifiedOn],
    [ModifiedBy],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [OwningBusinessUnit],
    [statecode],
    [statuscode],
    [VersionNumber],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [TimeZoneRuleVersionNumber],
    [UTCConversionTimeZoneCode],
    [cgi_callguideinfoname],
    [cgi_AgentName],
    [cgi_APhoneNumber],
    [cgi_BPhoneNumber],
    [cgi_CallguideSessionID],
    [cgi_Cid],
    [cgi_ContactSourceType],
    [cgi_Duration],
    [cgi_ErrandTaskType],
    [cgi_QueueTime],
    [cgi_ScreenPopChoice],
    [cgi_chatcustomeralias]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_callguideinfo_createdby].[FullName],
    [lk_cgi_callguideinfo_createdby].[YomiFullName],
    [lk_cgi_callguideinfo_createdonbehalfby].[FullName],
    [lk_cgi_callguideinfo_createdonbehalfby].[YomiFullName],
    [lk_cgi_callguideinfo_modifiedby].[FullName],
    [lk_cgi_callguideinfo_modifiedby].[YomiFullName],
    [lk_cgi_callguideinfo_modifiedonbehalfby].[FullName],
    [lk_cgi_callguideinfo_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_callguideinfoBase].OwnerId,
    OwnerName = XXowner.Name,
    OwnerYomiName =  XXowner.YomiName,
    OwnerDsc = 0, -- DSC is removed, stub it to 0
    OwnerIdType = XXowner.OwnerIdType,
    OwningUser = case 
 		when XXowner.OwnerIdType= 8 then XXowner.OwnerId
		else null
		end,
    OwningTeam = case 
 		when XXowner.OwnerIdType= 9 then XXowner.OwnerId
		else null
		end,

    -- physical attribute
    [cgi_callguideinfoBase].[cgi_callguideinfoId],
    [cgi_callguideinfoBase].[CreatedOn],
    [cgi_callguideinfoBase].[CreatedBy],
    [cgi_callguideinfoBase].[ModifiedOn],
    [cgi_callguideinfoBase].[ModifiedBy],
    [cgi_callguideinfoBase].[CreatedOnBehalfBy],
    [cgi_callguideinfoBase].[ModifiedOnBehalfBy],
    [cgi_callguideinfoBase].[OwningBusinessUnit],
    [cgi_callguideinfoBase].[statecode],
    [cgi_callguideinfoBase].[statuscode],
    [cgi_callguideinfoBase].[VersionNumber],
    [cgi_callguideinfoBase].[ImportSequenceNumber],
    [cgi_callguideinfoBase].[OverriddenCreatedOn],
    [cgi_callguideinfoBase].[TimeZoneRuleVersionNumber],
    [cgi_callguideinfoBase].[UTCConversionTimeZoneCode],
    [cgi_callguideinfoBase].[cgi_callguideinfoname],
    [cgi_callguideinfoBase].[cgi_AgentName],
    [cgi_callguideinfoBase].[cgi_APhoneNumber],
    [cgi_callguideinfoBase].[cgi_BPhoneNumber],
    [cgi_callguideinfoBase].[cgi_CallguideSessionID],
    [cgi_callguideinfoBase].[cgi_Cid],
    [cgi_callguideinfoBase].[cgi_ContactSourceType],
    [cgi_callguideinfoBase].[cgi_Duration],
    [cgi_callguideinfoBase].[cgi_ErrandTaskType],
    [cgi_callguideinfoBase].[cgi_QueueTime],
    [cgi_callguideinfoBase].[cgi_ScreenPopChoice],
    [cgi_callguideinfoBase].[cgi_chatcustomeralias]
from [cgi_callguideinfoBase] 
    left join [SystemUserBase] [lk_cgi_callguideinfo_createdby] with(nolock) on ([cgi_callguideinfoBase].[CreatedBy] = [lk_cgi_callguideinfo_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_callguideinfo_createdonbehalfby] with(nolock) on ([cgi_callguideinfoBase].[CreatedOnBehalfBy] = [lk_cgi_callguideinfo_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_callguideinfo_modifiedby] with(nolock) on ([cgi_callguideinfoBase].[ModifiedBy] = [lk_cgi_callguideinfo_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_callguideinfo_modifiedonbehalfby] with(nolock) on ([cgi_callguideinfoBase].[ModifiedOnBehalfBy] = [lk_cgi_callguideinfo_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_callguideinfoBase].OwnerId = XXowner.OwnerId)

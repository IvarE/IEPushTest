


--
-- base view for cgi_routingaction
--
create view dbo.[cgi_routingaction]
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
    [cgi_routingactionId],
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
    [cgi_routingactionname],
    [cgi_Action],
    [cgi_RoutingActionNumber]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_routingaction_createdby].[FullName],
    [lk_cgi_routingaction_createdby].[YomiFullName],
    [lk_cgi_routingaction_createdonbehalfby].[FullName],
    [lk_cgi_routingaction_createdonbehalfby].[YomiFullName],
    [lk_cgi_routingaction_modifiedby].[FullName],
    [lk_cgi_routingaction_modifiedby].[YomiFullName],
    [lk_cgi_routingaction_modifiedonbehalfby].[FullName],
    [lk_cgi_routingaction_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_routingactionBase].OwnerId,
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
    [cgi_routingactionBase].[cgi_routingactionId],
    [cgi_routingactionBase].[CreatedOn],
    [cgi_routingactionBase].[CreatedBy],
    [cgi_routingactionBase].[ModifiedOn],
    [cgi_routingactionBase].[ModifiedBy],
    [cgi_routingactionBase].[CreatedOnBehalfBy],
    [cgi_routingactionBase].[ModifiedOnBehalfBy],
    [cgi_routingactionBase].[OwningBusinessUnit],
    [cgi_routingactionBase].[statecode],
    [cgi_routingactionBase].[statuscode],
    [cgi_routingactionBase].[VersionNumber],
    [cgi_routingactionBase].[ImportSequenceNumber],
    [cgi_routingactionBase].[OverriddenCreatedOn],
    [cgi_routingactionBase].[TimeZoneRuleVersionNumber],
    [cgi_routingactionBase].[UTCConversionTimeZoneCode],
    [cgi_routingactionBase].[cgi_routingactionname],
    [cgi_routingactionBase].[cgi_Action],
    [cgi_routingactionBase].[cgi_RoutingActionNumber]
from [cgi_routingactionBase] 
    left join [SystemUserBase] [lk_cgi_routingaction_createdby] with(nolock) on ([cgi_routingactionBase].[CreatedBy] = [lk_cgi_routingaction_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_routingaction_createdonbehalfby] with(nolock) on ([cgi_routingactionBase].[CreatedOnBehalfBy] = [lk_cgi_routingaction_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_routingaction_modifiedby] with(nolock) on ([cgi_routingactionBase].[ModifiedBy] = [lk_cgi_routingaction_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_routingaction_modifiedonbehalfby] with(nolock) on ([cgi_routingactionBase].[ModifiedOnBehalfBy] = [lk_cgi_routingaction_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_routingactionBase].OwnerId = XXowner.OwnerId)

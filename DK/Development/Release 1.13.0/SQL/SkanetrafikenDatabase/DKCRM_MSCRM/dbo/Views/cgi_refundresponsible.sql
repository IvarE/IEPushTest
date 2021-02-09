


--
-- base view for cgi_refundresponsible
--
create view dbo.[cgi_refundresponsible]
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
    [cgi_refundresponsibleId],
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
    [cgi_name],
    [cgi_responsible]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_refundresponsible_createdby].[FullName],
    [lk_cgi_refundresponsible_createdby].[YomiFullName],
    [lk_cgi_refundresponsible_createdonbehalfby].[FullName],
    [lk_cgi_refundresponsible_createdonbehalfby].[YomiFullName],
    [lk_cgi_refundresponsible_modifiedby].[FullName],
    [lk_cgi_refundresponsible_modifiedby].[YomiFullName],
    [lk_cgi_refundresponsible_modifiedonbehalfby].[FullName],
    [lk_cgi_refundresponsible_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_refundresponsibleBase].OwnerId,
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
    [cgi_refundresponsibleBase].[cgi_refundresponsibleId],
    [cgi_refundresponsibleBase].[CreatedOn],
    [cgi_refundresponsibleBase].[CreatedBy],
    [cgi_refundresponsibleBase].[ModifiedOn],
    [cgi_refundresponsibleBase].[ModifiedBy],
    [cgi_refundresponsibleBase].[CreatedOnBehalfBy],
    [cgi_refundresponsibleBase].[ModifiedOnBehalfBy],
    [cgi_refundresponsibleBase].[OwningBusinessUnit],
    [cgi_refundresponsibleBase].[statecode],
    [cgi_refundresponsibleBase].[statuscode],
    [cgi_refundresponsibleBase].[VersionNumber],
    [cgi_refundresponsibleBase].[ImportSequenceNumber],
    [cgi_refundresponsibleBase].[OverriddenCreatedOn],
    [cgi_refundresponsibleBase].[TimeZoneRuleVersionNumber],
    [cgi_refundresponsibleBase].[UTCConversionTimeZoneCode],
    [cgi_refundresponsibleBase].[cgi_name],
    [cgi_refundresponsibleBase].[cgi_responsible]
from [cgi_refundresponsibleBase] 
    left join [SystemUserBase] [lk_cgi_refundresponsible_createdby] with(nolock) on ([cgi_refundresponsibleBase].[CreatedBy] = [lk_cgi_refundresponsible_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundresponsible_createdonbehalfby] with(nolock) on ([cgi_refundresponsibleBase].[CreatedOnBehalfBy] = [lk_cgi_refundresponsible_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundresponsible_modifiedby] with(nolock) on ([cgi_refundresponsibleBase].[ModifiedBy] = [lk_cgi_refundresponsible_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundresponsible_modifiedonbehalfby] with(nolock) on ([cgi_refundresponsibleBase].[ModifiedOnBehalfBy] = [lk_cgi_refundresponsible_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_refundresponsibleBase].OwnerId = XXowner.OwnerId)

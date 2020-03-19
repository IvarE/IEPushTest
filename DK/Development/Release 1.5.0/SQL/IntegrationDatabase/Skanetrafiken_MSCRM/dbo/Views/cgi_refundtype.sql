


--
-- base view for cgi_refundtype
--
create view dbo.[cgi_refundtype]
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
    [cgi_refundaccountidName],
    [cgi_refundresponsibleIdName],
    [cgi_refundproductidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_refundtypeId],
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
    [cgi_refundtypename],
    [cgi_FinancialTransaction],
    [cgi_RefundOption],
    [cgi_refundaccountid],
    [cgi_refundresponsibleId],
    [cgi_refundproductid],
    [cgi_reinvoice],
    [cgi_PrintText]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_refundtype_createdby].[FullName],
    [lk_cgi_refundtype_createdby].[YomiFullName],
    [lk_cgi_refundtype_createdonbehalfby].[FullName],
    [lk_cgi_refundtype_createdonbehalfby].[YomiFullName],
    [lk_cgi_refundtype_modifiedby].[FullName],
    [lk_cgi_refundtype_modifiedby].[YomiFullName],
    [lk_cgi_refundtype_modifiedonbehalfby].[FullName],
    [lk_cgi_refundtype_modifiedonbehalfby].[YomiFullName],
    [cgi_refundaccount_cgi_refundtype].[cgi_refundaccountname],
    [cgi_refundresponsible_cgi_refundtype].[cgi_name],
    [cgi_refundproduct_cgi_refundtype].[cgi_refundproductname],

    -- ownership entries
    OwnerId = [cgi_refundtypeBase].OwnerId,
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
    [cgi_refundtypeBase].[cgi_refundtypeId],
    [cgi_refundtypeBase].[CreatedOn],
    [cgi_refundtypeBase].[CreatedBy],
    [cgi_refundtypeBase].[ModifiedOn],
    [cgi_refundtypeBase].[ModifiedBy],
    [cgi_refundtypeBase].[CreatedOnBehalfBy],
    [cgi_refundtypeBase].[ModifiedOnBehalfBy],
    [cgi_refundtypeBase].[OwningBusinessUnit],
    [cgi_refundtypeBase].[statecode],
    [cgi_refundtypeBase].[statuscode],
    [cgi_refundtypeBase].[VersionNumber],
    [cgi_refundtypeBase].[ImportSequenceNumber],
    [cgi_refundtypeBase].[OverriddenCreatedOn],
    [cgi_refundtypeBase].[TimeZoneRuleVersionNumber],
    [cgi_refundtypeBase].[UTCConversionTimeZoneCode],
    [cgi_refundtypeBase].[cgi_refundtypename],
    [cgi_refundtypeBase].[cgi_FinancialTransaction],
    [cgi_refundtypeBase].[cgi_RefundOption],
    [cgi_refundtypeBase].[cgi_refundaccountid],
    [cgi_refundtypeBase].[cgi_refundresponsibleId],
    [cgi_refundtypeBase].[cgi_refundproductid],
    [cgi_refundtypeBase].[cgi_reinvoice],
    [cgi_refundtypeBase].[cgi_PrintText]
from [cgi_refundtypeBase] 
    left join [cgi_refundaccountBase] [cgi_refundaccount_cgi_refundtype] on ([cgi_refundtypeBase].[cgi_refundaccountid] = [cgi_refundaccount_cgi_refundtype].[cgi_refundaccountId])
    left join [cgi_refundproductBase] [cgi_refundproduct_cgi_refundtype] on ([cgi_refundtypeBase].[cgi_refundproductid] = [cgi_refundproduct_cgi_refundtype].[cgi_refundproductId])
    left join [cgi_refundresponsibleBase] [cgi_refundresponsible_cgi_refundtype] on ([cgi_refundtypeBase].[cgi_refundresponsibleId] = [cgi_refundresponsible_cgi_refundtype].[cgi_refundresponsibleId])
    left join [SystemUserBase] [lk_cgi_refundtype_createdby] with(nolock) on ([cgi_refundtypeBase].[CreatedBy] = [lk_cgi_refundtype_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundtype_createdonbehalfby] with(nolock) on ([cgi_refundtypeBase].[CreatedOnBehalfBy] = [lk_cgi_refundtype_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundtype_modifiedby] with(nolock) on ([cgi_refundtypeBase].[ModifiedBy] = [lk_cgi_refundtype_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundtype_modifiedonbehalfby] with(nolock) on ([cgi_refundtypeBase].[ModifiedOnBehalfBy] = [lk_cgi_refundtype_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_refundtypeBase].OwnerId = XXowner.OwnerId)

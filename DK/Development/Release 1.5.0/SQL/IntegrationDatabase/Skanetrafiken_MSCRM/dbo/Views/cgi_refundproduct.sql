


--
-- base view for cgi_refundproduct
--
create view dbo.[cgi_refundproduct]
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
    [cgi_refundproductId],
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
    [cgi_refundproductname],
    [cgi_Account]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_refundproduct_createdby].[FullName],
    [lk_cgi_refundproduct_createdby].[YomiFullName],
    [lk_cgi_refundproduct_createdonbehalfby].[FullName],
    [lk_cgi_refundproduct_createdonbehalfby].[YomiFullName],
    [lk_cgi_refundproduct_modifiedby].[FullName],
    [lk_cgi_refundproduct_modifiedby].[YomiFullName],
    [lk_cgi_refundproduct_modifiedonbehalfby].[FullName],
    [lk_cgi_refundproduct_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_refundproductBase].OwnerId,
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
    [cgi_refundproductBase].[cgi_refundproductId],
    [cgi_refundproductBase].[CreatedOn],
    [cgi_refundproductBase].[CreatedBy],
    [cgi_refundproductBase].[ModifiedOn],
    [cgi_refundproductBase].[ModifiedBy],
    [cgi_refundproductBase].[CreatedOnBehalfBy],
    [cgi_refundproductBase].[ModifiedOnBehalfBy],
    [cgi_refundproductBase].[OwningBusinessUnit],
    [cgi_refundproductBase].[statecode],
    [cgi_refundproductBase].[statuscode],
    [cgi_refundproductBase].[VersionNumber],
    [cgi_refundproductBase].[ImportSequenceNumber],
    [cgi_refundproductBase].[OverriddenCreatedOn],
    [cgi_refundproductBase].[TimeZoneRuleVersionNumber],
    [cgi_refundproductBase].[UTCConversionTimeZoneCode],
    [cgi_refundproductBase].[cgi_refundproductname],
    [cgi_refundproductBase].[cgi_Account]
from [cgi_refundproductBase] 
    left join [SystemUserBase] [lk_cgi_refundproduct_createdby] with(nolock) on ([cgi_refundproductBase].[CreatedBy] = [lk_cgi_refundproduct_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundproduct_createdonbehalfby] with(nolock) on ([cgi_refundproductBase].[CreatedOnBehalfBy] = [lk_cgi_refundproduct_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundproduct_modifiedby] with(nolock) on ([cgi_refundproductBase].[ModifiedBy] = [lk_cgi_refundproduct_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundproduct_modifiedonbehalfby] with(nolock) on ([cgi_refundproductBase].[ModifiedOnBehalfBy] = [lk_cgi_refundproduct_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_refundproductBase].OwnerId = XXowner.OwnerId)

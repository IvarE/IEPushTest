


--
-- base view for cgi_reimbursementform
--
create view dbo.[cgi_reimbursementform]
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
    [cgi_reimbursementformId],
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
    [cgi_reimbursementname],
    [cgi_ReInvoicing],
    [cgi_UseAccount],
    [cgi_UseProduct],
    [cgi_sendtostralfors],
    [cgi_loadcard],
    [cgi_attestation],
    [cgi_useresponsible],
    [cgi_payment],
    [cgi_payment_abroad],
    [cgi_time_valid],
    [cgi_giftcard],
    [cgi_Print],
    [cgi_couponsms],
    [cgi_couponemail]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_reimbursementform_createdby].[FullName],
    [lk_cgi_reimbursementform_createdby].[YomiFullName],
    [lk_cgi_reimbursementform_createdonbehalfby].[FullName],
    [lk_cgi_reimbursementform_createdonbehalfby].[YomiFullName],
    [lk_cgi_reimbursementform_modifiedby].[FullName],
    [lk_cgi_reimbursementform_modifiedby].[YomiFullName],
    [lk_cgi_reimbursementform_modifiedonbehalfby].[FullName],
    [lk_cgi_reimbursementform_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_reimbursementformBase].OwnerId,
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
    [cgi_reimbursementformBase].[cgi_reimbursementformId],
    [cgi_reimbursementformBase].[CreatedOn],
    [cgi_reimbursementformBase].[CreatedBy],
    [cgi_reimbursementformBase].[ModifiedOn],
    [cgi_reimbursementformBase].[ModifiedBy],
    [cgi_reimbursementformBase].[CreatedOnBehalfBy],
    [cgi_reimbursementformBase].[ModifiedOnBehalfBy],
    [cgi_reimbursementformBase].[OwningBusinessUnit],
    [cgi_reimbursementformBase].[statecode],
    [cgi_reimbursementformBase].[statuscode],
    [cgi_reimbursementformBase].[VersionNumber],
    [cgi_reimbursementformBase].[ImportSequenceNumber],
    [cgi_reimbursementformBase].[OverriddenCreatedOn],
    [cgi_reimbursementformBase].[TimeZoneRuleVersionNumber],
    [cgi_reimbursementformBase].[UTCConversionTimeZoneCode],
    [cgi_reimbursementformBase].[cgi_reimbursementname],
    [cgi_reimbursementformBase].[cgi_ReInvoicing],
    [cgi_reimbursementformBase].[cgi_UseAccount],
    [cgi_reimbursementformBase].[cgi_UseProduct],
    [cgi_reimbursementformBase].[cgi_sendtostralfors],
    [cgi_reimbursementformBase].[cgi_loadcard],
    [cgi_reimbursementformBase].[cgi_attestation],
    [cgi_reimbursementformBase].[cgi_useresponsible],
    [cgi_reimbursementformBase].[cgi_payment],
    [cgi_reimbursementformBase].[cgi_payment_abroad],
    [cgi_reimbursementformBase].[cgi_time_valid],
    [cgi_reimbursementformBase].[cgi_giftcard],
    [cgi_reimbursementformBase].[cgi_Print],
    [cgi_reimbursementformBase].[cgi_couponsms],
    [cgi_reimbursementformBase].[cgi_couponemail]
from [cgi_reimbursementformBase] 
    left join [SystemUserBase] [lk_cgi_reimbursementform_createdby] with(nolock) on ([cgi_reimbursementformBase].[CreatedBy] = [lk_cgi_reimbursementform_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_reimbursementform_createdonbehalfby] with(nolock) on ([cgi_reimbursementformBase].[CreatedOnBehalfBy] = [lk_cgi_reimbursementform_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_reimbursementform_modifiedby] with(nolock) on ([cgi_reimbursementformBase].[ModifiedBy] = [lk_cgi_reimbursementform_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_reimbursementform_modifiedonbehalfby] with(nolock) on ([cgi_reimbursementformBase].[ModifiedOnBehalfBy] = [lk_cgi_reimbursementform_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_reimbursementformBase].OwnerId = XXowner.OwnerId)

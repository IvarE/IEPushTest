


--
-- base view for cgi_rgolsetting
--
create view dbo.[cgi_rgolsetting]
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
    [cgi_RefundTypeidName],
    [cgi_ReimbursementFormidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_rgolsettingId],
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
    [cgi_rgolsettingno],
    [cgi_ReimbursementFormid],
    [cgi_RefundTypeid]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_rgolsetting_createdby].[FullName],
    [lk_cgi_rgolsetting_createdby].[YomiFullName],
    [lk_cgi_rgolsetting_createdonbehalfby].[FullName],
    [lk_cgi_rgolsetting_createdonbehalfby].[YomiFullName],
    [lk_cgi_rgolsetting_modifiedby].[FullName],
    [lk_cgi_rgolsetting_modifiedby].[YomiFullName],
    [lk_cgi_rgolsetting_modifiedonbehalfby].[FullName],
    [lk_cgi_rgolsetting_modifiedonbehalfby].[YomiFullName],
    [cgi_cgi_refundtype_cgi_rgolsetting_RefundTypeid].[cgi_refundtypename],
    [cgi_cgi_reimbursementform_cgi_rgolsetting_ReimbursementFormid].[cgi_reimbursementname],

    -- ownership entries
    OwnerId = [cgi_rgolsettingBase].OwnerId,
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
    [cgi_rgolsettingBase].[cgi_rgolsettingId],
    [cgi_rgolsettingBase].[CreatedOn],
    [cgi_rgolsettingBase].[CreatedBy],
    [cgi_rgolsettingBase].[ModifiedOn],
    [cgi_rgolsettingBase].[ModifiedBy],
    [cgi_rgolsettingBase].[CreatedOnBehalfBy],
    [cgi_rgolsettingBase].[ModifiedOnBehalfBy],
    [cgi_rgolsettingBase].[OwningBusinessUnit],
    [cgi_rgolsettingBase].[statecode],
    [cgi_rgolsettingBase].[statuscode],
    [cgi_rgolsettingBase].[VersionNumber],
    [cgi_rgolsettingBase].[ImportSequenceNumber],
    [cgi_rgolsettingBase].[OverriddenCreatedOn],
    [cgi_rgolsettingBase].[TimeZoneRuleVersionNumber],
    [cgi_rgolsettingBase].[UTCConversionTimeZoneCode],
    [cgi_rgolsettingBase].[cgi_name],
    [cgi_rgolsettingBase].[cgi_rgolsettingno],
    [cgi_rgolsettingBase].[cgi_ReimbursementFormid],
    [cgi_rgolsettingBase].[cgi_RefundTypeid]
from [cgi_rgolsettingBase] 
    left join [cgi_refundtypeBase] [cgi_cgi_refundtype_cgi_rgolsetting_RefundTypeid] on ([cgi_rgolsettingBase].[cgi_RefundTypeid] = [cgi_cgi_refundtype_cgi_rgolsetting_RefundTypeid].[cgi_refundtypeId])
    left join [cgi_reimbursementformBase] [cgi_cgi_reimbursementform_cgi_rgolsetting_ReimbursementFormid] on ([cgi_rgolsettingBase].[cgi_ReimbursementFormid] = [cgi_cgi_reimbursementform_cgi_rgolsetting_ReimbursementFormid].[cgi_reimbursementformId])
    left join [SystemUserBase] [lk_cgi_rgolsetting_createdby] with(nolock) on ([cgi_rgolsettingBase].[CreatedBy] = [lk_cgi_rgolsetting_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_rgolsetting_createdonbehalfby] with(nolock) on ([cgi_rgolsettingBase].[CreatedOnBehalfBy] = [lk_cgi_rgolsetting_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_rgolsetting_modifiedby] with(nolock) on ([cgi_rgolsettingBase].[ModifiedBy] = [lk_cgi_rgolsetting_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_rgolsetting_modifiedonbehalfby] with(nolock) on ([cgi_rgolsettingBase].[ModifiedOnBehalfBy] = [lk_cgi_rgolsetting_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_rgolsettingBase].OwnerId = XXowner.OwnerId)

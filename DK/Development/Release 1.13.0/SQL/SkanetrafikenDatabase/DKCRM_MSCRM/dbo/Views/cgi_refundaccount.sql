


--
-- base view for cgi_refundaccount
--
create view dbo.[cgi_refundaccount]
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
    [cgi_refundaccountId],
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
    [cgi_refundaccountname],
    [cgi_Account]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_refundaccount_createdby].[FullName],
    [lk_cgi_refundaccount_createdby].[YomiFullName],
    [lk_cgi_refundaccount_createdonbehalfby].[FullName],
    [lk_cgi_refundaccount_createdonbehalfby].[YomiFullName],
    [lk_cgi_refundaccount_modifiedby].[FullName],
    [lk_cgi_refundaccount_modifiedby].[YomiFullName],
    [lk_cgi_refundaccount_modifiedonbehalfby].[FullName],
    [lk_cgi_refundaccount_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_refundaccountBase].OwnerId,
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
    [cgi_refundaccountBase].[cgi_refundaccountId],
    [cgi_refundaccountBase].[CreatedOn],
    [cgi_refundaccountBase].[CreatedBy],
    [cgi_refundaccountBase].[ModifiedOn],
    [cgi_refundaccountBase].[ModifiedBy],
    [cgi_refundaccountBase].[CreatedOnBehalfBy],
    [cgi_refundaccountBase].[ModifiedOnBehalfBy],
    [cgi_refundaccountBase].[OwningBusinessUnit],
    [cgi_refundaccountBase].[statecode],
    [cgi_refundaccountBase].[statuscode],
    [cgi_refundaccountBase].[VersionNumber],
    [cgi_refundaccountBase].[ImportSequenceNumber],
    [cgi_refundaccountBase].[OverriddenCreatedOn],
    [cgi_refundaccountBase].[TimeZoneRuleVersionNumber],
    [cgi_refundaccountBase].[UTCConversionTimeZoneCode],
    [cgi_refundaccountBase].[cgi_refundaccountname],
    [cgi_refundaccountBase].[cgi_Account]
from [cgi_refundaccountBase] 
    left join [SystemUserBase] [lk_cgi_refundaccount_createdby] with(nolock) on ([cgi_refundaccountBase].[CreatedBy] = [lk_cgi_refundaccount_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundaccount_createdonbehalfby] with(nolock) on ([cgi_refundaccountBase].[CreatedOnBehalfBy] = [lk_cgi_refundaccount_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundaccount_modifiedby] with(nolock) on ([cgi_refundaccountBase].[ModifiedBy] = [lk_cgi_refundaccount_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_refundaccount_modifiedonbehalfby] with(nolock) on ([cgi_refundaccountBase].[ModifiedOnBehalfBy] = [lk_cgi_refundaccount_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_refundaccountBase].OwnerId = XXowner.OwnerId)

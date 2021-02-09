


--
-- base view for cgi_invoicerecipient
--
create view dbo.[cgi_invoicerecipient]
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
    [cgi_invoicerecipientId],
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
    [cgi_invoicerecipientname],
    [EmailAddress],
    [cgi_customer_no],
    [cgi_inv_reference],
    [cgi_address1],
    [cgi_postalcode],
    [cgi_postal_city]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_invoicerecipient_createdby].[FullName],
    [lk_cgi_invoicerecipient_createdby].[YomiFullName],
    [lk_cgi_invoicerecipient_createdonbehalfby].[FullName],
    [lk_cgi_invoicerecipient_createdonbehalfby].[YomiFullName],
    [lk_cgi_invoicerecipient_modifiedby].[FullName],
    [lk_cgi_invoicerecipient_modifiedby].[YomiFullName],
    [lk_cgi_invoicerecipient_modifiedonbehalfby].[FullName],
    [lk_cgi_invoicerecipient_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_invoicerecipientBase].OwnerId,
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
    [cgi_invoicerecipientBase].[cgi_invoicerecipientId],
    [cgi_invoicerecipientBase].[CreatedOn],
    [cgi_invoicerecipientBase].[CreatedBy],
    [cgi_invoicerecipientBase].[ModifiedOn],
    [cgi_invoicerecipientBase].[ModifiedBy],
    [cgi_invoicerecipientBase].[CreatedOnBehalfBy],
    [cgi_invoicerecipientBase].[ModifiedOnBehalfBy],
    [cgi_invoicerecipientBase].[OwningBusinessUnit],
    [cgi_invoicerecipientBase].[statecode],
    [cgi_invoicerecipientBase].[statuscode],
    [cgi_invoicerecipientBase].[VersionNumber],
    [cgi_invoicerecipientBase].[ImportSequenceNumber],
    [cgi_invoicerecipientBase].[OverriddenCreatedOn],
    [cgi_invoicerecipientBase].[TimeZoneRuleVersionNumber],
    [cgi_invoicerecipientBase].[UTCConversionTimeZoneCode],
    [cgi_invoicerecipientBase].[cgi_invoicerecipientname],
    [cgi_invoicerecipientBase].[EmailAddress],
    [cgi_invoicerecipientBase].[cgi_customer_no],
    [cgi_invoicerecipientBase].[cgi_inv_reference],
    [cgi_invoicerecipientBase].[cgi_address1],
    [cgi_invoicerecipientBase].[cgi_postalcode],
    [cgi_invoicerecipientBase].[cgi_postal_city]
from [cgi_invoicerecipientBase] 
    left join [SystemUserBase] [lk_cgi_invoicerecipient_createdby] with(nolock) on ([cgi_invoicerecipientBase].[CreatedBy] = [lk_cgi_invoicerecipient_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_invoicerecipient_createdonbehalfby] with(nolock) on ([cgi_invoicerecipientBase].[CreatedOnBehalfBy] = [lk_cgi_invoicerecipient_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_invoicerecipient_modifiedby] with(nolock) on ([cgi_invoicerecipientBase].[ModifiedBy] = [lk_cgi_invoicerecipient_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_invoicerecipient_modifiedonbehalfby] with(nolock) on ([cgi_invoicerecipientBase].[ModifiedOnBehalfBy] = [lk_cgi_invoicerecipient_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_invoicerecipientBase].OwnerId = XXowner.OwnerId)

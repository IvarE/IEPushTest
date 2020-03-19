


--
-- base view for cgi_creditorderrow
--
create view dbo.[cgi_creditorderrow]
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
    [cgi_AccountidYomiName],
    [cgi_AccountidName],
    [cgi_ContactidYomiName],
    [cgi_ContactidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_creditorderrowId],
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
    [cgi_OrderNumber],
    [cgi_Sum],
    [cgi_ReferenceNumber],
    [cgi_Success],
    [cgi_Message],
    [cgi_Date],
    [cgi_Time],
    [cgi_ProductNumber],
    [cgi_Accountid],
    [cgi_Contactid],
    [cgi_Reason],
    [cgi_CreatedBy]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_creditorderrow_createdby].[FullName],
    [lk_cgi_creditorderrow_createdby].[YomiFullName],
    [lk_cgi_creditorderrow_createdonbehalfby].[FullName],
    [lk_cgi_creditorderrow_createdonbehalfby].[YomiFullName],
    [lk_cgi_creditorderrow_modifiedby].[FullName],
    [lk_cgi_creditorderrow_modifiedby].[YomiFullName],
    [lk_cgi_creditorderrow_modifiedonbehalfby].[FullName],
    [lk_cgi_creditorderrow_modifiedonbehalfby].[YomiFullName],
    [cgi_account_cgi_creditorderrow_Account].[YomiName],
    [cgi_account_cgi_creditorderrow_Account].[Name],
    [cgi_contact_cgi_creditorderrow_Contactid].[YomiFullName],
    [cgi_contact_cgi_creditorderrow_Contactid].[FullName],

    -- ownership entries
    OwnerId = [cgi_creditorderrowBase].OwnerId,
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
    [cgi_creditorderrowBase].[cgi_creditorderrowId],
    [cgi_creditorderrowBase].[CreatedOn],
    [cgi_creditorderrowBase].[CreatedBy],
    [cgi_creditorderrowBase].[ModifiedOn],
    [cgi_creditorderrowBase].[ModifiedBy],
    [cgi_creditorderrowBase].[CreatedOnBehalfBy],
    [cgi_creditorderrowBase].[ModifiedOnBehalfBy],
    [cgi_creditorderrowBase].[OwningBusinessUnit],
    [cgi_creditorderrowBase].[statecode],
    [cgi_creditorderrowBase].[statuscode],
    [cgi_creditorderrowBase].[VersionNumber],
    [cgi_creditorderrowBase].[ImportSequenceNumber],
    [cgi_creditorderrowBase].[OverriddenCreatedOn],
    [cgi_creditorderrowBase].[TimeZoneRuleVersionNumber],
    [cgi_creditorderrowBase].[UTCConversionTimeZoneCode],
    [cgi_creditorderrowBase].[cgi_name],
    [cgi_creditorderrowBase].[cgi_OrderNumber],
    [cgi_creditorderrowBase].[cgi_Sum],
    [cgi_creditorderrowBase].[cgi_ReferenceNumber],
    [cgi_creditorderrowBase].[cgi_Success],
    [cgi_creditorderrowBase].[cgi_Message],
    [cgi_creditorderrowBase].[cgi_Date],
    [cgi_creditorderrowBase].[cgi_Time],
    [cgi_creditorderrowBase].[cgi_ProductNumber],
    [cgi_creditorderrowBase].[cgi_Accountid],
    [cgi_creditorderrowBase].[cgi_Contactid],
    [cgi_creditorderrowBase].[cgi_Reason],
    [cgi_creditorderrowBase].[cgi_CreatedBy]
from [cgi_creditorderrowBase] 
    left join [AccountBase] [cgi_account_cgi_creditorderrow_Account] on ([cgi_creditorderrowBase].[cgi_Accountid] = [cgi_account_cgi_creditorderrow_Account].[AccountId])
    left join [ContactBase] [cgi_contact_cgi_creditorderrow_Contactid] on ([cgi_creditorderrowBase].[cgi_Contactid] = [cgi_contact_cgi_creditorderrow_Contactid].[ContactId])
    left join [SystemUserBase] [lk_cgi_creditorderrow_createdby] with(nolock) on ([cgi_creditorderrowBase].[CreatedBy] = [lk_cgi_creditorderrow_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_creditorderrow_createdonbehalfby] with(nolock) on ([cgi_creditorderrowBase].[CreatedOnBehalfBy] = [lk_cgi_creditorderrow_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_creditorderrow_modifiedby] with(nolock) on ([cgi_creditorderrowBase].[ModifiedBy] = [lk_cgi_creditorderrow_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_creditorderrow_modifiedonbehalfby] with(nolock) on ([cgi_creditorderrowBase].[ModifiedOnBehalfBy] = [lk_cgi_creditorderrow_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_creditorderrowBase].OwnerId = XXowner.OwnerId)

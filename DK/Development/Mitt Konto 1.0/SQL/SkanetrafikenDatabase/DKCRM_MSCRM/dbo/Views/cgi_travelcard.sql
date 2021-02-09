


--
-- base view for cgi_travelcard
--
create view dbo.[cgi_travelcard]
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
    [cgi_ContactidName],
    [TransactionCurrencyIdName],
    [cgi_ContactidYomiName],
    [cgi_AccountidYomiName],
    [cgi_AccountidName],
    [cgi_CardTypeidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_travelcardId],
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
    [cgi_travelcardnumber],
    [cgi_Blocked],
    [cgi_NumberofZones],
    [cgi_TravelCardCVC],
    [cgi_TravelCardName],
    [cgi_ValidFrom],
    [cgi_ValidTo],
    [cgi_Accountid],
    [cgi_CardTypeid],
    [cgi_ImportID],
    [cgi_periodic_card_type],
    [cgi_value_card_type],
    [cgi_Contactid],
    [cgi_AutoloadConnectionDate],
    [cgi_AutoloadDisconnectionDate],
    [cgi_PeriodCardTypeId],
    [cgi_ValueCardTypeId],
    [cgi_LatestChargeDate],
    [cgi_LatestAutoloadAmount],
    [TransactionCurrencyId],
    [ExchangeRate],
    [cgi_latestautoloadamount_Base],
    [cgi_FailedAttemptsToChargeMoney],
    [cgi_CreditCardMask],
    [cgi_VerifyId],
    [cgi_Currency],
    [cgi_CardCategory],
    [cgi_LatestFailedAttempt],
    [cgi_AutoloadStatus],
    [st_OP_Offer],
    [st_OP_Offer_Date],
    [st_OP_Offer_Code]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_travelcard_createdby].[FullName],
    [lk_cgi_travelcard_createdby].[YomiFullName],
    [lk_cgi_travelcard_createdonbehalfby].[FullName],
    [lk_cgi_travelcard_createdonbehalfby].[YomiFullName],
    [lk_cgi_travelcard_modifiedby].[FullName],
    [lk_cgi_travelcard_modifiedby].[YomiFullName],
    [lk_cgi_travelcard_modifiedonbehalfby].[FullName],
    [lk_cgi_travelcard_modifiedonbehalfby].[YomiFullName],
    [cgi_contact_cgi_travelcard_Contactid].[FullName],
    [TransactionCurrency_cgi_travelcard].[CurrencyName],
    [cgi_contact_cgi_travelcard_Contactid].[YomiFullName],
    [cgi_account_cgi_travelcard_Account].[YomiName],
    [cgi_account_cgi_travelcard_Account].[Name],
    [cgi_cgi_travelcardtype_cgi_travelcard_CardTypeid].[cgi_travelcardtypename],

    -- ownership entries
    OwnerId = [cgi_travelcardBase].OwnerId,
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
    [cgi_travelcardBase].[cgi_travelcardId],
    [cgi_travelcardBase].[CreatedOn],
    [cgi_travelcardBase].[CreatedBy],
    [cgi_travelcardBase].[ModifiedOn],
    [cgi_travelcardBase].[ModifiedBy],
    [cgi_travelcardBase].[CreatedOnBehalfBy],
    [cgi_travelcardBase].[ModifiedOnBehalfBy],
    [cgi_travelcardBase].[OwningBusinessUnit],
    [cgi_travelcardBase].[statecode],
    [cgi_travelcardBase].[statuscode],
    [cgi_travelcardBase].[VersionNumber],
    [cgi_travelcardBase].[ImportSequenceNumber],
    [cgi_travelcardBase].[OverriddenCreatedOn],
    [cgi_travelcardBase].[TimeZoneRuleVersionNumber],
    [cgi_travelcardBase].[UTCConversionTimeZoneCode],
    [cgi_travelcardBase].[cgi_travelcardnumber],
    [cgi_travelcardBase].[cgi_Blocked],
    [cgi_travelcardBase].[cgi_NumberofZones],
    [cgi_travelcardBase].[cgi_TravelCardCVC],
    [cgi_travelcardBase].[cgi_TravelCardName],
    [cgi_travelcardBase].[cgi_ValidFrom],
    [cgi_travelcardBase].[cgi_ValidTo],
    [cgi_travelcardBase].[cgi_Accountid],
    [cgi_travelcardBase].[cgi_CardTypeid],
    [cgi_travelcardBase].[cgi_ImportID],
    [cgi_travelcardBase].[cgi_periodic_card_type],
    [cgi_travelcardBase].[cgi_value_card_type],
    [cgi_travelcardBase].[cgi_Contactid],
    [cgi_travelcardBase].[cgi_AutoloadConnectionDate],
    [cgi_travelcardBase].[cgi_AutoloadDisconnectionDate],
    [cgi_travelcardBase].[cgi_PeriodCardTypeId],
    [cgi_travelcardBase].[cgi_ValueCardTypeId],
    [cgi_travelcardBase].[cgi_LatestChargeDate],
    [cgi_travelcardBase].[cgi_LatestAutoloadAmount],
    [cgi_travelcardBase].[TransactionCurrencyId],
    [cgi_travelcardBase].[ExchangeRate],
    [cgi_travelcardBase].[cgi_latestautoloadamount_Base],
    [cgi_travelcardBase].[cgi_FailedAttemptsToChargeMoney],
    [cgi_travelcardBase].[cgi_CreditCardMask],
    [cgi_travelcardBase].[cgi_VerifyId],
    [cgi_travelcardBase].[cgi_Currency],
    [cgi_travelcardBase].[cgi_CardCategory],
    [cgi_travelcardBase].[cgi_LatestFailedAttempt],
    [cgi_travelcardBase].[cgi_AutoloadStatus],
    [cgi_travelcardBase].[st_OP_Offer],
    [cgi_travelcardBase].[st_OP_Offer_Date],
    [cgi_travelcardBase].[st_OP_Offer_Code]
from [cgi_travelcardBase] 
    left join [AccountBase] [cgi_account_cgi_travelcard_Account] on ([cgi_travelcardBase].[cgi_Accountid] = [cgi_account_cgi_travelcard_Account].[AccountId])
    left join [cgi_travelcardtypeBase] [cgi_cgi_travelcardtype_cgi_travelcard_CardTypeid] on ([cgi_travelcardBase].[cgi_CardTypeid] = [cgi_cgi_travelcardtype_cgi_travelcard_CardTypeid].[cgi_travelcardtypeId])
    left join [ContactBase] [cgi_contact_cgi_travelcard_Contactid] on ([cgi_travelcardBase].[cgi_Contactid] = [cgi_contact_cgi_travelcard_Contactid].[ContactId])
    left join [SystemUserBase] [lk_cgi_travelcard_createdby] with(nolock) on ([cgi_travelcardBase].[CreatedBy] = [lk_cgi_travelcard_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcard_createdonbehalfby] with(nolock) on ([cgi_travelcardBase].[CreatedOnBehalfBy] = [lk_cgi_travelcard_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcard_modifiedby] with(nolock) on ([cgi_travelcardBase].[ModifiedBy] = [lk_cgi_travelcard_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcard_modifiedonbehalfby] with(nolock) on ([cgi_travelcardBase].[ModifiedOnBehalfBy] = [lk_cgi_travelcard_modifiedonbehalfby].[SystemUserId])
    left join [TransactionCurrencyBase] [TransactionCurrency_cgi_travelcard] on ([cgi_travelcardBase].[TransactionCurrencyId] = [TransactionCurrency_cgi_travelcard].[TransactionCurrencyId])
    left join OwnerBase XXowner with(nolock) on ([cgi_travelcardBase].OwnerId = XXowner.OwnerId)




--
-- base view for cgi_travelcardtransaction
--
create view dbo.[cgi_travelcardtransaction]
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
    [cgi_TravelCardidName],
    [TransactionCurrencyIdName],
    [cgi_caseIdName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_travelcardtransactionId],
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
    [cgi_travelcardtransaction],
    [TransactionCurrencyId],
    [ExchangeRate],
    [cgi_TravelCardid],
    [cgi_date],
    [cgi_deviceid],
    [cgi_txnnum],
    [cgi_cardsect],
    [cgi_rectype],
    [cgi_txntype],
    [cgi_route],
    [cgi_currency],
    [cgi_origzone],
    [cgi_time],
    [cgi_caseId],
    [cgi_TravelCard],
    [cgi_OrigZoneName],
    [cgi_Amount]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_travelcardtransaction_createdby].[FullName],
    [lk_cgi_travelcardtransaction_createdby].[YomiFullName],
    [lk_cgi_travelcardtransaction_createdonbehalfby].[FullName],
    [lk_cgi_travelcardtransaction_createdonbehalfby].[YomiFullName],
    [lk_cgi_travelcardtransaction_modifiedby].[FullName],
    [lk_cgi_travelcardtransaction_modifiedby].[YomiFullName],
    [lk_cgi_travelcardtransaction_modifiedonbehalfby].[FullName],
    [lk_cgi_travelcardtransaction_modifiedonbehalfby].[YomiFullName],
    [cgi_cgi_travelcard_cgi_travelcardtransaction_TravelCardid].[cgi_travelcardnumber],
    [TransactionCurrency_cgi_travelcardtransaction].[CurrencyName],
    [cgi_incident_cgi_travelcardtransaction_caseId].[Title],

    -- ownership entries
    OwnerId = [cgi_travelcardtransactionBase].OwnerId,
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
    [cgi_travelcardtransactionBase].[cgi_travelcardtransactionId],
    [cgi_travelcardtransactionBase].[CreatedOn],
    [cgi_travelcardtransactionBase].[CreatedBy],
    [cgi_travelcardtransactionBase].[ModifiedOn],
    [cgi_travelcardtransactionBase].[ModifiedBy],
    [cgi_travelcardtransactionBase].[CreatedOnBehalfBy],
    [cgi_travelcardtransactionBase].[ModifiedOnBehalfBy],
    [cgi_travelcardtransactionBase].[OwningBusinessUnit],
    [cgi_travelcardtransactionBase].[statecode],
    [cgi_travelcardtransactionBase].[statuscode],
    [cgi_travelcardtransactionBase].[VersionNumber],
    [cgi_travelcardtransactionBase].[ImportSequenceNumber],
    [cgi_travelcardtransactionBase].[OverriddenCreatedOn],
    [cgi_travelcardtransactionBase].[TimeZoneRuleVersionNumber],
    [cgi_travelcardtransactionBase].[UTCConversionTimeZoneCode],
    [cgi_travelcardtransactionBase].[cgi_travelcardtransaction],
    [cgi_travelcardtransactionBase].[TransactionCurrencyId],
    [cgi_travelcardtransactionBase].[ExchangeRate],
    [cgi_travelcardtransactionBase].[cgi_TravelCardid],
    [cgi_travelcardtransactionBase].[cgi_date],
    [cgi_travelcardtransactionBase].[cgi_deviceid],
    [cgi_travelcardtransactionBase].[cgi_txnnum],
    [cgi_travelcardtransactionBase].[cgi_cardsect],
    [cgi_travelcardtransactionBase].[cgi_rectype],
    [cgi_travelcardtransactionBase].[cgi_txntype],
    [cgi_travelcardtransactionBase].[cgi_route],
    [cgi_travelcardtransactionBase].[cgi_currency],
    [cgi_travelcardtransactionBase].[cgi_origzone],
    [cgi_travelcardtransactionBase].[cgi_time],
    [cgi_travelcardtransactionBase].[cgi_caseId],
    [cgi_travelcardtransactionBase].[cgi_TravelCard],
    [cgi_travelcardtransactionBase].[cgi_OrigZoneName],
    [cgi_travelcardtransactionBase].[cgi_Amount]
from [cgi_travelcardtransactionBase] 
    left join [cgi_travelcardBase] [cgi_cgi_travelcard_cgi_travelcardtransaction_TravelCardid] on ([cgi_travelcardtransactionBase].[cgi_TravelCardid] = [cgi_cgi_travelcard_cgi_travelcardtransaction_TravelCardid].[cgi_travelcardId])
    left join [IncidentBase] [cgi_incident_cgi_travelcardtransaction_caseId] on ([cgi_travelcardtransactionBase].[cgi_caseId] = [cgi_incident_cgi_travelcardtransaction_caseId].[IncidentId])
    left join [SystemUserBase] [lk_cgi_travelcardtransaction_createdby] with(nolock) on ([cgi_travelcardtransactionBase].[CreatedBy] = [lk_cgi_travelcardtransaction_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtransaction_createdonbehalfby] with(nolock) on ([cgi_travelcardtransactionBase].[CreatedOnBehalfBy] = [lk_cgi_travelcardtransaction_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtransaction_modifiedby] with(nolock) on ([cgi_travelcardtransactionBase].[ModifiedBy] = [lk_cgi_travelcardtransaction_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_travelcardtransaction_modifiedonbehalfby] with(nolock) on ([cgi_travelcardtransactionBase].[ModifiedOnBehalfBy] = [lk_cgi_travelcardtransaction_modifiedonbehalfby].[SystemUserId])
    left join [TransactionCurrencyBase] [TransactionCurrency_cgi_travelcardtransaction] on ([cgi_travelcardtransactionBase].[TransactionCurrencyId] = [TransactionCurrency_cgi_travelcardtransaction].[TransactionCurrencyId])
    left join OwnerBase XXowner with(nolock) on ([cgi_travelcardtransactionBase].OwnerId = XXowner.OwnerId)

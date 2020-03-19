


--
-- base view for cgi_amounttranslation
--
create view dbo.[cgi_amounttranslation]
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
    [TransactionCurrencyIdName],
    [OrganizationIdName],

    -- physical attributes
    [cgi_amounttranslationId],
    [CreatedOn],
    [CreatedBy],
    [ModifiedOn],
    [ModifiedBy],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [OrganizationId],
    [statecode],
    [statuscode],
    [VersionNumber],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [TimeZoneRuleVersionNumber],
    [UTCConversionTimeZoneCode],
    [cgi_name],
    [cgi_Amount],
    [TransactionCurrencyId],
    [ExchangeRate],
    [cgi_amount_Base]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_amounttranslation_createdby].[FullName],
    [lk_cgi_amounttranslation_createdby].[YomiFullName],
    [lk_cgi_amounttranslation_createdonbehalfby].[FullName],
    [lk_cgi_amounttranslation_createdonbehalfby].[YomiFullName],
    [lk_cgi_amounttranslation_modifiedby].[FullName],
    [lk_cgi_amounttranslation_modifiedby].[YomiFullName],
    [lk_cgi_amounttranslation_modifiedonbehalfby].[FullName],
    [lk_cgi_amounttranslation_modifiedonbehalfby].[YomiFullName],
    [TransactionCurrency_cgi_amounttranslation].[CurrencyName],
    [organization_cgi_amounttranslation].[Name],

    -- physical attribute
    [cgi_amounttranslationBase].[cgi_amounttranslationId],
    [cgi_amounttranslationBase].[CreatedOn],
    [cgi_amounttranslationBase].[CreatedBy],
    [cgi_amounttranslationBase].[ModifiedOn],
    [cgi_amounttranslationBase].[ModifiedBy],
    [cgi_amounttranslationBase].[CreatedOnBehalfBy],
    [cgi_amounttranslationBase].[ModifiedOnBehalfBy],
    [cgi_amounttranslationBase].[OrganizationId],
    [cgi_amounttranslationBase].[statecode],
    [cgi_amounttranslationBase].[statuscode],
    [cgi_amounttranslationBase].[VersionNumber],
    [cgi_amounttranslationBase].[ImportSequenceNumber],
    [cgi_amounttranslationBase].[OverriddenCreatedOn],
    [cgi_amounttranslationBase].[TimeZoneRuleVersionNumber],
    [cgi_amounttranslationBase].[UTCConversionTimeZoneCode],
    [cgi_amounttranslationBase].[cgi_name],
    [cgi_amounttranslationBase].[cgi_Amount],
    [cgi_amounttranslationBase].[TransactionCurrencyId],
    [cgi_amounttranslationBase].[ExchangeRate],
    [cgi_amounttranslationBase].[cgi_amount_Base]
from [cgi_amounttranslationBase] 
    left join [SystemUserBase] [lk_cgi_amounttranslation_createdby] with(nolock) on ([cgi_amounttranslationBase].[CreatedBy] = [lk_cgi_amounttranslation_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_amounttranslation_createdonbehalfby] with(nolock) on ([cgi_amounttranslationBase].[CreatedOnBehalfBy] = [lk_cgi_amounttranslation_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_amounttranslation_modifiedby] with(nolock) on ([cgi_amounttranslationBase].[ModifiedBy] = [lk_cgi_amounttranslation_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_amounttranslation_modifiedonbehalfby] with(nolock) on ([cgi_amounttranslationBase].[ModifiedOnBehalfBy] = [lk_cgi_amounttranslation_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_amounttranslation] with(nolock) on ([cgi_amounttranslationBase].[OrganizationId] = [organization_cgi_amounttranslation].[OrganizationId])
    left join [TransactionCurrencyBase] [TransactionCurrency_cgi_amounttranslation] on ([cgi_amounttranslationBase].[TransactionCurrencyId] = [TransactionCurrency_cgi_amounttranslation].[TransactionCurrencyId])

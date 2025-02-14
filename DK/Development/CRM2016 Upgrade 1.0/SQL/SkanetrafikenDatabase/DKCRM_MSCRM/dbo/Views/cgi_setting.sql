﻿


--
-- base view for cgi_setting
--
create view dbo.[cgi_setting]
 (
    -- logical attributes
    [cgi_DefaultCustomerOnCaseName],
    [CreatedByName],
    [CreatedByYomiName],
    [CreatedOnBehalfByName],
    [CreatedOnBehalfByYomiName],
    [ModifiedByName],
    [ModifiedByYomiName],
    [ModifiedOnBehalfByName],
    [ModifiedOnBehalfByYomiName],
    [cgi_DefaultoutgoingemailqueueName],
    [TransactionCurrencyIdName],
    [cgi_category_detail2idName],
    [cgi_DefaultTeamonPASSCaseName],
    [cgi_useridName],
    [cgi_DefaultCustomerOnCaseYomiName],
    [cgi_case_rgol_defaultownerYomiName],
    [OrganizationIdName],
    [cgi_case_rgol_defaultownerName],
    [cgi_useridYomiName],
    [cgi_DefaultTeamonPASSCaseYomiName],
    [cgi_category_detail1idName],
    [cgi_category_detail3idName],
    [cgi_case_rgol_defaultqueueName],
    [cgi_refundtypeproductnotrequiredidName],

    -- physical attributes
    [cgi_settingId],
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
    [cgi_AttentionEmailTemplate],
    [cgi_Data],
    [cgi_RemitanceEmailTemplate],
    [cgi_ValidFrom],
    [cgi_ValidTo],
    [cgi_DefaultCustomerOnCase],
    [cgi_Defaultoutgoingemailqueue],
    [cgi_BIFFConnectorService],
    [cgi_EtisService],
    [cgi_PubTransService],
    [cgi_ExtConnectorService],
    [TransactionCurrencyId],
    [ExchangeRate],
    [cgi_milage_contribution],
    [cgi_milage_contribution_Base],
    [cgi_amount_limit],
    [cgi_amount_limit_Base],
    [cgi_BOMBUrl],
    [cgi_CubicService],
    [cgi_CRMCardService],
    [cgi_GiftcardService],
    [cgi_EhandelOrderService],
    [cgi_RaindancePrefix],
    [cgi_OrganizationPrefix],
    [cgi_recipient_entity_id],
    [cgi_chargeorderservice],
    [cgi_createcouponservice],
    [cgi_getoutstandingchargesservice],
    [cgi_rechargecardservice],
    [cgi_sendvaluecodemailservice],
    [cgi_ValueCodeValidForMonths],
    [cgi_amount_limit_warn],
    [cgi_amount_limit_warn_Base],
    [cgi_DefaultTeamonPASSCase],
    [cgi_userid],
    [cgi_SyncCustomerCardService],
    [cgi_createemailcouponservice],
    [cgi_CancelGiftCodeService],
    [cgi_reinvoicingphonenumber],
    [cgi_CRMUri],
    [cgi_synccustomerservice],
    [cgi_category_detail3id],
    [cgi_category_detail2id],
    [cgi_category_detail1id],
    [cgi_refundtypeproductnotrequiredid],
    [cgi_case_rgol_defaultowner],
    [cgi_case_rgol_defaultqueue]
) with view_metadata as
select
    -- logical attributes
    [cgi_account_cgi_setting_DefaultCustomerOnCase].[Name],
    [lk_cgi_setting_createdby].[FullName],
    [lk_cgi_setting_createdby].[YomiFullName],
    [lk_cgi_setting_createdonbehalfby].[FullName],
    [lk_cgi_setting_createdonbehalfby].[YomiFullName],
    [lk_cgi_setting_modifiedby].[FullName],
    [lk_cgi_setting_modifiedby].[YomiFullName],
    [lk_cgi_setting_modifiedonbehalfby].[FullName],
    [lk_cgi_setting_modifiedonbehalfby].[YomiFullName],
    [cgi_queue_cgi_setting_Defaultoutgoingemailqueue].[Name],
    [TransactionCurrency_cgi_setting].[CurrencyName],
    [cgi_cgi_categorydetail2_cgi_setting].[cgi_categorydetailname],
    [cgi_team_cgi_setting_DefaultTeamonPASSCase].[Name],
    [cgi_systemuser_setting].[FullName],
    [cgi_account_cgi_setting_DefaultCustomerOnCase].[YomiName],
    [cgi_systemuser_cgi_setting_case_rgol_defaultowner].[YomiFullName],
    [organization_cgi_setting].[Name],
    [cgi_systemuser_cgi_setting_case_rgol_defaultowner].[FullName],
    [cgi_systemuser_setting].[YomiFullName],
    [cgi_team_cgi_setting_DefaultTeamonPASSCase].[YomiName],
    [cgi_categorydetail1_cgi_setting].[cgi_categorydetailname],
    [cgi_categorydetail_cgi_setting].[cgi_categorydetailname],
    [cgi_queue_cgi_setting_case_rgol_defaultqueue].[Name],
    [cgi_cgi_refundtype_cgi_setting].[cgi_refundtypename],

    -- physical attribute
    [cgi_settingBase].[cgi_settingId],
    [cgi_settingBase].[CreatedOn],
    [cgi_settingBase].[CreatedBy],
    [cgi_settingBase].[ModifiedOn],
    [cgi_settingBase].[ModifiedBy],
    [cgi_settingBase].[CreatedOnBehalfBy],
    [cgi_settingBase].[ModifiedOnBehalfBy],
    [cgi_settingBase].[OrganizationId],
    [cgi_settingBase].[statecode],
    [cgi_settingBase].[statuscode],
    [cgi_settingBase].[VersionNumber],
    [cgi_settingBase].[ImportSequenceNumber],
    [cgi_settingBase].[OverriddenCreatedOn],
    [cgi_settingBase].[TimeZoneRuleVersionNumber],
    [cgi_settingBase].[UTCConversionTimeZoneCode],
    [cgi_settingBase].[cgi_name],
    [cgi_settingBase].[cgi_AttentionEmailTemplate],
    [cgi_settingBase].[cgi_Data],
    [cgi_settingBase].[cgi_RemitanceEmailTemplate],
    [cgi_settingBase].[cgi_ValidFrom],
    [cgi_settingBase].[cgi_ValidTo],
    [cgi_settingBase].[cgi_DefaultCustomerOnCase],
    [cgi_settingBase].[cgi_Defaultoutgoingemailqueue],
    [cgi_settingBase].[cgi_BIFFConnectorService],
    [cgi_settingBase].[cgi_EtisService],
    [cgi_settingBase].[cgi_PubTransService],
    [cgi_settingBase].[cgi_ExtConnectorService],
    [cgi_settingBase].[TransactionCurrencyId],
    [cgi_settingBase].[ExchangeRate],
    [cgi_settingBase].[cgi_milage_contribution],
    [cgi_settingBase].[cgi_milage_contribution_Base],
    [cgi_settingBase].[cgi_amount_limit],
    [cgi_settingBase].[cgi_amount_limit_Base],
    [cgi_settingBase].[cgi_BOMBUrl],
    [cgi_settingBase].[cgi_CubicService],
    [cgi_settingBase].[cgi_CRMCardService],
    [cgi_settingBase].[cgi_GiftcardService],
    [cgi_settingBase].[cgi_EhandelOrderService],
    [cgi_settingBase].[cgi_RaindancePrefix],
    [cgi_settingBase].[cgi_OrganizationPrefix],
    [cgi_settingBase].[cgi_recipient_entity_id],
    [cgi_settingBase].[cgi_chargeorderservice],
    [cgi_settingBase].[cgi_createcouponservice],
    [cgi_settingBase].[cgi_getoutstandingchargesservice],
    [cgi_settingBase].[cgi_rechargecardservice],
    [cgi_settingBase].[cgi_sendvaluecodemailservice],
    [cgi_settingBase].[cgi_ValueCodeValidForMonths],
    [cgi_settingBase].[cgi_amount_limit_warn],
    [cgi_settingBase].[cgi_amount_limit_warn_Base],
    [cgi_settingBase].[cgi_DefaultTeamonPASSCase],
    [cgi_settingBase].[cgi_userid],
    [cgi_settingBase].[cgi_SyncCustomerCardService],
    [cgi_settingBase].[cgi_createemailcouponservice],
    [cgi_settingBase].[cgi_CancelGiftCodeService],
    [cgi_settingBase].[cgi_reinvoicingphonenumber],
    [cgi_settingBase].[cgi_CRMUri],
    [cgi_settingBase].[cgi_synccustomerservice],
    [cgi_settingBase].[cgi_category_detail3id],
    [cgi_settingBase].[cgi_category_detail2id],
    [cgi_settingBase].[cgi_category_detail1id],
    [cgi_settingBase].[cgi_refundtypeproductnotrequiredid],
    [cgi_settingBase].[cgi_case_rgol_defaultowner],
    [cgi_settingBase].[cgi_case_rgol_defaultqueue]
from [cgi_settingBase] 
    left join [AccountBase] [cgi_account_cgi_setting_DefaultCustomerOnCase] on ([cgi_settingBase].[cgi_DefaultCustomerOnCase] = [cgi_account_cgi_setting_DefaultCustomerOnCase].[AccountId])
    left join [cgi_categorydetailBase] [cgi_categorydetail_cgi_setting] on ([cgi_settingBase].[cgi_category_detail3id] = [cgi_categorydetail_cgi_setting].[cgi_categorydetailId])
    left join [cgi_categorydetailBase] [cgi_categorydetail1_cgi_setting] on ([cgi_settingBase].[cgi_category_detail1id] = [cgi_categorydetail1_cgi_setting].[cgi_categorydetailId])
    left join [cgi_categorydetailBase] [cgi_cgi_categorydetail2_cgi_setting] on ([cgi_settingBase].[cgi_category_detail2id] = [cgi_cgi_categorydetail2_cgi_setting].[cgi_categorydetailId])
    left join [cgi_refundtypeBase] [cgi_cgi_refundtype_cgi_setting] on ([cgi_settingBase].[cgi_refundtypeproductnotrequiredid] = [cgi_cgi_refundtype_cgi_setting].[cgi_refundtypeId])
    left join [QueueBase] [cgi_queue_cgi_setting_case_rgol_defaultqueue] on ([cgi_settingBase].[cgi_case_rgol_defaultqueue] = [cgi_queue_cgi_setting_case_rgol_defaultqueue].[QueueId])
    left join [QueueBase] [cgi_queue_cgi_setting_Defaultoutgoingemailqueue] on ([cgi_settingBase].[cgi_Defaultoutgoingemailqueue] = [cgi_queue_cgi_setting_Defaultoutgoingemailqueue].[QueueId])
    left join [SystemUserBase] [cgi_systemuser_cgi_setting_case_rgol_defaultowner] with(nolock) on ([cgi_settingBase].[cgi_case_rgol_defaultowner] = [cgi_systemuser_cgi_setting_case_rgol_defaultowner].[SystemUserId])
    left join [SystemUserBase] [cgi_systemuser_setting] with(nolock) on ([cgi_settingBase].[cgi_userid] = [cgi_systemuser_setting].[SystemUserId])
    left join [TeamBase] [cgi_team_cgi_setting_DefaultTeamonPASSCase] on ([cgi_settingBase].[cgi_DefaultTeamonPASSCase] = [cgi_team_cgi_setting_DefaultTeamonPASSCase].[TeamId])
    left join [SystemUserBase] [lk_cgi_setting_createdby] with(nolock) on ([cgi_settingBase].[CreatedBy] = [lk_cgi_setting_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_setting_createdonbehalfby] with(nolock) on ([cgi_settingBase].[CreatedOnBehalfBy] = [lk_cgi_setting_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_setting_modifiedby] with(nolock) on ([cgi_settingBase].[ModifiedBy] = [lk_cgi_setting_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_setting_modifiedonbehalfby] with(nolock) on ([cgi_settingBase].[ModifiedOnBehalfBy] = [lk_cgi_setting_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_setting] with(nolock) on ([cgi_settingBase].[OrganizationId] = [organization_cgi_setting].[OrganizationId])
    left join [TransactionCurrencyBase] [TransactionCurrency_cgi_setting] on ([cgi_settingBase].[TransactionCurrencyId] = [TransactionCurrency_cgi_setting].[TransactionCurrencyId])

﻿


--
-- base view for Campaign
--
create view dbo.[Campaign]
 (
    -- logical attributes
    [ModifiedOnBehalfByYomiName],
    [TransactionCurrencyIdName],
    [ModifiedOnBehalfByName],
    [EntityImage_Timestamp],
    [PriceListName],
    [CreatedByName],
    [cgi_trafficcontractIdName],
    [ModifiedByName],
    [CreatedByYomiName],
    [EntityImage_URL],
    [ModifiedByYomiName],
    [EntityImage],
    [CreatedOnBehalfByYomiName],
    [CreatedOnBehalfByName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [TypeCode],
    [ProposedEnd],
    [BudgetedCost],
    [CreatedOn],
    [PromotionCodeName],
    [ModifiedOn],
    [PriceListId],
    [StatusCode],
    [CreatedBy],
    [IsTemplate],
    [CampaignId],
    [ActualStart],
    [OwningBusinessUnit],
    [TotalActualCost],
    [Message],
    [ModifiedBy],
    [ExpectedRevenue],
    [VersionNumber],
    [CodeName],
    [ProposedStart],
    [Objective],
    [ActualEnd],
    [StateCode],
    [OtherCost],
    [Description],
    [TotalCampaignActivityActualCost],
    [ExpectedResponse],
    [Name],
    [ExchangeRate],
    [TimeZoneRuleVersionNumber],
    [TransactionCurrencyId],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [UTCConversionTimeZoneCode],
    [TotalCampaignActivityActualCost_Base],
    [BudgetedCost_Base],
    [ExpectedRevenue_Base],
    [OtherCost_Base],
    [TotalActualCost_Base],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [ProcessId],
    [StageId],
    [EntityImageId],
    [cgi_marketarea],
    [cgi_strategy],
    [cgi_assignment_agency],
    [cgi_objective],
    [cgi_segment],
    [cgi_targetgroup],
    [cgi_communication_objective],
    [cgi_odr],
    [cgi_adr],
    [cgi_advertising_pr],
    [cgi_advertising_di],
    [cgi_outdoor_advertising],
    [cgi_vehicle_advertising],
    [cgi_social_media],
    [cgi_station_advertising],
    [cgi_infotainment],
    [cgi_customercenter],
    [cgi_event],
    [cgi_tv],
    [cgi_cinema],
    [cgi_radio],
    [cgi_objective_meet],
    [cgi_outcome_budget],
    [cgi_outcome_budget_Base],
    [cgi_evaluation],
    [cgi_recommendations],
    [cgi_newsletter],
    [cgi_trafficcontractId]
) with view_metadata as
select
    -- logical attributes
    [lk_campaign_modifiedonbehalfby].[YomiFullName],
    [transactioncurrency_campaign].[CurrencyName],
    [lk_campaign_modifiedonbehalfby].[FullName],
    [lk_campaign_entityimage].[ImageTimestamp],
    [PriceList_Campaigns].[Name],
    [lk_campaign_createdby].[FullName],
    [cgi_traffic_contract_campaign].[cgi_name],
    [lk_campaign_modifiedby].[FullName],
    [lk_campaign_createdby].[YomiFullName],
    [lk_campaign_entityimage].[ImageURL],
    [lk_campaign_modifiedby].[YomiFullName],
    [lk_campaign_entityimage].[ImageData],
    [lk_campaign_createdonbehalfby].[YomiFullName],
    [lk_campaign_createdonbehalfby].[FullName],

    -- ownership entries
    OwnerId = [CampaignBase].OwnerId,
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
    [CampaignBase].[TypeCode],
    [CampaignBase].[ProposedEnd],
    [CampaignBase].[BudgetedCost],
    [CampaignBase].[CreatedOn],
    [CampaignBase].[PromotionCodeName],
    [CampaignBase].[ModifiedOn],
    [CampaignBase].[PriceListId],
    [CampaignBase].[StatusCode],
    [CampaignBase].[CreatedBy],
    [CampaignBase].[IsTemplate],
    [CampaignBase].[CampaignId],
    [CampaignBase].[ActualStart],
    [CampaignBase].[OwningBusinessUnit],
    [CampaignBase].[TotalActualCost],
    [CampaignBase].[Message],
    [CampaignBase].[ModifiedBy],
    [CampaignBase].[ExpectedRevenue],
    [CampaignBase].[VersionNumber],
    [CampaignBase].[CodeName],
    [CampaignBase].[ProposedStart],
    [CampaignBase].[Objective],
    [CampaignBase].[ActualEnd],
    [CampaignBase].[StateCode],
    [CampaignBase].[OtherCost],
    [CampaignBase].[Description],
    [CampaignBase].[TotalCampaignActivityActualCost],
    [CampaignBase].[ExpectedResponse],
    [CampaignBase].[Name],
    [CampaignBase].[ExchangeRate],
    [CampaignBase].[TimeZoneRuleVersionNumber],
    [CampaignBase].[TransactionCurrencyId],
    [CampaignBase].[ImportSequenceNumber],
    [CampaignBase].[OverriddenCreatedOn],
    [CampaignBase].[UTCConversionTimeZoneCode],
    [CampaignBase].[TotalCampaignActivityActualCost_Base],
    [CampaignBase].[BudgetedCost_Base],
    [CampaignBase].[ExpectedRevenue_Base],
    [CampaignBase].[OtherCost_Base],
    [CampaignBase].[TotalActualCost_Base],
    [CampaignBase].[CreatedOnBehalfBy],
    [CampaignBase].[ModifiedOnBehalfBy],
    [CampaignBase].[ProcessId],
    [CampaignBase].[StageId],
    [CampaignBase].[EntityImageId],
    [CampaignBase].[cgi_marketarea],
    [CampaignBase].[cgi_strategy],
    [CampaignBase].[cgi_assignment_agency],
    [CampaignBase].[cgi_objective],
    [CampaignBase].[cgi_segment],
    [CampaignBase].[cgi_targetgroup],
    [CampaignBase].[cgi_communication_objective],
    [CampaignBase].[cgi_odr],
    [CampaignBase].[cgi_adr],
    [CampaignBase].[cgi_advertising_pr],
    [CampaignBase].[cgi_advertising_di],
    [CampaignBase].[cgi_outdoor_advertising],
    [CampaignBase].[cgi_vehicle_advertising],
    [CampaignBase].[cgi_social_media],
    [CampaignBase].[cgi_station_advertising],
    [CampaignBase].[cgi_infotainment],
    [CampaignBase].[cgi_customercenter],
    [CampaignBase].[cgi_event],
    [CampaignBase].[cgi_tv],
    [CampaignBase].[cgi_cinema],
    [CampaignBase].[cgi_radio],
    [CampaignBase].[cgi_objective_meet],
    [CampaignBase].[cgi_outcome_budget],
    [CampaignBase].[cgi_outcome_budget_Base],
    [CampaignBase].[cgi_evaluation],
    [CampaignBase].[cgi_recommendations],
    [CampaignBase].[cgi_newsletter],
    [CampaignBase].[cgi_trafficcontractId]
from [CampaignBase] 
    left join [cgi_traffic_contractBase] [cgi_traffic_contract_campaign] on ([CampaignBase].[cgi_trafficcontractId] = [cgi_traffic_contract_campaign].[cgi_traffic_contractId])
    left join [SystemUserBase] [lk_campaign_createdby] with(nolock) on ([CampaignBase].[CreatedBy] = [lk_campaign_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_campaign_createdonbehalfby] with(nolock) on ([CampaignBase].[CreatedOnBehalfBy] = [lk_campaign_createdonbehalfby].[SystemUserId])
    left join [ImageDescriptor] [lk_campaign_entityimage] on ([CampaignBase].[EntityImageId] = [lk_campaign_entityimage].[ImageDescriptorId])
    left join [SystemUserBase] [lk_campaign_modifiedby] with(nolock) on ([CampaignBase].[ModifiedBy] = [lk_campaign_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_campaign_modifiedonbehalfby] with(nolock) on ([CampaignBase].[ModifiedOnBehalfBy] = [lk_campaign_modifiedonbehalfby].[SystemUserId])
    left join [PriceLevelBase] [PriceList_Campaigns] on ([CampaignBase].[PriceListId] = [PriceList_Campaigns].[PriceLevelId])
    left join [TransactionCurrencyBase] [transactioncurrency_campaign] on ([CampaignBase].[TransactionCurrencyId] = [transactioncurrency_campaign].[TransactionCurrencyId])
    left join OwnerBase XXowner with(nolock) on ([CampaignBase].OwnerId = XXowner.OwnerId)

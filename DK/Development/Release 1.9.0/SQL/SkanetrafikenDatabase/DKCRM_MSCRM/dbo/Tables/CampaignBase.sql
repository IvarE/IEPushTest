CREATE TABLE [dbo].[CampaignBase] (
    [TypeCode]                             INT              NULL,
    [ProposedEnd]                          DATETIME         NULL,
    [BudgetedCost]                         MONEY            CONSTRAINT [DF_CampaignBase_BudgetedCost] DEFAULT ((0)) NULL,
    [CreatedOn]                            DATETIME         NULL,
    [PromotionCodeName]                    NVARCHAR (128)   NULL,
    [ModifiedOn]                           DATETIME         NULL,
    [PriceListId]                          UNIQUEIDENTIFIER NULL,
    [StatusCode]                           INT              NULL,
    [CreatedBy]                            UNIQUEIDENTIFIER NULL,
    [IsTemplate]                           BIT              CONSTRAINT [DF_CampaignBase_IsTemplate] DEFAULT ((0)) NULL,
    [CampaignId]                           UNIQUEIDENTIFIER NOT NULL,
    [ActualStart]                          DATETIME         NULL,
    [OwningBusinessUnit]                   UNIQUEIDENTIFIER NULL,
    [TotalActualCost]                      MONEY            CONSTRAINT [DF_CampaignBase_TotalActualCost] DEFAULT ((0)) NULL,
    [Message]                              NVARCHAR (256)   NULL,
    [ModifiedBy]                           UNIQUEIDENTIFIER NULL,
    [ExpectedRevenue]                      MONEY            CONSTRAINT [DF_CampaignBase_ExpectedRevenue] DEFAULT ((0)) NULL,
    [VersionNumber]                        ROWVERSION       NULL,
    [CodeName]                             NVARCHAR (32)    NULL,
    [ProposedStart]                        DATETIME         NULL,
    [Objective]                            NVARCHAR (MAX)   NULL,
    [ActualEnd]                            DATETIME         NULL,
    [StateCode]                            INT              CONSTRAINT [DF_CampaignBase_StateCode] DEFAULT ((0)) NOT NULL,
    [OtherCost]                            MONEY            CONSTRAINT [DF_CampaignBase_OtherCost] DEFAULT ((0)) NULL,
    [Description]                          NVARCHAR (MAX)   NULL,
    [TotalCampaignActivityActualCost]      MONEY            CONSTRAINT [DF_CampaignBase_TotalCampaignActivityActualCost] DEFAULT ((0)) NULL,
    [ExpectedResponse]                     INT              NULL,
    [Name]                                 NVARCHAR (128)   NOT NULL,
    [ExchangeRate]                         DECIMAL (23, 10) NULL,
    [TimeZoneRuleVersionNumber]            INT              NULL,
    [TransactionCurrencyId]                UNIQUEIDENTIFIER NULL,
    [ImportSequenceNumber]                 INT              NULL,
    [OverriddenCreatedOn]                  DATETIME         NULL,
    [UTCConversionTimeZoneCode]            INT              NULL,
    [TotalCampaignActivityActualCost_Base] MONEY            NULL,
    [BudgetedCost_Base]                    MONEY            NULL,
    [ExpectedRevenue_Base]                 MONEY            NULL,
    [OtherCost_Base]                       MONEY            NULL,
    [TotalActualCost_Base]                 MONEY            NULL,
    [OwnerId]                              UNIQUEIDENTIFIER CONSTRAINT [DF_CampaignBase_OwnerId] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedOnBehalfBy]                   UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]                    UNIQUEIDENTIFIER NULL,
    [OwnerIdType]                          INT              CONSTRAINT [DF_CampaignBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [StageId]                              UNIQUEIDENTIFIER NULL,
    [ProcessId]                            UNIQUEIDENTIFIER NULL,
    [EntityImageId]                        UNIQUEIDENTIFIER NULL,
    [cgi_marketarea]                       INT              NULL,
    [cgi_strategy]                         INT              NULL,
    [cgi_assignment_agency]                NVARCHAR (MAX)   NULL,
    [cgi_objective]                        NVARCHAR (MAX)   NULL,
    [cgi_segment]                          INT              NULL,
    [cgi_targetgroup]                      NVARCHAR (MAX)   NULL,
    [cgi_communication_objective]          NVARCHAR (MAX)   NULL,
    [cgi_odr]                              BIT              NULL,
    [cgi_adr]                              BIT              NULL,
    [cgi_advertising_pr]                   BIT              NULL,
    [cgi_advertising_di]                   BIT              NULL,
    [cgi_outdoor_advertising]              BIT              NULL,
    [cgi_vehicle_advertising]              BIT              NULL,
    [cgi_social_media]                     BIT              NULL,
    [cgi_station_advertising]              BIT              NULL,
    [cgi_infotainment]                     BIT              NULL,
    [cgi_customercenter]                   BIT              NULL,
    [cgi_event]                            BIT              NULL,
    [cgi_tv]                               BIT              NULL,
    [cgi_cinema]                           BIT              NULL,
    [cgi_radio]                            BIT              NULL,
    [cgi_objective_meet]                   BIT              NULL,
    [cgi_outcome_budget]                   MONEY            NULL,
    [cgi_outcome_budget_Base]              MONEY            NULL,
    [cgi_evaluation]                       NVARCHAR (MAX)   NULL,
    [cgi_recommendations]                  NVARCHAR (MAX)   NULL,
    [cgi_newsletter]                       BIT              NULL,
    [cgi_trafficcontractId]                UNIQUEIDENTIFIER NULL,
    CONSTRAINT [cndx_PrimaryKey_Campaign] PRIMARY KEY CLUSTERED ([CampaignId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [BusinessUnit_Campaigns] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_traffic_contract_campaign] FOREIGN KEY ([cgi_trafficcontractId]) REFERENCES [dbo].[cgi_traffic_contractBase] ([cgi_traffic_contractId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_campaigns] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION,
    CONSTRAINT [PriceList_Campaigns] FOREIGN KEY ([PriceListId]) REFERENCES [dbo].[PriceLevelBase] ([PriceLevelId]) NOT FOR REPLICATION,
    CONSTRAINT [transactioncurrency_campaign] FOREIGN KEY ([TransactionCurrencyId]) REFERENCES [dbo].[TransactionCurrencyBase] ([TransactionCurrencyId]) NOT FOR REPLICATION
);


GO
EXECUTE sp_tableoption @TableNamePattern = N'[dbo].[CampaignBase]', @OptionName = N'text in row', @OptionValue = N'7000';


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[CampaignBase]([StateCode] ASC, [StatusCode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_BusinessUnit_Campaigns]
    ON [dbo].[CampaignBase]([OwningBusinessUnit] ASC) WHERE ([OwningBusinessUnit] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [fndx_Sync_VersionNumber]
    ON [dbo].[CampaignBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[CampaignBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_PriceList_Campaigns]
    ON [dbo].[CampaignBase]([PriceListId] ASC) WHERE ([PriceListId] IS NOT NULL) WITH (FILLFACTOR = 80);


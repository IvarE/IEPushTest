CREATE TABLE [dbo].[cgi_settingBase] (
    [cgi_settingId]                      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                          DATETIME         NULL,
    [CreatedBy]                          UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                         DATETIME         NULL,
    [ModifiedBy]                         UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]                  UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]                 UNIQUEIDENTIFIER NULL,
    [OrganizationId]                     UNIQUEIDENTIFIER NULL,
    [statecode]                          INT              NOT NULL,
    [statuscode]                         INT              NULL,
    [VersionNumber]                      ROWVERSION       NULL,
    [ImportSequenceNumber]               INT              NULL,
    [OverriddenCreatedOn]                DATETIME         NULL,
    [TimeZoneRuleVersionNumber]          INT              NULL,
    [UTCConversionTimeZoneCode]          INT              NULL,
    [cgi_name]                           NVARCHAR (100)   NULL,
    [cgi_AttentionEmailTemplate]         NVARCHAR (100)   NULL,
    [cgi_Data]                           NVARCHAR (MAX)   NULL,
    [cgi_RemitanceEmailTemplate]         NVARCHAR (100)   NULL,
    [cgi_ValidFrom]                      DATETIME         NULL,
    [cgi_ValidTo]                        DATETIME         NULL,
    [cgi_DefaultCustomerOnCase]          UNIQUEIDENTIFIER NULL,
    [cgi_Defaultoutgoingemailqueue]      UNIQUEIDENTIFIER NULL,
    [cgi_BIFFConnectorService]           NVARCHAR (100)   NULL,
    [cgi_EtisService]                    NVARCHAR (100)   NULL,
    [cgi_PubTransService]                NVARCHAR (100)   NULL,
    [cgi_ExtConnectorService]            NVARCHAR (100)   NULL,
    [TransactionCurrencyId]              UNIQUEIDENTIFIER NULL,
    [ExchangeRate]                       DECIMAL (23, 10) NULL,
    [cgi_milage_contribution]            MONEY            NULL,
    [cgi_milage_contribution_Base]       MONEY            NULL,
    [cgi_amount_limit]                   MONEY            NULL,
    [cgi_amount_limit_Base]              MONEY            NULL,
    [cgi_BOMBUrl]                        NVARCHAR (100)   NULL,
    [cgi_CubicService]                   NVARCHAR (100)   NULL,
    [cgi_CRMCardService]                 NVARCHAR (100)   NULL,
    [cgi_GiftcardService]                NVARCHAR (100)   NULL,
    [cgi_EhandelOrderService]            NVARCHAR (100)   NULL,
    [cgi_RaindancePrefix]                NVARCHAR (100)   NULL,
    [cgi_OrganizationPrefix]             NVARCHAR (100)   NULL,
    [cgi_recipient_entity_id]            NVARCHAR (100)   NULL,
    [cgi_chargeorderservice]             NVARCHAR (100)   NULL,
    [cgi_createcouponservice]            NVARCHAR (100)   NULL,
    [cgi_getoutstandingchargesservice]   NVARCHAR (100)   NULL,
    [cgi_rechargecardservice]            NVARCHAR (100)   NULL,
    [cgi_sendvaluecodemailservice]       NVARCHAR (100)   NULL,
    [cgi_ValueCodeValidForMonths]        INT              NULL,
    [cgi_amount_limit_warn]              MONEY            NULL,
    [cgi_amount_limit_warn_Base]         MONEY            NULL,
    [cgi_DefaultTeamonPASSCase]          UNIQUEIDENTIFIER NULL,
    [cgi_userid]                         UNIQUEIDENTIFIER NULL,
    [cgi_SyncCustomerCardService]        NVARCHAR (100)   NULL,
    [cgi_createemailcouponservice]       NVARCHAR (100)   NULL,
    [cgi_CancelGiftCodeService]          NVARCHAR (200)   NULL,
    [cgi_reinvoicingphonenumber]         NVARCHAR (100)   NULL,
    [cgi_CRMUri]                         NVARCHAR (100)   NULL,
    [cgi_synccustomerservice]            NVARCHAR (100)   NULL,
    [cgi_category_detail3id]             UNIQUEIDENTIFIER NULL,
    [cgi_category_detail2id]             UNIQUEIDENTIFIER NULL,
    [cgi_category_detail1id]             UNIQUEIDENTIFIER NULL,
    [cgi_refundtypeproductnotrequiredid] UNIQUEIDENTIFIER NULL,
    [cgi_case_rgol_defaultowner]         UNIQUEIDENTIFIER NULL,
    [cgi_case_rgol_defaultqueue]         UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_settingBase] PRIMARY KEY CLUSTERED ([cgi_settingId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_account_cgi_setting_DefaultCustomerOnCase] FOREIGN KEY ([cgi_DefaultCustomerOnCase]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_categorydetail_cgi_setting] FOREIGN KEY ([cgi_category_detail3id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_categorydetail1_cgi_setting] FOREIGN KEY ([cgi_category_detail1id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail2_cgi_setting] FOREIGN KEY ([cgi_category_detail2id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_refundtype_cgi_setting] FOREIGN KEY ([cgi_refundtypeproductnotrequiredid]) REFERENCES [dbo].[cgi_refundtypeBase] ([cgi_refundtypeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_queue_cgi_setting_case_rgol_defaultqueue] FOREIGN KEY ([cgi_case_rgol_defaultqueue]) REFERENCES [dbo].[QueueBase] ([QueueId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_queue_cgi_setting_Defaultoutgoingemailqueue] FOREIGN KEY ([cgi_Defaultoutgoingemailqueue]) REFERENCES [dbo].[QueueBase] ([QueueId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_systemuser_cgi_setting_case_rgol_defaultowner] FOREIGN KEY ([cgi_case_rgol_defaultowner]) REFERENCES [dbo].[SystemUserBase] ([SystemUserId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_systemuser_setting] FOREIGN KEY ([cgi_userid]) REFERENCES [dbo].[SystemUserBase] ([SystemUserId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_team_cgi_setting_DefaultTeamonPASSCase] FOREIGN KEY ([cgi_DefaultTeamonPASSCase]) REFERENCES [dbo].[TeamBase] ([TeamId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_settingBase]([OrganizationId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_settingBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_settingBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_name]
    ON [dbo].[cgi_settingBase]([cgi_name] ASC) WITH (FILLFACTOR = 80);


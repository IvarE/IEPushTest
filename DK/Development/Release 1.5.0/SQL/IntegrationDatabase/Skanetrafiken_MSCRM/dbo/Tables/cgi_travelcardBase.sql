CREATE TABLE [dbo].[cgi_travelcardBase] (
    [cgi_travelcardId]                UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                       DATETIME         NULL,
    [CreatedBy]                       UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                      DATETIME         NULL,
    [ModifiedBy]                      UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]               UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]              UNIQUEIDENTIFIER NULL,
    [OwnerId]                         UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]                     INT              CONSTRAINT [DF_cgi_travelcardBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]              UNIQUEIDENTIFIER NULL,
    [statecode]                       INT              NOT NULL,
    [statuscode]                      INT              NULL,
    [VersionNumber]                   ROWVERSION       NULL,
    [ImportSequenceNumber]            INT              NULL,
    [OverriddenCreatedOn]             DATETIME         NULL,
    [TimeZoneRuleVersionNumber]       INT              NULL,
    [UTCConversionTimeZoneCode]       INT              NULL,
    [cgi_travelcardnumber]            NVARCHAR (100)   NULL,
    [cgi_Blocked]                     BIT              NULL,
    [cgi_NumberofZones]               NVARCHAR (10)    NULL,
    [cgi_TravelCardCVC]               NVARCHAR (10)    NULL,
    [cgi_TravelCardName]              NVARCHAR (100)   NULL,
    [cgi_ValidFrom]                   DATETIME         NULL,
    [cgi_ValidTo]                     DATETIME         NULL,
    [cgi_Accountid]                   UNIQUEIDENTIFIER NULL,
    [cgi_CardTypeid]                  UNIQUEIDENTIFIER NULL,
    [cgi_ImportID]                    NVARCHAR (100)   NULL,
    [cgi_periodic_card_type]          NVARCHAR (100)   NULL,
    [cgi_value_card_type]             NVARCHAR (100)   NULL,
    [cgi_Contactid]                   UNIQUEIDENTIFIER NULL,
    [cgi_AutoloadConnectionDate]      DATETIME         NULL,
    [cgi_AutoloadDisconnectionDate]   DATETIME         NULL,
    [cgi_PeriodCardTypeId]            INT              NULL,
    [cgi_ValueCardTypeId]             INT              NULL,
    [cgi_LatestChargeDate]            DATETIME         NULL,
    [cgi_LatestAutoloadAmount]        MONEY            NULL,
    [TransactionCurrencyId]           UNIQUEIDENTIFIER NULL,
    [ExchangeRate]                    DECIMAL (23, 10) NULL,
    [cgi_latestautoloadamount_Base]   MONEY            NULL,
    [cgi_FailedAttemptsToChargeMoney] INT              NULL,
    [cgi_CreditCardMask]              NVARCHAR (100)   NULL,
    [cgi_VerifyId]                    NVARCHAR (100)   NULL,
    [cgi_Currency]                    NVARCHAR (100)   NULL,
    [cgi_CardCategory]                INT              NULL,
    [cgi_LatestFailedAttempt]         DATETIME         NULL,
    [cgi_AutoloadStatus]              INT              NULL,
    [st_OP_Offer]                     BIT              NULL,
    [st_OP_Offer_Date]                DATETIME         NULL,
    [st_OP_Offer_Code]                NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_travelcardBase] PRIMARY KEY CLUSTERED ([cgi_travelcardId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_travelcard] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_account_cgi_travelcard_Account] FOREIGN KEY ([cgi_Accountid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_travelcardtype_cgi_travelcard_CardTypeid] FOREIGN KEY ([cgi_CardTypeid]) REFERENCES [dbo].[cgi_travelcardtypeBase] ([cgi_travelcardtypeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_contact_cgi_travelcard_Contactid] FOREIGN KEY ([cgi_Contactid]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_travelcard] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_travelcardBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_travelcardBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_travelcardBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_travelcardnumber]
    ON [dbo].[cgi_travelcardBase]([cgi_travelcardnumber] ASC) WITH (FILLFACTOR = 80);


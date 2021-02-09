CREATE TABLE [dbo].[cgi_travelcardtransactionBase] (
    [cgi_travelcardtransactionId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                   DATETIME         NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                  DATETIME         NULL,
    [ModifiedBy]                  UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]           UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]          UNIQUEIDENTIFIER NULL,
    [OwnerId]                     UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]                 INT              CONSTRAINT [DF_cgi_travelcardtransactionBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]          UNIQUEIDENTIFIER NULL,
    [statecode]                   INT              NOT NULL,
    [statuscode]                  INT              NULL,
    [VersionNumber]               ROWVERSION       NULL,
    [ImportSequenceNumber]        INT              NULL,
    [OverriddenCreatedOn]         DATETIME         NULL,
    [TimeZoneRuleVersionNumber]   INT              NULL,
    [UTCConversionTimeZoneCode]   INT              NULL,
    [cgi_travelcardtransaction]   NVARCHAR (100)   NULL,
    [TransactionCurrencyId]       UNIQUEIDENTIFIER NULL,
    [ExchangeRate]                DECIMAL (23, 10) NULL,
    [cgi_TravelCardid]            UNIQUEIDENTIFIER NULL,
    [cgi_date]                    NVARCHAR (100)   NULL,
    [cgi_deviceid]                NVARCHAR (100)   NULL,
    [cgi_txnnum]                  NVARCHAR (100)   NULL,
    [cgi_cardsect]                NVARCHAR (100)   NULL,
    [cgi_rectype]                 NVARCHAR (100)   NULL,
    [cgi_txntype]                 NVARCHAR (100)   NULL,
    [cgi_route]                   NVARCHAR (100)   NULL,
    [cgi_currency]                NVARCHAR (100)   NULL,
    [cgi_origzone]                NVARCHAR (100)   NULL,
    [cgi_time]                    NVARCHAR (100)   NULL,
    [cgi_caseId]                  UNIQUEIDENTIFIER NULL,
    [cgi_TravelCard]              NVARCHAR (100)   NULL,
    [cgi_OrigZoneName]            NVARCHAR (100)   NULL,
    [cgi_Amount]                  NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_travelcardtransactionBase] PRIMARY KEY CLUSTERED ([cgi_travelcardtransactionId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_travelcardtransaction] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_travelcard_cgi_travelcardtransaction_TravelCardid] FOREIGN KEY ([cgi_TravelCardid]) REFERENCES [dbo].[cgi_travelcardBase] ([cgi_travelcardId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_cgi_travelcardtransaction_caseId] FOREIGN KEY ([cgi_caseId]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_travelcardtransaction] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_travelcardtransactionBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_travelcardtransactionBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_travelcardtransactionBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_travelcardtransaction]
    ON [dbo].[cgi_travelcardtransactionBase]([cgi_travelcardtransaction] ASC) WITH (FILLFACTOR = 80);


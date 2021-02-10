CREATE TABLE [dbo].[cgi_refundtypeBase] (
    [cgi_refundtypeId]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_refundtypeBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_refundtypename]        NVARCHAR (100)   NULL,
    [cgi_FinancialTransaction]  BIT              NULL,
    [cgi_RefundOption]          INT              NULL,
    [cgi_refundaccountid]       UNIQUEIDENTIFIER NULL,
    [cgi_refundresponsibleId]   UNIQUEIDENTIFIER NULL,
    [cgi_refundproductid]       UNIQUEIDENTIFIER NULL,
    [cgi_reinvoice]             BIT              NULL,
    [cgi_PrintText]             NVARCHAR (200)   NULL,
    CONSTRAINT [PK_cgi_refundtypeBase] PRIMARY KEY CLUSTERED ([cgi_refundtypeId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_refundtype] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_refundaccount_cgi_refundtype] FOREIGN KEY ([cgi_refundaccountid]) REFERENCES [dbo].[cgi_refundaccountBase] ([cgi_refundaccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_refundproduct_cgi_refundtype] FOREIGN KEY ([cgi_refundproductid]) REFERENCES [dbo].[cgi_refundproductBase] ([cgi_refundproductId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_refundresponsible_cgi_refundtype] FOREIGN KEY ([cgi_refundresponsibleId]) REFERENCES [dbo].[cgi_refundresponsibleBase] ([cgi_refundresponsibleId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_refundtype] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_refundtypeBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_refundtypeBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_refundtypeBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_refundtypename]
    ON [dbo].[cgi_refundtypeBase]([cgi_refundtypename] ASC) WITH (FILLFACTOR = 80);


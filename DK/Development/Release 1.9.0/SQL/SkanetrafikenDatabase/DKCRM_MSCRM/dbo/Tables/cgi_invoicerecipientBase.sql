CREATE TABLE [dbo].[cgi_invoicerecipientBase] (
    [cgi_invoicerecipientId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_invoicerecipientBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_invoicerecipientname]  NVARCHAR (100)   NULL,
    [EmailAddress]              NVARCHAR (256)   NULL,
    [cgi_customer_no]           NVARCHAR (100)   NULL,
    [cgi_inv_reference]         NVARCHAR (100)   NULL,
    [cgi_address1]              NVARCHAR (100)   NULL,
    [cgi_postalcode]            NVARCHAR (100)   NULL,
    [cgi_postal_city]           NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_invoicerecipientBase] PRIMARY KEY CLUSTERED ([cgi_invoicerecipientId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_invoicerecipient] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_invoicerecipient] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_invoicerecipientBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_invoicerecipientBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_invoicerecipientBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_invoicerecipientname]
    ON [dbo].[cgi_invoicerecipientBase]([cgi_invoicerecipientname] ASC) WITH (FILLFACTOR = 80);


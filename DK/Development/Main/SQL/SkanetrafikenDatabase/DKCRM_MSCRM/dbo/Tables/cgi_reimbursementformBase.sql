CREATE TABLE [dbo].[cgi_reimbursementformBase] (
    [cgi_reimbursementformId]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_reimbursementformBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_reimbursementname]     NVARCHAR (100)   NULL,
    [cgi_ReInvoicing]           BIT              NULL,
    [cgi_UseAccount]            BIT              NULL,
    [cgi_UseProduct]            BIT              NULL,
    [cgi_sendtostralfors]       BIT              NULL,
    [cgi_loadcard]              BIT              NULL,
    [cgi_attestation]           BIT              NULL,
    [cgi_useresponsible]        BIT              NULL,
    [cgi_payment]               BIT              NULL,
    [cgi_payment_abroad]        BIT              NULL,
    [cgi_time_valid]            BIT              NULL,
    [cgi_giftcard]              BIT              NULL,
    [cgi_Print]                 BIT              NULL,
    [cgi_couponsms]             BIT              NULL,
    [cgi_couponemail]           BIT              NULL,
    CONSTRAINT [PK_cgi_reimbursementformBase] PRIMARY KEY CLUSTERED ([cgi_reimbursementformId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_reimbursementform] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_reimbursementform] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_reimbursementformBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_reimbursementformBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_reimbursementformBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_reimbursementname]
    ON [dbo].[cgi_reimbursementformBase]([cgi_reimbursementname] ASC) WITH (FILLFACTOR = 80);


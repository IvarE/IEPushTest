CREATE TABLE [dbo].[cgi_rgolsettingBase] (
    [cgi_rgolsettingId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_rgolsettingBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_name]                  NVARCHAR (100)   NULL,
    [cgi_rgolsettingno]         NVARCHAR (100)   NULL,
    [cgi_ReimbursementFormid]   UNIQUEIDENTIFIER NULL,
    [cgi_RefundTypeid]          UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_rgolsettingBase] PRIMARY KEY CLUSTERED ([cgi_rgolsettingId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_rgolsetting] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_refundtype_cgi_rgolsetting_RefundTypeid] FOREIGN KEY ([cgi_RefundTypeid]) REFERENCES [dbo].[cgi_refundtypeBase] ([cgi_refundtypeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_reimbursementform_cgi_rgolsetting_ReimbursementFormid] FOREIGN KEY ([cgi_ReimbursementFormid]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_rgolsetting] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_rgolsettingBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_rgolsettingBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_rgolsettingBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_name]
    ON [dbo].[cgi_rgolsettingBase]([cgi_name] ASC) WITH (FILLFACTOR = 80);


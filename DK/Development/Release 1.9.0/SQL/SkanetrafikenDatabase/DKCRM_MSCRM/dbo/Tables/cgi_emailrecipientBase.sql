CREATE TABLE [dbo].[cgi_emailrecipientBase] (
    [cgi_emailrecipientId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_emailrecipientBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_emailrecipientname]    NVARCHAR (100)   NULL,
    [cgi_EmailAddress]          NVARCHAR (106)   NULL,
    [cgi_Role]                  NVARCHAR (121)   NULL,
    [cgi_EmailGroupid]          UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_emailrecipientBase] PRIMARY KEY CLUSTERED ([cgi_emailrecipientId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_emailrecipient] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_emailgroup_cgi_emailrecipient_EmailGroupid] FOREIGN KEY ([cgi_EmailGroupid]) REFERENCES [dbo].[cgi_emailgroupBase] ([cgi_emailgroupId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_emailrecipient] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_emailrecipientBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_emailrecipientBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_emailrecipientBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_emailrecipientname]
    ON [dbo].[cgi_emailrecipientBase]([cgi_emailrecipientname] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_EmailAddress]
    ON [dbo].[cgi_emailrecipientBase]([cgi_EmailAddress] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_Role]
    ON [dbo].[cgi_emailrecipientBase]([cgi_Role] ASC) WITH (FILLFACTOR = 80);


CREATE TABLE [dbo].[cgi_representativeBase] (
    [cgi_representativeId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_representativeBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_name]                  NVARCHAR (100)   NULL,
    [cgi_Mainphone]             NVARCHAR (100)   NULL,
    [cgi_FirstName]             NVARCHAR (100)   NULL,
    [cgi_LastName]              NVARCHAR (100)   NULL,
    [cgi_OtherPhone]            NVARCHAR (100)   NULL,
    [cgi_Telephone3]            NVARCHAR (100)   NULL,
    [cgi_Email]                 NVARCHAR (100)   NULL,
    [cgi_StreetAddress]         NVARCHAR (100)   NULL,
    [cgi_COaddress]             NVARCHAR (100)   NULL,
    [cgi_ZIPPostalCode]         NVARCHAR (100)   NULL,
    [cgi_City]                  NVARCHAR (100)   NULL,
    [EmailAddress]              NVARCHAR (256)   NULL,
    [cgi_country]               NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_representativeBase] PRIMARY KEY CLUSTERED ([cgi_representativeId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_representative] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_representative] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_representativeBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_representativeBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_representativeBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_name]
    ON [dbo].[cgi_representativeBase]([cgi_name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_FirstName]
    ON [dbo].[cgi_representativeBase]([cgi_FirstName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_Telephone3]
    ON [dbo].[cgi_representativeBase]([cgi_Telephone3] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_Email]
    ON [dbo].[cgi_representativeBase]([cgi_Email] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_Mainphone]
    ON [dbo].[cgi_representativeBase]([cgi_Mainphone] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_LastName]
    ON [dbo].[cgi_representativeBase]([cgi_LastName] ASC) WITH (FILLFACTOR = 80);


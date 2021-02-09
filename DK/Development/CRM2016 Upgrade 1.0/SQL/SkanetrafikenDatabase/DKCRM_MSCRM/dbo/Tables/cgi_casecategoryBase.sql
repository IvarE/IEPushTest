CREATE TABLE [dbo].[cgi_casecategoryBase] (
    [cgi_casecategoryId]        UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_casecategoryBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_casecategoryname]      NVARCHAR (100)   NULL,
    [cgi_Category1id]           UNIQUEIDENTIFIER NULL,
    [cgi_Category2id]           UNIQUEIDENTIFIER NULL,
    [cgi_Category3id]           UNIQUEIDENTIFIER NULL,
    [cgi_Caseid]                UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_casecategoryBase] PRIMARY KEY CLUSTERED ([cgi_casecategoryId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_casecategory] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail_cgi_casecategory_Category1id] FOREIGN KEY ([cgi_Category1id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail_cgi_casecategory_Category2id] FOREIGN KEY ([cgi_Category2id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail_cgi_casecategory_Category3id] FOREIGN KEY ([cgi_Category3id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_cgi_casecategory_Caseid] FOREIGN KEY ([cgi_Caseid]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_casecategory] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_casecategoryBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_casecategoryBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_casecategoryBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_casecategoryname]
    ON [dbo].[cgi_casecategoryBase]([cgi_casecategoryname] ASC) WITH (FILLFACTOR = 80);


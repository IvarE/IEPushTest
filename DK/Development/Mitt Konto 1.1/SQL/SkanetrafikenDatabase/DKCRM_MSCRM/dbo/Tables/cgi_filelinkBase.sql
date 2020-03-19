CREATE TABLE [dbo].[cgi_filelinkBase] (
    [cgi_filelinkId]            UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_filelinkBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_filelinkname]          NVARCHAR (150)   NULL,
    [cgi_IncidentId]            UNIQUEIDENTIFIER NULL,
    [cgi_URL]                   NVARCHAR (250)   NULL,
    CONSTRAINT [PK_cgi_filelinkBase] PRIMARY KEY CLUSTERED ([cgi_filelinkId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_filelink] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_cgi_filelink] FOREIGN KEY ([cgi_IncidentId]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_filelink] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_filelinkBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_filelinkBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_filelinkBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_filelinkname]
    ON [dbo].[cgi_filelinkBase]([cgi_filelinkname] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_for_cascaderelationship_cgi_incident_cgi_filelink]
    ON [dbo].[cgi_filelinkBase]([cgi_IncidentId] ASC) WHERE ([cgi_IncidentId] IS NOT NULL) WITH (FILLFACTOR = 80);


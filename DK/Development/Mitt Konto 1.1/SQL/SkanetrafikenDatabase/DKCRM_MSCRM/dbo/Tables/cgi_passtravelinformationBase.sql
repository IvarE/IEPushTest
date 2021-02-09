CREATE TABLE [dbo].[cgi_passtravelinformationBase] (
    [cgi_passtravelinformationId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                   DATETIME         NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                  DATETIME         NULL,
    [ModifiedBy]                  UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]           UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]          UNIQUEIDENTIFIER NULL,
    [OwnerId]                     UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]                 INT              CONSTRAINT [DF_cgi_passtravelinformationBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]          UNIQUEIDENTIFIER NULL,
    [statecode]                   INT              NOT NULL,
    [statuscode]                  INT              NULL,
    [VersionNumber]               ROWVERSION       NULL,
    [ImportSequenceNumber]        INT              NULL,
    [OverriddenCreatedOn]         DATETIME         NULL,
    [TimeZoneRuleVersionNumber]   INT              NULL,
    [UTCConversionTimeZoneCode]   INT              NULL,
    [cgi_passtravelinformation]   NVARCHAR (100)   NULL,
    [cgi_sTCN]                    NVARCHAR (100)   NULL,
    [cgi_sTCID]                   NVARCHAR (100)   NULL,
    [cgi_iTLID]                   INT              NULL,
    [cgi_sTLN]                    NVARCHAR (100)   NULL,
    [cgi_sTRN]                    NVARCHAR (100)   NULL,
    [cgi_iTJID]                   INT              NULL,
    [cgi_iTFID]                   INT              NULL,
    [cgi_sTFN]                    NVARCHAR (100)   NULL,
    [cgi_sTFD]                    NVARCHAR (100)   NULL,
    [cgi_sTFT]                    NVARCHAR (100)   NULL,
    [cgi_iTTID]                   INT              NULL,
    [cgi_sTTN]                    NVARCHAR (100)   NULL,
    [cgi_sTTD]                    NVARCHAR (100)   NULL,
    [cgi_sTTT]                    NVARCHAR (100)   NULL,
    [cgi_IncidentId]              UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_passtravelinformationBase] PRIMARY KEY CLUSTERED ([cgi_passtravelinformationId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_passtravelinformation] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_cgi_passtravelinformation_IncidentId] FOREIGN KEY ([cgi_IncidentId]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_passtravelinformation] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_passtravelinformationBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_passtravelinformationBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_passtravelinformationBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_passtravelinformation]
    ON [dbo].[cgi_passtravelinformationBase]([cgi_passtravelinformation] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_for_cascaderelationship_cgi_incident_cgi_passtravelinformation_IncidentId]
    ON [dbo].[cgi_passtravelinformationBase]([cgi_IncidentId] ASC) WHERE ([cgi_IncidentId] IS NOT NULL) WITH (FILLFACTOR = 80);


CREATE TABLE [dbo].[cgi_travelinformationBase] (
    [cgi_travelinformationId]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_travelinformationBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_travelinformation]     NVARCHAR (100)   NULL,
    [cgi_ArivalActual]          NVARCHAR (100)   NULL,
    [cgi_ArivalPlanned]         DATETIME         NULL,
    [cgi_City]                  NVARCHAR (100)   NULL,
    [cgi_Contractor]            NVARCHAR (100)   NULL,
    [cgi_DirectionText]         NVARCHAR (100)   NULL,
    [cgi_Line]                  NVARCHAR (100)   NULL,
    [cgi_StartActual]           NVARCHAR (100)   NULL,
    [cgi_StartPlanned]          DATETIME         NULL,
    [cgi_Stop]                  NVARCHAR (100)   NULL,
    [cgi_Tour]                  NVARCHAR (100)   NULL,
    [cgi_Transport]             NVARCHAR (100)   NULL,
    [cgi_Caseid]                UNIQUEIDENTIFIER NULL,
    [cgi_Start]                 NVARCHAR (100)   NULL,
    [cgi_Deviationmessage]      NVARCHAR (MAX)   NULL,
    [cgi_DisplayText]           NVARCHAR (500)   NULL,
    [cgi_JourneyNumber]         NVARCHAR (100)   NULL,
    [cgi_LineType]              NVARCHAR (100)   NULL,
    [cgi_TrainNumber]           NVARCHAR (10)    NULL,
    CONSTRAINT [PK_cgi_travelinformationBase] PRIMARY KEY CLUSTERED ([cgi_travelinformationId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_travelinformation] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_cgi_travelinformation_Caseid] FOREIGN KEY ([cgi_Caseid]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_travelinformation] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_travelinformationBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_travelinformationBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_travelinformationBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_travelinformation]
    ON [dbo].[cgi_travelinformationBase]([cgi_travelinformation] ASC) WITH (FILLFACTOR = 80);


CREATE TABLE [dbo].[cgi_callguideinfoBase] (
    [cgi_callguideinfoId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_callguideinfoBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_callguideinfoname]     NVARCHAR (200)   NULL,
    [cgi_AgentName]             NVARCHAR (100)   NULL,
    [cgi_APhoneNumber]          NVARCHAR (100)   NULL,
    [cgi_BPhoneNumber]          NVARCHAR (100)   NULL,
    [cgi_CallguideSessionID]    NVARCHAR (100)   NULL,
    [cgi_Cid]                   NVARCHAR (100)   NULL,
    [cgi_ContactSourceType]     NVARCHAR (100)   NULL,
    [cgi_Duration]              NVARCHAR (100)   NULL,
    [cgi_ErrandTaskType]        NVARCHAR (100)   NULL,
    [cgi_QueueTime]             NVARCHAR (100)   NULL,
    [cgi_ScreenPopChoice]       NVARCHAR (100)   NULL,
    [cgi_chatcustomeralias]     NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_callguideinfoBase] PRIMARY KEY CLUSTERED ([cgi_callguideinfoId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_callguideinfo] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_callguideinfo] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_callguideinfoBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_callguideinfoBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_callguideinfoBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_callguideinfoname]
    ON [dbo].[cgi_callguideinfoBase]([cgi_callguideinfoname] ASC) WITH (FILLFACTOR = 80);


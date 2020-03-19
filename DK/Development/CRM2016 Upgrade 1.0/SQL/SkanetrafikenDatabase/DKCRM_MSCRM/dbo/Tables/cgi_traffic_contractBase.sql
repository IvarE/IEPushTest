CREATE TABLE [dbo].[cgi_traffic_contractBase] (
    [cgi_traffic_contractId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_traffic_contractBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_name]                  NVARCHAR (100)   NULL,
    [cgi_contract_no]           NVARCHAR (100)   NULL,
    [cgi_contract_note]         NVARCHAR (100)   NULL,
    [cgi_contract_startdate]    DATETIME         NULL,
    [cgi_contract_enddate]      DATETIME         NULL,
    CONSTRAINT [PK_cgi_traffic_contractBase] PRIMARY KEY CLUSTERED ([cgi_traffic_contractId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_traffic_contract] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_traffic_contract] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_traffic_contractBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_traffic_contractBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_traffic_contractBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_name]
    ON [dbo].[cgi_traffic_contractBase]([cgi_name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_contract_startdate]
    ON [dbo].[cgi_traffic_contractBase]([cgi_contract_startdate] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_contract_note]
    ON [dbo].[cgi_traffic_contractBase]([cgi_contract_note] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_contract_no]
    ON [dbo].[cgi_traffic_contractBase]([cgi_contract_no] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_contract_enddate]
    ON [dbo].[cgi_traffic_contractBase]([cgi_contract_enddate] ASC) WITH (FILLFACTOR = 80);


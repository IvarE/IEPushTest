CREATE TABLE [dbo].[cgi_creditorderrowBase] (
    [cgi_creditorderrowId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_creditorderrowBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_name]                  NVARCHAR (100)   NULL,
    [cgi_OrderNumber]           NVARCHAR (100)   NULL,
    [cgi_Sum]                   NVARCHAR (100)   NULL,
    [cgi_ReferenceNumber]       NVARCHAR (100)   NULL,
    [cgi_Success]               NVARCHAR (100)   NULL,
    [cgi_Message]               NVARCHAR (100)   NULL,
    [cgi_Date]                  NVARCHAR (100)   NULL,
    [cgi_Time]                  NVARCHAR (100)   NULL,
    [cgi_ProductNumber]         NVARCHAR (100)   NULL,
    [cgi_Accountid]             UNIQUEIDENTIFIER NULL,
    [cgi_Contactid]             UNIQUEIDENTIFIER NULL,
    [cgi_Reason]                NVARCHAR (100)   NULL,
    [cgi_CreatedBy]             NVARCHAR (100)   NULL,
    CONSTRAINT [PK_cgi_creditorderrowBase] PRIMARY KEY CLUSTERED ([cgi_creditorderrowId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_creditorderrow] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_account_cgi_creditorderrow_Account] FOREIGN KEY ([cgi_Accountid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_contact_cgi_creditorderrow_Contactid] FOREIGN KEY ([cgi_Contactid]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_creditorderrow] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_creditorderrowBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_creditorderrowBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_creditorderrowBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_name]
    ON [dbo].[cgi_creditorderrowBase]([cgi_name] ASC) WITH (FILLFACTOR = 80);


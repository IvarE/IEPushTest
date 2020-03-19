CREATE TABLE [dbo].[st_update_travelcardBase] (
    [st_update_travelcardId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_st_update_travelcardBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [st_name]                   NVARCHAR (100)   NULL,
    [st_CardNumber]             NVARCHAR (100)   NULL,
    [st_ReplyDate]              DATETIME         NULL,
    [st_CardNum]                UNIQUEIDENTIFIER NULL,
    [st_OP_Offer]               BIT              NULL,
    [st_OP_Offer_Date]          DATETIME         NULL,
    [st_OP_Offer_Code]          NVARCHAR (20)    NULL,
    CONSTRAINT [PK_st_update_travelcardBase] PRIMARY KEY CLUSTERED ([st_update_travelcardId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_st_update_travelcard] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_st_update_travelcard] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION,
    CONSTRAINT [st_cgi_travelcard_st_update_travelcard_CardNum] FOREIGN KEY ([st_CardNum]) REFERENCES [dbo].[cgi_travelcardBase] ([cgi_travelcardId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[st_update_travelcardBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[st_update_travelcardBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[st_update_travelcardBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_st_name]
    ON [dbo].[st_update_travelcardBase]([st_name] ASC) WITH (FILLFACTOR = 80);


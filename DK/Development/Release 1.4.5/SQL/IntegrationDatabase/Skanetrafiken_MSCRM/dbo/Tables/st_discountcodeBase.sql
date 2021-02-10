CREATE TABLE [dbo].[st_discountcodeBase] (
    [st_discountcodeId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_st_discountcodeBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [st_name]                   NVARCHAR (100)   NULL,
    [st_Contact]                UNIQUEIDENTIFIER NULL,
    [st_Lead]                   UNIQUEIDENTIFIER NULL,
    [st_Campaign]               UNIQUEIDENTIFIER NULL,
    [st_TravelCard]             UNIQUEIDENTIFIER NULL,
    [st_DiscountCode]           NVARCHAR (100)   NULL,
    [st_ValidFrom]              DATETIME         NULL,
    [st_ValidTo]                DATETIME         NULL,
    [st_CardNo]                 NVARCHAR (100)   NULL,
    CONSTRAINT [PK_st_discountcodeBase] PRIMARY KEY CLUSTERED ([st_discountcodeId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_st_discountcode] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_st_discountcode] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION,
    CONSTRAINT [st_campaign_st_discountcode_Campaign] FOREIGN KEY ([st_Campaign]) REFERENCES [dbo].[CampaignBase] ([CampaignId]) NOT FOR REPLICATION,
    CONSTRAINT [st_cgi_travelcard_st_discountcode_TravelCard] FOREIGN KEY ([st_TravelCard]) REFERENCES [dbo].[cgi_travelcardBase] ([cgi_travelcardId]) NOT FOR REPLICATION,
    CONSTRAINT [st_contact_st_discountcode_Contact] FOREIGN KEY ([st_Contact]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [st_lead_st_discountcode_Lead] FOREIGN KEY ([st_Lead]) REFERENCES [dbo].[LeadBase] ([LeadId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[st_discountcodeBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[st_discountcodeBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[st_discountcodeBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_st_name]
    ON [dbo].[st_discountcodeBase]([st_name] ASC) WITH (FILLFACTOR = 80);


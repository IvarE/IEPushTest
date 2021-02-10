CREATE TABLE [dbo].[cgi_categorydetailBase] (
    [cgi_categorydetailId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                 DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [OwnerId]                   UNIQUEIDENTIFIER NOT NULL,
    [OwnerIdType]               INT              CONSTRAINT [DF_cgi_categorydetailBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [OwningBusinessUnit]        UNIQUEIDENTIFIER NULL,
    [statecode]                 INT              NOT NULL,
    [statuscode]                INT              NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [cgi_categorydetailname]    NVARCHAR (100)   NULL,
    [cgi_CallguideCategory]     NVARCHAR (100)   NULL,
    [cgi_Color]                 INT              NULL,
    [cgi_Sortorder]             INT              NULL,
    [cgi_Parentid]              UNIQUEIDENTIFIER NULL,
    [cgi_Level]                 NVARCHAR (100)   NULL,
    [cgi_parentid2]             UNIQUEIDENTIFIER NULL,
    [cgi_requirestravelinfo]    BIT              NULL,
    CONSTRAINT [PK_cgi_categorydetailBase] PRIMARY KEY CLUSTERED ([cgi_categorydetailId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_cgi_categorydetail] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_categorydetail_cgi_categorydetail_parentid2] FOREIGN KEY ([cgi_parentid2]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail_cgi_categorydetail_Parentid] FOREIGN KEY ([cgi_Parentid]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_cgi_categorydetail] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_categorydetailBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_categorydetailBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_categorydetailBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_categorydetailname]
    ON [dbo].[cgi_categorydetailBase]([cgi_categorydetailname] ASC) WITH (FILLFACTOR = 80);


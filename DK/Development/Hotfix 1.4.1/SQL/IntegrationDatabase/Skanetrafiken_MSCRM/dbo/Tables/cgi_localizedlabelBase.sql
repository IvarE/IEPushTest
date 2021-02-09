CREATE TABLE [dbo].[cgi_localizedlabelBase] (
    [cgi_localizedlabelId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                  DATETIME         NULL,
    [CreatedBy]                  UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                 DATETIME         NULL,
    [ModifiedBy]                 UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]          UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [OrganizationId]             UNIQUEIDENTIFIER NULL,
    [statecode]                  INT              NOT NULL,
    [statuscode]                 INT              NULL,
    [VersionNumber]              ROWVERSION       NULL,
    [ImportSequenceNumber]       INT              NULL,
    [OverriddenCreatedOn]        DATETIME         NULL,
    [TimeZoneRuleVersionNumber]  INT              NULL,
    [UTCConversionTimeZoneCode]  INT              NULL,
    [cgi_localizedlabelname]     NVARCHAR (100)   NULL,
    [cgi_LocalizedControlId]     NVARCHAR (100)   NULL,
    [cgi_LocalizationLanguageid] UNIQUEIDENTIFIER NULL,
    [cgi_LocalizedLabelGroupid]  UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_localizedlabelBase] PRIMARY KEY CLUSTERED ([cgi_localizedlabelId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_localizationlanguage_cgi_localizedlabel_LocalizationLanguageid] FOREIGN KEY ([cgi_LocalizationLanguageid]) REFERENCES [dbo].[cgi_localizationlanguageBase] ([cgi_localizationlanguageId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_localizedlabelgroup_cgi_localizedlabel_LocalizedLabelGroupid] FOREIGN KEY ([cgi_LocalizedLabelGroupid]) REFERENCES [dbo].[cgi_localizedlabelgroupBase] ([cgi_localizedlabelgroupId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[cgi_localizedlabelBase]([OrganizationId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[cgi_localizedlabelBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_localizedlabelBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_localizedlabelname]
    ON [dbo].[cgi_localizedlabelBase]([cgi_localizedlabelname] ASC) WITH (FILLFACTOR = 80);


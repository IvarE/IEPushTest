CREATE TABLE [dbo].[mbs_pluginprofileBase] (
    [mbs_pluginprofileId]                 UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]                           DATETIME         NULL,
    [CreatedBy]                           UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                          DATETIME         NULL,
    [ModifiedBy]                          UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]                   UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]                  UNIQUEIDENTIFIER NULL,
    [OrganizationId]                      UNIQUEIDENTIFIER NULL,
    [statecode]                           INT              NOT NULL,
    [statuscode]                          INT              NULL,
    [VersionNumber]                       ROWVERSION       NULL,
    [ImportSequenceNumber]                INT              NULL,
    [OverriddenCreatedOn]                 DATETIME         NULL,
    [TimeZoneRuleVersionNumber]           INT              NULL,
    [UTCConversionTimeZoneCode]           INT              NULL,
    [mbs_label]                           NVARCHAR (100)   NULL,
    [mbs_Configuration]                   NVARCHAR (MAX)   NULL,
    [mbs_CorrelationId]                   NVARCHAR (50)    NULL,
    [mbs_Depth]                           INT              NULL,
    [mbs_MessageName]                     NVARCHAR (100)   NULL,
    [mbs_mode]                            INT              NULL,
    [mbs_OperationType]                   INT              NULL,
    [mbs_PerformanceConstructorDuration]  INT              NULL,
    [mbs_PerformanceConstructorStartTime] DATETIME         NULL,
    [mbs_PerformanceExecutionDuration]    INT              NULL,
    [mbs_PerformanceExecutionStartTime]   DATETIME         NULL,
    [mbs_PersistenceKey]                  NVARCHAR (50)    NULL,
    [mbs_PrimaryEntity]                   NVARCHAR (100)   NULL,
    [mbs_Profile]                         NVARCHAR (MAX)   NULL,
    [mbs_RequestId]                       NVARCHAR (50)    NULL,
    [mbs_SecureConfiguration]             NVARCHAR (MAX)   NULL,
    [mbs_WorkflowStepId]                  NVARCHAR (50)    NULL,
    [mbs_BusinessUnitId]                  UNIQUEIDENTIFIER NULL,
    [mbs_InitiatingUserId]                UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_mbs_pluginprofileBase] PRIMARY KEY CLUSTERED ([mbs_pluginprofileId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [mbs_businessunit_mbs_pluginprofile] FOREIGN KEY ([mbs_BusinessUnitId]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [mbs_systemuser_mbs_pluginprofile] FOREIGN KEY ([mbs_InitiatingUserId]) REFERENCES [dbo].[SystemUserBase] ([SystemUserId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[mbs_pluginprofileBase]([OrganizationId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[mbs_pluginprofileBase]([statecode] ASC, [statuscode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[mbs_pluginprofileBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_mbs_label]
    ON [dbo].[mbs_pluginprofileBase]([mbs_label] ASC) WITH (FILLFACTOR = 80);


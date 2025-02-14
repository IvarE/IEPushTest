﻿CREATE TABLE [dbo].[SocialInsightsConfigurationBase] (
    [ModifiedOn]                    DATETIME         NULL,
    [RegardingObjectId]             UNIQUEIDENTIFIER NULL,
    [OrganizationId]                UNIQUEIDENTIFIER NOT NULL,
    [SocialDataItemId]              NVARCHAR (100)   NULL,
    [ModifiedBy]                    UNIQUEIDENTIFIER NULL,
    [SocialDataParameters]          NVARCHAR (MAX)   NULL,
    [ModifiedOnBehalfBy]            UNIQUEIDENTIFIER NULL,
    [ControlId]                     NVARCHAR (100)   NULL,
    [CreatedOn]                     DATETIME         NULL,
    [CreatedOnBehalfBy]             UNIQUEIDENTIFIER NULL,
    [FormId]                        UNIQUEIDENTIFIER NOT NULL,
    [SocialDataItemType]            INT              NULL,
    [SocialInsightsConfigurationId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]                     UNIQUEIDENTIFIER NULL,
    [FormIdName]                    NVARCHAR (4000)  NULL,
    [RegardingObjectTypeCode]       INT              NULL,
    [FormTypeCode]                  INT              NULL,
    [RegardingObjectIdName]         NVARCHAR (256)   NULL,
    [RegardingObjectIdYomiName]     NVARCHAR (160)   NULL,
    CONSTRAINT [PK_SocialInsightsConfigurationBase] PRIMARY KEY CLUSTERED ([SocialInsightsConfigurationId] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [ndx_RetrieveForControlInForm]
    ON [dbo].[SocialInsightsConfigurationBase]([FormId] ASC, [ControlId] ASC, [RegardingObjectId] ASC, [RegardingObjectTypeCode] ASC)
    INCLUDE([SocialInsightsConfigurationId], [SocialDataItemId], [SocialDataParameters], [FormTypeCode], [SocialDataItemType]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_RetrieveForControlInDashboard]
    ON [dbo].[SocialInsightsConfigurationBase]([FormId] ASC, [ControlId] ASC)
    INCLUDE([SocialInsightsConfigurationId], [SocialDataItemId], [SocialDataParameters], [FormTypeCode], [SocialDataItemType]) WITH (FILLFACTOR = 80);


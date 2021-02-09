CREATE TABLE [dbo].[AuditBase] (
    [CallingUserId]  UNIQUEIDENTIFIER NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]      DATETIME         NOT NULL,
    [TransactionId]  UNIQUEIDENTIFIER NOT NULL,
    [ChangeData]     NVARCHAR (MAX)   NULL,
    [Action]         INT              NULL,
    [Operation]      INT              NOT NULL,
    [ObjectId]       UNIQUEIDENTIFIER NOT NULL,
    [AuditId]        UNIQUEIDENTIFIER CONSTRAINT [DF_AuditBase_AuditId] DEFAULT (newsequentialid()) NOT NULL,
    [AttributeMask]  NVARCHAR (MAX)   NULL,
    [ObjectTypeCode] INT              NULL,
    [ObjectIdName]   NVARCHAR (1)     NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [cndx_PrimaryKey_Audit]
    ON [dbo].[AuditBase]([CreatedOn] DESC, [AuditId] DESC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_UserId]
    ON [dbo].[AuditBase]([UserId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_ObjectId]
    ON [dbo].[AuditBase]([ObjectId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_ObjectTypeCode]
    ON [dbo].[AuditBase]([ObjectTypeCode] ASC) WHERE ([ObjectTypeCode] IS NOT NULL) WITH (FILLFACTOR = 80);


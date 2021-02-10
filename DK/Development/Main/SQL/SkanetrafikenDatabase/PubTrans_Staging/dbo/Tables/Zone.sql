CREATE TABLE [dbo].[Zone] (
    [Id]                     NUMERIC (16)  NOT NULL,
    [Gid]                    NUMERIC (16)  NOT NULL,
    [Number]                 INT           NOT NULL,
    [Name]                   NVARCHAR (50) NULL,
    [ShortName]              NVARCHAR (16) NULL,
    [TypeCode]               NVARCHAR (8)  NOT NULL,
    [TransportAuthorityCode] NVARCHAR (8)  NOT NULL,
    [ExistsFromDate]         DATE          NOT NULL,
    [Code]                   NVARCHAR (8)  NULL,
    [ExistsUptoDate]         DATE          NULL
);


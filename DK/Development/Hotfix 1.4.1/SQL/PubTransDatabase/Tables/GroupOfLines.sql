CREATE TABLE [dbo].[GroupOfLines] (
    [Id]                     NUMERIC (16)  NOT NULL,
    [TransportAuthorityCode] NVARCHAR (8)  NULL,
    [Code]                   NVARCHAR (8)  NULL,
    [Name]                   NVARCHAR (50) NOT NULL,
    [PurposeOfGroupingCode]  NVARCHAR (8)  NULL,
    [ExistsFromDate]         DATE          NOT NULL,
    [ExistsUptoDate]         DATE          NULL
);


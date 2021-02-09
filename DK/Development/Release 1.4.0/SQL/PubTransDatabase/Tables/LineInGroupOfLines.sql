CREATE TABLE [dbo].[LineInGroupOfLines] (
    [Id]                 NUMERIC (16)   NOT NULL,
    [IsLineId]           NUMERIC (16)   NOT NULL,
    [IsLineGid]          NUMERIC (16)   NOT NULL,
    [IsInGroupOfLinesId] NUMERIC (16)   NOT NULL,
    [IsInGroupOfLineId]  NVARCHAR (255) NULL,
    [ExistsFromDate]     DATE           NOT NULL,
    [ExistsUptoDate]     DATE           NULL
);


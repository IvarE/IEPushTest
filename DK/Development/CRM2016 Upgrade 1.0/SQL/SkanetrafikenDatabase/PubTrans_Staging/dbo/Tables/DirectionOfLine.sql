CREATE TABLE [dbo].[DirectionOfLine] (
    [Id]            NUMERIC (16)  NOT NULL,
    [Gid]           NUMERIC (16)  NOT NULL,
    [DirectionCode] NVARCHAR (1)  NOT NULL,
    [Name]          NVARCHAR (50) NULL,
    [IsOnLineId]    NUMERIC (16)  NOT NULL
);


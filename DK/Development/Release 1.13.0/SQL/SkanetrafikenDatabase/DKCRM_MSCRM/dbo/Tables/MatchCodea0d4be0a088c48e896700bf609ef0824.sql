CREATE TABLE [dbo].[MatchCodea0d4be0a088c48e896700bf609ef0824] (
    [ObjectId]   UNIQUEIDENTIFIER NOT NULL,
    [MatchCode]  NVARCHAR (450)   NULL,
    [ModifiedOn] DATETIME         NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [Index3]
    ON [dbo].[MatchCodea0d4be0a088c48e896700bf609ef0824]([ObjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [Index1]
    ON [dbo].[MatchCodea0d4be0a088c48e896700bf609ef0824]([MatchCode] ASC);


GO
CREATE NONCLUSTERED INDEX [Index2]
    ON [dbo].[MatchCodea0d4be0a088c48e896700bf609ef0824]([ModifiedOn] ASC);


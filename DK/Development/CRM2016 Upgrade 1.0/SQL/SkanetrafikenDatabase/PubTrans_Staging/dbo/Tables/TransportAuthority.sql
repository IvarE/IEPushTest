CREATE TABLE [dbo].[TransportAuthority] (
    [Id]                                    NUMERIC (16)   NOT NULL,
    [Gid]                                   NUMERIC (16)   NULL,
    [IsOrganisationId]                      NUMERIC (16)   NOT NULL,
    [TimetableReleasedForPublicUseUptoDate] DATE           NOT NULL,
    [ExistsFromDate]                        NVARCHAR (255) NULL
);


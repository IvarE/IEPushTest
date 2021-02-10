CREATE TABLE [dbo].[Contractor] (
    [Id]                               NUMERIC (16) NOT NULL,
    [Gid]                              NUMERIC (16) NULL,
    [IsOrganisationId]                 NUMERIC (16) NOT NULL,
    [IsPromotedByTransportAuthorityId] NUMERIC (16) NOT NULL,
    [ExistsFromDate]                   DATE         NULL,
    [ExistsUptoDate]                   DATE         NULL
);


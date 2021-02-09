CREATE TABLE [dbo].[OrganisationalUnit] (
    [Id]                         NUMERIC (16)   NOT NULL,
    [Code]                       NVARCHAR (8)   NOT NULL,
    [Name]                       NVARCHAR (50)  NOT NULL,
    [MailAddressName]            NVARCHAR (50)  NULL,
    [VisitingAddressName]        NVARCHAR (50)  NULL,
    [Postcode]                   NVARCHAR (25)  NULL,
    [PostOfficeName]             NVARCHAR (50)  NULL,
    [CountryName]                NVARCHAR (50)  NULL,
    [ExistsFromDate]             NVARCHAR (255) NULL,
    [PhoneNumber]                NVARCHAR (50)  NULL,
    [ExistsUptoDate]             NVARCHAR (255) NULL,
    [CoordinateSystemName]       NVARCHAR (50)  NULL,
    [CentroidNorthingCoordinate] NVARCHAR (30)  NULL,
    [CentroidEastingCoordinate]  NVARCHAR (30)  NULL
);


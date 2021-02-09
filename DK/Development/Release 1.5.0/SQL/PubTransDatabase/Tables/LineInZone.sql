CREATE TABLE [dbo].[LineInZone] (
    [Gid]                    NUMERIC (16)  NOT NULL,
    [Name]                   NVARCHAR (50) NULL,
    [Number]                 NUMERIC (4)   NOT NULL,
    [Designation]            NVARCHAR (8)  NOT NULL,
    [DisplayOrder]           INT           NOT NULL,
    [TransportAuthorityCode] NVARCHAR (8)  NOT NULL,
    [OperatorCode]           NVARCHAR (8)  NOT NULL,
    [ExistsFromDate]         DATE          NOT NULL,
    [ExistsUptoDate]         DATE          NULL,
    [OperatesZoneId]         NUMERIC (16)  NOT NULL
);


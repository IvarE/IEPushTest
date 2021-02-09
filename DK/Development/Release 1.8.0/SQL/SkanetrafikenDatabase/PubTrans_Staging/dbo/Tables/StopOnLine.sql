CREATE TABLE [dbo].[StopOnLine] (
    [StopAreaId]             NUMERIC (16)  NULL,
    [StopAreaGid]            NUMERIC (16)  NULL,
    [StopAreaName]           NVARCHAR (50) NULL,
    [StopAreaShortName]      NVARCHAR (16) NULL,
    [StopPointId]            NUMERIC (16)  NULL,
    [StopPointGid]           NUMERIC (16)  NULL,
    [JourneyPatternPointGid] NUMERIC (16)  NULL,
    [TransportAuthorityCode] NVARCHAR (25) NULL,
    [ExistsFromDate]         DATE          NULL,
    [IsOnLineId]             NUMERIC (16)  NULL,
    [IsOnLineGid]            NUMERIC (16)  NULL,
    [IsOnDirectionOfLineGid] NUMERIC (16)  NULL,
    [ExistsUptoDate]         DATE          NULL,
    [StopAreaAbbreviation]   NVARCHAR (8)  NULL,
    [StopPointName]          NVARCHAR (50) NULL,
    [StopPointShortName]     NVARCHAR (16) NULL,
    [StopPointDesignation]   NVARCHAR (4)  NULL,
    [StopPointLocalNumber]   NUMERIC (3)   NULL,
    [DisplayOrderNumber]     NUMERIC (12)  NULL
);


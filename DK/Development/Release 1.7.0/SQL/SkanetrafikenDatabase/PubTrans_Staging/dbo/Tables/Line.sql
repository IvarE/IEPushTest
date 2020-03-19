CREATE TABLE [dbo].[Line] (
    [Id]                       NUMERIC (16)  NOT NULL,
    [Gid]                      NUMERIC (16)  NOT NULL,
    [Name]                     NVARCHAR (50) NULL,
    [Number]                   NUMERIC (4)   NOT NULL,
    [Designation]              NVARCHAR (8)  NOT NULL,
    [DefaultTransportModeCode] NVARCHAR (8)  NOT NULL,
    [TransportAuthorityCode]   NVARCHAR (8)  NOT NULL,
    [TransportAuthorityName]   NVARCHAR (50) NOT NULL,
    [DisplayOrder]             INT           NOT NULL,
    [ExistsFromDate]           DATE          NOT NULL,
    [ExistsUptoDate]           DATE          NULL
);


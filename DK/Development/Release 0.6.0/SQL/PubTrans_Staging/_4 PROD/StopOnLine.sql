USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[StopOnLine]    Script Date: 2015-04-24 13:40:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StopOnLine](
	[StopAreaId] [numeric](16, 0) NULL,
	[StopAreaGid] [numeric](16, 0) NULL,
	[StopAreaName] [nvarchar](50) NULL,
	[StopAreaShortName] [nvarchar](16) NULL,
	[StopPointId] [numeric](16, 0) NULL,
	[StopPointGid] [numeric](16, 0) NULL,
	[JourneyPatternPointGid] [numeric](16, 0) NULL,
	[TransportAuthorityCode] [nvarchar](25) NULL,
	[ExistsFromDate] [date] NULL,
	[IsOnLineId] [numeric](16, 0) NULL,
	[IsOnLineGid] [numeric](16, 0) NULL,
	[IsOnDirectionOfLineGid] [numeric](16, 0) NULL,
	[ExistsUptoDate] [date] NULL,
	[StopAreaAbbreviation] [nvarchar](8) NULL,
	[StopPointName] [nvarchar](50) NULL,
	[StopPointShortName] [nvarchar](16) NULL,
	[StopPointDesignation] [nvarchar](4) NULL,
	[StopPointLocalNumber] [numeric](3, 0) NULL,
	[DisplayOrderNumber] [numeric](12, 0) NULL
) ON [PRIMARY]

GO


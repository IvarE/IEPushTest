USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[LineInZone]    Script Date: 2015-04-24 12:48:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LineInZone](
	[Gid] [numeric](16, 0) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Number] [numeric](4, 0) NOT NULL,
	[Designation] [nvarchar](8) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[TransportAuthorityCode] [nvarchar](8) NOT NULL,
	[OperatorCode] [nvarchar](8) NOT NULL,
	[ExistsFromDate] [date] NOT NULL,
	[ExistsUptoDate] [date] NULL,
	[OperatesZoneId] [numeric](16, 0) NOT NULL
) ON [PRIMARY]

GO


USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[LineInGroupOfLines]    Script Date: 2015-04-24 12:48:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LineInGroupOfLines](
	[Id] [numeric](16, 0) NOT NULL,
	[IsLineId] [numeric](16, 0) NOT NULL,
	[IsLineGid] [numeric](16, 0) NOT NULL,
	[IsInGroupOfLinesId] [numeric](16, 0) NOT NULL,
	[IsInGroupOfLineId] [nvarchar](255) NULL,
	[ExistsFromDate] [date] NOT NULL,
	[ExistsUptoDate] [date] NULL
) ON [PRIMARY]

GO


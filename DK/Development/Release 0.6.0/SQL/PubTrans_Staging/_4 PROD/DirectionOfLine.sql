USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[DirectionOfLine]    Script Date: 2015-04-24 13:39:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DirectionOfLine](
	[Id] [numeric](16, 0) NOT NULL,
	[Gid] [numeric](16, 0) NOT NULL,
	[DirectionCode] [nvarchar](1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsOnLineId] [numeric](16, 0) NOT NULL
) ON [PRIMARY]

GO


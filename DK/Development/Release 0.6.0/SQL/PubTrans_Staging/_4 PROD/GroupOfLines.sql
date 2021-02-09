USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[GroupOfLines]    Script Date: 2015-04-24 13:39:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GroupOfLines](
	[Id] [numeric](16, 0) NOT NULL,
	[TransportAuthorityCode] [nvarchar](8) NULL,
	[Code] [nvarchar](8) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PurposeOfGroupingCode] [nvarchar](8) NULL,
	[ExistsFromDate] [date] NOT NULL,
	[ExistsUptoDate] [date] NULL
) ON [PRIMARY]

GO


USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[Zone]    Script Date: 2015-04-24 13:41:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Zone](
	[Id] [numeric](16, 0) NOT NULL,
	[Gid] [numeric](16, 0) NOT NULL,
	[Number] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ShortName] [nvarchar](16) NULL,
	[TypeCode] [nvarchar](8) NOT NULL,
	[TransportAuthorityCode] [nvarchar](8) NOT NULL,
	[ExistsFromDate] [date] NOT NULL,
	[Code] [nvarchar](8) NULL,
	[ExistsUptoDate] [date] NULL
) ON [PRIMARY]

GO


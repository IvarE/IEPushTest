USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[Line]    Script Date: 2015-04-24 12:28:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Line](
	[Id] [numeric](16, 0) NOT NULL,
	[Gid] [numeric](16, 0) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Number] [numeric](4, 0) NOT NULL,
	[Designation] [nvarchar](8) NOT NULL,
	[DefaultTransportModeCode] [nvarchar](8) NOT NULL,
	[TransportAuthorityCode] [nvarchar](8) NOT NULL,
	[TransportAuthorityName] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[ExistsFromDate] [date] NOT NULL,
	[ExistsUptoDate] [date] NULL
) ON [PRIMARY]

GO


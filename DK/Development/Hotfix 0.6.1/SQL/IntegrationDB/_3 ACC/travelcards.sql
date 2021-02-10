USE [IntegrationDB]
GO

/****** Object:  Table [dbo].[travelcards]    Script Date: 2015-04-24 12:41:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[travelcards](
	[customer_id] [varchar](255) NULL,
	[cardnumber] [varchar](255) NULL,
	[travelcardid] [varchar](255) NULL,
	[contactidx] [varchar](255) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


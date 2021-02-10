USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[LineDetails]    Script Date: 2015-04-24 12:28:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[LineDetails](
	[LineDetails] [xml] NULL,
	[LineType] [varchar](50) NULL,
	[CreatedOn] [date] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LineDetails] ADD  DEFAULT (CONVERT([varchar](10),getdate(),(110))) FOR [CreatedOn]
GO


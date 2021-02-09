USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[TransportAuthority]    Script Date: 2015-04-24 10:56:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TransportAuthority](
	[Id] [numeric](16, 0) NOT NULL,
	[Gid] [numeric](16, 0) NULL,
	[IsOrganisationId] [numeric](16, 0) NOT NULL,
	[TimetableReleasedForPublicUseUptoDate] [date] NOT NULL,
	[ExistsFromDate] [nvarchar](255) NULL
) ON [PRIMARY]

GO


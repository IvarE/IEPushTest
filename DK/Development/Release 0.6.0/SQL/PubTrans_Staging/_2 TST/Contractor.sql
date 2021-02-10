USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[Contractor]    Script Date: 2015-04-24 12:27:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contractor](
	[Id] [numeric](16, 0) NOT NULL,
	[Gid] [numeric](16, 0) NULL,
	[IsOrganisationId] [numeric](16, 0) NOT NULL,
	[IsPromotedByTransportAuthorityId] [numeric](16, 0) NOT NULL,
	[ExistsFromDate] [date] NULL,
	[ExistsUptoDate] [date] NULL
) ON [PRIMARY]

GO


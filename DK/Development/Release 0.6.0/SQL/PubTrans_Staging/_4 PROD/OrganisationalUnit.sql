USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[OrganisationalUnit]    Script Date: 2015-04-24 13:40:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrganisationalUnit](
	[Id] [numeric](16, 0) NOT NULL,
	[Code] [nvarchar](8) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MailAddressName] [nvarchar](50) NULL,
	[VisitingAddressName] [nvarchar](50) NULL,
	[Postcode] [nvarchar](25) NULL,
	[PostOfficeName] [nvarchar](50) NULL,
	[CountryName] [nvarchar](50) NULL,
	[ExistsFromDate] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[ExistsUptoDate] [nvarchar](255) NULL,
	[CoordinateSystemName] [nvarchar](50) NULL,
	[CentroidNorthingCoordinate] [nvarchar](30) NULL,
	[CentroidEastingCoordinate] [nvarchar](30) NULL
) ON [PRIMARY]

GO


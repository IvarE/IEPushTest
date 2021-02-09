USE [PubTrans_Staging]
GO

/****** Object:  Table [dbo].[Contractor]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[DirectionOfLine]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[GroupOfLines]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[Line]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[LineDetails]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[LineInGroupOfLines]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[LineInZone]    Script Date: 2015-01-30 14:52:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LineInZone](
	[Gid] [numeric](16, 0) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Number] [numeric](4, 0) NOT NULL,
	[Designation] [nvarchar](8) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[TransportAuthorityCode] [nvarchar](8) NOT NULL,
	[OperatorCode] [nvarchar](8) NOT NULL,
	[ExistsFromDate] [date] NOT NULL,
	[ExistsUptoDate] [date] NULL,
	[OperatesZoneId] [numeric](16, 0) NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OrganisationalUnit]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[StopOnLine]    Script Date: 2015-01-30 14:52:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StopOnLine](
	[StopAreaId] [numeric](16, 0) NULL,
	[StopAreaGid] [numeric](16, 0) NULL,
	[StopAreaName] [nvarchar](50) NULL,
	[StopAreaShortName] [nvarchar](16) NULL,
	[StopPointId] [numeric](16, 0) NULL,
	[StopPointGid] [numeric](16, 0) NULL,
	[JourneyPatternPointGid] [numeric](16, 0) NULL,
	[TransportAuthorityCode] [nvarchar](25) NULL,
	[ExistsFromDate] [date] NULL,
	[IsOnLineId] [numeric](16, 0) NULL,
	[IsOnLineGid] [numeric](16, 0) NULL,
	[IsOnDirectionOfLineGid] [numeric](16, 0) NULL,
	[ExistsUptoDate] [date] NULL,
	[StopAreaAbbreviation] [nvarchar](8) NULL,
	[StopPointName] [nvarchar](50) NULL,
	[StopPointShortName] [nvarchar](16) NULL,
	[StopPointDesignation] [nvarchar](4) NULL,
	[StopPointLocalNumber] [numeric](3, 0) NULL,
	[DisplayOrderNumber] [numeric](12, 0) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[TransportAuthority]    Script Date: 2015-01-30 14:52:26 ******/
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

/****** Object:  Table [dbo].[Zone]    Script Date: 2015-01-30 14:52:26 ******/
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



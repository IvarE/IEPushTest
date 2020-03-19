/****** Object:  StoredProcedure [dbo].[CreateTables]    Script Date: 2017-08-18 13:37:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	Create tables
-- =============================================
CREATE PROCEDURE [dbo].[CreateTables] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	drop table dbo.[CRMtravelcards]
	drop table dbo.[BIFFtravelcards]
	drop table dbo.[AllBIFFtravelcards]
	drop table dbo.[AllUniqueBIFFtravelcards]
	drop table dbo.[TravelcardDiff]
	drop table dbo.[TravelcardError]
	drop table dbo.[LoadDates]

	CREATE TABLE [dbo].[CRMtravelcards](
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [int] NULL,
		[CardOfferingPeriodkort] [int] NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		-- Periodkort
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		-- Reskassa
		[BlockedValue] [int] NULL,
		[BlockedDateValue] [date] NULL,
		[BlockedDescriptionValue] [nvarchar](MAX) NULL,
		-- Hela kortet spärrat
		[BlockedCard] [int] NULL,
		[BlockedDateCard] [date] NULL,
		[BlockedDescriptionCard] [nvarchar](MAX) NULL,

		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL
	) ON [PRIMARY]

	CREATE INDEX [CRMtravelcards_index]
	ON [CRMtravelcards] ([CardNumber]) 

	CREATE TABLE [dbo].[AllUniqueBIFFtravelcards](
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [int] NULL,
		[CardOfferingPeriodkort] [int] NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		-- Periodkort
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		-- Reskassa
		[BlockedValue] [int] NULL,
		[BlockedDateValue] [date] NULL,
		[BlockedDescriptionValue] [nvarchar](MAX) NULL,
		-- Hela kortet spärrat
		[BlockedCard] [int] NULL,
		[BlockedDateCard] [date] NULL,
		[BlockedDescriptionCard] [nvarchar](MAX) NULL,

		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL
	) ON [PRIMARY]

	CREATE UNIQUE INDEX [AllUniqueBIFFtravelcards_index]
	ON [AllUniqueBIFFtravelcards] ([CardNumber]) 

	CREATE TABLE [dbo].[AllBIFFtravelcards](
		[CardNumber] [bigint] NULL,
		[CardSection] [int] NULL,
		[CardOffering] [int] NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL
	) ON [PRIMARY]

	CREATE INDEX [AllBIFFtravelcards_index]
	ON [AllBIFFtravelcards] ([CardNumber],[CardSection]) 

	CREATE TABLE [dbo].[BIFFtravelcards](
		[Activated] [nvarchar](max) NULL,
		-- Periodkort & reskassa
		[BlockDate] [date] NULL, 
		[BlockDescription] [nvarchar](max) NULL,
		[BlockState] [int] NULL,
		/*[CardCategory] [nvarchar](max) NULL,*/
		/*[CardId] [bigint] NULL,*/
		[CardNumber] [bigint] NULL,
		[CardSection] [int] NULL,		-- 1 = Period, 2 = Reskassa
		/* [CardType] [varchar](max) NULL, */
		/*[ConnectedToAccountNumber] [nvarchar](max) NULL, */
		/* [ConnectedToContactDate] [date] NULL, */
		/*[ConnectedToContactNumber] [nvarchar](max) NULL,*/
		/*[CreatedDate] [date] NULL,*/
		/*[CreatedOn] [date] NULL,*/
		[ExpireDay] [date] NULL,
		[IsActive] [bit] NULL,
		/*[Latest_Transaction] [date] NULL,*/
		/*[LatestLoadDate] [date] NULL,*/
		[LatestTravelDate] [date] NULL,
		[ModifiedOn] [datetime] NULL,
		/*[OfferingName] [nvarchar](max) NULL,*/
		[PeriodStartDay] [date] NULL,
		[TicketCodeNumber] [int] NULL
	) ON [PRIMARY]

	CREATE UNIQUE INDEX [BIFFtravelcards_index]
	ON [BIFFtravelcards] ([CardNumber],[CardSection]) 

	CREATE TABLE [dbo].[TravelcardDiff](
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [int] NULL,
		[CardOfferingPeriodkort] [int] NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		-- Periodkort
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		-- Reskassa
		[BlockedValue] [int] NULL,
		[BlockedDateValue] [date] NULL,
		[BlockedDescriptionValue] [nvarchar](MAX) NULL,
		-- Hela kortet spärrat
		[BlockedCard] [int] NULL,
		[BlockedDateCard] [date] NULL,
		[BlockedDescriptionCard] [nvarchar](MAX) NULL,

		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL,
		[DiffActive] [bit] NULL,
		[DiffActivated] [bit] NULL,
		[DiffBlocked] [bit] NULL,
		[DiffOffering] [bit] NULL,
		[DiffValid] [bit] NULL,
	) ON [PRIMARY]

	CREATE INDEX [TravelcardDiff_index]
	ON [TravelcardDiff] ([CardNumber]) 

	CREATE TABLE [dbo].[TravelcardError](
		[INFO] [nvarchar](50),
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [int] NULL,
		[CardOfferingPeriodkort] [int] NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL
	) ON [PRIMARY]

	CREATE INDEX [TravelcardError_index]
	ON [TravelcardError] ([CardNumber]) 

	CREATE TABLE [dbo].[LoadDates](
		[Source] [nvarchar](50),
		[LatestModifiedDate] [datetime] NULL
	) ON [PRIMARY]

	INSERT INTO [dbo].[LoadDates]
	SELECT 'GetCardView','1900-01-01'

	INSERT INTO [dbo].[LoadDates]
	SELECT 'Filteredcgi_Travelcard','1900-01-01'

END

GO


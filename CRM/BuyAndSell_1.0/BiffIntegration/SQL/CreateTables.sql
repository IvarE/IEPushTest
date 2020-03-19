/****** Object:  StoredProcedure [dbo].[CreateTables]    Script Date: 2017-08-18 13:37:53 ******/
SET ANSI_NULLS ON
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
	--drop table dbo.[TravelcardError]
	drop table dbo.[LoadDates]

	CREATE TABLE [dbo].[CRMtravelcards](
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [nvarchar](50) NULL,
		[CardOfferingPeriodkort] [nvarchar](50) NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		-- Periodkort
		-- [Blocked] [int] NULL,							-- ta bort - ed_blockedperiod
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		-- Reskassa
		-- [BlockedValue] [int] NULL,						-- ta bort - ed_blockedvalue
		[BlockedDateValue] [date] NULL,
		[BlockedDescriptionValue] [nvarchar](MAX) NULL,
		-- Spärrad status / optionset.
		[BlockedStatus] [int] NULL,
		-- Hela kortet spärrat
		-- [BlockedCard] [int] NULL,						-- ta bort - ed_blockedcard
		-- [BlockedDateCard] [date] NULL,					-- ta bort - ed_blockeddatecard
		-- [BlockedDescriptionCard] [nvarchar](MAX) NULL,	-- ta bort - ed_blockeddescriptioncard

		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL,
		-- Typ av tjänst, t.ex. 2161
		[TicketCodeNumber] [int] NULL,
		[TicketCodeNumberValue] [int] NULL,
		[Balance] numeric(12,2) NULL,
	) ON [PRIMARY]

	CREATE INDEX [CRMtravelcards_index]
	ON [CRMtravelcards] ([CardNumber]) 

	CREATE TABLE [dbo].[AllUniqueBIFFtravelcards](
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [nvarchar](50) NULL,
		[CardOfferingPeriodkort] [nvarchar](50) NULL, 
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
		-- 
		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL,
		[Balance] numeric(12,2) NULL,
		-- Typ av tjänst, t.ex. 2161
		[TicketCodeNumber] [int] NULL,
		[TicketCodeNumberValue] [int] NULL,
	) ON [PRIMARY]

	CREATE UNIQUE INDEX [AllUniqueBIFFtravelcards_index]
	ON [AllUniqueBIFFtravelcards] ([CardNumber]) 

	CREATE TABLE [dbo].[AllBIFFtravelcards](
		[CardNumber] [bigint] NULL,
		[CardSection] [int] NULL,
		[CardOffering] [nvarchar](50) NULL, 
		[TicketCodeNumber] [int] NULL, /* Typ av tjänst på kortet */
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		[Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL,
		[IsPassBlock] [int] NOT NULL, 
		[IsPurseBlock] [int] NOT NULL, 
		[IsCardBlock] [int] NOT NULL,
		[Balance] numeric(12,2) NULL
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
		[OfferingName] [nvarchar](50) NULL, /* Typ av tjänst på kortet */
		[PeriodStartDay] [date] NULL,
		[TicketCodeNumber] [int] NULL, /* Typ av tjänst på kortet */
		[IsPassBlock] [bit] NOT NULL, 
		[IsPurseBlock] [bit] NOT NULL, 
		[IsCardBlock] [bit] NOT NULL,
		[Balance] numeric(12,2) NULL
	) ON [PRIMARY]

	CREATE UNIQUE INDEX [BIFFtravelcards_index]
	ON [BIFFtravelcards] ([CardNumber],[CardSection]) 

	CREATE TABLE [dbo].[TravelcardDiff]( 
		[CardNumber] [bigint] NULL,
		[CardOfferingReskassa] [nvarchar](50) NULL,
		[CardOfferingPeriodkort] [nvarchar](50) NULL, 
		[Activated] [int] NULL,
		[IsActive] [int] NULL,
		-- Periodkort
		-- [Blocked] [int] NULL,
		[BlockedDate] [date] NULL,
		[BlockedDescription] [nvarchar](MAX) NULL,
		-- Reskassa
		-- [BlockedValue] [int] NULL,
		[BlockedDateValue] [date] NULL,
		[BlockedDescriptionValue] [nvarchar](MAX) NULL,
		-- Spärrad status	// optionset.
		[BlockedStatus] [int] NULL,
		-- Hela kortet spärrat
		-- [BlockedCard] [int] NULL,
		-- [BlockedDateCard] [date] NULL,
		-- [BlockedDescriptionCard] [nvarchar](MAX) NULL,

		[ValidFromDate] [date] NULL,
		[ValidToDate] [date] NULL,
		[LatestTravelDate] [date] NULL,
		[ModifiedDate] [datetime] NULL,
		[Balance] numeric(12,2) NULL,

		-- Typ av tjänst, t.ex. 2161
		[TicketCodeNumber] [int] NULL,
		[TicketCodeNumberValue] [int] NULL,

		[DiffActive] [bit] NULL,
		[DiffActivated] [bit] NULL,
		[DiffBlocked] [bit] NULL,
		[DiffOffering] [bit] NULL,
		[DiffValid] [bit] NULL,
		[DiffValue] [bit] NULL,			-- Diff in reskassa
	) ON [PRIMARY]

	CREATE INDEX [TravelcardDiff_index]
	ON [TravelcardDiff] ([CardNumber]) 

	-- Not used, JA:181025
	--CREATE TABLE [dbo].[TravelcardError](
	--	[INFO] [nvarchar](50),
	--	[CardNumber] [bigint] NULL,
	--	[CardOfferingReskassa] [nvarchar](50) NULL,
	--	[CardOfferingPeriodkort] [nvarchar](50) NULL, 
	--	[Activated] [int] NULL,
	--	[IsActive] [int] NULL,
	--	[Blocked] [int] NULL,
	--	[BlockedDate] [date] NULL,
	--	[BlockedDescription] [nvarchar](MAX) NULL,
	--	[ValidFromDate] [date] NULL,
	--	[ValidToDate] [date] NULL,
	--	[LatestTravelDate] [date] NULL,
	--	[ModifiedDate] [datetime] NULL
	--) ON [PRIMARY]

	--CREATE INDEX [TravelcardError_index]
	--ON [TravelcardError] ([CardNumber]) 

	CREATE TABLE [dbo].[LoadDates](
		[Source] [nvarchar](50),
		[LatestModifiedDate] [datetime] NULL,
		[MaxCardNumber] [bigint] NULL
	) ON [PRIMARY]

	INSERT INTO [dbo].[LoadDates]
	SELECT 'GetCardView','1900-01-01',0

	INSERT INTO [dbo].[LoadDates]
	SELECT 'Filteredcgi_Travelcard','1900-01-01',0

END

GO


CREATE TABLE [dbo].[GetCardView](
	[Activated] [nvarchar](max) NULL,
	[BlockDate] [date] NULL,
	[BlockDescription] [nvarchar](max) NULL,
	[BlockState] [int] NULL,
	[CardCategory] [nvarchar](max) NULL,
	[CardId] [bigint] NULL,
	[CardNumber] [bigint] NULL,
	[CardSection] [int] NULL,
	[CardType] [varchar](max) NULL,
	[ConnectedToAccountNumber] [nvarchar](max) NULL,
	[ConnectedToContactDate] [date] NULL,
	[ConnectedToContactNumber] [nvarchar](max) NULL,
	[CreatedDate] [date] NULL,
	[CreatedOn] [date] NULL,
	[ExpireDay] [date] NULL,
	[IsActive] [bit] NULL,
	[Latest_Transaction] [date] NULL,
	[LatestLoadDate] [date] NULL,
	[LatestTravelDate] [date] NULL,
	[ModifiedOn] [datetime] NULL,
	[OfferingName] [nvarchar](max) NULL,
	[PeriodStartDay] [date] NULL,
	[OfferingName] [nvarchar](max) NULL,
	[TicketCodeNumber] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


insert into [GetCardView]
select * from crmtravelcards.dbo.getcardview

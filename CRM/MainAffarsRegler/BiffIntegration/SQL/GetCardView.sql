
TABLE_QUALIFIER	TABLE_OWNER	TABLE_NAME	COLUMN_NAME	DATA_TYPE	TYPE_NAME	PRECISION	LENGTH	SCALE	RADIX	NULLABLE	REMARKS	COLUMN_DEF	SQL_DATA_TYPE	SQL_DATETIME_SUB	CHAR_OCTET_LENGTH	ORDINAL_POSITION	IS_NULLABLE	SS_DATA_TYPE
STDW	dbo	GetCardView	CardId	4	int identity	10	4	0	10	0	NULL	NULL	4	NULL	NULL	1	NO	56
STDW	dbo	GetCardView	CardNumber	2	numeric	10	12	0	10	0	NULL	NULL	2	NULL	NULL	2	NO	63
STDW	dbo	GetCardView	ConnectedToContactDate	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	3	YES	111
STDW	dbo	GetCardView	ConnectedToContactNumber	-11	uniqueidentifier	36	16	NULL	NULL	1	NULL	NULL	-11	NULL	NULL	4	YES	37
STDW	dbo	GetCardView	ConnectedToAccountNumber	-11	uniqueidentifier	36	16	NULL	NULL	1	NULL	NULL	-11	NULL	NULL	5	YES	37
STDW	dbo	GetCardView	CreatedOn	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	6	YES	111
STDW	dbo	GetCardView	CardType	12	varchar	50	50	NULL	NULL	0	NULL	NULL	12	NULL	50	7	NO	39
STDW	dbo	GetCardView	CardSection	2	numeric	1	3	0	10	1	NULL	NULL	2	NULL	NULL	8	YES	108
STDW	dbo	GetCardView	Activated	12	varchar	50	50	NULL	NULL	0	NULL	NULL	12	NULL	50	9	NO	39
STDW	dbo	GetCardView	CardCategory	12	varchar	50	50	NULL	NULL	0	NULL	NULL	12	NULL	50	10	NO	39
STDW	dbo	GetCardView	TicketCodeNumber	2	numeric	5	7	0	10	1	NULL	NULL	2	NULL	NULL	11	YES	108
STDW	dbo	GetCardView	ExpireDay	-9	date	10	20	NULL	NULL	1	NULL	NULL	-9	NULL	NULL	12	YES	0
STDW	dbo	GetCardView	PeriodStartDay	-9	date	10	20	NULL	NULL	1	NULL	NULL	-9	NULL	NULL	13	YES	0
STDW	dbo	GetCardView	CreatedDate	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	14	YES	111
STDW	dbo	GetCardView	BlockDescription	12	varchar	255	255	NULL	NULL	1	NULL	NULL	12	NULL	255	15	YES	39
STDW	dbo	GetCardView	BlockState	-6	tinyint	3	1	0	10	1	NULL	NULL	-6	NULL	NULL	16	YES	38
STDW	dbo	GetCardView	BlockDate	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	17	YES	111
STDW	dbo	GetCardView	IsActive	-7	bit	1	1	NULL	NULL	0	NULL	NULL	-7	NULL	NULL	18	NO	50
STDW	dbo	GetCardView	IsCardBlock	-7	bit	1	1	NULL	NULL	1	NULL	NULL	-7	NULL	NULL	19	YES	50
STDW	dbo	GetCardView	IsPassBlock	-7	bit	1	1	NULL	NULL	1	NULL	NULL	-7	NULL	NULL	20	YES	50
STDW	dbo	GetCardView	IsPurseBlock	-7	bit	1	1	NULL	NULL	1	NULL	NULL	-7	NULL	NULL	21	YES	50
STDW	dbo	GetCardView	ModifiedOn	-9	datetime2	19	38	NULL	NULL	0	NULL	NULL	-9	NULL	NULL	22	NO	0
STDW	dbo	GetCardView	OfferingName	12	varchar	50	50	NULL	NULL	1	NULL	NULL	12	NULL	50	23	YES	39
STDW	dbo	GetCardView	LatestTravelDate	-9	date	10	20	NULL	NULL	1	NULL	NULL	-9	NULL	NULL	24	YES	0
STDW	dbo	GetCardView	LatestLoadDate	-9	date	10	20	NULL	NULL	1	NULL	NULL	-9	NULL	NULL	25	YES	0
STDW	dbo	GetCardView	LatestTransactionTrain	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	26	YES	111
STDW	dbo	GetCardView	LatestTransactionBus	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	27	YES	111
STDW	dbo	GetCardView	LatestTransaction	11	datetime	23	16	3	NULL	1	NULL	NULL	9	3	NULL	28	YES	111
STDW	dbo	GetCardView	Balance	2	numeric	10	12	2	10	1	NULL	NULL	2	NULL	NULL	29	YES	108

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [CardId]
      ,[CardNumber]
      ,[ConnectedToContactDate]
      ,[ConnectedToContactNumber]
      ,[ConnectedToAccountNumber]
      ,[CreatedOn]
      ,[CardType]
      ,[CardSection]
      ,[Activated]
      ,[CardCategory]
      ,[TicketCodeNumber]
      ,[ExpireDay]
      ,[PeriodStartDay]
      ,[CreatedDate]
      ,[BlockDescription]
      ,[BlockState]
      ,[BlockDate]
      ,[IsActive]
      ,[IsCardBlock]
      ,[IsPassBlock]
      ,[IsPurseBlock]
      ,[ModifiedOn]
      ,[OfferingName]
      ,[LatestTravelDate]
      ,[LatestLoadDate]
      ,[LatestTransactionTrain]
      ,[LatestTransactionBus]
      ,[LatestTransaction]
      ,[Balance]
  FROM [STDW].[dbo].[GetCardView]
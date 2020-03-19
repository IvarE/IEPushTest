/****** Object:  StoredProcedure [dbo].[CheckValidDatesInBIFF]    Script Date: 2017-08-18 13:39:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckBlockedInBIFF
-- =============================================
alter PROCEDURE [dbo].[CheckValidDatesInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @EnumBlockCard int
	declare @EnumBlockPeriod int
	declare @EnumBlockPurse int
	select @EnumBlockPeriod = 899310000
	select @EnumBlockPurse = 899310001
	select @EnumBlockCard = 899310002


	/* MARK CARDS THAT ARE DIFFERENT IN BIFF */
	INSERT INTO [TravelcardDiff] 
	SELECT B.CardNumber
		,B.CardOfferingReskassa
		,B.[CardOfferingPeriodkort] 
		,B.[Activated]
		,B.[IsActive]  
		,B.[BlockedDate] 
		,B.[BlockedDescription] 
		,B.[BlockedDateValue] 
		,B.[BlockedDescriptionValue] 
		,	case when ISNULL(B.[Blocked],0) = 1 then  @EnumBlockPeriod 
				 when ISNULL(B.[BlockedValue],0) = 1 then @EnumBlockPurse
				 when ISNULL(B.[BlockedCard],0) = 1 then @EnumBlockCard 
				 else null
				 end as 'BlockedStatus'
		,B.[ValidFromDate] 
		,B.[ValidToDate] 
		,B.[LatestTravelDate] 
		,B.[ModifiedDate] 
		,B.[Balance]
		,B.[TicketCodeNumber]
		,B.[TicketCodeNumberValue]
		,0 --B.[DiffActive] 
		,0 --B.[DiffActivated] 
		,0 --B.[DiffBlocked] 
		,0 --B.[DiffOffering] 
		,1 as 'DiffValid'
		,0 as 'DiffValue'	
	
	FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE NOT (C.[ValidFromDate] = B.[ValidFromDate] AND C.[ValidToDate] = B.[ValidToDate]) AND B.[ValidFromDate] > '1901-01-01' AND NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])

	UPDATE [TravelcardDiff]
	SET [DiffValid] = 1
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE NOT (C.[ValidFromDate] = B.[ValidFromDate] AND C.[ValidToDate] = B.[ValidToDate]) AND B.[ValidFromDate] > '1901-01-01'

END

GO


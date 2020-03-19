SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckBlockedInBIFF
-- =============================================
alter PROCEDURE [dbo].[CheckTravelBalanceInBIFF] 
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


	/* ADD CARDS THAT ARE DIFFERENT IN BIFF Blocked (And not already there) */
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
		,0 --B.[DiffValid] 
		,1 as 'DiffValue'

	FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])
	  AND C.[Balance] != B.[Balance] 

	UPDATE [TravelcardDiff]
	SET DiffValue = 1
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE C.[Balance] != B.[Balance] 

END
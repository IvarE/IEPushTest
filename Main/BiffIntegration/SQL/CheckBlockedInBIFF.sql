SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckBlockedInBIFF
-- =============================================
alter PROCEDURE [dbo].[CheckBlockedInBIFF] 
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

	-- Make sure we have valid blocked statuses in table AllUniqueBIFFtravelcards
	-- Make sure we have valid blocked statuses in table AllUniqueBIFFtravelcards
	update AllUniqueBIFFtravelcards
	set BlockedCard = 1
	, Blocked = 0
	, BlockedValue = 0
	where ISNULL(BlockedCard,0) = 1
	and (ISNULL(Blocked,0) != 0
	or ISNULL(BlockedValue,0) != 0)

	update AllUniqueBIFFtravelcards
	set BlockedCard = 1
	, Blocked = 0
	, BlockedValue = 0
	where ISNULL(BlockedCard,0) = 0
	and (ISNULL(Blocked,0) = 1
	and ISNULL(BlockedValue,0) = 1)

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
		,1 --B.[DiffBlocked] 
		,0 --B.[DiffOffering] 
		,0 --B.[DiffValid] 
		,0 as 'DiffValue'

	FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])
	  AND  (((
			case when ISNULL(C.[BlockedStatus],0) = @EnumBlockPeriod then 1 else 0 end != ISNULL(B.[Blocked],0)			-- Period
		 OR case when ISNULL(C.[BlockedStatus],0) = @EnumBlockPurse then 1 else 0 end != ISNULL(B.[BlockedValue],0)		-- Purse
		 OR case when ISNULL(C.[BlockedStatus],0) = @EnumBlockCard then 1 else 0 end != ISNULL(B.[BlockedCard],0)		-- Full card
		 )
		-- Filter out null in both
		AND NOT (ISNULL(C.[BlockedStatus],0) = 0 
			AND ISNULL(B.[Blocked],0) = 0
			AND ISNULL(B.[BlockedValue],0) = 0
			AND ISNULL(B.[BlockedCard],0) = 0
		  ))

	     OR ISNULL(C.[BlockedDate],'1899-12-30') != ISNULL(B.[BlockedDate],'1899-12-30') 
	     OR ISNULL(C.[BlockedDateValue],'1899-12-30') != ISNULL(B.[BlockedDateValue],'1899-12-30')
          
	     OR ISNULL(C.[BlockedDescription],'') != ISNULL(B.[BlockedDescription],'') 
	     OR ISNULL(C.[BlockedDescriptionValue],'') != ISNULL(B.[BlockedDescriptionValue],'')
		 )

	-- Not needed, this is executed first.
	--UPDATE [TravelcardDiff]
	--SET [DiffBlocked] = 1
	--	--,[IsPassBlock] = ISNULL(T.[IsPassBlock],0)
	--	--,[IsPurseBlock] = ISNULL(T.[IsPurseBlock],0)
	--	--,[IsCardBlock] = ISNULL(T.[IsCardBlock],0)
	--FROM [TravelcardDiff] T
	--INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	--INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	--WHERE (
	--		case when ISNULL(C.[BlockedStatus],0) = @EnumBlockPeriod then 1 else -1 end != ISNULL(B.[Blocked],0)		-- Period
	--	 OR case when ISNULL(C.[BlockedStatus],0) = @EnumBlockPurse then 1 else -1 end != ISNULL(B.[BlockedValue],0)	-- Purse
	--	 OR case when ISNULL(C.[BlockedStatus],0) = @EnumBlockCard then 1 else -1 end != ISNULL(B.[BlockedCard],0)		-- Full card
           
	--     OR ISNULL(C.[BlockedDate],'1990-01-01 00:00:00.001') != ISNULL(B.[BlockedDate],'1990-01-01 00:00:00.001') 
	--     OR ISNULL(C.[BlockedDateValue],'1990-01-01 00:00:00.001') != ISNULL(B.[BlockedDateValue],'1990-01-01 00:00:00.001')
          
	--     OR ISNULL(C.[BlockedDescription],'') != ISNULL(B.[BlockedDescription],'') 
	--     -- OR ISNULL(C.[BlockedDescriptionValue],'') != ISNULL(B.[BlockedDescriptionCard],'')
	--	 OR ISNULL(C.[BlockedDescriptionValue],'') != ISNULL(B.[BlockedDescriptionValue],'')
	--	 )

END
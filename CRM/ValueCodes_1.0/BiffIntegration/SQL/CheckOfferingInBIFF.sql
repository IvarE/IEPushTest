/****** Object:  StoredProcedure [dbo].[CheckOfferingInBIFF]    Script Date: 2017-08-18 13:38:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	[CheckOfferingInBIFF]
-- =============================================
alter PROCEDURE [dbo].[CheckOfferingInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
		,NULL as 'BlockedStatus'
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
		,1 as 'DiffOffering' 
		,0 --B.[DiffValid] 
		,0 as 'DiffValue'
	 FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE (ISNULL(C.[CardOfferingPeriodkort],'') != ISNULL(B.[CardOfferingPeriodkort],'') 
	    OR ISNULL(C.[CardOfferingReskassa],'') != ISNULL(B.[CardOfferingReskassa],'')
		OR C.[TicketCodeNumber] != B.[TicketCodeNumber]
		OR C.[TicketCodeNumberValue] != B.[TicketCodeNumberValue]) 
	 AND NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])


	UPDATE [TravelcardDiff]
	SET [DiffOffering] = 1
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE (ISNULL(C.[CardOfferingPeriodkort],'') != ISNULL(B.[CardOfferingPeriodkort],'') 
	    OR ISNULL(C.[CardOfferingReskassa],'') != ISNULL(B.[CardOfferingReskassa],'')
		OR C.[TicketCodeNumber] != B.[TicketCodeNumber]
		OR C.[TicketCodeNumberValue] != B.[TicketCodeNumberValue]) 

END

GO


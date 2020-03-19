SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckBlockedInBIFF
-- =============================================
CREATE PROCEDURE [dbo].[CheckBlockedInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* ADD CARDS THAT ARE DIFFERENT IN BIFF Blocked (And not already there) */
	INSERT INTO [TravelcardDiff] 
	SELECT B.*,0,0,0,0,0 
	FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])
	  AND  (
	       ISNULL(C.[Blocked],0) != ISNULL(B.[Blocked],0) 
	     OR ISNULL(C.[BlockedValue],0) != ISNULL(B.[BlockedValue],0)
	     OR ISNULL(C.[BlockedCard],0) != ISNULL(B.[BlockedCard],0)
           
	     OR ISNULL(C.[BlockedDate],'1899-12-30') != ISNULL(B.[BlockedDate],'1899-12-30') 
	     OR ISNULL(C.[BlockedDateValue],'1899-12-30') != ISNULL(B.[BlockedDateValue],'1899-12-30')
	     OR ISNULL(C.[BlockedDateCard],'1899-12-30') != ISNULL(B.[BlockedDateCard],'1899-12-30')
          
	     OR ISNULL(C.[BlockedDescription],'') != ISNULL(B.[BlockedDescription],'') 
	     OR ISNULL(C.[BlockedDescriptionValue],'') != ISNULL(B.[BlockedDescriptionValue],'')
	     OR ISNULL(C.[BlockedDescriptionCard],'') != ISNULL(B.[BlockedDescriptionCard],'')
		 )

	UPDATE [TravelcardDiff]
	SET [DiffBlocked] = 1
		,[Blocked] = ISNULL(T.[Blocked],0)
		,[BlockedValue] = ISNULL(T.[BlockedValue],0)
		,[BlockedCard] = ISNULL(T.[BlockedCard],0)
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE (
	       ISNULL(C.[Blocked],0) != ISNULL(B.[Blocked],0) 
	     OR ISNULL(C.[BlockedValue],0) != ISNULL(B.[BlockedValue],0)
	     OR ISNULL(C.[BlockedCard],0) != ISNULL(B.[BlockedCard],0)
           
	     OR ISNULL(C.[BlockedDate],'1990-01-01 00:00:00.001') != ISNULL(B.[BlockedDate],'1990-01-01 00:00:00.001') 
	     OR ISNULL(C.[BlockedDateValue],'1990-01-01 00:00:00.001') != ISNULL(B.[BlockedDateValue],'1990-01-01 00:00:00.001')
	     OR ISNULL(C.[BlockedDateCard],'1990-01-01 00:00:00.001') != ISNULL(B.[BlockedDateCard],'1990-01-01 00:00:00.001')
          
	     OR ISNULL(C.[BlockedDescription],'') != ISNULL(B.[BlockedDescription],'') 
	     OR ISNULL(C.[BlockedDescriptionValue],'') != ISNULL(B.[BlockedDescriptionCard],'')
	     OR ISNULL(C.[BlockedDescriptionCard],'') != ISNULL(B.[BlockedDescriptionCard],'')
		 )

END
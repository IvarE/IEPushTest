/****** Object:  StoredProcedure [dbo].[GetTravelCardsFromBIFF4]    Script Date: 2017-08-18 13:39:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromBIFF4
-- =============================================
CREATE PROCEDURE [dbo].[GetTravelCardsFromBIFF4] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* UPDATE OFFERINGS IN [AllUniqueBIFFtravelcards] 

	UPDATE [AllUniqueBIFFtravelcards] SET
	  [CardOfferingPeriodkort] = B.[CardOffering]
	FROM [AllUniqueBIFFtravelcards] A
	INNER JOIN [AllBIFFtravelcards] B ON A.[CardNumber] = B.[CardNumber] AND B.[CardSection] = 1 

	UPDATE [AllUniqueBIFFtravelcards] SET
	  [CardOfferingReskassa] = B.[CardOffering]
	FROM [AllUniqueBIFFtravelcards] A
	INNER JOIN [AllBIFFtravelcards] B ON A.[CardNumber] = B.[CardNumber] AND B.[CardSection] = 2 
	*/

END

GO


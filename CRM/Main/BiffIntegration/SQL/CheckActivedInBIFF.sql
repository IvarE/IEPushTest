/****** Object:  StoredProcedure [dbo].[CheckActivedInBIFF]    Script Date: 2017-08-18 13:38:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckActivatedInBIFF
-- =============================================
CREATE PROCEDURE [dbo].[CheckActivedInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* MARK CARDS THAT ARE DIFFERENT IN BIFF */
	INSERT INTO [TravelcardDiff] 
	SELECT B.*,0,0,0,0,0 FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE C.[Activated] != B.[Activated] AND NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])

	UPDATE [TravelcardDiff]
	SET [DiffActivated] = 1
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE C.[Activated] != B.[Activated]

END

GO


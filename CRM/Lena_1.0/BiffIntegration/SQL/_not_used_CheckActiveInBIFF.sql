SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckActiveInBIFF
-- =============================================
CREATE PROCEDURE [dbo].[CheckActiveInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* MARK CARDS THAT ARE DIFFERENT IN BIFF */
	INSERT INTO [TravelcardDiff] 
	SELECT B.*,0,0,0,0,0 FROM [AllUniqueBIFFtravelcards] B
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE C.[IsActive] != B.[IsActive] AND NOT C.[CardNumber] IN (SELECT [CardNumber] FROM [TravelcardDiff])

	UPDATE [TravelcardDiff]
	SET [DiffActive] = 1
	FROM [TravelcardDiff] T
	INNER JOIN [AllUniqueBIFFtravelcards] B ON T.[CardNumber] = B.[CardNumber]
	INNER JOIN [CRMtravelcards] C ON C.[CardNumber] = B.[CardNumber]
	WHERE C.[IsActive] != B.[IsActive]

END

GO


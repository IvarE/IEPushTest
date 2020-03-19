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
CREATE PROCEDURE [dbo].[CheckValidDatesInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* MARK CARDS THAT ARE DIFFERENT IN BIFF */
	INSERT INTO [TravelcardDiff] 
	SELECT B.*,0,0,0,0,0 FROM [AllUniqueBIFFtravelcards] B
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


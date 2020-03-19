/****** Object:  StoredProcedure [dbo].[GetTravelCardsFromBIFF3]    Script Date: 2017-08-18 13:39:24 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromBIFF3
-- =============================================
CREATE PROCEDURE [dbo].[GetTravelCardsFromBIFF3] 
AS
BEGIN
	/* CREATE A NEW TABLE WITH UNIQUE BIFF-VALUES IN [AllUniqueBIFFtravelcards] */


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @MaxCardNumber bigint
	DECLARE @rowsAffected bigint
	select @MaxCardNumber = -1
	select @rowsAffected = 1	-- To run

	TRUNCATE TABLE [AllUniqueBIFFtravelcards]

	WHILE(@rowsAffected > 0)
	BEGIN

		INSERT INTO [AllUniqueBIFFtravelcards] 
		SELECT top 10000
			[CardNumber],
			0,
			0,
			MAX([Activated]),
			MAX([IsActive]),
			-- Periodkort
			(SELECT MAX([Blocked]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as 'Blocked',
			(SELECT MAX([BlockedDate]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as 'BlockedDate',
			(SELECT MAX([BlockedDescription]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as'BlockedDescription',
			-- Reskassa			
			(SELECT MAX([Blocked]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'BlockedValue',
			(SELECT MAX([BlockedDate]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'BlockedDateValue',
			(SELECT MAX([BlockedDescription]) FROM [AllBIFFtravelcards] A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'BlockedDescriptionValue',
			-- Hela kortet
			NULL as 'BlockedCard',
			NULL as 'BlockedDateCard',
			NULL as 'BlockedDescriptionCard',
			MAX([ValidFromDate]),
			MAX([ValidToDate]),
			MAX([LatestTravelDate]),
			MAX([ModifiedDate])
		FROM [AllBIFFtravelcards] B
		WHERE B.[CardNumber] > @MaxCardNumber
		GROUP BY B.[CardNumber]
		order by B.[CardNumber]

		SELECT @rowsAffected = @@rowcount

		select @MaxCardNumber = max([CardNumber])
		  FROM [AllUniqueBIFFtravelcards]

	END

END

GO


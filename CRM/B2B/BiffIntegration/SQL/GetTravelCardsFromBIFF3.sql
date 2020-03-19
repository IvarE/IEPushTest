--Aktuellt index i skrivande stund:
--CREATE NONCLUSTERED INDEX IX_ED_BIFFTravelCards_CardNumber ON AllBIFFtravelcards (CardNumber);
--...som kan tas bort när riktiga primärnyckeln läses på igen


SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromBIFF3
-- =============================================
alter PROCEDURE [dbo].[GetTravelCardsFromBIFF3] 
AS
BEGIN
	/* CREATE A NEW TABLE WITH UNIQUE BIFF-VALUES IN [AllUniqueBIFFtravelcards] */


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @MaxCardNumber bigint
	DECLARE @rowsAffected bigint
	DECLARE @Interval int
	select @MaxCardNumber = -1
	select @rowsAffected = 1	-- To run
	select @Interval = 100000


	TRUNCATE TABLE [AllUniqueBIFFtravelcards]

	DECLARE @TravelCardNumber TABLE (CardNumber BIGINT NOT NULL PRIMARY KEY CLUSTERED);

	-- Perform insert of first batch of CardNumber values to working table:
	;WITH CTE AS (SELECT TOP (@Interval)
		CardNumber
		FROM AllBIFFtravelcards WITH (READUNCOMMITTED)
		WHERE CardNumber > @MaxCardNumber
		ORDER BY CardNumber
	)
	INSERT INTO @TravelCardNumber (CardNumber)
	SELECT DISTINCT CardNumber FROM CTE;	
	
	--Set Max CardNumber value:
	SELECT @MaxCardNumber = MAX(CardNumber)
	FROM @TravelCardNumber;

	WHILE(@rowsAffected > 0)
	BEGIN

		;WITH BIFF_CTE AS (
			SELECT 
				B.CardNumber,
				B.Activated,
				B.IsActive,
				B.Blocked,
				B.BlockedDate,
				B.BlockedDescription,
				B.CardSection,
				B.ValidFromDate,
				B.ValidToDate,
				B.LatestTravelDate,
				B.ModifiedDate,
				B.IsPassBlock, 
				B.IsPurseBlock, 
				B.IsCardBlock,
				B.TicketCodeNumber,
				B.Balance,
				B.CardOffering
			FROM [AllBIFFtravelcards] AS B
			INNER JOIN @TravelCardNumber AS T
			ON B.CardNumber = T.CardNumber
		)
		INSERT INTO [AllUniqueBIFFtravelcards] 
		SELECT 
			B.[CardNumber],
			(SELECT MAX([CardOffering]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'CardOfferingReskassa',		-- CardSection 2 = Reskassa
			(SELECT MAX([CardOffering]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as 'CardOfferingPeriodkort',		-- CardSection 1 = Period
			MAX(B.[Activated]),
			MAX(B.[IsActive]),
			-- Periodkort
			MAX(B.[IsPassBlock]) as 'Blocked',
			(SELECT MAX([BlockedDate]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as 'BlockedDate',
			(SELECT MAX([BlockedDescription]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as'BlockedDescription',
			-- Reskassa			
			MAX(B.[IsPurseBlock]) as 'BlockedValue',
			(SELECT MAX([BlockedDate]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'BlockedDateValue',
			(SELECT MAX([BlockedDescription]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'BlockedDescriptionValue',
			-- Hela kortet
			MAX(B.[IsCardBlock]) as 'BlockedCard',
			NULL as 'BlockedDateCard',				-- Not available 171220
			NULL as 'BlockedDescriptionCard',		-- Not available 171220
			MAX(B.[ValidFromDate]),
			MAX(B.[ValidToDate]),
			MAX(B.[LatestTravelDate]),
			MAX(B.[ModifiedDate]),
			MAX(ISNULL(B.[Balance],0)),
			-- Typ av tjänst, t.ex. 2161
			(SELECT MAX([TicketCodeNumber]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 1) as 'TicketCodeNumber',
			(SELECT MAX([TicketCodeNumber]) FROM BIFF_CTE A WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = 2) as 'TicketCodeNumberValue'
		FROM BIFF_CTE AS B
		GROUP BY B.[CardNumber]
		ORDER BY B.[CardNumber];

		--Delete currently handled batch from working table:
		DELETE FROM @TravelCardNumber;

		--Fill working table with next batch of card numbers:
		;WITH CTE AS (SELECT TOP (@Interval)
			CardNumber
			FROM AllBIFFtravelcards WITH (READUNCOMMITTED)
			WHERE CardNumber > @MaxCardNumber
			ORDER BY CardNumber
		)
		INSERT INTO @TravelCardNumber (CardNumber)
		SELECT DISTINCT CardNumber FROM CTE;
		
		--Select number of rows affected (used as stop condition in while loop, no rows left = we're done)
		SELECT @rowsAffected = @@ROWCOUNT;
		
		--Set new Max CardNumber value:
		SELECT @MaxCardNumber = MAX(CardNumber)
		FROM @TravelCardNumber;
	END
END
GO
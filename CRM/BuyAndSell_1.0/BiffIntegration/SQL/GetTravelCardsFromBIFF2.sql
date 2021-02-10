SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromBIFF2
-- Johan A 31/10-17. Clear AllBifftravelcards table. 
-- =============================================
CREATE PROCEDURE [dbo].[GetTravelCardsFromBIFF2] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* ADD ALL UPDATES IN [AllBIFFtravelcards] */
	Truncate table AllBIFFtravelcards

	INSERT INTO [AllBIFFtravelcards] SELECT
		A.[CardNumber],
		A.[CardSection],
		A.[OfferingName],
		A.[TicketCodeNumber],
		iif(A.[Activated]='Aktiv',1,2), 
		A.[IsActive],
		iif(A.[BlockState] > 0,1,0),
		A.[BlockDate],
		A.[BlockDescription],
		A.[PeriodStartDay],
		A.[ExpireDay],
		A.[LatestTravelDate],
		A.[ModifiedOn],
		A.[IsPassBlock], 
		A.[IsPurseBlock], 
		A.[IsCardBlock],
		A.[Balance] 
	FROM [BIFFtravelcards] A 

	/* TA BORT GAMLA RADER */
	--DELETE A FROM [AllBIFFtravelcards] A
	--WHERE EXISTS (SELECT * FROM [AllBIFFtravelcards] B
	--WHERE A.[CardNumber] = B.[CardNumber] AND A.[CardSection] = B.[CardSection] AND A.[ModifiedDate] < B.[ModifiedDate])

END


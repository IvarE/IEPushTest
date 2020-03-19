/****** Object:  StoredProcedure [dbo].[GetTravelCardsFromBIFF1]    Script Date: 2017-08-18 13:39:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromBIFF1
-- =============================================
CREATE PROCEDURE [dbo].[GetTravelCardsFromBIFF1] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* EMPTY AND ADD ALL UPDATES IN BIFF IN [BIFFtravelcards] */

	--TRUNCATE TABLE [BIFFtravelcards]

	/*
	INSERT INTO [BIFFtravelcards] 
	SELECT  [Activated],
			[BlockDate],
			[BlockDescription],
			[BlockState],
			[CardNumber],
			[CardSection],
			[ExpireDay],
			[IsActive],
			[LatestTravelDate],
			[ModifiedOn], 
			[PeriodStartDay],
			[TicketCodeNumber]
	FROM [V-DW-TST].[STDW].[dbo].[GetCardView]
	WHERE [ModifiedOn] >= (SELECT MAX([LatestModifiedDate]) 
							FROM LoadDates 
							WHERE [Source] = 'GetCardView') */

	INSERT INTO [dbo].[LoadDates] 
	SELECT 'GetCardView', MAX([ModifiedOn])
	FROM [BIFFtravelcards]

END

GO


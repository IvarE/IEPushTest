/****** Object:  StoredProcedure [dbo].[RunAll]    Script Date: 2017-08-18 13:39:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	RunAll
-- =============================================
CREATE PROCEDURE [dbo].[RunAll] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* Öppna eventuellt brandväggar och länka databasservrar (Server Objects/Linked Servers) till V-DW och V-DKSQL5. */
	/* Ändra [GetTravelCardsFromBIFF1] så att GetCardView hämtar från direkt från V-DW.STDW.GetCardView */
	/* Ändra [GetTravelCardsFromCRM] så att FilteredCgi_Travelcard hämtar direkt från V-DKSQL5.DKCRM_MSCRM.FilteredCgi_Travelcard */


	EXEC CreateTables
	EXEC GetTravelCardsFromBIFF1 /* läs in ändrad rådata till BIFFtravelcards 86s */  
	EXEC GetTravelCardsFromBIFF2 /* läs in till AllBIFFtravelcards 92s */
	EXEC GetTravelCardsFromBIFF3 /* läs in till AllUniqueBIFFtravelcards 72s */
	EXEC GetTravelCardsFromBIFF4 /* uppdatera AllUniqueBIFFtravelcards 50s */
	EXEC GetTravelCardsFromCRM	 /* läs in till CRMtravelcards 20s*/

	EXEC CheckMissingInBIFF
	EXEC CheckBlockedInBIFF
	EXEC CheckOfferingInBIFF
	EXEC CheckValidDatesInBIFF
	EXEC CheckActiveInBIFF

	EXEC CheckActivedInBIFF /*??????*/

	
	/*************************************************/
	select * from TravelCardError
	select * from TravelCardDiff
	truncate table TravelCardDiff
	truncate table TravelCardError

	/*************************************************/
	select * from GetCardView where CardNumber = 3522256859
	select * from BIFFtravelcards where cardnumber = 3522256859
	select * from AllBIFFtravelcards where cardnumber = 3522256859
	select * from AllUniqueBIFFtravelcards where cardnumber = 3522256859
	select * from FilteredCgi_Travelcard where cgi_travelcardnumber like '%3522256859%'
	select * from CRMtravelcards where cardnumber = 3522256859
	/*************************************************/
	select * from GetCardView where CardNumber = 1237033014
	select * from BIFFtravelcards where cardnumber = 1237033014
	select * from AllBIFFtravelcards where cardnumber = 1237033014
	select * from AllUniqueBIFFtravelcards where cardnumber = 1237033014
	select * from FilteredCgi_Travelcard where cgi_travelcardnumber like '%1237033014%'
	select * from CRMtravelcards where cardnumber = 1237033014
	/*************************************************/
	select * from GetCardView where CardNumber = 17255946
	select * from BIFFtravelcards where cardnumber = 17255946
	select * from AllBIFFtravelcards where cardnumber = 17255946
	select * from AllUniqueBIFFtravelcards where cardnumber = 17255946
	select * from FilteredCgi_Travelcard where cgi_travelcardnumber like '%17255946%'
	select * from CRMtravelcards where cardnumber = 17255946
	
	select * from GetCardView where activated = 'Ej Aktiv'


END

GO


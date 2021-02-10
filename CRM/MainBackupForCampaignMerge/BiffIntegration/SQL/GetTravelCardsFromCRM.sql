SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	GetTravelCardsFromCRM
-- =============================================
create PROCEDURE [dbo].[GetTravelCardsFromCRM] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* EMPTY AND ADD ALL UPDATES IN CRM AND BIFF IN [CRMtravelcards] */

	TRUNCATE TABLE [CRMtravelcards]

	INSERT INTO [CRMtravelcards] 
	SELECT DISTINCT
	SUBSTRING([cgi_travelcardnumber], PATINDEX('%[^0]%', [cgi_travelcardnumber]+'.'), LEN([cgi_travelcardnumber])),
	[cgi_valuecardtypeid],
	[cgi_periodcardtypeid],
	[statuscode], /* 1=Active, 2=Inactive */
	[statecode], 

	-- Periodkort
	[cgi_blocked],
	Dateadd(hh,2, [ed_BlockedDate]) as 'ed_BlockedDate',				
	[ed_BlockedDescription],		
	-- Reskassa
	[ed_BlockedValue],
	Dateadd(hh,2, [ed_BlockedDateValue]) as 'ed_BlockedDateValue',				
	[ed_BlockedDescriptionValue],		
	-- Hela kortet spärrat
	[ed_BlockedCard],
	Dateadd(hh,2, [ed_BlockedDateCard]) as 'ed_BlockedDateCard',				
	[ed_BlockedDescriptionCard],		

	Dateadd(hh,2, [cgi_validfrom]) as 'cgi_validfrom',
	Dateadd(hh,2, [cgi_validto]) as 'cgi_validto',
	Dateadd(hh,2, [ed_LastTravelDate]) as 'ed_LastTravelDate', 
	Dateadd(hh,2, [modifiedon]) as 'modifiedon'
	FROM DKCRM_MSCRM.dbo.cgi_travelcardBase 
	WHERE ISNUMERIC([cgi_travelcardnumber] ) = 1 AND
	([modifiedon] >= (SELECT MAX([LatestModifiedDate]) FROM LoadDates WHERE [Source] = 'Filteredcgi_Travelcard')
	OR
	SUBSTRING([cgi_travelcardnumber], PATINDEX('%[^0]%', [cgi_travelcardnumber]+'.'), LEN([cgi_travelcardnumber])) IN
	(SELECT [CardNumber] FROM [BIFFtravelcards]))

	/* DELETE TRANSACTION ROWS IN CRM */
	DELETE A FROM [CRMtravelcards] A
	WHERE EXISTS (SELECT * FROM [CRMtravelcards] B
	WHERE A.[CardNumber] = B.[CardNumber] AND A.[ModifiedDate] < B.[ModifiedDate])
	
	INSERT INTO LoadDates 
	SELECT 'Filteredcgi_Travelcard', MAX([ModifiedDate])
	FROM [CRMtravelcards]

END

GO


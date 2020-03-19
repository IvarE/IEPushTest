/****** Object:  StoredProcedure [dbo].[CheckMissingInBIFF]    Script Date: 2017-08-18 13:38:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Björn Stadig
-- Create date: 27mar2017
-- Description:	CheckMissingInBIFF
-- =============================================
CREATE PROCEDURE [dbo].[CheckMissingInBIFF] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* MARK CARDS THAT EXISTS IN CRM BUT ARE MISSING IN BIFF */

	INSERT INTO [TravelcardError]
	SELECT DISTINCT 'CARDNO NOT FOUND IN BIFF', * FROM [CRMtravelcards] 
	WHERE [CardNumber] NOT IN (SELECT [CardNumber] FROM [AllUniqueBIFFtravelcards])
	 
	DELETE FROM [CRMtravelcards]
	WHERE [CardNumber] IN (SELECT [CardNumber] FROM [TravelcardError])

END

GO


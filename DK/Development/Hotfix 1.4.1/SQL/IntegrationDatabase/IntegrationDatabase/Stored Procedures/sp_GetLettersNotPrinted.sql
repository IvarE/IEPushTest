-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLettersNotPrinted] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

    SELECT a.incidentid FROM [$(Skanetrafiken_MSCRM)].dbo.FilteredIncident a
	WHERE a.cgi_letter_templateid IS NOT null AND a.incidentid NOT IN
	(SELECT DISTINCT incidentid FROM letter_template)

END

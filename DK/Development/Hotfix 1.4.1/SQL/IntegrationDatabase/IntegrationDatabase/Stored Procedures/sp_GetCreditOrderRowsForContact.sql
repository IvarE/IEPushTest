-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-24
-- Description:	Get creditorderrows on caseid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCreditOrderRowsForContact]
	@customerid			uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	a.cgi_accountid,
	a.cgi_contactid,
	a.cgi_date,
	a.cgi_message,
	a.cgi_name,
	a.cgi_ordernumber,
	a.cgi_referencenumber,
	a.cgi_success,
	a.cgi_sum,
	a.cgi_time,
	a.cgi_productnumber,
	a.cgi_reason,
	a.cgi_createdby
	from [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_creditorderrow a
	where a.cgi_contactid = @customerid
	for xml path ('creditrow'), root ('creditrows'), type

	REVERT

END



USE [IntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSavedTravelCardTransactions]    Script Date: 2015-09-21 13:23:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-06
-- Modify date: 2015-09-15 (Henric Fröberg, added fields returned)
-- Description:	get saved travelcard transactions
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetSavedTravelCardTransactions] 
	@transactionid		uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	travelcardtransaction.cgi_cardsect, 
	travelcardtransaction.cgi_caseid, 
	travelcardtransaction.cgi_caseidname, 
	travelcardtransaction.cgi_amount,
	travelcardtransaction.cgi_currency, 
	travelcardtransaction.cgi_date, 
	travelcardtransaction.cgi_deviceid, 
	travelcardtransaction.cgi_origzone, 
	travelcardtransaction.cgi_origzonename,
	travelcardtransaction.cgi_rectype, 
	travelcardtransaction.cgi_route, 
	travelcardtransaction.cgi_time, 
	travelcardtransaction.cgi_travelcard,
	travelcardtransaction.cgi_travelcardid, 
	travelcardtransaction.cgi_travelcardidname, 
	travelcardtransaction.cgi_travelcardtransaction, 
	travelcardtransaction.cgi_travelcardtransactionid, 
	travelcardtransaction.cgi_txnnum, 
	travelcardtransaction.cgi_txntype
	from DKCRMUAT_MSCRM.dbo.Filteredcgi_travelcardtransaction travelcardtransaction
	where travelcardtransaction.cgi_caseid = @transactionid
	for xml path ('transaction'), root ('transactions'), type

	REVERT

END

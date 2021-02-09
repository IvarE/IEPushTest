USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetCreditOrderRowsForContact]    Script Date: 2015-04-24 13:36:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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
	a.cgi_productnumber
	from DKCRM_MSCRM.dbo.Filteredcgi_creditorderrow a
	where a.cgi_contactid = @customerid
	for xml path ('creditrow'), root ('creditrows'), type

	REVERT

END




GO


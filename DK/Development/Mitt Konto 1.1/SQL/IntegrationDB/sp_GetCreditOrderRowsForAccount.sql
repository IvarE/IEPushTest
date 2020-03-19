USE [IntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCreditOrderRowsForAccount]    Script Date: 2015-09-04 13:33:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-24
-- Description:	Get creditorderrows on caseid
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetCreditOrderRowsForAccount]
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
	from DKCRMUAT_MSCRM.dbo.Filteredcgi_creditorderrow a
	where a.cgi_accountid = @customerid
	for xml path ('creditrow'), root ('creditrows'), type

	REVERT

END
USE [IntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTravelCardExtended]    Script Date: 2015-10-08 14:13:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-04
-- Description:	Get information of travelcard from crm
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetTravelCardExtended] 
	@cardnumber		varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	travelcard.cgi_accountid,
	travelcard.cgi_accountidname,
	travelcard.cgi_autoloadconnectiondate,
	travelcard.cgi_autoloaddisconnectiondate,
	travelcard.cgi_creditcardmask,
	travelcard.cgi_contactid,
	travelcard.cgi_contactidname,
	travelcard.cgi_failedattemptstochargemoney,
	travelcard.cgi_travelcardid,
	travelcard.cgi_blocked,
	travelcard.cgi_cardtypeid,
	travelcard.cgi_cardtypeidname,
	travelcard.cgi_numberofzones,
	travelcard.cgi_periodic_card_type,
	travelcard.cgi_travelcardname,
	travelcard.cgi_validfrom,
	travelcard.cgi_validto,
	travelcard.cgi_value_card_type,
	travelcard.statecode,
	travelcard.cgi_autoloadstatus,
	travelcard.cgi_autoloadconnectiondate,
	travelcard.cgi_autoloaddisconnectiondate,
	contact.cgi_contactnumber,
	account.accountnumber
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_travelcard travelcard
	left outer join Skanetrafiken_MSCRM.dbo.FilteredContact contact on travelcard.cgi_contactid = contact.contactid
	left outer join Skanetrafiken_MSCRM.dbo.FilteredAccount account on travelcard.cgi_accountid = account.accountid
	where travelcard.cgi_travelcardnumber = @cardnumber
	and travelcard.statecode = 0
	for xml path ('card'), root ('cards'), type

	REVERT

END
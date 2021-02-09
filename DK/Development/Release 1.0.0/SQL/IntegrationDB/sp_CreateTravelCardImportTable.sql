USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateTravelCardImportTable]    Script Date: 2015-04-24 10:46:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-27
-- Description:	Populate importtable for travelcards
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateTravelCardImportTable] 

AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	delete from travelcards

	select
	travelcard.cgi_importid,
	travelcard.cgi_travelcardid,
	travelcard.cgi_contactid,
	travelcard.cgi_travelcardname,
	travelcard.cgi_travelcardnumber
	into #travelcards
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_travelcard travelcard
	where travelcard.cgi_importid is not null

	insert into travelcards (customer_id, cardnumber)
	select
	customer_id, cardnumber
	from contactimport
	order by customer_id

	update travelcards
	set travelcardid = '00000000-0000-0000-0000-000000000000' ,
	contactidx = '00000000-0000-0000-0000-000000000000'

	update travelcards
	set travelcards.travelcardid = #travelcards.cgi_travelcardid
	from travelcards
	inner join #travelcards
	on travelcards.customer_id collate Latin1_General_CI_AI = #travelcards.cgi_importid
	
	update travelcards
	set travelcards.contactidx = contact.contactid
	from travelcards
	inner join Skanetrafiken_MSCRM.dbo.FilteredContact contact
	on travelcards.customer_id collate Latin1_General_CI_AI = contact.cgi_importid
	
	drop table #travelcards

	REVERT

END


GO


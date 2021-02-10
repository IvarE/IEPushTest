USE [IntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2016-03-24 11:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on emailaddress
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetAccountFromEmail] 
	@emailaddress	varchar(255),
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.firstname as firstname,
		contact.lastname as lastname,
		contact.cgi_socialsecuritynumber as socialsecuritynumber,
		contact.emailaddress1 as emailaddress,
		contact.telephone2 as telephone2,
		contact.address1_line1 as address1_line1,
		contact.address1_line2 as address1_line2,
		contact.address1_postalcode as address1_postalcode,
		contact.address1_city as address1_city,
		contact.address1_country as address1_country
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where contact.emailaddress1 = @emailaddress
		and contact.statecode = 0
		ORDER BY contact.createdon DESC
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
		account.accountid as accountid,
		account.accountnumber as accountnumber,
		account.name as name,
		account.emailaddress1 as emailaddress,
		account.telephone2 as telephone2,
		account.address1_line1 as address1_line1,
		account.address1_line2 as address1_line2,
		account.address1_postalcode as address1_postalcode,
		account.address1_city as address1_city,
		account.address1_country as address1_country
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
		where account.emailaddress1 = @emailaddress
		and account.statecode = 0
		ORDER BY account.createdon DESC
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END



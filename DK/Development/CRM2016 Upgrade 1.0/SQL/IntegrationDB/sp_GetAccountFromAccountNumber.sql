USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountNumber]    Script Date: 2015-04-24 10:46:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on accountnumber
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromAccountNumber] 
	@accountnumber	varchar(255),
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
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
			where contact.cgi_contactnumber = @accountnumber
			and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
			account.accountid as accountid,
			account.accountnumber as accountnumber,
			account.name as name,
			account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
			where account.accountnumber = @accountnumber
			and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END




GO


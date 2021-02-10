USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2015-04-24 12:43:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on emailaddress
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromEmail] 
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
		contact.emailaddress1 as emailaddress
		from DKCRMUAT_MSCRM.dbo.FilteredContact contact
		where contact.emailaddress1 = @emailaddress
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
		from DKCRMUAT_MSCRM.dbo.FilteredAccount account
		where account.emailaddress1 = @emailaddress
		and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END




GO


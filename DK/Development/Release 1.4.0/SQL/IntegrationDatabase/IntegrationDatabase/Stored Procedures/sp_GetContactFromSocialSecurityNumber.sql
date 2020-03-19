



-- =============================================
-- Author:		Per Lindblom
-- Create date: 2016-03-03
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetContactFromSocialSecurityNumber] 
	@SocialSecurityNumber           varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from [$(Skanetrafiken_MSCRM)].dbo.FilteredContact contact
		where 
		 LTRIM(RTRIM(contact.cgi_socialsecuritynumber))=LTRIM(RTRIM(@SocialSecurityNumber))
		AND contact.statecode = 0
		ORDER BY contact.createdon DESC

		for xml path ('account'), root ('accounts'), type
		
	
	REVERT

END





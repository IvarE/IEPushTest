


-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetContactFromNameAndAddress] 
	@FirstName           varchar(255), 
	@LastName         	   varchar(255),
	@Address_Line2    	   varchar(255),
	@Address_PostalCode	   varchar(255),
	@Address_City     	   varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.firstname as firstname,
		contact.lastname as lastname,
		contact.cgi_socialsecuritynumber as socialsecuritynumber,
		contact.emailaddress1 as emailaddress,
		contact.telephone2 as telephone2,
		contact.telephone3 as telephone3,
		contact.address1_line1 as address1_line1,
		contact.address1_line2 as address1_line2,
		contact.address1_postalcode as address1_postalcode,
		contact.address1_city as address1_city,
		contact.address1_country as address1_country
		from DKCRM_MSCRM.dbo.FilteredContact contact
		where 
		 LTRIM(RTRIM(contact.firstname))=LTRIM(RTRIM(@FirstName))
		AND LTRIM(RTRIM( contact.lastname))=LTRIM(RTRIM(@LastName))
		AND LTRIM(RTRIM( contact.address1_line2))=LTRIM(RTRIM(@Address_Line2))
		AND LTRIM(RTRIM( contact.address1_postalcode))=	LTRIM(RTRIM(@Address_PostalCode))
		AND LTRIM(RTRIM( contact.address1_city))=LTRIM(RTRIM(@Address_City))
		AND contact.statecode = 0
		ORDER BY contact.createdon DESC

		for xml path ('account'), root ('accounts'), type
		
	
	REVERT

END




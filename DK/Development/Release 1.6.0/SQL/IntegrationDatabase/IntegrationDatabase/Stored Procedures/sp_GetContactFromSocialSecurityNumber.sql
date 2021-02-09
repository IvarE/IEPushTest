USE [IntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetContactFromSocialSecurityNumber]    Script Date: 2016-03-24 13:47:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Per Lindblom
-- Create date: 2016-03-03
-- Description: 
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetContactFromSocialSecurityNumber] 
	@SocialSecurityNumber           varchar(255)
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
		contact.address1_line1 as address1_line1,
		contact.address1_line2 as address1_line2,
		contact.address1_postalcode as address1_postalcode,
		contact.address1_city as address1_city,
		contact.address1_country as address1_country
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where 
		 LTRIM(RTRIM(contact.cgi_socialsecuritynumber))=LTRIM(RTRIM(@SocialSecurityNumber))
		AND contact.statecode = 0
		ORDER BY contact.createdon DESC

		for xml path ('account'), root ('accounts'), type
		
	
	REVERT

END





-- =============================================
-- Author:		Max Persson
-- Create date: 2016-03-04
-- Description:	Procedure for the ReportPrintBulk
-- =============================================
CREATE PROCEDURE sp_RefundPrintBulk
	-- Add the parameters for the stored procedure her
	--@RefundID varchar(max)
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

   DECLARE @incidentid AS varchar(max) 
   DECLARE @RefundID AS varchar(max) 

   SET @RefundID = 'F641E8D0-EC4F-E511-80DF-0050569010AD'

   --SELECT a.cgi_caseid
   --FROM  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_refund a
   --WHERE a.cgi_refundid = @RefundID
   
    
   
   SELECT systemuserid, cgi_emailphrase AS contactdetails
   INTO #test1
   FROM  [$(Skanetrafiken_MSCRM)].dbo.FilteredSystemUser
   
   SELECT        a.cgi_refundid, d .cgi_printtext
   INTO #test2
   FROM  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_refund a 
	INNER JOIN  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_cgi_refundtype_cgi_reimbursementform b ON a.cgi_refundtypeid = b.cgi_refundtypeid 
	INNER JOIN  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_reimbursementform c ON b.cgi_reimbursementformid = c.cgi_reimbursementformid 
	INNER JOIN [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_refundtype d ON b.cgi_refundtypeid = d .cgi_refundtypeid
   WHERE a.cgi_refundid = @RefundID AND c.cgi_print = 1

SELECT        ticketnumber, Filteredcgi_refund.cgi_last_valid, Filteredcgi_refund.cgi_amount, FilteredSystemUser.fullname,
    #test1.contactdetails, FilteredIncident.createdon, cgi_actiondate, Filteredcgi_amounttranslation.cgi_name, #test2.cgi_printtext, 
    CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_name WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.fullname WHEN FilteredIncident.cgi_accountid IS NOT NULL THEN FilteredAccount.name END AS Name, 
    CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_streetaddress WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.address1_line2 WHEN FilteredIncident.cgi_accountid IS NOT NULL 
    THEN FilteredAccount.address1_line2 END AS PostAddress, CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_zippostalcode WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.address1_postalcode WHEN FilteredIncident.cgi_accountid IS NOT NULL 
    THEN FilteredAccount.address1_postalcode END AS PostalCode, CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_city WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.address1_city WHEN FilteredIncident.cgi_accountid IS NOT NULL 
    THEN FilteredAccount.address1_city END AS PostCity, CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_country WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.address1_country WHEN FilteredIncident.cgi_accountid IS NOT NULL 
    THEN FilteredAccount.address1_country END AS Country, CASE WHEN FilteredIncident.cgi_representativid IS NOT NULL 
    THEN Filteredcgi_representative.cgi_coaddress WHEN FilteredIncident.cgi_contactid IS NOT NULL 
    THEN FilteredContact.address1_line1 WHEN FilteredIncident.cgi_accountid IS NOT NULL 
    THEN FilteredAccount.address1_line1 END AS CO
FROM [$(Skanetrafiken_MSCRM)].dbo.FilteredIncident 
INNER JOIN  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_refund ON Filteredcgi_refund.cgi_caseid = FilteredIncident.incidentid 
LEFT OUTER JOIN  [$(Skanetrafiken_MSCRM)].dbo.FilteredAccount ON FilteredIncident.accountid = FilteredAccount.accountid 
LEFT OUTER JOIN  [$(Skanetrafiken_MSCRM)].dbo.FilteredContact ON FilteredIncident.contactid = FilteredContact.contactid 
LEFT OUTER JOIN  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_representative ON FilteredIncident.cgi_representativid = Filteredcgi_representative.cgi_representativeid 
LEFT OUTER JOIN  [$(Skanetrafiken_MSCRM)].dbo.FilteredSystemUser ON FilteredIncident.ownerid = FilteredSystemUser.systemuserid 
LEFT OUTER JOIN #test1 ON FilteredIncident.ownerid = #test1.systemuserid 
LEFT OUTER JOIN  [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_amounttranslation ON Filteredcgi_refund.cgi_amount = Filteredcgi_amounttranslation.cgi_amount 
LEFT OUTER JOIN #test2 ON Filteredcgi_refund.cgi_refundid = #test2.cgi_refundid
WHERE Filteredcgi_refund.cgi_refundid = @RefundID AND (FilteredIncident.cgi_accountid IS NOT NULL 
OR FilteredIncident.cgi_contactid IS NOT NULL) 

DROP TABLE #test1 

DROP TABLE #test2

END

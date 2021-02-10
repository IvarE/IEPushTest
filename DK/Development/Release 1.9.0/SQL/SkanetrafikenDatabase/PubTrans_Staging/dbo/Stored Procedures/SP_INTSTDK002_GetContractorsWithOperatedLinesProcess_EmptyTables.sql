-- =============================================
-- Author:		Daniel Ceder
-- Create date: 2014-12-16
-- Description:	Deletes all data from 
-- =============================================
CREATE PROCEDURE dbo.SP_INTSTDK002_GetContractorsWithOperatedLinesProcess_EmptyTables

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from PubTrans_Staging.dbo.Contractor
	delete from PubTrans_Staging.dbo.OrganisationalUnit
	delete from PubTrans_Staging.dbo.TransportAuthority
END

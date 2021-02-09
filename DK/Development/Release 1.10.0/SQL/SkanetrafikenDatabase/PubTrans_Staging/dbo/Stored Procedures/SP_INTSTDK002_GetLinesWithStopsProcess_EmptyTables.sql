-- =============================================
-- Author:		Daniel Ceder
-- Create date: 2014-12-16
-- Description:	Deletes all data from 
-- =============================================
CREATE PROCEDURE dbo.SP_INTSTDK002_GetLinesWithStopsProcess_EmptyTables

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from PubTrans_Staging.dbo.DirectionOfLine
	delete from PubTrans_Staging.dbo.GroupOfLines
	delete from PubTrans_Staging.dbo.Line
	delete from PubTrans_Staging.dbo.LineInGroupOfLines
	delete from PubTrans_Staging.dbo.StopOnLine
END

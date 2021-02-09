-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Ceder
-- Create date: 2014-12-16
-- Description:	Deletes all data from 
-- =============================================
CREATE PROCEDURE dbo.SP_INTSTDK002_GetZonesProcess_EmptyTables

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from PubTrans_Staging.dbo.Zone
	delete from PubTrans_Staging.dbo.LineInZone
END
GO
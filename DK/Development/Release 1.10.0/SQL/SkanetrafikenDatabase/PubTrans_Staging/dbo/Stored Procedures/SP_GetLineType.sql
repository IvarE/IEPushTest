
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLineType]  
	-- Add the parameters for the stored procedure here
	@lineGID numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/****** Script for SelectTopNRows command from SSMS  ******/
	SELECT TOP 1
	 GOL.Name
	  FROM [PubTrans_Staging].[dbo].Line as L
	  join [PubTrans_Staging].[dbo].[LineInGroupOfLines] as LIGL on LIGL.IsLineId = L.Id
	  join [PubTrans_Staging].[dbo].GroupOfLines as GOL on LIGL.IsInGroupOfLineId = GOL.Id	 
	  WHERE L.Gid = @lineGID
	  and GOL.PurposeOfGroupingCode = 'PRODUCT'
END


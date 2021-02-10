USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetLineType]    Script Date: 2015-11-23 18:06:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLineType]  
	-- Add the parameters for the stored procedure here
	@lineDesignation numeric
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
	  WHERE L.Number = @lineDesignation
	  and GOL.PurposeOfGroupingCode = 'PRODUCT'
END

GO



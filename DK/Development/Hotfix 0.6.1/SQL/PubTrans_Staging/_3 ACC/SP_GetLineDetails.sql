USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetLineDetails]    Script Date: 2015-04-24 12:50:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Suman Subramanian
-- Create date: 06-29-2014
-- Description: Procedure for getting the line Details.
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLineDetails] @LineType varchar(50) 
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	select 
		LD.LineDetails LineDetails 
	from 
		LineDetails LD
	Where
		LD.LineType=@LineType
	And
		LD.CreatedOn = CONVERT(varchar(10),GETDATE(),110)
   
END


GO


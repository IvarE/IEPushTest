
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


USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetContractors]    Script Date: 2015-04-24 13:42:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2015-01-14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetContractors]
	
AS
BEGIN
	SET NOCOUNT ON;

	select 
		c.Id,
		c.Gid,
		c.IsOrganisationId
	from Contractor c
	order by c.Id
	for xml path ('Contractor'), root('Contractors'), TYPE

END


GO


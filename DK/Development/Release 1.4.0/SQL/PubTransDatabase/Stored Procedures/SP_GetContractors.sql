


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
		c.[IdTemp],
		c.Gid,
		c.IsOrganisationId
	from Contractor c
	order by c.[IdTemp]
	for xml path ('Contractor'), root('Contractors'), TYPE

END


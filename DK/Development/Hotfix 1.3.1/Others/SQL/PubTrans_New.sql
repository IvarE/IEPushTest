USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetContractors]    Script Date: 2015-01-16 08:35:38 ******/
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

/****** Object:  StoredProcedure [dbo].[SP_GetOrganisationalUnits]    Script Date: 2015-01-16 08:35:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2015-01-14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetOrganisationalUnits]
	
AS
BEGIN
	SET NOCOUNT ON;

    select
		o.Id,
		o.Code,
		o.Name
	from OrganisationalUnit o
	order by o.Id
	for xml path ('OrganisationalUnit'), root('OrganisationalUnits'), TYPE

END

GO



USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetOrganisationalUnits]    Script Date: 2015-04-24 13:02:24 ******/
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


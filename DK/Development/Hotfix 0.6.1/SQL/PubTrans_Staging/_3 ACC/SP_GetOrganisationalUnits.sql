USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetOrganisationalUnits]    Script Date: 2015-04-24 12:50:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
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


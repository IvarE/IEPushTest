
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


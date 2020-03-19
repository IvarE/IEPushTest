-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-10
-- Description:	Get basecurrency for the organization
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBaseCurrency]

AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	org.basecurrencyid,
	org.basecurrencyidname 
	from DKCRM_MSCRM.dbo.FilteredOrganization org
	for xml path ('currency'), root ('currencies'), type

	REVERT

END

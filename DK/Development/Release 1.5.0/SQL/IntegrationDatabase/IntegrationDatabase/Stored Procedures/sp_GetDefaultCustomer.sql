-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-20
-- Description:	Get default customer from setting
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDefaultCustomer]
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	declare @now datetime
	select @now = DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)

	select setting.cgi_defaultcustomeroncase as defaultcustomeroncase
	from [$(Skanetrafiken_MSCRM)].dbo.Filteredcgi_setting setting
	where setting.cgi_validfrom <= @now
	and (setting.cgi_validto is null or setting.cgi_validto >= @now)
	and setting.statecode = 0
	for xml path ('setting'), root ('settings'), type

	REVERT

END

﻿-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-05
-- Description:	Get transaltion for zonenames
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetZoneNames]

AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	zonename.cgi_zoneid,
	zonename.cgi_name
	from DKCRM_MSCRM.dbo.Filteredcgi_zonename zonename
	for xml path ('zonename'), root ('zonenames'), type

	REVERT

END

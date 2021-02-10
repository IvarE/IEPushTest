USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetZoneNames]    Script Date: 2015-04-24 10:49:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
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
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_zonename zonename
	for xml path ('zonename'), root ('zonenames'), type

	REVERT

END

GO


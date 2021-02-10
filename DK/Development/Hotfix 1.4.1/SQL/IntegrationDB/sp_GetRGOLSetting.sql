USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetRGOLSetting]    Script Date: 2015-04-24 10:48:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-11-10
-- Description:	Get RGOL setting
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRGOLSetting] 
	@rgolsetting	int
AS
BEGIN
	SET NOCOUNT ON;
    
	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	rgol.cgi_rgolsettingid,
	rgol.cgi_rgolsettingno,
	rgol.cgi_name,
	rgol.cgi_refundtypeid,
	rgol.cgi_refundtypeidname,
	rgol.cgi_reimbursementformid,
	rgol.cgi_reimbursementformidname
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_rgolsetting rgol
	where rgol.cgi_rgolsettingno = @rgolsetting
	for xml path ('rgolsetting'), root ('rgolsettings'), type

	REVERT

END

GO


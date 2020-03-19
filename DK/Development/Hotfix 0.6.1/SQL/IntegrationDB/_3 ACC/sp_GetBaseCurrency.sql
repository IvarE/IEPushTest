USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetBaseCurrency]    Script Date: 2015-04-24 12:43:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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
	from DKCRMUAT_MSCRM.dbo.FilteredOrganization org
	for xml path ('currency'), root ('currencies'), type

	REVERT

END


GO


USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetBaseCurrency]    Script Date: 2015-04-24 10:47:31 ******/
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
	from Skanetrafiken_MSCRM.dbo.FilteredOrganization org
	for xml path ('currency'), root ('currencies'), type

	REVERT

END

GO


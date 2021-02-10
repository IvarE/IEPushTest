USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetIncidentFromIncidentId]    Script Date: 2015-04-24 10:48:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Danny Janlöv
-- Create date: 2015-10-26
-- Description:	Lists incidents on remittance status with responses
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetIncidentsWithRemittanceResponse] 
AS
BEGIN
	SET NOCOUNT ON;

	-- Bortkommenterad kod avser spårning av remisspåminnelser

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	SELECT
		i.ticketnumber,
		i.Title,
		i.createdon,
		i.createdbyname,
		i.modifiedon,
		i.modifiedbyname,
		i.cgi_case_remittancename,
		i.cgi_case_remittance,
		i.cgi_track_token,
		i.statuscodename,
		cgi_track_token_responsedt =
		(
			SELECT 
				MAX(e.createdon)
				FROM DKCRM_MSCRM.dbo.FilteredEmail e  WITH (READUNCOMMITTED)
				WHERE
					e.trackingtoken = i.cgi_track_token AND
					e.regardingobjectid = i.IncidentId AND
					e.directioncode = 0 AND
					e.CreatedOn >= i.CreatedOn
		)--,
		--i.cgi_track_token_remiss_reminder,
		--cgi_track_token_remiss_reminder_responsedt =
		--(
		--	SELECT 
		--		MAX(e.createdon)
		--		FROM FilteredEmail e  WITH (READUNCOMMITTED)
		--		WHERE
		--			e.trackingtoken = i.cgi_track_token AND
		--			e.regardingobjectid = i.IncidentId AND
		--			e.directioncode = 0 AND
		--			e.CreatedOn >= i.CreatedOn
		--)
		FROM DKCRM_MSCRM.dbo.FilteredIncident i WITH (READUNCOMMITTED)
		WHERE 
			i.cgi_case_remittance = 285050001 AND
			i.statuscode = 1 AND
			EXISTS (
				SELECT 
					1
					FROM DKCRM_MSCRM.dbo.FilteredEmail e  WITH (READUNCOMMITTED)
					WHERE
						e.trackingtoken = i.cgi_track_token AND
						e.regardingobjectid = i.IncidentId AND
						e.directioncode = 0 AND
						e.CreatedOn >= i.CreatedOn
				--UNION ALL
				--SELECT
				--	1
				--	FROM DKCRM_MSCRM.dbo.FilteredEmail e  WITH (READUNCOMMITTED)
				--	WHERE
				--		e.trackingtoken = i.cgi_track_token_remiss_reminder AND
				--		e.regardingobjectid = i.IncidentId AND
				--		e.directioncode = 0 AND
				--		e.CreatedOn >= i.CreatedOn			
			)

	REVERT

END
GO

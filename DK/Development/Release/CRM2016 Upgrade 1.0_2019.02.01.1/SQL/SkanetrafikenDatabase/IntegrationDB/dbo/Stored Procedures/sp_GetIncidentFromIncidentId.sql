
-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	get incident from incidentid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetIncidentFromIncidentId] 
	@incidentid		uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	incident.ticketnumber as ticketnumber,
	incident.title as title,
	incident.cgi_travelcardno,
	incident.cgi_accountid,
	incident.cgi_accountidname,
	incident.cgi_contactid,
	incident.cgi_contactidname,
	incident.incidentid,
	incident.cgi_telephonenumber,
	incident.cgi_rgol_delivery_email,
	incident.cgi_rgol_telephonenumber
	from DKCRM_MSCRM.dbo.FilteredIncident incident
	where incident.incidentid = @incidentid
	for xml path ('incident'), root ('incidents'), type
	
	REVERT

END
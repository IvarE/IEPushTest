

--
-- report view for leadcompetitors
--
create view dbo.[FilteredLeadCompetitors] (
    [competitorid],
    [leadcompetitorid],
    [leadid],
    [versionnumber]
) with view_metadata as
select
    [LeadCompetitors].[CompetitorId],
    [LeadCompetitors].[LeadCompetitorId],
    [LeadCompetitors].[LeadId],
    [LeadCompetitors].[VersionNumber]
from LeadCompetitors

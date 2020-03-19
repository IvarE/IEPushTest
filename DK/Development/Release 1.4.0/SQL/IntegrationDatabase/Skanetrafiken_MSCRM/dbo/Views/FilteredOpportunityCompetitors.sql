

--
-- report view for opportunitycompetitors
--
create view dbo.[FilteredOpportunityCompetitors] (
    [competitorid],
    [opportunitycompetitorid],
    [opportunityid],
    [versionnumber]
) with view_metadata as
select
    [OpportunityCompetitors].[CompetitorId],
    [OpportunityCompetitors].[OpportunityCompetitorId],
    [OpportunityCompetitors].[OpportunityId],
    [OpportunityCompetitors].[VersionNumber]
from OpportunityCompetitors

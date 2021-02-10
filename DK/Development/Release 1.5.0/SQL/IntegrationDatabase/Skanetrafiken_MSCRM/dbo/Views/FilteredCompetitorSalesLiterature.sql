

--
-- report view for competitorsalesliterature
--
create view dbo.[FilteredCompetitorSalesLiterature] (
    [competitorid],
    [competitorsalesliteratureid],
    [salesliteratureid],
    [versionnumber]
) with view_metadata as
select
    [CompetitorSalesLiterature].[CompetitorId],
    [CompetitorSalesLiterature].[CompetitorSalesLiteratureId],
    [CompetitorSalesLiterature].[SalesLiteratureId],
    [CompetitorSalesLiterature].[VersionNumber]
from CompetitorSalesLiterature

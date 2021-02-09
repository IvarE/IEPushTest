

--
-- report view for competitorproduct
--
create view dbo.[FilteredCompetitorProduct] (
    [competitorid],
    [competitorproductid],
    [productid],
    [versionnumber]
) with view_metadata as
select
    [CompetitorProduct].[CompetitorId],
    [CompetitorProduct].[CompetitorProductId],
    [CompetitorProduct].[ProductId],
    [CompetitorProduct].[VersionNumber]
from CompetitorProduct

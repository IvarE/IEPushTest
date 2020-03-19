

--
-- report view for entitlementproducts
--
create view dbo.[FilteredEntitlementProducts] (
    [entitlementid],
    [entitlementproductid],
    [productid],
    [versionnumber]
) with view_metadata as
select
    [EntitlementProducts].[EntitlementId],
    [EntitlementProducts].[EntitlementProductId],
    [EntitlementProducts].[ProductId],
    [EntitlementProducts].[VersionNumber]
from EntitlementProducts

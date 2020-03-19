

--
-- report view for entitlementtemplateproducts
--
create view dbo.[FilteredEntitlementTemplateProducts] (
    [entitlementtemplateid],
    [entitlementtemplateproductid],
    [productid],
    [versionnumber]
) with view_metadata as
select
    [EntitlementTemplateProducts].[EntitlementTemplateId],
    [EntitlementTemplateProducts].[EntitlementTemplateProductId],
    [EntitlementTemplateProducts].[ProductId],
    [EntitlementTemplateProducts].[VersionNumber]
from EntitlementTemplateProducts

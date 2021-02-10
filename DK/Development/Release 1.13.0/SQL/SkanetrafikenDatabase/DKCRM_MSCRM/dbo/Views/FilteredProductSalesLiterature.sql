

--
-- report view for productsalesliterature
--
create view dbo.[FilteredProductSalesLiterature] (
    [productid],
    [productsalesliteratureid],
    [salesliteratureid],
    [versionnumber]
) with view_metadata as
select
    [ProductSalesLiterature].[ProductId],
    [ProductSalesLiterature].[ProductSalesLiteratureId],
    [ProductSalesLiterature].[SalesLiteratureId],
    [ProductSalesLiterature].[VersionNumber]
from ProductSalesLiterature



--
-- report view for productsubstitute
--
create view dbo.[FilteredProductSubstitute] (
    [productid],
    [productsubstituteid],
    [substitutedproductid],
    [versionnumber]
) with view_metadata as
select
    [ProductSubstitute].[ProductId],
    [ProductSubstitute].[ProductSubstituteId],
    [ProductSubstitute].[SubstitutedProductId],
    [ProductSubstitute].[VersionNumber]
from ProductSubstitute

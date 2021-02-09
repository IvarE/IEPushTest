

--
-- report view for productassociation
--
create view dbo.[FilteredProductAssociation] (
    [associatedproduct],
    [productassociationid],
    [productid],
    [versionnumber]
) with view_metadata as
select
    [ProductAssociation].[AssociatedProduct],
    [ProductAssociation].[ProductAssociationId],
    [ProductAssociation].[ProductId],
    [ProductAssociation].[VersionNumber]
from ProductAssociation

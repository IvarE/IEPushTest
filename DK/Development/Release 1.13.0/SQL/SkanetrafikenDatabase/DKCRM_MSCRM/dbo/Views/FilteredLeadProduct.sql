

--
-- report view for leadproduct
--
create view dbo.[FilteredLeadProduct] (
    [leadid],
    [leadproductid],
    [productid],
    [versionnumber]
) with view_metadata as
select
    [LeadProduct].[LeadId],
    [LeadProduct].[LeadProductId],
    [LeadProduct].[ProductId],
    [LeadProduct].[VersionNumber]
from LeadProduct

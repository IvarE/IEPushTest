

--
-- report view for contactorders
--
create view dbo.[FilteredContactOrders] (
    [contactid],
    [contactorderid],
    [salesorderid],
    [versionnumber]
) with view_metadata as
select
    [ContactOrders].[ContactId],
    [ContactOrders].[ContactOrderId],
    [ContactOrders].[SalesOrderId],
    [ContactOrders].[VersionNumber]
from ContactOrders

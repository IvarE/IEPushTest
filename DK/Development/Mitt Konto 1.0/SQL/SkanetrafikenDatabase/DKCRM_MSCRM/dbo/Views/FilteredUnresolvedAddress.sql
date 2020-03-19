

--
-- report view for unresolvedaddress
--
create view dbo.[FilteredUnresolvedAddress] (
    [emailaddress],
    [fullname],
    [telephone],
    [unresolvedaddressid],
    [versionnumber]
) with view_metadata as
select
    [UnresolvedAddress].[EMailAddress],
    [UnresolvedAddress].[FullName],
    [UnresolvedAddress].[Telephone],
    [UnresolvedAddress].[UnresolvedAddressId],
    [UnresolvedAddress].[VersionNumber]
from UnresolvedAddress

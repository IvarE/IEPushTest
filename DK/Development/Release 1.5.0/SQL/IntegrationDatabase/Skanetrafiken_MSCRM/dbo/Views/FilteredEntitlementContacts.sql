

--
-- report view for entitlementcontacts
--
create view dbo.[FilteredEntitlementContacts] (
    [contactid],
    [entitlementcontactid],
    [entitlementid],
    [versionnumber]
) with view_metadata as
select
    [EntitlementContacts].[ContactId],
    [EntitlementContacts].[EntitlementContactId],
    [EntitlementContacts].[EntitlementId],
    [EntitlementContacts].[VersionNumber]
from EntitlementContacts

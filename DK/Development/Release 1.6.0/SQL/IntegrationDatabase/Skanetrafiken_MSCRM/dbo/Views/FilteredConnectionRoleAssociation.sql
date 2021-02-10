

--
-- report view for connectionroleassociation
--
create view dbo.[FilteredConnectionRoleAssociation] (
    [associatedconnectionroleid],
    [connectionroleassociationid],
    [connectionroleid],
    [versionnumber]
) with view_metadata as
select
    [ConnectionRoleAssociation].[AssociatedConnectionRoleId],
    [ConnectionRoleAssociation].[ConnectionRoleAssociationId],
    [ConnectionRoleAssociation].[ConnectionRoleId],
    [ConnectionRoleAssociation].[VersionNumber]
from ConnectionRoleAssociation

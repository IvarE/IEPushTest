

--
-- report view for systemuserroles
--
create view dbo.[FilteredSystemUserRoles] (
    [roleid],
    [systemuserid],
    [systemuserroleid],
    [versionnumber]
) with view_metadata as
select
    [SystemUserRoles].[RoleId],
    [SystemUserRoles].[SystemUserId],
    [SystemUserRoles].[SystemUserRoleId],
    [SystemUserRoles].[VersionNumber]
from SystemUserRoles

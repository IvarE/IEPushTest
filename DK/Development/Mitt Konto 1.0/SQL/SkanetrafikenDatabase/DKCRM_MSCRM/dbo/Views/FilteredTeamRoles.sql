

--
-- report view for teamroles
--
create view dbo.[FilteredTeamRoles] (
    [roleid],
    [teamid],
    [teamroleid],
    [versionnumber]
) with view_metadata as
select
    [TeamRoles].[RoleId],
    [TeamRoles].[TeamId],
    [TeamRoles].[TeamRoleId],
    [TeamRoles].[VersionNumber]
from TeamRoles

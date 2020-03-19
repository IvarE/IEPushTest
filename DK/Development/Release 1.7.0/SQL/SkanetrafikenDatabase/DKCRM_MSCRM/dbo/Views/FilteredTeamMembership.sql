

--
-- report view for teammembership
--
create view dbo.[FilteredTeamMembership] (
    [systemuserid],
    [teamid],
    [teammembershipid],
    [versionnumber]
) with view_metadata as
select
    [TeamMembership].[SystemUserId],
    [TeamMembership].[TeamId],
    [TeamMembership].[TeamMembershipId],
    [TeamMembership].[VersionNumber]
from TeamMembership

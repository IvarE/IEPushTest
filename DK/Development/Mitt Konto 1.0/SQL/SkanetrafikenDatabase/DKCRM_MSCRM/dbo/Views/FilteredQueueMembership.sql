

--
-- report view for queuemembership
--
create view dbo.[FilteredQueueMembership] (
    [queueid],
    [queuemembershipid],
    [systemuserid],
    [versionnumber]
) with view_metadata as
select
    [QueueMembership].[QueueId],
    [QueueMembership].[QueueMembershipId],
    [QueueMembership].[SystemUserId],
    [QueueMembership].[VersionNumber]
from QueueMembership

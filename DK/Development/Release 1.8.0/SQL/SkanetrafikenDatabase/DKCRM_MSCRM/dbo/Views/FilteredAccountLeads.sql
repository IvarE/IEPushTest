

--
-- report view for accountleads
--
create view dbo.[FilteredAccountLeads] (
    [accountid],
    [accountleadid],
    [leadid],
    [versionnumber]
) with view_metadata as
select
    [AccountLeads].[AccountId],
    [AccountLeads].[AccountLeadId],
    [AccountLeads].[LeadId],
    [AccountLeads].[VersionNumber]
from AccountLeads

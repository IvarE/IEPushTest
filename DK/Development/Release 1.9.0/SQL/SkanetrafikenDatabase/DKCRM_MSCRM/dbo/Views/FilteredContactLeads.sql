

--
-- report view for contactleads
--
create view dbo.[FilteredContactLeads] (
    [contactid],
    [contactleadid],
    [leadid],
    [versionnumber]
) with view_metadata as
select
    [ContactLeads].[ContactId],
    [ContactLeads].[ContactLeadId],
    [ContactLeads].[LeadId],
    [ContactLeads].[VersionNumber]
from ContactLeads

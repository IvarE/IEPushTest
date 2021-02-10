

--
-- report view for cgi_account_contact
--
create view dbo.[Filteredcgi_account_contact] (
    [accountid],
    [cgi_account_contactid],
    [contactid],
    [versionnumber]
) with view_metadata as
select
    [cgi_account_contact].[accountid],
    [cgi_account_contact].[cgi_account_contactId],
    [cgi_account_contact].[contactid],
    [cgi_account_contact].[VersionNumber]
from cgi_account_contact

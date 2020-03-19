


--
-- base view for cgi_account_contact
--
create view dbo.[cgi_account_contact]
 (

    -- physical attributes
    [cgi_account_contactId],
    [VersionNumber],
    [accountid],
    [contactid]
) with view_metadata as
select

    -- physical attribute
    [cgi_account_contactBase].[cgi_account_contactId],
    [cgi_account_contactBase].[VersionNumber],
    [cgi_account_contactBase].[accountid],
    [cgi_account_contactBase].[contactid]
from [cgi_account_contactBase] 

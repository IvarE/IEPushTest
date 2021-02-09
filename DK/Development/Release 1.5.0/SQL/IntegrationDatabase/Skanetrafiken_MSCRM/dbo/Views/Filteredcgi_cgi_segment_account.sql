

--
-- report view for cgi_cgi_segment_account
--
create view dbo.[Filteredcgi_cgi_segment_account] (
    [accountid],
    [cgi_cgi_segment_accountid],
    [cgi_segmentid],
    [versionnumber]
) with view_metadata as
select
    [cgi_cgi_segment_account].[accountid],
    [cgi_cgi_segment_account].[cgi_cgi_segment_accountId],
    [cgi_cgi_segment_account].[cgi_segmentid],
    [cgi_cgi_segment_account].[VersionNumber]
from cgi_cgi_segment_account

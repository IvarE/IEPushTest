


--
-- base view for cgi_cgi_segment_account
--
create view dbo.[cgi_cgi_segment_account]
 (

    -- physical attributes
    [cgi_cgi_segment_accountId],
    [VersionNumber],
    [cgi_segmentid],
    [accountid]
) with view_metadata as
select

    -- physical attribute
    [cgi_cgi_segment_accountBase].[cgi_cgi_segment_accountId],
    [cgi_cgi_segment_accountBase].[VersionNumber],
    [cgi_cgi_segment_accountBase].[cgi_segmentid],
    [cgi_cgi_segment_accountBase].[accountid]
from [cgi_cgi_segment_accountBase] 

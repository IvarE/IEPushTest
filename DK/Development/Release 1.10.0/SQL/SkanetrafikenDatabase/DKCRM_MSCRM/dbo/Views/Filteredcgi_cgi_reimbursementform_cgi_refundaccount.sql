

--
-- report view for cgi_cgi_reimbursementform_cgi_refundaccount
--
create view dbo.[Filteredcgi_cgi_reimbursementform_cgi_refundaccount] (
    [cgi_cgi_reimbursementform_cgi_refundaccountid],
    [cgi_refundaccountid],
    [cgi_reimbursementformid],
    [versionnumber]
) with view_metadata as
select
    [cgi_cgi_reimbursementform_cgi_refundaccount].[cgi_cgi_reimbursementform_cgi_refundaccountId],
    [cgi_cgi_reimbursementform_cgi_refundaccount].[cgi_refundaccountid],
    [cgi_cgi_reimbursementform_cgi_refundaccount].[cgi_reimbursementformid],
    [cgi_cgi_reimbursementform_cgi_refundaccount].[VersionNumber]
from cgi_cgi_reimbursementform_cgi_refundaccount

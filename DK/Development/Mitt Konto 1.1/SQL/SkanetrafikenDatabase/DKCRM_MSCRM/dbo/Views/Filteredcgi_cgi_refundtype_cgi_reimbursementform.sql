

--
-- report view for cgi_cgi_refundtype_cgi_reimbursementform
--
create view dbo.[Filteredcgi_cgi_refundtype_cgi_reimbursementform] (
    [cgi_cgi_refundtype_cgi_reimbursementformid],
    [cgi_refundtypeid],
    [cgi_reimbursementformid],
    [versionnumber]
) with view_metadata as
select
    [cgi_cgi_refundtype_cgi_reimbursementform].[cgi_cgi_refundtype_cgi_reimbursementformId],
    [cgi_cgi_refundtype_cgi_reimbursementform].[cgi_refundtypeid],
    [cgi_cgi_refundtype_cgi_reimbursementform].[cgi_reimbursementformid],
    [cgi_cgi_refundtype_cgi_reimbursementform].[VersionNumber]
from cgi_cgi_refundtype_cgi_reimbursementform

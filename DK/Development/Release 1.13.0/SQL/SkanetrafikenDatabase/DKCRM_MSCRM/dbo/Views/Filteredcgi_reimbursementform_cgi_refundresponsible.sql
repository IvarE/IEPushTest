

--
-- report view for cgi_reimbursementform_cgi_refundresponsible
--
create view dbo.[Filteredcgi_reimbursementform_cgi_refundresponsible] (
    [cgi_refundresponsibleid],
    [cgi_reimbursementformid],
    [cgi_reimbursementform_cgi_refundresponsibleid],
    [versionnumber]
) with view_metadata as
select
    [cgi_reimbursementform_cgi_refundresponsible].[cgi_refundresponsibleid],
    [cgi_reimbursementform_cgi_refundresponsible].[cgi_reimbursementformid],
    [cgi_reimbursementform_cgi_refundresponsible].[cgi_reimbursementform_cgi_refundresponsibleId],
    [cgi_reimbursementform_cgi_refundresponsible].[VersionNumber]
from cgi_reimbursementform_cgi_refundresponsible

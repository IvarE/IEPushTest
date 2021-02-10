

--
-- report view for cgi_cgi_reimbursementform_cgi_refundproduct
--
create view dbo.[Filteredcgi_cgi_reimbursementform_cgi_refundproduct] (
    [cgi_cgi_reimbursementform_cgi_refundproductid],
    [cgi_refundproductid],
    [cgi_reimbursementformid],
    [versionnumber]
) with view_metadata as
select
    [cgi_cgi_reimbursementform_cgi_refundproduct].[cgi_cgi_reimbursementform_cgi_refundproductId],
    [cgi_cgi_reimbursementform_cgi_refundproduct].[cgi_refundproductid],
    [cgi_cgi_reimbursementform_cgi_refundproduct].[cgi_reimbursementformid],
    [cgi_cgi_reimbursementform_cgi_refundproduct].[VersionNumber]
from cgi_cgi_reimbursementform_cgi_refundproduct

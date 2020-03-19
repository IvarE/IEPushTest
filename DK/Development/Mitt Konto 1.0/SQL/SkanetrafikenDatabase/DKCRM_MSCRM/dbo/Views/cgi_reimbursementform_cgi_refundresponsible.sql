


--
-- base view for cgi_reimbursementform_cgi_refundresponsible
--
create view dbo.[cgi_reimbursementform_cgi_refundresponsible]
 (

    -- physical attributes
    [cgi_reimbursementform_cgi_refundresponsibleId],
    [VersionNumber],
    [cgi_reimbursementformid],
    [cgi_refundresponsibleid]
) with view_metadata as
select

    -- physical attribute
    [cgi_reimbursementform_cgi_refundresponsibleBase].[cgi_reimbursementform_cgi_refundresponsibleId],
    [cgi_reimbursementform_cgi_refundresponsibleBase].[VersionNumber],
    [cgi_reimbursementform_cgi_refundresponsibleBase].[cgi_reimbursementformid],
    [cgi_reimbursementform_cgi_refundresponsibleBase].[cgi_refundresponsibleid]
from [cgi_reimbursementform_cgi_refundresponsibleBase] 

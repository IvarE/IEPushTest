


--
-- base view for cgi_cgi_refundtype_cgi_reimbursementform
--
create view dbo.[cgi_cgi_refundtype_cgi_reimbursementform]
 (

    -- physical attributes
    [cgi_cgi_refundtype_cgi_reimbursementformId],
    [VersionNumber],
    [cgi_refundtypeid],
    [cgi_reimbursementformid]
) with view_metadata as
select

    -- physical attribute
    [cgi_cgi_refundtype_cgi_reimbursementformBase].[cgi_cgi_refundtype_cgi_reimbursementformId],
    [cgi_cgi_refundtype_cgi_reimbursementformBase].[VersionNumber],
    [cgi_cgi_refundtype_cgi_reimbursementformBase].[cgi_refundtypeid],
    [cgi_cgi_refundtype_cgi_reimbursementformBase].[cgi_reimbursementformid]
from [cgi_cgi_refundtype_cgi_reimbursementformBase] 

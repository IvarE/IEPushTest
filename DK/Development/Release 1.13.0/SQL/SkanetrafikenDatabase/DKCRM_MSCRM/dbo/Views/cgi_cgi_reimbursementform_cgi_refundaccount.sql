


--
-- base view for cgi_cgi_reimbursementform_cgi_refundaccount
--
create view dbo.[cgi_cgi_reimbursementform_cgi_refundaccount]
 (

    -- physical attributes
    [cgi_cgi_reimbursementform_cgi_refundaccountId],
    [VersionNumber],
    [cgi_reimbursementformid],
    [cgi_refundaccountid]
) with view_metadata as
select

    -- physical attribute
    [cgi_cgi_reimbursementform_cgi_refundaccountBase].[cgi_cgi_reimbursementform_cgi_refundaccountId],
    [cgi_cgi_reimbursementform_cgi_refundaccountBase].[VersionNumber],
    [cgi_cgi_reimbursementform_cgi_refundaccountBase].[cgi_reimbursementformid],
    [cgi_cgi_reimbursementform_cgi_refundaccountBase].[cgi_refundaccountid]
from [cgi_cgi_reimbursementform_cgi_refundaccountBase] 

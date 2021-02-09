


--
-- base view for cgi_cgi_reimbursementform_cgi_refundproduct
--
create view dbo.[cgi_cgi_reimbursementform_cgi_refundproduct]
 (

    -- physical attributes
    [cgi_cgi_reimbursementform_cgi_refundproductId],
    [VersionNumber],
    [cgi_reimbursementformid],
    [cgi_refundproductid]
) with view_metadata as
select

    -- physical attribute
    [cgi_cgi_reimbursementform_cgi_refundproductBase].[cgi_cgi_reimbursementform_cgi_refundproductId],
    [cgi_cgi_reimbursementform_cgi_refundproductBase].[VersionNumber],
    [cgi_cgi_reimbursementform_cgi_refundproductBase].[cgi_reimbursementformid],
    [cgi_cgi_reimbursementform_cgi_refundproductBase].[cgi_refundproductid]
from [cgi_cgi_reimbursementform_cgi_refundproductBase] 

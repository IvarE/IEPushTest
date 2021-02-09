UPDATE refund2016Prod
SET    refund2016Prod.ed_voucher_printed = SYSDATETIME()
FROM   [V-DK-SQL01].DKCRM_MSCRM.dbo.cgi_refund refund2016Prod
       JOIN [CLCMSDB-SQL1].IntegrationDB.dbo.refund_printed refund2013Prod
         ON refund2016Prod.cgi_refundId = refund2013Prod.refundid


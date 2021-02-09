CREATE TABLE [dbo].[cgi_cgi_reimbursementform_cgi_refundaccountBase] (
    [cgi_cgi_reimbursementform_cgi_refundaccountId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]                                 ROWVERSION       NULL,
    [cgi_reimbursementformid]                       UNIQUEIDENTIFIER NOT NULL,
    [cgi_refundaccountid]                           UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_cgi_reimbursementform_cgi_refundaccountBase] PRIMARY KEY CLUSTERED ([cgi_cgi_reimbursementform_cgi_refundaccountId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_reimbursementform_cgi_refundaccountOne] FOREIGN KEY ([cgi_reimbursementformid]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_reimbursementform_cgi_refundaccountTwo] FOREIGN KEY ([cgi_refundaccountid]) REFERENCES [dbo].[cgi_refundaccountBase] ([cgi_refundaccountId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundaccountBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_cgi_reimbursementform_cgi_refundaccount]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundaccountBase]([cgi_reimbursementformid] ASC, [cgi_refundaccountid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_refundaccountid]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundaccountBase]([cgi_refundaccountid] ASC) WITH (FILLFACTOR = 80);


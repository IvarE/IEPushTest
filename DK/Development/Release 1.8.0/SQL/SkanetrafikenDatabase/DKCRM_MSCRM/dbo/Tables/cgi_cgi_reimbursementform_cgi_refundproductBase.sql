CREATE TABLE [dbo].[cgi_cgi_reimbursementform_cgi_refundproductBase] (
    [cgi_cgi_reimbursementform_cgi_refundproductId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]                                 ROWVERSION       NULL,
    [cgi_reimbursementformid]                       UNIQUEIDENTIFIER NOT NULL,
    [cgi_refundproductid]                           UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_cgi_reimbursementform_cgi_refundproductBase] PRIMARY KEY CLUSTERED ([cgi_cgi_reimbursementform_cgi_refundproductId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_reimbursementform_cgi_refundproductOne] FOREIGN KEY ([cgi_reimbursementformid]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_reimbursementform_cgi_refundproductTwo] FOREIGN KEY ([cgi_refundproductid]) REFERENCES [dbo].[cgi_refundproductBase] ([cgi_refundproductId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundproductBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_cgi_reimbursementform_cgi_refundproduct]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundproductBase]([cgi_reimbursementformid] ASC, [cgi_refundproductid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_refundproductid]
    ON [dbo].[cgi_cgi_reimbursementform_cgi_refundproductBase]([cgi_refundproductid] ASC) WITH (FILLFACTOR = 80);


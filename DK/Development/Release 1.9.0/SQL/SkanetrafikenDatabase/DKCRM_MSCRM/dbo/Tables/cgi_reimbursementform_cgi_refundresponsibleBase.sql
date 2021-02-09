CREATE TABLE [dbo].[cgi_reimbursementform_cgi_refundresponsibleBase] (
    [cgi_reimbursementform_cgi_refundresponsibleId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]                                 ROWVERSION       NULL,
    [cgi_reimbursementformid]                       UNIQUEIDENTIFIER NOT NULL,
    [cgi_refundresponsibleid]                       UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_reimbursementform_cgi_refundresponsibleBase] PRIMARY KEY CLUSTERED ([cgi_reimbursementform_cgi_refundresponsibleId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_reimbursementform_cgi_refundresponsibleOne] FOREIGN KEY ([cgi_reimbursementformid]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_reimbursementform_cgi_refundresponsibleTwo] FOREIGN KEY ([cgi_refundresponsibleid]) REFERENCES [dbo].[cgi_refundresponsibleBase] ([cgi_refundresponsibleId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_reimbursementform_cgi_refundresponsibleBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_reimbursementform_cgi_refundresponsible]
    ON [dbo].[cgi_reimbursementform_cgi_refundresponsibleBase]([cgi_reimbursementformid] ASC, [cgi_refundresponsibleid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_refundresponsibleid]
    ON [dbo].[cgi_reimbursementform_cgi_refundresponsibleBase]([cgi_refundresponsibleid] ASC) WITH (FILLFACTOR = 80);


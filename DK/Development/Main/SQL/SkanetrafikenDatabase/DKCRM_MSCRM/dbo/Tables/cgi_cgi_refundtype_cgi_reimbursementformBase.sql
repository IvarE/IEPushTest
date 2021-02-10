CREATE TABLE [dbo].[cgi_cgi_refundtype_cgi_reimbursementformBase] (
    [cgi_cgi_refundtype_cgi_reimbursementformId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]                              ROWVERSION       NULL,
    [cgi_refundtypeid]                           UNIQUEIDENTIFIER NOT NULL,
    [cgi_reimbursementformid]                    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_cgi_refundtype_cgi_reimbursementformBase] PRIMARY KEY CLUSTERED ([cgi_cgi_refundtype_cgi_reimbursementformId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_refundtype_cgi_reimbursementformOne] FOREIGN KEY ([cgi_refundtypeid]) REFERENCES [dbo].[cgi_refundtypeBase] ([cgi_refundtypeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_refundtype_cgi_reimbursementformTwo] FOREIGN KEY ([cgi_reimbursementformid]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_cgi_refundtype_cgi_reimbursementformBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_cgi_refundtype_cgi_reimbursementform]
    ON [dbo].[cgi_cgi_refundtype_cgi_reimbursementformBase]([cgi_refundtypeid] ASC, [cgi_reimbursementformid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_cgi_reimbursementformid]
    ON [dbo].[cgi_cgi_refundtype_cgi_reimbursementformBase]([cgi_reimbursementformid] ASC) WITH (FILLFACTOR = 80);


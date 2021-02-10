CREATE TABLE [dbo].[cgi_cgi_segment_accountBase] (
    [cgi_cgi_segment_accountId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [cgi_segmentid]             UNIQUEIDENTIFIER NOT NULL,
    [accountid]                 UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_cgi_segment_accountBase] PRIMARY KEY CLUSTERED ([cgi_cgi_segment_accountId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_segment_accountOne] FOREIGN KEY ([cgi_segmentid]) REFERENCES [dbo].[cgi_segmentBase] ([cgi_segmentId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_segment_accountTwo] FOREIGN KEY ([accountid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_cgi_segment_accountBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_cgi_segment_account]
    ON [dbo].[cgi_cgi_segment_accountBase]([cgi_segmentid] ASC, [accountid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_accountid]
    ON [dbo].[cgi_cgi_segment_accountBase]([accountid] ASC) WITH (FILLFACTOR = 80);


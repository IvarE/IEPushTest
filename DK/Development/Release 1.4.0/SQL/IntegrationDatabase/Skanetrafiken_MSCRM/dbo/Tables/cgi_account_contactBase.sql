CREATE TABLE [dbo].[cgi_account_contactBase] (
    [cgi_account_contactId] UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]         ROWVERSION       NULL,
    [accountid]             UNIQUEIDENTIFIER NOT NULL,
    [contactid]             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_cgi_account_contactBase] PRIMARY KEY CLUSTERED ([cgi_account_contactId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_account_contactOne] FOREIGN KEY ([accountid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_account_contactTwo] FOREIGN KEY ([contactid]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_Sync]
    ON [dbo].[cgi_account_contactBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ndx_cgi_account_contact]
    ON [dbo].[cgi_account_contactBase]([accountid] ASC, [contactid] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_contactid]
    ON [dbo].[cgi_account_contactBase]([contactid] ASC) WITH (FILLFACTOR = 80);


CREATE TABLE [dbo].[PhoneCallBase] (
    [ActivityId]          UNIQUEIDENTIFIER NOT NULL,
    [cgi_CreateCase]      BIT              NULL,
    [cgi_CallguideInfoid] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_PhoneCallBase] PRIMARY KEY CLUSTERED ([ActivityId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_callguideinfo_phonecall_CallguideInfoid] FOREIGN KEY ([cgi_CallguideInfoid]) REFERENCES [dbo].[cgi_callguideinfoBase] ([cgi_callguideinfoId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_PhoneCallBase_ActivityPointerBase] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION
);


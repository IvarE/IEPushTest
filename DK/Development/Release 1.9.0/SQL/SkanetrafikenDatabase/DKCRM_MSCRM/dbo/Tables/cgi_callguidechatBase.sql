CREATE TABLE [dbo].[cgi_callguidechatBase] (
    [ActivityId]           UNIQUEIDENTIFIER NOT NULL,
    [cgi_ChatConversation] NVARCHAR (MAX)   NULL,
    [cgi_CreateCase]       BIT              NULL,
    [cgi_CallguideInfoid]  UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_cgi_callguidechatBase] PRIMARY KEY CLUSTERED ([ActivityId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_callguideinfo_cgi_callguidechat_CallguideInfoid] FOREIGN KEY ([cgi_CallguideInfoid]) REFERENCES [dbo].[cgi_callguideinfoBase] ([cgi_callguideinfoId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_cgi_callguidechatBase_ActivityPointerBase] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION
);


CREATE TABLE [dbo].[cgi_callguidefacebookBase] (
    [ActivityId]          UNIQUEIDENTIFIER NOT NULL,
    [cgi_CreateCase]      BIT              NULL,
    [cgi_FacebookUrl]     NVARCHAR (200)   NULL,
    [cgi_CallguideInfoid] UNIQUEIDENTIFIER NULL,
    [cgi_FaceBookPost]    NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_cgi_callguidefacebookBase] PRIMARY KEY CLUSTERED ([ActivityId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_callguideinfo_cgi_callguidefacebook_CallguideInfoid] FOREIGN KEY ([cgi_CallguideInfoid]) REFERENCES [dbo].[cgi_callguideinfoBase] ([cgi_callguideinfoId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_cgi_callguidefacebookBase_ActivityPointerBase] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION
);


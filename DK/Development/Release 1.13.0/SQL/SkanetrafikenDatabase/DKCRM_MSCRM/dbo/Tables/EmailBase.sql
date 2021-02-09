CREATE TABLE [dbo].[EmailBase] (
    [ActivityId]             UNIQUEIDENTIFIER NOT NULL,
    [cgi_Attention]          BIT              NULL,
    [cgi_Remittance]         BIT              NULL,
    [cgi_ask_customer]       BIT              NULL,
    [cgi_email_recipient_Id] UNIQUEIDENTIFIER NULL,
    [cgi_button_customer]    NVARCHAR (100)   NULL,
    [cgi_cc_emailrecipient]  UNIQUEIDENTIFIER NULL,
    [cgi_bcc_emailrecipient] UNIQUEIDENTIFIER NULL,
    [cgi_SourceType]         INT              NULL,
    [cgi_Type]               NVARCHAR (50)    NULL,
    [cgi_getfilelink]        BIT              NULL,
    CONSTRAINT [PK_EmailBase] PRIMARY KEY CLUSTERED ([ActivityId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [cgi_cgi_emailrecipient_email_bcc_emailrecipient] FOREIGN KEY ([cgi_bcc_emailrecipient]) REFERENCES [dbo].[cgi_emailrecipientBase] ([cgi_emailrecipientId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_emailrecipient_email_cc_emailrecipient] FOREIGN KEY ([cgi_cc_emailrecipient]) REFERENCES [dbo].[cgi_emailrecipientBase] ([cgi_emailrecipientId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_emailrecipient_email] FOREIGN KEY ([cgi_email_recipient_Id]) REFERENCES [dbo].[cgi_emailrecipientBase] ([cgi_emailrecipientId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EmailBase_ActivityPointerBase] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION
);


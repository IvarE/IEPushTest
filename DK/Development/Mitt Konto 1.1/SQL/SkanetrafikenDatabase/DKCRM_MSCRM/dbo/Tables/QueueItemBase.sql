CREATE TABLE [dbo].[QueueItemBase] (
    [QueueItemId]                         UNIQUEIDENTIFIER NOT NULL,
    [QueueId]                             UNIQUEIDENTIFIER NULL,
    [ObjectId]                            UNIQUEIDENTIFIER NULL,
    [ObjectTypeCode]                      INT              NULL,
    [Title]                               NVARCHAR (300)   NULL,
    [EnteredOn]                           DATETIME         NULL,
    [Priority]                            INT              NULL,
    [State]                               INT              NULL,
    [Status]                              INT              NULL,
    [CreatedOn]                           DATETIME         NULL,
    [CreatedBy]                           UNIQUEIDENTIFIER NULL,
    [ModifiedBy]                          UNIQUEIDENTIFIER NULL,
    [ModifiedOn]                          DATETIME         NULL,
    [ToRecipients]                        NVARCHAR (500)   NULL,
    [Sender]                              NVARCHAR (250)   NULL,
    [OrganizationId]                      UNIQUEIDENTIFIER NOT NULL,
    [VersionNumber]                       ROWVERSION       NULL,
    [TimeZoneRuleVersionNumber]           INT              NULL,
    [UTCConversionTimeZoneCode]           INT              NULL,
    [TransactionCurrencyId]               UNIQUEIDENTIFIER NULL,
    [ModifiedOnBehalfBy]                  UNIQUEIDENTIFIER NULL,
    [ImportSequenceNumber]                INT              NULL,
    [CreatedOnBehalfBy]                   UNIQUEIDENTIFIER NULL,
    [StateCode]                           INT              CONSTRAINT [DF_QueueItemBase_StateCode] DEFAULT ((0)) NOT NULL,
    [OverriddenCreatedOn]                 DATETIME         NULL,
    [ExchangeRate]                        DECIMAL (23, 10) NULL,
    [StatusCode]                          INT              CONSTRAINT [DF_QueueItemBase_StatusCode] DEFAULT ((1)) NOT NULL,
    [WorkerId]                            UNIQUEIDENTIFIER NULL,
    [WorkerIdModifiedOn]                  DATETIME         NULL,
    [WorkerIdName]                        NVARCHAR (4000)  NULL,
    [WorkerIdType]                        INT              NULL,
    [WorkerIdYomiName]                    NVARCHAR (4000)  NULL,
    [cgi_incidentstagecode]               NVARCHAR (100)   NULL,
    [cgi_priority]                        NVARCHAR (100)   NULL,
    [cgi_customer]                        NVARCHAR (100)   NULL,
    [cgi_case_type]                       NVARCHAR (100)   NULL,
    [cgi_case_remittance]                 NVARCHAR (100)   NULL,
    [cgi_action_date]                     NVARCHAR (100)   NULL,
    [cgi_resolve_by]                      NVARCHAR (100)   NULL,
    [cgi_customer_number]                 NVARCHAR (100)   NULL,
    [cgi_customer_email]                  NVARCHAR (100)   NULL,
    [cgi_soc_sec_number]                  NVARCHAR (100)   NULL,
    [cgi_customer_telephonenumber]        NVARCHAR (100)   NULL,
    [cgi_customer_telephonenumber_work]   NVARCHAR (100)   NULL,
    [cgi_customer_telephonenumber_mobile] NVARCHAR (100)   NULL,
    [cgi_arrival_date]                    DATETIME         NULL,
    [cgi_ticketnumber]                    NVARCHAR (100)   NULL,
    [cgi_caseorigincode]                  NVARCHAR (100)   NULL,
    [cgi_casdet_row1_cat3]                NVARCHAR (100)   NULL,
    [cgi_refund_type]                     NVARCHAR (100)   NULL,
    [cgi_refund_status]                   NVARCHAR (100)   NULL,
    [cgi_travelinformationline]           NVARCHAR (50)    NULL,
    [cgi_refund_approvaltype]             NVARCHAR (50)    NULL,
    [cgi_refund_typename]                 NVARCHAR (100)   NULL,
    [cgi_reimbursement_name]              NVARCHAR (100)   NULL,
    CONSTRAINT [cndx_PrimaryKey_QueueItem] PRIMARY KEY CLUSTERED ([QueueItemId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [queue_entries] FOREIGN KEY ([QueueId]) REFERENCES [dbo].[QueueBase] ([QueueId]) NOT FOR REPLICATION,
    CONSTRAINT [TransactionCurrency_QueueItem] FOREIGN KEY ([TransactionCurrencyId]) REFERENCES [dbo].[TransactionCurrencyBase] ([TransactionCurrencyId]) NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [ndx_StateCodeWorkerIdEnteredOn]
    ON [dbo].[QueueItemBase]([StateCode] ASC, [WorkerId] ASC, [EnteredOn] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [fndx_Sync_VersionNumber]
    ON [dbo].[QueueItemBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_queue_entries]
    ON [dbo].[QueueItemBase]([QueueId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_TitleQueueItemId]
    ON [dbo].[QueueItemBase]([Title] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_Sync_QueueItem_statecode]
    ON [dbo].[QueueItemBase]([StateCode] ASC, [QueueId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_for_cascaderelationship_Letter_QueueItem]
    ON [dbo].[QueueItemBase]([ObjectId] ASC, [ObjectTypeCode] ASC, [StateCode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_customer_number]
    ON [dbo].[QueueItemBase]([cgi_customer_number] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_customer_telephonenumber_mobile]
    ON [dbo].[QueueItemBase]([cgi_customer_telephonenumber_mobile] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_soc_sec_number]
    ON [dbo].[QueueItemBase]([cgi_soc_sec_number] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_customer_telephonenumber_work]
    ON [dbo].[QueueItemBase]([cgi_customer_telephonenumber_work] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_customer_telephonenumber]
    ON [dbo].[QueueItemBase]([cgi_customer_telephonenumber] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_customer_email]
    ON [dbo].[QueueItemBase]([cgi_customer_email] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_ticketnumber]
    ON [dbo].[QueueItemBase]([cgi_ticketnumber] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_case_type]
    ON [dbo].[QueueItemBase]([cgi_case_type] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_arrival_date]
    ON [dbo].[QueueItemBase]([cgi_arrival_date] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_casdet_row1_cat3]
    ON [dbo].[QueueItemBase]([cgi_casdet_row1_cat3] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_QF_cgi_caseorigincode]
    ON [dbo].[QueueItemBase]([cgi_caseorigincode] ASC) WITH (FILLFACTOR = 80);


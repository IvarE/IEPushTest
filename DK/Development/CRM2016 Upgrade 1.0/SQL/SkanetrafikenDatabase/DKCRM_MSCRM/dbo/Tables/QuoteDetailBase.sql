﻿CREATE TABLE [dbo].[QuoteDetailBase] (
    [QuoteDetailId]             UNIQUEIDENTIFIER NOT NULL,
    [QuoteId]                   UNIQUEIDENTIFIER NOT NULL,
    [SalesRepId]                UNIQUEIDENTIFIER NULL,
    [LineItemNumber]            INT              NULL,
    [UoMId]                     UNIQUEIDENTIFIER NULL,
    [ProductId]                 UNIQUEIDENTIFIER NULL,
    [RequestDeliveryBy]         DATETIME         NULL,
    [Quantity]                  DECIMAL (23, 10) NULL,
    [PricingErrorCode]          INT              NULL,
    [ManualDiscountAmount]      MONEY            NULL,
    [ProductDescription]        NVARCHAR (500)   NULL,
    [VolumeDiscountAmount]      MONEY            NULL,
    [PricePerUnit]              MONEY            NULL,
    [BaseAmount]                MONEY            NULL,
    [ExtendedAmount]            MONEY            NULL,
    [Description]               NVARCHAR (MAX)   NULL,
    [ShipTo_Name]               NVARCHAR (200)   NULL,
    [IsPriceOverridden]         BIT              NULL,
    [Tax]                       MONEY            NULL,
    [ShipTo_Line1]              NVARCHAR (4000)  NULL,
    [CreatedOn]                 DATETIME         NULL,
    [ShipTo_Line2]              NVARCHAR (4000)  NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [ShipTo_Line3]              NVARCHAR (4000)  NULL,
    [ShipTo_City]               NVARCHAR (80)    NULL,
    [ModifiedOn]                DATETIME         NULL,
    [ShipTo_StateOrProvince]    NVARCHAR (50)    NULL,
    [ShipTo_Country]            NVARCHAR (80)    NULL,
    [ShipTo_PostalCode]         NVARCHAR (20)    NULL,
    [WillCall]                  BIT              NULL,
    [IsProductOverridden]       BIT              CONSTRAINT [Set_To_Zero138] DEFAULT ((0)) NULL,
    [ShipTo_Telephone]          NVARCHAR (50)    NULL,
    [ShipTo_Fax]                NVARCHAR (50)    NULL,
    [ShipTo_FreightTermsCode]   INT              NULL,
    [ShipTo_AddressId]          UNIQUEIDENTIFIER NULL,
    [ShipTo_ContactName]        NVARCHAR (150)   NULL,
    [VersionNumber]             ROWVERSION       NULL,
    [ImportSequenceNumber]      INT              NULL,
    [UTCConversionTimeZoneCode] INT              NULL,
    [OverriddenCreatedOn]       DATETIME         NULL,
    [TransactionCurrencyId]     UNIQUEIDENTIFIER NULL,
    [ExchangeRate]              DECIMAL (23, 10) NULL,
    [TimeZoneRuleVersionNumber] INT              NULL,
    [Tax_Base]                  MONEY            NULL,
    [ExtendedAmount_Base]       MONEY            NULL,
    [PricePerUnit_Base]         MONEY            NULL,
    [BaseAmount_Base]           MONEY            NULL,
    [ManualDiscountAmount_Base] MONEY            NULL,
    [VolumeDiscountAmount_Base] MONEY            NULL,
    [ModifiedOnBehalfBy]        UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]         UNIQUEIDENTIFIER NULL,
    [SequenceNumber]            INT              NULL,
    CONSTRAINT [cndx_PrimaryKey_QuoteDetail] PRIMARY KEY NONCLUSTERED ([QuoteDetailId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [product_quote_details] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProductBase] ([ProductId]) NOT FOR REPLICATION,
    CONSTRAINT [quote_details] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[QuoteBase] ([QuoteId]) NOT FOR REPLICATION,
    CONSTRAINT [system_user_quotedetail] FOREIGN KEY ([SalesRepId]) REFERENCES [dbo].[SystemUserBase] ([SystemUserId]) NOT FOR REPLICATION,
    CONSTRAINT [transactioncurrency_quotedetail] FOREIGN KEY ([TransactionCurrencyId]) REFERENCES [dbo].[TransactionCurrencyBase] ([TransactionCurrencyId]) NOT FOR REPLICATION,
    CONSTRAINT [unit_of_measurement_quote_details] FOREIGN KEY ([UoMId]) REFERENCES [dbo].[UoMBase] ([UoMId]) NOT FOR REPLICATION
);


GO
EXECUTE sp_tableoption @TableNamePattern = N'[dbo].[QuoteDetailBase]', @OptionName = N'text in row', @OptionValue = N'7000';


GO
CREATE CLUSTERED INDEX [cndx_for_cascaderelationship_quote_details]
    ON [dbo].[QuoteDetailBase]([QuoteId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [fndx_Sync_VersionNumber]
    ON [dbo].[QuoteDetailBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_product_quote_details]
    ON [dbo].[QuoteDetailBase]([ProductId] ASC) WHERE ([ProductId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_system_user_quotedetail]
    ON [dbo].[QuoteDetailBase]([SalesRepId] ASC) WHERE ([SalesRepId] IS NOT NULL) WITH (FILLFACTOR = 80);


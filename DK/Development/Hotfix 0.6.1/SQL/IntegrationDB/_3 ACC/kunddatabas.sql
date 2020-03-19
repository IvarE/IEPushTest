USE [IntegrationDB]
GO

/****** Object:  Table [dbo].[kunddatabas]    Script Date: 2015-04-24 12:41:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[kunddatabas](
	[customer_id] [varchar](255) NULL,
	[customer_type] [varchar](255) NULL,
	[one_time_customer] [varchar](255) NULL,
	[company] [varchar](255) NULL,
	[address] [varchar](255) NULL,
	[address2] [varchar](255) NULL,
	[ZIP] [varchar](255) NULL,
	[city] [varchar](255) NULL,
	[province_state] [varchar](255) NULL,
	[ccode] [varchar](255) NULL,
	[language] [varchar](255) NULL,
	[FAX] [varchar](255) NULL,
	[orgnr] [varchar](255) NULL,
	[VATnumber] [varchar](255) NULL,
	[customer_no] [varchar](255) NULL,
	[comments] [varchar](5000) NULL,
	[pricelist_id] [varchar](255) NULL,
	[warehouse_id] [varchar](255) NULL,
	[customer_category_id] [varchar](255) NULL,
	[preferred_shippinglocation_id] [varchar](255) NULL,
	[discount] [varchar](255) NULL,
	[currency] [varchar](255) NULL,
	[credit_limit] [varchar](255) NULL,
	[credit_open] [varchar](255) NULL,
	[customer_keys] [varchar](255) NULL,
	[state] [varchar](255) NULL,
	[validated_name] [varchar](255) NULL,
	[validated_firstname] [varchar](255) NULL,
	[validated_lastname] [varchar](255) NULL,
	[validated_address] [varchar](255) NULL,
	[validated_ZIP] [varchar](255) NULL,
	[validated_city] [varchar](255) NULL,
	[validation_date] [varchar](255) NULL,
	[validation_service] [varchar](255) NULL,
	[validation_state] [varchar](255) NULL,
	[validation_string] [varchar](255) NULL,
	[default_payment_id] [varchar](255) NULL,
	[default_shipping_id] [varchar](255) NULL,
	[ctext1] [varchar](255) NULL,
	[ctext2] [varchar](255) NULL,
	[ctext3] [varchar](255) NULL,
	[ctext4] [varchar](255) NULL,
	[ctext5] [varchar](255) NULL,
	[ctext6] [varchar](255) NULL,
	[cselect1] [varchar](255) NULL,
	[cselect2] [varchar](255) NULL,
	[cselect3] [varchar](255) NULL,
	[cselect4] [varchar](255) NULL,
	[cselect5] [varchar](255) NULL,
	[cselect6] [varchar](255) NULL,
	[cselect7] [varchar](255) NULL,
	[cselect8] [varchar](255) NULL,
	[cselect9] [varchar](255) NULL,
	[cselect10] [varchar](255) NULL,
	[affiliate_id] [varchar](255) NULL,
	[affiliate_date] [varchar](255) NULL,
	[affiliate_member] [varchar](255) NULL,
	[surname] [varchar](255) NULL,
	[lastname] [varchar](255) NULL,
	[phone] [varchar](255) NULL,
	[cellphone] [varchar](255) NULL,
	[email] [varchar](255) NULL,
	[login] [varchar](255) NULL,
	[password] [varchar](255) NULL,
	[want_newsletter] [varchar](255) NULL,
	[have_sent_welcomemail] [varchar](255) NULL,
	[creditworthy] [varchar](255) NULL,
	[admin_id] [varchar](255) NULL,
	[order_count] [varchar](255) NULL,
	[order_totals] [varchar](255) NULL,
	[last_order] [varchar](255) NULL,
	[changed] [varchar](255) NULL,
	[created] [varchar](255) NULL,
	[synced] [varchar](255) NULL,
	[upd_state] [varchar](255) NULL,
	[cardnumber] [varchar](255) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


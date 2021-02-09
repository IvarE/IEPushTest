USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetIncidentFromIncidentId]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_GetIncidentFromIncidentId]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetDefaultCustomer]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_GetDefaultCustomer]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromEmail]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountNumber]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromAccountNumber]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountId]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromAccountId]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateTravelCardImportTable]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_CreateTravelCardImportTable]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateContactImportTable]    Script Date: 2014-10-27 17:52:13 ******/
DROP PROCEDURE [dbo].[sp_CreateContactImportTable]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateContactImportTable]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-27
-- Description:	Populate importtable for contacts
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateContactImportTable] 

AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	delete from contacts

	select
	contact.cgi_importid,
	contact.contactid,
	contact.firstname,
	contact.lastname
	into #contacts
	from Skanetrafiken_MSCRM.dbo.FilteredContact contact
	where contact.cgi_importid is not null


	insert into contacts (customer_id, customer_type, one_time_customer, company, address, address2, ZIP, city, province_state, ccode, language, FAX, 
						orgnr, VATnumber, customer_no, comments, pricelist_id, warehouse_id, customer_category_id, preferred_shippinglocation_id, 
						discount, currency, credit_limit, credit_open, customer_keys, state, validated_name, validated_firstname, validated_lastname, 
						validated_address, validated_ZIP, validated_city, validation_date, validation_service, validation_state, validation_string, 
						default_payment_id, default_shipping_id, ctext1, ctext2, ctext3, ctext4, ctext5, ctext6, 
						cselect1, cselect2, cselect3, cselect4, cselect5, cselect6, cselect7, cselect8, cselect9, cselect10, 
						affiliate_id, affiliate_date, affiliate_member, surname, lastname, phone, cellphone, email, login, password, 
						want_newsletter, have_sent_welcomemail, creditworthy, admin_id, order_count, order_totals, last_order, changed, created, synced, upd_state)
	select
	distinct
	c.customer_id, c.customer_type, c.one_time_customer, c.company, c.address, c.address2, c.ZIP, c.city, c.province_state, c.ccode,
	c.language, c.FAX, c.orgnr, c.VATnumber, c.customer_no, c.comments, c.pricelist_id, c.warehouse_id, c.customer_category_id, c.preferred_shippinglocation_id,
	c.discount, c.currency, c.credit_limit, c.credit_open, c.customer_keys, c.state, c.validated_name, c.validated_firstname, c.validated_lastname, c.validated_address,
	c.validated_ZIP, c.validated_city, c.validation_date, c.validation_service, c.validation_state, c.validation_string, c.default_payment_id, c.default_shipping_id,
	c.ctext1, c.ctext2, c.ctext3, c.ctext4, c.ctext5, c.ctext6, c.cselect1, c.cselect2, c.cselect3, c.cselect4, c.cselect5,
	c.cselect6, c.cselect7, c.cselect8, c.cselect9, c.cselect10,
	c.affiliate_id, c.affiliate_date, c.affiliate_member, c.surname, c.lastname, c.phone, c.cellphone, c.email, c.login, c.password, c.want_newsletter, c.have_sent_welcomemail,
	c.creditworthy, c.admin_id, c.order_count, c.order_totals, c.last_order, c.changed, c.created, c.synced, c.upd_state
	from contactimport c
	order by c.customer_id

	update contacts
	set contactidx = '00000000-0000-0000-0000-000000000000'

	update contacts
	set contacts.contactidx = #contacts.contactid
	from contacts
	inner join #contacts
	on contacts.customer_id collate Latin1_General_CI_AI = #contacts.cgi_importid

	drop table #contacts

	REVERT

END

GO

/****** Object:  StoredProcedure [dbo].[sp_CreateTravelCardImportTable]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-27
-- Description:	Populate importtable for travelcards
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateTravelCardImportTable] 

AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE AS LOGIN = 'D1\CRMAdmin'

	delete from travelcards

	select
	travelcard.cgi_importid,
	travelcard.cgi_travelcardid,
	travelcard.cgi_contactid,
	travelcard.cgi_travelcardname,
	travelcard.cgi_travelcardnumber
	into #travelcards
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_travelcard travelcard
	where travelcard.cgi_importid is not null

	insert into travelcards (customer_id, cardnumber)
	select
	customer_id, cardnumber
	from contactimport
	order by customer_id

	update travelcards
	set travelcardid = '00000000-0000-0000-0000-000000000000' ,
	contactidx = '00000000-0000-0000-0000-000000000000'

	update travelcards
	set travelcards.travelcardid = #travelcards.cgi_travelcardid
	from travelcards
	inner join #travelcards
	on travelcards.customer_id collate Latin1_General_CI_AI = #travelcards.cgi_importid
	
	update travelcards
	set travelcards.contactidx = contact.contactid
	from travelcards
	inner join Skanetrafiken_MSCRM.dbo.FilteredContact contact
	on travelcards.customer_id collate Latin1_General_CI_AI = contact.cgi_importid
	
	drop table #travelcards

	REVERT

END


GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountId]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on accountid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromAccountId] 
	@accountid	uniqueidentifier,
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where contact.contactid = @accountid
		and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
		account.accountid as accountid,
		account.accountnumber as accountnumber,
		account.name as name,
		account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
		where account.accountid = @accountid
		and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END



GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountNumber]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on accountnumber
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromAccountNumber] 
	@accountnumber	varchar(255),
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'
	
	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
			where contact.cgi_contactnumber = @accountnumber
			and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
			account.accountid as accountid,
			account.accountnumber as accountnumber,
			account.name as name,
			account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
			where account.accountnumber = @accountnumber
			and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END




GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on emailaddress
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromEmail] 
	@emailaddress	varchar(255),
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where contact.emailaddress1 = @emailaddress
		and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
		account.accountid as accountid,
		account.accountnumber as accountnumber,
		account.name as name,
		account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
		where account.emailaddress1 = @emailaddress
		and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END



GO

/****** Object:  StoredProcedure [dbo].[sp_GetDefaultCustomer]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-20
-- Description:	Get default customer from setting
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDefaultCustomer]
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	declare @now datetime
	select @now = DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)

	select setting.cgi_defaultcustomeroncase as defaultcustomeroncase
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_setting setting
	where setting.cgi_validfrom <= @now
	and (setting.cgi_validto is null or setting.cgi_validto >= @now)
	and setting.statecode = 0
	for xml path ('setting'), root ('settings'), type

	REVERT

END

GO

/****** Object:  StoredProcedure [dbo].[sp_GetIncidentFromIncidentId]    Script Date: 2014-10-27 17:52:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	get incident from incidentid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetIncidentFromIncidentId] 
	@incidentid		uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	incident.ticketnumber as ticketnumber,
	incident.title as title
	from Skanetrafiken_MSCRM.dbo.FilteredIncident incident
	where incident.incidentid = @incidentid
	for xml path ('incident'), root ('incidents'), type
	
	REVERT

END

GO



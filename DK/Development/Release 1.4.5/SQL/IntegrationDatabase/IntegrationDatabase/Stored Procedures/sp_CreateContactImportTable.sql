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
	from [$(Skanetrafiken_MSCRM)].dbo.FilteredContact contact
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

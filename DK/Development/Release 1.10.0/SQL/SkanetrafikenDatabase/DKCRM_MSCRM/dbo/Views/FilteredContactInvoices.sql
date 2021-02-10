

--
-- report view for contactinvoices
--
create view dbo.[FilteredContactInvoices] (
    [contactid],
    [contactinvoiceid],
    [invoiceid],
    [versionnumber]
) with view_metadata as
select
    [ContactInvoices].[ContactId],
    [ContactInvoices].[ContactInvoiceId],
    [ContactInvoices].[InvoiceId],
    [ContactInvoices].[VersionNumber]
from ContactInvoices

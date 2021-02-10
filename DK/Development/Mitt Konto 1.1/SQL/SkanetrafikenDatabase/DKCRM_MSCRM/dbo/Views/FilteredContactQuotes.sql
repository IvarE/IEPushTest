

--
-- report view for contactquotes
--
create view dbo.[FilteredContactQuotes] (
    [contactid],
    [contactquoteid],
    [quoteid],
    [versionnumber]
) with view_metadata as
select
    [ContactQuotes].[ContactId],
    [ContactQuotes].[ContactQuoteId],
    [ContactQuotes].[QuoteId],
    [ContactQuotes].[VersionNumber]
from ContactQuotes

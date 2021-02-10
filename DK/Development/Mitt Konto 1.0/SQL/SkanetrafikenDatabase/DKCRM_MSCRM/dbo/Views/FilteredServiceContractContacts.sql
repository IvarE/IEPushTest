

--
-- report view for servicecontractcontacts
--
create view dbo.[FilteredServiceContractContacts] (
    [contactid],
    [contractid],
    [servicecontractcontactid],
    [servicelevel],
    [versionnumber]
) with view_metadata as
select
    [ServiceContractContacts].[ContactId],
    [ServiceContractContacts].[ContractId],
    [ServiceContractContacts].[ServiceContractContactId],
    [ServiceContractContacts].[ServiceLevel],
    [ServiceContractContacts].[VersionNumber]
from ServiceContractContacts

using System;
using Generic=System.Collections.Generic;
using System.Linq;
using System.Web;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Threading;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;


namespace CGICRMPortalService
{
    public class CustomerContactManager
    {
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;

        public CustomerContactManager()
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public CustomerContactManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }

        #region [Private Methods]
        private Contact GetContactFromCustomerCustomer(string AccountNumber,CustomerContact customerContact,bool bUpdate)
        {
            Guid AccountId =xrmHelper.GetIdByValue(AccountNumber,"accountnumber","account",xrmMgr);
            Contact contact = new Contact
            {
               
                ContactId = bUpdate?customerContact.ContactId:Guid.Empty,                              
                FirstName = customerContact.FirstName,
                LastName = customerContact.LastName,                
                Street1 = customerContact.Street1,
                PostalCode = customerContact.PostalCode,
                City = customerContact.City,
                Address1_County = customerContact.County,
                Address1_Country = customerContact.Country,
                Address1_CareOff = customerContact.CareOff,
                ContactPerson = customerContact.ContactPerson,                
                ContactPhoneNumber = customerContact.ContactPhoneNumber,
                SMSNotificationNumber = customerContact.SMSNotificationNumber,
                Email = customerContact.Email,
                IsPrimaryAddress = customerContact.IsPrimaryAddress,
                AddressType = new OptionSetValue((Int32)customerContact.AddressType),
                ParentCustomer = new EntityReference("account",AccountId)
            };
            return contact;
        }
        private CustomerContact GetCustomerContactFromContact(Contact contact)
        {
            CustomerContact customerContact = new CustomerContact
            {
                ContactId=contact.ContactId,                
                FirstName = contact.FirstName,
                LastName=contact.LastName,                                 
                Street1 = contact.Street1,
                PostalCode = contact.PostalCode,
                City = contact.City,
                County = contact.Address1_County,
                Country = contact.Address1_Country,
                CareOff = contact.Address1_CareOff,
                ContactPerson = contact.ContactPerson,                
                ContactPhoneNumber = contact.ContactPhoneNumber,
                SMSNotificationNumber = contact.SMSNotificationNumber,
                Email = contact.Email,      
                IsPrimaryAddress =contact.IsPrimaryAddress,
                AddressType = (AddressTypeCode)contact.AddressType.Value
            };
            return customerContact;
        }

        #endregion
        #region [Public Methods]

        internal string CreateCustomerContact(string customerId, CustomerContact customerContact)
        {
            Entity entity = xrmMgr.Create<Contact>(GetContactFromCustomerCustomer(customerId,customerContact,false));
            return entity.Attributes.ContainsKey("contactnumber") ? entity.GetAttributeValue<string>("contactnumber") : string.Empty;           
        }

        internal bool UpdateCustomerContact(string accountNumber, string contactNumber,CustomerContact customerContact)
        {
            bool UpdateCustomerContact = false;

            try
            {
                Contact contact = GetContactFromCustomerCustomer(accountNumber,customerContact,true);
                contact.ContactId = xrmHelper.GetIdByValue(contactNumber,"cgi_contactnumber","contact", xrmMgr);
                xrmMgr.Update<Contact>(contact);
                UpdateCustomerContact = true;
            }
            catch (Exception)
            {
                throw;
            }
            return UpdateCustomerContact;            
            
        }
        public CustomerContact GetCustomerContact(string accountNumber, string contactNumber)
        {
            Guid parentCustomerid = xrmHelper.GetIdByValue(accountNumber, "accountnumber", "account", xrmMgr);

            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName = "contact";
            queryByAttribute.ColumnSet = new ColumnSet(true);
            queryByAttribute.AddAttributeValue("parentcustomerid", parentCustomerid);
            queryByAttribute.AddAttributeValue("cgi_contactnumber", contactNumber);
            ObservableCollection<Contact> contacts = xrmMgr.Get<Contact>(queryByAttribute);
            return GetCustomerContactFromContact(contacts.FirstOrDefault());
        }
        public List<CustomerContact> GetCustomerContacts(string accountNumber)
        {
            Guid parentCustomerid = xrmHelper.GetIdByValue(accountNumber, "accountnumber", "account", xrmMgr);

            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName = "contact";
            queryByAttribute.ColumnSet = new ColumnSet(true);
            queryByAttribute.AddAttributeValue("parentcustomerid", parentCustomerid);            
            ObservableCollection<Contact> contacts = xrmMgr.Get<Contact>(queryByAttribute);
            List<CustomerContact> lstCustomerContact = (from contact in contacts
                                                         select GetCustomerContactFromContact(contact)).ToList();

            return lstCustomerContact;
        }
        internal bool RemoveCustomerContact(string accountNumber, string contactNumber)
        {
            bool bRemoveCustomerContact = false;

            try
            {
                Guid parentCustomerid = xrmHelper.GetIdByValue(accountNumber, "accountnumber", "account", xrmMgr);
                QueryByAttribute queryByAttribute = new QueryByAttribute();
                queryByAttribute.EntityName = "contact";
                queryByAttribute.ColumnSet = new ColumnSet(true);
                queryByAttribute.AddAttributeValue("parentcustomerid", parentCustomerid);
                queryByAttribute.AddAttributeValue("cgi_contactnumber", contactNumber);
                ObservableCollection<Contact> contacts = xrmMgr.Get<Contact>(queryByAttribute);
                xrmMgr.Delete("contact",contacts.FirstOrDefault().ContactId);
            }
            catch (Exception)
            {
                throw;
            }

            return bRemoveCustomerContact;

        }
        #endregion

        
        
    }
}
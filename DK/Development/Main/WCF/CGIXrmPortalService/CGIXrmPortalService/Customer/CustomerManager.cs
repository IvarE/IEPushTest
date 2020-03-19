using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;
using CGICRMPortalService.Customer.CrmClasses;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService.Shared;
using CGICRMPortalService.Shared.Models;
using CGIXrmWin;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGICRMPortalService.Customer
{
    public class CustomerManager
    {
        #region Declarations
        private readonly XrmManager _xrmMgr;
        private readonly XrmHelper _xrmHelper;
        readonly LogToCrm _log2Crm;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomerManager()
        {
            _xrmHelper = new XrmHelper();
            _xrmMgr = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
            _log2Crm = new LogToCrm();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        public CustomerManager(Guid callerId)
        {
            _xrmHelper = new XrmHelper();
            _xrmMgr = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
            _log2Crm = new LogToCrm();
        }
        #endregion

        #region Private Methods

        private bool ConvertCustomerState(int statecode)
        {
            switch(statecode)
            {
                case (int)StateCode.Active:
                    return false;
                case (int)StateCode.Inactive:
                    return true;
                default:
                    return true;
            }
        }


        private int ConvertCustomerStateToCrm(bool statecode)
        {
            switch (statecode)
            {
                case false:
                    return (int)StateCode.Active; //Active
                case true:
                    return (int)StateCode.Inactive; //Inactive
                default:
                    return (int)StateCode.Inactive; //Inactive
            }
        }


        /// <summary>
        /// Transforms Customer Object to Account Entity Object
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="bUpdate">Is the transformation for Update</param>
        /// <returns>Account Entity</returns>
        private Entity GetAccountEntityFromCustomer(Models.Customer customer, bool bUpdate)
        {
            Entity account = new Entity("account");

            try
            {
                if (bUpdate)
                {
                    account.Id = customer.AccountId;
                }

                if (String.IsNullOrEmpty(customer.CompanyName))
                {
                    account.Attributes.Add("name", customer.AccountFirstName + " " + customer.AccountLastName);
                }
                else
                {
                    account.Attributes.Add("name", customer.CompanyName);
                }

                account.Attributes.Add("cgi_firstname", customer.AccountFirstName);
                account.Attributes.Add("cgi_lastname", customer.AccountLastName);
                account.Attributes.Add("telephone1", customer.Phone);
                account.Attributes.Add("telephone2", customer.MobilePhone);
                account.Attributes.Add("emailaddress1", customer.Email);
                account.Attributes.Add("cgi_allow_autoload", customer.AllowAutoLoad);
                account.Attributes.Add("cgi_max_cards_autoload", customer.MaxCardsAutoLoad);
                account.Attributes.Add("accountcategorycode", new OptionSetValue((Int32)customer.CustomerType));
                account.Attributes.Add("cgi_organizational_number", customer.OrganizationNumber);
                account.Attributes.Add("cgi_organization_sub_number", customer.OrganizationSubNumber);
                account.Attributes.Add("cgi_socialsecuritynumber", customer.SocialSecurtiyNumber);
                account.Attributes.Add("cgi_rsid", customer.RSID);

                account.Attributes.Add("cgi_debtcollection", customer.OrganizationCreditApproved);      //Creditability Active/Inactive
                account.Attributes.Add("cgi_activated", !customer.InActive);                             //WEB E-Commerce Active/Inactive
                account.Attributes.Add("statuscode", new OptionSetValue(ConvertCustomerStateToCrm(customer.Deleted)));      //CRM Entity Active/Inactive
                if (customer.HasEpiserverAccount != null)
                {
                    account.Attributes.Add("cgi_myaccount", customer.HasEpiserverAccount);
                }
            }
            catch (Exception ex)
            {
                CreateExceptionEventLogEntry(ex.Message + " \n\n" + ex.InnerException + " \n\n" + ex.StackTrace);
                throw new WebException(ex.Message);
            }

            return account;
        }



        /// <summary>
        /// Transforms Customer Object to Account Entity Object
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="bUpdate">Is the transformation for Update</param>
        /// <returns>Account Entity</returns>
        //  Obsolete - Removed by JohanA
        //private Entity GetContactEntityFromCustomer(Models.Customer customer, bool bUpdate)
        //{
        //    Entity contact = new Entity("contact");
        //    if (bUpdate)
        //    {
        //        contact.Id = customer.AccountId;
        //    }

        //    contact.Attributes.Add("firstname", customer.AccountFirstName);
        //    contact.Attributes.Add("lastname", customer.AccountLastName);
        //    contact.Attributes.Add("telephone1", customer.Phone);
        //    contact.Attributes.Add("telephone2", customer.MobilePhone);
        //    contact.Attributes.Add("emailaddress1", customer.Email);
        //    contact.Attributes.Add("cgi_allow_autoload", customer.AllowAutoLoad);
        //    contact.Attributes.Add("cgi_max_cards_autoload", customer.MaxCardsAutoLoad);
        //    contact.Attributes.Add("cgi_socialsecuritynumber", customer.SocialSecurtiyNumber);

        //    contact.Attributes.Add("cgi_debtcollection", customer.OrganizationCreditApproved);        //Creditability Active/Inactive
        //    contact.Attributes.Add("statuscode", new OptionSetValue(ConvertCustomerStateToCrm(customer.Deleted)));      //CRM Entity Active/Inactive
        //    contact.Attributes.Add("cgi_activated", !customer.InActive);                             //WEB E-Commerce Active/Inactive
        //    if (customer.HasEpiserverAccount!=null) {
        //        contact.Attributes.Add("cgi_myaccount", customer.HasEpiserverAccount);
        //    }

        //    return contact;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="address"></param>
        /// <param name="bUpdate"></param>
        /// <returns></returns>
        private Entity AddAddesstoCustomerEntity(Entity entity, Address address, bool bUpdate)
        {
            if (bUpdate)
            {
                entity.Attributes.Add("address1_addressid", new Guid(address.AddressId));
            }
            
            entity.Attributes.Add("address1_name", address.CompanyName);
            entity.Attributes.Add("address1_line1", address.CareOff);
            entity.Attributes.Add("address1_line2", address.Street);
            entity.Attributes.Add("address1_postalcode", address.PostalCode);
            entity.Attributes.Add("address1_city", address.City);
            entity.Attributes.Add("address1_county", address.County);
            entity.Attributes.Add("address1_country", address.Country);
            entity.Attributes.Add("address1_primarycontactname", address.ContactPerson);
            entity.Attributes.Add("address1_telephone1", address.ContactPhoneNumber);
            entity.Attributes.Add("address1_telephone2", address.SMSNotificationNumber);
            entity.Attributes.Add("address1_addresstypecode", new OptionSetValue((int)AddressTypeCode.Invoice));

            return entity;
        }


        /// <summary>
        /// Transform Account Object to Customer Object
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Customer Object</returns>
        private Models.Customer GetCustomerFromAccount(Account account)
        {
            Models.Customer customer = new Models.Customer
            {
                AccountId = account.AccountId == Guid.Empty ? Guid.NewGuid() : account.AccountId,
                AccountNumber = account.AccountNumber ?? String.Empty,
                CompanyName = account.Name ?? String.Empty,
                AccountFirstName = account.AccountFirstName ?? String.Empty,
                AccountLastName = account.AccountLastName ?? String.Empty,
                SocialSecurtiyNumber = account.SocialSecurtiyNumber ?? String.Empty,
                Email = account.Email ?? String.Empty,
                Phone = account.MainPhone ?? String.Empty,
                MobilePhone = account.OtherPhone ?? String.Empty,
                AllowAutoLoad = account.AllowAutoLoad,
                MaxCardsAutoLoad = account.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Company,
                OrganizationCreditApproved = account.OrganizationCreditApproved,
                OrganizationNumber = account.OrganizationNumber ?? String.Empty,
                OrganizationSubNumber = account.OrganizationSubNumber ?? String.Empty,
                InActive = !account.InActive,
                Deleted = ConvertCustomerState(account.Deleted.Value),
                RSID = account.CareOff,
            };


            return customer;
        }


        /// <summary>
        /// Transform Account Object to Customer Object
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Customer Object</returns>
        private Models.Customer GetCustomerFromContact(Contact contact)
        {
            Models.Customer customer = new Models.Customer
            {
                AccountId = contact.ContactId == Guid.Empty ? Guid.NewGuid() : contact.ContactId,
                AccountNumber = contact.ContactNumber ?? String.Empty,
                CompanyName = contact.FullName ?? String.Empty,
                AccountFirstName = contact.FirstName ?? String.Empty,
                AccountLastName = contact.LastName ?? String.Empty,
                SocialSecurtiyNumber = contact.SocialSecurtiyNumber ?? String.Empty,
                Email = contact.Email ?? String.Empty,
                Phone = contact.MainPhone ?? String.Empty,
                MobilePhone = contact.OtherPhone ?? String.Empty,
                AllowAutoLoad = contact.AllowAutoLoad,
                MaxCardsAutoLoad = contact.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Private,
                InActive = !contact.InActive,
                Deleted = ConvertCustomerState(contact.Deleted.Value),
            };

            return customer;
        }


        /// <summary>
        /// Method retrieving the address for a customer based on given parameters.
        /// </summary>
        /// <param name="accountId">Customer Unique Identifier</param>
        /// <param name="address">Address Entity</param>
        /// <param name="bUpdate">True: Update adress, False: Do not update the address</param>
        /// <returns></returns>
        private Entity GetCustomerAddressEntity(Guid accountId, Address address, bool bUpdate)
        {
            Entity customerAddress = new Entity("customeraddress");
            if (bUpdate)
            {
                customerAddress.Id = address.CustomerAddressId;
            }
            else
            {
                customerAddress.Attributes.Add("parentid", new EntityReference("account", accountId));
            }

            customerAddress.Attributes.Add("addresstypecode", new OptionSetValue((int)address.AddressType));
            customerAddress.Attributes.Add("name", address.CompanyName);
            customerAddress.Attributes.Add("line2", address.Street);
            customerAddress.Attributes.Add("postalcode", address.PostalCode);
            customerAddress.Attributes.Add("city", address.City);
            customerAddress.Attributes.Add("county", address.County);
            customerAddress.Attributes.Add("country", address.Country);
            customerAddress.Attributes.Add("line1", address.CareOff);
            customerAddress.Attributes.Add("primarycontactname", address.ContactPerson);
            customerAddress.Attributes.Add("telephone1", address.ContactPhoneNumber);
            customerAddress.Attributes.Add("telephone2", address.SMSNotificationNumber);

            customerAddress.Attributes.Add("cgi_email",
                !string.IsNullOrEmpty(address.EmailNotificationAddress) ? address.EmailNotificationAddress : "<missing>");

            return customerAddress;
        }


        /// <summary>
        /// Method de-activating an existing customer based on given parameters.
        /// </summary>
        /// <param name="customerId">Customer Unique Identifier</param>
        /// <param name="entityName">Entity name: Account for company 
        /// customer or Contact for private customer</param>
        private void DeActivateCustomer(Guid customerId, string entityName)
        {

            SetStateRequest deactivateCustomerRequest = new SetStateRequest()
            {
                EntityMoniker = new EntityReference()
                {
                    Id = customerId,
                    LogicalName = entityName,
                },
                State = new OptionSetValue(1),
                Status = new OptionSetValue(2)
            };
            _xrmMgr.Service.Execute(deactivateCustomerRequest);

        }


        private ObservableCollection<CustomerAddress> GetAddressesForAccount(Guid parentAccountId)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
            queryByAttribute.AddAttributeValue("parentid", parentAccountId);
            
            queryByAttribute.ColumnSet = new ColumnSet(true);
            ObservableCollection<CustomerAddress> customerAddresses = _xrmMgr.Get<CustomerAddress>(queryByAttribute);

            return customerAddresses;
        }


        /// <summary>
        /// Method creating an event log entry in Windows OS this service is running at.
        /// </summary>
        /// <param name="errorMessage">Exception Message</param>
        private void CreateExceptionEventLogEntry(string errorMessage)
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var logType = "Application";

            if (!EventLog.SourceExists(appName))
                EventLog.CreateEventSource(appName, logType);
                
            //EventLog.WriteEntry(appName, errorMessage, EventLogEntryType.Error);


            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(errorMessage, EventLogEntryType.Error, 101, 1);
            } 
        }

        private static void SetGetCustomerMethodDefaultValueForHasEpiserverAccount(Models.Customer customer)
        {
            /** Star and episerver has bool on the other side. 
             * Cant be null for serialization. They dont use this field. Maybe another class or subclass in future,
             * or just a bool on customer that even if not retrieved serializes to false. ( proberbly )
             * But that might effect create and update if they dont send it in in all calls.
             * A bool that has required attribute would proberbly be the best thing.
             **/
            customer.HasEpiserverAccount = false;
        }


        private void HandleFaultException(UpdateCustomerResponse response, FaultException<OrganizationServiceFault> ex, string methodName)
        {
            CreateExceptionEventLogEntry(ex.Message + "\n\n" + ex.Detail + "\n\n" + ex.StackTrace);
            string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp + "Code: " + ex.Detail.ErrorCode + "Message: " + ex.Detail.Message + "Trace: " + ex.Detail.TraceText + "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            _log2Crm.Exception(exceptionMsg, methodName, ex, "Portal Web Service");
            response.Status = ProcessingStatus.FAILED;
            response.Message = exceptionMsg;
        }

        private void HandleTimeOutExceptionn(UpdateCustomerResponse response, TimeoutException ex, string methodName)
        {
            string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
            _log2Crm.Exception(exceptionMsg, methodName, ex, "Portal Web Service");

            response.Status = ProcessingStatus.FAILED;
            response.Message = exceptionMsg;
        }

        private void HandleException(UpdateCustomerResponse response, Exception ex, string methodName)
        {
            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string exceptionMsg = ex.InnerException.Message;

                FaultException<OrganizationServiceFault> fe = ex.InnerException as FaultException<OrganizationServiceFault>;

                if (fe != null)
                {
                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp + "Code: " + fe.Detail.ErrorCode + "Message: " + fe.Detail.Message + "Trace: " + fe.Detail.TraceText + "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                }
                _log2Crm.Exception(exceptionMsg, methodName, ex, "Portal Web Service");
            }
            else
            {
                string exceptionMsg = "The application terminated with an error." + ex.Message;
                _log2Crm.Exception(exceptionMsg, "UpdateMyAccountLastLogin", ex, "Portal Web Service");

                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This Method is used to get the Account number for the given guid of the account record.
        /// </summary>
        /// <param name="customerId">Guid of the Account Record</param>
        /// <param name="customerType">Type of customer</param>
        /// <returns>Account Number of the Record as a String</returns>
        public string GetAccountNumber(Guid customerId, AccountCategoryCode customerType)
        {
            string retValue = string.Empty;
            if (customerId == Guid.Empty)
            {
                string errorMsg = "Object reference not set to an instance of an object (Customer Id).";
                NullReferenceException nullReferenceException = new NullReferenceException(errorMsg);
                string exceptionMsg = "The application terminated with an error. Timestamp: " + DateTime.Now.ToUniversalTime() + "Message: " + errorMsg;
                _log2Crm.Exception(exceptionMsg, "GetAccountNumber", nullReferenceException, "Portal Web Service");
                throw nullReferenceException;
            }
            switch (customerType)
            {
                case AccountCategoryCode.Private:
                    Contact contact = _xrmMgr.Get<Contact>(customerId, "cgi_contactnumber");
                    if (contact != null)
                        retValue = contact.ContactNumber;
                    break;
                case AccountCategoryCode.Company:
                    Account account = _xrmMgr.Get<Account>(customerId, "accountnumber");
                    if (account != null)
                        retValue = account.AccountNumber;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("customerType", customerType, null);
            }


            return retValue;
        }


        /// <summary>
        /// This method will check if the email provided already exist in the system.
        /// This check will be performed against the registered email id of the customer.
        /// </summary>
        /// <param name="customerEmail">email id of the customer</param>
        /// <returns>returns true or false based on the result. if the email id already exist then it will retrun true else false</returns>
        public CheckCustomerExistResponse CheckCustomerExist(string customerEmail)
        {
            CheckCustomerExistResponse chkCustomerExistResponse = new CheckCustomerExistResponse();

            try
            {
                if (customerEmail == null)
                {
                    string errorMsg = "Object reference not set to an instance of an object (Customer Email).";
                    NullReferenceException nullReferenceException =  new NullReferenceException(errorMsg);
                    string exceptionMsg = "The application terminated with an error. Timestamp: " + DateTime.Now.ToUniversalTime() + "Message: " + errorMsg;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", nullReferenceException, "Portal Web Service");
                    throw nullReferenceException;
                }
                Guid accountGuid = _xrmHelper.GetIdByValue(customerEmail, "emailaddress1", "contact", _xrmMgr);

                if (accountGuid != Guid.Empty)
                {
                    Entity contact = _xrmMgr.Get("contact", accountGuid, "cgi_contactnumber");
                    chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                    chkCustomerExistResponse.AccountId = accountGuid;
                    chkCustomerExistResponse.CustomerType = AccountCategoryCode.Private;
                    chkCustomerExistResponse.AccountNumber = contact.Attributes.Contains("cgi_contactnumber") ? contact.GetAttributeValue<string>("cgi_contactnumber") : string.Empty;
                    chkCustomerExistResponse.CustomerExists = true;
                    chkCustomerExistResponse.Deleted = false;
                }
                else
                {
                    accountGuid = _xrmHelper.GetIdByValue(customerEmail, "emailaddress1", "account", _xrmMgr);

                    if (accountGuid != Guid.Empty)
                    {
                        Entity account = _xrmMgr.Get("account", accountGuid, "accountnumber");
                        chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                        chkCustomerExistResponse.AccountId = accountGuid;
                        chkCustomerExistResponse.CustomerType = AccountCategoryCode.Company;
                        chkCustomerExistResponse.AccountNumber = account.Attributes.Contains("accountnumber") ? account.GetAttributeValue<string>("accountnumber") : string.Empty;
                        chkCustomerExistResponse.CustomerExists = true;
                        chkCustomerExistResponse.Deleted = false;
                    }
                    else
                    {
                        chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                        chkCustomerExistResponse.Deleted = true;
                        chkCustomerExistResponse.Message = "No customer with email, " + customerEmail + " could be found.";
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp + "Code: " + ex.Detail.ErrorCode + "Message: " + ex.Detail.Message + "Trace: " + ex.Detail.TraceText + "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");

                chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                chkCustomerExistResponse.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                chkCustomerExistResponse.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp + "Code: " + fe.Detail.ErrorCode + "Message: " + fe.Detail.Message + "Trace: " + fe.Detail.TraceText + "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                    chkCustomerExistResponse.Message = exceptionMsg;
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                    chkCustomerExistResponse.Message = exceptionMsg;
                }
            }
            return chkCustomerExistResponse;
        }


        /// <summary>
        /// This method will create a new customer in crm system based on the provided data. 
        /// the method will not perform any duplicate check before creating the new record
        /// </summary>
        /// <param name="customer">customer object</param>
        /// <returns>this will return createcustomerresponse.</returns>
        public CreateCustomerResponse CreateCustomer(Models.Customer customer)
        {
            CreateCustomerResponse response = new CreateCustomerResponse();
            try
            {
                _log2Crm.LogMessage("Enter CreateCustomer", "CGIXrmPrortalService");

                if (customer == null)
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.Message = "Unable to Process request. Customer Object is null or not an Object";
                    return response;
                }
                Guid accountId;
                switch (customer.CustomerType)
                {
                    case AccountCategoryCode.Private:
                        // Endeavor JoAn - 161129
                        throw new Exception("Obsolete kod hittad (Create contact). Denna metod skall inte anropas!");

                        ////Create contact
                        //Entity contactEntity = GetContactEntityFromCustomer(customer, false);
                        //if (customer.Addresses != null && customer.Addresses.Length > 0)
                        //{
                        //    contactEntity = AddAddesstoCustomerEntity(contactEntity, customer.Addresses[0], false);
                        //} 
                        //accountId = _xrmMgr.Service.Create(contactEntity);
                        //_log2Crm.LogMessage("Customer created", "CGIXrmPrortalService");
                        ////Create Address
                        //break;
                    case AccountCategoryCode.Company:
                        //var _rsid = String.Empty;
                        accountId = _xrmMgr.Service.Create(GetAccountEntityFromCustomer(customer, false));
                        //Create Address
                        if (customer.Addresses != null)
                        {
                            foreach (Address address in customer.Addresses)
                            {
                                address.CareOff = customer.RSID;
                                _xrmMgr.Service.Create(GetCustomerAddressEntity(accountId, address, false));
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //Build Response
                if (accountId != Guid.Empty)
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.AccountNumber = GetAccountNumber(accountId, customer.CustomerType);
                    response.AccountId = accountId;
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                CreateExceptionEventLogEntry(ex.Message + " \n\n" + ex.InnerException + " \n\n" + ex.StackTrace);
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp + "Code: " + ex.Detail.ErrorCode + "Message: " + ex.Detail.Message + "Trace: " + ex.Detail.TraceText + "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp + "Code: " + fe.Detail.ErrorCode + "Message: " + fe.Detail.Message + "Trace: " + fe.Detail.TraceText + "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                    _log2Crm.Exception(exceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = exceptionMsg;
                }
            }
            return response;
        }

        public UpdateCustomerResponse UpdateMyAccountLastLogin(Guid customerId, bool isCompany, DateTime myAccountLastLogin)
        {
            string methodName = "UpdateMyAccountLastLogin";
            UpdateCustomerResponse response = new UpdateCustomerResponse();

            try
            {
                // Kör inte för kontakter. Måste gå genom fasaden.
                if(isCompany == false)
                {
                    // Do nothing...
                }
                else {

                    //var entity = isCompany ? new Entity("account") : new Entity("contact");
                    var entity = new Entity("account");
                    entity.Id = customerId;
                    DateTime _myAccountLastLogin = DateTime.SpecifyKind(myAccountLastLogin, DateTimeKind.Utc);
                    entity["cgi_myaccount_lastlogin"] = _myAccountLastLogin;
                    _xrmMgr.Service.Update(entity);
                }
                response.Status = ProcessingStatus.SUCCESS;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                HandleFaultException(response, ex, methodName);
            }
            catch (TimeoutException ex)
            {
                HandleTimeOutExceptionn(response, ex, methodName);
            }
            catch (Exception ex)
            {
                HandleException(response, ex, methodName);
            }
            return response;
        }

        /// <summary>
        /// This method will update the customer record. 
        /// All the data needs to be provided as this method will perform full upadate only.
        /// </summary>
        /// <param name="customerId">The Guid of the record which needs to be updated</param>
        /// <param name="customer">The customer object with the data that needs to updated.</param>
        /// <returns>return true or false based on the sucess or failure of updating the record.</returns>
        public UpdateCustomerResponse UpdateCustomer(Guid customerId, Models.Customer customer)
        {
            UpdateCustomerResponse response = new UpdateCustomerResponse();

            try
            {
                if (customer == null)
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.Message = "Unable to Process request. Customer Object is null or not an Object";
                    return response;
                }

                if (customer.Deleted)
                {
                    DeActivateCustomer(customerId, customer.CustomerType == AccountCategoryCode.Private ? "contact" : "account");
                }
                else
                {
                    switch (customer.CustomerType)
                    {
                        case AccountCategoryCode.Private:
                            throw new Exception("Obsolete kod hittad (Update contact). Denna skall inte anropas!");
                            ////Create contact
                            //Entity contactEntity = GetContactEntityFromCustomer(customer, true);
                            //if (customer.Addresses != null && customer.Addresses.Length > 0)
                            //{
                            //    contactEntity = AddAddesstoCustomerEntity(contactEntity, customer.Addresses[0], true);
                            //}
                            //_xrmMgr.Service.Update(contactEntity);

                            //break;
                        case AccountCategoryCode.Company:
                            //Update Account
                            _xrmMgr.Service.Update(GetAccountEntityFromCustomer(customer, true));

                            //Update Address
                            ObservableCollection<CustomerAddress> crmCustomerAddress = GetAddressesForAccount(customerId);
                            if (customer.Addresses.Length != crmCustomerAddress.Count)
                            {
                                var customerAddressIds = customer.Addresses.Select(ca => ca.AddressId);
                                List<CustomerAddress> lstAddressToDelete = crmCustomerAddress.Where(ca => !customerAddressIds.Contains(ca.AddressId.ToString())).ToList();
                                if (lstAddressToDelete.Count > 0)
                                {
                                    foreach (CustomerAddress itemToDelete in lstAddressToDelete)
                                    {
                                        _xrmMgr.Service.Delete("customeraddress", itemToDelete.CustomerAddressId);
                                    }
                                }
                            }
                            foreach (Address address in customer.Addresses)
                            {
                                var matchedAddress = crmCustomerAddress.FirstOrDefault(ca => ca.AddressId.ToString() == address.AddressId);

                                address.CareOff = customer.RSID;
                                if (matchedAddress != null)
                                {
                                    address.CustomerAddressId = matchedAddress.CustomerAddressId;
                                    _xrmMgr.Service.Update(GetCustomerAddressEntity(customerId, address, true));
                                }
                                else
                                {
                                    _xrmMgr.Service.Create(GetCustomerAddressEntity(customerId, address, false));
                                }
                            }

                            break;
                    }
                }
                //Build Response

                response.Status = ProcessingStatus.SUCCESS;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                HandleFaultException(response, ex, "UpdateCustomer");
            }
            catch (TimeoutException ex)
            {
                HandleTimeOutExceptionn(response, ex, "UpdateCustomer");
            }
            catch (Exception ex)
            {
                HandleException(response, ex, "UpdateCustomer");
            }
            return response;
        }

        /// <summary>
        /// Get the customer based on the customer id supplied
        /// </summary>
        /// <param name="customerId">guid of the customer record in crm</param>
        /// <param name="customerType">Type of customer</param>
        /// <returns>customer object</returns>
        public GetCustomerResponse GetCustomer(Guid customerId, AccountCategoryCode customerType)
        {
            GetCustomerResponse response = new GetCustomerResponse();
            if (customerId == Guid.Empty)
            {
                string errorMsg = "Object reference not set to an instance of an object (Customer Id).";
                NullReferenceException nullReferenceException = new NullReferenceException(errorMsg);
                string exceptionMsg = "The application terminated with an error. Timestamp: " + DateTime.Now.ToUniversalTime() + "Message: " + errorMsg;
                _log2Crm.Exception(exceptionMsg, "GetAccountNumber", nullReferenceException, "Portal Web Service");
                throw nullReferenceException;
            }

            try
            {
                switch (customerType)
                {
                    case AccountCategoryCode.Private:
                        QueryExpression queryGetCustomerByContact = new QueryExpression("contact")
                        {
                            ColumnSet = new ColumnSet(true)
                        };

                        FilterExpression filterExpressionForContact = new FilterExpression(LogicalOperator.And);
                        filterExpressionForContact.AddCondition(new ConditionExpression("contactid", ConditionOperator.Equal, customerId));
                        filterExpressionForContact.AddCondition(new ConditionExpression("statecode", ConditionOperator.In, 0, 1)); //Statecode 0 = Active contact
                        queryGetCustomerByContact.Criteria.AddFilter(filterExpressionForContact);

                        ObservableCollection<Contact> contacts = _xrmMgr.Get<Contact>(queryGetCustomerByContact);

                        if (contacts != null && contacts.Count > 0)
                        {
                            Contact retrievedContact = contacts.FirstOrDefault();

                            Models.Customer customer = GetCustomerFromContact(retrievedContact);
                            SetGetCustomerMethodDefaultValueForHasEpiserverAccount(customer);

                            if (retrievedContact != null)
                            {
                                Address address = new Address
                                {
                                    AddressType = AddressTypeCode.Invoice, AddressId = retrievedContact.AddressId.ToString(), CustomerAddressId = retrievedContact.AddressId, CompanyName = retrievedContact.Address1_Name, CareOff = retrievedContact.Address1_CareOff, Street = retrievedContact.Address1_Street1, City = retrievedContact.Address1_City, PostalCode = retrievedContact.Address1_PostalCode, County = retrievedContact.Address1_County, Country = retrievedContact.Address1_Country, ContactPerson = retrievedContact.Address1_ContactPerson, ContactPhoneNumber = retrievedContact.Address1_ContactPersonPhoneNumber, SMSNotificationNumber = retrievedContact.Address1_SMSNotificationNumber,
                                };

                                customer.Addresses = new[] {address};
                            }
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            var customer = new Models.Customer
                            {
                                AccountId = customerId,
                                CustomerType = customerType
                            };
                            SetGetCustomerMethodDefaultValueForHasEpiserverAccount(customer);
                            // TODO: customer är inte kopplat!!
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No matching records were found or customer has been deleted.";
                        }
                        break;
                    case AccountCategoryCode.Company:
                        QueryExpression queryGetCustomerByAccount = new QueryExpression("account")
                        {
                            ColumnSet = new ColumnSet(true)
                        };

                        FilterExpression filterExpressionForAccount = new FilterExpression(LogicalOperator.And);
                        filterExpressionForAccount.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                        filterExpressionForAccount.AddCondition(new ConditionExpression("statecode", ConditionOperator.In, 0, 1)); //Statecode 0 = active account
                        queryGetCustomerByAccount.Criteria.AddFilter(filterExpressionForAccount);

                        ObservableCollection<Account> accounts = _xrmMgr.Get<Account>(queryGetCustomerByAccount);

                        if (accounts != null && accounts.Count > 0)
                        {
                            Account retrievedAccount = accounts.FirstOrDefault();

                            Models.Customer customer = GetCustomerFromAccount(retrievedAccount);
                            SetGetCustomerMethodDefaultValueForHasEpiserverAccount(customer);

                            if (retrievedAccount != null)
                            {
                                customer.AccountFirstName = retrievedAccount.PrimaryContactFirstName;
                                customer.AccountLastName = retrievedAccount.PrimaryContactLastName;

                                QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
                                queryByAttribute.AddAttributeValue("parentid", retrievedAccount.AccountId);
                                queryByAttribute.ColumnSet = new ColumnSet(true);
                                ObservableCollection<CustomerAddress> customerAddresses = _xrmMgr.Get<CustomerAddress>(queryByAttribute);
                                customer.Addresses = (from address in customerAddresses
                                    where address.Street != null
                                    select new Address()
                                    {
                                        AddressId = address.CustomerAddressId.ToString(), CustomerAddressId = address.CustomerAddressId, AddressType = address.AddressType != null ? (AddressTypeCode) address.AddressType.Value : AddressTypeCode.None, CompanyName = address.CompanyName, Street = address.Street, PostalCode = address.PostalCode, City = address.City, County = address.County, Country = address.Country, CareOff = customer.RSID, ContactPerson = address.ContactPerson, ContactPhoneNumber = address.ContactPhoneNumber, SMSNotificationNumber = address.SMSNotificationNumber,
                                    }).ToArray();
                            }


                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            var customer = new Models.Customer
                            {
                                AccountId = customerId,
                                CustomerType = customerType
                            };
                            SetGetCustomerMethodDefaultValueForHasEpiserverAccount(customer);
                            response.Customer = customer;
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No matching records were found or customer has been deleted.";
                        }

                        break;
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp + "Code: " + ex.Detail.ErrorCode + "Message: " + ex.Detail.Message + "Trace: " + ex.Detail.TraceText + "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");

                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp + "Code: " + fe.Detail.ErrorCode + "Message: " + fe.Detail.Message + "Trace: " + fe.Detail.TraceText + "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                    _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");

                    response.Status = ProcessingStatus.FAILED;
                    response.Message = exceptionMsg;
                }
            }

            return response;
        }
        #endregion
    }
}
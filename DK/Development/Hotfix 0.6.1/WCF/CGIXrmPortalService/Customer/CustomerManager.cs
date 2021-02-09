using System;
using System.Linq;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel;
using CGIXrmLogger;
using CGICRMPortalService.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



namespace CGICRMPortalService
{
    public class CustomerManager
    {
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;
        LogToCrm log2Crm = new LogToCrm();
        public CustomerManager()
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
            log2Crm = new LogToCrm();
        }

        public CustomerManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
            log2Crm = new LogToCrm();
        }

        #region [Private Methods]
        /// <summary>
        /// Transforms Customer Object to Account Entity Object
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="bUpdate">Is the transformation for Update</param>
        /// <returns>Account Entity</returns>
        private Entity GetAccountEntityFromCustomer(Customer customer, bool bUpdate)
        {
            Entity account = new Entity("account");

            try
            {


                if (bUpdate)
                {
                    //accountEntity.Attributes.Add("accountid", customer.AccountId);
                    account.Id = customer.AccountId;
                }

                //if (customer.CustomerType == AccountCategoryCode.Company)
                //{
                //account.Attributes.Add("name", customer.CompanyName);
                //account.Attributes.Add("primarycontactid", new EntityReference("contact", customer.PrimaryContact));
                ////}
                //else
                //{
                if (string.IsNullOrEmpty(customer.CompanyName))
                    account.Attributes.Add("name", customer.AccountFirstName + " " + customer.AccountLastName);
                else
                    account.Attributes.Add("name", customer.CompanyName);

                account.Attributes.Add("cgi_firstname", customer.AccountFirstName);
                account.Attributes.Add("cgi_lastname", customer.AccountLastName);
                //}
                account.Attributes.Add("telephone1", customer.Phone);
                account.Attributes.Add("telephone2", customer.MobilePhone);
                account.Attributes.Add("emailaddress1", customer.Email);
                //accountEntity.Attributes.Add("donotbulkemail", !customer.AllowBulkEmail);
                //accountEntity.Attributes.Add("donotemail", !customer.AllowEmail);
                //accountEntity.Attributes.Add("donotphone", !customer.AllowPhone);
                //accountEntity.Attributes.Add("donotmail", !customer.AllowMail);     
                account.Attributes.Add("cgi_allow_autoload", customer.AllowAutoLoad);
                account.Attributes.Add("cgi_max_cards_autoload", customer.MaxCardsAutoLoad);
                account.Attributes.Add("accountcategorycode", new OptionSetValue((Int32)customer.CustomerType));
                account.Attributes.Add("cgi_debtcollection", customer.OrganizationCreditApproved);
                account.Attributes.Add("cgi_organizational_number", customer.OrganizationNumber);
                account.Attributes.Add("cgi_organization_sub_number", customer.OrganizationSubNumber);
                account.Attributes.Add("cgi_socialsecuritynumber", customer.SocialSecurtiyNumber);
                account.Attributes.Add("cgi_activated", !customer.InActive); //Inactive = false - Account Active , Inactive = true - Account Inactive
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return account;
        }

        /// <summary>
        /// Transforms Customer Object to Account Entity Object
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="bUpdate">Is the transformation for Update</param>
        /// <returns>Account Entity</returns>
        private Entity GetContactEntityFromCustomer(Customer customer, bool bUpdate)
        {
            Entity contact = new Entity("contact");
            if (bUpdate)
            {
                //accountEntity.Attributes.Add("accountid", customer.AccountId);
                contact.Id = customer.AccountId;
            }

            //if (string.IsNullOrEmpty(customer.CompanyName))
            //    contact.Attributes.Add("name", customer.AccountFirstName + " " + customer.AccountLastName);
            //else
            //    contact.Attributes.Add("name", customer.CompanyName);

            contact.Attributes.Add("firstname", customer.AccountFirstName);
            contact.Attributes.Add("lastname", customer.AccountLastName);

            contact.Attributes.Add("telephone1", customer.Phone);
            contact.Attributes.Add("telephone2", customer.MobilePhone);
            contact.Attributes.Add("emailaddress1", customer.Email);
            //accountEntity.Attributes.Add("donotbulkemail", !customer.AllowBulkEmail);
            //accountEntity.Attributes.Add("donotemail", !customer.AllowEmail);
            //accountEntity.Attributes.Add("donotphone", !customer.AllowPhone);
            //accountEntity.Attributes.Add("donotmail", !customer.AllowMail);     
            contact.Attributes.Add("cgi_allow_autoload", customer.AllowAutoLoad);
            contact.Attributes.Add("cgi_max_cards_autoload", customer.MaxCardsAutoLoad);
            //contact.Attributes.Add("accountcategorycode", new OptionSetValue((Int32)customer.CustomerType));
            //contact.Attributes.Add("cgi_debtcollection", customer.OrganizationCreditApproved);
            contact.Attributes.Add("cgi_socialsecuritynumber", customer.SocialSecurtiyNumber);
            contact.Attributes.Add("cgi_activated", !customer.InActive); //Inactive = false - Account Active , Inactive = true - Account Inactive

            return contact;
        }



        private Entity AddAddesstoCustomerEntity(Entity entity, Address address, bool bUpdate)
        {
            //entity.Attributes.Add("cgi_addressid", address.AddressId);

            if (bUpdate == true)
                entity.Attributes.Add("address1_addressid", new Guid(address.AddressId));
            
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
            //entity.Attributes.Add("cgi_email", address.EmailNotificationAddress);
            entity.Attributes.Add("address1_addresstypecode", new OptionSetValue((int)AddressTypeCode.Invoice));
            return entity;
        }

        /// <summary>
        /// Transforms Customer Object to Account Object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Account Object</returns>
        private Account GetAccountFromCustomer(Customer customer)
        {
            Account account = new Account
            {
                AccountId = customer.AccountId,
                //AccountNumber = customer.AccountNumber,
                //Name = customer.Name,
                Email = customer.Email,
                MainPhone = customer.Phone,
                OtherPhone = customer.MobilePhone,
                //Telephone3 = customer.Telephone3,
                //Address1_Street1 = customer.Address1_Street1,
                //Address1_PostalCode = customer.Address1_PostalCode,
                //Address1_City = customer.Address1_City,
                //Address1_County = customer.Address1_County,
                //Address1_Country = customer.Address1_Country,
                //Address2_Street1 = customer.Address2_Street1,
                //Address2_PostalCode = customer.Address2_PostalCode,
                //Address2_City = customer.Address2_City,
                //Address2_County = customer.Address2_County,
                //Address2_Country = customer.Address2_Country,
                //AllowBulkEmail = customer.AllowBulkEmail,
                //AllowEmail = customer.AllowEmail,
                //AllowMail = customer.AllowMail,
                //AllowPhone = customer.AllowPhone,
                AllowAutoLoad = customer.AllowAutoLoad,
                MaxCardsAutoLoad = customer.MaxCardsAutoLoad,
                CustomerType = new OptionSetValue((Int32)customer.CustomerType),
                OrganizationCreditApproved = customer.OrganizationCreditApproved,
                OrganizationNumber = customer.OrganizationNumber,
                OrganizationSubNumber = customer.OrganizationSubNumber,
                SocialSecurtiyNumber = customer.SocialSecurtiyNumber
            };
            return account;
        }


        /// <summary>
        /// Transform Account Object to Customer Object
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Customer Object</returns>
        private Customer GetCustomerFromAccount(Account account)
        {
            Customer customer = new Customer
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
                MaxCardsAutoLoad = account.MaxCardsAutoLoad == null ? 0 : account.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Company,
                OrganizationCreditApproved = account.OrganizationCreditApproved,
                OrganizationNumber = account.OrganizationNumber ?? String.Empty,
                OrganizationSubNumber = account.OrganizationSubNumber ?? String.Empty,
                InActive = !account.InActive,
                Deleted = account.Deleted,
            };


            return customer;
        }
        /// <summary>
        /// Transform Account Object to Customer Object
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Customer Object</returns>
        private Customer GetCustomerFromContact(Contact contact)
        {
            Customer customer = new Customer
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
                MaxCardsAutoLoad = contact.MaxCardsAutoLoad == null ? 0 : contact.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Private,
                InActive = !contact.InActive,
                Deleted = contact.Deleted,
            };


            return customer;
        }


        private Entity GetCustomerAddressEntity(Guid accountId, Address address, bool bUpdate)
        {
            Entity customerAddress = new Entity("customeraddress");
            if (bUpdate)
            {
                //entity.Attributes.Add("customerid", address.CustomerAddressId);
                customerAddress.Id = address.CustomerAddressId;
            }
            else
            {
                customerAddress.Attributes.Add("parentid", new EntityReference("account", accountId));
            }

            //customerAddress.Attributes.Add("cgi_addressid", address.AddressId);
            //customerAddress.Attributes.Add("address1_addressid", new Guid(address.AddressId));
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

            if (!string.IsNullOrEmpty(address.EmailNotificationAddress))
                customerAddress.Attributes.Add("cgi_email", address.EmailNotificationAddress);
            else
                customerAddress.Attributes.Add("cgi_email", "<missing>");

            return customerAddress;
        }

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
            xrmMgr.Service.Execute(deactivateCustomerRequest);

        }

        private ObservableCollection<CustomerAddress> GetAddressesForAccount(Guid parentAccountId)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
            queryByAttribute.AddAttributeValue("parentid", parentAccountId);
            
            queryByAttribute.ColumnSet = new ColumnSet(true);
            ObservableCollection<CustomerAddress> customerAddresses = xrmMgr.Get<CustomerAddress>(queryByAttribute);

            return customerAddresses;
        }

        #endregion

        #region [Customer]
        /// <summary>
        /// This Method is used to get the Account number for the given guid of the account record.
        /// </summary>
        /// <param name="customerId">Guid of the Account Record</param>
        /// <returns>Account Number of the Record as a String</returns>
        internal string GetAccountNumber(Guid customerId, AccountCategoryCode customerType)
        {
            string retValue = string.Empty;

            switch (customerType)
            {
                case AccountCategoryCode.Private:
                    Contact contact = xrmMgr.Get<Contact>(customerId, new string[] { "cgi_contactnumber" });
                    if (contact != null)
                        retValue = contact.ContactNumber;
                    break;
                case AccountCategoryCode.Company:
                    Account account = xrmMgr.Get<Account>(customerId, new string[] { "accountnumber" });
                    if (account != null)
                        retValue = account.AccountNumber;
                    break;
                default:
                    break;
            }


            return retValue;
        }

        /// <summary>
        /// This method will check if the email provided already exist in the system.
        /// This check will be performed against the registered email id of the customer.
        /// </summary>
        /// <param name="customerEmail">email id of the customer</param>
        /// <returns>returns true or false based on the result. if the email id already exist then it will retrun true else false</returns>
        internal CheckCustomerExistResponse CheckCustomerExist(string customerEmail)
        {
            CheckCustomerExistResponse chkCustomerExistResponse = new CheckCustomerExistResponse();

            try
            {
                Guid accountGuid = xrmHelper.GetIdByValue(customerEmail, "emailaddress1", "contact", xrmMgr);

                if (accountGuid != Guid.Empty)
                {
                    Entity contact = xrmMgr.Get("contact", accountGuid, new string[] { "cgi_contactnumber" });
                    chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                    chkCustomerExistResponse.AccountId = accountGuid;
                    //chkCustomerExistResponse.CustomerType = AccountCategoryCode.Private;
                    chkCustomerExistResponse.CustomerType = AccountCategoryCode.Private;
                    chkCustomerExistResponse.AccountNumber = contact.Attributes.Contains("cgi_contactnumber") ? contact.GetAttributeValue<string>("cgi_contactnumber") : string.Empty;
                    chkCustomerExistResponse.CustomerExists = true;

                }
                else
                {
                    accountGuid = xrmHelper.GetIdByValue(customerEmail, "emailaddress1", "account", xrmMgr);
                    if (accountGuid != Guid.Empty)
                    {
                        Entity account = xrmMgr.Get("account", accountGuid, new string[] { "accountnumber" });
                        chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                        chkCustomerExistResponse.AccountId = accountGuid;
                        //chkCustomerExistResponse.CustomerType = AccountCategoryCode.Company;
                        chkCustomerExistResponse.CustomerType = AccountCategoryCode.Company;
                        chkCustomerExistResponse.AccountNumber = account.Attributes.Contains("accountnumber") ? account.GetAttributeValue<string>("accountnumber") : string.Empty;
                        chkCustomerExistResponse.CustomerExists = true;

                    }
                    else
                    {
                        chkCustomerExistResponse.Status = ProcessingStatus.SUCCESS;
                        chkCustomerExistResponse.Message = "No Customer Exist With Email - " + customerEmail;
                    }
                }
                
                


            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                chkCustomerExistResponse.Message = ExceptionMsg;
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                chkCustomerExistResponse.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.Exception ex)
            {


                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        //log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                        //chkCustomerExistResponse._Status = Status;
                        //chkCustomerExistResponse._Message = ExceptionMsg;
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                    chkCustomerExistResponse.Message = ExceptionMsg;
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                    chkCustomerExistResponse.Message = ExceptionMsg;
                    //throw new Exception(ExceptionMsg, ex);
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
        public CreateCustomerResponse CreateCustomer(Customer customer)
        {
            CreateCustomerResponse response = new CreateCustomerResponse();
            try
            {
                log2Crm.LogMessage("Enter CreateCustomer", "CGIXrmPrortalService");

                if (customer == null)
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.Message = "Unable to Process request. Customer Object is null or not an Object";
                    return response;
                }
                Guid accountId = Guid.Empty;
                switch (customer.CustomerType)
                {
                    case AccountCategoryCode.Private:
                        //Create contact
                        Entity contactEntity = GetContactEntityFromCustomer(customer, false);
                        if (customer.Addresses != null && customer.Addresses.Length > 0)
                        {
                            contactEntity = AddAddesstoCustomerEntity(contactEntity, customer.Addresses[0], false);
                        }
                        accountId = xrmMgr.Service.Create(contactEntity);
                        log2Crm.LogMessage("Customer created", "CGIXrmPrortalService");
                        //Create Address
                        break;
                    case AccountCategoryCode.Company:
                        //Create Contact
                        //if (!string.IsNullOrEmpty(customer.AccountFirstName) && !string.IsNullOrEmpty(customer.AccountLastName))
                        //{
                        //    Guid primaryContactId = xrmMgr.Service.Create(GetPrimaryContactFromCustomer(customer, false));
                        //    customer.PrimaryContact = primaryContactId;
                        //}
                        //Create Account
                        string _rsid = String.Empty;
                        accountId = xrmMgr.Service.Create(GetAccountEntityFromCustomer(customer, false));
                        //Create Address
                        foreach (Address address in customer.Addresses)
                        {
                            if (!string.IsNullOrEmpty(address.CareOff))
                            {
                                _rsid = address.CareOff;
                            }

                            xrmMgr.Service.Create(GetCustomerAddressEntity(accountId, address, false));
                        }

                        //Update account with rsid. RSID comes from attribute CareOff from address object.
                        if (!string.IsNullOrEmpty(_rsid))
                            _updateRSIDOnAccount(accountId, _rsid);

                        break;
                    default:
                        break;
                }
                //Build Response
                if (accountId != Guid.Empty)
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.AccountNumber = GetAccountNumber(accountId, customer.CustomerType);
                    response.AccountId = accountId;
                    //response.Message = "Account Created Sucessfully";
                }

                //return entity.Attributes.ContainsKey("accountnumber") ? entity.GetAttributeValue<string>("accountnumber") : string.Empty;

            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
            catch (System.Exception ex)
            {


                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        //log2Crm.Exception(ExceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                    //zthrow new Exception(ExceptionMsg, ex);
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "CreateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = ExceptionMsg;
                }
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
        public UpdateCustomerResponse UpdateCustomer(Guid customerId, Customer customer)
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
                            //Create contact
                            Entity contactEntity = GetContactEntityFromCustomer(customer, true);
                            if (customer.Addresses != null && customer.Addresses.Length > 0)
                            {
                                contactEntity = AddAddesstoCustomerEntity(contactEntity, customer.Addresses[0], true);
                            }
                            xrmMgr.Service.Update(contactEntity);

                            break;
                        case AccountCategoryCode.Company:
                            //Get Primary Contact
                            //Entity retEntity = xrmMgr.Service.Retrieve("account", customerId, new ColumnSet(new string[] { "primarycontactid" }));
                            //Guid primaryContactId = retEntity.Attributes.Contains("primarycontactid") ? retEntity.GetAttributeValue<EntityReference>("primarycontactid").Id : Guid.Empty;

                            //if (primaryContactId != Guid.Empty)
                            //{
                            //    customer.PrimaryContact = primaryContactId;
                            //    xrmMgr.Service.Update(GetPrimaryContactFromCustomer(customer, true));

                            //}
                            //else
                            ////{
                            //    if (!string.IsNullOrEmpty(customer.AccountFirstName) && !string.IsNullOrEmpty(customer.AccountLastName))
                            //    {
                            //        primaryContactId = xrmMgr.Service.Create(GetPrimaryContactFromCustomer(customer, false));
                            //        customer.PrimaryContact = primaryContactId;
                            //    }
                            //}


                            //Update Account
                            string _rsid = String.Empty;
                            xrmMgr.Service.Update(GetAccountEntityFromCustomer(customer, true));
                            //Update Address

                            
                            ObservableCollection<CustomerAddress> crmCustomerAddress = GetAddressesForAccount(customerId);
                            if (customer.Addresses.Length != crmCustomerAddress.Count)
                            {
                                var customerAddressIds = customer.Addresses.Select(ca => ca.AddressId);
                                List<CustomerAddress> lstAddressToDelete = crmCustomerAddress.Where(ca => !customerAddressIds.Contains(ca.AddressId.ToString())).ToList<CustomerAddress>();
                                if (lstAddressToDelete != null && lstAddressToDelete.Count > 0)
                                {
                                    foreach (CustomerAddress itemToDelete in lstAddressToDelete)
                                    {
                                        xrmMgr.Service.Delete("customeraddress", itemToDelete.CustomerAddressId);
                                    }
                                }
                            }
                            foreach (Address address in customer.Addresses)
                            {

                                //Guid customerAddressId = xrmHelper.GetIdByValue(address.AddressId, "cgi_addressid", "customeraddress", xrmMgr, false);
                                //var customerAddressId = (from ca in crmCustomerAddress
                                                         //where ca.AddressId == address.AddressId
                                                         //select ca.CustomerAddressId).;

                                var matchedAddress = crmCustomerAddress.FirstOrDefault(ca => ca.AddressId.ToString() == address.AddressId);
                                
                                if (!string.IsNullOrEmpty(address.CareOff))
                                {
                                    _rsid = address.CareOff;
                                    //address.CareOff = "";
                                }

                                    //if (customerAddressId != Guid.Empty)
                                    //if (string.IsNullOrEmpty(customerAddressId))
                                    if (matchedAddress!=null)
                                    {
                                        //address.CustomerAddressId = customerAddressId;
                                        address.CustomerAddressId=matchedAddress.CustomerAddressId;
                                        xrmMgr.Service.Update(GetCustomerAddressEntity(customerId, address, true));
                                    }
                                    else
                                    {
                                        xrmMgr.Service.Create(GetCustomerAddressEntity(customerId, address, false));
                                    }
                            }
                                
                            //Update account with rsid. RSID comes from attribute CareOff from address object.
                            if (_rsid != String.Empty)
                                _updateRSIDOnAccount(customerId, _rsid);

                            break;
                        default:
                            break;
                    }
                }
                //Build Response

                response.Status = ProcessingStatus.SUCCESS;
                //response.Message = "Customer info Update Sucessfully";


                //return entity.Attributes.ContainsKey("accountnumber") ? entity.GetAttributeValue<string>("accountnumber") : string.Empty;

            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
            catch (System.Exception ex)
            {


                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        //log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = ExceptionMsg;
                }
            }
            return response;
        }
        /// <summary>
        /// Get the customer based on the customer id supplied
        /// </summary>
        /// <param name="customerId">guid of the customer record in crm</param>
        /// <returns>customer object</returns>
        internal GetCustomerResponse GetCustomer0(Guid customerId, AccountCategoryCode accountType)
        {
            GetCustomerResponse response = new GetCustomerResponse();

            try
            {
                QueryExpression queryGetCustomer = new QueryExpression("account");
                queryGetCustomer.ColumnSet = new ColumnSet(true);

                FilterExpression filterExpression = new FilterExpression(LogicalOperator.And);
                filterExpression.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                filterExpression.AddCondition(new ConditionExpression("statecode", ConditionOperator.In, 0,1));
                queryGetCustomer.Criteria.AddFilter(filterExpression);

                if (accountType.Equals(AccountCategoryCode.Company))
                {
                    queryGetCustomer.AddLink("contact", "primarycontactid", "contactid", JoinOperator.LeftOuter);
                    queryGetCustomer.LinkEntities[0].Columns.AddColumns("firstname", "lastname");
                    queryGetCustomer.LinkEntities[0].EntityAlias = "primarycontact";
                }
                ObservableCollection<Account> accounts = xrmMgr.Get<Account>(queryGetCustomer);

                if (accounts != null && accounts.Count > 0)
                {
                    Account retrievedAccount = accounts.FirstOrDefault();



                    Customer customer = GetCustomerFromAccount(retrievedAccount);
                    if (customer.CustomerType == AccountCategoryCode.Company)
                    {

                        customer.AccountFirstName = retrievedAccount.PrimaryContactFirstName;
                        customer.AccountLastName = retrievedAccount.PrimaryContactLastName;

                        QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
                        queryByAttribute.AddAttributeValue("parentid", accounts[0].AccountId);
                        queryByAttribute.ColumnSet = new ColumnSet(true);
                        ObservableCollection<CustomerAddress> customerAddresses = xrmMgr.Get<CustomerAddress>(queryByAttribute);
                        customer.Addresses = (from address in customerAddresses
                                              where address.AddressId != null
                                              select new Address()
                                              {
                                                  AddressId = address.AddressId.ToString(),
                                                  AddressType = address.AddressType != null ? (AddressTypeCode)address.AddressType.Value : AddressTypeCode.None,
                                                  CompanyName = address.CompanyName,
                                                  Street = address.Street,
                                                  PostalCode = address.PostalCode,
                                                  City = address.City,
                                                  County = address.County,
                                                  Country = address.Country,
                                                  CareOff = address.CareOff,
                                                  ContactPerson = address.ContactPerson,
                                                  ContactPhoneNumber = address.ContactPhoneNumber,
                                                  SMSNotificationNumber = address.SMSNotificationNumber
                                              }).ToArray();
                    }
                    else
                    {
                        Address address = new Address
                        {
                            AddressType = AddressTypeCode.Invoice,
                            AddressId = accounts[0].AddressId.ToString(),
                            CompanyName = accounts[0].Address1_Name,
                            CareOff = accounts[0].Address1_CareOff,
                            Street = accounts[0].Address1_Street1,
                            City = accounts[0].Address1_City,
                            PostalCode = accounts[0].Address1_PostalCode,
                            County = accounts[0].Address1_County,
                            Country = accounts[0].Address1_Country,
                            ContactPerson = accounts[0].Address1_ContactPerson,
                            ContactPhoneNumber = accounts[0].Address1_ContactPersonPhoneNumber,
                            SMSNotificationNumber = accounts[0].Address1_SMSNotificationNumber
                        };

                        customer.Addresses = new Address[] { address };
                    }
                    response.Status = ProcessingStatus.SUCCESS;
                    response.Customer = customer;
                }
                else
                {
                    response.Status = ProcessingStatus.SUCCESS;
                    response.Message = "No Matching Record Found!!!";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
            catch (System.Exception ex)
            {


                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        //log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = ExceptionMsg;
                }
            }
            return response;



        }


        /// <summary>
        /// Get the customer based on the customer id supplied
        /// </summary>
        /// <param name="customerId">guid of the customer record in crm</param>
        /// <returns>customer object</returns>
        public GetCustomerResponse GetCustomer(Guid customerId, AccountCategoryCode customerType)
        {
            GetCustomerResponse response = new GetCustomerResponse();

            try
            {
                switch (customerType)
                {
                    case AccountCategoryCode.Private:
                        QueryExpression queryGetCustomerByContact = new QueryExpression("contact");
                        queryGetCustomerByContact.ColumnSet = new ColumnSet(true);

                        FilterExpression filterExpressionForContact = new FilterExpression(LogicalOperator.And);
                        filterExpressionForContact.AddCondition(new ConditionExpression("contactid", ConditionOperator.Equal, customerId));
                        filterExpressionForContact.AddCondition(new ConditionExpression("statecode", ConditionOperator.In, 0, 1)); //Statecode 0 = Active contact
                        queryGetCustomerByContact.Criteria.AddFilter(filterExpressionForContact);

                        ObservableCollection<Contact> contacts = xrmMgr.Get<Contact>(queryGetCustomerByContact);

                        if (contacts != null && contacts.Count > 0)
                        {
                            Contact retrievedContact = contacts.FirstOrDefault();

                            Customer customer = GetCustomerFromContact(retrievedContact);

                            Address address = new Address
                            {
                                AddressType = AddressTypeCode.Invoice,
                                AddressId = retrievedContact.AddressId.ToString(),
                                CustomerAddressId = retrievedContact.AddressId,
                                CompanyName = retrievedContact.Address1_Name,
                                CareOff = retrievedContact.Address1_CareOff,
                                Street = retrievedContact.Address1_Street1,
                                City = retrievedContact.Address1_City,
                                PostalCode = retrievedContact.Address1_PostalCode,
                                County = retrievedContact.Address1_County,
                                Country = retrievedContact.Address1_Country,
                                ContactPerson = retrievedContact.Address1_ContactPerson,
                                ContactPhoneNumber = retrievedContact.Address1_ContactPersonPhoneNumber,
                                SMSNotificationNumber = retrievedContact.Address1_SMSNotificationNumber
                            };

                            customer.Addresses = new Address[] { address };
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            var customer = new Customer();
                            customer.AccountId = customerId;
                            customer.CustomerType = customerType;
                            customer.Deleted = true;
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No matching records were found or customer has been deleted.";
                        }
                        break;
                    case AccountCategoryCode.Company:
                        QueryExpression queryGetCustomerByAccount = new QueryExpression("account");
                        queryGetCustomerByAccount.ColumnSet = new ColumnSet(true);

                        FilterExpression filterExpressionForAccount = new FilterExpression(LogicalOperator.And);
                        filterExpressionForAccount.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                        filterExpressionForAccount.AddCondition(new ConditionExpression("statecode", ConditionOperator.In, 0, 1)); //Statecode 0 = active account
                        queryGetCustomerByAccount.Criteria.AddFilter(filterExpressionForAccount);

                        ObservableCollection<Account> accounts = xrmMgr.Get<Account>(queryGetCustomerByAccount);

                        if (accounts != null && accounts.Count > 0)
                        {
                            Account retrievedAccount = accounts.FirstOrDefault();

                            Customer customer = GetCustomerFromAccount(retrievedAccount);
                            customer.AccountFirstName = retrievedAccount.PrimaryContactFirstName;
                            customer.AccountLastName = retrievedAccount.PrimaryContactLastName;

                            QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
                            queryByAttribute.AddAttributeValue("parentid", retrievedAccount.AccountId);
                            queryByAttribute.ColumnSet = new ColumnSet(true);
                            ObservableCollection<CustomerAddress> customerAddresses = xrmMgr.Get<CustomerAddress>(queryByAttribute);
                            customer.Addresses = (from address in customerAddresses
                                                  where address.AddressId != null
                                                  && address.Street != null
                                                  select new Address()
                                                  {
                                                      AddressId = address.CustomerAddressId.ToString(),     //            address.AddressId.ToString(),
                                                      CustomerAddressId = address.CustomerAddressId,
                                                      AddressType = address.AddressType != null ? (AddressTypeCode)address.AddressType.Value : AddressTypeCode.None,
                                                      CompanyName = address.CompanyName,
                                                      Street = address.Street,
                                                      PostalCode = address.PostalCode,
                                                      City = address.City,
                                                      County = address.County,
                                                      Country = address.Country,
                                                      CareOff = address.CareOff,
                                                      ContactPerson = address.ContactPerson,
                                                      ContactPhoneNumber = address.ContactPhoneNumber,
                                                      SMSNotificationNumber = address.SMSNotificationNumber
                                                  }).ToArray();


                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            var customer = new Customer();
                            customer.AccountId = customerId;
                            customer.CustomerType = customerType;
                            customer.Deleted = true;
                            response.Customer = customer;
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No matching records were found or customer has been deleted.";
                        }

                        break;
                    default:
                        break;
                }


            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
            catch (System.Exception ex)
            {


                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        //log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = ExceptionMsg;
                }
            }
            return response;



        }

        private void _updateRSIDOnAccount(Guid accountid, string rsid)
        {
            try
            {
                Entity account = new Entity("account");
                account.Id = accountid;
                if (string.IsNullOrEmpty(rsid))
                    account.Attributes.Add("cgi_rsid", "");
                else
                    account.Attributes.Add("cgi_rsid", rsid);

                xrmMgr.Service.Update(account);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Deactivate the customer record associated with the guid of provided
        /// </summary>
        /// <param name="customerId">guid of the customer record to be deactivated</param>
        /// <returns>returns true or false based on the result of the deactivation request</returns>
        //internal bool DeactivateCustomer(Guid customerId)
        //{
        //    bool bDeactivate = false;
        //    try
        //    {
        //        //Guid recordId = xrmHelper.GetId(accountNumber, "accountnumber", "account", xrmMgr); ;
        //        SetStateRequest setStateRequest = new SetStateRequest()
        //        {
        //            EntityMoniker = new EntityReference
        //            {
        //                Id = customerId,
        //                LogicalName = "account",
        //            },
        //            State = new OptionSetValue(1),
        //            Status = new OptionSetValue(2)
        //        };
        //        OrganizationResponse orgResponse = xrmMgr.Service.Execute(setStateRequest);
        //        bDeactivate = true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return bDeactivate;
        //}


        #endregion



    }
}
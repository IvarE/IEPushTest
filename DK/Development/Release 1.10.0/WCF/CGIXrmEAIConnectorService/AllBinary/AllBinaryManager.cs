using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;
using CGIXrmEAIConnectorService.AllBinary.CrmClasses;
using CGIXrmEAIConnectorService.AllBinary.Models;
using CGIXrmEAIConnectorService.Shared;
using CGIXrmEAIConnectorService.Shared.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmEAIConnectorService.AllBinary
{
    public class AllBinaryManager
    {
        #region Declarations
        private readonly XrmManager _xrmMgr;
        readonly LogToCrm _log2Crm = new LogToCrm();
        #endregion

        #region Constructor
        public AllBinaryManager()
        {
            var xrmHelper = new XrmHelper();
            _xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public AllBinaryManager(Guid callerId)
        {
            var xrmHelper = new XrmHelper();
            _xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transform Account Object to Customer Object
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerType"></param>
        /// <returns>Customer Object</returns>
        public GetCustomerDetailsResponse GetCustomerDetails(Guid customerId, AccountCategoryCode customerType)
        {
            GetCustomerDetailsResponse response = new GetCustomerDetailsResponse
            {
                RequestAccountCategoryCode = customerType
            };

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
                        filterExpressionForContact.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                        queryGetCustomerByContact.Criteria.AddFilter(filterExpressionForContact);

                        ObservableCollection<Contact> contacts = _xrmMgr.Get<Contact>(queryGetCustomerByContact);

                        if (contacts != null && contacts.Count > 0)
                        {
                            Contact retrievedContact = contacts.FirstOrDefault();
                            var customer = GetCustomerFromContact(retrievedContact);

                            if (retrievedContact != null)
                            {
                                Address address = new Address
                                {
                                    AddressType = AddressTypeCode.Invoice,
                                    AddressId = retrievedContact.AddressId,
                                    CompanyName = retrievedContact.Address1_Name,
                                    CareOff = retrievedContact.Address1_CareOff,
                                    Street = retrievedContact.Address1_Street1,
                                    City = retrievedContact.Address1_City,
                                    PostalCode = retrievedContact.Address1_PostalCode,
                                    County = retrievedContact.Address1_County,
                                    Country = retrievedContact.Address1_Country,
                                    ContactPerson = retrievedContact.Address1_ContactPerson,
                                    ContactPhoneNumber = retrievedContact.Address1_ContactPersonPhoneNumber,
                                    SMSNotificationNumber = retrievedContact.Address1_SMSNotificationNumber,
                                };

                                customer.Addresses = new[] { address };
                            }

                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No Matching Record Found!!!";
                        }
                        break;
                    case AccountCategoryCode.Company:
                        QueryExpression queryGetCustomerByAccount = new QueryExpression("account")
                        {
                            ColumnSet = new ColumnSet(true)
                        };

                        FilterExpression filterExpressionForAccount = new FilterExpression(LogicalOperator.And);
                        filterExpressionForAccount.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                        filterExpressionForAccount.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                        queryGetCustomerByAccount.Criteria.AddFilter(filterExpressionForAccount);

                        ObservableCollection<Account> accounts = _xrmMgr.Get<Account>(queryGetCustomerByAccount);

                        if (accounts != null && accounts.Count > 0)
                        {
                            Account retrievedAccount = accounts.FirstOrDefault();

                            var customer = GetCustomerFromAccount(retrievedAccount);
                            if (retrievedAccount != null)
                            {
                                customer.AccountFirstName = retrievedAccount.PrimaryContactFirstName;
                                customer.AccountLastName = retrievedAccount.PrimaryContactLastName;

                                QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
                                queryByAttribute.AddAttributeValue("parentid", retrievedAccount.AccountId);
                                queryByAttribute.ColumnSet = new ColumnSet(true);
                                ObservableCollection<CustomerAddress> customerAddresses = _xrmMgr.Get<CustomerAddress>(queryByAttribute);

                                customer.Addresses = (from address in customerAddresses
                                    where address.CustomerAddressId != null
                                          && address.AddressType != null        //We don't return addresses with no address type.
                                          && address.CompanyName != null        //Mandatory field in CRM when adding new addresses.
                                    select new Address()
                                    {
                                        AddressId = address.CustomerAddressId.ToString(),
                                        AddressType = (AddressTypeCode)address.AddressType.Value,
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


                            response.Status = ProcessingStatus.SUCCESS;
                            response.Customer = customer;
                        }
                        else
                        {
                            response.Status = ProcessingStatus.SUCCESS;
                            response.Message = "No Matching Record Found!!!";
                        }

                        break;
                }

                response.IsNull = response.Customer == null;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                      "Code: " + ex.Detail.ErrorCode +
                                      "Message: " + ex.Detail.Message +
                                      "Trace: " + ex.Detail.TraceText +
                                      "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
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

                string exceptionMsg;
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                       "Code: " + fe.Detail.ErrorCode +
                                       "Message: " + fe.Detail.Message +
                                       "Trace: " + fe.Detail.TraceText +
                                       "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                        throw new Exception(exceptionMsg, fe);
                    }
                    _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    throw new Exception(exceptionMsg, ex);
                }
                exceptionMsg = "The application terminated with an error." + ex.Message;
                _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            return response;
        }
        #endregion

        #region Internal 
        internal GetCustomerIdForTravelCardResponse GetCustomerIdForTravelCard(string[] travelCard)
        {
            GetCustomerIdForTravelCardResponse response = new GetCustomerIdForTravelCardResponse();
            try
            {
                // removed add zeros to fill up for 10-digit numbers because of the use of 8-digit numbers
                //var travelCards = VerifyTravelCards(travelCard);
                var travelCards = travelCard;
                var lstEntityTraveCards = new List<Entity>();
                int count = 0;
                int batchsize = 1000;

                while (count < travelCards.Length)
                {
                    string[] tmpTravelCards;
                    if (travelCards.Length > batchsize && (travelCards.Length - count) >= batchsize)
                    {
                        tmpTravelCards = travelCards.Skip(count).Take(batchsize).ToArray();
                        count += batchsize;
                    }
                    else if (travelCards.Length > batchsize && (travelCards.Length - count) < batchsize)
                    {
                        tmpTravelCards = travelCards.Skip(count).Take((travelCards.Length - count)).ToArray();
                        count += (travelCards.Length - count);
                    }
                    else
                    {
                        tmpTravelCards = travelCards;
                        count = travelCards.Length;
                    }

                    // Run Query here
                    QueryExpression queryCustomerIdByTravelCard = new QueryExpression("cgi_travelcard")
                    {
                        ColumnSet =
                            new ColumnSet(
                                "cgi_accountid",
                                "cgi_contactid",
                                "cgi_travelcardnumber",
                                "cgi_travelcardname"),
                        Criteria = new FilterExpression()
                    };
                    queryCustomerIdByTravelCard.Criteria.AddCondition(new ConditionExpression("cgi_travelcardnumber", ConditionOperator.In, tmpTravelCards));
                    queryCustomerIdByTravelCard.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

                    EntityCollection tmpEntityCollectionTraveCard = _xrmMgr.Service.RetrieveMultiple(queryCustomerIdByTravelCard);
                    lstEntityTraveCards.AddRange(tmpEntityCollectionTraveCard.Entities);
                }

                if (lstEntityTraveCards.Count > 0)
                {
                    response.Details = new List<Detail>();
                    foreach (Entity entityTravelCard in lstEntityTraveCards)
                    {
                        response.Details.Add(new Detail()
                        {
                            CustomerId = entityTravelCard.Contains("cgi_accountid") ? (entityTravelCard.GetAttributeValue<EntityReference>("cgi_accountid")).Id : (entityTravelCard.Contains("cgi_contactid") ? (entityTravelCard.GetAttributeValue<EntityReference>("cgi_contactid")).Id : Guid.Empty),
                            CustomerType = entityTravelCard.Contains("cgi_accountid") ? AccountCategoryCode.Company : AccountCategoryCode.Private,
                            TravelCardNumber = entityTravelCard.Contains("cgi_travelcardnumber") ? entityTravelCard.GetAttributeValue<string>("cgi_travelcardnumber") : string.Empty,
                            TravelCardName = entityTravelCard.Contains("cgi_travelcardname") ? entityTravelCard.GetAttributeValue<string>("cgi_travelcardname") : string.Empty
                        });
                    }
                    response.Status = ProcessingStatus.SUCCESS;

                }
                else
                {
                    response.Status = ProcessingStatus.FAILED;
                    response.Message = "No Matching Record Found";
                }
                response.IsNull = response.Details == null || response.Details.Count == 0;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                      "Code: " + ex.Detail.ErrorCode +
                                      "Message: " + ex.Detail.Message +
                                      "Trace: " + ex.Detail.TraceText +
                                      "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
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
                string exceptionMsg;
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                       "Code: " + fe.Detail.ErrorCode +
                                       "Message: " + fe.Detail.Message +
                                       "Trace: " + fe.Detail.TraceText +
                                       "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                        throw new Exception(exceptionMsg, fe);
                    }
                    _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    throw new Exception(exceptionMsg, ex);
                }
                exceptionMsg = "The application terminated with an error." + ex.Message;
                _log2Crm.Exception(exceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = exceptionMsg;
            }
            return response;
        }
        #endregion

        #region Private 
        private Customer GetCustomerFromAccount(Account account)
        {
            Customer customer = new Customer
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber ?? "",
                CompanyName = account.Name,
                AccountFirstName = account.AccountFirstName ?? "",
                AccountLastName = account.AccountLastName ?? "",
                SocialSecurityNumber = account.SocialSecurityNumber ?? "",
                Email = account.Email ?? "",
                Phone = account.MainPhone ?? "",
                MobilePhone = account.OtherPhone ?? "",
                AllowAutoLoad = account.AllowAutoLoad,
                MaxCardsAutoLoad = account.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Company,
                OrganizationCreditApproved = account.OrganizationCreditApproved,
                OrganizationNumber = account.OrganizationNumber ?? "",
                OrganizationSubNumber = account.OrganizationSubNumber ?? "",
                Responsibility = account.Responsibility ?? "",
                Rsid = account.Rsid ?? "",
                Counterpart = account.OrganizationSubNumber ?? ""
            };

            return customer;
        }


        /// <summary>
        /// Method verifying if the array of travel card IDs has a minimum of 10 digits.
        /// In case that travel cards have an Id of less than 10 digits the method
        /// adds leading zeros on the left side of the string.
        /// </summary>
        /// <param name="cards">Array of travel card Ids</param>
        /// <returns>Verified travel card Ids</returns>
        private string[] VerifyTravelCards(string[] cards)
        {
            string[] revisedArray = new string[cards.Length];

            for (int i = 0; i < cards.Length;i++)
            {
                var chars = cards[i].ToCharArray();
                int rest = 10 - chars.Length;
                if (rest > 0)
                {
                    char[] extraChars = new char[rest];
                    for (int charsIndex = 0; charsIndex < rest; charsIndex++)
                    {
                        extraChars[charsIndex] = '0';
                    }
                    var list = extraChars.ToList();
                    list.AddRange(chars);
                    revisedArray[i] = new string(list.ToArray());
                }
                else
                    revisedArray[i] = cards[i];
            }

            return revisedArray;   
        }

        private Customer GetCustomerFromContact(Contact contact)
        {
            Customer customer = new Customer
            {
                AccountId = contact.ContactId,
                AccountNumber = contact.ContactNumber ?? "",
                CompanyName = contact.FullName ?? "",
                AccountFirstName = contact.FirstName ?? "",
                AccountLastName = contact.LastName ?? "",
                SocialSecurityNumber = contact.SocialSecurtiyNumber ?? "",
                Email = contact.Email ?? "",
                Phone = contact.MainPhone ?? "",
                MobilePhone = contact.OtherPhone ?? "",
                AllowAutoLoad = contact.AllowAutoLoad,
                MaxCardsAutoLoad = contact.MaxCardsAutoLoad,
                CustomerType = AccountCategoryCode.Private,
                InActive = !contact.InActive,

            };
            return customer;
        }
        #endregion
    }
}
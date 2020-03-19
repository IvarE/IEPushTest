using System;
using System.Linq;
using System.ServiceModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CGIXrmWin;
using CGIXrmEAIConnectorService;
using CGIXrmEAIConnectorService.AllBinary.Models;
using Microsoft.Xrm.Sdk.Query;
using CGIXrmLogger;
using Microsoft.Xrm.Sdk;

public class AllBinaryManager
{
    #region Global Variables
    private XrmManager xrmMgr;
    private XrmHelper xrmHelper;
    LogToCrm log2Crm = new LogToCrm();
    #endregion

    #region Constructor
    public AllBinaryManager()
    {
        xrmHelper = new XrmHelper();
        xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);

    }
   
    public AllBinaryManager(Guid callerId)
    {
        xrmHelper = new XrmHelper();
        xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Transform Account Object to Customer Object
    /// </summary>
    /// <param name="account"></param>
    /// <returns>Customer Object</returns>
    /// 
    public GetCustomerDetailsResponse GetCustomerDetails(Guid customerId, AccountCategoryCode customerType)
    {
        GetCustomerDetailsResponse response = new GetCustomerDetailsResponse();
        response.RequestAccountCategoryCode = customerType;

        try
        {
            switch (customerType)
            {
                case AccountCategoryCode.Private:
                    QueryExpression queryGetCustomerByContact = new QueryExpression("contact");
                    queryGetCustomerByContact.ColumnSet = new ColumnSet(true);

                    FilterExpression filterExpressionForContact = new FilterExpression(LogicalOperator.And);
                    filterExpressionForContact.AddCondition(new ConditionExpression("contactid", ConditionOperator.Equal, customerId));
                    filterExpressionForContact.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    queryGetCustomerByContact.Criteria.AddFilter(filterExpressionForContact);

                    ObservableCollection<Contact> contacts = xrmMgr.Get<Contact>(queryGetCustomerByContact);

                    if (contacts != null && contacts.Count > 0)
                    {
                        Contact retrievedContact = contacts.FirstOrDefault();
                        Customer customer = new Customer();
                        customer = GetCustomerFromContact(retrievedContact);

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

                        customer.Addresses = new Address[] { address };

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
                    QueryExpression queryGetCustomerByAccount = new QueryExpression("account");
                    queryGetCustomerByAccount.ColumnSet = new ColumnSet(true);

                    FilterExpression filterExpressionForAccount = new FilterExpression(LogicalOperator.And);
                    filterExpressionForAccount.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                    filterExpressionForAccount.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    queryGetCustomerByAccount.Criteria.AddFilter(filterExpressionForAccount);

                    ObservableCollection<Account> accounts = xrmMgr.Get<Account>(queryGetCustomerByAccount);

                    if (accounts != null && accounts.Count > 0)
                    {
                        Account retrievedAccount = accounts.FirstOrDefault();

                        Customer customer = new Customer();
                        customer = GetCustomerFromAccount(retrievedAccount);
                        customer.AccountFirstName = retrievedAccount.PrimaryContactFirstName;
                        customer.AccountLastName = retrievedAccount.PrimaryContactLastName;

                        QueryByAttribute queryByAttribute = new QueryByAttribute("customeraddress");
                        queryByAttribute.AddAttributeValue("parentid", retrievedAccount.AccountId);
                        queryByAttribute.ColumnSet = new ColumnSet(true);
                        ObservableCollection<CustomerAddress> customerAddresses = xrmMgr.Get<CustomerAddress>(queryByAttribute);

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


                        response.Status = ProcessingStatus.SUCCESS;
                        response.Customer = customer;
                    }
                    else
                    {
                        response.Status = ProcessingStatus.SUCCESS;
                        response.Message = "No Matching Record Found!!!";
                    }

                    break;
                default:
                    break;
            }

            response.IsNull = response.Customer == null;
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
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
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
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    throw new Exception(ExceptionMsg, fe);
                }
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
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
            var travelCards = VerifyTravelCards(travelCard);
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
                QueryExpression queryCustomerIdByTravelCard = new QueryExpression("cgi_travelcard");
                queryCustomerIdByTravelCard.ColumnSet = new ColumnSet(new string[] { "cgi_accountid", "cgi_contactid", "cgi_travelcardnumber", "cgi_travelcardname" });
                queryCustomerIdByTravelCard.Criteria = new FilterExpression();
                queryCustomerIdByTravelCard.Criteria.AddCondition(new ConditionExpression("cgi_travelcardnumber", ConditionOperator.In, tmpTravelCards));
                queryCustomerIdByTravelCard.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

                EntityCollection tmpEntityCollectionTraveCard = xrmMgr.Service.RetrieveMultiple(queryCustomerIdByTravelCard);
                lstEntityTraveCards.AddRange(tmpEntityCollectionTraveCard.Entities);
            }

            if (lstEntityTraveCards != null && lstEntityTraveCards.Count > 0)
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
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
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
                    log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                    throw new Exception(ExceptionMsg, fe);
                }
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                log2Crm.Exception(ExceptionMsg, "UpdateCustomer", ex, "Portal Web Service");
                response.Status = ProcessingStatus.FAILED;
                response.Message = ExceptionMsg;
            }
        }
        return response;
    }
    #endregion

    #region Private 
    private Customer GetCustomerFromAccount(Account account)
    {
        Customer customer = new Customer();
        
        customer.AccountId = account.AccountId;
        customer.AccountNumber = account.AccountNumber == null ? "" : account.AccountNumber;
        customer.CompanyName = account.Name;
        customer.AccountFirstName = account.AccountFirstName == null ? "" : account.AccountFirstName;
        customer.AccountLastName = account.AccountLastName == null ? "" : account.AccountLastName;
        customer.SocialSecurityNumber = account.SocialSecurityNumber == null ? "" : account.SocialSecurityNumber;
        customer.Email = account.Email == null ? "" : account.Email;
        customer.Phone = account.MainPhone == null ? "" : account.MainPhone;
        customer.MobilePhone = account.OtherPhone == null ? "" : account.OtherPhone;
        customer.AllowAutoLoad = account.AllowAutoLoad;
        customer.MaxCardsAutoLoad = account.MaxCardsAutoLoad;

        customer.CustomerType = AccountCategoryCode.Company;
            
        customer.OrganizationCreditApproved = account.OrganizationCreditApproved;
        customer.OrganizationNumber = account.OrganizationNumber == null ? "" : account.OrganizationNumber;
        customer.OrganizationSubNumber = account.OrganizationSubNumber == null ? "" : account.OrganizationSubNumber;
        customer.Responsibility = account.Responsibility == null ? "" : account.Responsibility;
        customer.Rsid = account.Rsid == null ? "" : account.Rsid;
        customer.Counterpart = account.OrganizationSubNumber == null ? "" : account.OrganizationSubNumber;
        
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


    private T ConvertToObject<T>(object source)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
        StringWriter strWriter = new StringWriter();

        XmlWriter xmlWriter = XmlWriter.Create(strWriter);
        xmlSerializer.Serialize(xmlWriter, source);
        string serializedXml = strWriter.ToString();

        XmlReaderSettings xmlSettings = new XmlReaderSettings();


        StringReader strReader = new StringReader(serializedXml);
        XmlReader xmlReader = XmlReader.Create(strReader, xmlSettings);
        XmlSerializer xmlDeSerializer = new XmlSerializer(typeof(T));
        return (T)xmlDeSerializer.Deserialize(xmlReader);
    }

    private Customer GetCustomerFromContact(Contact contact)
    {
        Customer customer = new Customer
        {
            AccountId = contact.ContactId,
            AccountNumber = contact.ContactNumber == null ? "" : contact.ContactNumber,
            CompanyName = contact.FullName == null ? "" : contact.FullName,
            AccountFirstName = contact.FirstName == null ? "" : contact.FirstName,
            AccountLastName = contact.LastName == null ? "" : contact.LastName,
            SocialSecurityNumber = contact.SocialSecurtiyNumber == null ? "" : contact.SocialSecurtiyNumber,
            Email = contact.Email == null ? "" : contact.Email,
            Phone = contact.MainPhone == null ? "" : contact.MainPhone,
            MobilePhone = contact.OtherPhone == null ? "" : contact.OtherPhone,
            AllowAutoLoad = contact.AllowAutoLoad,
            MaxCardsAutoLoad = contact.MaxCardsAutoLoad,
            CustomerType = AccountCategoryCode.Private,
            InActive = !contact.InActive,

        };
        return customer;
    }
    #endregion
}
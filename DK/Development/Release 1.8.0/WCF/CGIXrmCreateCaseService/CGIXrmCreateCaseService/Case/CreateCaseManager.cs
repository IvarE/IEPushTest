using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;
using CGIXrmWin;
using CRM2013.SkanetrafikenPlugins.Common;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmCreateCaseService.Case
{
    public class CreateCaseManager
    {
        #region Global Variables --------------------------------------------------------------------------------------

        readonly XrmManager _xrmManager;
        private readonly object _lockSql = new object();
        readonly Entity _incidentEntity = new Entity("incident");
        private const string CgiTravelInformation = "cgi_travelinformation";
        private const string CgiBiffTransaction = "cgi_travelcardtransaction";

        #endregion

        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateCaseManager()
        {
            _xrmManager = InitializeManager();
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private XrmManager InitializeManager()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];

                if (string.IsNullOrEmpty(crmServerUrl) ||
                    string.IsNullOrEmpty(domain) ||
                    string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(password))
                    throw new Exception();

                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);

                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SqlConnection OpenSql()
        {
            lock (_lockSql)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        private void CloseSql(IDbConnection connection)
        {
            lock (_lockSql)
            {
                connection?.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLink"></param>
        /// <param name="incidentid"></param>
        private void CreateFileLink(FileLink fileLink, Guid incidentid)
        {
            if (string.IsNullOrEmpty(fileLink.Url)) return;

            var entity = new Entity("cgi_filelink")
            {
                Attributes =
                {
                    ["cgi_incidentid"] = new EntityReference("incident", incidentid),
                    ["cgi_url"] = fileLink.Url.Replace(@"\", @"/")
                }
            };

            _xrmManager.Create(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountOrContact"></param>
        /// <returns></returns>
        private bool TryIdentifyAccountOrContactFromAutoRg(AutoRgCaseRequest request,
            out Account accountOrContact,
            out string errorMessage)
        {
            accountOrContact = null;
            Guid customerIdGuid;
            bool isContactOrAccountInCrm;
            Account account = null;
            Account contact = null;
            errorMessage = string.Empty;

            if (request.IsOrganisation)
            {
                if (Guid.TryParse(request.CustomerId, out customerIdGuid) == false)
                {
                    return false;
                }

                account = GetAccountFromId(customerIdGuid, (CustomerType)request.CustomerType);
                if (account == null)
                {
                    errorMessage = $"Kunden med id {request.CustomerId} hittas inte i databasen!";
                    isContactOrAccountInCrm = false;
                }
                else {
                    isContactOrAccountInCrm = true;
                }
            }
            else if (request.IsOrganisation == false && request.IsLoggedIn)
            {
                if (Guid.TryParse(request.CustomerId, out customerIdGuid) == false)
                {
                    return false;
                }

                contact = GetAccountFromId(customerIdGuid, (CustomerType)request.CustomerType);
                if (contact == null)
                {
                    errorMessage = $"Kunden med id {request.CustomerId} hittas inte i databasen!";
                    isContactOrAccountInCrm = false;
                }
                else {
                    isContactOrAccountInCrm = true;
                }
            }
            else
            {
                isContactOrAccountInCrm =
                    TryGetAccountOrContactByEMail(request.EmailAddress, (CustomerType)request.CustomerType, out contact)
                    || TryGetContactBySocialSecurityNumber(request.CustomerSocialSecurityNumber, out contact)
                    || TryGetContactByNameAndAddress(request.CustomerFirstName, request.CustomerLastName, request.CustomerAddress1Line2, request.CustomerAddress1Postalcode, request.CustomerAddress1City, out contact);
            }
            accountOrContact = contact ?? account;
            return isContactOrAccountInCrm;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool TryGetContactBySocialSecurityNumber(string socialSecurityNumber, out Account contact)
        {
            contact = null;

            if (string.IsNullOrEmpty(socialSecurityNumber)) return false;
            // TODO regex check could be here
            var sqlCon = OpenSql();
            using (var command = new SqlCommand("sp_GetContactFromSocialSecurityNumber", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SocialSecurityNumber",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = socialSecurityNumber
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                contact = accounts.Accounts[0];
                contact.RecordCount = accounts.Accounts.Count;

                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Guid CreateContact(AutoRgCaseRequest request)
        {
            // Guard rule
            if (request.CustomerType != 0) // 0   Privatkund
            {
                return Guid.Empty;
                //throw new Exception("Trying to create a private customer (1) with organisation customer request data in CreateContact method. This is not allowed.");
            }

            //Create private customer (contact) from firstname, lastname and emailaddress
            var contactEntity = new Entity("contact")
            {
                Attributes = new AttributeCollection()
            };

            if (!string.IsNullOrEmpty(request.CustomerFirstName))
                contactEntity.Attributes.Add("firstname", request.CustomerFirstName);

            if (!string.IsNullOrEmpty(request.CustomerLastName))
                contactEntity.Attributes.Add("lastname", request.CustomerLastName);

            if (!string.IsNullOrEmpty(request.EmailAddress))
                contactEntity.Attributes.Add("emailaddress1", request.EmailAddress);

            if (!string.IsNullOrEmpty(request.CustomerTelephonenumber))
                contactEntity.Attributes.Add("telephone2", request.CustomerTelephonenumber);

            if (!string.IsNullOrEmpty(request.CustomerAddress1Line2) && !string.IsNullOrEmpty(request.CustomerAddress1Postalcode))
            {
                contactEntity.Attributes.Add("address1_line1", request.CustomerAddress1Line1);
                contactEntity.Attributes.Add("address1_line2", request.CustomerAddress1Line2);
                contactEntity.Attributes.Add("address1_postalcode", request.CustomerAddress1Postalcode);
                contactEntity.Attributes.Add("address1_city", request.CustomerAddress1City);
                contactEntity.Attributes.Add("address1_country", request.CustomerAddress1Country);
            }

            if (!string.IsNullOrEmpty(request.CustomerSocialSecurityNumber))
            {
                //kontrollera att det inte finns någon annan kontakt med det angivna personnumret
                Account contact;
                if (!TryGetContactBySocialSecurityNumber(request.CustomerSocialSecurityNumber, out contact))
                contactEntity.Attributes.Add("cgi_socialsecuritynumber", request.CustomerSocialSecurityNumber);
            }


            return _xrmManager.Create(contactEntity);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountId"></param>
        private void UpdateContactFieldsIfOriginalFieldsAreNull(AutoRgCaseRequest request, Account customer)
        {
            if (request.CustomerType != 0) // 0   Privatkund
            {
                return;
            }

            Entity origContact = new Entity("contact");
            origContact.Id = customer.AccountId;

            // uppdatera för och efternamn om efternamn är "Ange namn"
            if (!string.IsNullOrEmpty(customer?.LastName) && customer.LastName.ToUpperInvariant() == "ANGE NAMN")
            {
                if (!string.IsNullOrEmpty(request.CustomerFirstName) && !string.IsNullOrEmpty(request.CustomerLastName))
                {
                    origContact.Attributes.Add("firstname", request.CustomerFirstName);
                    origContact.Attributes.Add("lastname", request.CustomerLastName);
                }
            }
            else
            {
                // uppdatera bara fält som tidigare saknats i systemet
                if (string.IsNullOrEmpty(customer?.FirstName) && !string.IsNullOrEmpty(request.CustomerFirstName))
                    origContact.Attributes.Add("firstname", request.CustomerFirstName);

                if (string.IsNullOrEmpty(customer?.LastName) && !string.IsNullOrEmpty(request.CustomerLastName))
                    origContact.Attributes.Add("lastname", request.CustomerLastName);
            }

            // uppdatera bara fält som tidigare saknats i systemet
            if (string.IsNullOrEmpty(customer?.EmailAddress) && !string.IsNullOrEmpty(request.EmailAddress))
                origContact.Attributes.Add("emailaddress1", request.EmailAddress);

            if (string.IsNullOrEmpty(customer?.Telephone2) && !string.IsNullOrEmpty(request.CustomerTelephonenumber))
                origContact.Attributes.Add("telephone2", request.CustomerTelephonenumber);

            if (string.IsNullOrEmpty(customer?.SocialSecurityNumber) && !string.IsNullOrEmpty(request.CustomerSocialSecurityNumber))
            {
                //kontrollera att det inte finns någon annan kontakt med det angivna personnumret
                Account contact;
                if (!TryGetContactBySocialSecurityNumber(request.CustomerSocialSecurityNumber, out contact))
                origContact.Attributes.Add("cgi_socialsecuritynumber", request.CustomerSocialSecurityNumber);
            }


            if (string.IsNullOrEmpty(customer?.Address1_line2) && string.IsNullOrEmpty(customer?.Address1_city))
            {
                origContact.Attributes.Add("address1_line1", request.CustomerAddress1Line1);
                origContact.Attributes.Add("address1_line2", request.CustomerAddress1Line2);
                origContact.Attributes.Add("address1_postalcode", request.CustomerAddress1Postalcode);
                origContact.Attributes.Add("address1_city", request.CustomerAddress1City);
                origContact.Attributes.Add("address1_country", request.CustomerAddress1Country);
            }



            _xrmManager.Update(origContact);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="addressLine2"></param>
        /// <param name="addressPostalCode"></param>
        /// <param name="addressCity"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private bool TryGetContactByNameAndAddress(
            string firstName,
            string lastName,
            string addressLine2,
            string addressPostalCode,
            string addressCity,
            out Account account)
        {
            account = null;

            //Avoid searching for empty valuees
            if (string.IsNullOrEmpty(firstName)) return false;
            if (string.IsNullOrEmpty(lastName)) return false;
            if (string.IsNullOrEmpty(addressLine2)) return false;
            if (string.IsNullOrEmpty(addressPostalCode)) return false;
            if (string.IsNullOrEmpty(addressCity)) return false;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetContactFromNameAndAddress", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstName",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = firstName
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LastName",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = lastName
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Address_Line2",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = addressLine2
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Address_PostalCode",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = addressPostalCode
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Address_City",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = addressCity
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                account = accounts.Accounts[0];
                account.RecordCount = accounts.Accounts.Count;

            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromEmail(string emailAddress, CustomerType customerType)
        {
            Account returnValue;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@emailaddress", SqlDbType = SqlDbType.VarChar, SqlValue = emailAddress });
                command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = SqlDbType.Int, SqlValue = (int)customerType });

                var reader = command.ExecuteXmlReader();
                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }
                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                returnValue = accounts.Accounts[0];
                returnValue.RecordCount = accounts.Accounts.Count;
            }

            return returnValue;
        }


        /// <summary>
        /// Deletes two possible entities:
        /// * TravelInformation
        /// * TravelCardTransaction ( BIFF )
        /// if they are attached to a Incident.
        /// </summary>
        /// <param name="entityLogicalName"></param>
        /// <param name="caseId"></param>
        /// <param name="guardErrorMessage"></param>
        private void DeleteCurrentEntitiesAttachedToCase(string entityLogicalName, Guid caseId, string guardErrorMessage)
        {
            if (caseId == Guid.Empty)
            {
                throw new Exception("Parameter caseId is empty guid.");
            }
            var columns = new string[] { };

            var attributes = new[]
            {
                "cgi_caseid"
            };

            object[] values =
            {
                caseId.ToString()
            };

            QueryBase query = CreateQueryAttribute(entityLogicalName, columns, attributes, values);
            var entityResults = _xrmManager.Service.RetrieveMultiple(query);

            if (entityResults?.Entities == null || entityResults.Entities.Count <= 0) return;

            if (entityResults.Entities.Count > 50)
            {
                throw new Exception(guardErrorMessage);
            }

            foreach (var entity in entityResults.Entities)
            {
                _xrmManager.Service.Delete(entityLogicalName, entity.Id);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromId(Guid customerid, CustomerType customerType)
        {
            Account returnValue;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@accountid",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    SqlValue = customerid
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = (int)customerType
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                returnValue = accounts.Accounts[0];
                returnValue.RecordCount = accounts.Accounts.Count;
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromIdRgol(Guid customerid, CustomerType customerType) //TODO: This private method is never used? Delete (Last check 2016-03-13)
        {
            Account returnValue = null;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@accountid",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    SqlValue = customerid
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = (int)customerType
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }
                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                returnValue = accounts.Accounts[0];
                returnValue.RecordCount = accounts.Accounts.Count;
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="customerType"></param>
        /// <param name="contactOrAccount"></param>
        /// <returns></returns>
        private bool TryGetAccountOrContactByEMail(string emailAddress, CustomerType customerType, out Account contactOrAccount)
        {
            contactOrAccount = null;
            if (string.IsNullOrEmpty(emailAddress)) return false;

            // TODO could do a regex check here

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@emailaddress",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = emailAddress
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = (int)customerType
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                contactOrAccount = accounts.Accounts[0];
                contactOrAccount.RecordCount = accounts.Accounts.Count;

                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="emailcount"></param>
        /// <returns></returns>
        private Account GetAccountFromEmailRgol(AutoRgCaseRequest request, out string emailcount) //TODO: This private method is never used? Delete (Last check 2016-03-13)
        {
            Account returnValue;
            emailcount = "0";

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@emailaddress",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = request.EmailAddress
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = request.CustomerType
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                returnValue = accounts.Accounts[0];
                emailcount = accounts.Accounts.Count.ToString();
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Setting GetDefaultCustomer()
        {
            Setting returnValue = null;

            var sqlCon = OpenSql();
            using (var command = new SqlCommand("sp_GetDefaultCustomer", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteXmlReader();

                SettingSqlList settings;
                {
                    var ser = new XmlSerializer(typeof(SettingSqlList));
                    settings = ser.Deserialize(reader) as SettingSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (settings?.Settings != null && settings.Settings.Any())
                {
                    returnValue = settings.Settings[0];
                }
            }

            return returnValue;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="incidentid"></param>
        /// <returns></returns>
        private Incident GetIncidentFromId(Guid incidentid)
        {
            Incident returnValue = null;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetIncidentFromIncidentId", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@incidentid",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    SqlValue = incidentid
                });

                var reader = command.ExecuteXmlReader();
                IncidentSqlList incidents;
                {
                    var ser = new XmlSerializer(typeof(IncidentSqlList));
                    incidents = ser.Deserialize(reader) as IncidentSqlList;
                }
                reader.Close();
                CloseSql(sqlCon);

                if (incidents?.Incidents != null && incidents.Incidents.Any())
                {
                    returnValue = incidents.Incidents[0];
                }
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundType"></param>
        /// <returns></returns>
        private RgolSetting GetRgolSettingFromRefundType(int refundType)
        {
            RgolSetting returnValue = null;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetRGOLSetting", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@rgolsetting",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = refundType,
                });

                var reader = command.ExecuteXmlReader();

                RgolSettingSqlList rgolsettings;
                {
                    var ser = new XmlSerializer(typeof(RgolSettingSqlList));
                    rgolsettings = ser.Deserialize(reader) as RgolSettingSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (rgolsettings?.RgolSettings != null && rgolsettings.RgolSettings.Any())
                {
                    returnValue = rgolsettings.RgolSettings[0];
                }
                else {
                    throw new Exception($"RGOL setting with refundType number {refundType} do not exist.");
                }
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardnumber"></param>
        /// <returns></returns>
        private string GetTravelCardIdFromCardNumber(string cardnumber) //TODO: This private method is never used? Delete (Last check 2016-03-13)
        {
            string returnValue = null;
            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetTravelCard", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@cardnumber",
                    SqlDbType = SqlDbType.VarChar,
                    SqlValue = cardnumber
                });

                var reader = command.ExecuteXmlReader();

                CardSqlList cards;
                {
                    var ser = new XmlSerializer(typeof(CardSqlList));
                    cards = ser.Deserialize(reader) as CardSqlList;
                }

                reader.Close();
                CloseSql(sqlCon);

                if (cards?.Cards != null && cards.Cards.Any())
                {
                    returnValue = cards.Cards[0].TravelCardId;
                }
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Guid CreateIncident(CreateCaseRequest request, Account customer)
        {
            _incidentEntity.Attributes = new AttributeCollection
            {
                {"caseorigincode", new OptionSetValue(3)}
            };


            if (!string.IsNullOrEmpty(request.CustomersCategory))
                _incidentEntity.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                _incidentEntity.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            var sendtoqueue = GetSendToQueue(request.CustomersCategory, request.CustomersSubcategory);
            _incidentEntity.Attributes.Add("cgi_sendtoqueue", new OptionSetValue(sendtoqueue));

            var titleText = string.Empty;
            var descriptionText = string.Empty;

            if (!string.IsNullOrEmpty(request.Title))
            {
                titleText = request.Title;
            }
            if (sendtoqueue == 285050008)
            {
                titleText = "Bestridan kontrollavgift";
            }

            _incidentEntity.Attributes.Add("title", titleText);

            if (!string.IsNullOrEmpty(request.Description))
                descriptionText = $"{request.Description}{Environment.NewLine}";

            if (customer.AccountName == "CGI_DEFAULT")
            {
                _incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            }
            else
            {
                if (request.CustomerType == CustomerType.Organisation)
                {
                    _incidentEntity.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    _incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if (request.CustomerType == CustomerType.Private)
                {
                    _incidentEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    _incidentEntity.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                _incidentEntity.Attributes.Add("cgi_invoiceno", request.InvoiceNumber);
                descriptionText += $"{Environment.NewLine}Fakturanummer: {request.InvoiceNumber}";
            }

            if (!string.IsNullOrEmpty(request.ControlFeeNumber))
            {
                _incidentEntity.Attributes.Add("cgi_controlfeeno", request.ControlFeeNumber);
                descriptionText += $"{Environment.NewLine}Kontrollavgiftsnummer: {request.ControlFeeNumber}";
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                _incidentEntity.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);
            }

            if (!string.IsNullOrEmpty(request.WayOfTravel))
                _incidentEntity.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);

            if (!string.IsNullOrEmpty(request.Line))
            {
                _incidentEntity.Attributes.Add("cgi_line", request.Line);
                descriptionText += $"{Environment.NewLine}Linje: {request.Line}";
            }

            if (!string.IsNullOrEmpty(request.Train))
            {
                _incidentEntity.Attributes.Add("cgi_train", request.Train);
                descriptionText += $"{Environment.NewLine}Linje: {request.Train}";
            }

            if (!string.IsNullOrEmpty(request.County))
            {
                _incidentEntity.Attributes.Add("cgi_county", request.County);
                descriptionText += $"{Environment.NewLine}Kommun: {request.County}";
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                _incidentEntity.Attributes.Add("cgi_city", request.City);
                descriptionText += $"{Environment.NewLine}Stad: {request.City}";
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
                _incidentEntity.Attributes.Add("cgi_emailaddress", request.EmailAddress);

            _incidentEntity.Attributes.Add("cgi_contactcustomer", request.ContactCustomer);

            if (customer.RecordCount != null)
                _incidentEntity.Attributes.Add("cgi_emailcount", customer.RecordCount.ToString());

            if (request.ActionDate != null)
            {
                _incidentEntity.Attributes.Add("cgi_actiondate", request.ActionDate.Value);
            }

            _incidentEntity.Attributes.Add("description", descriptionText);

            var g = _xrmManager.Create(_incidentEntity);
            return g;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Guid CreateIncidentRgol(AutoRgCaseRequest request, Account customer)
        {
            var incident = new Entity("incident")
            {
                Attributes = new AttributeCollection
                {
                    {"caseorigincode", new OptionSetValue(285050007)},//resegaranti online
                    {"casetypecode", new OptionSetValue(285050003)},//travel warranty
                    {"cgi_contactcustomer", false},
                }
            };

            if (!string.IsNullOrEmpty(request.CustomersCategory))
                incident.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                incident.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            if (!string.IsNullOrEmpty(request.Title))
                incident.Attributes.Add("title", request.Title);

            incident.Attributes.Add("description",
                !string.IsNullOrEmpty(request.Description) ? request.Description : "Ej angivet");

            if (customer.AccountName == "CGI_DEFAULT")
            {
                incident.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            }
            else
            {
                if ((CustomerType)request.CustomerType == CustomerType.Organisation)
                {
                    incident.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    incident.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if ((CustomerType)request.CustomerType == CustomerType.Private)
                {
                    incident.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    incident.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                incident.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);
            }                            

            if (!string.IsNullOrEmpty(request.WayOfTravel))
            {
                incident.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);
            }

            if (!string.IsNullOrEmpty(request.Line))
            {
                incident.Attributes.Add("cgi_line", request.Line);
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                incident.Attributes.Add("cgi_emailaddress", request.EmailAddress);
            }

            if (!string.IsNullOrEmpty(request.DeliveryEmailAddress))
            {
                incident.Attributes.Add("cgi_rgol_delivery_email", request.DeliveryEmailAddress);
            }
            
            if (!string.IsNullOrEmpty(request.MobileNo))
            {
                incident.Attributes.Add("cgi_customer_telephonenumber_mobile", request.MobileNo);
                incident.Attributes.Add("cgi_bombmobilenumber", request.MobileNo);
                incident.Attributes.Add("cgi_rgol_telephonenumber", request.MobileNo);
            }

            if (!string.IsNullOrEmpty(request.RGOLIssueID))
            {
                incident.Attributes.Add("cgi_rgolissueid", request.RGOLIssueID);
            }

            if (request.DepartureDateTime != null)
            {
                incident.Attributes.Add("cgi_departuredatetime", request.DepartureDateTime);
            }

            if (request.DepartureDateTime != null)
            {
                incident.Attributes.Add("cgi_actiondate", request.DepartureDateTime);
            }

            if (customer.RecordCount != null)
            {
                incident.Attributes.Add("cgi_emailcount", customer.RecordCount.ToString());
            }

            if (string.IsNullOrEmpty(request.TicketType1) == false)
            {
                incident.Attributes.Add("cgi_tickettype_1", request.TicketType1);
            }

            if (string.IsNullOrEmpty(request.TicketType2) == false)
            {
                incident.Attributes.Add("cgi_tickettype_2", request.TicketType2);
            }

            if (string.IsNullOrEmpty(request.TicketNumber1) == false)
            {
                incident.Attributes.Add("cgi_ticketnumber1", request.TicketNumber1);
            }

            if (string.IsNullOrEmpty(request.TicketNumber2) == false)
            {
                incident.Attributes.Add("cgi_ticketnumber2", request.TicketNumber2);
            }

            if (string.IsNullOrEmpty(request.MileageFrom) == false)
            {
                incident.Attributes.Add("cgi_milagefrom", request.MileageFrom);
            }

            if (string.IsNullOrEmpty(request.MileageTo) == false)
            {
                incident.Attributes.Add("cgi_milageto", request.MileageTo);
            }

            if (string.IsNullOrEmpty(request.MileageKilometers) == false)
            {
                incident.Attributes.Add("cgi_milagekilometers", request.MileageKilometers);
            }

            if (string.IsNullOrEmpty(request.MileageLicencePlateNumber) == false)
            {
                incident.Attributes.Add("cgi_milagelicenseplatenumber", request.MileageLicencePlateNumber);
            }

            if (string.IsNullOrEmpty(request.TaxiFrom) == false)
            {
                incident.Attributes.Add("cgi_taxifrom", request.TaxiFrom);
            }

            if (string.IsNullOrEmpty(request.TaxiTo) == false)
            {
                incident.Attributes.Add("cgi_taxito", request.TaxiTo);
            }

            if (string.IsNullOrEmpty(request.Iban) == false)
            {
                incident.Attributes.Add("cgi_iban", request.Iban);
            }

            if (string.IsNullOrEmpty(request.Bic) == false)
            {
                incident.Attributes.Add("cgi_bic", request.Bic);
            }

            incident.Attributes.Add("cgi_description2",
                !string.IsNullOrEmpty(request.Description) ? request.Description : "Ej angivet");

            incident.Attributes.Add("cgi_taxiclaimedamount", new Money(request.TaxiClaimedAmount));

            incident.Attributes.Add("ownerid", GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultowner"));

            if (string.IsNullOrEmpty(request.Address_Line1) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_line1", request.Address_Line1);
            }

            if (string.IsNullOrEmpty(request.Address_Line2) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_line2", request.Address_Line2);
            }

            if (string.IsNullOrEmpty(request.Address_PostalCode) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_postalcode", request.Address_PostalCode);
            }

            if (string.IsNullOrEmpty(request.Address_City) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_city", request.Address_City);
            }

            if (string.IsNullOrEmpty(request.Address_Country) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_country", request.Address_Country);
            }

            if (string.IsNullOrEmpty(request.FirstName) == false && string.IsNullOrEmpty(request.LastName) == false)
            {
                incident.Attributes.Add("cgi_rgol_fullname", $"{request.FirstName} {request.LastName}");
            }

            if (string.IsNullOrEmpty(request.CustomerSocialSecurityNumber) == false)
            {
                incident.Attributes.Add("cgi_soc_sec_number", request.CustomerSocialSecurityNumber);
            }

            if (string.IsNullOrEmpty(request.SocialSecurityNumber) == false)
            {
                incident.Attributes.Add("cgi_rgol_socialsecuritynumber", request.SocialSecurityNumber);
            }

            if (!string.IsNullOrEmpty(request.CustomerTelephonenumber))
            {
                incident.Attributes.Add("cgi_telephonenumber", request.CustomerTelephonenumber);
            }


            incident.Attributes.Add("cgi_notravelinfo", true);

            var incidentId = _xrmManager.Create(incident);

            //add filelinks from collection
            if (request.FileLinks != null && request.FileLinks.Any())
            {
                foreach (var fileLink in request.FileLinks)
                {
                    CreateFileLink(fileLink, incidentId);
                }
            }

            return incidentId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundAmount"></param>
        /// <param name="rgolsetting"></param>
        /// <param name="incident"></param>
        /// <param name="currencyid"></param>
        /// <param name="internalMessage"></param>
        /// <param name="customerMessage"></param>
        /// <returns></returns>
        private Guid CreateDecisionRgol(
            decimal refundAmount,
            RgolSetting rgolsetting,
            Incident incident,
            string currencyid,
            string internalMessage,
            string customerMessage)
        {
            var refundEntity = new Entity("cgi_refund")
            {
                Attributes = new AttributeCollection()
            };

            if (!string.IsNullOrEmpty(rgolsetting.RefundTypeId))
                refundEntity.Attributes.Add("cgi_refundtypeid", new EntityReference("cgi_refundtype", new Guid(rgolsetting.RefundTypeId)));

            if (!string.IsNullOrEmpty(rgolsetting.ReImBursementFormId))
                refundEntity.Attributes.Add("cgi_reimbursementformid", new EntityReference("cgi_reimbursementform", new Guid(rgolsetting.ReImBursementFormId)));

            if (!string.IsNullOrEmpty(incident.TravelCardNo))
                refundEntity.Attributes.Add("cgi_travelcard_number", incident.TravelCardNo);

            if (!string.IsNullOrEmpty(incident.ContactId))
                refundEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(incident.ContactId)));

            if (!string.IsNullOrEmpty(incident.TicketNumber))
                refundEntity.Attributes.Add("cgi_refundnumber", incident.TicketNumber);

            if (!string.IsNullOrEmpty(internalMessage))
                refundEntity.Attributes.Add("cgi_comments", internalMessage);

            if (!string.IsNullOrEmpty(customerMessage))
                refundEntity.Attributes.Add("cgi_customermessage", customerMessage);

            refundEntity.Attributes.Add("cgi_isautogenerated", true);

            refundEntity.Attributes.Add("cgi_caseid", new EntityReference("incident", incident.IncidentId));

            if (!string.IsNullOrEmpty(incident.DeliveryTelephoneNumber))
                refundEntity.Attributes.Add("cgi_mobilenumber", incident.DeliveryTelephoneNumber);

            if (!string.IsNullOrEmpty(incident.DeliveryEmailAddress))
                refundEntity.Attributes.Add("cgi_email", incident.DeliveryEmailAddress);

            if (refundAmount > 0)
            {
                var money = new Money
                {
                    Value = refundAmount
                };

                refundEntity.Attributes.Add("cgi_amount", money);
            }

            if (!string.IsNullOrEmpty(currencyid))
                refundEntity.Attributes.Add("transactioncurrencyid", new EntityReference("transactioncurrency", new Guid(currencyid)));

            refundEntity.Attributes.Add("cgi_attestation", new OptionSetValue(285050004));

            refundEntity.Attributes.Add("ownerid", GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultowner"));

            refundEntity.Attributes.Add("cgi_last_valid", DateTime.Now.AddMonths(6));

            var returnValue = _xrmManager.Create(refundEntity);

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="incidentid"></param>
        private void CreateAnnotation(Document document, Guid incidentid)
        {
            var encodedData = Convert.ToBase64String(document.DocumentBody);

            var annotation = new Entity("annotation")
            {
                Attributes =
                {
                    ["objectid"] = new EntityReference("incident", incidentid),
                    ["objecttypecode"] = "incident",
                    ["subject"] = document.Subject,
                    ["documentbody"] = encodedData,
                    ["mimetype"] = @"text/plain",
                    ["notetext"] = document.NoteText,
                    ["filename"] = document.FileName
                }
            };

            _xrmManager.Create(annotation);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="mobilePhoneNumber"></param>
        /// <returns></returns>
        private Guid CreateCustomer(string firstName, string lastName, string emailAddress, string mobilePhoneNumber)
        {
            var customerEntity = new Entity("contact")
            {
                Attributes = new AttributeCollection()
            };

            if (!string.IsNullOrEmpty(firstName))
                customerEntity.Attributes.Add("firstname", firstName);

            if (!string.IsNullOrEmpty(lastName))
                customerEntity.Attributes.Add("lastname", lastName);

            if (!string.IsNullOrEmpty(emailAddress))
                customerEntity.Attributes.Add("emailaddress1", emailAddress);

            //MaxP Ändrat från fältet telephone3 till telephone2
            if (!string.IsNullOrEmpty(mobilePhoneNumber))
                customerEntity.Attributes.Add("telephone2", mobilePhoneNumber);

            var g = _xrmManager.Create(customerEntity);
            return g;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="customercategory"></param>
        /// <param name="custumersubcategory"></param>
        /// <returns></returns>
        private static int GetSendToQueue(string customercategory, string custumersubcategory)
        {
            // 285,050,006  Kundtjänst 1 Linjen
            var returnValue = 285050000;

            if (customercategory == "Synpunkter trafikinformation" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Tankar och önskemål om ny trafik" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Skador och vandalisering" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Regionbuss")
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Stadsbuss")
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Tåg")
                returnValue = 285050000;
            else if (customercategory == "Övrigt" && (string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006; // 285,050,006  Kundtjänst 1 Linjen
            else if (customercategory == "Överklaga kontroll avgift" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050008;
            else if (customercategory == "Överklaga kontrollavgift" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))//Same as previous with alternative spelling on customercategory
                returnValue = 285050008;
            else if (customercategory == "JoJo kort och priser" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050004;
            else if (customercategory == "Min resa" && custumersubcategory == "Närtrafik")
                returnValue = 285050001;
            else if (customercategory == "Min resa" && custumersubcategory == "Färdtjänst/sjukresa")
                returnValue = 285050001;
            else if (customercategory == "Boka gruppresa" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006;
            else if (customercategory == "Boka ledsagning" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006;
            else if (customercategory == "Kundtjänst Serviceresor" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050001;
            else if ((customercategory == "" || string.IsNullOrEmpty(customercategory)) && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private BaseCurrency GetCurrency()
        {
            BaseCurrency returnValue = null;

            SqlConnection sqlCon = OpenSql();
            using (var command = new SqlCommand("sp_GetBaseCurrency", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteXmlReader();
                BaseCurrencySqlList currencies;
                {
                    var ser = new XmlSerializer(typeof(BaseCurrencySqlList));
                    currencies = ser.Deserialize(reader) as BaseCurrencySqlList;
                }
                reader.Close();
                CloseSql(sqlCon);

                if (currencies?.BaseCurrencies != null && currencies.BaseCurrencies.Any())
                {
                    returnValue = currencies.BaseCurrencies[0];
                }
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void CreateLogFile(CreateCaseRequest request)
        {
            if (request == null) return;
            var sw = new StreamWriter("C:\\Temp\\createcase.log", true);
            sw.WriteLine("======================================================================================================================");
            sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(request.CustomersCategory))
                sw.WriteLine("CustomersCategory : " + request.CustomersCategory);
            else
                sw.WriteLine("CustomersCategory is missing");

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                sw.WriteLine("CustomersSubcategory : " + request.CustomersSubcategory);
            else
                sw.WriteLine("CustomersSubcategory is missing");

            if (!string.IsNullOrEmpty(request.Title))
                sw.WriteLine("Title : " + request.Title);
            else
                sw.WriteLine("Title is missing");

            if (!string.IsNullOrEmpty(request.Description))
                sw.WriteLine("Description : " + request.Description);
            else
                sw.WriteLine("Description is missing");

            if (request.Customer != null && request.Customer != Guid.Empty)
                sw.WriteLine("Customer : " + request.Customer);
            else
                sw.WriteLine("Customer is missing");

            switch (request.CustomerType)
            {
                case CustomerType.Private:
                    sw.WriteLine("Customer : Private");
                    break;
                case CustomerType.Organisation:
                    sw.WriteLine("Customer : Organisation");
                    break;
                default:
                    sw.WriteLine("Customer is missing");
                    break;
            }

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
                sw.WriteLine("InvoiceNumber : " + request.InvoiceNumber);
            else
                sw.WriteLine("InvoiceNumber is missing");

            if (!string.IsNullOrEmpty(request.ControlFeeNumber))
                sw.WriteLine("ControlFeeNumber : " + request.ControlFeeNumber);
            else
                sw.WriteLine("ControlFeeNumber is missing");

            if (!string.IsNullOrEmpty(request.CardNumber))
                sw.WriteLine("CardNumber : " + request.CardNumber);
            else
                sw.WriteLine("CardNumber is missing");

            if (!string.IsNullOrEmpty(request.WayOfTravel))
                sw.WriteLine("WayOfTravel : " + request.WayOfTravel);
            else
                sw.WriteLine("WayOfTravel is missing");

            if (!string.IsNullOrEmpty(request.Line))
                sw.WriteLine("Line : " + request.Line);
            else
                sw.WriteLine("Line is missing");

            if (!string.IsNullOrEmpty(request.City))
                sw.WriteLine("City : " + request.City);
            else
                sw.WriteLine("City is missing");

            if (!string.IsNullOrEmpty(request.Train))
                sw.WriteLine("Train : " + request.Train);
            else
                sw.WriteLine("Train is missing");

            if (!string.IsNullOrEmpty(request.County))
                sw.WriteLine("County : " + request.County);
            else
                sw.WriteLine("County is missing");

            if (!string.IsNullOrEmpty(request.FirstName))
                sw.WriteLine("firstName : " + request.FirstName);
            else
                sw.WriteLine("firstName is missing");

            if (!string.IsNullOrEmpty(request.LastName))
                sw.WriteLine("lastName : " + request.LastName);
            else
                sw.WriteLine("lastName is missing");

            if (!string.IsNullOrEmpty(request.EmailAddress))
                sw.WriteLine("emailAddress : " + request.EmailAddress);
            else
                sw.WriteLine("emailAddress is missing");

            switch (request.ContactCustomer)
            {
                case true:
                    sw.WriteLine("ContactCustomer : true");
                    break;
                case false:
                    sw.WriteLine("ContactCustomer : false");
                    break;
                default:
                    sw.WriteLine("ContactCustomer is missing");
                    break;
            }


            sw.Flush();
            sw.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundId"></param>
        /// <param name="state"></param>
        /// <param name="status"></param>
        private void SetRefundDescisionStatus(Guid refundId, int state, int status)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed

            var statusrequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference("cgi_refund", refundId),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };

            _xrmManager.Service.Execute(statusrequest);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private QueryByAttribute CreateQueryAttribute(string entityName, string[] columns, string[] attributes, object[] values)
        {
            var query = new QueryByAttribute(entityName)
            {
                ColumnSet = new ColumnSet()
            };

            query.ColumnSet.AddColumns(columns);
            query.Attributes.AddRange(attributes);
            query.Values.AddRange(values);

            return query;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="state"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private static SetStateRequest CreateStateRequest(string entityName, Guid entityId, int state, int status)
        {
            var stateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference(entityName, entityId),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };

            return stateRequest;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="incidentId"></param>
        private void CloseIncident(Guid incidentId)
        {
            SetCaseReopen(incidentId);

            var caseresolution = new Entity("incidentresolution");
            caseresolution.Attributes.Add("incidentid", new EntityReference("incident", incidentId));
            caseresolution.Attributes.Add("subject", "Problemet löst.");

            var closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = caseresolution,
                Status = new OptionSetValue(5)
            };

            _xrmManager.Service.Execute(closerequest);

        }

        /// <summary>
        /// Adds RGOL data to case in fields and annotation.
        /// This is used for queueitems, views and quickforms.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="incident"></param>
        /// <param name="setting"></param>
        private void UpdateCaseWithRgolData(UpdateAutoRgCaseRequest request, Incident incident, RgolSetting setting, int incidentStageCode)
        {
            Entity cgiSetting = GetSettings(_xrmManager.Service, new[] { "cgi_category_detail1id", "cgi_category_detail2id", "cgi_category_detail3id" });
            Entity incidentEntity = new Entity("incident");

            incidentEntity.Id = incident.IncidentId;
            incidentEntity["cgi_iscompleted"] = request.IsCompleted;
            incidentEntity["cgi_rgolcaselog"] = request.RGOLCaseLog;
            incidentEntity["cgi_compensationclaimfromrgol"] = new Money(request.CompensationClaimFromRGOL);
            incidentEntity["cgi_refundtypes"] = new EntityReference("cgi_refundtype", new Guid(setting.RefundTypeId));
            incidentEntity["cgi_casdet_row1_cat1id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail1id");
            incidentEntity["cgi_casdet_row1_cat2id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail2id");
            incidentEntity["cgi_casdet_row1_cat3id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail3id");
            incidentEntity["incidentstagecode"] = new OptionSetValue(incidentStageCode);

            // Could be null in 'Avslag' as it doesnt have a reimbursement form
            if (string.IsNullOrEmpty(setting.ReImBursementFormId) == false)
            {
                incidentEntity["cgi_refundreimbursementform"] = new EntityReference("cgi_reimbursementform", new Guid(setting.ReImBursementFormId));
            }
            if (request.IsCompleted == true)
            {
                incidentEntity["cgi_refundapprovaltype"] = request.Approved ? "Bifall" : "Avslag";
            }

            if (!string.IsNullOrEmpty(request.CustomerDemand))
            {
                incidentEntity["cgi_customer_demand"] = request.CustomerDemand;
            }

            _xrmManager.Service.Update(incidentEntity);
        }
        /// <summary>
        /// Sätter cgi_case_reopen till 1
        /// </summary>
        /// <param name="IncidentId"></param>
        private void SetCaseReopen(Guid IncidentId)
        {
            Entity incidentEntity = new Entity("incident");
            incidentEntity.Id = IncidentId;

            incidentEntity["cgi_case_reopen"] = "1";

            _xrmManager.Service.Update(incidentEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseId"></param>
        private void RouteIncidentToRgolQueue(Guid caseId)
        {
            EntityReference reference = GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultqueue");

            AddToQueueRequest addToSourceQueue = new AddToQueueRequest
            {
                DestinationQueueId = reference.Id,
                Target = new EntityReference("incident", caseId),
            };

            OrganizationResponse response = _xrmManager.Service.Execute(addToSourceQueue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="serviceAttributeName"></param>
        /// <returns></returns>
        private static T GetSetting<T>(IOrganizationService service, string serviceAttributeName)
        {
            return GetSettings(service, new[] { serviceAttributeName }).GetAttributeValue<T>(serviceAttributeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="serviceAttributeNameInLowerCase"></param>
        /// <returns></returns>
        private static Entity GetSettings(IOrganizationService service, string[] serviceAttributeNames)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            for (int i = 0; i < serviceAttributeNames.Length; i++)
            {
                _xml += "       <attribute name='" + serviceAttributeNames[i] + "' />";
            }
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            for (int i = 0; i < serviceAttributeNames.Length; i++)
            {
                string serviceAttributeName = serviceAttributeNames[i];

                for (int j = 0; j < settingscollection.Entities.Count; j++)
                {
                    Entity setting = settingscollection.Entities[j];

                    if (!setting.Contains(serviceAttributeName) && setting[serviceAttributeName] == null)
                    {
                        throw new Exception("Required setting is missing: " + serviceAttributeName);
                    }
                }
            }
            return settingscollection.Entities[0];
        }


        //Create Incident from web.
        internal void RequestCreateCase(CreateCaseRequest request)
        {
            CreateLogFile(request);

            Account account;

            if (request.Customer != null && request.Customer.Value != null && request.Customer.Value != Guid.Empty)
            {
                account = GetAccountFromId((Guid)request.Customer, request.CustomerType);
                if (account == null)
                    throw new Exception($"Kunden med id {request.Customer} hittas inte i databasen!");

                account.RecordCount = account.RecordCount;
            }
            else
            {
                if (request.ContactCustomer)
                {
                    var acc = GetAccountFromEmail(request.EmailAddress, request.CustomerType);
                    if (acc != null)
                    {
                        if (acc.RecordCount == 1)
                        {
                            account = new Account
                            {
                                AccountId = acc.AccountId,
                                RecordCount = acc.RecordCount
                            };
                        }
                        else
                        {
                            var setting = GetDefaultCustomer();

                            if (setting != null)
                            {
                                account = new Account
                                {
                                    AccountId = new Guid(setting.DefaultCustomer),
                                    AccountName = "CGI_DEFAULT",
                                    RecordCount = acc.RecordCount
                                };
                            }
                            else
                            {
                                throw new Exception("Det finns ingen default kund definierad!");
                            }
                        }
                    }
                    else
                    {
                        //Create private customer (contact) from firstname, lastname and emailaddress
                        account = new Account();
                        var g = CreateCustomer(request.FirstName, request.LastName, request.EmailAddress, request.MobilePhoneNumber);
                        account.AccountId = g;
                        account.RecordCount = 1;
                    }
                }
                else
                {
                    var setting = GetDefaultCustomer();
                    if (setting != null)
                    {
                        account = new Account
                        {
                            AccountId = new Guid(setting.DefaultCustomer),
                            AccountName = "CGI_DEFAULT",
                            RecordCount = 1
                        };
                    }
                    else
                        throw new Exception("Det finns ingen default kund definierad!");
                }
            }

            //create case from request
            var incidentId = CreateIncident(request, account);

            //add documents from collection
            if (request.DocumentList != null && request.DocumentList.Any())
            {
                foreach (var doc in request.DocumentList)
                {
                    CreateAnnotation(doc, incidentId);
                }
            }
            //add filelinks from collection
            if (request.FileLinks == null || !request.FileLinks.Any()) return;

            foreach (var fileLink in request.FileLinks)
            {
                CreateFileLink(fileLink, incidentId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal AutoRgCaseResponse RequestCreateAutoRgCase(AutoRgCaseRequest request)
        {
            var response = new AutoRgCaseResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };


            //dublettkontroll, finns redan ett ärende för detta RGOLIssueID?
            QueryBase query = CreateQueryAttribute("incident", new string[] { "incidentid", "ticketnumber" }, new string[] { "cgi_rgolissueid" }, new object[] { request.RGOLIssueID });
            EntityCollection entityResults = _xrmManager.Service.RetrieveMultiple(query);
            if (entityResults.Entities.Count() > 0)
            {
                response.CaseId = (Guid)entityResults.Entities[0]["incidentid"];

                if (entityResults.Entities[0].Contains("ticketnumber") && entityResults.Entities[0]["ticketnumber"] != null)
                    response.CaseNumber = (string)entityResults.Entities[0]["ticketnumber"];

                response.Success = false;
                response.ErrorMessage = "Duplicate RGOLIssueID";//Do not change this error message. Service consumer code may use it to identify duplicates.

                return response;
            }

            try
            {
                Account accountOrContact;
                string errorMessage;

                //För organisationskunder krävs att användaren är inloggad och således krävs ett kundid 
                if (request.IsOrganisation && request.IsLoggedIn == false)
                    throw new Exception("CustomerId, which was not supplied, is required to handle caseregistration for CustomerType 1 = Organization!");

                bool isAccountOrContactInCRM = TryIdentifyAccountOrContactFromAutoRg(request, out accountOrContact, out errorMessage);
                if (string.IsNullOrEmpty(errorMessage) == false)
                    throw new Exception(errorMessage);

                if (request.IsOrganisation == false)
                {
                    if (isAccountOrContactInCRM)
                    {
                        // uppdatera privat kund med tillgänliga uppgifter [grön box i max diagram]
                        UpdateContactFieldsIfOriginalFieldsAreNull(request, accountOrContact);
                    }
                    else
                    {
                        // skapa privat kund med uppgifter från request [Skapa ny kund Max grön ruta]
                        Guid contactId = CreateContact(request);

                        // These values are used in CreateIncidentRgol since oldtimes: TODO check for refac possibility
                        Account contact = new Account()
                        {
                            AccountId = contactId,
                            AccountName = string.Empty,
                            RecordCount = 1,
                        };
                        accountOrContact = contact;
                    }
                }

                var caseId = CreateIncidentRgol(request, accountOrContact);

                RouteIncidentToRgolQueue(caseId);
                response.CaseId = caseId;

                Incident incidentCase = GetIncidentFromId(caseId);
                response.CaseNumber = incidentCase.TicketNumber;

                response.Success = true;
            }
            catch (FaultException faultex)
            {
                response.CaseId = Guid.Empty;
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.CaseId = Guid.Empty;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Create Incident from AutoRG
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal UpdateAutoRgCaseResponse RequestCreateAutoRgRefundDecision(UpdateAutoRgCaseRequest request)
        {
            UpdateAutoRgCaseResponse response = new UpdateAutoRgCaseResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    response.Success = false;
                    response.ErrorMessage = "Guid is guid empty format";
                    return response;
                }
                Incident incidentCase = GetIncidentFromId(request.CaseID);
                // TODO guard rule för fler än ett anrop till denna metod

                RgolSetting setting = GetRgolSettingFromRefundType(request.RefundType);

                BaseCurrency currency = GetCurrency();

                if (request.IsCompleted == false && request.ReqReceipt)
                {
                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050002); // 285050002 | Not answered
                    // Incident status  1    In Progress | 3    Waiting for details
                    var stateRequest = CreateStateRequest("incident", incidentCase.IncidentId, 0, 3);
                    _xrmManager.Service.Execute(stateRequest);
                }
                else if (request.IsCompleted == false && request.ReqReceipt == false)
                {
                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050000); // 285050000 | Ongoing
                    // Incident status  1    In Progress | 3    Waiting for details
                    var stateRequest = CreateStateRequest("incident", incidentCase.IncidentId, 0, 1);
                    _xrmManager.Service.Execute(stateRequest);
                }
                else if (request.IsCompleted && request.Approved)
                {
                    response.RefundID = CreateDecisionRgol(request.Value, setting, incidentCase, currency.BaseCurrencyId, request.InternalMessage, request.CustomerMessage);
                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050006); // 285050006 | Resolved-Approved
                    SetRefundDescisionStatus(response.RefundID, 0, 285050005);// 285050005 = Approved
                    ReimbursementHandler rh = new ReimbursementHandler();
                    rh.ExecuteRefundAndUpdatesStatus(response.RefundID, _xrmManager.Service);
                }
                else if (request.IsCompleted && request.Approved == false)
                {
                    response.RefundID = CreateDecisionRgol(request.Value, setting, incidentCase, currency.BaseCurrencyId, request.InternalMessage, request.CustomerMessage);

                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050007); // 285050007 | Resolved-Denied
                    SetRefundDescisionStatus(response.RefundID, 0, 285050006);// 0 = Active, Declined = 285 050 006
                    CloseIncident(incidentCase.IncidentId);
                }
                else {
                    throw new Exception($"IsCompleted {request.IsCompleted}, request.ReqReceipt {request.ReqReceipt}, IsApproved {request.Approved} is not a valid combination.");
                }

                response.Success = true;
            }
            catch (FaultException faultex)
            {
                response.RefundID = Guid.Empty;
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.RefundID = Guid.Empty;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal UpdateAutoRgResponse RequestCreateBiffTransactionsLinkedToCase(UpdateAutoRgCaseBiffTransactionsRequest request)
        {
            var response = new UpdateAutoRgResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    throw new Exception("Request parameter CaseId is guid empty format.");
                }

                // Delete all current biff transactions as this method could be called several times.
                var guardErrorMessage = "More then 50 bifftransactions attached to case. Something is proberbly wrong.";
                DeleteCurrentEntitiesAttachedToCase(CgiBiffTransaction, request.CaseID, guardErrorMessage);

                foreach (var biffTrans in request.BiffTransactions)
                {
                    var biffTransEntity = new Entity(CgiBiffTransaction)
                    {
                        Attributes = new AttributeCollection
                        {
                            {"cgi_caseid", new EntityReference("incident", request.CaseID)}
                        }
                    };


                    if (string.IsNullOrEmpty(biffTrans.Amount) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_amount", biffTrans.Amount);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Cardsect) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_cardsect", biffTrans.Cardsect);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Date) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_date", biffTrans.Date);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Deviceid) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_deviceid", biffTrans.Deviceid);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Origzone) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_origzone", biffTrans.Origzone);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Origzonename) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_origzonename", biffTrans.Origzonename);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Rectype) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_rectype", biffTrans.Rectype);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Route) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_route", biffTrans.Route);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Time) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_time", biffTrans.Time);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Travelcard) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcard", biffTrans.Travelcard);
                    }
                    if (string.IsNullOrEmpty(biffTrans.TravelcardId) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcardid", biffTrans.TravelcardId);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Travelcardtransaction) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcardtransaction", biffTrans.Travelcardtransaction);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Txntype) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_txntype", biffTrans.Txntype);
                    }

                    _xrmManager.Create(biffTransEntity);
                }
                response.Success = true;

            }
            catch (FaultException faultex)
            {
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        internal UpdateAutoRgResponse RequestCreateTravelInformationLinkedToCase(UpdateAutoRgCaseTravelInformationsRequest request)
        {
            var response = new UpdateAutoRgResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    throw new Exception("Request parameter CaseId is guid empty format.");
                }

                // Delete all current travel informations as this method could be called several times.
                var guardErrorMessage = "More then 50 travelinformations attached to case. Something is proberbly wrong.";
                DeleteCurrentEntitiesAttachedToCase(CgiTravelInformation, request.CaseID, guardErrorMessage);

                foreach (var travelInfo in request.TravelInformations)
                {
                    var travelInformation = new Entity(CgiTravelInformation)
                    {
                        Attributes = new AttributeCollection
                        {
                            {"cgi_caseid", new EntityReference("incident", request.CaseID)}
                        }
                    };

                    if (!string.IsNullOrEmpty(travelInfo.Line))
                    {
                        travelInformation.Attributes.Add("cgi_line", travelInfo.Line);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Company))
                    {
                        travelInformation.Attributes.Add("cgi_contractor", travelInfo.Company);
                    }

                    if (travelInfo.StartPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_startplanned", travelInfo.StartPlanned);
                    }

                    if (travelInfo.ArrivalPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_arivalplanned", travelInfo.ArrivalPlanned);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Title))
                    {
                        travelInformation.Attributes.Add("cgi_travelinformation", travelInfo.Title);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.ArrivalActual))
                    {
                        travelInformation.Attributes.Add("cgi_arivalactual", travelInfo.ArrivalActual);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.City))
                    {
                        travelInformation.Attributes.Add("cgi_city", travelInfo.City);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.DeviationMessage))
                    {
                        travelInformation.Attributes.Add("cgi_deviationmessage", travelInfo.DeviationMessage);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.DirectionText))
                    {
                        travelInformation.Attributes.Add("cgi_directiontext", travelInfo.DirectionText);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                    {
                        //Start time

                        var plannedStartTime = string.Empty;
                        string actualStartTime;
                        var diffStartTime = string.Empty;

                        try
                        {
                            actualStartTime = Convert.ToDateTime(travelInfo.StartActual).ToString("H:mm:ss");
                            plannedStartTime = travelInfo.StartPlanned.Value.ToString("H:mm:ss");
                            var diffStartTimeSpan = Convert.ToDateTime(travelInfo.StartActual) - travelInfo.StartPlanned.Value;

                            if (diffStartTimeSpan.TotalMinutes > 0)
                            {
                                diffStartTime += "+";
                            }

                            if (diffStartTimeSpan.TotalDays > 364 || diffStartTimeSpan.TotalDays < -364)
                            {
                                diffStartTime = "X";
                                actualStartTime = string.Empty;
                            }
                            else
                            {
                                diffStartTime += diffStartTimeSpan.TotalMinutes.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        catch
                        {
                            diffStartTime = "X";
                            actualStartTime = string.Empty;
                        }


                        //Arrive time
                        var plannedEndTime = string.Empty;
                        string actualEndTime;
                        var diffEndTime = string.Empty;

                        try
                        {
                            plannedEndTime = travelInfo.ArrivalPlanned.Value.ToString("H:mm:ss");
                            actualEndTime = Convert.ToDateTime(travelInfo.ArrivalActual).ToString("H:mm:ss");
                            var diffEndTimeSpan = Convert.ToDateTime(travelInfo.ArrivalActual) - travelInfo.ArrivalPlanned.Value;

                            if (diffEndTimeSpan.TotalMinutes > 0)
                            {
                                diffEndTime += "+";
                            }

                            if (diffEndTimeSpan.TotalDays > 364 || diffEndTimeSpan.TotalDays < -364)
                            {
                                diffEndTime = "X";
                                actualEndTime = "";
                            }
                            else
                            {
                                diffEndTime += diffEndTimeSpan.TotalMinutes.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        catch
                        {
                            diffEndTime = "X";
                            actualEndTime = "";
                        }


                        string travelCompany = "(ingen operatör)";
                        if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                        {
                            travelCompany = travelInfo.Company;
                        }

                        travelInformation.Attributes.Add("cgi_displaytext",
                            $"Linje: [{travelInfo.Line}] Tur:{plannedStartTime}[{actualStartTime}({diffStartTime})] {travelInfo.Start} - {plannedEndTime}[{actualEndTime}({diffEndTime})] {travelInfo.Stop} Entreprenör: {travelCompany}");
                    }
                    else if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                    {

                    }

                    if (!string.IsNullOrEmpty(travelInfo.Start))
                    {
                        travelInformation.Attributes.Add("cgi_start", travelInfo.Start);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.StartActual))
                    {
                        travelInformation.Attributes.Add("cgi_startactual", travelInfo.StartActual);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.Stop))
                    {
                        travelInformation.Attributes.Add("cgi_stop", travelInfo.Stop);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Tour))
                    {
                        travelInformation.Attributes.Add("cgi_tour", travelInfo.Tour);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Transport))
                    {
                        travelInformation.Attributes.Add("cgi_transport", travelInfo.Transport);
                    }

                    _xrmManager.Create(travelInformation);
                }

                response.Success = true;

            }
            catch (FaultException faultex)
            {
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        #endregion
    }
}
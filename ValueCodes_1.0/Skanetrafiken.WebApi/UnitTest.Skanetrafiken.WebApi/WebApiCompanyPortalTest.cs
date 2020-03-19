using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Skanetrafiken.Crm.Controllers;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System.Threading;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm.Extensions;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class WebApiCompanyPortalTest : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        private static string WebApiRootEndpoint = @"http://localhost:37909/api/";
        //private static string WebApiRootEndpoint = @"https://crmwebapi-tst.skanetrafiken.se/api";

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }

        [Test, Explicit, Category("Debug")]
        public void GetAccountTest()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    string InputID = "1993-unik";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Accounts/{InputID}");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                    string response = MakeRequest(localContext, httpWebRequest);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [Test, Explicit, Category("Debug")]
        public void CreateAccountsTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                #region Test Code

                Guid correctGuid;

                FilterExpression findAccount = new FilterExpression(LogicalOperator.And);
                findAccount.AddCondition(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, "111-unik");
                AccountEntity existingAccount = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(false), findAccount);

                if (existingAccount != null)
                {
                    XrmHelper.Delete(localContext, existingAccount.ToEntityReference());
                }

                List<AccountInfo> accounts = new List<AccountInfo>();

                AccountInfo correctAccount = new AccountInfo()
                {
                    InformationSource = 13,
                    PortalId = "527458",
                    OrganizationName = "Festbolaget AB",
                    OrganizationNumber = "6348693246",
                    PaymentMethod = 899310000,
                    IsLockedPortal = false,
                    EMail = "generellepost@festbolaget.se",
                    CostSite = "",
                    BillingEmailAddress = "fakturaadress@festbolaget.se",
                    BillingMethod = 899310000,
                    Suborgname = "Festens Bolag"
                };

                AddressInfo correctAddress = new AddressInfo()
                {
                    TypeCode = 1,
                    Name = "TypeCode 1 Adress",
                    Street = "TypeCode 1 Adress gata 1B",
                    PostalCode = "41660",
                    City = "Göteborg",
                    CountryISO = "SE"
                };

                correctAccount.Addresses = new List<AddressInfo>();
                correctAccount.Addresses.Add(correctAddress);

                AccountInfo noOrgNr = new AccountInfo()
                {
                    InformationSource = 13,
                    PortalId = "888888",
                    OrganizationName = "Organization 12",
                    OrganizationNumber = "",
                    PaymentMethod = 899310000,
                    IsLockedPortal = false,
                    EMail = "info@ed.se",
                    CostSite = "243 - RÖR",
                    BillingEmailAddress = "endeavor@ed.se",
                    BillingMethod = 899310000
                };

                AccountInfo noPortalId = new AccountInfo()
                {
                    InformationSource = 13,
                    PortalId = "",
                    OrganizationName = "Organization 12",
                    OrganizationNumber = "333-unique",
                    PaymentMethod = 899310000,
                    IsLockedPortal = false,
                    EMail = "info@ed.se",
                    CostSite = "243 - RÖR",
                    BillingEmailAddress = "endeavor@ed.se",
                    BillingMethod = 899310000
                };


                accounts.Add(correctAccount);
                accounts.Add(noOrgNr);
                accounts.Add(noPortalId);

                // SAVE

                foreach (AccountInfo account in accounts)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Accounts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(account, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in Create Account: " + ex.Message, ex);

                    }
                }

                //CleanUpAccounts(localContext, accounts);


                List<CustomerInfo> contacts = new List<CustomerInfo>();

                CustomerInfo testContact = new CustomerInfo()
                {
                    Source = 13,
                    FirstName = "Marcus",
                    LastName = "Stenswed",
                    SocialSecurityNumber = "199102013936",
                    SwedishSocialSecurityNumber = true,
                    SwedishSocialSecurityNumberSpecified = true,
                    CreditsafeOkSpecified = false,
                    AvlidenSpecified = false,
                    UtvandradSpecified = false,
                    EmailInvalidSpecified = false,
                    Guid = "00000000-0000-0000-0000-000000000000",
                    isLockedPortal = false,
                    isLockedPortalSpecified = true,
                    isAddressEnteredManuallySpecified = false,
                    isAddressInformationCompleteSpecified = false,
                    Email = "ms@endeavor.se",
                    Telephone = "131313"

                };

                testContact.CompanyRole = new CustomerInfoCompanyRole[1];
                testContact.CompanyRole[0] = new CustomerInfoCompanyRole()
                {
                    PortalId = "2045",
                    CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                    CompanyRoleSpecified = true,
                    Email = "1234@190604.se",
                    Telephone = "0767999999",
                    deleteCompanyRole = false,
                    isLockedPortal = false,
                    isLockedPortalSpecified = true
                };

                contacts.Add(testContact);


                //CustomerInfo correctContact = new CustomerInfo()
                //{
                //    Source = 13,
                //    FirstName = "Marcus",
                //    LastName = "Stenswedd",
                //    SocialSecurityNumber = "199209221770",
                //    SwedishSocialSecurityNumber = true,
                //    Email = "m@endeavor.se",
                //    Telephone = "232323",
                //    SwedishSocialSecurityNumberSpecified = true

                //};

                //correctContact.CompanyRole = new CustomerInfoCompanyRole[1];
                //correctContact.CompanyRole[0] = new CustomerInfoCompanyRole()
                //{
                //    PortalId = "527458",
                //    CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                //    CompanyRoleSpecified = true,
                //    deleteCompanyRole = false,
                //    Email = "m@endeavor.se",
                //    isLockedPortal = false,
                //    isLockedPortalSpecified = false,
                //    Telephone = "0735198846"
                //};

                //contacts.Add(correctContact);
                

                // CREATE
                foreach (CustomerInfo customer in contacts)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(customer, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in Create Account: " + ex.Message, ex);

                    }
                }



                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("CreateAccountsTest time = {0}", stopwatch.Elapsed);
            }
        }

        [Test, Explicit, Category("Debug")]
        public void UpdateAccountsTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                #region Test Code

                #region Save

                Guid correctGuid;
                
                FilterExpression findAccount = new FilterExpression(LogicalOperator.And);
                findAccount.AddCondition(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, "111-unik");
                AccountEntity correctAccount = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(false), findAccount);

                if(correctAccount != null)
                {
                    correctGuid = correctAccount.Id;
                }
                else
                {
                    correctAccount = new AccountEntity()
                    {
                        AccountNumber = "111-unik",
                        Name = "Organization 12",
                        cgi_organizational_number = "9999999",
                        ed_PaymentMethod = Generated.ed_account_ed_paymentmethod.Invoice,
                        ed_IsLockedPortal = false,
                        EMailAddress1 = "indiens@ed.se",
                        ed_CostSite = "243 - RÖR",
                        ed_BillingEmailAddress = "endeavor@ed.se",
                        ed_BillingMethod = Generated.ed_account_ed_billingmethod.Email
                    };

                    correctGuid = XrmHelper.Create(localContext, correctAccount);
                }
                // SAVE   
                #endregion

                #region Update

                List<AccountInfo> updateAccounts = new List<AccountInfo>();

                AccountInfo correctUpdate = new AccountInfo()
                {
                    Guid = correctGuid.ToString(),
                    InformationSource = 13,
                    PortalId = "111-unik",
                    OrganizationName = "Organization 12",
                    OrganizationNumber = "10101010",
                    PaymentMethod = 899310000,
                    IsLockedPortal = false,
                    EMail = "UNIKEMAIL@ed.se",
                    CostSite = "243 - RÖR",
                    BillingEmailAddress = "endeavor@ed.se",
                    BillingMethod = 899310000
                };

                AccountInfo noPortalId = new AccountInfo()
                {
                    Guid = correctGuid.ToString(),
                    InformationSource = 13,
                    PortalId = "111-unik",
                    OrganizationName = "Organization 12",
                    OrganizationNumber = "111-unique",
                    PaymentMethod = 899310000,
                    IsLockedPortal = false,
                    EMail = "UNIKEMAIL@ed.se",
                    CostSite = "243 - RÖR",
                    BillingEmailAddress = "endeavor@ed.se",
                    BillingMethod = 899310000
                };
                
                updateAccounts.Add(correctUpdate);
                updateAccounts.Add(noPortalId);

                // Update       
                foreach (AccountInfo account in updateAccounts)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Accounts/{account.Guid}");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(account, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in Create Account: " + ex.Message, ex);

                    }
                }
                #endregion
                
                CleanUpAccounts(localContext, updateAccounts);

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
            }
        }


        private void CleanUpAccounts(Plugin.LocalPluginContext localContext, List<AccountInfo> inputaccounts)
        {
            foreach (AccountInfo inputaccount in inputaccounts)
            {
                string orgNr = inputaccount.OrganizationNumber;

                IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, orgNr)
                            }
                        });

                foreach (AccountEntity account in accounts)
                {
                    XrmHelper.Delete(localContext, account.ToEntityReference());

                    IList<CustomerAddressEntity> addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, account.Id)
                            }
                        });
                    
                }
            }
        }

        [Test, Explicit, Category("Debug")]
        public void GetContactTest()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    string InputID = "199311290770";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts/{InputID}");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    string response = MakeRequest(localContext, httpWebRequest);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [Test, Explicit, Category("Debug")]
        public void CreateContactsTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                #region Test Code

                List<CustomerInfo> contacts = new List<CustomerInfo>();

                CustomerInfo correctContact = new CustomerInfo()
                {
                    Source = 13,
                    FirstName = "Daniel",
                    LastName = "Test",
                    SocialSecurityNumber = "199311290770",
                    SwedishSocialSecurityNumber = true,
                    Email = "d@endeavor.se",
                    Telephone = "232323",
                    SwedishSocialSecurityNumberSpecified = true
                };

                correctContact.CompanyRole = new CustomerInfoCompanyRole[1];
                correctContact.CompanyRole[0] = new CustomerInfoCompanyRole()
                {
                    PortalId = "1993-unik",
                    CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                    CompanyRoleSpecified = false // TODO CHANGE
                };
                
                contacts.Add(correctContact);

                // CREATE
                foreach (CustomerInfo customer in contacts)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(customer, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in Create Account: " + ex.Message, ex);

                    }
                }

                CleanUpContacts(localContext, contacts);

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("CreateContactsTest time = {0}", stopwatch.Elapsed);
            }
        }

        [Test, Explicit, Category("Debug")]
        public void CreateAndDeleteContactsTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                List<CustomerInfo> contacts = new List<CustomerInfo>();

                CustomerInfo correctContact = new CustomerInfo()
                {
                    Source = 13,
                    FirstName = "Daniel",
                    LastName = "Test",
                    SocialSecurityNumber = "199311290770",
                    SwedishSocialSecurityNumber = true,
                    Email = "d@endeavor.se",
                    Telephone = "232323",
                    SwedishSocialSecurityNumberSpecified = true,
                };

                correctContact.CompanyRole = new CustomerInfoCompanyRole[1];
                correctContact.CompanyRole[0] = new CustomerInfoCompanyRole()
                {
                    PortalId = "1993-unik",
                    CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                    CompanyRoleSpecified = true 
                };

                //contacts.Add(correctContact);

                //// CREATE
                //foreach (CustomerInfo customer in contacts)
                //{
                try
                {

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(correctContact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    string response = MakeRequest(localContext, httpWebRequest);

                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Create Account: " + ex.Message, ex);

                }
                //}

                QueryExpression query = new QueryExpression
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, correctContact.SocialSecurityNumber)
                            }
                        },
                    TopCount = 5
                };
                IList<ContactEntity> contactLst = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);

                ContactEntity contactCreated = new ContactEntity();

                foreach (var contact in contactLst)
                {
                    contactCreated = contact;
                }


                correctContact.CompanyRole[0].deleteCompanyRole = true;
                correctContact.CompanyRole[0].isLockedPortal = true;
                correctContact.CompanyRole[0].isLockedPortalSpecified = true;

                try
                {

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts/{contactCreated.Id}");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(correctContact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    string response = MakeRequest(localContext, httpWebRequest);

                }
                catch (Exception ex)
                {
                    throw new Exception("Error in UpdateContact: " + ex.Message, ex);

                }


            }
        }


        [Test, Explicit, Category("Debug")]
        public void UpdateContactsTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                #region Test Code

                #region Save

                //Guid correctGuid;

                //FilterExpression findContact = new FilterExpression(LogicalOperator.And);
                //findContact.AddCondition(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, "199311290770");
                //ContactEntity correctContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false), findContact);

                //if (correctContact != null)
                //{
                //    correctGuid = correctContact.Id;
                //}
                //else
                //{
                //    correctContact = new ContactEntity()
                //    {
                //        FirstName = "Daniel",
                //        LastName = "Test",
                //        cgi_socialsecuritynumber = "199311290770"
                //    };

                //    correctGuid = XrmHelper.Create(localContext, correctContact);
                //}
                // SAVE   
                #endregion

                #region Update

                List<CustomerInfo> updateContacts= new List<CustomerInfo>();

                //CustomerInfo wrongSSN = new CustomerInfo()
                //{
                //    Source = 13,
                //    FirstName = "Daniel",
                //    LastName = "Test",
                //    SocialSecurityNumber = "19931290770",
                //    SwedishSocialSecurityNumber = true
                //};

                CustomerInfo correctContact1 = new CustomerInfo()
                {
                    Source = 13,
                    FirstName = "Marcus",
                    LastName = "Stenswedd",
                    SocialSecurityNumber = "199209221770",
                    SwedishSocialSecurityNumber = true,
                    Email = "m@endeavor.se",
                    Telephone = "232323",
                    SwedishSocialSecurityNumberSpecified = true,
                    Guid = "3f4fe5a1-e1ca-e711-80ef-005056b64d75"

                };

                correctContact1.CompanyRole = new CustomerInfoCompanyRole[1];
                correctContact1.CompanyRole[0] = new CustomerInfoCompanyRole()
                {
                    PortalId = "527458",
                    CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                    CompanyRoleSpecified = true,
                    deleteCompanyRole = true,
                    Email = "m@endeavor.se",
                    isLockedPortal = false,
                    isLockedPortalSpecified = false,
                    Telephone = "0735198846"
                };

                //updateContacts.Add(wrongSSN);
                updateContacts.Add(correctContact1);

                // Update       
                foreach (CustomerInfo contact in updateContacts)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts/{"3f4fe5a1-e1ca-e711-80ef-005056b64d75"}");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(contact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in UpdateContact: " + ex.Message, ex);

                    }
                }
                #endregion

                //CleanUpContacts(localContext, updateContacts);

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
            }
        }

        [Test, Explicit, Category("Debug")]
        public void FullFlowPortalTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                #region Test Setup
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                
                {
                    /* Temp */
                    // Generate info for creating main account (which creates cost site as well)
                    AccountInfo accountTest = new AccountInfo
                    {
                        InformationSource = 13,
                        Guid = Guid.Empty.ToString(),
                        PortalId = "2098839",
                        ReferencePortal = "",
                        OrganizationName = "Tapir Utveckling AB",
                        OrganizationNumber = "5569707556",
                        PaymentMethod = null,
                        IsLockedPortal = false,
                        BillingEmailAddress = "",
                        BillingMethod = 899310000,
                        Addresses = new List<AddressInfo>
                        {
                            new AddressInfo
                            {
                                TypeCode = 1,
                                Street = "Kund-id WIN4902, FE nr 160",
                                PostalCode = "10569",
                                City = "Stockholm"
                            }
                        },

                    };

                    // Create Account
                    #region Account - POST
                    try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Accounts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(accountTest, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result, we);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion

                    CustomerInfo customerTest = new CustomerInfo()
                    {
                        Source = 13,
                        FirstName = "MIJA",
                        LastName = "TOFTNER",
                        CompanyRole = new CustomerInfoCompanyRole[]
                        {
                            new CustomerInfoCompanyRole()
                            {
                                PortalId = "2098839",
                                CompanyRole = 899310000,
                                CompanyRoleSpecified = true,
                                Email = "lotta@tapirutveckling.se",
                                Telephone = "0721613617",
                                deleteCompanyRole = false,
                                isLockedPortal = false,
                                isLockedPortalSpecified = true
                            }
                        },
                        SocialSecurityNumber = "196104293920",
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                        CreditsafeOkSpecified = false,
                        AvlidenSpecified = false,
                        UtvandradSpecified = false,
                        EmailInvalidSpecified = false,
                        Guid = Guid.Empty.ToString(),
                        isLockedPortal = false,
                        isLockedPortalSpecified = true,
                        isAddressEnteredManuallySpecified = false,
                        isAddressInformationCompleteSpecified = false
                    };

                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(customerTest, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result, we);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                    CustomerInfo customerTest2 = new CustomerInfo()
                    {
                        Source = 13,
                        FirstName = "Lotta",
                        LastName = "Green Dahlberg",
                        CompanyRole = new CustomerInfoCompanyRole[]
                        {
                            new CustomerInfoCompanyRole()
                            {
                                PortalId = "2098839",
                                CompanyRole = 899310000,
                                CompanyRoleSpecified = true,
                                Email = "lotta@tapirutveckling.se",
                                Telephone = "0721613617",
                                deleteCompanyRole = false,
                                isLockedPortal = false,
                                isLockedPortalSpecified = true
                            }
                        },
                        SocialSecurityNumber = "196009124329",
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                        CreditsafeOkSpecified = false,
                        AvlidenSpecified = false,
                        UtvandradSpecified = false,
                        EmailInvalidSpecified = false,
                        Guid = Guid.Empty.ToString(),
                        isLockedPortal = false,
                        isLockedPortalSpecified = true,
                        isAddressEnteredManuallySpecified = false,
                        isAddressInformationCompleteSpecified = false
                    };

                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Contacts");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(customerTest2, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        string response = MakeRequest(localContext, httpWebRequest);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result, we);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }




                // Generate info for creating main account (which creates cost site as well)
                AccountInfo account = GenerateAccountToCreate();

                // Create Account
                #region Account - POST
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiRootEndpoint}Accounts");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(account, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    string response = MakeRequest(localContext, httpWebRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in Create Account (POST): " + ex.Message, ex);
                }
                #endregion

                #region Controls Account
                // Get Account and see if created with correct values
                AccountEntity retrievedAccount = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(true),
                    new FilterExpression()
                    {
                        Conditions =
                            {
                                new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, account.PortalId)
                            }
                    });

                // Check if Account was found/created
                if(retrievedAccount == null)
                    throw new Exception("No Account was found or created");

                // Check if Parent Account was found/created
                if (retrievedAccount.ParentAccountId == null)
                    throw new Exception("No Parent Account (main organization) was created");

                // Check flags and correct value
                if ((retrievedAccount.ed_AllowCreate == null || retrievedAccount.ed_AllowCreate == false) ||
                    retrievedAccount.ed_IsLockedPortal == null || retrievedAccount.ed_IsLockedPortal == true)
                    throw new Exception("AllowCreate or IsLockedPortal is either null or set to true (should be false)");

                AccountEntity parentAccount = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(true),
                     new FilterExpression()
                     {
                         Conditions =
                             {
                                new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, account.OrganizationNumber)
                             }
                     });

                // Check if retrieved parent account is same as parent account that was created
                if (parentAccount.Id != retrievedAccount.ParentAccountId.Id)
                    throw new Exception("Retrieved parent account (from organizational number) does not match with the created one");
                #endregion







                stopwatch.Stop();
                localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
            }
        }

        public CustomerInfo GenereateContactRoleToCreate()
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(100, 1000);
            int randomPhone = rnd.Next(100000, 1000000);
            

            CustomerInfo customerInfo = new CustomerInfo()
            {
                Source = 13,
                FirstName = "Marcus",
                LastName = "Stenswed" + rnd.Next(1, 10),
                SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                SwedishSocialSecurityNumber = true,
                Email = "adminstrator" + randomNumber + "@test.se",
                Telephone = randomPhone.ToString(),
                SwedishSocialSecurityNumberSpecified = true
            };

            return customerInfo;
        }

        public AccountInfo GenerateAccountToCreate()
        {
            Random rnd = new Random();
            int portalId = rnd.Next(1000000, 10000000);
            int organizationNumber = rnd.Next(1000000, 10000000);

            AccountInfo account = new AccountInfo()
            {
                InformationSource = 13,
                PortalId = portalId.ToString(),
                OrganizationName = "Test Organisationen " + portalId,
                OrganizationNumber = organizationNumber.ToString(),
                PaymentMethod = 899310000,
                IsLockedPortal = false,
                CostSite = "Kostnadsställe " + portalId,
                EMail = "epost_" + portalId + "@test.se",
                BillingEmailAddress = "fakturaepost_" + portalId + "@test.se",
                BillingMethod = 899310000
            };

            return account;
        }


        private void CleanUpContacts(Plugin.LocalPluginContext localContext, List<CustomerInfo> inputContacts)
        {
            foreach (ContactInfo inputContact in inputContacts)
            {
                string SSN = inputContact.SocialSecurityNumber;

                IList<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, SSN)
                            }
                        });

                foreach (ContactEntity contact in contacts)
                {
                    XrmHelper.Delete(localContext, contact.ToEntityReference());
                }
            }
        }

        public string MakeRequest(Plugin.LocalPluginContext localContext, HttpWebRequest httpWebRequest)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.(int)Help.errorCodes.MissingInformation

            try
            {
                try
                {

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Opportunity Test results 1 = {0}", result);

                        return result;
                        //OpportunityWebApi info = Newtonsoft.Json.JsonConvert.DeserializeObject<OpportunityWebApi>(result);
                        throw new Exception("Opportunity test passed");
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null || response.StatusCode == HttpStatusCode.InternalServerError)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response == null)
                    throw we;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("Opportunity test returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                    throw new Exception(result, we);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }

    }
}

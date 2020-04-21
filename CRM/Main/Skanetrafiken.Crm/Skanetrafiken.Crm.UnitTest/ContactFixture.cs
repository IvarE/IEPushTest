using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Globalization;
using System.Net;
using Skanetrafiken.Crm;
using System.Text.RegularExpressions;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class ContactFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test]
        public void TestCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                var con = new ContactEntity();
                XrmHelper.Create(localContext, con);


            }
        }

        [Test, Explicit]
        public void Contact_Util()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();


                //ContactEntity c1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(),new ColumnSet(false));
                //ContactEntity c2 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(), new ColumnSet(false));

                MergeRecordsEntity merge = XrmRetrieveHelper.Retrieve<MergeRecordsEntity>(localContext, new Guid("F5B46823-322C-E811-826F-00155D010B00"), new ColumnSet(true));

                merge.PerformeMerge(localContext);


            }
        }
        [Test, Category("Run Always")]
        public void MergeRequestFunctionality()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                ContactEntity mainContact = new ContactEntity
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    EMailAddress2 = "merge@test.com",
                    FirstName = "Test",
                    LastName = "Merge" + DateTime.Now.ToString()
                };

                mainContact.Id = XrmHelper.Create(localContext, mainContact);
                mainContact.ContactId = mainContact.Id;

                ContactEntity mergeContact = new ContactEntity
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    MobilePhone = "46321956",
                    Telephone1 = "214553215",
                    FirstName = "Test2",
                    LastName = "Merge2"
                };

                mergeContact.Id = XrmHelper.Create(localContext, mergeContact);
                mergeContact.ContactId = mergeContact.Id;

                mainContact.CombineContacts(localContext, new List<ContactEntity> { mergeContact });

            }
        }

        [Test]
        public void Contact_HasMKLValueTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                ContactEntity.HasMKLBalance(localContext, new EntityReference { Id = new Guid("9FCD3F23-46F8-E611-812B-00155D010B02"), LogicalName = ContactEntity.EntityLogicalName });


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test]
        public void Contact_CreateUnittestContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                //ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid("d1fefc59-7ba6-e611-8112-00155d0a6b01"), new ColumnSet(false));
                ContactEntity contact = ContactFixture.BuildUnitTestContactNoCreate(localContext);
                contact.HandlePreContactCreate(localContext);

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test]
        public void Contact_CreateMiniContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                //ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid("d1fefc59-7ba6-e611-8112-00155d0a6b01"), new ColumnSet(false));
                ContactEntity contact = new ContactEntity();
                contact.FirstName = "to";
                contact.LastName = "mas";
                contact.ed_HasSwedishSocialSecurityNumber = false;
                //contact.cgi_socialsecuritynumber = "123456";
                contact.ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund;

                contact.HandlePreContactCreate(localContext);

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test, Explicit]
        public void PreContactSetStateTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                //ContactEntity contact = null;
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                //ContactEntity preImage = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.EntityLogicalName, new ColumnSet(false),
                //    new FilterExpression
                //    {
                //        Conditions =
                //        {
                //            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull),
                //           new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                //           //new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, new Guid("D9DD5F7F-B6E7-E611-8122-00155D010B02"))
                //        }
                //    });
                //contact = new ContactEntity
                //{
                //    ContactId = preImage.ContactId
                //};

                ContactEntity.HandlePreContacSetState(localContext, new EntityReference { Id = new Guid("9FCD3F23-46F8-E611-812B-00155D010B02"), LogicalName = ContactEntity.EntityLogicalName });
            }
        }

        [Test, Category("Run Always")]
        public void PreContactPluginTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                ContactEntity contact = null;
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                ContactEntity c = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false));

                contact = new ContactEntity
                {
                    FirstName = "RandomFirstName",
                    LastName = "MoreRandom",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                };
                contact.HandlePreContactCreate(localContext);

                try
                {
                    contact = BuildUnitTestContactNoCreate(localContext);
                    contact.HandlePreContactCreate(localContext);
                    contact.Id = localContext.OrganizationService.Create(contact);
                    contact.ContactId = contact.Id;

                    ContactEntity conflictContact;

                    #region admskapakund SocSecConflict
                    conflictContact = new ContactEntity
                    {
                        FirstName = "RandomFirstName",
                        LastName = "MoreRandom",
                        cgi_socialsecuritynumber = contact.cgi_socialsecuritynumber,
                        ed_HasSwedishSocialSecurityNumber = contact.ed_HasSwedishSocialSecurityNumber,
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                        ContactId = new Guid()
                    };

                    bool exception = false;
                    try
                    {
                        conflictContact.HandlePreContactCreate(localContext);
                    }
                    catch (ApplicationException)
                    {
                        exception = true;
                    }
                    NUnit.Framework.Assert.True(exception);
                    exception = false;
                    #endregion

                    #region admskapakund EmailConflict
                    conflictContact = new ContactEntity
                    {
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        EMailAddress2 = contact.EMailAddress2,
                        ContactId = new Guid()
                    };

                    try
                    {
                        conflictContact.HandlePreContactCreate(localContext);
                    }
                    catch (ApplicationException)
                    {
                        exception = true;
                    }
                    NUnit.Framework.Assert.True(exception);
                    exception = false;
                    #endregion

                    ContactEntity preImage = BuildUnitTestContactNoCreate(localContext, DateTime.Now.AddMonths(-5));
                    preImage.Id = XrmHelper.Create(localContext, preImage);
                    preImage.ContactId = preImage.Id;

                    #region admuppdaterakund SocSecConflict
                    conflictContact = new ContactEntity
                    {
                        cgi_socialsecuritynumber = contact.cgi_socialsecuritynumber,
                        ed_HasSwedishSocialSecurityNumber = contact.ed_HasSwedishSocialSecurityNumber,
                        ed_InformationSource = Generated.ed_informationsource.AdmAndraKund,
                        ContactId = preImage.ContactId,
                        Id = preImage.Id
                    };

                    try
                    {
                        conflictContact.HandlePreContactUpdate(localContext, preImage);
                    }
                    catch (ApplicationException)
                    {
                        exception = true;
                    }
                    NUnit.Framework.Assert.True(exception);
                    exception = false;
                    #endregion

                    #region admuppdaterakund EmailConflict
                    conflictContact = new ContactEntity
                    {
                        ed_InformationSource = Generated.ed_informationsource.AdmAndraKund,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        EMailAddress2 = contact.EMailAddress2,
                        ContactId = preImage.ContactId,
                        Id = preImage.Id
                    };

                    try
                    {
                        conflictContact.HandlePreContactUpdate(localContext, preImage);
                    }
                    catch (ApplicationException)
                    {
                        exception = true;
                    }
                    NUnit.Framework.Assert.True(exception);
                    exception = false;
                    #endregion

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (contact?.Id != null)
                    {
                        SetStateRequest req = new SetStateRequest
                        {
                            EntityMoniker = contact.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ContactState.Inactive),
                            Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                        };
                        localContext.OrganizationService.Execute(req);
                    }
                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
        }

        public static ContactEntity CreateUnitTestContact(Plugin.LocalPluginContext localContext)
        {
            ContactEntity contact = BuildUnitTestContactNoCreate(localContext);
            contact.Id = localContext.OrganizationService.Create(contact);
            return contact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public static ContactEntity BuildUnitTestContactNoCreate(Plugin.LocalPluginContext localContext, DateTime dateTime)
        {
            string socSecNumber = CustomerUtility.GenerateValidSocialSecurityNumber(dateTime);
            CustomerInfo info = new CustomerInfo
            {
                Email = $"{socSecNumber}@unit.test",
                FirstName = "UnitTestFirstName",
                LastName = $"UnitTest{socSecNumber}",
                Mobile = "987654321",
                Telephone = "87654321",
                SocialSecurityNumber = socSecNumber,
                Source = (int)Generated.ed_informationsource.SkapaMittKonto,
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
                AddressBlock = new CustomerInfoAddressBlock
                {
                    Line1 = "testLine1",
                    PostalCode = "12345",
                    City = "gStad",
                    CountryISO = "SE"
                }
            };
            ContactEntity contact = new ContactEntity(localContext, info);
            contact.EMailAddress2 = contact.EMailAddress1;
            contact.EMailAddress1 = null;
            return contact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public static ContactEntity BuildUnitTestContactNoCreate(Plugin.LocalPluginContext localContext)
        {
            return BuildUnitTestContactNoCreate(localContext, DateTime.Now);
        }

        [Test, Explicit]
        public void ContactInfoScrambler()
        {
            return;

            //  TODO - Gör smartare så att man inte behöver göra en Fetch på alla Contacts samtidigt.

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                int emailCounter = 100, testCounter = 0;
                bool keepAnonymizing = true;
                List<Tuple<Guid, string, Exception>> errors = new List<Tuple<Guid, string, Exception>>();

                while (keepAnonymizing)
                {
                    IList<ContactEntity> batchOfContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext,
                        new QueryExpression
                        {
                            EntityName = ContactEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(ContactEntity.Fields.EMailAddress1,
                            ContactEntity.Fields.EMailAddress2,
                            ContactEntity.Fields.Address1_Line1,
                            ContactEntity.Fields.Address1_PostalCode,
                            ContactEntity.Fields.cgi_socialsecuritynumber,
                            ContactEntity.Fields.LastName),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.DoesNotBeginWith, "test-"),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.DoesNotBeginWith, "email1-"),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.DoesNotEndWith, "@example.com"),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.DoesNotBeginWith, "email2-"),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.DoesNotEndWith, "@example.com"),
                                    new ConditionExpression(ContactEntity.Fields.Address1_Line1, ConditionOperator.DoesNotBeginWith, "test-"),
                                    new ConditionExpression(ContactEntity.Fields.Address1_PostalCode, ConditionOperator.DoesNotBeginWith, "test-")
                                }
                            },
                            TopCount = 100
                        });
                    if (batchOfContacts.Count == 0)
                        keepAnonymizing = false;
                    foreach (ContactEntity c in batchOfContacts)
                    {
                        try
                        {
                            localContext.Trace("testCounter = {0}, emailCounter = {1}", ++testCounter, emailCounter);

                            if (!string.IsNullOrWhiteSpace(c.EMailAddress1))
                                c.EMailAddress1 = string.Format("email-{0}@example.com", emailCounter++);
                            if (!string.IsNullOrWhiteSpace(c.EMailAddress2))
                                c.EMailAddress2 = string.Format("email2-{0}@example.com", emailCounter++);
                            if (!string.IsNullOrWhiteSpace(c.Address1_Line1))
                                c.Address1_Line1 = string.Format("test-{0}", testCounter);
                            if (!string.IsNullOrWhiteSpace(c.Address1_PostalCode))
                                c.Address1_PostalCode = string.Format("test-{0}", testCounter);
                            if (!string.IsNullOrWhiteSpace(c.LastName))
                                c.LastName = string.Format("test-{0}", testCounter);
                            if (!string.IsNullOrWhiteSpace(c.cgi_socialsecuritynumber))
                                c.cgi_socialsecuritynumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
                            localContext.OrganizationService.Update(c);
                        }
                        catch (Exception e)
                        {
                            errors.Add(new Tuple<Guid, string, Exception>(c.Id, e.Message, e));
                        }
                    }
                }
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Tuple<Guid, string, Exception>>));
                ser.WriteObject(stream, errors);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);

                string[] serialized = { sr.ReadToEnd(), string.Format("emailCounter = {0}, testCounter = {1}", emailCounter.ToString(), testCounter.ToString()) };

                System.IO.File.WriteAllLines(@"C:\Stuff\Utils\SkaneInfoScrambleErrors.txt", serialized);


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test, Explicit]
        public void ContactEmailScrambler()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                IList<ContactEntity> allContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext,
                    new ColumnSet(ContactEntity.Fields.EMailAddress1,
                    ContactEntity.Fields.EMailAddress2,
                    ContactEntity.Fields.cgi_ContactNumber),
                    new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Like, "%@mailnator.com%"),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Like, "%@mailnator.com%")
                        },
                    });

                int testCounter = 0;
                ErrorLogList errors = new ErrorLogList();
                foreach (ContactEntity c in allContacts)
                {
                    try
                    {
                        localContext.Trace("testCounter = {0}", testCounter);
                        testCounter++;
                        if (!string.IsNullOrWhiteSpace(c.EMailAddress1))
                            c.EMailAddress1 = c.EMailAddress1.Replace("mailnator", "mailinator");
                        if (!string.IsNullOrWhiteSpace(c.EMailAddress2))
                            c.EMailAddress2 = c.EMailAddress2.Replace("mailnator", "mailinator");
                        localContext.OrganizationService.Update(c);
                    }
                    catch (Exception e)
                    {
                        errors.AddLog(c.Id, e.Message, e);
                    }
                }

                localContext.Trace($"allContacts.Count = {allContacts.Count}");
                localContext.Trace($"Number of errors = {errors.Count()}");


                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ErrorLogList));
                ser.WriteObject(stream, errors);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);

                string[] serialized = { sr.ReadToEnd(), $"allContacts = {allContacts.Count}, testCounter = {testCounter.ToString()}" };

                System.IO.File.WriteAllLines(@"C:\Temp\SkaneMailScrambleErrors.txt", serialized);


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        //----------- TEST CK ----------------
        public static CompanyRoleEntity GetCompanyRole(Plugin.LocalPluginContext localContext, string companyRoleId)
        {
            CompanyRoleEntity coRole = null;
            Guid coRoleGuid;

            if (!string.IsNullOrWhiteSpace(companyRoleId))
            {
                bool isValidGuid = Guid.TryParse(companyRoleId, out coRoleGuid);

                if (isValidGuid == true)
                {
                    coRole = XrmRetrieveHelper.Retrieve<CompanyRoleEntity>(localContext, coRoleGuid, new ColumnSet(true));
                }
            }

            return coRole;
        }

        [Test, Explicit, Category("Debug")]
        public void MatchCompanyRoleWithContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                bool checkSingleCompanyRole = false;

                List<CompanyRoleEntity> allCompanyRoles = new List<CompanyRoleEntity>();

                if (checkSingleCompanyRole == true)
                {
                    //Test
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "C8084AEB-D0C4-E911-80F0-005056B61FFF"));

                    //Acceptans
                    allCompanyRoles.Add(GetCompanyRole(localContext, "6154D44F-D76F-E811-80F0-005056B665EC"));
                    allCompanyRoles.Add(GetCompanyRole(localContext, "66C17142-7BF1-E811-80F1-005056B665EC"));//Magic
                    allCompanyRoles.Add(GetCompanyRole(localContext, "07F7253D-05B0-E811-80F1-005056B665EC")); //karlsson 1
                    allCompanyRoles.Add(GetCompanyRole(localContext, "AA9F401A-49FD-E811-80F1-005056B665EC"));
                    allCompanyRoles.Add(GetCompanyRole(localContext, "43B2EED2-19A5-E811-80F0-005056B665EC"));//Karlsson 2
                    allCompanyRoles.Add(GetCompanyRole(localContext, "7250527A-BFF7-E811-80F1-005056B665EC"));

                    //Test
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "7AC37E7C-7178-E911-80F0-005056B61FFF"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "BD3FB828-C823-E911-80F0-005056B61FFF"));
                }
                else
                {
                    //Hämta alla Organisationsroller (kontrollera status aktiv?)
                    FilterExpression getCompanyRole = new FilterExpression(LogicalOperator.And);
                    getCompanyRole.AddCondition(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.NotNull);
                    getCompanyRole.AddCondition(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.NotNull);
                    allCompanyRoles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, new ColumnSet(true), getCompanyRole).ToList();
                }

                if (allCompanyRoles != null && allCompanyRoles.Count > 0)
                {
                    //Handle every CompanyRole Found
                    foreach (CompanyRoleEntity coRole in allCompanyRoles)
                    {
                        //Get the associated Contact
                        ContactEntity coContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, coRole.ed_Contact.Id, new ColumnSet(
                                ContactEntity.Fields.EMailAddress1,
                                ContactEntity.Fields.EMailAddress2,
                                ContactEntity.Fields.Telephone2,
                                ContactEntity.Fields.FirstName,
                                ContactEntity.Fields.LastName,
                                ContactEntity.Fields.AccountRoleCode,
                                ContactEntity.Fields.ed_SocialSecurityNumber2,
                                ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                                ContactEntity.Fields.ed_BusinessContact));

                        //Get Associated Account
                        AccountEntity coAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coRole.ed_Account.Id, new ColumnSet(true));

                        //Make sure CompanyRole has email and telephone
                        if (!string.IsNullOrWhiteSpace(coRole.ed_EmailAddress))
                        {
                            //Make sure we found contact and account
                            if (coContact != null && coAccount != null)
                            {
                                //Compare Email1/Email2 to cRole_Email
                                if ((!string.IsNullOrWhiteSpace(coContact.EMailAddress1) || !string.IsNullOrWhiteSpace(coContact.EMailAddress2)))
                                {

                                    //Check if related contacts data (email) matches with CompanyRoles data (email), if not... enter.
                                    if ((string.Compare(coContact.EMailAddress1, coRole.ed_EmailAddress, true) != 0 &&
                                        string.Compare(coContact.EMailAddress2, coRole.ed_EmailAddress, true) != 0))
                                    {
                                        //Create CompanyRole Object that will be updated
                                        CompanyRoleEntity uppCompanyRole = new CompanyRoleEntity();

                                        //if not the same, search for a contact in the system with cRole_Email + cRole_Telephone
                                        List<ContactEntity> emMoContacts = new List<ContactEntity>();
                                        ContactEntity newContact = null;

                                        #region Email Filter (emailFilter)

                                        FilterExpression emailFilter = new FilterExpression(LogicalOperator.Or)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, coRole.ed_EmailAddress),
                                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, coRole.ed_EmailAddress)
                                            }
                                        };

                                        #endregion

                                        emMoContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                            },
                                            Filters =
                                            {
                                                emailFilter,
                                            }

                                        }).ToList();

                                        if (emMoContacts.Count == 1)
                                        {
                                            //Found Contact, update CompanyRole with new Contact
                                            newContact = emMoContacts[0];

                                            #region Handle CompanyRole

                                            uppCompanyRole.Id = coRole.Id;
                                            uppCompanyRole.ed_Contact = newContact.ToEntityReference();

                                            if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                                !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                string firstnameNewContact = Regex.Replace(newContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                                string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                                string lastnameNewContact = Regex.Replace(newContact.LastName, @"[^\p{L}\p{N}]+", "");
                                                string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                                // Compare first- and lastname
                                                if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                }

                                                if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_LastName = newContact.LastName;
                                                }
                                            }
                                            else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                                string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                uppCompanyRole.ed_LastName = newContact.LastName;
                                            }

                                            //Update Company Role
                                            XrmHelper.Update(localContext, uppCompanyRole); //UPDATE COMPANY ROLE

                                            #endregion

                                            #region Uppdate Contact With Information Fom CompanyRole

                                            //Uppdate Contact with some information
                                            ContactEntity uppdateContact = new ContactEntity();
                                            uppdateContact.Id = newContact.Id;
                                            uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                            uppdateContact.ed_BusinessContact = true;
                                            uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            uppdateContact.ed_isLockedPortal = false;

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                uppdateContact.Company = coRole.ed_Telephone;
                                            }

                                            //handle social security number (change)
                                            if (!String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }

                                                if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    if (String.Compare(newContact.ed_SocialSecurityNumberBlock, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                    }
                                                }
                                                else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.ed_SocialSecurityNumberBlock) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }
                                            }
                                            else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock) && String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                }
                                            }
                                            else
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            XrmHelper.Update(localContext, uppdateContact); //UPDATE CONTACT

                                            #endregion

                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //add "AssociateEntitiesRequest" with the entities.
                                                // Connect Contact to Accounts (level 1 and 2)
                                                // Create an AssociateEntities request.

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestCostSiteRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestCostSiteRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                            if (coAccount.ParentAccountId != null)
                                            {
                                                //Check if association between parent account and contact exists
                                                if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                                {
                                                    #region AssociateEntities request

                                                    //Namespace is Microsoft.Crm.Sdk.Messages
                                                    AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                    // Set the ID of Moniker1 to the ID of the lead.
                                                    requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                    // Set the ID of Moniker2 to the ID of the contact.
                                                    requestOrgRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                    // Set the relationship name to associate on.
                                                    requestOrgRel.RelationshipName = "cgi_account_contact";

                                                    // Execute the request.
                                                    localContext.OrganizationService.Execute(requestOrgRel);

                                                    DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                    #endregion
                                                }
                                            }

                                            #region Update Account and Parent Account

                                            //Update Account with PrimaryContact + Type Of Acount
                                            AccountEntity updateAccount = new AccountEntity();
                                            updateAccount.Id = coAccount.Id;
                                            bool toUpdate = false;
                                            if (coAccount.PrimaryContactId == null)
                                            {
                                                updateAccount.PrimaryContactId = newContact.ToEntityReference();
                                                toUpdate = true;
                                            }

                                            if (coAccount.ed_PortalCustomer != true)
                                            {
                                                updateAccount.ed_PortalCustomer = true;
                                                toUpdate = true;
                                            }

                                            if (toUpdate == true)
                                            {
                                                XrmHelper.Update(localContext, updateAccount);
                                            }

                                            if (coAccount.ParentAccountId != null)
                                            {
                                                //Update ParentAccount with PrimaryContact + Type Of Acount
                                                AccountEntity parentAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coAccount.ParentAccountId.Id, 
                                                    new ColumnSet(AccountEntity.Fields.Id, AccountEntity.Fields.ed_PortalCustomer, AccountEntity.Fields.PrimaryContactId));

                                                if (parentAccount != null)
                                                {
                                                    AccountEntity updateAccount2 = new AccountEntity();
                                                    updateAccount2.Id = parentAccount.Id;
                                                    bool toUpdateParent = false;
                                                    if (parentAccount.PrimaryContactId == null)
                                                    {
                                                        updateAccount2.PrimaryContactId = newContact.ToEntityReference();
                                                        toUpdateParent = true;
                                                    }

                                                    if (parentAccount.ed_PortalCustomer != true)
                                                    {
                                                        updateAccount2.ed_PortalCustomer = true;
                                                        toUpdateParent = true;
                                                    }

                                                    if (toUpdateParent == true)
                                                    {
                                                        XrmHelper.Update(localContext, updateAccount2);
                                                    }
                                                }
                                            }

                                            #endregion

                                        }
                                        else if (emMoContacts.Count > 1)
                                        {

                                            //Find the contact with the latest created on
                                            newContact = emMoContacts.Where(i => i.CreatedOn == emMoContacts.Max(x => x.CreatedOn)).FirstOrDefault();

                                            #region Handle CompanyRole

                                            uppCompanyRole.Id = coRole.Id;
                                            uppCompanyRole.ed_Contact = newContact.ToEntityReference();

                                            if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                                !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                string firstnameNewContact = Regex.Replace(newContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                                string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                                string lastnameNewContact = Regex.Replace(newContact.LastName, @"[^\p{L}\p{N}]+", "");
                                                string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                                // Compare first- and lastname
                                                if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                }
                                                if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_LastName = newContact.LastName;
                                                }
                                            }
                                            else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                              string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                              !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                              !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                uppCompanyRole.ed_LastName = newContact.LastName;
                                            }

                                            //Update Company Role
                                            XrmHelper.Update(localContext, uppCompanyRole); //UPDATE COMPANY ROLE

                                            #endregion

                                            //Update Contact with information
                                            #region Update Contact with information from CompanyRole

                                            ContactEntity uppdateContact = new ContactEntity();
                                            uppdateContact.Id = newContact.Id;
                                            uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                            uppdateContact.ed_BusinessContact = true;
                                            uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            uppdateContact.ed_isLockedPortal = false;

                                            if (!String.IsNullOrWhiteSpace(uppCompanyRole.ed_Telephone))
                                            {
                                                uppdateContact.Company = uppCompanyRole.ed_Telephone;
                                            }

                                            //Socialsecuritynumber??
                                            //if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock) && !String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            //{
                                            //    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            //}

                                            //handle social security number (change)
                                            if (!String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }

                                                if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    if (String.Compare(newContact.ed_SocialSecurityNumberBlock, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                    }
                                                }
                                                else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.ed_SocialSecurityNumberBlock) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }
                                            }
                                            else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock) && String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                }
                                            }
                                            else
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            XrmHelper.Update(localContext, uppdateContact);

                                            #endregion

                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //add "AssociateEntitiesRequest" with the entities.
                                                // Connect Contact to Accounts (level 1 and 2)
                                                // Create an AssociateEntities request.

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestCostSiteRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestCostSiteRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                            if (coAccount.ParentAccountId != null)
                                            {
                                                //Check if association between parent account and contact exists
                                                if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                                {
                                                    #region AssociateEntities request

                                                    //Namespace is Microsoft.Crm.Sdk.Messages
                                                    AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                    // Set the ID of Moniker1 to the ID of the lead.
                                                    requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                    // Set the ID of Moniker2 to the ID of the contact.
                                                    requestOrgRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                    // Set the relationship name to associate on.
                                                    requestOrgRel.RelationshipName = "cgi_account_contact";

                                                    // Execute the request.
                                                    localContext.OrganizationService.Execute(requestOrgRel);

                                                    DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                    #endregion
                                                }
                                            }

                                            #region Update Account and Parent Account

                                            //Update Account with PrimaryContact + Type Of Acount
                                            AccountEntity updateAccount = new AccountEntity();
                                            updateAccount.Id = coAccount.Id;
                                            bool toUpdate = false;
                                            if (coAccount.PrimaryContactId == null)
                                            {
                                                updateAccount.PrimaryContactId = newContact.ToEntityReference();
                                                toUpdate = true;
                                            }

                                            if (coAccount.ed_PortalCustomer != true)
                                            {
                                                updateAccount.ed_PortalCustomer = true;
                                                toUpdate = true;
                                            }

                                            if (toUpdate == true)
                                            {
                                                XrmHelper.Update(localContext, updateAccount);
                                            }

                                            if (coAccount.ParentAccountId != null)
                                            {
                                                //Update ParentAccount with PrimaryContact + Type Of Acount
                                                AccountEntity parentAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coAccount.ParentAccountId.Id,
                                                    new ColumnSet(AccountEntity.Fields.Id, AccountEntity.Fields.ed_PortalCustomer, AccountEntity.Fields.PrimaryContactId));

                                                if (parentAccount != null)
                                                {
                                                    AccountEntity updateAccount2 = new AccountEntity();
                                                    updateAccount2.Id = parentAccount.Id;
                                                    bool toUpdateParent = false;
                                                    if (parentAccount.PrimaryContactId == null)
                                                    {
                                                        updateAccount2.PrimaryContactId = newContact.ToEntityReference();
                                                        toUpdateParent = true;
                                                    }

                                                    if (parentAccount.ed_PortalCustomer != true)
                                                    {
                                                        updateAccount2.ed_PortalCustomer = true;
                                                        toUpdateParent = true;
                                                    }

                                                    if (toUpdateParent == true)
                                                    {
                                                        XrmHelper.Update(localContext, updateAccount2);
                                                    }
                                                }
                                            }

                                            #endregion

                                        }
                                        else if (emMoContacts.Count == 0) //Did not find any contact: Create new
                                        {
                                            //if not found, create new contact with, cRole_F.Name + cRole_L.Name + cRole_Email + cRole_Telephone

                                            #region Create New Contact With Info. From Company Role

                                            newContact = new ContactEntity();

                                            //Create a new contact and place it in the company role
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_EmailAddress))
                                            {
                                                newContact.EMailAddress1 = coRole.ed_EmailAddress;
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                newContact.Telephone2 = coRole.ed_Telephone; //Add number to new field
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                newContact.Company = coRole.ed_Telephone; //Add number to new field
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_FirstName))
                                            {
                                                newContact.FirstName = coRole.ed_FirstName;
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_LastName))
                                            {
                                                newContact.LastName = coRole.ed_LastName;
                                            }

                                            //coRole PN -> Contact PersonNR (Social security Block)
                                            if (!string.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                newContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            newContact.ed_BusinessContact = true;
                                            newContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            newContact.ed_isLockedPortal = false;
                                            newContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;

                                            Guid newCreatedContact = XrmHelper.Create(localContext, newContact); //CREATE CONTACT

                                            #endregion

                                            ContactEntity newlyCreatedContact = null;

                                            if (newCreatedContact != null && newCreatedContact != Guid.Empty)
                                            {
                                                newlyCreatedContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newCreatedContact, new ColumnSet(true));
                                            }
                                            ////Retrieve contact (null control)
                                            //ContactEntity newlyCreatedContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newCreatedContact, new ColumnSet(true));

                                            if (newlyCreatedContact != null && newlyCreatedContact.ContactId != null)
                                            {

                                                #region Handle Company Role

                                                //Update CompanyRole with the created contact
                                                uppCompanyRole.Id = coRole.Id;
                                                uppCompanyRole.ed_Contact = newlyCreatedContact.ToEntityReference();

                                                XrmHelper.Update(localContext, uppCompanyRole);

                                                #endregion

                                                //3. Check so that there isn't already an association placed!!
                                                if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newlyCreatedContact.ContactId.Value))
                                                {
                                                    #region AssociateEntities request

                                                    //add "AssociateEntitiesRequest" with the entities.
                                                    // Connect Contact to Accounts (level 1 and 2)
                                                    // Create an AssociateEntities request.

                                                    //Namespace is Microsoft.Crm.Sdk.Messages
                                                    AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                    // Set the ID of Moniker1 to the ID of the lead.
                                                    requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                    // Set the ID of Moniker2 to the ID of the contact.
                                                    requestCostSiteRel.Moniker2 = new EntityReference { Id = newlyCreatedContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                    // Set the relationship name to associate on.
                                                    requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                    // Execute the request.
                                                    localContext.OrganizationService.Execute(requestCostSiteRel);

                                                    DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false); //??

                                                    #endregion
                                                }

                                                if (coAccount.ParentAccountId != null)
                                                {
                                                    //Check if association between parent account and contact exists
                                                    if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newlyCreatedContact.ContactId.Value))
                                                    {
                                                        #region AssociateEntities request

                                                        //Namespace is Microsoft.Crm.Sdk.Messages
                                                        AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                        // Set the ID of Moniker1 to the ID of the lead.
                                                        requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                        // Set the ID of Moniker2 to the ID of the contact.
                                                        requestOrgRel.Moniker2 = new EntityReference { Id = newlyCreatedContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                        // Set the relationship name to associate on.
                                                        requestOrgRel.RelationshipName = "cgi_account_contact";

                                                        // Execute the request.
                                                        localContext.OrganizationService.Execute(requestOrgRel);

                                                        DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false); //??

                                                        #endregion
                                                    }
                                                }

                                                #region Update Account and Parent Account

                                                //Update Account with PrimaryContact + Type Of Acount
                                                AccountEntity updateAccount = new AccountEntity();
                                                updateAccount.Id = coAccount.Id;
                                                bool toUpdate = false;
                                                if (coAccount.PrimaryContactId == null)
                                                {
                                                    updateAccount.PrimaryContactId = newlyCreatedContact.ToEntityReference();
                                                    toUpdate = true;
                                                }

                                                if (coAccount.ed_PortalCustomer != true)
                                                {
                                                    updateAccount.ed_PortalCustomer = true;
                                                    toUpdate = true;
                                                }

                                                if (toUpdate == true)
                                                {
                                                    XrmHelper.Update(localContext, updateAccount);
                                                }

                                                if (coAccount.ParentAccountId != null)
                                                {
                                                    //Update ParentAccount with PrimaryContact + Type Of Acount
                                                    AccountEntity parentAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coAccount.ParentAccountId.Id,
                                                        new ColumnSet(AccountEntity.Fields.Id, AccountEntity.Fields.ed_PortalCustomer, AccountEntity.Fields.PrimaryContactId));

                                                    if (parentAccount != null)
                                                    {
                                                        AccountEntity updateAccount2 = new AccountEntity();
                                                        updateAccount2.Id = parentAccount.Id;
                                                        bool toUpdateParent = false;
                                                        if (parentAccount.PrimaryContactId == null)
                                                        {
                                                            updateAccount2.PrimaryContactId = newlyCreatedContact.ToEntityReference();
                                                            toUpdateParent = true;
                                                        }

                                                        if (parentAccount.ed_PortalCustomer != true)
                                                        {
                                                            updateAccount2.ed_PortalCustomer = true;
                                                            toUpdateParent = true;
                                                        }

                                                        if (toUpdateParent == true)
                                                        {
                                                            XrmHelper.Update(localContext, updateAccount2);
                                                        }
                                                    }
                                                }

                                                #endregion

                                            }

                                            //throw new Exception(string.Format("Multiple Contacts found with the same Email: {0}, and Mobile Number: {1}", customerInfo.Email, customerInfo.Mobile)); //revise
                                        }
                                    }
                                    else //Contacts email + telephone is same as company roles, create association to account
                                    {
                                        //2. Uppdatera kontakt med rollen Administratör
                                        #region Update Contact with information from CompanyRole

                                        ContactEntity uppdateContact = new ContactEntity();
                                        uppdateContact.Id = coContact.Id;
                                        uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                        uppdateContact.ed_BusinessContact = true;
                                        uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;

                                        if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                        {
                                            uppdateContact.Company = coRole.ed_Telephone; //Update phone nu,ber with in new field
                                        }

                                        //handle social security number (change)
                                        if (!String.IsNullOrWhiteSpace(coContact.cgi_socialsecuritynumber))
                                        {
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                if (String.Compare(coRole.ed_SocialSecurityNumber, coContact.cgi_socialsecuritynumber) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                    //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                }
                                            }

                                            if (!String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (String.Compare(coContact.ed_SocialSecurityNumberBlock, coContact.cgi_socialsecuritynumber) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coContact.cgi_socialsecuritynumber;
                                            }
                                        }
                                        else if (!String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                        {
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                if (String.Compare(coRole.ed_SocialSecurityNumber, coContact.ed_SocialSecurityNumberBlock) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                    //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                }
                                            }
                                        }
                                        else if (String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock) && String.IsNullOrWhiteSpace(coContact.cgi_socialsecuritynumber))
                                        {
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }
                                        }
                                        else
                                        {
                                            uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                        }


                                        XrmHelper.Update(localContext, uppdateContact);

                                        #endregion

                                        #region Handle CompanyRole

                                        //Create CompanyRole Object that will be updated
                                        CompanyRoleEntity uppCompanyRole = new CompanyRoleEntity();
                                        uppCompanyRole.Id = coRole.Id;
                                        bool toBeUpdated = false;

                                        if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                            !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                            !string.IsNullOrWhiteSpace(coContact.FirstName) &&
                                            !string.IsNullOrWhiteSpace(coContact.LastName))
                                        {
                                            string firstnameNewContact = Regex.Replace(coContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                            string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                            string lastnameNewContact = Regex.Replace(coContact.LastName, @"[^\p{L}\p{N}]+", "");
                                            string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                            // Compare first- and lastname
                                            if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                            {
                                                uppCompanyRole.ed_FirstName = coContact.FirstName;
                                                toBeUpdated = true;
                                            }
                                            if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                            {
                                                uppCompanyRole.ed_LastName = coContact.LastName;
                                                toBeUpdated = true;
                                            }
                                        }
                                        else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                            string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                            !string.IsNullOrWhiteSpace(coContact.FirstName) &&
                                            !string.IsNullOrWhiteSpace(coContact.LastName))
                                        {
                                            uppCompanyRole.ed_FirstName = coContact.FirstName;
                                            uppCompanyRole.ed_LastName = coContact.LastName;
                                            toBeUpdated = true;
                                        }

                                        if (toBeUpdated == true)
                                        {
                                            XrmHelper.Update(localContext, uppCompanyRole);
                                        }

                                        #endregion

                                        //3. Check so that there isn't already an association placed!!
                                        if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.ContactId.Value))
                                        {
                                            #region AssociateEntities request

                                            //add "AssociateEntitiesRequest" with the entities.
                                            // Connect Contact to Accounts (level 1 and 2)
                                            // Create an AssociateEntities request.

                                            //Namespace is Microsoft.Crm.Sdk.Messages
                                            AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                            // Set the ID of Moniker1 to the ID of the lead.
                                            requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                            // Set the ID of Moniker2 to the ID of the contact.
                                            requestCostSiteRel.Moniker2 = new EntityReference { Id = coContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                            // Set the relationship name to associate on.
                                            requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                            // Execute the request.
                                            localContext.OrganizationService.Execute(requestCostSiteRel);

                                            #endregion
                                        }

                                        if (coAccount.ParentAccountId != null)
                                        {
                                            //Check if association between parent account and contact exists
                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestOrgRel.Moniker2 = new EntityReference { Id = coContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestOrgRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestOrgRel);

                                                #endregion
                                            }
                                        }
                                        

                                        #region Update Account and Parent Account

                                        //Update Account with PrimaryContact + Type Of Acount
                                        AccountEntity updateAccount = new AccountEntity();
                                        updateAccount.Id = coAccount.Id;
                                        bool toUpdate = false;
                                        if (coAccount.PrimaryContactId == null)
                                        {
                                            updateAccount.PrimaryContactId = coContact.ToEntityReference();
                                            toUpdate = true;
                                        }

                                        if (coAccount.ed_PortalCustomer != true)
                                        {
                                            updateAccount.ed_PortalCustomer = true;
                                            toUpdate = true;
                                        }

                                        if (toUpdate == true)
                                        {
                                            XrmHelper.Update(localContext, updateAccount);
                                        }

                                        if (coAccount.ParentAccountId != null)
                                        {
                                            //Update ParentAccount with PrimaryContact + Type Of Acount
                                            AccountEntity parentAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coAccount.ParentAccountId.Id,
                                                new ColumnSet(AccountEntity.Fields.Id, AccountEntity.Fields.ed_PortalCustomer, AccountEntity.Fields.PrimaryContactId));

                                            if (parentAccount != null)
                                            {
                                                AccountEntity updateAccount2 = new AccountEntity();
                                                updateAccount2.Id = parentAccount.Id;
                                                bool toUpdateParent = false;
                                                if (parentAccount.PrimaryContactId == null)
                                                {
                                                    updateAccount2.PrimaryContactId = coContact.ToEntityReference();
                                                    toUpdateParent = true;
                                                }

                                                if (parentAccount.ed_PortalCustomer != true)
                                                {
                                                    updateAccount2.ed_PortalCustomer = true;
                                                    toUpdateParent = true;
                                                }

                                                if (toUpdateParent == true)
                                                {
                                                    XrmHelper.Update(localContext, updateAccount2);
                                                }
                                            }
                                        }

                                        #endregion

                                    }

                                }//What to do if the found contat has null email / telephone (this wont happen according to them)
                            } // What to do if contact or account is null
                        } //What to do if companyRole Email/Telephone is null
                    }
                }

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        //Create method that checks association
        public static bool DoesRelationshipExist(Plugin.LocalPluginContext localContext, string relationshipSchemaName, string entity1Schema, Guid entity1GuidValue, string entity2Schema, Guid entity2GuidValue)
        {
            string fetchXml = "<fetch mapping='logical'> <entity name='" + relationshipSchemaName + "'>"
              + "<all-attributes />"
              + "<filter>"
              + "<condition attribute='" + entity1Schema + "id' operator='eq' value ='" + entity1GuidValue.ToString() + "' />"
              + "<condition attribute='" + entity2Schema + "id' operator='eq' value='" + entity2GuidValue.ToString() + "' />"
              + "</filter>"
              + "</entity>"
              + "</fetch>";

            //string fetchResult = localContact.
            var fetchResult = localContext.OrganizationService.RetrieveMultiple(new FetchExpression(fetchXml));

            if (fetchResult.Entities.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //remove association from contact
        public static void DisassociateEntities(Plugin.LocalPluginContext localContext, string relationshipSchema, string entity1Schema, Guid entity1Guid, string entity2Schema, Guid entity2Guid, bool skipCheck)
        {
            if (skipCheck || DoesRelationshipExist(localContext, relationshipSchema, entity1Schema, entity1Guid, entity2Schema, entity2Guid))
            {
                EntityReference moniker1 = new EntityReference { Id = entity1Guid, LogicalName = entity1Schema };
                EntityReference moniker2 = new EntityReference { Id = entity2Guid, LogicalName = entity2Schema };

                DisassociateEntitiesRequest request = new DisassociateEntitiesRequest();
                request.Moniker1 = moniker1;
                request.Moniker2 = moniker2;
                request.RelationshipName = relationshipSchema;

                localContext.OrganizationService.Execute(request);
            }
        }

        [DataContract]
        private class ErrorLogList
        {
            public ErrorLogList()
            {
                LogList = new List<ErrorLog>();
            }

            [DataMember]
            public List<ErrorLog> LogList { get; set; }

            public int Count()
            {
                return LogList.Count;
            }

            public void AddLog(Guid guid, string errorMessage, Exception exception)
            {
                LogList.Add(new ErrorLog(guid, errorMessage, exception));
            }
        }

        [DataContract]
        private class ErrorLog
        {
            public ErrorLog(Guid guid, string errorMessage, Exception exception)
            {
                Guid = guid;
                ErrorMessage = errorMessage;
                Exception = exception;
            }

            [DataMember]
            public Guid Guid { get; set; }
            [DataMember]
            public string ErrorMessage { get; set; }
            [DataMember]
            public Exception Exception { get; set; }
        }

        [Test, Explicit]
        public void TestRandomSocialSecurityNumber()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                List<string> numbers = new List<string>();

                for (int i = 0; i < 100; i++)
                {
                    numbers.Add(CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now.AddHours(new Random().Next(-87600))));
                }

                foreach (string n in numbers)
                {
                    localContext.Trace(n);
                }


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test, Explicit]
        public void TestRgolInfo()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                string testInstanceName = GetUnitTestID();
                CustomerInfo rgolInfo = GetDynamicRgolInfo(testInstanceName);

                ContactEntity rgolContact = new ContactEntity(localContext, rgolInfo);

                rgolContact.HandlePreContactCreate(localContext);

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test, Category("Run Always")]
        public void UpdateFBTestPostContactAsync()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                
                ContactEntity contactDoNotUpdateFBEqFalse = new ContactEntity();
                contactDoNotUpdateFBEqFalse.FirstName = "Marcus";
                contactDoNotUpdateFBEqFalse.LastName = "Pettersson " + DateTime.Now;
                contactDoNotUpdateFBEqFalse.ed_DoNotUpdateFB = false;
                contactDoNotUpdateFBEqFalse.ed_HasSwedishSocialSecurityNumber = true;
                contactDoNotUpdateFBEqFalse.ed_InformationSource = Generated.ed_informationsource.AdmAndraKund;
                Guid contactId = localContext.OrganizationService.Create(contactDoNotUpdateFBEqFalse);

                
                
                ContactEntity contactDoNotUpdateFBEqTrue = contactDoNotUpdateFBEqFalse;
                contactDoNotUpdateFBEqTrue.Id = contactId;
                contactDoNotUpdateFBEqTrue.ed_DoNotUpdateFB = true;
                localContext.OrganizationService.Update(contactDoNotUpdateFBEqTrue);



                string contactidTrim = contactId.ToString().Trim('{', '}');

                System.Threading.Thread.Sleep(20000);

                IList<DeltabatchQueueEntity> deltabatchQueue = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext,
                        new QueryExpression
                        {
                            EntityName = DeltabatchQueueEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(DeltabatchQueueEntity.Fields.ed_name),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactGuid, ConditionOperator.Equal, contactidTrim)
                                }
                            },
                            TopCount = 100
                        });

                

                if (deltabatchQueue == null || deltabatchQueue.Count <= 0)
                {
                    throw new InvalidPluginExecutionException();
                }
                else
                {
                    // Successfully created deltabatch queue item
                }


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        private CustomerInfo GetDynamicRgolInfo(string testInstanceName)
        {
            // Build a valid, unique personnummer
            StringBuilder personnummer = new StringBuilder(testInstanceName.Replace(".", ""));
            string pnr9digits = personnummer.ToString().Substring(2, 9);
            personnummer[11] = CustomerUtility.calculateCheckDigit(pnr9digits)[0];
            string SocSecNumber = $"14{personnummer.ToString().Substring(2)}";


            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.RGOL,
                FirstName = "TestRgolFirstName",
                LastName = "TestRgolName:" + SocSecNumber,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "RgolLine1 " + testInstanceName,
                    PostalCode = "12345",
                    City = "RgolVillage" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                //Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = SocSecNumber,
                Email = $"RGOL{testInstanceName}@r.gol",
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
            };
        }

        /// <summary>
        /// Get unique string YYMMDD.HHSS
        /// </summary>
        /// <returns></returns>
        private static string GetUnitTestID()
        {
            DateTime today = DateTime.Now;

            return today.ToString("yyyyMMdd.hhmm");
        }


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

    }
}

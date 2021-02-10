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
using Skanetrafiken.Crm.Controllers;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class ContactFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

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

                //LeadEntity lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, new Guid("55D20E13-3A3A-E711-80F8-00505690700F"), new ColumnSet(true));

                //ContactEntity contact = new ContactEntity
                //{
                //    EMailAddress2 = "fluff@test.scom",
                //    FirstName = "fluff",
                //    LastName = "NotFluff",
                //    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                //};

                //XrmHelper.Create(localContext.OrganizationService, contact);

                //contact.LastName = "DifferentName";
                //contact.FirstName = "OtherName";

                //XrmHelper.Create(localContext.OrganizationService, contact);

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
                contact.cgi_socialsecuritynumber = "123456";
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
                ContactEntity contact = null;
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
                
                ContactEntity.HandlePreContacSetState(localContext, new EntityReference{ Id = new Guid("9FCD3F23-46F8-E611-812B-00155D010B02"), LogicalName = ContactEntity.EntityLogicalName });
            }
        }

        [Test, Explicit]
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

                try
                {
                    contact = new ContactEntity
                    {
                        FirstName = "RandomFirstName",
                        LastName = "MoreRandom",
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };
                    contact.HandlePreContactCreate(localContext);
                }
                catch (Exception)
                {
                    throw;
                }
                try
                {
                    contact = BuildUnitTestContactNoCreate(localContext);
                    contact.HandlePreContactCreate(localContext);
                    contact.Id = localContext.OrganizationService.Create(contact);

                    ContactEntity conflictContact;

                    #region admskapakund SocSecConflict
                    conflictContact = new ContactEntity
                    {
                        FirstName = "RandomFirstName",
                        LastName = "MoreRandom",
                        cgi_socialsecuritynumber = contact.cgi_socialsecuritynumber,
                        ed_HasSwedishSocialSecurityNumber = contact.ed_HasSwedishSocialSecurityNumber,
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
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
                        EMailAddress2 = contact.EMailAddress2
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

                    ContactEntity mockPreImage = new ContactEntity
                    {
                        FirstName = conflictContact.FirstName,
                        LastName = conflictContact.LastName
                    };

                    #region admuppdaterakund SocSecConflict
                    conflictContact = new ContactEntity
                    {
                        FirstName = "RandomFirstName",
                        LastName = "MoreRandom",
                        cgi_socialsecuritynumber = contact.cgi_socialsecuritynumber,
                        ed_HasSwedishSocialSecurityNumber = contact.ed_HasSwedishSocialSecurityNumber,
                        ed_InformationSource = Generated.ed_informationsource.AdmAndraKund
                    };

                    try
                    {
                        conflictContact.HandlePreContactUpdate(localContext, mockPreImage);
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
                        EMailAddress2 = contact.EMailAddress2
                    };

                    try
                    {
                        conflictContact.HandlePreContactUpdate(localContext, mockPreImage);
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
        public static ContactEntity BuildUnitTestContactNoCreate(Plugin.LocalPluginContext localContext)
        {
            string socSecNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
            CustomerInfo info = new CustomerInfo
            {
                Email = $"{socSecNumber}@unit.test",
                FirstName = "UnitTestFirstName",
                LastName = $"UnitTest{socSecNumber}",
                Mobile = "987654321",
                Telephone = "87654321",
                SocialSecurityNumber = socSecNumber,
                Source = (int)CustomerUtility.Source.AdmSkapaKund,
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

        [Test, Explicit]
        public void ContactInfoScrambler()
        {

            return;

            //// Connect to the Organization service. 
            //// The using statement assures that the service proxy will be properly disposed.
            //using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            //{
            //    // This statement is required to enable early-bound type support.
            //    _serviceProxy.EnableProxyTypes();

            //    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

            //    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //    stopwatch.Start();

            //    IList<ContactEntity> allContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext,
            //        new ColumnSet(ContactEntity.Fields.EMailAddress1,
            //        ContactEntity.Fields.EMailAddress2,
            //        ContactEntity.Fields.Address1_Line1,
            //        ContactEntity.Fields.Address1_PostalCode,
            //        ContactEntity.Fields.cgi_socialsecuritynumber,
            //        ContactEntity.Fields.LastName));

            //    int emailCounter = 100, testCounter = 0;
            //    List<Tuple<Guid, string, Exception>> errors = new List<Tuple<Guid, string, Exception>>();
            //    foreach (ContactEntity c in allContacts)
            //    {
            //        try
            //        {
            //            localContext.Trace("testCounter = {0}, emailCounter = {1}", testCounter, emailCounter);
            //            testCounter++;
            //            if (!string.IsNullOrWhiteSpace(c.EMailAddress1))
            //                c.EMailAddress1 = string.Format("email-{0}@example.com", emailCounter++);
            //            if (!string.IsNullOrWhiteSpace(c.EMailAddress2))
            //                c.EMailAddress2 = string.Format("email2-{0}@example.com", emailCounter++);
            //            if (!string.IsNullOrWhiteSpace(c.Address1_Line1))
            //                c.Address1_Line1 = string.Format("test-{0}", testCounter);
            //            if (!string.IsNullOrWhiteSpace(c.Address1_PostalCode))
            //                c.Address1_PostalCode = string.Format("test-{0}", testCounter);
            //            if (!string.IsNullOrWhiteSpace(c.LastName))
            //                c.LastName = string.Format("test-{0}", testCounter);
            //            if (!string.IsNullOrWhiteSpace(c.cgi_socialsecuritynumber))
            //                c.cgi_socialsecuritynumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
            //            localContext.OrganizationService.Update(c);
            //        }
            //        catch (Exception e)
            //        {
            //            errors.Add(new Tuple<Guid, string, Exception>(c.Id, e.Message, e));
            //        }
            //    }

            //    MemoryStream stream = new MemoryStream();
            //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Tuple<Guid, string, Exception>>));
            //    ser.WriteObject(stream, errors);
            //    stream.Position = 0;
            //    StreamReader sr = new StreamReader(stream);

            //    string[] serialized = { sr.ReadToEnd(), string.Format("emailCounter = {0}, testCounter = {1}", emailCounter.ToString(), testCounter.ToString()) };

            //    System.IO.File.WriteAllLines(@"C:\Stuff\Utils\SkaneInfoScrambleErrors.txt", serialized);


            //    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            //}
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

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
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using Skanetrafiken.Crm.ValueCodes;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;

namespace Endeavor.Crm.IntegrationTests
{
    [TestFixture]
    public class ValueCodeFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;


        [Test]
        public void Test_CreateIncident()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                //var contactQuery = new QueryExpression()
                //{
                //    EntityName = ContactEntity.EntityLogicalName,
                //    ColumnSet = new ColumnSet(),
                //    Criteria =
                //    {
                //        Conditions =
                //        {
                //            new ConditionExpression(ContactEntity.Fields.cgi_ContactNumber, ConditionOperator.Equal, "1"),
                //            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Anonym")
                //        }
                //    }
                //};

                //var anonymContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);
                //if (anonymContact == null)
                //    throw new InvalidPluginExecutionException($"Contact 'Anonym' with number 1 does not exist in system.");


                //var categoryDetailQuery = new QueryExpression()
                //{
                //    EntityName = CgiCategoryDetailEntity.EntityLogicalName,
                //    ColumnSet = new ColumnSet(),
                //    Criteria =
                //    {
                //        Conditions =
                //        {
                //            new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_Level, ConditionOperator.Equal, "3"),
                //            new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_categorydetailname, ConditionOperator.Equal, "Webbshop Privat")
                //        }
                //    }
                //};

                //var categoryDetail = XrmRetrieveHelper.RetrieveFirst<CgiCategoryDetailEntity>(localContext, categoryDetailQuery);
                //if (categoryDetail == null)
                //    throw new InvalidPluginExecutionException($"Category Detail 'Webbshop Privat' with Level 3 does not exist in system.");

                //WhoAmIResponse whoAmI = (WhoAmIResponse)localContext.OrganizationService.Execute(new WhoAmIRequest());
                //var user = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, whoAmI.UserId, new ColumnSet(SystemUserEntity.Fields.TransactionCurrencyId));
            }
        }

        [Test]
        public void CreateValueCodeGeneric()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                var valcodeApprov = new ValueCodeApprovalEntity { Id = Guid.Parse("40347b5a-dd1f-e911-827e-00155d010b00") };

                var query = new QueryExpression()
                {
                    EntityName = Skanetrafiken.Crm.Entities.CgiSettingEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(Skanetrafiken.Crm.Entities.CgiSettingEntity.Fields.ed_BlockTravelCardClearOnTemplateId)
                };
                var templateNumber = XrmRetrieveHelper.RetrieveFirst<Skanetrafiken.Crm.Entities.CgiSettingEntity>(localContext, query);

                var contact = new ContactEntity { Id = Guid.Parse("36347b5a-dd1f-e911-827e-00155d010b00") };

                //ValueCodeHandler.CreateValueCodeGeneric(localContext, "Utansaldo", 365, 500, Generated.ed_valuecode_ed_typeoption.Mobile, null, null, templateNumber.ed_BlockTravelCardClearOnTemplateId, contact, null,null,null,null,null,null,null,null,null,null,null,null,"0700158181");

            }
        }

        [Test]
        public void CreateValueCodeWithAssociatedTravelCard()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());


                var contact = new ContactEntity { Id = Guid.Parse("36347b5a-dd1f-e911-827e-00155d010b00") };

                var valueCode = new ValueCodeEntity()
                {
                    ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forlustgaranti,
                    ed_Amount = new Money(20),
                    ed_CustomImage = "",
                    ed_CustomText = "",
                    ed_LastRedemptionDate = DateTime.Now.AddDays(1),
                    ed_Status = "522362",
                    ed_TypeOption = Generated.ed_valuecode_ed_typeoption.Mobile,
                    ed_MobileNumber = "0700158181",
                    ed_Contact = contact.ToEntityReference(),
                };



            }
        }

        [Test, Explicit]
        public void updateVaucherCodeID()
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

                #region Voucher CSV Code

                //Path to .csv file
                string csvFile = @"C:\Users\CKAEND\Documents\CK\voucherid-and-vouchercode.csv";
                string filePathMissing = @"C:\Users\CKAEND\Documents\CK\failedVoucherUpdate.txt";


                var csvStringBuilder = new StringBuilder();

                localContext.TracingService.Trace(csvFile);

                CsvHelper.Configuration.CsvConfiguration config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    MissingFieldFound = null
                };

                Debug.WriteLine($"Working with file \"{System.IO.Path.GetFileName(csvFile)}\".");

                using (TextReader reader = new System.IO.StreamReader(csvFile, System.Text.Encoding.GetEncoding(1252), true))
                {
                    using (CsvReader csv = new CsvReader(reader, config))
                    {
                        csv.Read();
                        csv.ReadHeader();
                        int count = 0;
                        while (csv.Read())
                        {
                            count++;
                            string voucherIdCSV = csv["VoucherId"];
                            string voucherCodeCSV = csv["VoucherCode"];
                            try
                            {
                                if (string.IsNullOrWhiteSpace(voucherIdCSV))
                                {
                                    //Fail logg
                                    string newLine = String.Empty;
                                    newLine = $"Count: {count}; Found no Voucher Nr;";
                                    csvStringBuilder.AppendLine(newLine);
                                    File.WriteAllText(filePathMissing, csvStringBuilder.ToString());
                                    Debug.WriteLine($"Count: {count}; Found no Voucher Nr;");
                                }
                                else
                                {
                                    if (string.IsNullOrWhiteSpace(voucherCodeCSV))
                                    {
                                        //Fail logg
                                        string newLine = String.Empty;
                                        newLine = $"Count: {count}; Found no Voucher Code for VoucherId {voucherIdCSV};";
                                        csvStringBuilder.AppendLine(newLine);
                                        File.WriteAllText(filePathMissing, csvStringBuilder.ToString());
                                        Debug.WriteLine($"Count: {count}; Found no Voucher Code for VoucherId {voucherIdCSV};");
                                    }
                                    else
                                    {
                                        //Found both VoucherId and VoucherCode
                                        FilterExpression voucherFilter = new FilterExpression(LogicalOperator.And);
                                        voucherFilter.AddCondition(ValueCodeEntity.Fields.ed_CodeId, ConditionOperator.Equal, voucherCodeCSV);

                                        ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext,
                                            new ColumnSet(
                                                ValueCodeEntity.Fields.ed_name,
                                                ValueCodeEntity.Fields.ed_CodeId,
                                                ValueCodeEntity.Fields.ed_ValueCodeVoucherId
                                                ), voucherFilter);

                                        if (valueCode != null)
                                        {
                                            if (valueCode.ed_name?.ToLower() != voucherIdCSV.ToLower() || valueCode.ed_ValueCodeVoucherId?.ToLower() != voucherIdCSV.ToLower())
                                            {
                                                ValueCodeEntity updateValueCode = new ValueCodeEntity();
                                                updateValueCode.Id = valueCode.Id;
                                                updateValueCode.ed_name = voucherIdCSV.ToLower();
                                                updateValueCode.ed_ValueCodeVoucherId = voucherIdCSV.ToLower();
                                                XrmHelper.Update(localContext, updateValueCode);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine($"Count: {count}; ValueCode Missing; {voucherIdCSV}; {voucherCodeCSV};");
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                var newLine = $"Exception during process: {e.Message}; {voucherIdCSV}; {voucherCodeCSV};";
                                csvStringBuilder.AppendLine(newLine);
                                File.WriteAllText(filePathMissing, csvStringBuilder.ToString());
                                Debug.WriteLine($"Count: {count}; Exception during process: {e.Message}; {voucherIdCSV}; {voucherCodeCSV};");
                            }
                        }
                    }
                }

                #endregion
            }
        }

        //[Test]
        //public void CreateSimpleValueCode()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());
        //        ValueCodeTemplateEntity template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(true));


        //Guid valueCodeId = ValueCodeHandler.CreateValueCode(localContext, 100, Generated.ed_valuecode_ed_typeoption.Mobile, template: template);
        //Assert.NotNull(valueCodeId);

        //ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));

        //Assert.NotNull(valueCode.ed_Link);

        //        //XrmHelper.Delete(localContext, valueCode.ToEntityReference());

        //    }
        //}

        //[Test]
        //public void SendSMSValueCode()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

        //        LeadEntity lead = new LeadEntity()
        //        {
        //            MobilePhone = "+4670000000"
        //        };
        //        lead.Id = XrmHelper.Create(localContext, lead);

        //        string mobileNumberToUse = "+46735198846";
        //        //string mobileNumberToUse = "+46735198846";
        //        ValueCodeTemplateEntity template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(true));

        //Guid valueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 100, mobileNumberToUse, template: template, lead: lead);

        //ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //Assert.AreEqual((int)ValueCodeEntity.Status.Skapad, valueCode.statuscode.Value);
        //valueCode.SendValueCode(localContext);

        //valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //Assert.AreEqual((int)ValueCodeEntity.Status.Skickad, valueCode.statuscode.Value);

        //FilterExpression filter = new FilterExpression(LogicalOperator.And);
        //filter.AddCondition(TextMessageEntity.Fields.RegardingObjectId, ConditionOperator.Equal, valueCodeId);

        //IList<TextMessageEntity> textMessages = XrmRetrieveHelper.RetrieveMultiple<TextMessageEntity>(localContext, new ColumnSet(true), filter);

        //Assert.AreEqual(1, textMessages.Count());
        //Assert.AreEqual(mobileNumberToUse, textMessages.First().ed_PhoneNumber);

        //        //XrmHelper.Delete(localContext, valueCode.ToEntityReference());

        //    }
        //}

        //[Test]
        //public void SendValueCodeToContactByEmail()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

        //        ContactEntity contact = new ContactEntity()
        //        {
        //            FirstName = "TestContactFirstName",
        //            LastName = "TestContactLastName",
        //            EMailAddress1 = "hello@endeavor.se",
        //            ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
        //        };
        //        contact.Id = XrmHelper.Create(localContext, contact);
        //        string adressToUse = "marcus.stenswed@endeavor.se";

        //Guid valueCodeId = ValueCodeHandler.CreateEmailValueCode(localContext, 100, adressToUse, templateNumber: 3, contact: contact);

        //ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //valueCode.ed_TypeOption = Generated.ed_valuecode_ed_typeoption.Email;
        //XrmHelper.Update(localContext, valueCode);

        //valueCode.SendValueCode(localContext);

        //valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));

        //Assert.AreEqual((int)ValueCodeEntity.Status.Skickad, valueCode.statuscode.Value);

        //FilterExpression filter = new FilterExpression(LogicalOperator.And);
        //filter.AddCondition(EmailEntity.Fields.RegardingObjectId, ConditionOperator.Equal, valueCodeId);

        //IList<EmailEntity> emails = XrmRetrieveHelper.RetrieveMultiple<EmailEntity>(localContext, new ColumnSet(true), filter);

        //Assert.AreEqual(1, emails.Count());
        //Assert.AreEqual(adressToUse, emails.First().ToRecipients.Trim(new char[] { ';' }));

        //contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact.Id, new ColumnSet(true));
        //Assert.AreEqual("hello@endeavor.se", contact.EMailAddress1);

        //        //XrmHelper.Delete(localContext, valueCode.ToEntityReference());

        //    }
        //}

        //[Test]
        //public void SendValueCodeToLeadByEmail()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

        //        LeadEntity lead = new LeadEntity()
        //        {
        //            EMailAddress1 = "anewlead@endeavor.se"
        //        };
        //        lead.Id = XrmHelper.Create(localContext, lead);
        //        string adressToUse = "marcus.stenswed@endeavor.se";

        //Guid valueCodeId = ValueCodeHandler.CreateEmailValueCode(localContext, 100, adressToUse, templateNumber: 3, lead: lead);

        //ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //Assert.AreEqual(valueCode.ed_TypeOption.Value, Generated.ed_valuecode_ed_typeoption.Email);
        //valueCode.SendValueCode(localContext);

        //FilterExpression filter = new FilterExpression(LogicalOperator.And);
        //filter.AddCondition(EmailEntity.Fields.RegardingObjectId, ConditionOperator.Equal, valueCodeId);

        //IList<EmailEntity> emails = XrmRetrieveHelper.RetrieveMultiple<EmailEntity>(localContext, new ColumnSet(true), filter);

        //Assert.AreEqual(1, emails.Count());
        //Assert.AreEqual(adressToUse, emails.First().ToRecipients.Trim(new char[] {';'}));

        //lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, lead.Id, new ColumnSet(true));
        //Assert.AreEqual("anewlead@endeavor.se", lead.EMailAddress1);

        //        //XrmHelper.Delete(localContext, valueCode.ToEntityReference());

        //    }
        //}

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

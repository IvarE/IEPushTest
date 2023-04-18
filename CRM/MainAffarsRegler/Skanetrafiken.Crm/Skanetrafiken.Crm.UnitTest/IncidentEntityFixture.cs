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

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class IncidentEntityFixture : PluginFixtureBase
    {

        #region General data

        private const string cIncidentTicketNumber = "ST-000001-0001";
        private const string cMobileNumber = "0735198867"; //VC
        private const string cEmail = "vancarl.nguyen@endeavor.se";
        private const string cTravelCardNumber = "";

        #endregion

        private ServerConnection _serverConnection;

        /// <summary>
        /// See RefundEntity and IncidentEntity for reference.
        /// </summary>
        [Test]
        public void CreateIncidentAndRefund()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                var localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                TestDataHelper.DeleteIncidentAndRelatedRefundByTicketNumber(localContext, cIncidentTicketNumber);

                var incidentCase1 = new IncidentEntity();

                incidentCase1.Description = string.Empty;
                incidentCase1.Title = $"{DateTime.UtcNow.Date.ToString("dd/MM/yyyy")} : Resekortsersättning via värdekod";
                incidentCase1.CaseTypeCode = Generated.incident_casetypecode.ViewpointRequests;
                incidentCase1.CaseOriginCode = Generated.incident_caseorigincode.ResegarantiOnline;
                incidentCase1.PriorityCode = Generated.incident_prioritycode._2;
                incidentCase1.cgi_ActionDate = DateTime.UtcNow;
                incidentCase1.cgi_arrival_date = DateTime.UtcNow.Date;
                //incidentCase1.cgi_UnregisterdTravelCard = travelCardNumber ?? throw new InvalidPluginExecutionException($"Argument '{nameof(travelCardNumber)}' is empty.");
                incidentCase1.cgi_Contactid = new EntityReference(ContactEntity.EntityLogicalName, Guid.Parse("9e207ae1-27f9-e811-827c-00155d010b00"));//contact ?? throw new InvalidPluginExecutionException($"Argument '{nameof(contact)}' is empty."); //Fås av VCA
                incidentCase1.TicketNumber = cIncidentTicketNumber;
                incidentCase1.cgi_TelephoneNumber = cMobileNumber;
                incidentCase1.cgi_EmailAddress = "";
                incidentCase1.cgi_customer_email = "";
                incidentCase1.cgi_emailcount = "1";
                incidentCase1.IncidentStageCode = Generated.incident_incidentstagecode.Ongoing;

                #region Queries
                var contactQuery = new QueryExpression()
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_ContactNumber, ConditionOperator.Equal, "1"),
                            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Anonym")
                        }
                    }
                };

                var anonymContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);
                if (anonymContact == null)
                    throw new InvalidPluginExecutionException($"Could not find Contact 'Anonym' with number 1 in system.");


                var categoryDetailQuery = new QueryExpression()
                {
                    EntityName = CgiCategoryDetailEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_Level, ConditionOperator.Equal, "3"),
                            new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_categorydetailname, ConditionOperator.Equal, "Webbshop Privat")
                        }
                    }
                };

                var categoryDetail = XrmRetrieveHelper.RetrieveFirst<CgiCategoryDetailEntity>(localContext, categoryDetailQuery);
                if (categoryDetail == null)
                    throw new InvalidPluginExecutionException($"Could not find Category Detail 'Webbshop Privat' with Level 3 does not exist in system.");

                //This fetch is used for getting TransactionCurrency
                WhoAmIResponse whoAmI = (WhoAmIResponse)localContext.OrganizationService.Execute(new WhoAmIRequest());
                var user = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, whoAmI.UserId, new ColumnSet(SystemUserEntity.Fields.TransactionCurrencyId));
                if (user == null)
                    throw new InvalidPluginExecutionException($"Could not find SystemUser.");

                #endregion

                incidentCase1.CustomerId = anonymContact.ToEntityReference();
                incidentCase1.TransactionCurrencyId = user.TransactionCurrencyId;
                incidentCase1.cgi_casdet_row1_cat3Id = categoryDetail.ToEntityReference();

                incidentCase1.Id = XrmHelper.Create(localContext, incidentCase1);
                var fetchIncident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, incidentCase1.Id, new ColumnSet(true));




                var refund = new RefundEntity();

                refund.TransactionCurrencyId = user.TransactionCurrencyId; //SEK
                refund.ed_refundtype_print = false;
                refund.cgi_last_valid = DateTime.UtcNow.AddYears(2).Date;

                refund.cgi_ReInvoicing = false;
                refund.cgi_IsAutoGenerated = false;
                refund.cgi_CustomerMessage = "Spärra resekort via hemsidan.";
                refund.cgi_ExportedRaindance = false;
                refund.cgi_refundnumber = fetchIncident.TicketNumber;
                refund.cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, fetchIncident.Id);
                refund.cgi_EHOrderNumber = null; //Clearon om vi får ngt.
                refund.ed_TypeOfValueCode = Generated.ed_valuecodetype.Medsaldo;
                refund.ed_TypeOfRefunding = Generated.ed_refundingtype.Resekortsersattning;

                var deliveryMethod = 2;
                if (deliveryMethod == 1) //Email
                    refund.cgi_email = fetchIncident.cgi_EmailAddress == null ? fetchIncident.cgi_customer_email : fetchIncident.cgi_EmailAddress;
                else if (deliveryMethod == 2)
                    refund.cgi_MobileNumber = fetchIncident.cgi_TelephoneNumber != null ? fetchIncident.cgi_TelephoneNumber : null;
                else throw new InvalidPluginExecutionException($"Given delivery method does not exist.");


                #region Queries
                var refundAccQuery = new QueryExpression()
                {
                    EntityName = RefundAccountEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(RefundAccountEntity.Fields.cgi_refundaccountname, ConditionOperator.Equal, "Konto 53970"),
                            new ConditionExpression(RefundAccountEntity.Fields.cgi_Account, ConditionOperator.Equal, "53970")
                        }
                    }
                };
                var refundAcc = XrmRetrieveHelper.RetrieveFirst<RefundAccountEntity>(localContext, refundAccQuery);
                if (refundAcc == null)
                    throw new InvalidPluginExecutionException("Could not find Refund Account 'Konto 53970' in system.");


                var refundResQuery = new QueryExpression()
                {
                    EntityName = RefundResponsibleEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(RefundResponsibleEntity.Fields.cgi_name, ConditionOperator.Equal, "Tåg 88000"),
                            new ConditionExpression(RefundResponsibleEntity.Fields.cgi_responsible, ConditionOperator.Equal, "88000"),
                        }
                    }
                };
                var refundRes = XrmRetrieveHelper.RetrieveFirst<RefundResponsibleEntity>(localContext, refundResQuery);
                if (refundRes == null)
                    throw new InvalidPluginExecutionException("Couold not find Refund Responsible: 'Tåg 88000' in system");

                var reimbursementSmsQuery = new QueryExpression()
                {
                    EntityName = ReimbursementFormEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ReimbursementFormEntity.Fields.cgi_reimbursementname, ConditionOperator.Equal, "Värdekod - SMS")
                        }
                    }
                };

                var reimbursementEpostQuery = new QueryExpression()
                {
                    EntityName = ReimbursementFormEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ReimbursementFormEntity.Fields.cgi_reimbursementname, ConditionOperator.Equal, "Värdekod - E-post")
                        }
                    }
                };

                ReimbursementFormEntity reimbursementForm = null;

                //Värdekod - SMS or Värdekod - Email
                if (fetchIncident.cgi_TelephoneNumber != null)
                    reimbursementForm = XrmRetrieveHelper.RetrieveFirst<ReimbursementFormEntity>(localContext, reimbursementSmsQuery);
                else
                    reimbursementForm = XrmRetrieveHelper.RetrieveFirst<ReimbursementFormEntity>(localContext, reimbursementEpostQuery);

                if (reimbursementForm == null)
                    throw new InvalidPluginExecutionException("Could not find reimbursement form.");


                var refundTypeQuery = new QueryExpression()
                {
                    EntityName = RefundTypeEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(RefundTypeEntity.Fields.cgi_refundtypename, ConditionOperator.Equal, "Prisavdrag")
                        }
                    }
                };
                var refundType = XrmRetrieveHelper.RetrieveFirst<RefundTypeEntity>(localContext, refundTypeQuery);
                if (refundType == null)
                    throw new InvalidPluginExecutionException("Could not find Refund Type 'Prisavdrag in system'.");

                #endregion
                ////Note: If cgi_ReimbursementFormid is assigned in CRM, then CRM will try to create a real value code through biztalk and such. Therefore, this will fail in DKCRM.
                refund.cgi_ReimbursementFormid = reimbursementForm.ToEntityReference(); /*incident.cgi_TelephoneNumber != null ? new EntityReference(Generated.cgi_reimbursementform.EntityLogicalName, Guid.Parse("98f4cc92-5ea4-e811-8276-00155d010b00")) :
            new EntityReference(Generated.cgi_reimbursementform.EntityLogicalName, Guid.Parse("3fa843a0-5ea4-e811-8276-00155d010b00")); //Värdekod - SMS or Värdekod - Email*/

                //Refund Account: Konto 53970 and Refund Responsible: Tåg 880000 is used for correct bookeeping in their economy system.
                refund.cgi_Accountid = refundAcc.ToEntityReference();  //Konto 53970
                refund.cgi_responsibleId = refundRes.ToEntityReference();  //Tåg 88000
                refund.cgi_RefundTypeid = refundType.ToEntityReference(); //Prisavdrag

                if (decimal.TryParse("200", out var money))
                    refund.cgi_Amount = new Money(money);
                else throw new InvalidPluginExecutionException("Could not parse argument 'amount' to decimal.");

                refund.Id = XrmHelper.Create(localContext, refund);

                //var refund = new RefundEntity();

                //refund.cgi_RefundTypeid = new EntityReference(RefundEntity.EntityLogicalName, Guid.Parse("d52504d8-9e26-e611-80df-005056903a38")); //Prisavdrag
                //refund.TransactionCurrencyId = new EntityReference("transactioncurrency", Guid.Parse("7db294f9-53a1-e411-80d4-005056903a38")); //SEK
                //refund.ed_refundtype_print = false;

                ////Note: If cgi_ReimbursementFormid is assigned, then CRM will try to create a real value code through biztalk and such. Therefore, this will fail in DKCRM.
                //refund.cgi_ReimbursementFormid = new EntityReference(Generated.cgi_reimbursementform.EntityLogicalName, Guid.Parse("98f4cc92-5ea4-e811-8276-00155d010b00")); //SMS
                //refund.cgi_last_valid = DateTime.UtcNow.AddYears(1).Date;
                //refund.cgi_Accountid = new EntityReference(AccountEntity.EntityLogicalName, Guid.Parse("405ff9cb-7ab7-e411-80d3-005056904f1c")); //Konto 53970
                //refund.cgi_responsibleId = new EntityReference(AccountEntity.EntityLogicalName, Guid.Parse("efaf0e19-7ab7-e411-80d3-005056904f1c")); //Tåg 88000
                //refund.cgi_ReInvoicing = false;
                //refund.cgi_MobileNumber = "0735198867"; //depends on delivery
                //refund.cgi_email = null; //depends on del
                //refund.cgi_IsAutoGenerated = false;
                //refund.cgi_CustomerMessage = "Spärra resekort via hemsidan.";
                //refund.cgi_ExportedRaindance = false;
                //refund.cgi_refundnumber = fetchIncident.TicketNumber; //Från case
                //refund.cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, incidentCase1.Id);
                //refund.cgi_EHOrderNumber = null; //Clearon om vi får ngt.
                //refund.ed_TypeOfRefunding = Generated.ed_refundingtype.Arendeersattning;
                //refund.ed_TypeOfValueCode = Generated.ed_valuecodetype.Utansaldo;
                //refund.cgi_Amount = new Money(200);
                //refund.cgi_Contactid = incidentCase1.cgi_Contactid;

                //refund.Id = XrmHelper.Create(localContext, refund);
            }
        }

        [Test, Explicit]
        public void TestCreateIncident()
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

                IncidentEntity incident = new IncidentEntity();
                //incident.CaseOriginCode = new OptionSetValue(3);
                incident.cgi_sendtoqueue = new OptionSetValue(285050000);
                incident.Title = "";
                incident.cgi_Contactid = new EntityReference(ContactEntity.EntityLogicalName, new Guid("d3f19a93-28df-e611-80f8-00505690700f"));
                incident.CustomerId = new EntityReference(ContactEntity.EntityLogicalName, new Guid("d3f19a93-28df-e611-80f8-00505690700f"));
                incident.cgi_EmailAddress = "HT.st_test001@mailinator.com";
                incident.cgi_ContactCustomer = true;
                incident.cgi_emailcount = "1";
                incident.cgi_ActionDate = DateTime.Now;
                incident.Description = "Cert rollover!";

                localContext.OrganizationService.Create(incident);

                 localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test, Explicit]
        public void PopulateSocialSecurityNumberFormat()
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

                QueryExpression query = new QueryExpression
                {
                    EntityName = IncidentEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(IncidentEntity.Fields.cgi_soc_sec_number, IncidentEntity.Fields.ed_socialsecuritynumberformat),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(IncidentEntity.Fields.cgi_soc_sec_number, ConditionOperator.NotNull),
                            new ConditionExpression(IncidentEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.IncidentState.Active),
                            new ConditionExpression(IncidentEntity.Fields.CreatedOn, ConditionOperator.OnOrAfter, DateTime.UtcNow)
                        }
                    }
                };

                IList<IncidentEntity> casesWithSocialSecurityNumber = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, query);

                var tempHoldingSocSec = "";

                foreach(IncidentEntity incident in casesWithSocialSecurityNumber)
                {
                    tempHoldingSocSec = incident.cgi_soc_sec_number;
                    tempHoldingSocSec.Insert(8, "-");

                    incident.ed_socialsecuritynumberformat = tempHoldingSocSec;
                }

            }
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

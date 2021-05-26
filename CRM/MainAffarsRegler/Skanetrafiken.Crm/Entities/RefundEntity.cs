using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using Generated = Skanetrafiken.Crm.Schema.Generated;

using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using Skanetrafiken.Crm;
using System.Linq;
using Skanetrafiken.Crm.ValueCodes;
using System.Collections.Generic;
using Skanetrafiken.Crm.Schema.Generated;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Skanetrafiken.Crm.Entities
{
    public class EventSingapore
    {
        public string RgolIssueId { get; set; }
        public decimal Amount { get; set; }
        public string RefundType { get; set; }
        public string CompensationForm { get; set; }
    }

    public class RefundEntity : Generated.cgi_refund
    {
        private const string MobileValueCodeReimbursementFormIdString = "98F4CC92-5EA4-E811-8276-00155D010B00";
        private const string EmailValueCodeReimbursementFormIdString = "3FA843A0-5EA4-E811-8276-00155D010B00";

        public void HandlePostRefundCreateFOLog(Plugin.LocalPluginContext localContext)
        {
            if(this.cgi_Caseid == null)
            {
                localContext.Trace($"The Target 'Refund' does not have an associated 'Case'");
                return;
            }

            ColumnSet columnSet = new ColumnSet(IncidentEntity.Fields.cgi_RGOLIssueId);
            IncidentEntity eCase = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, this.cgi_Caseid.Id, columnSet);

            if(eCase == null)
            {
                localContext.Trace($"The Target 'Refund' does not have an associated 'Case'");
                return;
            }

            if(string.IsNullOrEmpty(eCase.cgi_RGOLIssueId))
            {
                localContext.Trace($"The Origin of the 'Case' is null or is not an RGOL Case.");
                return;
            }

            EventSingapore evSingapore = new EventSingapore();
            evSingapore.RgolIssueId = eCase.cgi_RGOLIssueId;
            evSingapore.Amount = this.cgi_Amount != null ? this.cgi_Amount.Value : 0.0M;
            evSingapore.CompensationForm = this.cgi_ReimbursementFormid != null ? this.cgi_ReimbursementFormid.Name : string.Empty;
            evSingapore.RefundType = this.cgi_RefundTypeid != null ? this.cgi_RefundTypeid.Name : string.Empty;

            string jsonEvent = JsonHelper.JsonSerializer<EventSingapore>(evSingapore);

            string ticketId = "";
            string clientId = "";
            string clientSecret = "";
            string webApiURl = "https://stticketmasterint.azurewebsites.net" + "/v2/tickets/" + ticketId + "/events";
            string token = AzureHelper.GetAccessToken(webApiURl, clientId, clientSecret);

            //TODO TASK 6988

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, webApiURl);
                httpRequest.Headers.Add("Authorization", token);

                httpRequest.Content = new StringContent(jsonEvent, Encoding.UTF8, "application/json");

                var response = httpClient.SendAsync(httpRequest).Result;
                string responseJSON = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    //error
                    //LantmannenFinanceErrorDTO fault = JsonHelper.JsonDeserialize<LantmannenFinanceErrorDTO>(responseJSON);
                    //actionResponse.error = fault;
                    //return actionResponse;
                }
                else
                {
                    //sucess
                }
            }


            var authInfo = ApiBase.GetAuth();
            Task.Run(() => ApiBase.Authenticate(authInfo, authInfo.TicketMaster)).Wait();


            //ApiBase._authResult.AccessToken





        }

        public void HandlePostRefundCreateAsync(Plugin.LocalPluginContext localContext)
        {
            //Guid MobileValueCodeReimbursementFormId = new Guid(MobileValueCodeReimbursementFormIdString);
            //Guid EmailValueCodeReimbursementFormId = new Guid(EmailValueCodeReimbursementFormIdString);

            //if (this.cgi_ReimbursementFormid != null && (this.cgi_ReimbursementFormid.Id.Equals(MobileValueCodeReimbursementFormId) || this.cgi_ReimbursementFormid.Id.Equals(EmailValueCodeReimbursementFormId)))
            //{
            //    IncidentEntity connectedCase = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, this.cgi_Caseid.Id, new ColumnSet(IncidentEntity.Fields.cgi_Contactid, IncidentEntity.Fields.CaseTypeCode, IncidentEntity.Fields.cgi_RGOLIssueId, IncidentEntity.Fields.TicketNumber));


            //    if (connectedCase == null || connectedCase.cgi_Contactid == null)
            //        return;


            //    ContactEntity contact = new ContactEntity()
            //    {
            //        Id = connectedCase.cgi_Contactid.Id
            //    };

            //    if(!connectedCase.CaseTypeCode.HasValue)
            //    {
            //        throw new Exception("Case must have a Case Type in order to choose which Value Code Template that will be used");
            //    }

            //    int templateNumber = (int)connectedCase.CaseTypeCode.Value % 10000;

            //    Guid valueCodeId;
            //    if(this.cgi_ReimbursementFormid.Id.Equals(MobileValueCodeReimbursementFormId))
            //    {
            //        valueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, (float)cgi_Amount.Value, cgi_MobileNumber, templateNumber: templateNumber, contact: contact, incident: connectedCase, refund: this, lastValid: this.cgi_last_valid);
            //    }
            //    else
            //    {
            //        valueCodeId = ValueCodeHandler.CreateEmailValueCode(localContext, (float)cgi_Amount.Value, cgi_email, templateNumber: templateNumber, contact: contact, incident: connectedCase, refund: this, lastValid: this.cgi_last_valid);
            //    }

            //    if (valueCodeId == null)
            //        throw new InvalidPluginExecutionException("Could not create value code from refund.");

            //    // Trigger workflow to send Value Code to customer
            //    ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
            //    valueCode.SendValueCode(localContext);

            //}
        }

        public static RefundEntity CreateRefundAfterOnlineRefund(Plugin.LocalPluginContext localContext, IncidentEntity incident, int deliveryMethod, Money amount)
        {

            if (incident == null)
                throw new InvalidPluginExecutionException($"Argument '{nameof(incident)}' is empty.");

            //This fetch is used for getting TransactionCurrency
            WhoAmIResponse whoAmI = (WhoAmIResponse)localContext.OrganizationService.Execute(new WhoAmIRequest());
            var user = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, whoAmI.UserId, new ColumnSet(SystemUserEntity.Fields.TransactionCurrencyId));
            if (user == null)
                throw new InvalidPluginExecutionException($"Could not find SystemUser.");

            var refund = new RefundEntity();

            refund.TransactionCurrencyId = user.TransactionCurrencyId; //SEK
            refund.ed_refundtype_print = false;

            //Changed to add 24 moths instead of 12 (DevOps - 8059)
            refund.cgi_last_valid = DateTime.UtcNow.AddYears(2).Date;

            refund.cgi_ReInvoicing = false;
            refund.cgi_IsAutoGenerated = false;
            refund.cgi_CustomerMessage = "Spärra resekort via hemsidan.";
            refund.cgi_ExportedRaindance = false;
            refund.cgi_refundnumber = incident.TicketNumber;
            refund.cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, incident.Id);
            refund.cgi_EHOrderNumber = null; //Clearon om vi får ngt.
            refund.ed_TypeOfValueCode = Generated.ed_valuecodetype.Medsaldo;
            refund.ed_TypeOfRefunding = Generated.ed_refundingtype.Resekortsersattning;
            refund.cgi_Amount = amount;


            if (deliveryMethod == 1) //Email
                refund.cgi_email = incident.cgi_EmailAddress == null ? incident.cgi_customer_email : incident.cgi_EmailAddress;
            else if (deliveryMethod == 2)
                refund.cgi_MobileNumber = incident.cgi_TelephoneNumber != null ? incident.cgi_TelephoneNumber : null;
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
            if (incident.cgi_TelephoneNumber != null)
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
            if(refundType == null)
                throw new InvalidPluginExecutionException("Could not find Refund Type 'Prisavdrag in system'.");

            #endregion

            refund.cgi_ReimbursementFormid = reimbursementForm.ToEntityReference(); //Värdekod - SMS or Värdekod - Email*/

            //Refund Account: Konto 53970 and Refund Responsible: Tåg 880000 is used for correct bookeeping in their economy system.
            refund.cgi_Accountid = refundAcc.ToEntityReference();  //Konto 53970
            refund.cgi_responsibleId = refundRes.ToEntityReference();  //Tåg 88000
            refund.cgi_RefundTypeid = refundType.ToEntityReference(); //Prisavdrag

            refund.Id = XrmHelper.Create(localContext, refund);

            return refund;
        }

        internal void SetTransactionFailed(Plugin.LocalPluginContext localContext, string errorMessage)
        {
            if(errorMessage.Contains("Bad format on phone number"))
            {
                this.cgi_errormessage = "Felaktigt format på kunds mobilnummer";
            }
            else if (errorMessage.Contains("Object reference not set to an instance of an object"))
            {
                this.cgi_errormessage = "Oväntat fel. Försök skapa beslut igen, annars kontakta en systemadministratör";
            }
            else
            {
                if (errorMessage.Length > 100)
                    this.cgi_errormessage = "Kontakta en systemadminitratör Fel:" + errorMessage.Substring(0, 100);
                else
                    this.cgi_errormessage = "Kontakta en systemadminitratör Fel:" + errorMessage;
            }

            this.statuscode = Generated.cgi_refund_statuscode.Declined;
            XrmHelper.Update(localContext, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="refundId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static RefundEntity GetRefundById(Plugin.LocalPluginContext localContext, Guid refundId, ColumnSet column = null)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(GetRefundById)}.");

            if (refundId == null || refundId == Guid.Empty)
                localContext.TracingService.Trace($"Argument '{nameof(refundId)}' is empty.");

            var query = new QueryExpression()
            {
                EntityName = RefundEntity.EntityLogicalName,
                ColumnSet = column ?? new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RefundEntity.Fields.Id, ConditionOperator.Equal, refundId)
                    }
                }
            };

            var refund = XrmRetrieveHelper.RetrieveFirst<RefundEntity>(localContext, query);
            if(refund == null)
                localContext.TracingService.Trace($"Could not find refund based on id '{refundId}'.");

            localContext.TracingService.Trace($"<--- Exiting {nameof(GetRefundById)}.");

            return refund;
        }
    }
}

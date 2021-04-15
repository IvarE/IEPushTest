using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using System.Text.RegularExpressions;
using Skanetrafiken.Crm.ValueCodes;
using System.Linq;

namespace Skanetrafiken.Crm.Entities
{
    public class IncidentEntity : Generated.Incident
    {

        public static IncidentEntity CreateCaseAfterOnlineRefund(Plugin.LocalPluginContext localContext, string travelCardNumber, string mobile, string email, EntityReference contact)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mobile) && string.IsNullOrWhiteSpace(email))
                    throw new InvalidPluginExecutionException($"Arguments '{nameof(mobile)} and {nameof(email)}' cannot be empty at the same time.");

                var incidentCase = new IncidentEntity();

                incidentCase.Description = string.Empty;
                incidentCase.Title = $"{DateTime.UtcNow.Date.ToString("dd/MM/yyyy")} : Resekortsersättning via värdekod";
                incidentCase.CaseTypeCode = Generated.incident_casetypecode.ViewpointRequests;
                incidentCase.CaseOriginCode = Generated.incident_caseorigincode.ResegarantiOnline;
                incidentCase.PriorityCode = Generated.incident_prioritycode._2;
                incidentCase.cgi_ActionDate = DateTime.UtcNow;
                incidentCase.cgi_arrival_date = DateTime.UtcNow.Date;
                incidentCase.cgi_UnregisterdTravelCard = travelCardNumber ?? throw new InvalidPluginExecutionException($"Argument '{nameof(travelCardNumber)}' is empty.");
                incidentCase.cgi_Contactid = contact ?? throw new InvalidPluginExecutionException($"Argument '{nameof(contact)}' is empty.");

                incidentCase.cgi_TelephoneNumber = mobile;
                incidentCase.cgi_EmailAddress = email;
                incidentCase.cgi_customer_email = email;
                incidentCase.cgi_emailcount = "1";
                incidentCase.IncidentStageCode = Generated.incident_incidentstagecode.Ongoing;

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

                incidentCase.CustomerId = anonymContact.ToEntityReference();
                incidentCase.TransactionCurrencyId = user.TransactionCurrencyId;
                incidentCase.cgi_casdet_row1_cat3Id = categoryDetail.ToEntityReference();

                incidentCase.Id = XrmHelper.Create(localContext, incidentCase);

                return incidentCase;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IncidentEntity CreateCaseForTravelCardValueCodeExchange(Plugin.LocalPluginContext localContext, string travelCardNumber, decimal amount, ContactEntity contact,
                                                                                string emailaddress, string mobile, int deliveryMethod, TravelCardEntity travelCard)
        {
            IncidentEntity incident = new IncidentEntity();

            incident.Title = "Byte reskassa";
            incident.CaseTypeCode = Generated.incident_casetypecode.ViewpointRequests;
            incident.CaseOriginCode = Generated.incident_caseorigincode.WebSkanetrafikense;
            incident.cgi_ActionDate = DateTime.Now;
            incident.cgi_UnregisterdTravelCard = travelCardNumber;
            if (travelCard != null)
                incident.cgi_TravelCardid = travelCard.ToEntityReference();

            //Email
            if (deliveryMethod == 1)
                incident.Description = "Utskick av värdekod via epost.";
            else if (deliveryMethod == 2) incident.Description = "Utskick av värdekod via SMS.";
            else incident.Description = "";

            if (contact == null)
            {
                incident.cgi_Contactid = ContactEntity.GetAnonymousContact(localContext).ToEntityReference();
                incident.CustomerId = ContactEntity.GetAnonymousContact(localContext).ToEntityReference();
            }
            else
            {
                incident.cgi_Contactid = contact.ToEntityReference();
                incident.CustomerId = contact.ToEntityReference();
            }
                

            incident.cgi_customer_email = emailaddress;
            incident.cgi_customer_telephonenumber = mobile;

            #region Kategori 3
            var categoryDetailQuery = new QueryExpression()
            {
                EntityName = CgiCategoryDetailEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_Level, ConditionOperator.Equal, "3"),
                        new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_categorydetailname, ConditionOperator.Equal, "Återbetalning reskassa")
                    },
                }
            };

            CgiCategoryDetailEntity categoryDetail = XrmRetrieveHelper.RetrieveFirst<CgiCategoryDetailEntity>(localContext, categoryDetailQuery);

            if (categoryDetail != null)
                incident.cgi_casdet_row1_cat3Id = categoryDetail.ToEntityReference();

            categoryDetailQuery.Criteria = new FilterExpression
            {
                Conditions = {
                       new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_Level, ConditionOperator.Equal, "2"),
                       new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_categorydetailname, ConditionOperator.Equal, "Produkter")
                }
            };

            categoryDetail = XrmRetrieveHelper.RetrieveFirst<CgiCategoryDetailEntity>(localContext, categoryDetailQuery);
            if (categoryDetail != null)
                incident.cgi_casdet_row1_cat2Id = categoryDetail.ToEntityReference();

            categoryDetailQuery.Criteria = new FilterExpression
            {
                Conditions = {
                       new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_Level, ConditionOperator.Equal, "1"),
                       new ConditionExpression(CgiCategoryDetailEntity.Fields.cgi_categorydetailname, ConditionOperator.Equal, "Priser - Produkter - Villkor")
                }
            };

            categoryDetail = XrmRetrieveHelper.RetrieveFirst<CgiCategoryDetailEntity>(localContext, categoryDetailQuery);
            if (categoryDetail != null)
                incident.cgi_casdet_row1_cat1Id = categoryDetail.ToEntityReference();

            #endregion

            Guid incidentId = XrmHelper.Create(localContext, incident);
            incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, incidentId, new ColumnSet(true));

            // Add Incident to Queue
            EntityReference defaultQueue = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.ed_DefaultQueueValueCodes);

            AddToQueueRequest queueReq = new AddToQueueRequest()
            {
                DestinationQueueId = defaultQueue.Id,
                Target = incident.ToEntityReference()
            };

            // Execute Queue request
            localContext.OrganizationService.Execute(queueReq);
            
            // Trigga bekräftelseutskick via Epost
            if (deliveryMethod == 1)
            {
                ValueCodeTemplateEntity template = ValueCodeHandler.GetValueCodeTemplate(localContext, null, 21);

                // TODO : Can't use EmailValueCodeSender without ValueCodeEntity...
                
                TemplateEntity emailTemplate = template.GetEmailTemplate(localContext);

                EntityReference defaultSender = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.ed_DefaultSenderValueCodes);
                if (defaultSender != null && defaultSender.Id != null)
                {
                    // Initiate email
                    EmailEntity email = emailTemplate.InstantiateTemplate(localContext, incident.ToEntityReference());

                    // Set RegardingObject to Incident
                    email.RegardingObjectId = incident.ToEntityReference();

                    // Set Sender
                    ActivityPartyEntity[] sender = new ActivityPartyEntity[]
                    {
                        new ActivityPartyEntity()
                        {
                            PartyId = defaultSender
                        }
                    };
                    email.From = sender;

                    // Set Receiver
                    ActivityPartyEntity[] receiver;

                    // If emailaddress is entered, use this one.
                    if (emailaddress != null && emailaddress != "")
                    {
                        receiver = new ActivityPartyEntity[]
                        {
                            new ActivityPartyEntity()
                            {
                                AddressUsed = emailaddress
                            }
                        };
                        email.To = receiver;
                    }
                    else if (contact != null)
                    {
                        receiver = new ActivityPartyEntity[]
                        {
                            new ActivityPartyEntity()
                            {
                                PartyId = contact.ToEntityReference()
                            }
                        };
                        email.To = receiver;
                    }

                    // Create and Send Email
                    XrmHelper.CreateAndSendEmail(localContext, email, IssueSend: true);
                }
                
            }
            // Trigga bekräftelseutskick via SMS
            else if (deliveryMethod == 2)
            {
                ValueCodeTemplateEntity template = ValueCodeHandler.GetValueCodeTemplate(localContext, null, 21);

                TextMessageEntity textMessage = new TextMessageEntity();

                textMessage.RegardingObjectId = incident.ToEntityReference();
                textMessage.Description = template.ed_ValueCodeText;
                textMessage.Subject = template.ed_subject;
                textMessage.ed_PhoneNumber = mobile;
                textMessage.ed_SenderName = "Skanetrafik";

                EntityReference defaultSender = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.ed_DefaultSenderValueCodes);

                // Set Sender
                ActivityPartyEntity[] sender = new ActivityPartyEntity[]
                {
                        new ActivityPartyEntity()
                        {
                            PartyId = defaultSender
                        }
                };
                textMessage.From = sender;


                // Set Receiver
                ActivityPartyEntity[] receiver;

                // If emailaddress is entered, use this one.
                if (contact != null)
                {
                    receiver = new ActivityPartyEntity[]
                    {
                        new ActivityPartyEntity()
                        {
                            PartyId = contact.ToEntityReference()
                        }
                    };
                    textMessage.To = receiver;
                }

                Guid smsId = XrmHelper.Create(localContext.OrganizationService, textMessage);
                textMessage = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, smsId, new ColumnSet(true));

                textMessage.SendTextMessage(localContext);
            }

            return incident;
        }
        public void ReOpenCase(Plugin.LocalPluginContext localContext, Generated.IncidentState incidentState)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(ReOpenCase)}.");

            SetStateRequest state = new SetStateRequest();
            state.EntityMoniker = new EntityReference("incident", this.Id);
            state.State = new OptionSetValue((int)incidentState);
            state.Status = new OptionSetValue(1); // In Progress
            SetStateResponse stateSet = (SetStateResponse)localContext.OrganizationService.Execute(state);

            localContext.TracingService.Trace($"<--- Exiting {nameof(ReOpenCase)}.");
        }

        public void CloseCase(Plugin.LocalPluginContext localContext, string testInstanceName)
        {


            Entity IncidentResolution = new Entity("incidentresolution");
            IncidentResolution.Attributes["subject"] = "Subject Closed";
            IncidentResolution.Attributes["incidentid"] = new EntityReference("incident", this.Id);
            // Create the request to close the incident, and set its resolution to the
            // resolution created above
            CloseIncidentRequest closeRequest = new CloseIncidentRequest();
            closeRequest.IncidentResolution = IncidentResolution;
            // Set the requested new status for the closed Incident
            closeRequest.Status = new OptionSetValue(5);
            // Execute the close request
            CloseIncidentResponse closeResponse = (CloseIncidentResponse)localContext.OrganizationService.Execute(closeRequest);
        }

        public static IncidentEntity GetIncidentByRefundCaseId(Plugin.LocalPluginContext localContext, Guid caseId, ColumnSet column = null)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(GetIncidentByRefundCaseId)}.");
            var query = new QueryExpression()
            {
                EntityName = IncidentEntity.EntityLogicalName,
                ColumnSet = column ?? new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(IncidentEntity.Fields.Id, ConditionOperator.Equal, caseId)
                    }
                }
            };

            var incident = XrmRetrieveHelper.RetrieveFirst<IncidentEntity>(localContext, query);
            if(incident == null)
                localContext.TracingService.Trace($"Could not find incident based on id '{caseId}'.");
            localContext.TracingService.Trace($"<--- Exiting {nameof(GetIncidentByRefundCaseId)}.");

            return incident;
        }
    }
}
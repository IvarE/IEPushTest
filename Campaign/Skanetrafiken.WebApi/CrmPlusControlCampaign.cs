using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Endeavor.Crm.Extensions;
using System.Web;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Newtonsoft.Json;
using System.Web.Caching;
using Skanetrafiken.Crm.Properties;
using Microsoft.Xrm.Sdk.Client;
//using Microsoft.Web.Administration;


namespace Skanetrafiken.Crm.Controllers
{
    public class CrmPlusControlCampaign : CrmPlusControl
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static HttpResponseMessage RetrieveLeadInfo(string campaignCode)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();

                IList<LeadEntity> getLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, campaignCode.ToLower())
                        }
                    });
                if (getLeads.Count == 0){
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                    rm.Content = new StringContent(Resources.CampaignCodeError);
                    return rm;
                } else if (getLeads.Count > 1)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(Resources.MultipleLeadsFound);
                    return rm;
                } else
                {
                    LeadEntity lead = getLeads.First();
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    LeadInfo info = (LeadInfo)lead.ToLeadInfo(localContext);

                    response.Content = new StringContent(JsonConvert.SerializeObject(info));
                    return response;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage QualifyLeadToUnvalidatedCustomer(LeadInfo leadInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                if (leadInfo == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return respMess;
                }

                // Validate info
                _log.DebugFormat("QualifyCampaignLead, validating info");
                leadInfo.Source = (int)CustomerUtility.Source.Kampanj;
                CampaignEntity campaign = null;
                StatusBlock validateStatus = CustomerUtility.ValidateLeadInfo(localContext, leadInfo, ref campaign);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(validateStatus.ErrorMessage);
                    return rm;
                }

                //Find existing leads using same campaign
                IList<LeadEntity> activeLeads = LeadEntity.FindExistingCampaignLeads(localContext, leadInfo);
                if (activeLeads.Count < 1)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.CampaignCodeError);
                    return rm;
                }
                foreach (LeadEntity lead in activeLeads)
                {
                    //Will check after matching campaigns and also check if it found itself so tests will work.
                    if (lead.CampaignId.Id == campaign.CampaignId && lead.Id != new Guid(leadInfo.Guid))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.CampaignCodeError);
                        return rm;
                    }
                }

                QualifyLeadRequest req = new QualifyLeadRequest
                {
                    CreateAccount = false,
                    CreateContact = false,
                    CreateOpportunity = false,
                    LeadId = new EntityReference(LeadEntity.EntityLogicalName, new Guid(leadInfo.Guid)),
                    Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
                };
                ContactEntity activeContact = ContactEntity.FindActiveContact(localContext, leadInfo);
                if (activeContact == null)
                    req.CreateContact = true;
                
                QualifyLeadResponse resp = (QualifyLeadResponse)localContext.OrganizationService.Execute(req);

                if (activeContact == null)
                {
                    activeContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, resp.CreatedEntities.FirstOrDefault().Id)
                            }
                        });
                    if (activeContact == null)
                    {
                        throw new Exception(string.Format("Lead validation failed. No resulting contact found"));
                    }
                }
                else
                {
                    ContactEntity updateEntity = null;
                    if ((!string.IsNullOrWhiteSpace(activeContact.cgi_socialsecuritynumber) && activeContact.cgi_socialsecuritynumber.Equals(leadInfo.SocialSecurityNumber)) ||
                        (!string.IsNullOrWhiteSpace(activeContact.EMailAddress1) && activeContact.EMailAddress1.Equals(leadInfo.Email)))
                    {
                        updateEntity = activeContact.CreateUpdateEntityFromInfo(localContext, leadInfo);
                    }
                    else
                    {
                        updateEntity = activeContact.CreateUpdateEntityFromAuthorityInfo(localContext, leadInfo);
                    }
                    if (updateEntity != null)
                    {
                        XrmHelper.Update(localContext, updateEntity);
                        activeContact.CombineAttributes(updateEntity);
                    }
                }

                HttpResponseMessage goodResponse = new HttpResponseMessage(HttpStatusCode.OK);
                goodResponse.Content = new StringContent(SerializeNoNull(activeContact.ToCustomerInfo(localContext)));
                return goodResponse;
            }
            catch (Exception e)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                badResponse.Content = new StringContent(Resources.ContactNotCreated + "Mer info: " + e);
                return badResponse;
            }

        }

        internal static HttpResponseMessage PostLeadAndQualifyToContact(LeadInfo leadInfo)
        {
            Plugin.LocalPluginContext localContext = GenerateContext();

            if (leadInfo == null)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badResponse.Content = new StringContent(Resources.ContactNotCreated);
                return badResponse;
            }
            try
            {
                // Validate Info and return error message if faulty Info
                CampaignEntity campaign = null;
                StatusBlock validateLeadStatus = CustomerUtility.ValidateLeadInfo(localContext, leadInfo, ref campaign);
                if (!validateLeadStatus.TransactionOk)
                {
                    HttpResponseMessage invalidInfoMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    invalidInfoMess.Content = new StringContent(validateLeadStatus.ErrorMessage);
                    return invalidInfoMess;
                }

                //Find existing leads using same campaign
                IList<LeadEntity> activeLeads = LeadEntity.FindExistingCampaignLeads(localContext, leadInfo);
                if (activeLeads.Count > 0)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.CampaignCodeError);
                    return rm;
                }

                //If lead does not exists, create a new one
                LeadEntity newLead = LeadEntity.CreateLead(localContext, leadInfo, campaign.ed_LeadTopic.Value.ToString(), campaign);
                leadInfo.Guid = newLead.Id.ToString();

                //Update total leads field
                IList<LeadEntity> campaignConnectedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false), new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id)
                    }
                });
                if (campaign.ed_TotalLeads != campaignConnectedLeads.Count)
                {
                    CampaignEntity updateEntity = new CampaignEntity
                    {
                        Id = campaign.Id,
                        ed_TotalLeads = campaignConnectedLeads.Count
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEntity);
                }

                ////Update lead source and lead topic - (teo) This is done in Lead and Campaign Plugins
                //LeadEntity.UpdateLeadSourceAndLeadTopic(localContext, campaign, campaignConnectedLeads);

                QualifyLeadRequest req = new QualifyLeadRequest
                {
                    CreateAccount = false,
                    CreateContact = false,
                    CreateOpportunity = false,
                    LeadId = new EntityReference(LeadEntity.EntityLogicalName, new Guid(leadInfo.Guid)),
                    Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
                };
                ContactEntity activeContact = ContactEntity.FindActiveContact(localContext, leadInfo);
                if (activeContact == null)
                    req.CreateContact = true;

                QualifyLeadResponse resp = (QualifyLeadResponse)localContext.OrganizationService.Execute(req);

                if (activeContact == null)
                {
                    activeContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, resp.CreatedEntities.FirstOrDefault().Id)
                            }
                        });
                    if (activeContact == null)
                    {
                        throw new Exception(string.Format("Lead validation failed. No resulting contact found"));
                    }
                    ContactEntity updateEntity = new ContactEntity
                    {
                        Id = activeContact.Id,
                        EMailAddress2 = activeContact.EMailAddress1,
                        EMailAddress1 = null
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEntity);

                }
                else
                {
                    ContactEntity updateEntity = null;
                    if ((!string.IsNullOrWhiteSpace(activeContact.cgi_socialsecuritynumber) && activeContact.cgi_socialsecuritynumber.Equals(leadInfo.SocialSecurityNumber)) ||
                        (!string.IsNullOrWhiteSpace(activeContact.EMailAddress1) && activeContact.EMailAddress1.Equals(leadInfo.Email)))
                    {
                        updateEntity = activeContact.CreateUpdateEntityFromInfo(localContext, leadInfo);
                    }
                    else
                    {
                        updateEntity = activeContact.CreateUpdateEntityFromAuthorityInfo(localContext, leadInfo);
                    }
                    if (updateEntity != null)
                    {
                        XrmHelper.Update(localContext, updateEntity);
                        activeContact.CombineAttributes(updateEntity);
                    }
                }

                HttpResponseMessage goodResponse = new HttpResponseMessage(HttpStatusCode.OK);
                goodResponse.Content = new StringContent(SerializeNoNull(activeContact.ToCustomerInfo(localContext)));
                return goodResponse;
            }
            catch (Exception e)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                badResponse.Content = new StringContent(Resources.ContactNotCreated + "Mer info: " + e);
                return badResponse;
            }
        }



    }
}
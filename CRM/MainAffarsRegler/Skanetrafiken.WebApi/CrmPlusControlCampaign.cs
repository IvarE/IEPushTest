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
using System.Threading;
//using Microsoft.Web.Administration;


namespace Skanetrafiken.Crm.Controllers
{
    public class CrmPlusControlCampaign : CrmPlusControl
    {
        private static readonly AppInsightsLogger _logger = new AppInsightsLogger();
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        internal static HttpResponseMessage RetrieveLeadInfo(int threadId, string campaignCode, string prefix)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", prefix);

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {

                        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                        IList<LeadEntity> getLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(
                          localContext,
                          new QueryExpression()
                          {
                              EntityName = LeadEntity.EntityLogicalName,
                              ColumnSet = LeadEntity.LeadInfoBlock,
                              Criteria =
                              {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, campaignCode.ToLower())
                                }
                              },
                              LinkEntities =
                              {
                                new LinkEntity()
                                {
                                    LinkFromEntityName = LeadEntity.EntityLogicalName,
                                    LinkToEntityName = CampaignEntity.EntityLogicalName,
                                    LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                    LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                    EntityAlias = CampaignEntity.EntityLogicalName,
                                    JoinOperator = JoinOperator.Inner,
                                    Columns = new ColumnSet(
                                        CampaignEntity.Fields.StateCode,
                                        CampaignEntity.Fields.ed_ValidFromPhase1,
                                        CampaignEntity.Fields.ed_ValidToPhase1,
                                        CampaignEntity.Fields.ed_ValidFromPhase2,
                                        CampaignEntity.Fields.ed_ValidToPhase2)
                                }
                              }
                          });


                        if (getLeads.Count == 0)
                        {
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                            rm.Content = new StringContent(Resources.CampaignCodeError);
                            return rm;
                        }
                        else if (getLeads.Count > 1)
                        {
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            rm.Content = new StringContent(Resources.MultipleLeadsFound);
                            return rm;
                        }
                        else
                        {
                            LeadEntity lead = getLeads.First();

                            #region CampaignChecks
                            bool isCampaignActive = false;
                            DateTime validFromPhase1 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidFromPhase1).ToLocalTime();
                            DateTime validToPhase1 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidToPhase1).ToLocalTime();
                            DateTime validFromPhase2 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidFromPhase2).ToLocalTime();
                            DateTime validToPhase2 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidToPhase2).ToLocalTime();
                            if (validFromPhase1 != null && validToPhase1 != null)
                            {

                                DateTime universalDateNow = DateTime.Now.ToLocalTime();

                                if (universalDateNow.CompareTo(validFromPhase1) > 0 && universalDateNow.CompareTo(validToPhase1.AddDays(1)) < 0)
                                {
                                    isCampaignActive = true;
                                }
                                else if (validFromPhase2 != null && validToPhase2 != null)
                                {
                                    if (universalDateNow.CompareTo(validFromPhase2) > 0 && universalDateNow.CompareTo(validToPhase2.AddDays(1)) < 0)
                                    {
                                        isCampaignActive = true;
                                    }
                                }
                            }
                            else
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                rm.Content = new StringContent(string.Format(Resources.CampaignIncomplete, CampaignEntity.Fields.ed_ValidFromPhase1 + " och " + CampaignEntity.Fields.ed_ValidToPhase1));
                                return rm;
                            }

                            if (!isCampaignActive)
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm.Content = new StringContent(Resources.CampaignInvalid);
                                return rm;
                            }
                            #endregion

                            #region LeadChecks
                            if (Generated.LeadState.Qualified.Equals(lead.StateCode))
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm.Content = new StringContent(Resources.CampaignCodeUsed);
                                return rm;
                            }
                            else if (Generated.LeadState.Disqualified.Equals(lead.StateCode))
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm.Content = new StringContent(Resources.CampaignCodeTooOld);
                                return rm;
                            }
                            #endregion

                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                            LeadInfo info = (LeadInfo)lead.ToLeadInfo(localContext);

                            response.Content = new StringContent(JsonConvert.SerializeObject(info));
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    return rm;
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        internal static HttpResponseMessage QualifyLeadToUnvalidatedCustomer(int threadId, LeadInfo leadInfo, string prefix)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", prefix);

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {

                        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                        if (leadInfo == null)
                        {
                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                            return respMess;
                        }
                        if (string.IsNullOrWhiteSpace(leadInfo.Guid))
                        {
                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            respMess.Content = new StringContent(Resources.GuidMissing);
                            return respMess;
                        }
                        // retrieve existing
                        LeadEntity existingLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                            new FilterExpression
                            {
                                Conditions =
                                {
                                new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, new Guid(leadInfo.Guid))
                                }
                            });
                        if (existingLead == null)
                        {
                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            respMess.Content = new StringContent(Resources.GuidNotValid);
                            return respMess;
                        }

                        // Combine incoming info with existing.
                        LeadInfo existingLeadInfo = existingLead.ToLeadInfo(localContext);
                        CrmPlusUtility.CombineLeadInfos(localContext, leadInfo, existingLeadInfo);

                        // Validate info
                        leadInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.Kampanj;
                        CampaignEntity campaign = null;
                        StatusBlock validateStatus = CustomerUtility.ValidateLeadInfo(localContext, leadInfo, ref campaign);
                        if (!validateStatus.TransactionOk)
                        {
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rm.Content = new StringContent(validateStatus.ErrorMessage);
                            return rm;
                        }

                        LeadEntity updateLead = new LeadEntity(localContext, leadInfo);
                        updateLead.Id = existingLead.Id;
                        try
                        {
                            XrmHelper.Update(localContext, updateLead);
                        }
                        catch (Exception ex)
                        {
                            _exceptionCustomProperties["source"] = prefix;
                            _logger.LogException(ex, _exceptionCustomProperties);

                            if ("There is already a lead with the same Social Security Number in the assigned Campaign".Equals(ex.Message))
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm.Content = new StringContent(Resources.CampaignLeadConflict);
                                return rm;
                            }
                            if ("There is already a lead with the same Name and Email in the assigned Campaign".Equals(ex.Message))
                            {
                                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm.Content = new StringContent(Resources.CampaignLeadConflict);
                                return rm;
                            }
                            throw ex;
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

                            ContactEntity updateContact = new ContactEntity
                            {
                                Id = activeContact.Id,
                                EMailAddress2 = activeContact.EMailAddress1,
                                EMailAddress1 = null
                            };
                            XrmHelper.Update(localContext, updateContact);
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

                        // Create Campaign Response
                        CampaignResponseEntity response = new CampaignResponseEntity
                        {
                            Subject = $"{activeContact.FullName} har svarat på {campaign.Name}",
                            RegardingObjectId = campaign.ToEntityReference(),
                            Customer = new List<ActivityPartyEntity>
                        {
                            new ActivityPartyEntity() {
                                PartyId = activeContact.ToEntityReference()
                            }
                        },
                            PromotionCodeName = leadInfo.CampaignCode,
                            ReceivedOn = DateTime.Now
                        };
                        XrmHelper.Create(localContext, response);

                        //AddItemCampaignRequest campaignResponseReq = new AddItemCampaignRequest
                        //{
                        //    CampaignId = campaign.Id,
                        //    EntityId = activeContact.Id,
                        //    EntityName = ContactEntity.EntityLogicalName
                        //};
                        //localContext.OrganizationService.Execute(campaignResponseReq);

                        HttpResponseMessage goodResponse = new HttpResponseMessage(HttpStatusCode.OK);
                        goodResponse.Content = new StringContent(SerializeNoNull(activeContact.ToCustomerInfo(localContext)));
                        return goodResponse;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    return rm;
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        internal static HttpResponseMessage PostLeadAndQualifyToContact(int threadId, LeadInfo leadInfo, string prefix)
        {
            _logger.SetGlobalProperty("source", prefix);

            if (leadInfo == null)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string errorMessage = Resources.ContactNotCreated;
                badResponse.Content = new StringContent(errorMessage);

                _logger.LogError($"PostLeadAndQualifyToContact: Failed - {errorMessage}");

                return badResponse;
            }
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    // Validate Info and return error message if faulty Info
                    CampaignEntity campaign = null;
                    StatusBlock validateLeadStatus = CustomerUtility.ValidateLeadInfo(localContext, leadInfo, ref campaign);
                    if (!validateLeadStatus.TransactionOk)
                    {
                        HttpResponseMessage invalidInfoMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        string errorMessage = validateLeadStatus.ErrorMessage;
                        invalidInfoMess.Content = new StringContent(errorMessage);

                        _logger.LogError($"CreateCustomerLead: Failed - {errorMessage}");

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
            }
            catch (Exception ex)
            {
                _exceptionCustomProperties["source"] = prefix;
                _logger.LogException(ex, _exceptionCustomProperties);

                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
                _logger.Dispose();
            }
        }
    }
}
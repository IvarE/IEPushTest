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
using Skanetrafiken.Crm.Properties;
using Skanetrafiken.Crm;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Skanetrafiken.Crm.Entities
{
    public class LeadEntity : Generated.Lead
    {
        //private static string CreateAccountSubject = "Skapa Mitt Konto";

        public static ColumnSet LeadInfoBlock = new ColumnSet(
            LeadEntity.Fields.LeadId,
            LeadEntity.Fields.ed_InformationSource,
            LeadEntity.Fields.FirstName,
            LeadEntity.Fields.LastName,
            LeadEntity.Fields.Address1_Line1,
            LeadEntity.Fields.Address1_Line2,
            LeadEntity.Fields.Address1_PostalCode,
            LeadEntity.Fields.Address1_City,
            LeadEntity.Fields.ed_Address1_Country,
            LeadEntity.Fields.Telephone1,
            LeadEntity.Fields.MobilePhone,
            LeadEntity.Fields.ed_Personnummer,
            LeadEntity.Fields.ed_HasSwedishSocialSecurityNumber,
            LeadEntity.Fields.EMailAddress1,
            LeadEntity.Fields.CampaignId,
            LeadEntity.Fields.ed_CampaignCode,
            LeadEntity.Fields.StateCode
            );

        public LeadEntity()
        {
        }

        public LeadEntity(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
            FirstName = customerInfo.FirstName;
            LastName = customerInfo.LastName;
            Address1_Line1 = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.CO : null;
            Address1_Line2 = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.Line1 : null;
            Address1_PostalCode = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.PostalCode : null;
            Address1_City = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.City : null;
            ed_Address1_Country = customerInfo.AddressBlock != null ? string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ? null : CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO) : null;
            Telephone1 = customerInfo.Telephone;
            MobilePhone = customerInfo.Mobile;
            ed_Personnummer = customerInfo.SocialSecurityNumber;
            ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
            EMailAddress1 = customerInfo.Email;
        }

        public static StatusBlock CanLeadBeCreated(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            // Check inData
            if (customerInfo == null)
            {
                return new StatusBlock()
                {
                    TransactionOk = false,
                    ErrorMessage = Resources.IncomingDataCannotBeNull,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput
                };
            }
            bool filterAdded = false;
            // Check for conflicting Contact
            FilterExpression contactFilter = new FilterExpression(LogicalOperator.Or);
            if (customerInfo.SocialSecurityNumber != null && customerInfo.SwedishSocialSecurityNumberSpecified && customerInfo.SwedishSocialSecurityNumber)
            {
                FilterExpression persNrFilter = new FilterExpression(LogicalOperator.And);
                persNrFilter.AddCondition(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customerInfo.SocialSecurityNumber);
                persNrFilter.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                persNrFilter.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull);
                contactFilter.AddFilter(persNrFilter);
                filterAdded = true;
            }
            if (customerInfo.Email != null)
            {
                FilterExpression emailFilter = new FilterExpression(LogicalOperator.And);
                emailFilter.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                emailFilter.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email);
                contactFilter.AddFilter(emailFilter);
                filterAdded = true;
            }
            IList<ContactEntity> existingConflictingContacts = null;
            if (filterAdded)
                existingConflictingContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.Id), contactFilter);

            if (existingConflictingContacts != null && existingConflictingContacts.Count > 0)
            {
                if (customerInfo.Email == null || !customerInfo.Email.Equals(existingConflictingContacts[0].EMailAddress1, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (customerInfo.SwedishSocialSecurityNumber)
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherContactFound,
                            ErrorMessage = string.Format(Resources.CouldNotCreateCustomerSocSec)
                        };
                }
                else
                {
                    return new StatusBlock()
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherContactFound,
                        ErrorMessage = string.Format(Resources.CouldNotCreateCustomerEmail)
                    };
                }
            }

            // Check for conflicting Lead
            filterAdded = false;
            FilterExpression leadFilter = new FilterExpression(LogicalOperator.And)
            {
                Conditions =
                {
                    new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                }
            };
            FilterExpression leadOrFilter = new FilterExpression(LogicalOperator.Or);
            if (customerInfo.SocialSecurityNumber != null && customerInfo.SwedishSocialSecurityNumberSpecified && customerInfo.SwedishSocialSecurityNumber)
            {
                FilterExpression persNrFilter = new FilterExpression();
                persNrFilter.AddCondition(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, customerInfo.SocialSecurityNumber);
                persNrFilter.AddCondition(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open);
                leadOrFilter.AddFilter(persNrFilter);
                filterAdded = true;
            }
            if (customerInfo.Email != null)
            {
                FilterExpression leadEmailFilter = new FilterExpression(LogicalOperator.And);
                leadEmailFilter.AddCondition(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open);
                leadEmailFilter.AddCondition(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email);
                //leadEmailNameFilter.AddCondition(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName);
                //leadEmailNameFilter.AddCondition(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName);
                leadOrFilter.AddFilter(leadEmailFilter);
                filterAdded = true;
            }
            IList<LeadEntity> tempExistingConflictingLeads = null;
            if (filterAdded)
            {
                leadFilter.AddFilter(leadOrFilter);
                tempExistingConflictingLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadInfoBlock, leadFilter);
            }

            List<LeadEntity> existingConflictingLeads = new List<LeadEntity>();


            if (tempExistingConflictingLeads != null && tempExistingConflictingLeads.Count > 0)
            {
                foreach (var possibleLeadMatch in tempExistingConflictingLeads)
                {
                    string firstnameLeadMatch = Regex.Replace(possibleLeadMatch.FirstName, @"[^\p{L}\p{N}]+", "");
                    string firstnameCustomerInfo = Regex.Replace(customerInfo.FirstName, @"[^\p{L}\p{N}]+", "");

                    string lastnameLeadMatch = Regex.Replace(possibleLeadMatch.LastName, @"[^\p{L}\p{N}]+", "");
                    string lastnameCustomerInfo = Regex.Replace(customerInfo.LastName, @"[^\p{L}\p{N}]+", "");

                    // Compare first- and lastname (since email has been matched earlier)
                    if (String.Compare(firstnameLeadMatch, firstnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
                        && String.Compare(lastnameLeadMatch, lastnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                    {
                        // First- and Lastname Match (Emailaddress matched when retrieving)
                        // Add to new List of Leads
                        existingConflictingLeads.Add((LeadEntity)possibleLeadMatch);
                    }
                }
            }


            if (existingConflictingLeads != null && existingConflictingLeads.Count > 0)
            {
                if (existingConflictingLeads.Count == 1)
                {
                    IEnumerator<LeadEntity> enumerator = existingConflictingLeads.GetEnumerator();
                    enumerator.MoveNext();
                    return new StatusBlock()
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                        Information = enumerator.Current.Id.ToString()
                    };
                }
                else
                {
                    LeadEntity personnummerMatch = null, emailMatch = null;
                    foreach (LeadEntity lead in existingConflictingLeads)
                    {
                        if (lead.ed_Personnummer.Equals(customerInfo.SocialSecurityNumber))
                            personnummerMatch = lead;
                        if (lead.EMailAddress1.Equals(customerInfo.Email, StringComparison.InvariantCultureIgnoreCase))
                            emailMatch = lead;
                    }
                    if (personnummerMatch != null)
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                            Information = personnummerMatch.Id.ToString()
                        };
                    }
                    else if (emailMatch != null)
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                            Information = emailMatch.Id.ToString()
                        };
                    }
                    else
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.GenericError,
                            ErrorMessage = Resources.InconsistencyInDatabase
                        };
                    }
                }
            }
            return new StatusBlock()
            {
                TransactionOk = true,
                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.NoConflictingEntity
            };
        }

        internal static IList<LeadEntity> FindExistingCampaignLeads(Plugin.LocalPluginContext localContext, LeadInfo leadInfo)
        {
            if (leadInfo == null)
                return null;

            IList<LeadEntity> socSecLeads = new List<LeadEntity>();

            // Find by social security number first.
            if (!string.IsNullOrWhiteSpace(leadInfo.SocialSecurityNumber) && leadInfo.SwedishSocialSecurityNumberSpecified && leadInfo.SwedishSocialSecurityNumber)
            {
                QueryExpression query1 = new QueryExpression()
                {
                    EntityName = LeadEntity.EntityLogicalName,
                    ColumnSet = LeadEntity.LeadInfoBlock,
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, leadInfo.SocialSecurityNumber),
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
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
                            LinkCriteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, leadInfo.CampaignId)
                                }
                            }
                        }
                    }
                };

                socSecLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, query1);
            }

            #region Query Email
            QueryExpression query = new QueryExpression()
            {
                EntityName = LeadEntity.EntityLogicalName,
                ColumnSet = LeadEntity.LeadInfoBlock,
                Criteria =
                {
                    Conditions =
                    {
                        //new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Qualified),
                        //new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, leadInfo.FirstName),
                        //new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, leadInfo.LastName),
                        new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, leadInfo.Email)
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
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, leadInfo.CampaignId)
                            }
                        }
                    }
                }
            };
            #endregion

            #region old (not in use)
            //QueryExpression query = new QueryExpression()
            //{
            //    EntityName = LeadEntity.EntityLogicalName,
            //    ColumnSet = LeadEntity.LeadInfoBlock,
            //    Criteria =
            //    {
            //        Conditions =
            //        {
            //            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
            //            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, leadInfo.FirstName),
            //            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, leadInfo.LastName),
            //            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, leadInfo.Email)
            //        }
            //    },
            //    LinkEntities =
            //    {
            //        new LinkEntity()
            //        {
            //            LinkFromEntityName = LeadEntity.EntityLogicalName,
            //            LinkToEntityName = CampaignEntity.EntityLogicalName,
            //            LinkFromAttributeName = LeadEntity.Fields.CampaignId,
            //            LinkToAttributeName = CampaignEntity.Fields.CampaignId,
            //            EntityAlias = CampaignEntity.EntityLogicalName,
            //            JoinOperator = JoinOperator.Inner,
            //            LinkCriteria =
            //            {
            //                Conditions =
            //                {
            //                    new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, leadInfo.CampaignId)
            //                }
            //            }
            //        }
            //    }
            //};
            #endregion

            List<LeadEntity> tempPossibleMatches = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, query);
            List<LeadEntity> possibleMatches = new List<LeadEntity>();

            if (tempPossibleMatches.Count > 0)
            {
                // Loop trough possible matches of Leads
                foreach (var possibleLeadMatch in tempPossibleMatches)
                {
                    string firstnameLeadMatch = Regex.Replace(possibleLeadMatch.FirstName, @"[^\p{L}\p{N}]+", "");
                    string firstnameCustomerInfo = Regex.Replace(leadInfo.FirstName, @"[^\p{L}\p{N}]+", "");

                    string lastnameLeadMatch = Regex.Replace(possibleLeadMatch.LastName, @"[^\p{L}\p{N}]+", "");
                    string lastnameCustomerInfo = Regex.Replace(leadInfo.LastName, @"[^\p{L}\p{N}]+", "");

                    // Compare first- and lastname (since email has been matched earlier)
                    if (String.Compare(firstnameLeadMatch, firstnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
                        && String.Compare(lastnameLeadMatch, lastnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                    {
                        // First- and Lastname Match (Emailaddress matched when retrieving)
                        // Add to new List of Leads
                        possibleMatches.Add(possibleLeadMatch);
                    }
                }
            }

            possibleMatches.AddRange(socSecLeads);

            // Find by CampaignCode
            if (!string.IsNullOrWhiteSpace(leadInfo.CampaignCode))
            {
                LeadEntity campaignCodeLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression
                    {
                        Conditions = {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, leadInfo.CampaignCode)
                        }
                    });
                if (campaignCodeLead != null)
                {
                    possibleMatches.Add(campaignCodeLead);
                }
            }

            return possibleMatches;
        }

        internal static void UpdateLeadSourceAndLeadTopic(Plugin.LocalPluginContext localContext, CampaignEntity campaign, IList<LeadEntity> campaignConnectedLeads)
        {
            if (campaign == null)
            {
                throw new Exception(Resources.CampaignMissing);
            }

            foreach (LeadEntity l in campaignConnectedLeads)
            {
                if (l.LeadSourceCode == null || new OptionSetValue((int)l.LeadSourceCode) != campaign.ed_LeadSource)
                {
                    if (campaign.ed_LeadSource != null)
                    {
                        l.LeadSourceCode = (Generated.lead_leadsourcecode)campaign.ed_LeadSource.Value;
                    }
                    else
                    {
                        l.LeadSourceCode = null;
                    }
                }

                if (l.Subject == null || l.Subject != campaign.ed_LeadTopic.ToString())
                {
                    if (campaign.ed_LeadTopic != null)
                    {
                        l.Subject = campaign.ed_LeadTopic.ToString();
                    }
                    else
                    {
                        l.Subject = null;
                    }
                }

                XrmHelper.Update(localContext.OrganizationService, l);
            }


        }

        public static void HandlePreLeadSetState(Plugin.LocalPluginContext localContext, EntityReference leadRef, OptionSetValue stateCode, OptionSetValue statusCode)
        {
            if (stateCode?.Value == (int)Generated.LeadState.Qualified || stateCode?.Value == (int)Generated.LeadState.Disqualified)
            {
                LeadEntity updateEntity = new LeadEntity
                {
                    Id = leadRef.Id,
                    ed_CampaignCode = null
                };
                XrmHelper.Update(localContext, updateEntity);
            }
        }

        // Updates Campaign with statistics
        public static void HandlePostLeadSetStateAsync(Plugin.LocalPluginContext localContext, EntityReference leadRef, OptionSetValue stateCode, OptionSetValue statusCode)
        {
            if (stateCode?.Value == (int)Generated.LeadState.Qualified)
            {
                CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(
                    localContext,
                    new QueryExpression()
                    {
                        EntityName = CampaignEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(
                            CampaignEntity.Fields.StageId,
                            CampaignEntity.Fields.ed_QualifiedLeads,
                            CampaignEntity.Fields.ed_QualifiedLeadsDR1,
                            CampaignEntity.Fields.ed_QualifiedLeadsDR2,
                            CampaignEntity.Fields.ed_ValidFromPhase1,
                            CampaignEntity.Fields.ed_ValidFromPhase2,
                            CampaignEntity.Fields.ed_ValidToPhase1,
                            CampaignEntity.Fields.ed_ValidToPhase2
                            ),
                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = CampaignEntity.EntityLogicalName,
                                LinkToEntityName = LeadEntity.EntityLogicalName,
                                LinkFromAttributeName = CampaignEntity.Fields.CampaignId,
                                LinkToAttributeName = LeadEntity.Fields.CampaignId,
                                EntityAlias = LeadEntity.EntityLogicalName,
                                JoinOperator = JoinOperator.Inner,
                                LinkCriteria =
                                {
                                    Conditions = {
                                        new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, leadRef.Id)
                                    }
                                }
                            }
                        }
                    });

                if (campaign != null)
                {
                    bool update = false;
                    CampaignEntity updateEntity = new CampaignEntity()
                    {
                        Id = campaign.Id,
                        CampaignId = campaign.CampaignId
                    };
                    int totalLeads = 0;

                    if (campaign.Contains(CampaignEntity.Fields.ed_ValidFromPhase1) && campaign.ed_ValidFromPhase1.HasValue &&
                        campaign.Contains(CampaignEntity.Fields.ed_ValidToPhase1) && campaign.ed_ValidToPhase1.HasValue)
                    {
                        IList<LeadEntity> connectedLeadsDR1 = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id),
                                    new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Qualified),
                                    new ConditionExpression(LeadEntity.Fields.ModifiedOn, ConditionOperator.OnOrAfter, campaign.ed_ValidFromPhase1),
                                    new ConditionExpression(LeadEntity.Fields.ModifiedOn, ConditionOperator.OnOrBefore, campaign.ed_ValidToPhase1.Value.AddDays(1))
                                }
                            });
                        if (connectedLeadsDR1 != null)
                        {
                            //if (campaign.ed_QualifiedLeadsDR1 != connectedLeadsDR1.Count)
                            //{
                            //    update = true;
                            //    updateEntity.ed_QualifiedLeadsDR1 = connectedLeadsDR1.Count;
                            //}
                            //totalLeads += connectedLeadsDR1.Count;
                        }
                    }
                    

                    if (campaign.Contains(CampaignEntity.Fields.ed_ValidFromPhase2) && campaign.ed_ValidFromPhase2.HasValue &&
                        campaign.Contains(CampaignEntity.Fields.ed_ValidToPhase2) && campaign.ed_ValidToPhase2.HasValue)
                    {
                        IList<LeadEntity> connectedLeadsDR2 = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id),
                                    new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Qualified),
                                    new ConditionExpression(LeadEntity.Fields.ModifiedOn, ConditionOperator.OnOrAfter, campaign.ed_ValidFromPhase2),
                                    new ConditionExpression(LeadEntity.Fields.ModifiedOn, ConditionOperator.OnOrBefore, campaign.ed_ValidToPhase2.Value.AddDays(1))
                                }
                            });
                        if (connectedLeadsDR2 != null)
                        {
                            //if (campaign.ed_QualifiedLeadsDR2 != connectedLeadsDR2.Count)
                            //{
                            //    update = true;
                            //    updateEntity.ed_QualifiedLeadsDR2 = connectedLeadsDR2.Count;
                            //}
                            //totalLeads += connectedLeadsDR2.Count;
                        }
                    }
                    if (campaign.ed_QualifiedLeads != totalLeads)
                    {
                        update = true;
                        updateEntity.ed_QualifiedLeads = totalLeads;
                    }


                    if (update)
                    {
                        XrmHelper.Update(localContext.OrganizationService, updateEntity);
                    }
                }
            }
        }

        public void HandlePreLeadCreate(Plugin.LocalPluginContext localContext)
        {
            //if (this.ed_InformationSource == Generated.ed_informationsource.Kampanj)
            //{
            //    if (!Contains(LeadEntity.Fields.CampaignId) || Guid.Empty.Equals(CampaignId.Id))
            //    {
            //        throw new Exception(Resources.CampaignIdMissing);
            //    }

            //}

            if (Contains(LeadEntity.Fields.CampaignId) && !Guid.Empty.Equals(CampaignId.Id))
            {
                ThrowErrorIfConflictingCampaignLead(localContext);

                CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(CampaignEntity.Fields.ed_LeadTopic, CampaignEntity.Fields.ed_LeadSource),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.CampaignId, ConditionOperator.Equal, CampaignId.Id)
                        }
                    });
                //string debugMess = "campaign == " + campaign +
                //    ". campaign.ed_LeadTopic = " + (campaign != null ? (campaign.ed_LeadTopic.HasValue ? campaign.ed_LeadTopic.Value.ToString() : "missing") : "meh") +
                //    ". campaign.ed_LeadSource = " + (campaign != null ? (campaign.ed_LeadSource != null ? "" + campaign.ed_LeadSource.Value : "missing") : "meh");
                //throw new InvalidPluginExecutionException(debugMess);
                if (campaign != null)
                {
                    Subject = campaign.ed_LeadTopic.HasValue ? campaign.ed_LeadTopic.Value.ToString() : Subject;
                    LeadSourceCode = campaign.ed_LeadSource != null ? (Generated.lead_leadsourcecode)campaign.ed_LeadSource.Value : LeadSourceCode;
                }
            }
        }

        public void HandlePreLeadUpdate(Plugin.LocalPluginContext localContext, LeadEntity preImage)
        {
            LeadEntity combined = new LeadEntity { Id = this.Id };
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

            combined.ThrowErrorIfConflictingCampaignLead(localContext);
        }

        private void ThrowErrorIfConflictingCampaignLead(Plugin.LocalPluginContext localContext)
        {
            if (CampaignId != null && !Guid.Empty.Equals(CampaignId.Id))
            {
                // Check for Campaign Duplicates
                bool filterRelevant = false;
                FilterExpression nameEmailOrSocSecNrOrCampaignCodeFilter = new FilterExpression(LogicalOperator.Or);
                // add socSecNr Filter
                if (!string.IsNullOrWhiteSpace(this.ed_Personnummer))
                {
                    nameEmailOrSocSecNrOrCampaignCodeFilter.AddCondition(
                        new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, this.ed_Personnummer)
                        );
                    filterRelevant = true;
                }
                // Add Name + Email Filter
                if (!string.IsNullOrWhiteSpace(this.FirstName) && !string.IsNullOrWhiteSpace(this.LastName) && !string.IsNullOrWhiteSpace(this.EMailAddress1))
                {
                    nameEmailOrSocSecNrOrCampaignCodeFilter.AddFilter(new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, this.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, this.LastName),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, this.EMailAddress1)
                        }
                    });
                    filterRelevant = true;
                }
                if (!string.IsNullOrWhiteSpace(ed_CampaignCode))
                {
                    nameEmailOrSocSecNrOrCampaignCodeFilter.AddCondition(
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, this.ed_CampaignCode));
                }
                // Do not check for duplicates if no valid filter is added
                if (filterRelevant)
                {
                    IList<LeadEntity> conflictLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_Personnummer),
                        new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                //new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                                new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, CampaignId.Id),
                                new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.NotEqual, this.Id)
                            },
                            Filters =
                            {
                                nameEmailOrSocSecNrOrCampaignCodeFilter
                            }
                        });
                    if (conflictLeads != null && conflictLeads.Count > 0)
                    {
                        foreach (LeadEntity l in conflictLeads)
                        {
                            if (l.ed_Personnummer != null && l.ed_Personnummer == this.ed_Personnummer)
                                throw new Exception("There is already a lead with the same Social Security Number in the assigned Campaign");
                        }
                        throw new Exception("There is already a lead with the same Name and Email in the assigned Campaign");
                    }
                }
            }
        }

        public CustomerInfo ToCustomerInfo(Plugin.LocalPluginContext localContext)
        {
            return new CustomerInfo()
            {
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? null : this.EMailAddress1,
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? null : this.FirstName,
                Guid = this.Id.ToString(),
                LastName = string.IsNullOrWhiteSpace(this.LastName) ? null : this.LastName,
                Mobile = string.IsNullOrWhiteSpace(this.MobilePhone) ? null : this.MobilePhone,
                SocialSecurityNumber = string.IsNullOrWhiteSpace(this.ed_Personnummer) ? null : this.ed_Personnummer,
                Telephone = string.IsNullOrWhiteSpace(this.Telephone1) ? null : this.Telephone1,
                AddressBlock = (
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                this.ed_Address1_Country == null &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode)) ? null : new CustomerInfoAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address1_City) ? null : this.Address1_City,
                    CO = string.IsNullOrWhiteSpace(this.Address1_Line1) ? null : this.Address1_Line1,
                    CountryISO = this.ed_Address1_Country == null ? null : CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id),
                    Line1 = string.IsNullOrWhiteSpace(this.Address1_Line2) ? null : this.Address1_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address1_PostalCode) ? null : this.Address1_PostalCode
                },
                Source = this.ed_InformationSource.HasValue ? (int)this.ed_InformationSource.Value : -1,
                isAddressEnteredManually = true,
                isAddressEnteredManuallySpecified = true,
                isAddressInformationComplete = this.IsCustomerInformationComplete(localContext),
                isAddressInformationCompleteSpecified = true,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? this.ed_HasSwedishSocialSecurityNumber.Value : false,
            };
        }

        public LeadInfo ToLeadInfo(Plugin.LocalPluginContext localContext)
        {
            string[] products = { };
            CampaignEntity campaignEntity = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(CampaignEntity.Fields.ed_DiscountPercent, CampaignEntity.Fields.CodeName,
                CampaignEntity.Fields.ed_ValidFromPhase1, CampaignEntity.Fields.ed_ValidToPhase1, CampaignEntity.Fields.ed_ValidFromPhase2, CampaignEntity.Fields.ed_ValidToPhase2),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.Id, ConditionOperator.Equal, this.CampaignId.Id)
                        }
                    });

            bool isCampaignActiveDefined = false;
            bool isCampaignActive = false;

            if (campaignEntity != null)
            {
                IList<CampaignItemEntity> campaignItems = XrmRetrieveHelper.RetrieveMultiple<CampaignItemEntity>(localContext, new ColumnSet(CampaignItemEntity.Fields.EntityId),
                    new FilterExpression
                    {
                        Conditions =
                        {
                                new ConditionExpression(CampaignItemEntity.Fields.CampaignId, ConditionOperator.Equal, campaignEntity.Id)
                        }
                    }
                    );

                products = new string[campaignItems.Count];
                if (campaignItems.Count != 0)
                {
                    for (int i = 0; i < campaignItems.Count; i++)
                    {
                        ProductEntity product = XrmRetrieveHelper.RetrieveFirst<ProductEntity>(localContext, new ColumnSet(ProductEntity.Fields.Name),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ProductEntity.Fields.Id, ConditionOperator.Equal, campaignItems[i].EntityId)
                                }
                            });

                        products[i] = product.Name;
                    }
                }

                if (campaignEntity.ed_ValidFromPhase1 != null && campaignEntity.ed_ValidToPhase1.HasValue)
                {
                    if (DateTime.Now.CompareTo(campaignEntity.ed_ValidFromPhase1) > 0 && DateTime.Now.CompareTo(campaignEntity.ed_ValidToPhase1.Value.AddDays(1)) < 0)
                    {
                        isCampaignActive = true;
                        isCampaignActiveDefined = true;
                    }
                    else if (campaignEntity.ed_ValidFromPhase2 != null && campaignEntity.ed_ValidToPhase2.HasValue)
                    {
                        if (DateTime.Now.CompareTo(campaignEntity.ed_ValidFromPhase2) > 0 && DateTime.Now.CompareTo(campaignEntity.ed_ValidToPhase2.Value.AddDays(1)) < 0)
                        {
                            isCampaignActive = true;
                            isCampaignActiveDefined = true;
                        }
                    }
                }
            }

            return new LeadInfo()
            {
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? (string.IsNullOrWhiteSpace(this.EMailAddress2) ? null : this.EMailAddress2) : this.EMailAddress1,
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? null : this.FirstName,
                Guid = this.Id.ToString(),
                LastName = string.IsNullOrWhiteSpace(this.LastName) ? null : this.LastName,
                Mobile = string.IsNullOrWhiteSpace(this.MobilePhone) ? null : this.MobilePhone,
                SocialSecurityNumber = string.IsNullOrWhiteSpace(this.ed_Personnummer) ? null : this.ed_Personnummer,
                Telephone = string.IsNullOrWhiteSpace(this.Telephone1) ? null : this.Telephone1,
                AddressBlock = (
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode) &&
                this.ed_Address1_Country == null) ? null : new CustomerInfoAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address1_City) ? null : this.Address1_City,
                    CO = string.IsNullOrWhiteSpace(this.Address1_Line1) ? null : this.Address1_Line1,
                    CountryISO = this.ed_Address1_Country == null ? null : CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id),
                    Line1 = string.IsNullOrWhiteSpace(this.Address1_Line2) ? null : this.Address1_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address1_PostalCode) ? null : this.Address1_PostalCode
                },
                isAddressInformationComplete = this.IsCustomerInformationComplete(localContext),
                isAddressInformationCompleteSpecified = true,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? ed_HasSwedishSocialSecurityNumber.Value : false,
                SwedishSocialSecurityNumberSpecified = this.ed_HasSwedishSocialSecurityNumber.HasValue,
                isAddressEnteredManually = this.ed_InformationSource.HasValue && this.ed_InformationSource == Generated.ed_informationsource.Folkbokforing ? false : true,
                isAddressEnteredManuallySpecified = this.ed_InformationSource.HasValue,
                Source = this.ed_InformationSource.HasValue ? (int)this.ed_InformationSource.Value : -1,

                CampaignId = string.IsNullOrWhiteSpace(campaignEntity.CodeName) ? null : campaignEntity.CodeName,
                CampaignCode = ed_CampaignCode,
                IsCampaignActive = isCampaignActive,
                IsCampaignActiveSpecified = isCampaignActiveDefined,
                CampaignDiscountPercent = campaignEntity.ed_DiscountPercent.HasValue ? (int)campaignEntity.ed_DiscountPercent : 0,
                CampaignDiscountPercentSpecified = campaignEntity.ed_DiscountPercent.HasValue,
                CampaignProducts = products,
            };
        }

        public bool UpdateWithCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            bool updated = false;
            if (customerInfo == null)
                return false;
            if (!string.IsNullOrWhiteSpace(customerInfo.FirstName) && !customerInfo.FirstName.Equals(FirstName))
            {
                this.FirstName = customerInfo.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.LastName) && !customerInfo.LastName.Equals(LastName))
            {
                this.LastName = customerInfo.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !customerInfo.Email.Equals(EMailAddress1))
            {
                this.EMailAddress1 = customerInfo.Email;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Telephone) && !customerInfo.Telephone.Equals(Telephone1))
            {
                this.Telephone1 = customerInfo.Telephone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Mobile) && !customerInfo.Mobile.Equals(MobilePhone))
            {
                this.MobilePhone = customerInfo.Mobile;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SocialSecurityNumber.Equals(ed_Personnummer))
            {
                this.ed_Personnummer = customerInfo.SocialSecurityNumber;
                this.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                updated = true;
            }
            if (customerInfo.AddressBlock != null &&
                    (
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CO) && !customerInfo.AddressBlock.CO.Equals(Address1_Line2)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) && !customerInfo.AddressBlock.Line1.Equals(Address1_Line2)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) && !customerInfo.AddressBlock.PostalCode.Equals(Address1_PostalCode)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) && !customerInfo.AddressBlock.City.Equals(Address1_City)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) && (this.ed_Address1_Country == null || !CountryEntity.GetIsoCodeForCountry(localContext, ed_Address1_Country.Id).Equals(customerInfo.AddressBlock.CountryISO)))
                    )
                )
            {
                this.Address1_Line1 = customerInfo.AddressBlock.CO;
                this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                this.Address1_City = customerInfo.AddressBlock.City;
                this.ed_Address1_Country = CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);
                updated = true;
            }
            return updated;
        }

        private bool IsCustomerInformationComplete(Plugin.LocalPluginContext localContext)
        {
            StatusBlock validated = CustomerUtility.ValidateCustomerInfo(localContext, new CustomerInfo()
            {
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = this.Address1_Line1,
                    Line1 = this.Address1_Line2,
                    PostalCode = this.Address1_PostalCode,
                    City = this.Address1_City,
                    CountryISO = this.ed_Address1_Country != null ? CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id) : null
                },
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? this.EMailAddress2 : this.EMailAddress1,
                Telephone = this.Telephone1,
                Mobile = this.MobilePhone,
                SocialSecurityNumber = this.ed_Personnummer,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? this.ed_HasSwedishSocialSecurityNumber.Value : false,
                Source = (int)Crm.Schema.Generated.ed_informationsource.SkapaMittKonto
            });
            return validated.TransactionOk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static LeadEntity CreateLead(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo, string LeadSubject, CampaignEntity campaign = null)
        {
            if (customerInfo == null)
                return null;

            LeadInfo leadInfo = (LeadInfo)customerInfo;
            LeadEntity lead = new LeadEntity(localContext, customerInfo);
            lead.Subject = LeadSubject;

            if (campaign != null)
            {
                lead.CampaignId = campaign.ToEntityReference();
            }
            else if (leadInfo.CampaignId != null)
            {
                campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, leadInfo.CampaignId),
                            new ConditionExpression(CampaignEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.CampaignState.Active)
                        }
                    });

                lead.CampaignId = campaign.ToEntityReference();
            }

            lead.Id = localContext.OrganizationService.Create(lead);
            lead.LeadId = lead.Id;
            return lead;
        }

        //public static QueryExpression CreateConflictQueryForCampaign(LeadEntity lead)
        //{

        //    #region make Contact Conflict Query
        //    FilterExpression contactConflictFilter = new FilterExpression(LogicalOperator.Or);
        //    if (lead.ed_HasSwedishSocialSecurityNumber.HasValue && lead.ed_HasSwedishSocialSecurityNumber.Value)
        //    {
        //        FilterExpression SocSecFilter = new FilterExpression(LogicalOperator.And)
        //        {
        //            Conditions =
        //                        {
        //                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
        //                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, lead.ed_Personnummer)
        //                        }
        //        };
        //        contactConflictFilter.AddFilter(SocSecFilter);
        //    }

        //    FilterExpression nameMailFilter = new FilterExpression(LogicalOperator.And)
        //    {
        //        Conditions =
        //                    {
        //                        new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
        //                        //new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, lead.EMailAddress1)
        //                    },
        //        Filters =
        //                    {
        //                        new FilterExpression(LogicalOperator.Or)
        //                        {
        //                            Filters =
        //                            {
        //                                new FilterExpression(LogicalOperator.And)
        //                                {
        //                                    Conditions =
        //                                    {
        //                                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, lead.EMailAddress1)
        //                                    }
        //                                },
        //                                new FilterExpression(LogicalOperator.And)
        //                                {
        //                                    Conditions =
        //                                    {
        //                                        new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, lead.EMailAddress1)
        //                                    }
        //                                },
        //                            }

        //                        },

        //                        new FilterExpression(LogicalOperator.Or)
        //                        {
        //                            Filters =
        //                            {
        //                                new FilterExpression(LogicalOperator.And)
        //                                {
        //                                    Conditions =
        //                                    {
        //                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
        //                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
        //                                    }
        //                                },
        //                                new FilterExpression(LogicalOperator.And)
        //                                {
        //                                    Conditions =
        //                                    {
        //                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
        //                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //    };
        //    contactConflictFilter.AddFilter(nameMailFilter);


        //    QueryExpression contactConflictQuery = new QueryExpression()
        //    {
        //        EntityName = ContactEntity.EntityLogicalName,
        //        ColumnSet = new ColumnSet(true),
        //        Criteria = contactConflictFilter
        //    };
        //    #endregion
        //    return contactConflictQuery;
        //}

    }


}

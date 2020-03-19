using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class QueueItemEntity : Generated.QueueItem
    {
        //public void HandlePostQueueItemCreate(Plugin.LocalPluginContext localContext)
        //{
        //    localContext.TracingService.Trace("Entered HandlePostQueueItemCreate()");
        //    if (ObjectId != null)
        //    {
        //        EmailEntity email = XrmRetrieveHelper.RetrieveFirst<EmailEntity>(localContext,
        //            new ColumnSet(
        //                EmailEntity.Fields.RegardingObjectId,
        //                EmailEntity.Fields.From,
        //                EmailEntity.Fields.Subject),
        //            new FilterExpression()
        //            {
        //                Conditions =
        //                {
        //                    new ConditionExpression(EmailEntity.Fields.Id, ConditionOperator.Equal, ObjectId.Id)
        //                }
        //            });
        //        if (email == null)
        //        {
        //            localContext.Trace($"Could not find an EmailEntity with id: {ObjectId.Id}");
        //            return;
        //        }

        //        // Run only if not connected to case
        //        if (email.RegardingObjectId == null)
        //        {
        //            Generated.ActivityParty sender = email.From.ToList().ToArray()[0];
        //            string emailFrom = sender.AddressUsed;
        //            if (emailFrom == null)
        //            {
        //                localContext.TracingService.Trace("Could not retrieve the email address that sent the message, exiting.");
        //                return;
        //            }

        //            ContactEntity contact = GetContactFromEmail(localContext, emailFrom);
        //            EntityReference contactId = contact == null ? null : contact.ToEntityReference();
                    

        //            if (medlemsserviceInkorgType == null)
        //            {
        //                localContext.TracingService.Trace("Could not find the desired CaseType. Please add a CaseType with name 'Medlemsservice inkorg'. Exception thrown");
        //                throw new Exception("Could not find the desired CaseType. Please add a CaseType with name 'Medlemsservice inkorg'.");
        //            }
        //            IncidentEntity newIncident = new IncidentEntity()
        //            {
        //                OwnerId = medlemsserviceTeam,
        //                Title = email.Subject,
        //                //PrimaryContactId = contactId,
        //                CustomerId = contactId,
        //                ed_SocialSecurityNo = contact != null && !string.IsNullOrWhiteSpace(contact.ed_ContactNumber) ? contact.ed_ContactNumber : null,
        //                ed_AreaId = medlemsserviceArea,
        //                ed_CaseTypeId = medlemsserviceInkorgType.ToEntityReference(),
        //                CaseOriginCode = new OptionSetValue(2),
        //            };

        //            newIncident.Id = localContext.OrganizationService.Create(newIncident);

        //            // Connect incoming mail to new Case
        //            // *********************************
        //            EmailEntity emailUpdateRegarding = new EmailEntity();
        //            emailUpdateRegarding.Id = email.Id;
        //            emailUpdateRegarding.RegardingObjectId = newIncident.ToEntityReference();
        //            localContext.OrganizationService.Update(emailUpdateRegarding);

        //            // In debug mode? (ID = Guid.Empty
        //            if(target.Id != Guid.Empty)
        //            {
        //                SetStateRequest setState = new SetStateRequest
        //                {
        //                    EntityMoniker = target.ToEntityReference(),
        //                    State = new OptionSetValue((int)Generated.QueueItemState.Inactive),
        //                    Status = new OptionSetValue((int)Generated.queueitem_statuscode.Inaktiv)
        //                };

        //                localContext.OrganizationService.Execute(setState);
        //            }
        //        }
        //        else
        //        {
        //            localContext.TracingService.Trace("Email had a 'RegardingObject' so no new Incident will be created.");
        //        }
        //    }
        //    else
        //    {
        //        localContext.TracingService.Trace("target missing ObjectId");
        //    }

        //}

        //private static ContactEntity GetContactFromEmail(Plugin.LocalPluginContext localContext, string emailFrom)
        //{
        //    IList<ContactEntity> intressenter = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.StateCode
        //                                                                                                                    , ContactEntity.Fields.ed_ContactNumber),
        //        new FilterExpression()
        //        {
        //            Conditions =
        //            {
        //                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, emailFrom)
        //            }
        //        });
        //    ContactEntity intressent = null;
        //    foreach (ContactEntity c in intressenter)
        //    {
        //        if (c.StateCode == Generated.ContactState.Active)
        //        {
        //            intressent = c;
        //            break;
        //        }
        //        intressent = c;
        //    }

        //    return intressent == null ? ContactEntity.FindOrCreateDefaultContactEmailToCaseProcess(localContext) : intressent ;
        //}
    }
}
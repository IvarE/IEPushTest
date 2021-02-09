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

namespace Skanetrafiken.Crm.Entities
{
    public class MergeRecordsEntity : Generated.ed_MergeRecords
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public void PerformeMerge(Plugin.LocalPluginContext localContext)
        {
            try
            {
                // Verify input data
                if (string.IsNullOrEmpty(this.ed_MergeFromContact)
                    || string.IsNullOrEmpty(this.ed_MergeToContact))
                {
                    throw new Exception("Både från och till kontakt måste anges.");
                }

                Guid testGuid = Guid.Empty;
                if (!Guid.TryParse(this.ed_MergeFromContact, out testGuid)
                    || !Guid.TryParse(this.ed_MergeToContact, out testGuid))
                {
                    throw new Exception("Både från och till kontakt måste vara en giltig GUID");
                }

                // Make sure both contacts exists
                ContactEntity fromContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(this.ed_MergeFromContact)
                    , new ColumnSet(true));

                if (fromContact.StateCode != Generated.ContactState.Active)
                {
                    throw new Exception("Från kontakt är inte aktiv");
                }

                ContactEntity toContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(this.ed_MergeToContact)
                    , new ColumnSet(ContactEntity.Fields.FirstName
                                    , ContactEntity.Fields.LastName
                                    , ContactEntity.Fields.FullName
                                    , ContactEntity.Fields.StateCode
                                    , ContactEntity.Fields.StatusCode));

                if (toContact.StateCode != Generated.ContactState.Active)
                {
                    throw new Exception("Till kontakt är inte aktiv");
                }

                // Uppdatera med kontakter
                {
                    // The entity exists
                    if (this.Id != Guid.Empty)
                    {
                        MergeRecordsEntity updMergeSetContact = new MergeRecordsEntity();
                        updMergeSetContact.Id = this.Id;
                        updMergeSetContact.ed_MergeFromContactId = fromContact.ToEntityReference();
                        updMergeSetContact.ed_MergeToContactId = toContact.ToEntityReference();
                        localContext.OrganizationService.Update(updMergeSetContact);
                    }
                    // During debug
                    else
                    {
                        this.ed_MergeFromContactId = fromContact.ToEntityReference();
                        this.ed_MergeToContactId = toContact.ToEntityReference();
                    }



                }

                // Perform merge
                {
                    MergeRequest merge = new MergeRequest();
                    merge.SubordinateId = fromContact.Id;
                    merge.Target = toContact.ToEntityReference();
                    merge.PerformParentingChecks = false;

                    // Destination
                    merge.UpdateContent = toContact;

                    // Execute the request.
                    MergeResponse merged = (MergeResponse)localContext.OrganizationService.Execute(merge);

                    localContext.TracingService.Trace("Merge complete");
                }

                // Uppdatera med status. 
                {
                    // The entity exists
                    if (this.Id != Guid.Empty)
                    {
                        MergeRecordsEntity updMerge = new MergeRecordsEntity();
                        updMerge.Id = this.Id;
                        updMerge.ed_Message = string.Format("Merge klar.");
                        updMerge.ed_MergeFromContactId = fromContact.ToEntityReference();
                        updMerge.ed_MergeToContactId = toContact.ToEntityReference();
                        localContext.OrganizationService.Update(updMerge);
                    }
                    // During debug
                    else
                    {
                        this.ed_Message = string.Format("Merge klar.");
                    }

                }
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace("Exception catched. Message:{0}", ex.Message);

                // Uppdatera med status. Går update fel skall den kasta felet vidare.
                // ******************************************************************
                // The entity exists
                if (this.Id != Guid.Empty)
                {
                    MergeRecordsEntity updMerge = new MergeRecordsEntity();
                    updMerge.Id = this.Id;
                    updMerge.ed_Message = string.Format("{0}", ex.Message);
                    localContext.OrganizationService.Update(updMerge);
                    localContext.TracingService.Trace("End controled catch");
                }
                // During debug
                else
                {
                    this.ed_Message = string.Format("{0}", ex.Message);
                }

            }

        }
    }


}
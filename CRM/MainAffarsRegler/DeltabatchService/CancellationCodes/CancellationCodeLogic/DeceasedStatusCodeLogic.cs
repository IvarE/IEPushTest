using Skanetrafiken.Crm.Schema.Generated;
using System.Globalization;
using System;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public class DeceasedStatusCodeLogic : ICancellationCodeLogic
    {
        public void HandleStatusCode(Contact contact, DeltaBatchUpdateRow updatedContactData)
        {
            contact.ed_DeceasedDate =
                DateTime.TryParse(updatedContactData.RejectComment, new CultureInfo("sv-SE"), DateTimeStyles.AssumeLocal, out DateTime parsedDate) ?
                        parsedDate :
                        DateTime.Now;

            contact.ed_Deceased = true;
            contact.ed_CreditsafeRejectionCode = ed_creditsaferejectcodes.Deceased;

            Plugin.LocalPluginContext localContext = null;
            try
            {
                localContext = DeltabatchJobHelper.GenerateLocalContext();
                ContactEntity contactEntity = new ContactEntity();
                contactEntity.HandlePostContactDeleteAsync(localContext, new EntityReference(contact.LogicalName, contact.Id));

            }
            catch (Exception e)
            {
                if (localContext != null)
                DeltabatchJobHelper.SendErrorMailToDev(localContext, e);
            }
        }
    }
}

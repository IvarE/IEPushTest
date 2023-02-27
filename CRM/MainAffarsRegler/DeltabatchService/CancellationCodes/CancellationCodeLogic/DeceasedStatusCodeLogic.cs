using Skanetrafiken.Crm.Schema.Generated;
using System.Globalization;
using System;

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
        }
    }
}

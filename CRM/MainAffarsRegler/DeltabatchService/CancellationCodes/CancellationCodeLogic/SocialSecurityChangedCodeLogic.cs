using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public class SocialSecurityChangedCodeLogic : ICancellationCodeLogic
    {
        public void HandleStatusCode(Contact contact, DeltaBatchUpdateRow updatedContactData)
        {
            contact.ed_CreditsafeRejectionCode = ed_creditsaferejectcodes.Socialsecuritynumberchanged;
        }
    }
}

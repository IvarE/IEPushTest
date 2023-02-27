using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public class EmigratedStatusCodeLogic : ICancellationCodeLogic
    {
        public void HandleStatusCode(Contact contact, DeltaBatchUpdateRow updatedContactData)
        {
            contact.Address2_Line1 = updatedContactData.SpecialCo;
            contact.Address2_PostalCode = updatedContactData.SpecialPostalCode;
            contact.Address2_Country = updatedContactData.SpecialCountry;
            contact.Address2_City = updatedContactData.SpecialCity;
            contact.Address2_Line2 = updatedContactData.SpecialRegAddr;
        }
    }
}

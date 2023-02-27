using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public interface ICancellationCodeLogic
    {
        /// <summary>
        /// Applies the logic related to a particular cancellation code.
        /// </summary>
        void HandleStatusCode(Contact contact, DeltaBatchUpdateRow updatedContactData);
    }
}

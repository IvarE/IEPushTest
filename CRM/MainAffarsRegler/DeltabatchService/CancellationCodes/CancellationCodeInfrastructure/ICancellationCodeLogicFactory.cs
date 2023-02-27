#nullable enable

using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public interface ICancellationCodeLogicFactory
    {
        ICancellationCodeLogic? GetCancellationCodeHandler(ed_creditsaferejectcodes cancellationCode);
    }
}
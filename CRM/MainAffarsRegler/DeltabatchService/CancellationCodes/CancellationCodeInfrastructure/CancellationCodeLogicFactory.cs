#nullable enable
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;
using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    public class CancellationCodeLogicFactory : ICancellationCodeLogicFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CancellationCodeLogicFactory(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public ICancellationCodeLogic? GetCancellationCodeHandler(ed_creditsaferejectcodes cancellationCode)
        {
            switch(cancellationCode)
            {
                case ed_creditsaferejectcodes.Emigrated:
                    return new EmigratedStatusCodeLogic();
                case ed_creditsaferejectcodes.Deceased:
                    return new DeceasedStatusCodeLogic();
                case ed_creditsaferejectcodes.Socialsecuritynumberchanged:
                    return new SocialSecurityChangedCodeLogic();
                default:
                    return null;
            }
        }
    }
}
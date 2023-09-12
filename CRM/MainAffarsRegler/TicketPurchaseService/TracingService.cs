using Microsoft.Xrm.Sdk;
using System.Linq;
using Common.Logging;

namespace TicketPurchaseService
{
    internal class TracingService : ITracingService
    {
        private static ILog _log = LogManager.GetLogger(typeof(TracingService));

        public void Trace(string format, params object[] args)
        {
            if (args.Count() > 0)
                _log.InfoFormat(format, args);
            else
                _log.InfoFormat(format);
        }
    }
}

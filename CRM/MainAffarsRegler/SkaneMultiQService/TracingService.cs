using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.MultiQService
{
    internal class TracingService : ITracingService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Trace(string format, params object[] args)
        {
            if (args.Count() > 0)
                _log.InfoFormat(format, args);
            else
                _log.InfoFormat(format);
        }
    }
}

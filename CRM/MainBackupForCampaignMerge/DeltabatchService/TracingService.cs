using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using Microsoft.Xrm.Sdk;
using Common.Logging;

namespace Endeavor.Crm.DeltabatchService
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

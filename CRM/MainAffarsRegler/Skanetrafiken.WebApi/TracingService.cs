using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using Microsoft.Xrm.Sdk;

namespace Skanetrafiken.Crm
{
    internal class TracingService : ITracingService
    {
        private static readonly AppInsightsLogger _logger = new AppInsightsLogger();

        public void Trace(string format, params object[] args)
        {
            if (args.Count() > 0)
                _logger.InfoFormat(format, args);
            else
                _logger.InfoFormat(format);
        }
    }
}

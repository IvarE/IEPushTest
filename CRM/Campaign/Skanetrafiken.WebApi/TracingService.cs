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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.BiffIntegration
{
    class ServiceProvider : IServiceProvider
    {
        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ITracingService))
                return new TracingService();

            throw new NotImplementedException();
        }
    }
}

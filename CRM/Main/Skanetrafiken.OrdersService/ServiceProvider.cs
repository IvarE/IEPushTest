using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Skanetrafiken.OrdersService
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

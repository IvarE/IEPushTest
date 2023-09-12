using Microsoft.Xrm.Sdk;
using System;

namespace TicketPurchaseService
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

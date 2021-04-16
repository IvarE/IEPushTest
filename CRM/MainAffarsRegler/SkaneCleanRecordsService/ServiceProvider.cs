using System;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.CleanRecordsService
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

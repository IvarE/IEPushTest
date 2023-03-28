using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace Endeavor.Crm.CleanRecordsService.PandoraExtensions.Helpers
{
    public interface IRetrieveHelperWrapper
    {
        List<T> RetrieveMultiple<T>(Plugin.LocalPluginContext localContext, QueryExpression query, PagingInfo page = null) where T : Entity;
    }

    public class RetrieveHelperWrapper : IRetrieveHelperWrapper
    {
        public List<T> RetrieveMultiple<T>(Plugin.LocalPluginContext localContext, QueryExpression query, PagingInfo page = null) where T : Entity
        {
            return XrmRetrieveHelper.RetrieveMultiple<T>(localContext, query, page);
        }
    }
}

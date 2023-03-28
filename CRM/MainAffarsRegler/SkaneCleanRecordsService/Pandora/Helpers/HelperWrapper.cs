using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System;

namespace Endeavor.Crm.CleanRecordsService.PandoraExtensions.Helpers
{
    public interface IHelperWrapper
    {
        ExecuteMultipleResponse ExecuteMultipleRequests(Plugin.LocalPluginContext localContext, List<OrganizationRequest> requestsLst, Boolean continueOnError, Boolean returnResponses, int defaultBatchSize = 250);
    }

    /// <summary>
    /// A wrapper around the static Helper class method ExecuteMultipleRequests to allow mocking in unit tests.
    /// </summary>
    public class HelperWrapper : IHelperWrapper
    {
        public ExecuteMultipleResponse ExecuteMultipleRequests(Plugin.LocalPluginContext localContext, List<OrganizationRequest> requestsLst, bool continueOnError, bool returnResponses, int defaultBatchSize = 250)
        {
            return Helper.ExecuteMultipleRequests(localContext, requestsLst, continueOnError, returnResponses, defaultBatchSize);
        }
    }
}

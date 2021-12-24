using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm
{
    public class Helper
    {
        /// <summary>
        /// Saves the changes that the OrganizationServic is tracking to Microsoft Dynamics CRM.
        /// </summary>
        /// <param name="continueOnError">Indicates whether further execution of message requests should continue if a fault is returned for the current request being processed.</param>
        /// <param name="returnResponses">Indicates if a response for each message request processed should be returned. </param>
        /// <returns></returns>
        public static ExecuteMultipleResponse ExecuteMultipleRequests(Plugin.LocalPluginContext localContext, List<OrganizationRequest> requestsLst, Boolean continueOnError, Boolean returnResponses, int defaultBatchSize = 250)
        {
            // return reponses
            ExecuteMultipleResponse responseWithResults = null;

            // list of OrganizationRequests
            var organizationRequestsList = new List<OrganizationRequest>();
            // Add the custom requests to the request collection.
            organizationRequestsList.AddRange(requestsLst);

            // if no request is to be sent, exit
            if (organizationRequestsList.Count == 0)
                return null;

            // Create an ExecuteMultipleRequest object.
            var requestWithResults = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = continueOnError,
                    ReturnResponses = returnResponses
                },
            };

            // split requests according to BatchSize
            int batchSize = defaultBatchSize;
            int offset = 0;
            while (organizationRequestsList.Count > offset)
            {
                var tempRequestSet = organizationRequestsList.Skip(offset).Take(batchSize);

                // Create an empty organization request collection.
                requestWithResults.Requests = new OrganizationRequestCollection();
                requestWithResults.Requests.AddRange(tempRequestSet);

                // Execute all the requests in the request collection using a single web method call.
                try
                {
                    responseWithResults =
                        (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);
                }
                catch (FaultException<OrganizationServiceFault> fault)
                {
                    // Check if the maximum batch size has been exceeded. The maximum batch size is only included in the fault if it
                    // the input request collection count exceeds the maximum batch size.
                    if (fault.Detail.ErrorDetails.Contains("MaxBatchSize"))
                    {
                        int maxBatchSize = Convert.ToInt32(fault.Detail.ErrorDetails["MaxBatchSize"]);
                        if (maxBatchSize < requestWithResults.Requests.Count)
                        {
                            // Here you could reduce the size of your request collection and re-submit the ExecuteMultiple request.
                            // For this sample, that only issues a few requests per batch, we will just print out some info. However,
                            // this code will never be executed because the default max batch size is 1000.
                            batchSize = maxBatchSize;
                            continue;
                        }
                    }
                }

                // increment requests counter
                offset += tempRequestSet.Count();
            }

            // return responses
            return responseWithResults;
        }

        public static ExecuteTransactionResponse ExecuteMultipleRequestsTransaction(Plugin.LocalPluginContext localContext, List<OrganizationRequest> requestsLst, Boolean returnResponses, int defaultBatchSize = 250)
        {
            // return reponses
            ExecuteTransactionResponse responseWithResults = null;

            // Create an ExecuteTransactionRequest object.
            ExecuteTransactionRequest requestWithResults = new ExecuteTransactionRequest()
            {
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = returnResponses
            };

            // list of OrganizationRequests
            var organizationRequestsList = new List<OrganizationRequest>();
            // Add the custom requests to the request collection.
            organizationRequestsList.AddRange(requestsLst);

            // if no request is to be sent, exit
            if (organizationRequestsList.Count == 0)
                return null;

            // split requests according to BatchSize
            int batchSize = defaultBatchSize;
            int offset = 0;
            while (organizationRequestsList.Count > offset)
            {
                var tempRequestSet = organizationRequestsList.Skip(offset).Take(batchSize);
                requestWithResults.Requests.AddRange(tempRequestSet);

                // Execute all the requests in the request collection using a single web method call.
                try
                {
                    responseWithResults =
                        (ExecuteTransactionResponse)localContext.OrganizationService.Execute(requestWithResults);
                }
                catch (FaultException<OrganizationServiceFault> fault)
                {
                    // Check if the maximum batch size has been exceeded. The maximum batch size is only included in the fault if it
                    // the input request collection count exceeds the maximum batch size.
                    if (fault.Detail.ErrorDetails.Contains("MaxBatchSize"))
                    {
                        int maxBatchSize = Convert.ToInt32(fault.Detail.ErrorDetails["MaxBatchSize"]);
                        if (maxBatchSize < requestWithResults.Requests.Count)
                        {
                            // Here you could reduce the size of your request collection and re-submit the ExecuteMultiple request.
                            // For this sample, that only issues a few requests per batch, we will just print out some info. However,
                            // this code will never be executed because the default max batch size is 1000.
                            batchSize = maxBatchSize;
                            continue;
                        }
                    }
                }

                // increment requests counter
                offset += tempRequestSet.Count();
            }

            // return responses
            return responseWithResults;
        }
    }
}
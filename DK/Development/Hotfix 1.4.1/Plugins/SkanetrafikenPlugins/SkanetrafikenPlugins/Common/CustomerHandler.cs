using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class CustomerHandler
    {
        #region Public Methods
        // TODO flytta innehåll till extern metod likt de andra?
        public static void ExecuteCustomerSyncronization(IOrganizationService service, string customerId)
        {
            try
            {
                string SyncCustomerServiceUrl = Utilities.GetSetting(service, "cgi_synccustomerservice");

                SyncCustomerService.SyncCustomerRequest request = new SyncCustomerService.SyncCustomerRequest
                {
                    SyncFromCrmtoEPiRequestParameters = new SyncCustomerService.SyncFromCrmtoEPiRequestParameters
                    {
                        CustomerId = customerId // Cyrus ACC contact id "e0f9c312-d2e4-e411-80d7-005056906ae2"
                    }
                };

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(SyncCustomerServiceUrl);

                SyncCustomerService.SyncCustomerClient client = new SyncCustomerService.SyncCustomerClient(myBinding, endPointAddress);

                //SyncCustomerCard.SyncCustomerCardResponse respons = client.SyncCustomerCard(request);
                // TODO varför inte en "vanlig" request?

                SyncCustomerService.SyncFromCrmtoEPiResponseParameters responsParameters = client.SyncCustomer(request.SyncFromCrmtoEPiRequestParameters);

                uint statusCode = responsParameters.StatusCode;

                if (statusCode != 200)
                {
                    string errorMessage = "Ett fel uppstod vid synkronisering av kund. ";
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        errorMessage += " CustomerId: ";
                        errorMessage += customerId;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.Message))
                    {
                        errorMessage += " Felinformation: ";
                        errorMessage += responsParameters.Message;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.ErrorMessage))
                    {
                        errorMessage += " Detaljerat felmeddelande: " + responsParameters.ErrorMessage;
                    }
                    errorMessage += " Statuskod: " + statusCode.ToString();
                    throw new InvalidPluginExecutionException(errorMessage);
                }

            }
            catch (InvalidPluginExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i ExecuteCustomerSyncronization, customerId = {0}. Detaljerat felmeddelande: {1}", customerId, ex.ToString()));
            }
        }
        #endregion
    }
}

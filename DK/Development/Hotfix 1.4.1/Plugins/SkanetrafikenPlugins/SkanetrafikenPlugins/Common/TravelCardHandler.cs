using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class TravelCardHandler
    {
        #region Public Methods
        // TODO flytta innehåll till extern metod likt de andra?
        public static void ExecuteTravelCardSyncronization(IOrganizationService service, string customerId)
        {
            try
            {
                string SyncCustomerCardServiceUrl = Utilities.GetSetting(service, "cgi_synccustomercardservice");

                SyncCustomerCards.SyncCustomerCardsRequest request = new SyncCustomerCards.SyncCustomerCardsRequest
                {
                    SyncFromCrmtoEPiRequestParameters = new SyncCustomerCards.SyncFromCrmtoEPiRequestParameters
                    {
                        CustomerId = customerId // Cyrus ACC contact id "e0f9c312-d2e4-e411-80d7-005056906ae2"
                    }
                };

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(SyncCustomerCardServiceUrl);

                SyncCustomerCards.SyncCustomerCardsClient client = new SyncCustomerCards.SyncCustomerCardsClient(myBinding, endPointAddress);

                //SyncCustomerCard.SyncCustomerCardResponse respons = client.SyncCustomerCard(request);
                // TODO varför inte en "vanlig" request?

                SyncCustomerCards.SyncFromCrmtoEPiResponseParameters responsParameters = client.SyncCustomerCards(request.SyncFromCrmtoEPiRequestParameters);

                uint statusCode = responsParameters.StatusCode;

                if (statusCode != 200)
                {
                    string errorMessage = "Ett fel uppstod vid synkronisering av kort. ";
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
                // previous UserMessageException but I dont know why/how its used, maybe for SL?
                throw new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i ExecuteTravelCardSyncronization, customerId = {0}. Kontakta din administratör. Detaljerat felmeddelande: {1}", customerId, ex.Message));
            }
        }
        #endregion
    }
}

using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    public class TravelCardFixture : PluginFixtureBase
    {
        #region Configs
        private ServerConnection _serverConnection;

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }
        #endregion

        #region Api Call

        public void CallApi(string endpoint, object body, string httpMethod, Type destination, Action<Plugin.LocalPluginContext, string, HttpWebResponse> assert = null, Action<Plugin.LocalPluginContext> clearDataAction = null)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //Set up http request
                string url = endpoint; //$"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = httpMethod;


                try
                {
                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(Convert.ChangeType(body, destination));
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd().Replace("\"", "");
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);
                        assert(localContext, response, httpResponse);
                    }
                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd().Replace("\"", "");
                            assert(localContext, text, httpResponse);
                        }
                    }
                }


            }
        }
        #endregion

        [Test]
        public void FetchBlockedTravelCards()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());


                var queryTravelCard = new QueryExpression()
                {
                    EntityName = TravelCardEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.statuscode, ConditionOperator.Equal, (int)Skanetrafiken.Crm.Schema.Generated.cgi_travelcard_statuscode.Active),
                            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.NotNull),
                            new ConditionExpression(TravelCardEntity.Fields.CreatedOn, ConditionOperator.OnOrAfter, DateTime.Today)
                        }
                    }
                };

                var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, queryTravelCard);

                var blockedCards = new List<string>();
                var nonBlockedCards = new List<string>();
                foreach (var travelCard in travelCards)
                {
                    var card = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.cgi_travelcardnumber);
                    var parsedCard = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, card);
                    if (!string.IsNullOrWhiteSpace(parsedCard.CardNumberField))
                    {
                        if (parsedCard.CardHotlistedField)
                            blockedCards.Add(parsedCard.CardNumberField);
                        else nonBlockedCards.Add(parsedCard.CardNumberField);
                    }
                        
                }

                Console.WriteLine("Blocked cards:");
                foreach (var card in blockedCards)
                    Console.WriteLine(card);

                Console.WriteLine("\n\nNon blocked cards (Or to be blocked):");
                foreach (var card in nonBlockedCards)
                    Console.WriteLine(card);

            }

        }

        [Test]
        public void DeleteTravelCardFromContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var contactId = "68B11877-6771-E911-80F0-005056B61FFF";

                var queryTravelCard = new QueryExpression()
                {
                    EntityName = TravelCardEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Null),
                            new ConditionExpression(TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.Equal, Guid.Parse(contactId))
                        }
                    }
                };

                var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, queryTravelCard);

                XrmHelper.Delete(localContext, travelCard.ToEntityReference());
            }
        }

        [Test]
        public void UnblockTravelCard()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string cardDetailsResponse =
                    ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, "1315138659");

                var test = TravelCardEntity.GetCardDetails(localContext, "1315138659");

                var cardDetails =
                    ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, test);

                if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField)) { 
                }

                if (cardDetails.CardHotlistedField)
                {

                }

            }
        }
    }
}

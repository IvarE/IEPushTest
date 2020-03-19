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
using System.Xml.Serialization;
using static Skanetrafiken.Crm.Entities.TravelCardEntity;

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

        [Test, Category("Debug")]
        public void GetOutstandingCharges()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TravelCardEntity.OutstandingChargesEnvelope));
                //XmlSerializer serializer = new XmlSerializer(typeof(TravelCardEntity.EnvelopeBody));

                //string outstandingChargesResponse = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><ns0:OutstandingChargesResponse xmlns:ns0=\"http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310\"><Message>Kortet har ej några utestående laddningar.</Message><HasOutstandingCharge>false</HasOutstandingCharge><HasExpiredCharge>false</HasExpiredCharge><Amount>0</Amount><ErrorMessage></ErrorMessage><StatusCode>200</StatusCode></ns0:OutstandingChargesResponse></s:Body></s:Envelope>";
                string outstandingChargesResponse = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><ns0:OutstandingChargesResponse xmlns:ns0=\"http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310\"><Message>Kunde ej hitta kort med kortnummer: 1082313501.</Message><HasOutstandingCharge>false</HasOutstandingCharge><HasExpiredCharge>false</HasExpiredCharge><Amount>0</Amount><ErrorMessage>Found no registered card in e - commerce with cardnumber 1082313501.</ErrorMessage><StatusCode>400</StatusCode></ns0:OutstandingChargesResponse></s:Body></s:Envelope >";

                StringReader strReader = new StringReader(outstandingChargesResponse);

                var outstandingCharges = (TravelCardEntity.OutstandingChargesEnvelope)serializer.Deserialize(strReader);

                if(outstandingCharges != null && outstandingCharges.Body != null && outstandingCharges.Body.OutstandingChargesResponse != null)
                {
                    if(outstandingCharges.Body.OutstandingChargesResponse.HasOutstandingCharge == true)
                    {

                    }
                }
                //var outstandingCharges = (TravelCardEntity.EnvelopeBody)serializer.Deserialize(strReader);

            }
            catch (Exception ex)
            {

            }



            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string travelCardNumber = "3326396714";
                string apiOutstandingCharges = "http://v-dkbiz-tst.int.skanetrafiken.com/INTSTDK008.Card/GetOutstandingCharges.svc";


                // MAKE SOAP REQUEST
                string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310'>"
                + "<soapenv:Header/>"
                    + "<soapenv:Body>"
                        + "<ns:OutstandingChargesRequest>"
                            + "<CardNumber> " + travelCardNumber  + "</CardNumber>"
                        + "</ns:OutstandingChargesRequest>"
                    + "</soapenv:Body>"
                + "</soapenv:Envelope> ";

                string soapResponse = "";
                string bizTalkUrl = "";

                //TRY GET SERVICE URL
                try
                {
                    localContext.TracingService.Trace($"Getting url to BizTalk...");
                    bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.cgi_getoutstandingchargesservice);
                    localContext.TracingService.Trace($"Successfully retrieve Biztalk url: " + bizTalkUrl);
                }
                catch (Exception ex)
                {
                    localContext.TracingService.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                    throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
                }

                bizTalkUrl = apiOutstandingCharges;

                // REMOVE SUFFIX OF URL
                string[] parts = bizTalkUrl.Split('/');
                string soapActionAddress = "";
                for (int x = 0; x < parts.Length - 1; x++)
                {
                    soapActionAddress += parts[x];
                    soapActionAddress += "/";
                }

                localContext.TracingService.Trace($"Soap action address for BizTalk: " + soapActionAddress);

                //TRY SEND REQUEST
                try
                {
                    using (var client = new System.Net.WebClient())
                    {
                        // the Content-Type needs to be set to XML
                        client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                        client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                        client.Encoding = Encoding.UTF8;
                        //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");

                        localContext.TracingService.Trace($"Send soap message to biztalk");
                        soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                    }
                }
                catch (WebException ex)
                {
                    localContext.TracingService.Trace("Error while making call to BizTalk. Ex: " + ex.Message);
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception(resp, ex);
                }
                catch (Exception ex)
                {
                    localContext.TracingService.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                    throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
                }

                OutstandingChargesEnvelope envelope = null;

                try
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(CardDetailsEnvelope.Envelope));


                    if (serializer == null)
                        throw new Exception("ParseCardDetails: XmlSerializer is null.");

                    if (string.IsNullOrWhiteSpace(soapResponse))
                        throw new Exception("ParseCardDetails: soapResponse is null.");
                    StringReader strReader = new StringReader(soapResponse);

                    if (strReader == null)
                        throw new Exception("ParseCardDetails: StringReader is null.");

                    envelope = (OutstandingChargesEnvelope)serializer.Deserialize(strReader);
                    
                }
                catch (Exception ex)
                {
                    localContext.TracingService.Trace("Error from ParseCardDetails. Ex: " + ex.Message);
                    throw new Exception($"(ParseCardDetails) error: {ex}");
                }

                if (envelope != null)
                {
                    decimal amountOutstandingCharges = envelope.Body.OutstandingChargesResponse.Amount;

                    if (envelope.Body.OutstandingChargesResponse.Amount > 0)
                    {
                        throw new Exception("OutstandingCharges is above 0");
                    }
                }
            }
        }
    }
}

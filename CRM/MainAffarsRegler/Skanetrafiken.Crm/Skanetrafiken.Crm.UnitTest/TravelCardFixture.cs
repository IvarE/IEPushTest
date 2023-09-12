using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;

namespace Endeavor.Crm.IntegrationTests
{
    [TestFixture]
    public class TravelCardFixture : PluginFixtureBase
    {
        #region Config
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
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Null),
                            new ConditionExpression(TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.Equal, contactId)
                        }
                    }
                };

                var travelCard = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, queryTravelCard);


            }
        }

        [Test]
        public void CreateTravelCard()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var travelCard = new TravelCardEntity() { cgi_TravelCardName = "UnitTest" + DateTime.Now };

                travelCard.Id = XrmHelper.Create(localContext, travelCard);

                var foundTravelCard = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext, new EntityReference(TravelCardEntity.EntityLogicalName, travelCard.Id), new ColumnSet(true));
                Assert.IsNotNull(foundTravelCard, "Travel card could not be found.");
            }
        }

        [Test, Explicit, Category("Debug"), Category("TravelCard")]
        public void ValidateTravelCardResponse()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var doc = TravelCardEntity.GetAndParseCardDetails(localContext, "3331125786");
                TravelCardEntity.ValidateTravelCard(localContext, doc);

            }
        }

        [TestCase("1000076516")]
        public void ValidateTravelCard(string travelCardNumber)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var doc = TravelCardEntity.GetAndParseCardDetails(localContext, travelCardNumber);

                Assert.DoesNotThrow(() => TravelCardEntity.ValidateTravelCard(localContext, doc));
            }
        }

        [Test]
        public void FindLegitCardForTest()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());


                var travelCardQuery = new QueryExpression()
                {
                    EntityName = TravelCardEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.ed_ModifiedInBiff, ConditionOperator.OnOrAfter, new DateTime(2017,12,28)),
                            new ConditionExpression(TravelCardEntity.Fields.ed_ModifiedInBiff, ConditionOperator.OnOrBefore, new DateTime(2018,05,01))
                        }
                    }
                };

                var travelCardsInCRM = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, travelCardQuery);
                Assert.NotNull(travelCardsInCRM);
                //Find travel cards in BIFF
                foreach (var card in travelCardsInCRM)
                {
                    var tCard = TravelCardEntity.GetAndParseCardDetails(localContext, card.cgi_travelcardnumber);

                    if (tCard.Body == null ||
                        tCard.Body.GetCardDetails2Response == null ||
                        tCard.Body.GetCardDetails2Response.GetCardDetails2Result == null ||
                        tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2 == null ||
                        tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation == null ||
                        tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails == null ||
                        tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails == null)
                        continue;

                    var CardInformation = tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation;
                    var PurseDetails = tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails;
                    var PeriodDetails = tCard.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails;

                    if (CardInformation.CardHotlisted == true ||
                        PurseDetails.Hotlisted == true ||
                        PurseDetails.Balance < 1)
                        continue;

                    Console.WriteLine($"Valid travel card: {card.cgi_travelcardnumber}");

                }
            }
        }

        [TestCase("1200677750")]
        public void InvalidateTravelCard(string travelCardNumber)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    var card = TravelCardEntity.GetAndParseCardDetails(localContext, travelCardNumber);

                    //TravelCardEntity.ValidateValueCodeApproval(localContext, card);
                    if (card.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardHotlisted != true)
                    {
                        var cardBlock = TravelCardEntity.InvalidateCardAndGetResponse(localContext, travelCardNumber);
                        //TravelCardEntity.ValidateCardBlockResult(cardBlock);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        
 
    }
}

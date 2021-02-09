using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
using CGIXrmTravelCard.TravelCardClasses.CrmSqlClasses;
using CGIXrmTravelCard.TravelCardClasses.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;

namespace CGIXrmTravelCard.TravelCardClasses
{
    public class TravelCardManager
    {
        #region Declarations ------------------------------------------------------------------------------------------

        XrmManager _xrmManager;
        private readonly object _lockSql = new object();
        private bool useFetchXml = true;

        #endregion

        #region Constructors ------------------------------------------------------------------------------------------

        public TravelCardManager()
        {
            _xrmManager = _initXrmManager();
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        private XrmManager _initXrmManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();

                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings. Ex: " + ex.Message);
            }
        }

        private GetCRMCardDetailsResponse GetCard(string cardnumber, string storedProcedureName = "")
        {
            GetCRMCardDetailsResponse response = new GetCRMCardDetailsResponse();

            if (useFetchXml || storedProcedureName == "")
            {

                try
                {
                    #region FetchXML TravelCard
                    var _fx = "";
                    _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                    _fx += "<entity name='cgi_travelcard'>";
                    _fx += "<all-attributes />";
                    _fx += "<filter type='and'>";
                    _fx += "<condition attribute='cgi_travelcardnumber' operator='like' value='%" + cardnumber + "%' />";
                    _fx += "</filter>";
                    _fx += "<link-entity name='account' from='accountid' to='cgi_accountid' visible='false' link-type='outer' alias='account'>";
                    _fx += "<attribute name='accountnumber' />";
                    _fx += "</link-entity>";
                    _fx += "<link-entity name='contact' from='contactid' to='cgi_contactid' visible='false' link-type='outer' alias='contact'>";
                    _fx += "<attribute name='cgi_contactnumber' />";
                    _fx += "</link-entity>";
                    _fx += "</entity>";
                    _fx += "</fetch>";

                    #endregion


                    EntityCollection travelcards = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));
                    /*
                    ObservableCollection<Entity> travelcards_ = _xrmManager.Get<Entity>(_fx);
                    EntityCollection travelcards = null;
                    
                    foreach(Entity e in travelcards_)
                    {
                        travelcards.Entities.Add(e);
                    }
                    */

                    if (travelcards == null || travelcards.Entities.Count <= 0)
                        return null;

                    if (travelcards.Entities.First() == null)
                        return null;

                    //int i = 0;
                    //foreach (var entity in travelcards.Entities)
                    //{
                    //    if (i == 0)
                    //    {
                    //        response.Card = TransformEntityToCard(entity);
                    //        response.ErrorMessage = "";
                    //        return response;
                    //    }
                    //}

                    Card card = TransformEntityToCard(travelcards.Entities.First());

                    if (card == null)
                        return null;

                    response.Card = card;
                    response.ErrorMessage = "";

                }
                catch (FaultException faultex)
                {
                    response.Card = null;
                    response.ErrorMessage = faultex.Message;
                }
                catch (Exception ex)
                {
                    response.Card = null;
                    response.ErrorMessage = ex.Message;
                }
            }
            else
            {
                #region old sp_GetTravelCardExtended & sp_GetTravelCard
                try
                {
                    SqlConnection sqlCon = OpenSQL();
                    using (SqlCommand command = new SqlCommand(storedProcedureName, sqlCon))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter { ParameterName = "@cardnumber", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = cardnumber });

                        XmlReader reader = command.ExecuteXmlReader();
                        CardSqlList cards;
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(CardSqlList));
                            cards = ser.Deserialize(reader) as CardSqlList;
                        }
                        reader.Close();
                        CloseSQL(sqlCon);

                        if (cards != null && cards.Cards != null && cards.Cards.Any())
                        {
                            response.Card = new Card();
                            response.Card = cards.Cards[0];
                            response.ErrorMessage = "";
                        }
                    }
                }
                catch (FaultException faultex)
                {
                    response.Card = null;
                    response.ErrorMessage = faultex.Message;
                }
                catch (Exception ex)
                {
                    response.Card = null;
                    response.ErrorMessage = ex.Message;
                }
                #endregion
            }

            return response;
        }

        private Card TransformEntityToCard(Entity travelCard)
        {

            //Card returnValue = null;
            Card returnValue = new Card();
            try
            {
                if (travelCard.Contains("cgi_validfrom"))
                    returnValue.ValidFrom = travelCard.GetAttributeValue<DateTime>("cgi_validfrom").ToShortDateString();

                
                if (travelCard.Contains("cgi_validto"))
                    returnValue.ValidTo = travelCard.GetAttributeValue<DateTime>("cgi_validto").ToShortDateString();
                
                if (travelCard.Contains("cgi_travelcardid"))
                    returnValue.TravelCardID = travelCard.GetAttributeValue<Guid>("cgi_travelcardid");
                
                if (travelCard.Contains("cgi_travelcardname"))
                    returnValue.TravelCardName = travelCard.GetAttributeValue<string>("cgi_travelcardname");
                
                if (travelCard.Contains("cgi_blocked"))
                    returnValue.Blocked = Convert.ToInt32(travelCard.GetAttributeValue<bool>("cgi_blocked"));

                if (travelCard.Contains("cgi_cardtypeid"))
                    returnValue.CardTypeId = travelCard.GetAttributeValue<EntityReference>("cgi_cardtypeid").Id;

                if (travelCard.Contains("cgi_cardtypeidname"))
                    returnValue.CardTypeIdName = travelCard.GetAttributeValue<EntityReference>("cgi_cardtypeidname").Name;

                if (travelCard.Contains("cgi_numberofzones"))
                    returnValue.NumberOfZones = travelCard.GetAttributeValue<string>("cgi_numberofzones");

                if (travelCard.Contains("cgi_periodic_card_type"))
                    returnValue.PeriodeicCardType = travelCard.GetAttributeValue<string>("cgi_periodic_card_type");
                
                if (travelCard.Contains("cgi_value_card_type"))
                    returnValue.Value_card_type = travelCard.GetAttributeValue<string>("cgi_value_card_type");
                
                if (travelCard.Contains("cgi_accountid"))
                    returnValue.AccountId = travelCard.GetAttributeValue<EntityReference>("cgi_accountid").Id;

                if (travelCard.Contains("account.accountnumber"))
                    returnValue.AccountNumber = travelCard.GetAttributeValue<string>("account.accountnumber");

                if (travelCard.Contains("cgi_accountid"))
                    returnValue.AccountIdName = travelCard.GetAttributeValue<EntityReference>("cgi_accountid").Name;
                
                if (travelCard.Contains("statecode"))
                    returnValue.StateCode = travelCard.GetAttributeValue<OptionSetValue>("statecode").Value;

                if (travelCard.Contains("cgi_autoloadstatus"))
                    returnValue.Autoloadstatus = travelCard.GetAttributeValue<int>("cgi_autoloadstatus");

                if (travelCard.Contains("cgi_autoloadstatusname"))
                    returnValue.Autoloadstatusname = travelCard.GetAttributeValue<string>("cgi_autoloadstatusname");

                if (travelCard.Contains("cgi_autoloadconnectiondate"))
                    returnValue.Autoloadconnectiondate = travelCard.GetAttributeValue<string>("cgi_autoloadconnectiondate");

                if (travelCard.Contains("cgi_autoloaddisconnectiondate"))
                    returnValue.Autoloaddisconnectiondate = travelCard.GetAttributeValue<string>("cgi_autoloaddisconnectiondate");

                if (travelCard.Contains("cgi_creditcardmask"))
                    returnValue.Creditcardmask = travelCard.GetAttributeValue<string>("cgi_creditcardmask");

                if (travelCard.Contains("cgi_failedattemptstochargemoney"))
                    returnValue.Failedattemptstochargemoney = travelCard.GetAttributeValue<int>("cgi_failedattemptstochargemoney").ToString();

                if (travelCard.Contains("cgi_latestfailedattempt"))
                    returnValue.Latestfailedattempt = travelCard.GetAttributeValue<string>("cgi_latestfailedattempt");

                if (travelCard.Contains("cgi_contactid"))
                    returnValue.ContactId = travelCard.GetAttributeValue<EntityReference>("cgi_contactid").Id;

                if (travelCard.Contains("cgi_contactid.cgi_contactnumber"))
                    returnValue.Contactnumber = travelCard.GetAttributeValue<string>("cgi_contactid.cgi_contactnumber");

                if (travelCard.Contains("cgi_contactid"))
                    returnValue.ContactIdName = travelCard.GetAttributeValue<EntityReference>("cgi_contactid").Name;

                return returnValue;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private SqlConnection OpenSQL()
        {
            //Removed to test if all connections to integrationsDB are removed
            /*
            lock (_lockSql)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            */
            return null;
        }

        private void CloseSQL(SqlConnection connection)
        {
            lock (_lockSql)
            {
                if (connection != null)
                    connection.Close();
            }
        }
        #endregion

        #region Public Methods ----------------------------------------------------------------------------------------

        public GetCardDetailsResponse GetCardDetails(string cardnumber)
        {
            GetCardDetailsResponse response = new GetCardDetailsResponse();

            try
            {
                using (var client = new webclientx())
                {
                    string address1 = WebConfigurationManager.AppSettings["ehandeladdressCardDetails"];
                    string soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardDetails"];
                    string cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardDetails"];

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + address1 + "'>";
                    data += "    <soapenv:Header/>";
                    data += "    <soapenv:Body>";
                    data += "        <int:GetCardDetails2>";
                    data += "            <int:CardSerialNumber>" + cardnumber + "</int:CardSerialNumber>";
                    data += "        </int:GetCardDetails2>";
                    data += "    </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Timeout = 120000;
                    string responseStr = client.UploadString("" + cardDetailsServiveAddress + "", data);

                    response.ErrorMessage = "";
                    response.CardDetails = responseStr;
                }
            }
            catch (FaultException faultex)
            {
                response.CardDetails = "";
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.CardDetails = "";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Extended method 
        /// </summary>
        public GetCRMCardDetailsResponse GetCardFromCRMExtended(string cardnumber)
        {
            return GetCard(cardnumber, "sp_GetTravelCardExtended");
        }

        public GetCRMCardDetailsResponse GetCardFromCRM(string cardnumber)
        {
            return GetCard(cardnumber, "sp_GetTravelCard");
        }


        public GetOutstandingChargesResponse GetGetOutstandingCharges(string cardnumber)
        {
            GetOutstandingChargesResponse response = new GetOutstandingChargesResponse();

            try
            {
                using (var client = new webclientx())
                {
                    string address1 = WebConfigurationManager.AppSettings["ehandeladdressChargesDetails"];
                    string soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressChargesDetails"];
                    string cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressChargesDetails"];

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "       <soapenv:Body>";
                    data += "           <ns:OutstandingChargesRequest>";
                    data += "               <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "           </ns:OutstandingChargesRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Timeout = 120000;
                    string responseStr = client.UploadString("" + cardDetailsServiveAddress + "", data);

                    response.ErrorMessage = "";
                    response.OutstandingCharges = responseStr;
                }
            }
            catch (FaultException faultex)
            {
                response.OutstandingCharges = "";
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.OutstandingCharges = "";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public RechargeCardResponse RechargeCard(string cardnumber)
        {
            RechargeCardResponse response = new RechargeCardResponse();

            try
            {
                using (var client = new webclientx())
                {
                    string address1 = WebConfigurationManager.AppSettings["ehandeladdressRechargeCard"];
                    string soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressRechargeCard"];
                    string cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressRechargeCard"];

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "   <soapenv:Body>";
                    data += "       <ns:RechargeCardRequest>";
                    data += "           <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "       </ns:RechargeCardRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Timeout = 120000;
                    string responseStr = client.UploadString("" + cardDetailsServiveAddress + "", data);

                    response.ErrorMessage = "";
                    response.RechargeCard = responseStr;
                }
            }
            catch (FaultException faultex)
            {
                response.RechargeCard = "";
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.RechargeCard = "";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public GetCardTransactionsResponse GetCardTransactions(string cardnumber, string maxtrasactions, string datefrom, string dateto)
        {
            GetCardTransactionsResponse response = new GetCardTransactionsResponse();

            try
            {
                using (var client = new webclientx())
                {
                    string address1 = WebConfigurationManager.AppSettings["ehandeladdressCardTransactions"];
                    string soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardTransactions"];
                    string cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardTransactions"];

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "   <soapenv:Body>";
                    data += "       <int:GetCardTransactions>";
                    data += "           <int:CardSerialNumber>" + cardnumber + "</int:CardSerialNumber>";
                    data += "           <int:MaxTransactions>" + maxtrasactions + "</int:MaxTransactions>";
                    data += "           <int:StartDate>" + datefrom + "</int:StartDate>";
                    data += "           <int:EndDate>" + dateto + "</int:EndDate>";
                    data += "       </int:GetCardTransactions>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Timeout = 120000;
                    string responseStr = client.UploadString("" + cardDetailsServiveAddress + "", data);

                    response.ErrorMessage = "";
                    response.Transactions = responseStr;
                }
            }
            catch (FaultException faultex)
            {
                response.Transactions = "";
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.Transactions = "";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public GetZoneNamesResponse GetZoneNames()
        {
            GetZoneNamesResponse response = new GetZoneNamesResponse();

            if (useFetchXml)
            {

                try
                {
                    #region FetchXML Zones

                    var _fx = "";
                    _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                    _fx += "<entity name='cgi_zonename'>";
                    _fx += "<all-attributes />";
                    _fx += "</entity>";
                    _fx += "</fetch>";

                    #endregion

                    EntityCollection zones = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                    if (zones == null || zones.Entities == null || zones.Entities.Count <= 0)
                        return null;

                    List<Zone> listOfZones = new List<Zone>();
                    listOfZones = TransformEntityCollectionToListOfZones(zones);
                    //return settings.Entities.First();

                    response.Zones = listOfZones;
                    response.ErrorMessage = "";
                }
                catch (Exception ex)
                {
                    response.Zones = null;
                    response.ErrorMessage = ex.Message;
                }
            }
            else
            {
                #region old sp_GetZoneNames
                try
                {

                    SqlConnection sqlCon = OpenSQL();
                    using (SqlCommand command = new SqlCommand("sp_GetZoneNames", sqlCon))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        XmlReader reader = command.ExecuteXmlReader();
                        ZoneSqlList zonenames;
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(ZoneSqlList));
                            zonenames = ser.Deserialize(reader) as ZoneSqlList;
                        }
                        reader.Close();
                        CloseSQL(sqlCon);

                        if (zonenames != null && zonenames.Zones != null && zonenames.Zones.Any())
                        {
                            response.Zones = new List<Zone>();
                            response.Zones = zonenames.Zones;
                            response.ErrorMessage = "";
                        }
                    }
                }
                catch (FaultException faultex)
                {
                    response.Zones = null;
                    response.ErrorMessage = faultex.Message;
                }
                catch (Exception ex)
                {
                    response.Zones = null;
                    response.ErrorMessage = ex.Message;
                }
                #endregion
            }

            return response;
        }

        private List<Zone> TransformEntityCollectionToListOfZones(EntityCollection zones)
        {
            List<Zone> returnValues = new List<Zone>();

            try
            {
                foreach (Entity zone in zones.Entities)
                {
                    Zone returnValue = new Zone();

                    if (zone.Contains("cgi_zoneid"))
                        returnValue.ZoneId = zone.GetAttributeValue<string>("cgi_zoneid");

                    if (zone.Contains("cgi_name"))
                        returnValue.ZoneName = zone.GetAttributeValue<string>("cgi_name");

                    returnValues.Add(returnValue);

                }
            }
            catch (Exception e)
            {
                return null;
            }

            return returnValues;
        }

        private List<TravelCardTransaction> TransformEntityCollectionToListOfTravelCardTransactions(EntityCollection travelCardTransaction)
        {
            List<TravelCardTransaction> returnValues = new List<TravelCardTransaction>();
            
            try
            {
                foreach (Entity transaction in travelCardTransaction.Entities)
                {
                    TravelCardTransaction returnValue = new TravelCardTransaction();

                    #region Get Attributes from TravelCardTransaction
                    if (transaction.Contains("cgi_amount"))
                        returnValue.Amount = transaction.GetAttributeValue<string>("cgi_amount");

                    if (transaction.Contains("cgi_travelcardid"))
                        returnValue.CardId = transaction.GetAttributeValue<EntityReference>("cgi_travelcardid").Id;

                    if (transaction.Contains("cgi_cardsect"))
                        returnValue.CardSect = transaction.GetAttributeValue<string>("cgi_cardsect");

                    if (transaction.Contains("cgi_caseid"))
                        returnValue.CaseId = transaction.GetAttributeValue<EntityReference>("cgi_caseid").Id;

                    if (transaction.Contains("cgi_currency"))
                        returnValue.Currency = transaction.GetAttributeValue<string>("cgi_currency");

                    if (transaction.Contains("cgi_date"))
                        returnValue.Date = transaction.GetAttributeValue<string>("cgi_date");

                    if (transaction.Contains("cgi_deviceid"))
                        returnValue.DeviceID = transaction.GetAttributeValue<string>("cgi_deviceid");

                    if (transaction.Contains("cgi_travelcardidname"))
                        returnValue.Name = transaction.GetAttributeValue<string>("cgi_travelcardidname");

                    if (transaction.Contains("cgi_origzone"))
                        returnValue.OrigZone = transaction.GetAttributeValue<string>("cgi_origzone");

                    if (transaction.Contains("cgi_origzonename"))
                        returnValue.OrigZoneName = transaction.GetAttributeValue<string>("cgi_origzonename");

                    if (transaction.Contains("cgi_rectype"))
                        returnValue.RecType = transaction.GetAttributeValue<string>("cgi_rectype");

                    if (transaction.Contains("cgi_route"))
                        returnValue.Route = transaction.GetAttributeValue<string>("cgi_route");

                    if (transaction.Contains("cgi_time"))
                        returnValue.Time = transaction.GetAttributeValue<string>("cgi_time");

                    if (transaction.Contains("cgi_travelcardtransactionid"))
                        returnValue.Transactionid = transaction.GetAttributeValue<Guid>("cgi_travelcardtransactionid");

                    if (transaction.Contains("cgi_travelcard"))
                        returnValue.TravelCard = transaction.GetAttributeValue<string>("cgi_travelcard");

                    if (transaction.Contains("cgi_txntype"))
                        returnValue.TxnType = transaction.GetAttributeValue<string>("cgi_txntype");
                    #endregion

                    returnValues.Add(returnValue);

                }
            }
            catch (Exception e)
            {
                return null;
            }

            return returnValues;
        }

        public GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid)
        {
            GetTravelCardTransactionsResponse response = new GetTravelCardTransactionsResponse();

            if (useFetchXml)
            {
                try
                {
                    #region FetchXML TravelCardTransactions

                    var _fx = "";
                    _fx += "<fetch>"
                       + "<entity name='cgi_travelcardtransaction' >"
                       + "<attribute name='cgi_amount' />"
                       + "<attribute name='cgi_cardsect' />"
                       + "<attribute name='cgi_caseid' />"
                       + "<attribute name='cgi_currency' />"
                       + "<attribute name='cgi_date' />"
                       + "<attribute name='cgi_deviceid' />"
                       + "<attribute name='cgi_origzone' />"
                       + "<attribute name='cgi_origzonename' />"
                       + "<attribute name='cgi_rectype' />"
                       + "<attribute name='cgi_route' />"
                       + "<attribute name='cgi_time' />"
                       + "<attribute name='cgi_travelcardtransactionid' />"
                       + "<attribute name='cgi_travelcard' />"
                       + "<attribute name='cgi_txntype' />"
                       + "<attribute name='cgi_travelcardidname' />"
                       + "<attribute name='cgi_travelcardid' />"
                       + "<filter type='and' >"
                       + "<condition attribute='cgi_caseid' operator='eq' value='" + travelcardtransactionid + "' />"
                       + "</filter>"
                       + "</entity>"
                       + "</fetch>";

                    #endregion

                    EntityCollection transactions = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                    if (transactions == null || transactions.Entities == null || transactions.Entities.Count <= 0)
                        return null;

                    List<TravelCardTransaction> listOfTravelCardTransactions = new List<TravelCardTransaction>();
                    listOfTravelCardTransactions = TransformEntityCollectionToListOfTravelCardTransactions(transactions);
                    //return settings.Entities.First();

                    response.TravelCardTransactions = listOfTravelCardTransactions;
                    response.ErrorMessage = "";
                }
                catch (Exception ex)
                {
                    response.TravelCardTransactions = null;
                    response.ErrorMessage = ex.Message;
                }

            }
            else
            {

                try
                {
                    SqlConnection sqlCon = OpenSQL();
                    using (SqlCommand command = new SqlCommand("sp_GetSavedTravelCardTransactions", sqlCon))
                    {
                        Guid id = new Guid(travelcardtransactionid);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter { ParameterName = "@transactionid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = id });

                        XmlReader reader = command.ExecuteXmlReader();
                        CardTransactionSqlList cardtransactions;
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(CardTransactionSqlList));
                            cardtransactions = ser.Deserialize(reader) as CardTransactionSqlList;
                        }
                        reader.Close();
                        CloseSQL(sqlCon);

                        if (cardtransactions != null && cardtransactions.TravelCardTransactions != null && cardtransactions.TravelCardTransactions.Any())
                        {
                            response.TravelCardTransactions = new List<TravelCardTransaction>();
                            response.TravelCardTransactions = cardtransactions.TravelCardTransactions;
                            response.ErrorMessage = "";
                        }
                    }
                }
                catch (FaultException faultex)
                {
                    response.TravelCardTransactions = null;
                    response.ErrorMessage = faultex.Message;
                }
                catch (Exception ex)
                {
                    response.TravelCardTransactions = null;
                    response.ErrorMessage = ex.Message;
                }
            }

            return response;
        }
        #endregion
    }
}
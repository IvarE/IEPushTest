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

namespace CGIXrmTravelCard.TravelCardClasses
{
    public class TravelCardManager
    {
        #region Declarations ------------------------------------------------------------------------------------------

        XrmManager _xrmManager;
        private readonly object _lockSql = new object();

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
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        private GetCRMCardDetailsResponse GetCard(string cardnumber, string storedProcedureName)
        {
            GetCRMCardDetailsResponse response = new GetCRMCardDetailsResponse();

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

            return response;
        }


        private SqlConnection OpenSQL()
        {
            lock (_lockSql)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
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

            return response;
        }

        public GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid)
        {
            GetTravelCardTransactionsResponse response = new GetTravelCardTransactionsResponse();

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

            return response;
        }
        #endregion
    }
}
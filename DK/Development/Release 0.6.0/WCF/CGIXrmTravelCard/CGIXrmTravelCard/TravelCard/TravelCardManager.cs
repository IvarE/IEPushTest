using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Crm;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using System.Net;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

using System.Web.Configuration;
using System.Diagnostics;

namespace CGIXrmTravelCard
{
    public class TravelCardManager
    {

        XrmManager _xrmManager;
        private object LockSql = new object();
        
        public TravelCardManager()
        {
            _xrmManager = _initXrmManager();
        }

        private XrmManager _initXrmManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    return xrmMgr;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        private SqlConnection OpenSQL()
        {
            lock (LockSql)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
        }

        private void CloseSQL(SqlConnection connection)
        {
            lock (LockSql)
            {
                if (connection != null)
                    connection.Close();
            }
        }

        internal GetCardDetailsResponse GetCardDetails(string cardnumber)
        {
            GetCardDetailsResponse _response = new GetCardDetailsResponse();
            
            try
            {
                /*
                using (var client = new WebClient())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressCardDetails"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardDetails"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardDetails"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + _address1 + "'>";
                    data += "    <soapenv:Header/>";
                    data += "    <soapenv:Body>";
                    data += "        <int:GetCardDetails2>";
                    data += "            <int:CardSerialNumber>" + cardnumber + "</int:CardSerialNumber>";
                    data += "        </int:GetCardDetails2>";
                    data += "    </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    string response = client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.CardDetails = response;
                }
                */

                using (var _client = new webclientx())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressCardDetails"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardDetails"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardDetails"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + _address1 + "'>";
                    data += "    <soapenv:Header/>";
                    data += "    <soapenv:Body>";
                    data += "        <int:GetCardDetails2>";
                    data += "            <int:CardSerialNumber>" + cardnumber + "</int:CardSerialNumber>";
                    data += "        </int:GetCardDetails2>";
                    data += "    </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    _client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    _client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _client.Timeout = 120000;
                    string response = _client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.CardDetails = response;
                }
            }
            catch (FaultException faultex)
            {
                _response.CardDetails = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.CardDetails = "";
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }

        internal GetCRMCardDetailsResponse GetCardFromCRM(string cardnumber)
        {
            GetCRMCardDetailsResponse _response = new GetCRMCardDetailsResponse();

            try
            {
                SqlConnection sqlCon = OpenSQL();
                CardSqlList cards = null;
                using (SqlCommand command = new SqlCommand("sp_GetTravelCard", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@cardnumber", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = cardnumber });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(CardSqlList));
                        cards = ser.Deserialize(reader) as CardSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (cards != null && cards.Cards != null && cards.Cards.Count() > 0)
                    {
                        _response.Card = new Card();
                        _response.Card = cards.Cards[0];
                        _response.ErrorMessage = "";
                    }
                }
            }
            catch (FaultException faultex)
            {
                _response.Card = null;
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.Card = null;
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }

        internal GetOutstandingChargesResponse GetGetOutstandingCharges(string cardnumber)
        {
            GetOutstandingChargesResponse _response = new GetOutstandingChargesResponse();

            try
            {
                /*
                using (var client = new WebClient())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressChargesDetails"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressChargesDetails"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressChargesDetails"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + _address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "       <soapenv:Body>";
                    data += "           <ns:OutstandingChargesRequest>";
                    data += "               <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "           </ns:OutstandingChargesRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    string response = client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.OutstandingCharges = response;
                }
                */

                using (var _client = new webclientx())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressChargesDetails"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressChargesDetails"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressChargesDetails"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + _address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "       <soapenv:Body>";
                    data += "           <ns:OutstandingChargesRequest>";
                    data += "               <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "           </ns:OutstandingChargesRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    _client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    _client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _client.Timeout = 120000;
                    string response = _client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.OutstandingCharges = response;
                }
            }
            catch (FaultException faultex)
            {
                _response.OutstandingCharges = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.OutstandingCharges = "";
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }

        internal RechargeCardResponse RechargeCard(string cardnumber)
        {
            RechargeCardResponse _response = new RechargeCardResponse();

            try
            {
                /*
                using (var client = new WebClient())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressRechargeCard"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressRechargeCard"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressRechargeCard"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + _address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "   <soapenv:Body>";
                    data += "       <ns:RechargeCardRequest>";
                    data += "           <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "       </ns:RechargeCardRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    string response = client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.RechargeCard = response;
                }
                */

                using (var _client = new webclientx())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressRechargeCard"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressRechargeCard"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressRechargeCard"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + _address1 + "'>";
                    data += "   <soapenv:Header/>";
                    data += "   <soapenv:Body>";
                    data += "       <ns:RechargeCardRequest>";
                    data += "           <CardNumber>" + cardnumber + "</CardNumber>";
                    data += "       </ns:RechargeCardRequest>";
                    data += "   </soapenv:Body>";
                    data += "</soapenv:Envelope>";

                    // the Content-Type needs to be set to XML
                    _client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    _client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _client.Timeout = 120000;
                    string response = _client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.RechargeCard = response;
                }
            }
            catch (FaultException faultex)
            {
                _response.RechargeCard = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.RechargeCard = "";
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }

        internal GetCardTransactionsResponse GetCardTransactions(string cardnumber, string maxtrasactions, string datefrom, string dateto)
        {
            GetCardTransactionsResponse _response = new GetCardTransactionsResponse();

            try
            {
                /*
                using (var client = new WebClient())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressCardTransactions"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardTransactions"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardTransactions"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + _address1 + "'>";
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
                    client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    string response = client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.Transactions = response;
                }
                */

                using (var _client = new webclientx())
                {
                    string _address1 = WebConfigurationManager.AppSettings["ehandeladdressCardTransactions"].ToString();
                    string _soapActionAddress = WebConfigurationManager.AppSettings["soapActionAddressCardTransactions"].ToString();
                    string _cardDetailsServiveAddress = WebConfigurationManager.AppSettings["cardDetailsServiceAddressCardTransactions"].ToString();

                    var data = "";
                    data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + _address1 + "'>";
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
                    _client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    _client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _client.Timeout = 120000;
                    string response = _client.UploadString("" + _cardDetailsServiveAddress + "", data);

                    _response.ErrorMessage = "";
                    _response.Transactions = response;
                }
            }
            catch (FaultException faultex)
            {
                _response.Transactions = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.Transactions = "";
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }

        internal GetZoneNamesResponse GetZoneNames()
        {
            GetZoneNamesResponse _response = new GetZoneNamesResponse();

            try
            {
                SqlConnection sqlCon = OpenSQL();
                ZoneSqlList zonenames = null;
                using (SqlCommand command = new SqlCommand("sp_GetZoneNames", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(ZoneSqlList));
                        zonenames = ser.Deserialize(reader) as ZoneSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (zonenames != null && zonenames.Zones != null && zonenames.Zones.Count() > 0)
                    {
                        _response.Zones = new List<Zone>();
                        _response.Zones = zonenames.Zones;
                        _response.ErrorMessage = "";
                    }
                }
            }
            catch (FaultException faultex)
            {
                _response.Zones = null;
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.Zones = null;
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }

        internal GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid)
        {
            GetTravelCardTransactionsResponse _response = new GetTravelCardTransactionsResponse();

            try
            {
                SqlConnection sqlCon = OpenSQL();
                CardTransactionSqlList _cardtransactions = null;
                using (SqlCommand command = new SqlCommand("sp_GetSavedTravelCardTransactions", sqlCon))
                {
                    Guid _id = new Guid(travelcardtransactionid);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@transactionid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = _id });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(CardTransactionSqlList));
                        _cardtransactions = ser.Deserialize(reader) as CardTransactionSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (_cardtransactions != null && _cardtransactions.TravelCardTransactions != null && _cardtransactions.TravelCardTransactions.Count() > 0)
                    {
                        _response.TravelCardTransactions = new List<TravelCardTransaction>();
                        _response.TravelCardTransactions = _cardtransactions.TravelCardTransactions;
                        _response.ErrorMessage = "";
                    }
                }
            }
            catch (FaultException faultex)
            {
                _response.TravelCardTransactions = null;
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.TravelCardTransactions = null;
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }
        

    }
}
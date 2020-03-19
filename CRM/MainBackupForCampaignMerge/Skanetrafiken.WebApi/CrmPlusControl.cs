using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Endeavor.Crm.Extensions;
using System.Web;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Newtonsoft.Json;
using System.Web.Caching;
using Skanetrafiken.Crm.Properties;
using Microsoft.Xrm.Sdk.Client;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;

namespace Skanetrafiken.Crm.Controllers
{
    public class CrmPlusControl
    {

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string _generateContextString = "localContext";
        private static readonly byte[] _entropyString = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        //private static System.Diagnostics.Stopwatch _stopwatch;
        //private static CrmServiceClient _conn;

        private static string NonLoginRefillSubject = "Ladda Kort";
        private static string CreateAccountSubject = "Skapa mitt konto";


        internal static HttpResponseMessage PingConnection()
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (localContext?.OrganizationService != null)
                    {
                        HttpResponseMessage resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                        return resp1;
                    }
                    else
                    {
                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        resp.Content = new StringContent("Could not connect to CRM");
                        return resp;
                    }
                }
            }
            catch (Exception e)
            {
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                resp.Content = new StringContent(e.Message);
                return resp;
            }
        }

        internal static HttpResponseMessage SalesOrderPut(SalesOrderInfo info)
        {
            HttpResponseMessage resp = null;
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    resp = ValidateSalesOrderInfo(localContext, info, true);
                    if (!HttpStatusCode.OK.Equals(resp.StatusCode))
                        return resp;

                    SalesOrderEntity salesOrder = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, new ColumnSet(false),
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, info.OrderNo)
                            }
                        });
                    if (salesOrder == null)
                    {
                        resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        resp.Content = new StringContent(string.Format(Resources.NoSalesOrderFoundWithInfo, info.OrderNo));
                        return resp;
                    }
                    info.Guid = salesOrder.Id.ToString();
                    SalesOrderEntity updateSalesOrder = new SalesOrderEntity
                    {
                        Id = salesOrder.Id,
                        statuscode = Schema.Generated.ed_salesorder_statuscode.OrderSent
                    };
                    XrmHelper.Update(localContext, updateSalesOrder);

                    foreach (Productinfo prodInfo in info.Productinfos)
                    {
                        TravelCardEntity travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, new ColumnSet(false),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(TravelCardEntity.Fields.ed_ReferenceNo, ConditionOperator.Equal, prodInfo.Reference)
                                }
                            });
                        if (travelCard == null)
                        {
                            resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            resp.Content = new StringContent(string.Format(Resources.NoTravelCardFoundWithInfo, prodInfo.Reference));
                            return resp;
                        }
                        string responseStr = null;
#if !DEV
                        // Copy functionality from GetCardDetails and use that information
                        using (var client = new WebClientX())
                        {
                            string ehandeladdressCardDetails = Settings.Default.ehandeladdressCardDetails;
                            string soapActionAddressCardDetails = Settings.Default.soapActionAddressCardDetails;
                            string cardDetailsServiceAddressCardDetails = Settings.Default.cardDetailsServiceAddressCardDetails;

                            var data = "";
                            data += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='" + ehandeladdressCardDetails + "'>";
                            data += "    <soapenv:Header/>";
                            data += "    <soapenv:Body>";
                            data += "        <int:GetCardDetails2>";
                            data += "            <int:CardSerialNumber>" + prodInfo.Serial + "</int:CardSerialNumber>";
                            data += "        </int:GetCardDetails2>";
                            data += "    </soapenv:Body>";
                            data += "</soapenv:Envelope>";

                            // the Content-Type needs to be set to XML
                            client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                            client.Headers.Add("SOAPAction", "\"" + soapActionAddressCardDetails + "");
                            client.Timeout = 120000;
#if TEST
                            _log.Info($"dataString = {data}");
                            _log.Info($"adressString = {cardDetailsServiceAddressCardDetails}");
#endif
                            responseStr = client.UploadString("" + cardDetailsServiceAddressCardDetails + "", data);
                        }
#endif
                        TravelCardEntity updateEntity = new TravelCardEntity
                        {
                            Id = travelCard.Id,
                            cgi_travelcardnumber = prodInfo.Serial
                        };

                        if (!string.IsNullOrWhiteSpace(responseStr))
                        {
                            _log.Debug($"Result of call to BIFF: {responseStr}");
                            XDocument doc = null;
                            try
                            {
                                XDocument.Parse(responseStr);
                            }
                            catch (Exception e)
                            {
                                _log.Error($"Could not parse result to an XDocument, errorMessage: {e.Message}\nresult:\n{responseStr}");
                            }
                            if (doc != null)
                            {
                                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
                                namespaceManager.AddNamespace("ns", "http://schemas.xmlsoap.org/soap/envelope/");
                                namespaceManager.AddNamespace("ns0", "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216");
                                namespaceManager.AddNamespace("ns1", "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216");

                                XElement elem = doc.XPathSelectElement("./ns:Envelope/ns:Body/ns1:GetCardDetails2Response/ns1:GetCardDetails2Result/ns0:CardDetails2", namespaceManager);
                                
                                XmlSerializer serializer = new XmlSerializer(typeof(CardDetails2));
                                byte[] utf8Array = Encoding.UTF8.GetBytes(new XDocument(elem).ToString());
                                using (MemoryStream memStream = new MemoryStream(utf8Array))
                                {
                                    CardDetails2 cardDetails = (CardDetails2)serializer.Deserialize(memStream);

                                    _log.Info($"CardDetails attained is = {cardDetails.ToString()}");

                                    // TODO: teo - Fill TravelCardEntity with information
                                }
                            }
                        }

                        XrmHelper.Update(localContext, updateEntity);
                    }
                }

                resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(info));
                return resp;
            }
            catch (Exception ex)
            {
                resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                resp.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return resp;
            }
        }

        internal static HttpResponseMessage SalesOrderPost(SalesOrderInfo salesOrderInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    HttpResponseMessage validateInfoMess = ValidateSalesOrderInfo(localContext, salesOrderInfo, false);
                    if (!HttpStatusCode.OK.Equals(validateInfoMess.StatusCode))
                        return validateInfoMess;

                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, salesOrderInfo.Customer);

                    SalesOrderEntity salesOrder = new SalesOrderEntity
                    {
                        ed_Name = salesOrderInfo.OrderNo + ", " + contact.FirstName + " " + contact.LastName,
                        ed_OrderNo = salesOrderInfo.OrderNo,
                        ed_OrderPlacedOn = salesOrderInfo.OrderTime,
                        ed_ContactId = contact.ToEntityReference(),
                        statuscode = Generated.ed_salesorder_statuscode.OrderCreated
                    };
                    salesOrder.Id = XrmHelper.Create(localContext, salesOrder);
                    salesOrderInfo.Guid = salesOrder.Id.ToString();

                    foreach (Productinfo prodInfo in salesOrderInfo.Productinfos)
                    {
                        TravelCardEntity travelCard = new TravelCardEntity
                        {
                            cgi_travelcardnumber = TravelCardEntity._NotDefined,
                            cgi_TravelCardName = prodInfo.NameOnCard,
                            cgi_Blocked = false,
                            cgi_ValueCardTypeId = prodInfo.ProductCode,
                            ed_ReferenceNo = prodInfo.Reference,
                            ed_SalesOrderId = salesOrder.ToEntityReference(),
                            cgi_Contactid = contact.ToEntityReference()
                        };
                        travelCard.Id = XrmHelper.Create(localContext, travelCard);
                    }
                }

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(salesOrderInfo));
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        private static HttpResponseMessage ValidateSalesOrderInfo(Plugin.LocalPluginContext localContext, SalesOrderInfo salesOrderInfo, bool isPut)
        {
            HttpResponseMessage respMess;
            if (salesOrderInfo == null)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                return respMess;
            }
            if (salesOrderInfo.Customer == null)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.NoCustomerInfoFoundOnSalesOrder);
                return respMess;
            }
            if (salesOrderInfo.Productinfos == null || salesOrderInfo.Productinfos.Length < 1)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.NoProductInfoFoundOnSalesOrder);
                return respMess;
            }

            List<string> missingSalesOrderFields = new List<string>();
            if (string.IsNullOrWhiteSpace(salesOrderInfo.OrderNo))
                missingSalesOrderFields.Add("OrderNo");
            if (salesOrderInfo.OrderTime == null || salesOrderInfo.OrderTime == DateTime.MinValue)
                missingSalesOrderFields.Add("OrderTime");

            if (missingSalesOrderFields.Count > 0)
            {
                string errorMess = Properties.Resources.MissingFields;
                foreach (string field in missingSalesOrderFields)
                {
                    errorMess += field + "<BR>";
                }
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(errorMess);
                return respMess;
            }

            salesOrderInfo.Customer.Source = (int)CustomerUtility.Source.OinloggatKop;
            StatusBlock validateCustomerInfoStatus = CustomerUtility.ValidateCustomerInfo(localContext, salesOrderInfo.Customer);
            if (!validateCustomerInfoStatus.TransactionOk)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(validateCustomerInfoStatus.ErrorMessage);
                return respMess;                
            }

            List<string> missingProductInfoFields = new List<string>();
            foreach(Productinfo prodInfo in salesOrderInfo.Productinfos)
            {
                if (string.IsNullOrWhiteSpace(prodInfo.Reference) && !missingProductInfoFields.Contains("Reference"))
                    missingProductInfoFields.Add("Reference");
                if (string.IsNullOrWhiteSpace(prodInfo.NameOnCard) && !missingProductInfoFields.Contains("NameOnCard"))
                    missingProductInfoFields.Add("NameOnCard");
                if (prodInfo.ProductCode == null && !missingProductInfoFields.Contains("ProductCode"))
                    missingProductInfoFields.Add("ProductCode");
                if (isPut && string.IsNullOrWhiteSpace(prodInfo.Serial) && !missingProductInfoFields.Contains("Serial"))
                    missingProductInfoFields.Add("Serial");
            }

            if (missingProductInfoFields.Count > 0)
            {
                string errorMess = Properties.Resources.MissingProductInfoFields;
                foreach (string field in missingProductInfoFields)
                {
                    errorMess += field + "<BR>";
                }
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(errorMess);
                return respMess;
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        internal static HttpResponseMessage RetrieveLeadLinkGuid(string eamil)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.StateCode, LeadEntity.Fields.ed_LatestLinkGuid),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, eamil)
                        }
                    });

                    if (leads.Count == 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                        rm.Content = new StringContent($"Hittade inget Lead med {eamil}");
                        return rm;
                    }
                    else if (leads.Count > 1)
                    {
                        int actives = 0;
                        LeadEntity activeLead = null;
                        foreach (LeadEntity l in leads)
                        {
                            if (l.StateCode == Generated.LeadState.Open)
                            {
                                activeLead = l;
                                actives++;
                            }
                        }
                        if (actives > 1)
                        {
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rm.Content = new StringContent($"Hittade multipla Lead med {eamil}");
                            return rm;
                        }
                        else if (actives == 0)
                        {
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rm.Content = new StringContent($"Hittade inga aktiva Lead med {eamil}");
                            return rm;
                        }
                        else
                        {
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                            GuidsPlaceholder guidPlaceholder = new GuidsPlaceholder
                            {
                                CrmGuid = activeLead.Id.ToString(),
                                LinkId = activeLead.ed_LatestLinkGuid
                            };
                            response.Content = new StringContent(JsonConvert.SerializeObject(guidPlaceholder));
                            return response;
                        }
                    }
                    else if (!leads[0].StateCode.Equals(Generated.LeadState.Open))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Lead med {eamil} har redan validerats.");
                        return rm;
                    }
                    if (string.IsNullOrWhiteSpace(leads[0].ed_LatestLinkGuid))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                        rm.Content = new StringContent($"Hittade inget LänkId för Lead med {eamil}");
                        return rm;
                    }
                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    GuidsPlaceholder gp = new GuidsPlaceholder
                    {
                        CrmGuid = leads[0].Id.ToString(),
                        LinkId = leads[0].ed_LatestLinkGuid
                    };
                    resp.Content = new StringContent(JsonConvert.SerializeObject(gp));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage RetrieveContactLinkGuid(string idOrEmail)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    IList<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.ed_LatestLinkGuid),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_EmailToBeVerified, ConditionOperator.Equal, idOrEmail)
                        }
                    });

                    if (contacts.Count == 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                        rm.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, idOrEmail));
                        return rm;
                    }
                    else if (contacts.Count > 1)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Hittade multipla Kunder som försöker byta till {idOrEmail}");
                        return rm;
                    }
                    if (string.IsNullOrWhiteSpace(contacts[0].ed_LatestLinkGuid))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                        rm.Content = new StringContent($"Hittade inget länkId för Kund med {idOrEmail}");
                        return rm;
                    }
                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    GuidsPlaceholder gp = new GuidsPlaceholder
                    {
                        CrmGuid = contacts[0].Id.ToString(),
                        LinkId = contacts[0].ed_LatestLinkGuid
                    };
                    resp.Content = new StringContent(JsonConvert.SerializeObject(gp));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage PASS(CustomerInfo customerInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customerInfo == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return respMess;
                    }

                    // Validate info
                    _log.DebugFormat("PASS, validating info");
                    customerInfo.Source = (int)CustomerUtility.Source.PASS;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat("PASS, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                    _log.DebugFormat("PASS, Found/created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                    // Attach Lead
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName)
                            }
                        });
                    bool updated = false;
                    foreach (LeadEntity lead in leads)
                    {
                        if (UpdateContactWithLead(ref contact, lead))
                            updated = true;
                    }
                    if (updated)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.PASS;
                        localContext.OrganizationService.Update(contact);
                    }

                    HttpResponseMessage finalrm = new HttpResponseMessage(HttpStatusCode.OK);
                    finalrm.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return finalrm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }


        public static HttpResponseMessage ChangeEmailAddress([FromBody] CustomerInfo customer)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customer == null)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return rm;
                    }
                    // Validera att inkommande information är giltig (ValidateCustomerInfo)
                    customer.Source = (int)CustomerUtility.Source.BytEpost;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                    if (!validateStatus.TransactionOk)
                    {
                        // Skicka vidare eventuellt felmeddelande
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(validateStatus.ErrorMessage);
                        return respMess;
                    }
                    // Kolla så att en eventuell uppdatering inte skulle skapa dublett - Om så, returnera lämpligt felmeddelande
                    IList<ContactEntity> mailConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                             new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                             new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.NewEmail)
                            }
                        });
                    if (mailConflicts.Count > 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.EmailAlreadyInUse);
                        return rm;
                    }
                    // Uppdatera Contact's EmailToBeVerified-fält och skicka ett valideringsmail.
                    ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email)
                            }
                        });
                    if (contact == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, string.Format("{0} + {1}", customer.SocialSecurityNumber, customer.Email)));
                        return respMess;
                    }

                    int validityHours = 0;
                    try
                    {
                        validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
                    }
                    catch (MissingFieldException e)
                    {
                        _log.Error($"Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        respMess.Content = new StringContent(string.Format(Resources.SettingsFetchError, e.Message));
                        return respMess;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    contact.ed_EmailToBeVerified = customer.NewEmail;
                    contact.ed_LinkExpiryDate = DateTime.Now.AddHours(validityHours);
                    contact.ed_LatestLinkGuid = Guid.NewGuid().ToString();
                    contact.ed_InformationSource = Generated.ed_informationsource.BytEpost;
                    localContext.OrganizationService.Update(contact);
                    SendEmailResponse resp = CrmPlusUtility.SendValidationEmail(localContext, contact);

                    HttpResponseMessage finalMess = new HttpResponseMessage(HttpStatusCode.OK);
                    finalMess.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return finalMess;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage CreateAngeNamn(CustomerInfo info)
        {
            //Plugin.LocalPluginContext localContext = GenerateContext(true);
            CrmServiceClient serviceClient = CreateCrmConnection(true);
            _log.DebugFormat("Creating serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
            using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
            {

                _log.DebugFormat("Assembling localContext");
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                ContactEntity angeNamn = new ContactEntity(localContext, info);
                angeNamn.Id = XrmHelper.Create(localContext, angeNamn);
                angeNamn.ContactId = angeNamn.Id;

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(angeNamn.ToCustomerInfo(localContext)));
                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCustomerLead([FromBody] CustomerInfo customer)
        {
            try
            {
                LeadEntity newLead = null;
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customer == null)
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return response;
                    }
                    if (!customer.SwedishSocialSecurityNumberSpecified)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(Properties.Resources.SwedishSocialSecurityNumberMustBeSpecified);
                        return respMess;
                    }
                    customer.Source = (int)CustomerUtility.Source.SkapaMittKonto;
                    StatusBlock validationStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                    if (!validationStatus.TransactionOk)
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(validationStatus.ErrorMessage);
                        return response;
                    }
                    StatusBlock canLeadBeCreated = LeadEntity.CanLeadBeCreated(localContext, customer);
                    if (canLeadBeCreated.TransactionOk)
                    {
                        // Create a new lead.
                        newLead = LeadEntity.CreateLead(localContext, customer, CreateAccountSubject);
                    }
                    else if (canLeadBeCreated.StatusBlockCode == (int)CustomerUtility.StatusBlockCode.OtherLeadFound)
                    {
                        Guid leadGuid = Guid.Empty;
                        if (Guid.TryParse(canLeadBeCreated.Information, out leadGuid))
                        {
                            try
                            {
                                newLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, leadGuid, LeadEntity.LeadInfoBlock);
                            }
                            catch (Exception e)
                            {
                                localContext.TracingService.Trace(string.Format("Inconcistency in Database. Unexpected exception caught:\n{0}", e.Message));

                                HttpResponseMessage exceptionResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                exceptionResponse.Content = new StringContent(Resources.InconsistencyInDatabase);
                                return exceptionResponse;
                            }

                            newLead.UpdateWithCustomerInfo(localContext, customer);
                        }
                        else
                        {
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            response.Content = new StringContent(Resources.InconsistencyInDatabase);
                            return response;
                        }
                    }
                    else
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(canLeadBeCreated.ErrorMessage);
                        return response;
                    }

                    int validityHours = 0;
                    try
                    {
                        validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
                    }
                    catch (MissingFieldException e)
                    {
                        _log.Error($"Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        respMess.Content = new StringContent(string.Format(Resources.SettingsFetchError, e.Message));
                        return respMess;
                    }

                    newLead.ed_LinkExpiryDate = DateTime.Now.AddHours(validityHours);
                    newLead.ed_LatestLinkGuid = Guid.NewGuid().ToString();
                    newLead.ed_InformationSource = Generated.ed_informationsource.SkapaMittKonto;
                    newLead.LeadSourceCode = Generated.lead_leadsourcecode.MittKonto;
                    localContext.OrganizationService.Update(newLead);

                    SendEmailResponse mailResponse = CrmPlusUtility.SendValidationEmail(localContext, newLead);

                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.Created);
                    rm.Content = new StringContent(SerializeNoNull(newLead.ToCustomerInfo(localContext)));
                    return rm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage GetContactTroubleshooting(string contactGuidOrEmail)
        {
            //OrganizationServiceProxy serviceProxy = null;
            try
            {

                HttpContext httpContext = HttpContext.Current;
                _log.DebugFormat("httpContext = {0}", httpContext);

                CrmServiceClient _conn = null;
                if (httpContext != null)
                {
                    _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
                }
                if (_conn == null)
                {
                    _log.Debug("Creating new connection from CRM (no cache found)");
                    string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                    _log.DebugFormat("connectionString = {0}", connectionString);
                    //  Connect to the CRM web service using a connection string.
                    _conn = new CrmServiceClient(connectionString);

                    _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
                    if (_conn.IsReady)
                    {
                        httpContext.Cache.Insert(_generateContextString, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        //ServerManager serverManager = new ServerManager();

                        //ApplicationPoolCollection appPoolCollection = serverManager.ApplicationPools;
                        //foreach (ApplicationPool pool in appPoolCollection)
                        //{
                        //    if (pool.Name != null && pool.Name.Equals(_appPoolName))
                        //    {

                        //    }
                        //}

                        //_log.Debug($"Acessing cache for error count");
                        //int? errorCount = httpContext.Cache.Get(_cacheErrorCountString) as int?;
                    }
                }

                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)_conn.OrganizationServiceProxy;
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);

                resp.Content = new StringContent("Debug");


                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            //finally
            //{
            //    if (serviceProxy != null)
            //        serviceProxy.Dispose();
            //}
        }

        public static HttpResponseMessage GetContact(string contactGuidOrEmail)
        {
            ContactEntity contact;
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    Guid contactId = Guid.Empty;
                    // Split contactId / email.
                    if (Guid.TryParse(contactGuidOrEmail, out contactId))
                    {
                        contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, contactId)
                                }
                            });
                    }
                    else
                    {
                        contact = ContactEntity.GetValidatedContactFromEmail(localContext, contactGuidOrEmail);
                    }
                    if (contact == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respMess.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, contactGuidOrEmail));
                        return respMess;
                    }
                    if (contact.StateCode != Generated.ContactState.Active)
                    {
                        // TODD teo - Verify httpStatusCode usage
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respMess.Content = new StringContent(string.Format(Resources.ContactIsInactive));
                        return respMess;
                    }
                    CustomerInfo info = contact.ToCustomerInfo(localContext);

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(info));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage GetLead(string id)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    if (string.IsNullOrWhiteSpace(id))
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return response;
                    }
                    Guid guid = Guid.Empty;
                    if (!Guid.TryParse(id, out guid))
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(Resources.GuidNotValid);
                        return response;
                    }
                    LeadEntity lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                        new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, guid)
                            }
                        });
                    if (lead == null)
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, guid));
                        return response;
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(lead.ToCustomerInfo(localContext)));
                    return resp;

                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginCustomerIncident([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customerInfo == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return respMess;
                    }

                    // Validate info.
                    customerInfo.Source = (int)CustomerUtility.Source.OinloggatKundArende;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(validateStatus.ErrorMessage);
                        return respMess;
                    }
                    // Find customer according to rules
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                    // Find possible Leads and connect possible purchaseInfo.
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName)
                            }
                        });
                    foreach (LeadEntity lead in leads)
                    {
                        bool update = false;
                        if (UpdateContactWithLead(ref contact, lead))
                            update = true;
                        if (update)
                        {
                            contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKundArende;
                            localContext.OrganizationService.Update(contact);
                        }
                    }

                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.OK);
                    rm.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return rm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginPurchase([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    if (customerInfo == null)
                    {
                        HttpResponseMessage invalidInputResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        invalidInputResp.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return invalidInputResp;
                    }

                    // Validate info
                    customerInfo.Source = (int)CustomerUtility.Source.OinloggatKop;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat("NonLoginPurchase, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                    _log.DebugFormat("NonLoginPurchase, Found/Created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                    // Attach Leads
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName)
                            }
                        });
                    bool updated = false;
                    foreach (LeadEntity lead in leads)
                    {
                        if (UpdateContactWithLead(ref contact, lead))
                            updated = true;
                    }
                    if (updated)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKop;
                        localContext.OrganizationService.Update(contact);
                    }

                    HttpResponseMessage leadResp = new HttpResponseMessage(HttpStatusCode.OK);
                    leadResp.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return leadResp;

                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginRefill([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    if (customerInfo == null)
                    {
                        HttpResponseMessage invalidInputResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        invalidInputResp.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return invalidInputResp;
                    }

                    // Validate info
                    customerInfo.Source = (int)CustomerUtility.Source.OinloggatLaddaKort;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    ContactEntity contact = ContactEntity.FindActiveContact(localContext, customerInfo);

                    if (contact != null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.OK);
                        respMess.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                        return respMess;
                    }
                    else
                    {
                        LeadEntity lead = null;
                        // INFO - har inte refaktoriserats på grund av att felmeddelanden ska vara HttpResponseMessage
                        StatusBlock canLeadBeCreated = LeadEntity.CanLeadBeCreated(localContext, customerInfo);
                        if (canLeadBeCreated.TransactionOk)
                        {
                            lead = new LeadEntity(localContext, customerInfo);
                            lead.LeadSourceCode = Generated.lead_leadsourcecode.LaddaKort;
                            lead.Subject = NonLoginRefillSubject;
                            lead.Id = localContext.OrganizationService.Create(lead);
                        }
                        else if (canLeadBeCreated.StatusBlockCode == (int)CustomerUtility.StatusBlockCode.OtherLeadFound)
                        {
                            try
                            {
                                lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, new Guid(canLeadBeCreated.Information), LeadEntity.LeadInfoBlock);
                                bool update = false;
                                if (lead.UpdateWithCustomerInfo(localContext, customerInfo))
                                    update = true;
                                if (!NonLoginRefillSubject.Equals(lead.Subject))
                                {
                                    lead.Subject = NonLoginRefillSubject;
                                    update = true;
                                }
                                if (!Generated.lead_leadsourcecode.LaddaKort.Equals(lead.LeadSourceCode))
                                {
                                    lead.LeadSourceCode = Generated.lead_leadsourcecode.LaddaKort;
                                    update = true;
                                }
                                if (update)
                                    XrmHelper.Update(localContext.OrganizationService, lead);
                            }
                            catch (Exception e)
                            {
                                _log.DebugFormat("Unexpected Exception caught when retrieving Lead. Error messagee: {0}", e.Message);
                                HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                errorResp.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                                return errorResp;
                            }
                        }
                        else
                        {
                            HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            errorResp.Content = new StringContent(canLeadBeCreated.ErrorMessage);
                            return errorResp;
                        }

                        HttpResponseMessage leadResp = new HttpResponseMessage(HttpStatusCode.OK);
                        leadResp.Content = new StringContent(SerializeNoNull(lead.ToCustomerInfo(localContext)));
                        return leadResp;
                    }

                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static HttpResponseMessage RGOL([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customerInfo == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return respMess;
                    }

                    //// Validate connection to server
                    //WhoAmIResponse whoAmI = (WhoAmIResponse)localContext.OrganizationService.Execute(new WhoAmIRequest());
                    //_log.DebugFormat("RGOL, whoAmI:{0}", whoAmI.UserId.ToString());

                    // Validate info
                    _log.DebugFormat("RGOL, validating info");
                    customerInfo.Source = (int)CustomerUtility.Source.RGOL;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat("RGOL, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                    _log.DebugFormat("RGOL, Found/created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                    // Attach Lead if contains Email
                    if (!string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                            new FilterExpression()
                            {
                                Conditions =
                                {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName)
                                }
                            });
                        bool updated = false;
                        foreach (LeadEntity lead in leads)
                        {
                            if (UpdateContactWithLead(ref contact, lead))
                                updated = true;
                        }
                        if (updated)
                        {
                            contact.ed_InformationSource = Generated.ed_informationsource.RGOL;
                            localContext.OrganizationService.Update(contact);
                        }
                    }


                    HttpResponseMessage finalrm = new HttpResponseMessage(HttpStatusCode.OK);
                    finalrm.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return finalrm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        private static ContactEntity QualifyLeadToContact(Plugin.LocalPluginContext localContext, LeadEntity lead)
        {
            QualifyLeadRequest req = new QualifyLeadRequest()
            {
                LeadId = lead.ToEntityReference(),
                CreateAccount = false,
                CreateContact = true,
                CreateOpportunity = false,
                Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
            };

            QualifyLeadResponse resp = (QualifyLeadResponse)localContext.OrganizationService.Execute(req);
            ContactEntity qualifiedContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, resp.CreatedEntities.FirstOrDefault().Id)
                    }
                });
            if (qualifiedContact == null)
            {
                throw new Exception(string.Format("Lead validation failed. No contact found."));
            }
            if (UpdateContactWithLead(ref qualifiedContact, lead))
                localContext.OrganizationService.Update(qualifiedContact);
            return qualifiedContact;
        }

        public static HttpResponseMessage NotifyMKLSent(NotificationInfo notificationInfo)
        {
            //Plugin.LocalPluginContext localContext = GenerateContext(true);
            CrmServiceClient serviceClient = CreateCrmConnection(true);
            _log.DebugFormat("Creating serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
            using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
            {

                _log.DebugFormat("Assembling localContext");
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                return HandleSingleNotifyMKL(localContext, notificationInfo);
            }
        }

        private static HttpResponseMessage HandleSingleNotifyMKL(Plugin.LocalPluginContext localContext, NotificationInfo notificationInfo)
        {
            try
            {
                _log.Debug("Entered NotifyMKLSent");

                HttpResponseMessage verificationRespMess = VerifyNotificationInfo(notificationInfo);
                if (verificationRespMess.StatusCode != HttpStatusCode.OK)
                    return verificationRespMess;

                ContactEntity contact = null;
                contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, notificationInfo.Guid)
                        }
                    });
                if (contact == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, notificationInfo.Guid))
                    };
                }

                bool notifyMKLCreated = false;
                if (!string.IsNullOrWhiteSpace(notificationInfo.MessageType))
                {

                    NotifyMKLEntity notification = new NotifyMKLEntity
                    {
                        Subject = $"{notificationInfo.MessageType} - {((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType).ToString()}",
                        ed_MessageType = notificationInfo.MessageType,
                        ed_Method = (Generated.ed_notifymkl_ed_method)notificationInfo.ContactType,
                        ed_TargetAddress = notificationInfo.SendTo,
                        RegardingObjectId = new EntityReference
                        {
                            Id = contact.Id,
                            LogicalName = ContactEntity.EntityLogicalName
                        }
                    };
                    notification.Id = XrmHelper.Create(localContext.OrganizationService, notification);
                    notifyMKLCreated = true;
                }
                if (notificationInfo.LoginDate != null)
                {
                    ContactEntity updateContact = new ContactEntity
                    {
                        Id = contact.Id,
                        cgi_MyAccount_LastLogin = notificationInfo.LoginDate
                    };
                    XrmHelper.Update(localContext, updateContact);
                }
                if (notifyMKLCreated)
                {
                    return new HttpResponseMessage(HttpStatusCode.Created)
                    {
                        Content = new StringContent(Resources.NotificationCreated)
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(Resources.LastLoginUpdated)
                    };
                }
            }
            catch (Exception e)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage NotifyMKLsSent(NotificationInfo[] notificationInfos)
        {
            List<Tuple<string, string>> errorMesses = new List<Tuple<string, string>>();

            //Plugin.LocalPluginContext localContext = GenerateContext(true);
            CrmServiceClient serviceClient = CreateCrmConnection(true);
            _log.DebugFormat("Creating serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
            using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
            {

                _log.DebugFormat("Assembling localContext");
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                for (int i = 0; i < notificationInfos.Length; i++)
                {
                    Guid guid = Guid.Empty;
                    if (notificationInfos[i].Guid == null || !Guid.TryParse(notificationInfos[i].Guid, out guid))
                    {
                        errorMesses.Add(new Tuple<string, string>(SerializeNoNull(notificationInfos[i]), Resources.GuidNotValid));
                        continue;
                    }

                    HttpResponseMessage respMess = HandleSingleNotifyMKL(localContext, notificationInfos[i]);
                    if (!HttpStatusCode.Created.Equals(respMess.StatusCode) && !HttpStatusCode.OK.Equals(respMess.StatusCode))
                    {
                        errorMesses.Add(new Tuple<string, string>(SerializeNoNull(notificationInfos[i]), respMess.Content.ReadAsStringAsync().Result));
                    }
                }

                if (errorMesses.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Tuple<string, string> t in errorMesses)
                        sb.AppendLine($"object: {t.Item1}\nresulted in: {t.Item2}");

                    if (errorMesses.Count == notificationInfos.Length)
                    {
                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        resp.Content = new StringContent(sb.ToString());
                        return resp;
                    }
                    else
                    {
                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.Ambiguous);
                        resp.Content = new StringContent(sb.ToString());
                        return resp;
                    }
                }

                HttpResponseMessage okResp = new HttpResponseMessage(HttpStatusCode.OK);
                okResp.Content = new StringContent($"Created or Updated {notificationInfos.Length} instances");
                return okResp;
            }
        }

        private static HttpResponseMessage VerifyNotificationInfo(NotificationInfo notificationInfo)
        {
            if (string.IsNullOrWhiteSpace(notificationInfo.Guid))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Resources.GuidMissing)
                };
            }

            bool messageTypeMissing = false, sendToMissing = false, invalidContactType = false, lastLoginDateMissing = false;
            if (string.IsNullOrWhiteSpace(notificationInfo.MessageType))
            {
                messageTypeMissing = true;
            }
            if (string.IsNullOrWhiteSpace(notificationInfo.SendTo))
            {
                sendToMissing = true;
            }
            if (notificationInfo.ContactType != 0 && notificationInfo.ContactType != 1)
            {
                invalidContactType = true;
            }
            if (notificationInfo.LoginDate == null)
            {
                lastLoginDateMissing = true;
            }

            if ((messageTypeMissing ^ sendToMissing)) // XOR
            {
                if (messageTypeMissing)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(Resources.NotificationMissingMessageType)
                    };
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(Resources.NotificationMissingSendTo)
                    };
            }
            if (lastLoginDateMissing && messageTypeMissing && sendToMissing)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Resources.NotificationMissingMessageType)
                };
            }
            if (invalidContactType && lastLoginDateMissing)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Resources.NotificationUnrecognisedContactType)
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static HttpResponseMessage UpdateContact([FromBody] CustomerInfo customer)
        {
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customer == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return respMess;
                    }

                    // Validera inkommande information (validateCustomerInfo)
                    customer.Source = (int)CustomerUtility.Source.UppdateraMittKonto;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateStatus.ErrorMessage);
                        return responseMessage;
                    }

                    // Uppdatera och returnera lämpligt meddelande
                    ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email),
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, customer.Guid)
                            }
                        });
                    if (contact == null)
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customer.Email + " och " + customer.Guid));
                        return response;
                    }
                    //customer.Guid = contact.Id.ToString();

                    // Validera att en eventuell uppdatering inte skulle bryta mot affärsregler (inga dubletter) - Om så returnera lämpligt felmeddelande
                    if (customer.SwedishSocialSecurityNumberSpecified && customer.SwedishSocialSecurityNumber)
                    {
                        QueryExpression query = new QueryExpression()
                        {
                            EntityName = ContactEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(false),
                            Criteria = new FilterExpression()
                            {
                                Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.NotEqual, customer.Guid)
                        }
                            }
                        };
                        IList<ContactEntity> possibleConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);
                        if (possibleConflicts.Count > 0)
                        {
                            // Om kunden har fått ett nytt, svenskt personnummer som krockar med existerande kund
                            if (string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber) && customer.SwedishSocialSecurityNumberSpecified && customer.SwedishSocialSecurityNumber && !string.IsNullOrWhiteSpace(customer.SocialSecurityNumber))
                            {
                                if (possibleConflicts.Count > 1)
                                {
                                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                    response.Content = new StringContent(Resources.MultipleSocSecConflictsFound);
                                    return response;
                                }
                                ContactEntity conflict = possibleConflicts[0];
                                ContactEntity conflictUpdate = new ContactEntity()
                                {
                                    ContactId = conflict.ContactId,
                                    ed_InformationSource = Generated.ed_informationsource.AdmAndraKund,
                                    cgi_socialsecuritynumber = null,
                                    ed_ConflictConnectionGuid = Guid.NewGuid().ToString()
                                };
                                XrmHelper.Update(localContext.OrganizationService, conflictUpdate);
                                contact.ed_ConflictConnectionGuid = conflictUpdate.ed_ConflictConnectionGuid;
                            }
                            else {
                                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                response.Content = new StringContent(Resources.CouldNotUpdateCustomerSocialSecurityConflict);
                                return response;
                            }
                        }
                    }

                    ContactEntity newInfo = new ContactEntity(localContext, customer);
                    if (contact.IsAnyAttributeModified(newInfo, ContactEntity.ContactInfoBlock.Columns.ToArray()))
                    {
                        contact.CombineAttributes(newInfo);
                        contact.ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto;
                        localContext.OrganizationService.Update(contact);
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        private static bool UpdateContactWithLead(ref ContactEntity contact, LeadEntity lead)
        {
            bool updated = false;
            if (!string.IsNullOrWhiteSpace(lead.FirstName) && string.IsNullOrWhiteSpace(contact.FirstName))
            {
                contact.FirstName = lead.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.LastName) && string.IsNullOrWhiteSpace(contact.LastName))
            {
                contact.LastName = lead.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.Telephone1) && string.IsNullOrWhiteSpace(contact.Telephone1))
            {
                contact.Telephone1 = lead.Telephone1;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.MobilePhone) && string.IsNullOrWhiteSpace(contact.Telephone2))
            {
                contact.Telephone2 = lead.MobilePhone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.ed_Personnummer) && string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber))
            {
                contact.cgi_socialsecuritynumber = lead.ed_Personnummer;
                contact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(contact.Address1_Line1) &&
                string.IsNullOrWhiteSpace(contact.Address1_Line2) &&
                string.IsNullOrWhiteSpace(contact.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(contact.Address1_City) &&
                contact.ed_Address1_Country == null)
            {
                updated = true;
                if (!string.IsNullOrWhiteSpace(lead.Address1_Line1))
                    contact.Address1_Line1 = lead.Address1_Line1;
                if (!string.IsNullOrWhiteSpace(lead.Address1_Line2))
                    contact.Address1_Line2 = lead.Address1_Line2;
                if (!string.IsNullOrWhiteSpace(lead.Address1_PostalCode))
                    contact.Address1_PostalCode = lead.Address1_PostalCode;
                if (!string.IsNullOrWhiteSpace(lead.Address1_City))
                    contact.Address1_City = lead.Address1_City;
                if (lead.ed_Address1_Country != null)
                    contact.ed_Address1_Country = lead.ed_Address1_Country;
            }

            if ((contact.ed_InformationSource == null || !contact.ed_InformationSource.HasValue) && lead.ed_InformationSource != null && lead.ed_InformationSource.HasValue)
            {
                contact.ed_InformationSource = lead.ed_InformationSource;
                updated = true;
            }
            // TODO teo - transfer activities from lead to contact.

            return updated;
        }

        public static HttpResponseMessage ValidateEmail(Guid customerId, int entityTypeCode, string latestLinkGuid, string mklId)
        {
            _log.DebugFormat($"Entered ValidateEmail(), customerId: {customerId}, entityTypeCode: {entityTypeCode}, latestLinkGuid: {latestLinkGuid}");
            try
            {
                //Plugin.LocalPluginContext localContext = GenerateContext(true);
                CrmServiceClient serviceClient = CreateCrmConnection(true);
                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

                    _log.DebugFormat("Assembling localContext");
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    // validating indata
                    if (customerId == null || Guid.Empty.Equals(customerId))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.GuidMissing);
                        return rm;
                    }
                    if (string.IsNullOrWhiteSpace(mklId))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.MklIdMissing);
                        return rm;
                    }

                    switch (entityTypeCode)
                    {
#region case Contact
                        case ContactEntity.EntityTypeCode:
                            _log.DebugFormat("Retrieving Contact with Id: {0}", customerId);

                            ColumnSet columnSet = ContactEntity.ContactInfoBlock;
                            columnSet.AddColumn(ContactEntity.Fields.ed_LatestLinkGuid);
                            ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, columnSet, new FilterExpression()
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, customerId)
                                }
                            });
                            if (contact == null)
                            {
                                _log.Error("Could not find a Contact with the provided Guid.");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.NotFound);
                                rml.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customerId));
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(contact.ed_LatestLinkGuid))
                            {
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(string.Format(Resources.NoLinkGuidFoundOnContact));
                                return rml;
                            }
                            if (contact.ed_LinkExpiryDate.HasValue && contact.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) > 0)
                            {
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(string.Format(Resources.OldLinkUsed));
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(contact.ed_EmailToBeVerified))
                            {
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(string.Format(Resources.NoEmailToBeVerifiedFoundOnContact));
                                return rml;
                            }
                            if (!contact.ed_LatestLinkGuid.Equals(latestLinkGuid))
                            {
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(string.Format(Resources.LinkNotMeantForThisContact));
                                return rml;
                            }

#region Conflicting Contacts Query
                            FilterExpression conflictFilter = new FilterExpression(LogicalOperator.Or);
                            FilterExpression mailFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                //new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                //new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                            }
                            };
                            conflictFilter.AddFilter(mailFilter);
                            if (contact.ed_HasSwedishSocialSecurityNumber.HasValue && contact.ed_HasSwedishSocialSecurityNumber.Value)
                            {
                                FilterExpression socsecFilter = new FilterExpression(LogicalOperator.And)
                                {
                                    Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contact.cgi_socialsecuritynumber),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)

                                }
                                };
                                conflictFilter.AddFilter(socsecFilter);
                            }
                            QueryExpression conflictContactQuery = new QueryExpression()
                            {
                                EntityName = ContactEntity.EntityLogicalName,
                                ColumnSet = new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.EMailAddress1),
                                Criteria = conflictFilter
                            };
#endregion
                            IList<ContactEntity> contactConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, conflictContactQuery);
                            if (contactConflicts.Count > 0)
                            {
                                if (contact.ed_EmailToBeVerified.Equals(contactConflicts[0].EMailAddress1))
                                {
                                    _log.DebugFormat($"Found existing, validated contact with conflicting email {contact.ed_EmailToBeVerified}. Cannot validate this contact.");
                                    HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                    rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerEmail);
                                    return rm3;
                                }
                                else
                                {
                                    _log.DebugFormat($"Found existing, validated contact with Social Security Number {contactConflicts[0].cgi_socialsecuritynumber}. Cannot validate this contact.");
                                    HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                    rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerSocSec);
                                    return rm3;
                                }
                            }

#region MergeContacts Query
                            FilterExpression mergeContactFilter = new FilterExpression(LogicalOperator.Or);
                            FilterExpression nameFilter = new FilterExpression(LogicalOperator.Or)
                            {
                                Filters =
                            {
                                new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName)
                                    }
                                },
                                new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                                    }
                                }
                            }
                            };

                            if (contact.ed_HasSwedishSocialSecurityNumber.HasValue && contact.ed_HasSwedishSocialSecurityNumber.Value)
                            {
                                FilterExpression persNrFilter = new FilterExpression(LogicalOperator.And)
                                {
                                    Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contact.cgi_socialsecuritynumber),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                                },
                                    Filters =
                                {
                                    nameFilter
                                }
                                };
                                mergeContactFilter.AddFilter(persNrFilter);
                            }
                            FilterExpression epostFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                //new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, contact.EMailAddress1),
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                            },
                                Filters =
                                {
                                    nameFilter
                                }
                            };
                            mergeContactFilter.AddFilter(epostFilter);
                            QueryExpression mergeContactQuery = new QueryExpression()
                            {
                                EntityName = ContactEntity.EntityLogicalName,
                                ColumnSet = ContactEntity.ContactInfoBlock,
                                Criteria = mergeContactFilter
                            };
#endregion
                            IList<ContactEntity> mergeContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, mergeContactQuery);
                            _log.DebugFormat("Found {0} contacts to merge with the one to be validated", mergeContacts.Count);
#region Conflicting Leads Query
                            QueryExpression conflictLeadQuery = new QueryExpression()
                            {
                                EntityName = LeadEntity.EntityLogicalName,
                                ColumnSet = LeadEntity.LeadInfoBlock,
                                Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                    //new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.EMailAddress1),
                                    new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                    new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                    new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                                }
                            }
                            };
#endregion
                            IList<LeadEntity> leadConflicts = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, conflictLeadQuery);
                            _log.DebugFormat("Found {0} conflicting leads to merge with the one to be validated", leadConflicts.Count);

                            if (mergeContacts.Count > 0)
                            {
                                contact.CombineContacts(localContext, mergeContacts);
                            }
                            if (leadConflicts.Count > 0)
                            {
                                foreach (LeadEntity l in leadConflicts)
                                    UpdateContactWithLead(ref contact, l);
                            }

                            contact.EMailAddress1 = contact.ed_EmailToBeVerified;
                            contact.ed_EmailToBeVerified = CustomerUtility._NEWEMAILDONE;
                            contact.EMailAddress2 = null;
                            contact.ed_LatestLinkGuid = null;
                            contact.ed_MklId = mklId;
                            contact.ed_InformationSource = Generated.ed_informationsource.BytEpost;
                            localContext.OrganizationService.Update(contact);

                            CrmPlusUtility.SendConfirmationEmail(localContext, contact);

                            HttpResponseMessage rm2 = new HttpResponseMessage(HttpStatusCode.OK);
                            rm2.Content = new StringContent(SerializeNoNull(contact.ToCustomerInfo(localContext)));
                            return rm2;
#endregion
#region case Lead
                        case LeadEntity.EntityTypeCode: // Lead
#region Find Lead
                            ContactEntity newContact = null;
                            _log.DebugFormat("Retrieving Lead with Id: {0}", customerId);
                            ColumnSet columns = LeadEntity.LeadInfoBlock;
                            columns.AddColumn(LeadEntity.Fields.ed_LatestLinkGuid);
                            columns.AddColumn(LeadEntity.Fields.ed_LinkExpiryDate);
                            LeadEntity lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, columns, new FilterExpression()
                            {
                                Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, customerId),
                                new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                            }
                            });
                            if (lead == null)
                            {
                                _log.ErrorFormat("Could not find a Lead with the provided Guid: {0}", customerId);
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.NotFound);
                                rml.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customerId));
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(lead.ed_LatestLinkGuid))
                            {
                                _log.ErrorFormat("Could not find a LatestLinkGuid on the retrieved Lead");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.NoLinkGuidFoundOnLead);
                                return rml;
                            }
                            if (!lead.ed_LatestLinkGuid.Equals(latestLinkGuid))
                            {
                                _log.ErrorFormat("LatestLinkGuid used is not the expected one");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.LinkNotMeantForThisLead);
                                return rml;
                            }
                            if (lead.ed_LinkExpiryDate.HasValue && lead.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) < 0)
                            {
                                _log.ErrorFormat("LinkExpiryDate is less that DateTime.Now");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.OldLinkUsed);
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(lead.EMailAddress1))
                            {
                                _log.ErrorFormat("Lead to be validated does not contain an email address");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.NoEmailToBeVerifiedFoundOnLead);
                                return rml;
                            }
#endregion
#region make Contact Conflict Query
                            FilterExpression contactConflictFilter = new FilterExpression(LogicalOperator.Or);
                            if (lead.ed_HasSwedishSocialSecurityNumber.HasValue && lead.ed_HasSwedishSocialSecurityNumber.Value)
                            {
                                FilterExpression SocSecFilter = new FilterExpression(LogicalOperator.And)
                                {
                                    Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, lead.ed_Personnummer)
                                }
                                };
                                contactConflictFilter.AddFilter(SocSecFilter);
                            }
                            FilterExpression nameMailFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, lead.EMailAddress1)
                            },
                                Filters =
                            {
                                new FilterExpression(LogicalOperator.Or)
                                {
                                    Filters =
                                    {
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
                                            }
                                        },
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                                            }
                                        }
                                    }
                                }
                            }
                            };
                            contactConflictFilter.AddFilter(nameMailFilter);

                            ColumnSet contactConflictColumnSet = ContactEntity.ContactInfoBlock;
                            contactConflictColumnSet.AddColumn(ContactEntity.Fields.DoNotEMail);

                            QueryExpression contactConflictQuery = new QueryExpression()
                            {
                                EntityName = ContactEntity.EntityLogicalName,
                                ColumnSet = contactConflictColumnSet,
                                Criteria = contactConflictFilter
                            };
#endregion
                            IList<ContactEntity> conflictContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, contactConflictQuery);
                            _log.DebugFormat("Found {0} conflicting Contacts", conflictContacts.Count);
#region make Lead Conflict Query
                            QueryExpression leadConflictQuery = new QueryExpression()
                            {
                                EntityName = LeadEntity.EntityLogicalName,
                                ColumnSet = LeadEntity.LeadInfoBlock,
                                Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.NotEqual, lead.Id),
                                    new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, lead.EMailAddress1),
                                    new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                                    new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
                                }
                            }
                            };
#endregion
                            IList<LeadEntity> conflictLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, leadConflictQuery);
                            _log.DebugFormat("Found {0} conflicting Leads", conflictLeads.Count);

#region if Contact conflicts are found
                            if (conflictContacts.Count > 0)
                            {
                                ContactEntity socialSecurityConflict = null;
                                foreach (ContactEntity c in conflictContacts)
                                {
                                    if (lead.ed_Personnummer != null && lead.ed_Personnummer.Equals(c.cgi_socialsecuritynumber))
                                    {
                                        if (socialSecurityConflict == null)
                                        {
                                            _log.DebugFormat("Found Contact matching on social security number.");
                                            socialSecurityConflict = c;
                                        }
                                        else
                                        {
                                            _log.DebugFormat("Multiple conflicting Contacts with the same socialSecurityNumber were found.");
                                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                            respMess.Content = new StringContent(Resources.MultipleSocSecConflictsFound);
                                            return respMess;
                                        }
                                    }
                                }
                                if (socialSecurityConflict != null)
                                {
                                    conflictContacts.Remove(socialSecurityConflict);
                                    newContact = socialSecurityConflict;
                                    UpdateContactWithAuthorityLead(ref newContact, lead);

                                    SetStateRequest req = new SetStateRequest()
                                    {
                                        EntityMoniker = lead.ToEntityReference(),
                                        State = new OptionSetValue((int)Generated.LeadState.Disqualified),
                                        Status = new OptionSetValue((int)Generated.lead_statuscode.Canceled)
                                    };
                                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                    newContact.EMailAddress1 = lead.EMailAddress1;
                                    newContact.EMailAddress2 = null;
                                }
                                else
                                {
                                    //newContact = new ContactEntity(lead.ToCustomerInfo());
                                    //newContact.Id = localContext.OrganizationService.Create(newContact);
                                    _log.DebugFormat("No SocialSecurity conflict was found. Qualifying Lead.");
                                    newContact = QualifyLeadToContact(localContext, lead);
                                }
                                _log.DebugFormat($"Combining {conflictContacts.Count} conflicting contacts.");
                                newContact.CombineContacts(localContext, conflictContacts);

                                ////inactivate other customers
                                //foreach (ContactEntity c in conflictContacts)
                                //{
                                //    SetStateRequest req = new SetStateRequest()
                                //    {
                                //        EntityMoniker = c.ToEntityReference(),
                                //        State = new OptionSetValue((int)Generated.ContactState.Inactive),
                                //        Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                                //    };
                                //    localContext.OrganizationService.Execute(req);
                                //}
                            }
#endregion
                            else
                            {
                                _log.DebugFormat("No valid existing contact found, creating new from the Lead.");
                                newContact = QualifyLeadToContact(localContext, lead);
                            }

                            if (newContact == null)
                            {
                                HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                respMess.Content = new StringContent(Resources.UnexpectedErrorWhenValidatingEmail);
                                return respMess;
                            }

                            if (conflictLeads.Count > 0)
                            {
                                _log.DebugFormat("Merging {0} Leads with the new Contact", conflictLeads.Count);
                                foreach (LeadEntity leadConflict in conflictLeads)
                                {
                                    UpdateContactWithLead(ref newContact, leadConflict);
                                }
                            }

                            if (newContact.DoNotEMail == true)
                                newContact.DoNotEMail = false;

                            newContact.ed_MklId = mklId;
                            newContact.ed_InformationSource = Generated.ed_informationsource.SkapaMittKonto;
                            localContext.OrganizationService.Update(newContact);

                            CrmPlusUtility.SendConfirmationEmail(localContext, newContact);

                            HttpResponseMessage rm4 = new HttpResponseMessage(HttpStatusCode.OK);
                            rm4.Content = new StringContent(SerializeNoNull(newContact.ToCustomerInfo(localContext)));
                            return rm4;

#endregion
                        default:
                            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rm.Content = new StringContent(string.Format(Resources.UnsupportedEntityTypeCode, entityTypeCode));
                            return rm;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        private static bool UpdateContactWithAuthorityLead(ref ContactEntity contact, LeadEntity lead)
        {
            bool updated = false;
            if (!string.IsNullOrWhiteSpace(lead.FirstName) && !lead.FirstName.Equals(contact.FirstName))
            {
                contact.FirstName = lead.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.LastName) && !lead.LastName.Equals(contact.LastName))
            {
                contact.LastName = lead.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.Telephone1) && !lead.Telephone1.Equals(contact.Telephone1))
            {
                contact.Telephone1 = lead.Telephone1;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.MobilePhone) && !lead.MobilePhone.Equals(contact.Telephone2))
            {
                contact.Telephone2 = lead.MobilePhone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.ed_Personnummer) && !lead.ed_Personnummer.Equals(contact.cgi_socialsecuritynumber))
            {
                contact.cgi_socialsecuritynumber = lead.ed_Personnummer;
                contact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
                updated = true;
            }

            if (!(string.IsNullOrWhiteSpace(lead.Address1_Line1) &&
                string.IsNullOrWhiteSpace(lead.Address1_Line2) &&
                string.IsNullOrWhiteSpace(lead.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(lead.Address1_City) &&
                lead.ed_Address1_Country == null))
            {
                if (lead.Address1_Line1?.Equals(contact.Address1_Line1) == false ||
                    lead.Address1_Line2?.Equals(contact.Address1_Line2) == false ||
                    lead.Address1_PostalCode?.Equals(contact.Address1_PostalCode) == false ||
                    lead.Address1_City?.Equals(contact.Address1_City) == false ||
                    lead.ed_Address1_Country?.Equals(contact.ed_Address1_Country) == false)
                {
                    updated = true;
                    contact.Address1_Line1 = lead.Address1_Line1;
                    contact.Address1_Line2 = lead.Address1_Line2;
                    contact.Address1_PostalCode = lead.Address1_PostalCode;
                    contact.Address1_City = lead.Address1_City;
                    contact.ed_Address1_Country = lead.ed_Address1_Country;
                }
            }

            if (lead.ed_InformationSource.HasValue && (!contact.ed_InformationSource.HasValue || (lead.ed_InformationSource.Value != contact.ed_InformationSource.Value)))
            {
                contact.ed_InformationSource = lead.ed_InformationSource;
                updated = true;
            }
            // TODO teo - transfer activities from lead to contact?

            return updated;
        }

        public static string SerializeNoNull(object info)
        {
            return JsonConvert.SerializeObject(info, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        //private static Plugin.LocalPluginContext GenerateContext(bool print)
        //{
        //    HttpContext httpContext = HttpContext.Current;

        //    CrmServiceClient _conn = null;
        //    if(httpContext != null)
        //    {
        //        _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
        //    }
        //    if (_conn == null)
        //    {
        //        if (print)
        //            _log.Debug("Creating new connection from CRM (no cache found)");
        //        string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
        //        if (print)
        //            _log.DebugFormat("connectionString = {0}", connectionString);
        //        //  Connect to the CRM web service using a connection string.
        //        _conn = new CrmServiceClient(connectionString);

        //        if (print)
        //            _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
        //        if (_conn.IsReady)
        //        {
        //            httpContext.Cache.Insert(_generateContextString, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    //string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);

        //    ////  Connect to the CRM web service using a connection string.
        //    //CrmServiceClient _conn = new CrmServiceClient(connectionString);

        //    if (print)
        //        _log.DebugFormat("Creating serviceProxy");
        //    // Cast the proxy client to the IOrganizationService interface.
        //    //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
        //    OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)_conn.OrganizationServiceProxy;

        //    if (print)
        //        _log.DebugFormat("Assembling localContext");
        //    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

        //    if (localContext.OrganizationService == null)
        //        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

        //    return localContext;

        //}

        private static CrmServiceClient CreateCrmConnection(bool print)
        {
            HttpContext httpContext = HttpContext.Current;

            CrmServiceClient _conn = null;
            if (httpContext != null)
            {
                _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
            }
            if (_conn == null)
            {
                if (print)
                    _log.Debug("Creating new connection from CRM (no cache found)");
                string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                if (print)
                    _log.DebugFormat("connectionString = {0}", connectionString);
                //  Connect to the CRM web service using a connection string.
                _conn = new CrmServiceClient(connectionString);

                if (print)
                    _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
                if (_conn.IsReady)
                {
                    httpContext.Cache.Insert(_generateContextString, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                }
                else
                {

                }
            }
            //string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);

            ////  Connect to the CRM web service using a connection string.
            //CrmServiceClient _conn = new CrmServiceClient(connectionString);

            return _conn;
        }

        public class GuidsPlaceholder
        {
            public string LinkId { get; set; }

            public string CrmGuid { get; set; }
        }
    }
}
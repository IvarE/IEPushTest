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
using System.IdentityModel;
using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Controllers
{
    public class CrmPlusControl
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string _generateContextString = "localContext";
        //private static readonly string _cacheErrorCountString = "cacheErrorCount";
        //private static readonly string _appPoolName = "CRMPlus";
        private static readonly byte[] _entropyString = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        //private static System.Diagnostics.Stopwatch _stopwatch;
        //private static CrmServiceClient _conn;

        private static string NonLoginRefillSubject = "Ladda Kort";
        private static string CreateAccountSubject = "Skapa mitt konto";

        internal static HttpResponseMessage ConnectionHandlingTest(int threadId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage PingConnection(int threadId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, false);
                //_log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage SalesOrderPut(int threadId, SalesOrderInfo info)
        {
            HttpResponseMessage resp = null;
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {

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
                            _log.Info($"Th={threadId} - dataString = {data}");
                            _log.Info($"Th={threadId} - adressString = {cardDetailsServiceAddressCardDetails}");
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
                            _log.Debug($"Th={threadId} - Result of call to BIFF: {responseStr}");
                            XDocument doc = null;
                            try
                            {
                                XDocument.Parse(responseStr);
                            }
                            catch (Exception e)
                            {
                                _log.Error($"Th={threadId} - Could not parse result to an XDocument, errorMessage: {e.Message}\nresult:\n{responseStr}");
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

                                    _log.Info($"Th={threadId} - CardDetails attained is = {cardDetails.ToString()}");

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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage KopOchSkickaKund(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    customerInfo.Source = (int)Generated.ed_informationsource.KopOchSkicka;

                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo); //CHECK

                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateStatus.ErrorMessage);
                        return responseMessage;
                    }

                    var contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(contact.Id));
                    return resp;

                }

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage SalesOrderPost(int threadId, SalesOrderInfo salesOrderInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                            cgi_Blocked = false,            // Obsolete field.
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// Creates a new sales order entity with data from Buy and Send.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="salesOrderInfo"></param>
        /// <returns></returns>
        internal static HttpResponseMessage KopOchSkickaSalesOrderPost(int threadId, SalesOrderInfo salesOrderInfo, bool isFTG)
        {
            CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
            _log.DebugFormat($"Th={threadId} - Creating serviceProxy");

            // Cast the proxy client to the IOrganizationService interface.
            using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
            {
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                _log.Debug($"Entered KopOchSkickaSalesOrderPost.");

                _log.Debug($"Entering ValidateKopOchSkickaSalesOrderInfo.");
                var response = ValidateKopOchSkickaSalesOrderInfo(localContext, salesOrderInfo, isFTG);
                _log.Debug($"Exited ValidateKopOchSkickaSalesOrderInfo.");

                if (!HttpStatusCode.OK.Equals(response.StatusCode))
                    return response;

                ContactEntity contact = null;
                AccountEntity account = null;
                SalesOrderEntity newSalesOrder = null;

                #region Handle Account / Contact Flow
                if (isFTG == false)
                {
                    if (!string.IsNullOrEmpty(salesOrderInfo.ContactGuid))
                        contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(salesOrderInfo.ContactGuid), new ColumnSet(ContactEntity.Fields.StateCode));
                    else if (!string.IsNullOrEmpty(salesOrderInfo.Customer.Guid))
                        contact = ContactEntity.GetContactById(localContext, new ColumnSet(ContactEntity.Fields.StateCode), Guid.Parse(salesOrderInfo.Customer.Guid));

                    salesOrderInfo.Customer = new CustomerInfo() { Guid = salesOrderInfo.ContactGuid };

                    if (salesOrderInfo.Customer.Source == 0)
                        salesOrderInfo.Customer.Source = salesOrderInfo.InformationSource;

                    //var contact = ContactEntity.GetContactById(localContext, new ColumnSet(ContactEntity.Fields.StateCode), Guid.Parse(salesOrderInfo.Customer.Guid));
                    if (contact == null || contact.StateCode == Generated.ContactState.Inactive)
                    {
                        var contactresp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        contactresp.Content = new StringContent(SerializeNoNull($"Contact '{salesOrderInfo.Customer.Guid}' is either inactive or does not exist in CRM."));
                        return contactresp;
                    }

                    #region Create SalesOrder from SalesOrderInfo
                    //var newSalesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, false);
                    newSalesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, false, false);
                    newSalesOrder.ed_ContactId = contact.ToEntityReference(); //check if FTG

                    newSalesOrder.Id = XrmHelper.Create(localContext, newSalesOrder);
                    #endregion
                }
                else
                {
                    account = AccountEntity.GetAccountByPortalId(localContext, new ColumnSet(AccountEntity.Fields.StateCode), salesOrderInfo.PortalId);

                    if (account == null || account.StateCode == Generated.AccountState.Inactive)
                    {
                        var accountresp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        accountresp.Content = new StringContent(SerializeNoNull($"Account with PortalId '{salesOrderInfo.PortalId}' is either inactive or does not exist in CRM."));
                        return accountresp;
                    }

                    #region Create SalesOrder from SalesOrderInfo
                    //var newSalesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, true);
                    newSalesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, false, true);
                    newSalesOrder.ed_AccountId = account.ToEntityReference(); //check if FTG

                    newSalesOrder.Id = XrmHelper.Create(localContext, newSalesOrder);
                    #endregion
                }

                #endregion


                if (salesOrderInfo.SalesOrderLines != null)
                {
                    #region Loop through all SalesOrderLines from SalesOrderinfo
                    _log.Debug($"Looping through all SalesOrderLines.");
                    foreach (SalesOrderLineInfo salesOrderLineInfo in salesOrderInfo.SalesOrderLines)
                    {
                        SkaKortEntity skaKortEnt = new SkaKortEntity();

                        if (!String.IsNullOrEmpty(salesOrderLineInfo.CardNumber))
                        {
                            #region Create SkaKort

                            var query = new QueryExpression()
                            {
                                EntityName = SkaKortEntity.EntityLogicalName,
                                ColumnSet = new ColumnSet(),
                                Criteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, salesOrderLineInfo.CardNumber)
                                    }
                                }
                            };

                            localContext.TracingService.Trace($"Query for SkaKort with card number '{salesOrderLineInfo.CardNumber}'");

                            skaKortEnt = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, query);

                            if (skaKortEnt == null) //Do not create a new SkaKort if one already exists.
                            {
                                localContext.TracingService.Trace($"Could not find a SkaKort with card number '{salesOrderLineInfo.CardNumber}'");
                                //var skaKort = new SkaKortEntity()
                                //{
                                //    ed_CardNumber = salesOrderLineInfo.CardNumber,
                                //    ed_InformationSource = Generated.ed_informationsource.KopOchSkicka,
                                //    ed_Contact = contact.ToEntityReference(),
                                //    ed_name = salesOrderLineInfo.CardNumber
                                //};

                                SkaKortEntity skaKort = new SkaKortEntity();
                                skaKort.ed_CardNumber = salesOrderLineInfo.CardNumber;
                                skaKort.ed_name = salesOrderLineInfo.CardNumber;
                                if (isFTG == false)
                                {
                                    skaKort.ed_InformationSource = Generated.ed_informationsource.KopOchSkicka;
                                    skaKort.ed_Contact = contact.ToEntityReference();
                                }
                                else
                                {
                                    skaKort.ed_InformationSource = Generated.ed_informationsource.KopOchSkickaFTG;
                                    skaKort.ed_Account = account.ToEntityReference();
                                }


                                localContext.TracingService.Trace($"Creating a new SkaKort.");
                                skaKort.Trace(localContext.TracingService);
                                skaKort.Id = XrmHelper.Create(localContext, skaKort);

                                skaKortEnt = skaKort;
                            }

                            #endregion
                        }

                        #region Create SalesOrderLineEntity from SalesOrderLineInfo

                        _log.Debug($"Check if OrderStatus '{salesOrderLineInfo.Status}' exists.");
                        var orderStatus = OrderStatusEntity.FindOrCreateOrderStatus(localContext, salesOrderLineInfo.Status);

                        var newSalesOrderLine = SalesOrderLineInfo.GetSalesOrderLineEntityFromKopOchSkicka(
                                localContext,
                                salesOrderLineInfo,
                                newSalesOrder.ToEntityReference(),
                                orderStatus.ToEntityReference(),
                                (skaKortEnt != null && skaKortEnt.Id != null && skaKortEnt.Id != Guid.Empty) ? skaKortEnt.ToEntityReference() : null
                            );


                        newSalesOrderLine.Id = XrmHelper.Create(localContext, newSalesOrderLine);

                        #endregion
                    }
                    #endregion
                }

                _log.Debug($"Exiting KopOchSkickaSalesOrderPost.");

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(""));
                return resp;
            }
        }

        internal static HttpResponseMessage KopOchSkickaSalesOrderPut(int threadId, SalesOrderInfo salesOrderInfo, bool isFTG)
        {
            CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
            _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
            {
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                _log.Debug($"Entering KopOchSkickaSalesOrderPost.");

                HttpResponseMessage response = ValidateKopOchSkickaSalesOrderInfo(localContext, salesOrderInfo, isFTG); //Will we always have portalID?
                if (!HttpStatusCode.OK.Equals(response.StatusCode))
                    return response;

                //Since no Customer is sent into this model, we'll create a new one with contactid from call.
                //salesOrderInfo.Customer = new CustomerInfo() { Guid = salesOrderInfo.ContactGuid };


                //QueryExpression query = new QueryExpression()
                //{
                //    EntityName = SalesOrderEntity.EntityLogicalName,
                //    ColumnSet = new ColumnSet(SalesOrderEntity.Fields.ed_OrderNo),
                //    Criteria =
                //    {
                //        Conditions =
                //        {
                //            new ConditionExpression(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, salesOrderInfo.OrderNo),
                //            new ConditionExpression(SalesOrderEntity.Fields.ed_informationsource, ConditionOperator.Equal, (int)Generated.ed_informationsource.KopOchSkicka)
                //        }
                //    }
                //};

                FilterExpression salesOrderFilter = new FilterExpression();
                salesOrderFilter.AddCondition(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, salesOrderInfo.OrderNo);

                if (isFTG == false)
                {
                    //Since no Customer is sent into this model, we'll create a new one with contactid from call.
                    salesOrderInfo.Customer = new CustomerInfo() { Guid = salesOrderInfo.ContactGuid };
                    salesOrderFilter.AddCondition(SalesOrderEntity.Fields.ed_informationsource, ConditionOperator.Equal, (int)Generated.ed_informationsource.KopOchSkicka);
                }
                else
                {
                    salesOrderFilter.AddCondition(SalesOrderEntity.Fields.ed_informationsource, ConditionOperator.Equal, (int)Generated.ed_informationsource.KopOchSkickaFTG);
                }

                //Fetch SalesOrder
                _log.Debug($"Fetching SalesOrderEntity '{salesOrderInfo.OrderNo}'");
                //SalesOrderEntity salesOrderObj = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, query);
                SalesOrderEntity salesOrderObj = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, new ColumnSet(SalesOrderEntity.Fields.ed_OrderNo), salesOrderFilter);

                if (salesOrderObj != null)
                {
                    _log.Debug($"Found SalesOrderEntity. Getting attributes from SalesOrderInfo.");

                    //SalesOrderEntity salesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, true);
                    SalesOrderEntity salesOrder = null;
                    if (isFTG == false)
                    {
                        salesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, false);
                    }
                    else
                    {
                        salesOrder = SalesOrderInfo.GetSalesOrderEntityFromKopAndSkicka(localContext, salesOrderInfo, true);
                    }

                    salesOrder.Id = salesOrderObj.Id;
                    XrmHelper.Update(localContext, salesOrder);

                    _log.Debug($"Updating SalesOrder.");

                    QueryExpression salesOrderLineQuery = new QueryExpression()
                    {
                        EntityName = SalesOrderLineEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(SalesOrderLineEntity.Fields.st_SalesOrderLineID),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SalesOrderLineEntity.Fields.ed_SalesOrderId, ConditionOperator.Equal, salesOrder.Id)
                            }
                        }
                    };

                    _log.Debug($"Fetching all related SalesOrderLines to SalesOrder.");
                    List<SalesOrderLineEntity> salesOrderLines = XrmRetrieveHelper.RetrieveMultiple<SalesOrderLineEntity>(localContext, salesOrderLineQuery);

                    _log.Debug($"Found {salesOrderLines.Count} SalesOrderLines.");
                    foreach (var salesOrderLineFromSeKund in salesOrderLines)
                    {
                        foreach (SalesOrderLineInfo salesOrderLineFromPUT in salesOrderInfo.SalesOrderLines)
                        {
                            // item = salesOrderLineFromPUT
                            // salesOrderLine = salesOrderLineFromSeKund

                            if (salesOrderLineFromPUT.OrderLineNo == salesOrderLineFromSeKund.st_SalesOrderLineID)
                            {
                                _log.Debug($"Fetching/Creating OrderStatus.");
                                OrderStatusEntity orderStatus = OrderStatusEntity.FindOrCreateOrderStatus(localContext, salesOrderLineFromPUT.Status);
                                _log.Debug($"OrderStatus id '{orderStatus.Id}'.");

                                _log.Debug($"Fetching SkaKort.");
                                SkaKortEntity skaKort = SkaKortEntity.FetchSkaKort(localContext, salesOrderLineFromPUT.CardNumber);
                                //_log.Debug($"SkaKort card number '{skaKort.ed_CardNumber}'");

                                if (skaKort == null)
                                {
                                    _log.Debug($"SKÅ Kort not found. Create new.");

                                    //skaKort = new SkaKortEntity()
                                    //{
                                    //    ed_CardNumber = salesOrderLineFromPUT.CardNumber,
                                    //    ed_InformationSource = Generated.ed_informationsource.KopOchSkicka,
                                    //    ed_Contact = salesOrder.ed_ContactId,
                                    //    ed_name = salesOrderLineFromPUT.CardNumber
                                    //};

                                    skaKort = new SkaKortEntity();
                                    skaKort.ed_CardNumber = salesOrderLineFromPUT.CardNumber;
                                    skaKort.ed_name = salesOrderLineFromPUT.CardNumber;

                                    if (isFTG == false)
                                    {
                                        skaKort.ed_InformationSource = Generated.ed_informationsource.KopOchSkicka;
                                        skaKort.ed_Contact = salesOrder.ed_ContactId;
                                    }
                                    else
                                    {
                                        skaKort.ed_InformationSource = Generated.ed_informationsource.KopOchSkickaFTG;
                                        skaKort.ed_Account = salesOrder.ed_AccountId;
                                    }

                                    _log.Debug($"Creating new SKÅ Kort.");

                                    skaKort.Id = XrmHelper.Create(localContext, skaKort);

                                    _log.Debug($"Created new SKÅ Kort.");

                                    //HttpResponseMessage skaKortResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                                    //skaKortResponse.Content = new StringContent($"Cound not find SkaKort with card number '{salesOrderLineFromPUT.CardNumber}'");
                                    //return skaKortResponse;
                                }

                                SalesOrderLineEntity updatedSalesOrderLine = SalesOrderLineInfo.GetSalesOrderLineEntityFromKopOchSkicka(localContext, salesOrderLineFromPUT,
                                    salesOrder.ToEntityReference(), orderStatus.ToEntityReference(), skaKort.ToEntityReference());



                                _log.Debug($"Updating SalesOrderLine with data from SalesOrderLineInfo");
                                updatedSalesOrderLine.Id = salesOrderLineFromSeKund.Id;
                                XrmHelper.Update(localContext, updatedSalesOrderLine);
                                continue;
                            }
                        }
                    }


                    _log.Debug($"Exiting KopOchSkickaSalesOrderPost.");

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(""));
                    return resp;
                }
                else
                {
                    _log.Debug($"Exiting KopOchSkickaSalesOrderPost.");

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent($"Could not find SalesOrder with OrderNo '{salesOrderInfo.OrderNo}'.");
                    return resp;
                }


            }
        }

        internal static HttpResponseMessage CompanySalesOrderPost(int threadId, SalesOrderInfo salesOrderInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    HttpResponseMessage validateInfoMess = ValidateCompanySalesOrderInfo(localContext, salesOrderInfo);
                    if (!HttpStatusCode.OK.Equals(validateInfoMess.StatusCode))
                        return validateInfoMess;

                    SalesOrderEntity newSalesOrder = SalesOrderInfo.GetSalesOrderEntityFromSalesOrderInfo(localContext, salesOrderInfo);


                    if (salesOrderInfo.Customer != null && newSalesOrder.ed_CompanyRoleId == null)
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.NotFound);
                        error.Content = new StringContent($"Could not find company role for SalesOrder {salesOrderInfo.OrderNo}.");
                        return error;
                    }

                    if (newSalesOrder.ed_AccountId == null)
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.NotFound);
                        error.Content = new StringContent($"Could not find company {salesOrderInfo.PortalId}.");
                        return error;
                    }

                    newSalesOrder.Id = XrmHelper.Create(localContext, newSalesOrder);

                    if (salesOrderInfo.SalesOrderLines != null)
                    {
                        foreach (SalesOrderLineInfo salesOrderLineInfo in salesOrderInfo.SalesOrderLines)
                        {
                            SalesOrderLineEntity newSalesOrderLine = SalesOrderLineInfo.GetSalesOrderLineEntityFromSalesOrderLineInfo(localContext, salesOrderLineInfo);
                            newSalesOrderLine.ed_SalesOrderId = newSalesOrder.ToEntityReference();

                            newSalesOrderLine.Id = XrmHelper.Create(localContext, newSalesOrderLine);


                            if (salesOrderLineInfo.SalesOrderLineTravellers != null)
                            {
                                foreach (SalesOrderLineTravellerInfo TravellerInfo in salesOrderLineInfo.SalesOrderLineTravellers)
                                {
                                    SalesOrderLineTravellerEntity newTraveller = SalesOrderLineTravellerInfo.GetTravellerEntityFromTravellerInfo(localContext, TravellerInfo);
                                    newTraveller.ed_SalesOrderLineId = newSalesOrderLine.ToEntityReference();

                                    XrmHelper.Create(localContext, newTraveller);
                                }
                            }
                        }
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
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

            salesOrderInfo.Customer.Source = (int)Crm.Schema.Generated.ed_informationsource.OinloggatKop;
            StatusBlock validateCustomerInfoStatus = CustomerUtility.ValidateCustomerInfo(localContext, salesOrderInfo.Customer);
            if (!validateCustomerInfoStatus.TransactionOk)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(validateCustomerInfoStatus.ErrorMessage);
                return respMess;
            }

            List<string> missingProductInfoFields = new List<string>();
            foreach (Productinfo prodInfo in salesOrderInfo.Productinfos)
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

        private static HttpResponseMessage ValidateKopOchSkickaSalesOrderInfo(Plugin.LocalPluginContext localContext, SalesOrderInfo salesOrderInfo, bool isFTG)
        {
            _log.Debug($"Entering ValidateKopOchSkickaSalesOrderInfo.");
            HttpResponseMessage respMess;
            if (salesOrderInfo == null)
            {
                _log.Debug($"{nameof(salesOrderInfo)} is empty.");
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                return respMess;
            }
            if (isFTG == false && string.IsNullOrWhiteSpace(salesOrderInfo.ContactGuid))
            {
                _log.Debug($"{nameof(salesOrderInfo.Customer)} is empty.");
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.NoContactGuidOnSalesOrderInfo);
                return respMess;
            }

            if (isFTG == true && string.IsNullOrWhiteSpace(salesOrderInfo.PortalId))
            {
                _log.Debug($"{nameof(salesOrderInfo.PortalId)} is empty.");
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.NoContactGuidOnSalesOrderInfo);
                return respMess;
            }

            #region Check for missing field values or default values (by not passing anything)

            var errorMsg = Resources.MissingFields;
            if (salesOrderInfo.OrderTime == null ||
                salesOrderInfo.OrderTime == DateTime.MaxValue ||
                salesOrderInfo.OrderTime == DateTime.MinValue)
            {
                errorMsg += nameof(salesOrderInfo.OrderTime) + "<BR>";
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(errorMsg);
                return respMess;
            }

            if (salesOrderInfo.SalesOrderLines == null)
            {
                _log.Debug($"{nameof(salesOrderInfo.SalesOrderLines)} is empty.");
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent($"SalesOrderLines - {Resources.IncomingDataCannotBeNull}");
                return respMess;
            }
            #endregion

            //Check if it is required that order lines is needed.
            if (salesOrderInfo.SalesOrderLines.Length < 1)
            {
                _log.Debug($"{nameof(salesOrderInfo.SalesOrderLines)} does not have any order lines.");
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.KopOchSkicka_MissingOrderLines);
                return respMess;
            }
            _log.Debug($"Exiting ValidateKopOchSkickaSalesOrderInfo.");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private static HttpResponseMessage ValidateCompanySalesOrderInfo(Plugin.LocalPluginContext localContext, SalesOrderInfo salesOrderInfo)
        {
            HttpResponseMessage respMess;
            if (salesOrderInfo == null)
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                return respMess;
            }

            if (string.IsNullOrWhiteSpace(salesOrderInfo.OrderNo))
            {
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent($"{nameof(salesOrderInfo.OrderNo)} cannot be empty.");
                return respMess;
            }

            List<string> missingSalesOrderFields = new List<string>();
            if (string.IsNullOrWhiteSpace(salesOrderInfo.PortalId))
                missingSalesOrderFields.Add("PortalId");

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

            List<string> missingCustomerFields = new List<string>();
            if (salesOrderInfo.Customer != null)
            {
                bool validCustomer = true;
                if (!string.IsNullOrWhiteSpace(salesOrderInfo.Customer.SocialSecurityNumber))
                {
                    if (!CustomerUtility.CheckPersonnummerFormat(salesOrderInfo.Customer.SocialSecurityNumber))
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"{salesOrderInfo.Customer.SocialSecurityNumber} is not a valid SSN.");
                        return rm;
                    }

                    validCustomer = true;
                }

                if (!string.IsNullOrWhiteSpace(salesOrderInfo.Customer.Guid))
                {
                    Guid newGuid = Guid.Empty;
                    bool validGuid = Guid.TryParse(salesOrderInfo.Customer.Guid, out newGuid);
                    if (!validGuid)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"{salesOrderInfo.Customer.Guid} is not a valid Guid.");
                        return rm;
                    }

                    validCustomer = true;
                }

                if (!validCustomer)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Properties.Resources.NoCustomerInfoFoundOnSalesOrder);
                    return rm;
                }

                /*  COMPANY NOT BOUND TO THE ROLE BUT THE COMPANY ENTERED IN THE SALES ORDER. USE THIS CODE IF MULTIPLE COMPANIES ARE USED IN THE FUTURE.
                if(salesOrderInfo.Customer.CompanyRole == null || salesOrderInfo.Customer.CompanyRole.Length != 1)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Properties.Resources.InvalidNumberOfRoles);
                    return rm;
                }

                if (string.IsNullOrWhiteSpace(salesOrderInfo.Customer.CompanyRole[0].PortalId) && !missingCustomerFields.Contains("PortalId"))
                    missingCustomerFields.Add("PortalId"); */
            }

            if (missingCustomerFields.Count > 0)
            {
                string errorMess = Properties.Resources.MissingFields;
                foreach (string field in missingCustomerFields)
                {
                    errorMess += field + "<BR>";
                }
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(errorMess);
                return respMess;
            }

            List<string> missingSalesOrderLineFields = new List<string>();
            if (salesOrderInfo.SalesOrderLines != null)
            {
                foreach (SalesOrderLineInfo salesOrderLine in salesOrderInfo.SalesOrderLines)
                {
                    if (string.IsNullOrWhiteSpace(salesOrderLine.PortalId) && !missingSalesOrderLineFields.Contains("PortalId"))
                        missingSalesOrderLineFields.Add("PortalId");
                    if (string.IsNullOrWhiteSpace(salesOrderLine.TicketId) && !missingSalesOrderLineFields.Contains("TicketId"))
                        missingSalesOrderLineFields.Add("TicketId");

                    //if(salesOrderLine.TicketOfferType)

                    //if (!Enum.IsDefined(typeof(Generated.ed_salesorderline_ed_ticketoffertype), salesOrderLine.TicketOfferType))
                    //{
                    //    respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //    respMess.Content = new StringContent(CrmPlusUtility.GetEnumString("TicketOfferType", typeof(Generated.ed_salesorderline_ed_ticketoffertype)));
                    //    return respMess;
                    //}

                    List<string> missingSalesOrderLineTravellerFields = new List<string>();
                    foreach (SalesOrderLineTravellerInfo salesOrderLineTraveller in salesOrderLine.SalesOrderLineTravellers)
                    {
                        if (string.IsNullOrWhiteSpace(salesOrderLineTraveller.PortalId) && !missingSalesOrderLineTravellerFields.Contains("PortalId"))
                            missingSalesOrderLineTravellerFields.Add("PortalId");



                        //if (!Enum.IsDefined(typeof(Generated.ed_salesorderlinetraveller_ed_travellertype), salesOrderLineTraveller.TravellerType))
                        //{
                        //    respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //    respMess.Content = new StringContent(CrmPlusUtility.GetEnumString("TravellerType", typeof(Generated.ed_salesorderlinetraveller_ed_travellertype)));
                        //    return respMess;
                        //}
                    }

                    if (missingSalesOrderLineTravellerFields.Count > 0)
                    {
                        string errorMess = Properties.Resources.MissingSalesOrderLineTravellerInfoFields; // TODO
                        foreach (string field in missingSalesOrderLineTravellerFields)
                        {
                            errorMess += field + "<BR>";
                        }
                        missingSalesOrderLineFields.Add(errorMess);
                    }
                }
            }

            if (missingSalesOrderLineFields.Count > 0)
            {
                string errorMess = Properties.Resources.MissingSalesOrderLineInfoFields;
                foreach (string field in missingSalesOrderLineFields)
                {
                    errorMess += field + "<BR>";
                }
                respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                respMess.Content = new StringContent(errorMess);
                return respMess;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        internal static HttpResponseMessage RetrieveLeadLinkGuid(int threadId, string eamil)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.StateCode, LeadEntity.Fields.ed_LatestLinkGuid),
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, eamil),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage RetrieveContactLinkGuid(int threadId, string idOrEmail)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage PASS(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    _log.DebugFormat($"Th={threadId} - PASS, validating info");
                    customerInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.PASS;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo); //Check
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat($"Th={threadId} - PASS, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); //CHECK ??
                    string contactString = contact == null ? "null" : contact.ContactId.ToString();
                    _log.DebugFormat($"Th={threadId} - PASS, Found/created contact {contactString}");

                    // Attach Lead
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                                new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                                new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                                new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                                new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                            }
                        });
                    bool updated = false;

                    // Object used for update.
                    ContactEntity updContact = new ContactEntity()
                    {
                        ContactId = contact.ContactId
                    };


                    foreach (LeadEntity lead in leads)
                    {
                        if (ContactEntity.UpdateContactWithLead(ref contact, ref updContact, lead))
                            updated = true;
                    }
                    if (updated)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.PASS;
                        updContact.ed_InformationSource = Generated.ed_informationsource.PASS;
                        localContext.OrganizationService.Update(updContact);
                    }

                    //return new HttpResponseMessage(HttpStatusCode.OK) {
                    //                Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                    //            };

                    HttpResponseMessage finalrm = new HttpResponseMessage(HttpStatusCode.OK);
                    finalrm.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                    return finalrm;

                }
            }
            catch (Exception ex)
            {
                //return new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                //                Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message))
                //            };

                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }


        public static HttpResponseMessage ChangeEmailAddress(int threadId, CustomerInfo customer)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    customer.Source = (int)Crm.Schema.Generated.ed_informationsource.BytEpost;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                    if (!validateStatus.TransactionOk)
                    {
                        // Skicka vidare eventuellt felmeddelande
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(validateStatus.ErrorMessage);
                        return respMess;
                    }
                    // Kolla så att en eventuell uppdatering inte skulle skapa dublett - Om så, returnera lämpligt felmeddelande
                    FilterExpression getMailConflictContact = new FilterExpression(LogicalOperator.And);
                    getMailConflictContact.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                    getMailConflictContact.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.NewEmail);
                    if (customer.Mobile != null)
                    {
                        getMailConflictContact.AddCondition(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customer.Mobile);
                    }
                    getMailConflictContact.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);

                    IList<ContactEntity> mailConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(false), getMailConflictContact);
                    //new FilterExpression()
                    //{
                    //    Conditions =
                    //        {
                    //            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                    //            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.NewEmail),    //The new email that we are checking
                    //            new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customer.Mobile),         //The mobile phone is a key now (not anymore)
                    //        }
                    //}

                    if (mailConflicts.Count > 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(Resources.EmailAlreadyInUse);
                        return rm;
                    }

                    //If ContactInfo contains Email and Mobile, 
                    //FilterExpression getContactToUpdate = new FilterExpression(LogicalOperator.And);
                    //if (customer != null && !String.IsNullOrWhiteSpace(customer.SocialSecurityNumber))
                    //{
                    //    getContactToUpdate.AddCondition(ContactEntity.Fields.ed_SocialSecurityNumberBlock, ConditionOperator.Equal, customer.SocialSecurityNumber);
                    //}
                    //if (customer != null && !String.IsNullOrWhiteSpace(customer.Email))
                    //{
                    //    getContactToUpdate.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email);
                    //}
                    //if (customer != null && !String.IsNullOrWhiteSpace(customer.Mobile))
                    //{
                    //    getContactToUpdate.AddCondition(ContactEntity.Fields.MobilePhone, ConditionOperator.Equal, customer.Mobile);
                    //}
                    //ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock, getContactToUpdate);

                    // **************************************************************************
                    // Uppdatera Contact's EmailToBeVerified-fält och skicka ett valideringsmail. (lägg till control för att det är en företagskund)
                    // **************************************************************************

                    // Get original contact

                    FilterExpression getContact = new FilterExpression(LogicalOperator.And);

                    Guid guidOutput;
                    if (customer.Guid != null && Guid.TryParse(customer.Guid, out guidOutput) == true)
                    {
                        getContact.AddCondition(ContactEntity.Fields.Id, ConditionOperator.Equal, new Guid(customer.Guid));
                        getContact.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                    }
                    else
                    {
                        getContact = new FilterExpression(LogicalOperator.And);
                        getContact.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email);
                        if (customer.Mobile != null && customer.Mobile != String.Empty)
                        {
                            getContact.AddCondition(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customer.Mobile);
                        }
                        getContact.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                    }

                    ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock, getContact);
                    //new FilterExpression() //Changed filterexpression to only add mobile if this information is sent in
                    //{
                    //    Conditions =
                    //        {
                    //            //new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber), //ed_SocialSecurityNumberBlock
                    //            //new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, false),
                    //            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email),
                    //            new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customer.Mobile), //Dont use this unless its included
                    //        }
                    //}

                    if (contact == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, string.Format("{0} + {1}", customer.SocialSecurityNumber, customer.Email)))
                        };
                    }

                    int validityHours = 0;
                    try
                    {
                        validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
                    }
                    catch (MissingFieldException e)
                    {
                        _log.Error($"Th={threadId} - Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent(string.Format(Resources.SettingsFetchError, e.Message))
                        };
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

                    SendEmailResponse resp = CrmPlusUtility.SendValidationEmail(localContext, threadId, contact);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message))
                };
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        #region Not used
        /*
        internal static HttpResponseMessage CreateAngeNamn(int threadId, CustomerInfo info)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }
        */
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCustomerLead(int threadId, CustomerInfo customer)
        {
            try
            {
                customer = (LeadInfo)customer;
                LeadEntity newLead = null;
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (customer == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(Resources.IncomingDataCannotBeNull)
                        };
                    }
                    //if (!customer.SwedishSocialSecurityNumberSpecified)
                    //{
                    //    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //    respMess.Content = new StringContent(Properties.Resources.SwedishSocialSecurityNumberMustBeSpecified);
                    //    return respMess;
                    //}
                    customer.Source = (int)Crm.Schema.Generated.ed_informationsource.SkapaMittKonto;
                    StatusBlock validationStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                    if (!validationStatus.TransactionOk)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(validationStatus.ErrorMessage)
                        };
                    }
                    StatusBlock canLeadBeCreated = LeadEntity.CanLeadBeCreated(localContext, customer);
                    if (canLeadBeCreated.TransactionOk)
                    {
                        // Workaround!
                        // Issue during "Nya affärsregler". Was unable to qualify a lead if name was missing. Workaround is to add a lastname
                        if (string.IsNullOrWhiteSpace(customer.LastName))
                            customer.LastName = LeadEntity.CreateLead_LastNameToUseIfEmpty;

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
                        _log.Error($"Th={threadId} - Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        respMess.Content = new StringContent(string.Format(Resources.SettingsFetchError, e.Message));
                        return respMess;
                    }

                    newLead.ed_LinkExpiryDate = DateTime.Now.AddHours(validityHours);
                    newLead.ed_LatestLinkGuid = Guid.NewGuid().ToString();
                    newLead.ed_InformationSource = Generated.ed_informationsource.SkapaMittKonto;
                    newLead.LeadSourceCode = Generated.lead_leadsourcecode.MittKonto;
                    localContext.OrganizationService.Update(newLead);

                    SendEmailResponse mailResponse = CrmPlusUtility.SendValidationEmail(localContext, threadId, newLead);

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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage GetContactTroubleshooting(int threadId, string contactGuidOrEmail)
        {
            //OrganizationServiceProxy serviceProxy = null;
            try
            {

                HttpContext httpContext = HttpContext.Current;
                _log.DebugFormat($"Th={threadId} - httpContext = {httpContext}");

                CrmServiceClient _conn = null;
                if (httpContext != null)
                {
                    _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
                }
                if (_conn == null)
                {
                    _log.Debug($"Th={threadId} - Creating new connection from CRM (no cache found)");
                    string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                    _log.DebugFormat($"Th={threadId} - connectionString = {connectionString}");
                    //  Connect to the CRM web service using a connection string.
                    _conn = new CrmServiceClient(connectionString);

                    _log.DebugFormat($"Th={threadId} - Connection state: _conn.IsReady = {_conn.IsReady}");
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

                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
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

        public static HttpResponseMessage GetContact(int threadId, string contactGuidOrEmailorSSN)
        {
            ContactEntity contact = null;

            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    Guid contactId = Guid.Empty;
                    // GUID
                    if (Guid.TryParse(contactGuidOrEmailorSSN, out contactId))
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
                    else if (CustomerUtility.CheckPersonnummerFormat(contactGuidOrEmailorSSN)) //Pers.Nr
                    {
                        contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contactGuidOrEmailorSSN)
                                }
                            });
                    }
                    // EMAIL (Should we add Mobile in some way?)
                    else if (CustomerUtility.CheckEmailFormat(contactGuidOrEmailorSSN))
                    {
                        contact = ContactEntity.GetValidatedContactFromEmail(localContext, contactGuidOrEmailorSSN);
                    }

                    if (contact == null)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respMess.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, contactGuidOrEmailorSSN));
                        return respMess;
                    }
                    if (contact.StateCode != Generated.ContactState.Active)
                    {
                        // TODD teo - Verify httpStatusCode usage
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respMess.Content = new StringContent(string.Format(Resources.ContactIsInactive));
                        return respMess;
                    }
                    ContactInfo info = contact.ToContactInfo(localContext);

                    IList<CompanyRoleEntity> contactRoles = contact.GetCompanyRoles(localContext);

                    if (contactRoles != null && contactRoles.Count > 0)
                    {
                        info.CompanyRole = new CustomerInfoCompanyRole[contactRoles.Count];
                        for (int i = 0; i < contactRoles.Count; i++)
                        {
                            CustomerInfoCompanyRole role = CompanyRoleEntity.GetRoleInfoFromCompanyRole(localContext, contactRoles[0]);
                            info.CompanyRole[i] = role;
                        }
                    }

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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage GetLead(int threadId, string id)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage GetValueCodesWithMklId(int threadId, string id)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    if (string.IsNullOrWhiteSpace(id))
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                        return response;
                    }

                    _log.DebugFormat($"Th={threadId} - Get all active/not used value codes for MklId/ContactId/SocialSecurityNumber (contact): {id}");

                    ContactEntity contact;
                    Guid newGuid;
                    if (Guid.TryParse(id, out newGuid))
                    {
                        contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newGuid, new ColumnSet(false));
                    }
                    else
                    {
                        contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(
                        ContactEntity.Fields.ContactId),
                        new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, id)
                            }
                        });
                    }

                    if (contact == null)
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent($"No Contact found with Id = {id}");
                        return response;
                    }

                    IList<ValueCodeEntity> valueCodeCollection = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, new ColumnSet(
                        ValueCodeEntity.Fields.ed_CodeId,
                        ValueCodeEntity.Fields.ed_LastRedemptionDate,
                        ValueCodeEntity.Fields.ed_Amount,
                        ValueCodeEntity.Fields.ed_CreatedTimestamp,
                        ValueCodeEntity.Fields.ed_Link),
                        new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression(ValueCodeEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.ContactId),
                                new ConditionExpression(ValueCodeEntity.Fields.ed_RedemptionDate, ConditionOperator.Null),
                                new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active),
                                new ConditionExpression(ValueCodeEntity.Fields.ed_LastRedemptionDate, ConditionOperator.GreaterThan, DateTime.Today.ToLocalTime().AddDays(1))
                            }
                        });

                    if (valueCodeCollection == null)
                    {
                        if (valueCodeCollection.Count() < 1)
                        {
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                            response.Content = new StringContent(string.Format("Could not find any active value codes", id));
                            return response;
                        }
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(valueCodeCollection));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage GetAccount(int threadId, string accountId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    if (String.IsNullOrEmpty(accountId))
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"No Id entered.");
                        return error;
                    }

                    // SEARCH ON ORGANIZATIONALNUMBER (PARENT ACCOUNT)
                    IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, AccountEntity.AccountInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                                new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, accountId),
                                new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.Null),
                                new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                        }
                    });

                    if (accounts == null || accounts.Count == 0)
                    {
                        // SEARCH ON PORTALID/ORG NR (PARENT ACCOUNT)
                        accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, AccountEntity.AccountInfoBlock,
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, accountId),
                                new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.NotNull),
                                new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                                }
                            });
                    }

                    // IF STILL NOT FOUND THROW ERROR
                    if (accounts == null || accounts.Count == 0)
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.NotFound);
                        error.Content = new StringContent($"No active organization with Id {accountId} was found.");
                        return error;
                    }

                    //if (accounts.Count > 1)
                    //{
                    //    HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.Conflict);
                    //    error.Content = new StringContent($"Found multiple posts containing organization number {accountId}.");
                    //    return error;
                    //}

                    AccountEntity account = accounts[0];

                    AccountInfo accountInfo = AccountInfo.GetAccountInfoFromAccount(account);

                    IList<CustomerAddressEntity> addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, CustomerAddressEntity.CustomerAddressInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, account.Id)
                            }
                        });

                    if (addresses != null && addresses.Count > 0)
                    {
                        accountInfo.Addresses = new List<AddressInfo>();

                        foreach (CustomerAddressEntity cae in addresses)
                        {
                            AddressInfo adi = AddressInfo.GetAddressInfoFromCustomerAddress(cae);

                            accountInfo.Addresses.Add(adi);
                        }
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(accountInfo));
                    return resp;

                }

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// Getting valid orders to be sent to MultiQ
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetOrders(int threadId, int probability)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");

                if (serviceClient == null)
                    throw new Exception(string.Format("Failed to retrieve CrmServiceClient. Please check Available Connections. serviceClient is null."));

                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    ColumnSet columnsOrder = new ColumnSet(OrderEntity.Fields.OrderNumber, OrderEntity.Fields.Name, OrderEntity.Fields.ed_DeliveryReportStatus,
                                                OrderEntity.Fields.OwnerId, OrderEntity.Fields.CustomerId, OrderEntity.Fields.ed_Probability, OrderEntity.Fields.ed_campaigndatestart,
                                                OrderEntity.Fields.ed_campaigndateend);

                    QueryExpression queryOrders = new QueryExpression(OrderEntity.EntityLogicalName);
                    queryOrders.NoLock = true;
                    queryOrders.ColumnSet = columnsOrder;
                    queryOrders.Criteria.AddCondition(OrderEntity.Fields.ed_Probability, ConditionOperator.GreaterEqual, probability);
                    queryOrders.Criteria.AddCondition(OrderEntity.Fields.StatusCode, ConditionOperator.Equal, (int)salesorder_statuscode.Complete);

                    _log.Debug($"Query Orders. query: {queryOrders}");
                    List<OrderEntity> lOrders = XrmRetrieveHelper.RetrieveMultiple<OrderEntity>(localContext, queryOrders);

                    if (lOrders != null)
                        _log.Debug($"Number of orders retrieved: {lOrders.Count}");

                    if (lOrders == null || lOrders.Count == 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NoContent);
                        rm.Content = new StringContent(string.Format(Resources.NoSalesOrderFoundWithInfo, ""));
                        return rm;
                    }

                    OrderMQInfo orderMQInfo = new OrderMQInfo();

                    orderMQInfo.error = null;
                    orderMQInfo.data = new List<OrderMQ>();

                    foreach (OrderEntity order in lOrders)
                    {
                        _log.Debug($"Looping orders. OrderId: {order.Id}");

                        try
                        {
                            OrderMQ orderMQ = OrderMQ.GetOrderMQInfoFromOrderEntity(localContext, order, _log);

                            if (orderMQ != null)
                            {
                                orderMQInfo.data.Add(orderMQ);
                            }
                        }
                        catch (Exception e)
                        {
                            _log.DebugFormat($"Th={threadId} - Error converting Order Info:\n " + e.Message);
                            orderMQInfo.error = e.Message;
                        }
                    }

                    Metadata metadataMQ = new Metadata();
                    metadataMQ.limit = null; //TODO CODE LIMIT?
                    metadataMQ.total = orderMQInfo.data.Count;
                    metadataMQ.offset = 0;

                    orderMQInfo.metadata = metadataMQ;

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(SerializeNoNull(orderMQInfo));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage PostDeliveryReport(int threadId, FileInfoMQ fileInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");

                if (serviceClient == null)
                    throw new Exception(string.Format("Failed to retrieve CrmServiceClient. Please check Available Connections. serviceClient is null."));

                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    QueryExpression queryOrder = new QueryExpression(OrderEntity.EntityLogicalName);
                    queryOrder.NoLock = true;
                    queryOrder.ColumnSet.AddColumn(OrderEntity.Fields.SalesOrderId);
                    queryOrder.Criteria.AddCondition(OrderEntity.Fields.OrderNumber, ConditionOperator.Equal, fileInfo.OrderId);

                    List<OrderEntity> lOrders = XrmRetrieveHelper.RetrieveMultiple<OrderEntity>(localContext, queryOrder);

                    if (lOrders.Count == 0)
                    {
                        _log.Debug(string.Format(Resources.NoSalesOrderFoundWithInfo, fileInfo.OrderId));
                        HttpResponseMessage respNoOrders = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respNoOrders.Content = new StringContent(string.Format(Resources.NoSalesOrderFoundWithInfo, fileInfo.OrderId));
                        return respNoOrders;
                    }
                    else if (lOrders.Count > 1)
                    {
                        _log.Debug(string.Format(Resources.MultipleOrdersFound, fileInfo.OrderId));
                        HttpResponseMessage respNoOrders = new HttpResponseMessage(HttpStatusCode.NotFound);
                        respNoOrders.Content = new StringContent(string.Format(Resources.MultipleOrdersFound, fileInfo.OrderId));
                        return respNoOrders;
                    }
                    else
                    {
                        OrderEntity order = lOrders.FirstOrDefault();

                        OrderEntity uOrder = new OrderEntity();
                        uOrder.Id = (Guid)order.SalesOrderId;
                        uOrder.ed_DeliveryReportName = fileInfo.FileName;
                        uOrder.ed_DeliveryReportStatus = ed_deliveryreportstatus.Creatednotuploaded;

                        XrmHelper.Update(localContext, uOrder);

                        _log.Debug("Delivery Report Information successfully updated on Sekund.");
                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = new StringContent("Delivery Report Information successfully updated on Sekund.");
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Unexpected error from PostDeliveryReport-POST");
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage AccountPost(int threadId, AccountInfo accountInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    StatusBlock validateStatus = AccountUtility.ValidateAccountInfo(localContext, accountInfo, true);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Try retrieve Account / Cost Site with PortalId sent in request
                    // Do not accept if match PortalId on Account-POST
                    IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, accountInfo.PortalId),
                                new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                            }
                        });

                    if (accounts != null && accounts.Count > 0) //Vi ska inte hitta någon account vid en post
                    {
                        _log.Error("Account-POST. Organization found with passed PortalId. PortalId: {accountInfo.PortalId}");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"Organization with ID {accountInfo.PortalId} already exists.");
                        return error;
                    }


                    _log.Debug($"Creating (at least) new Cost Site for {accountInfo.PortalId}");

                    // Create new Cost Site
                    AccountEntity newAccount = AccountInfo.GetAccountEntityFromAccountInfo(accountInfo); //Kontot skapas, matchning?, type of account
                    newAccount.ed_TypeOfAccount = Generated.ed_account_ed_typeofaccount.Companycustomerportal;

                    // Try retrieve existing parent account/organization
                    AccountEntity parentAccount = null;
                    IList<AccountEntity> parentAccounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(
                        AccountEntity.Fields.ed_PortalCustomer,
                        AccountEntity.Fields.ed_SchoolCustomer,
                        AccountEntity.Fields.ed_SeniorCustomer
                        ),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, accountInfo.OrganizationNumber),
                                new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.Null),
                                new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                                //new ConditionExpression(AccountEntity.Fields.ed_TypeOfAccount, ConditionOperator.Equal, (int)Generated.ed_account_ed_typeofaccount.Companycustomerportal)
                            }
                        });

                    if (parentAccounts == null || parentAccounts.Count == 0)
                    {
                        _log.Debug($"No parent account/organization found with org. nr: {accountInfo.OrganizationNumber}. Create new parent account.");
                        parentAccount = new AccountEntity()
                        {
                            Name = accountInfo.OrganizationName,
                            cgi_organizational_number = accountInfo.OrganizationNumber,
                            ed_TypeOfAccount = Generated.ed_account_ed_typeofaccount.Companycustomerportal
                        };

                        //Make sure to check information source for type of account bool
                        if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.ForetagsPortal)
                        {
                            parentAccount.ed_PortalCustomer = true;
                            //parentAccount.cgi_DebtCollection = true; //??
                            //parentAccount.AccountCategoryCode = Schema.Generated.account_accountcategorycode.Business;//optionset
                        }
                        else if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.SeniorPortal)
                        {
                            parentAccount.ed_SeniorCustomer = true;
                            //parentAccount.cgi_DebtCollection = true; //??
                            //parentAccount.AccountCategoryCode = Schema.Generated.account_accountcategorycode.Senior;//optionset
                        }
                        else if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.SkolPortal)
                        {
                            parentAccount.ed_SchoolCustomer = true;
                            //parentAccount.cgi_DebtCollection = true; //??
                            //parentAccount.AccountCategoryCode = Schema.Generated.account_accountcategorycode.School;//optionset
                        }

                        parentAccount.Id = XrmHelper.Create(localContext, parentAccount);
                        _log.Debug("Parent account/organization created.");
                    }
                    else
                    {
                        /* REMOVED CHECK FOR CONFLICTS
                        if (parentAccounts.Count > 1)
                        {
                            HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.Ambiguous);
                            error.Content = new StringContent($"Found multiple organizations with OrganizationNumber {accountInfo.OrganizationNumber}.");
                            return error;
                        } */

                        _log.Debug($"Existing parent account/organization found. Org. nr: {accountInfo.OrganizationNumber}");
                        parentAccount = parentAccounts[0];

                        AccountEntity uppdateParentAccount = new AccountEntity();
                        uppdateParentAccount.Id = parentAccount.Id;
                        //update parentaccount with type of contact
                        if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.ForetagsPortal && parentAccount.ed_PortalCustomer != true)
                        {
                            uppdateParentAccount.ed_PortalCustomer = true;
                            //parentAccount.AccountCategoryCode = //optionset
                            XrmHelper.Update(localContext, uppdateParentAccount);
                        }
                        else if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.SkolPortal && parentAccount.ed_SchoolCustomer != true)
                        {
                            uppdateParentAccount.ed_SchoolCustomer = true;
                            //parentAccount.AccountCategoryCode = //optionset
                            XrmHelper.Update(localContext, uppdateParentAccount);
                        }
                        else if (accountInfo.InformationSource == (int)Schema.Generated.ed_informationsource.SeniorPortal && parentAccount.ed_SeniorCustomer != true)
                        {
                            uppdateParentAccount.ed_SeniorCustomer = true;
                            //parentAccount.AccountCategoryCode = //optionset
                            XrmHelper.Update(localContext, uppdateParentAccount);
                        }
                    }

                    newAccount.ParentAccountId = parentAccount.ToEntityReference();
                    newAccount.Id = XrmHelper.Create(localContext, newAccount);

                    _log.Debug($"New Cost Site (Account) created with parent account. AccountId: {newAccount.Id}");

                    IList<CustomerAddressEntity> addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, CustomerAddressEntity.CustomerAddressInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, newAccount.Id)
                            }
                        });

                    if (accountInfo.Addresses != null && accountInfo.Addresses.Count > 0)
                    {
                        foreach (AddressInfo addressInfo in accountInfo.Addresses)
                        {
                            if (addressInfo.TypeCode.HasValue)
                            {
                                bool found = false;
                                foreach (CustomerAddressEntity existingAddress in addresses)
                                {
                                    if ((int?)existingAddress.AddressTypeCode == addressInfo.TypeCode)
                                    {
                                        found = true;

                                        CustomerAddressEntity newAddress = new CustomerAddressEntity();
                                        if (AddressInfo.GetChangedCustomerAddressEntity(existingAddress, addressInfo, ref newAddress))
                                        {
                                            XrmHelper.Update(localContext, newAddress);
                                        }
                                    }
                                }

                                if (!found)
                                {
                                    _log.Debug($"No existing addresses found on new account. Creating addresses for new account PortalId: {newAccount.cgi_organizational_number} ");
                                    CustomerAddressEntity cae = AddressInfo.GetCustomerAddressEntityFromAddressInfo(addressInfo);
                                    cae.ParentId = newAccount.ToEntityReference();

                                    XrmHelper.Create(localContext, cae);
                                }

                            }
                            else
                            {
                                _log.Error("Invalid address. Must contain TypeCode.");
                                HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                error.Content = new StringContent($"Address {addressInfo.Name} must contain valid TypeCode.");
                                return error;
                            }
                        }
                    }

                    _log.Debug("Account successfully created! ");
                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent("Account created.");
                    return resp;

                }

            }
            catch (Exception ex)
            {
                _log.Error("Unexpected error from Account-POST");
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage AccountPut(int threadId, AccountInfo accountInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    StatusBlock validateStatus = AccountUtility.ValidateAccountInfo(localContext, accountInfo, false); //Kontrollera så att detta stämmer med det vi har diskuterat
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, AccountEntity.AccountInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(AccountEntity.Fields.AccountId, ConditionOperator.Equal, accountInfo.Guid)
                            }
                        });

                    if (accounts == null || accounts.Count == 0)
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"Found no accounts with ID {accountInfo.Guid}.");
                        return error;
                    }

                    AccountEntity existingAccount = accounts[0];
                    if (existingAccount.StateCode != Generated.AccountState.Active)
                    {
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.Forbidden);
                        error.Content = new StringContent($"The requsted Account must be active.");
                        return error;
                    }

                    AccountEntity newAccount = new AccountEntity();
                    if (AccountInfo.GetChangedAccountEntity(accounts[0], accountInfo, ref newAccount))
                    {
                        if (!string.IsNullOrEmpty(newAccount.cgi_organizational_number))
                        {
                            HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            error.Content = new StringContent($"Cannot change value of existing OrganizationNumber.");
                            return error;
                        }

                        XrmHelper.Update(localContext, newAccount);

                        IList<CustomerAddressEntity> addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, CustomerAddressEntity.CustomerAddressInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, newAccount.Id)
                            }
                        });

                        if (accountInfo.Addresses != null && accountInfo.Addresses.Count > 0)
                        {
                            foreach (AddressInfo addressInfo in accountInfo.Addresses)
                            {
                                if (addressInfo.TypeCode.HasValue)
                                {
                                    bool found = false;
                                    foreach (CustomerAddressEntity existingAddress in addresses)
                                    {
                                        if ((int?)existingAddress.AddressTypeCode == addressInfo.TypeCode)
                                        {
                                            found = true;

                                            CustomerAddressEntity newAddress = new CustomerAddressEntity();
                                            if (AddressInfo.GetChangedCustomerAddressEntity(existingAddress, addressInfo, ref newAddress))
                                            {
                                                XrmHelper.Update(localContext, newAddress);
                                            }
                                        }
                                    }

                                    if (!found)
                                    {
                                        CustomerAddressEntity cae = AddressInfo.GetCustomerAddressEntityFromAddressInfo(addressInfo);
                                        cae.ParentId = newAccount.ToEntityReference();

                                        XrmHelper.Create(localContext, cae);
                                    }

                                }
                                else
                                {
                                    HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                    error.Content = new StringContent($"Address {addressInfo.Name} must contain valid TypeCode.");
                                    return error;
                                }
                            }
                        }
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent("Account updated.");
                    return resp;

                }

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// Method for creating a customer (Konto) connected to an Account.
        /// Creating a Company, School or Senior typ of customer (Konto).
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreatePortalCustomer(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    #region Validations

                    // Validates Email (from Company Role) and Social Security Number
                    StatusBlock validateEmailAndSocialSecurityNumberStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);

                    // Return 400 Bad Request if not validated
                    if (!validateEmailAndSocialSecurityNumberStatus.TransactionOk)
                    {
                        _log.Debug("Returning Bad Request. Email or SocialSecurityNumber was not valid.");

                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateEmailAndSocialSecurityNumberStatus.ErrorMessage);
                        return responseMessage;
                    }


                    // Validate First- and Lastname
                    StatusBlock validateFirstLastNameStatus = CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);

                    // Return 400 Bad Request if not validated
                    if (!validateFirstLastNameStatus.TransactionOk)
                    {
                        _log.Debug("Returning Bad Request. First- or Lastname was not valid.");

                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateFirstLastNameStatus.ErrorMessage);
                        return responseMessage;
                    }


                    // Validate Company Role object of CustomerInfo
                    StatusBlock validateRoleStatus = CustomerUtility.ValidateRoleInfo(localContext, customerInfo);

                    // Return 400 Bad Request if not validated
                    if (!validateRoleStatus.TransactionOk)
                    {
                        _log.Debug("Returning Bad Request. Company Role object was not valid.");

                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateRoleStatus.ErrorMessage);
                        return responseMessage;
                    }
                    #endregion


                    HttpResponseMessage resp = new HttpResponseMessage();
                    resp.StatusCode = HttpStatusCode.OK;

                    Guid? contactId = CompanyRoleEntity.CreateNewCompanyRole(localContext, customerInfo, ref resp); //This is where the Match happens

                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        return resp;
                    }
                    else
                    {
                        resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = new StringContent(SerializeNoNull(contactId));
                        return resp;
                    }

                }

            }
            catch (BadRequestException ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(ex.Message);
                return rm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage NonLoginCustomerIncident(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    customerInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.OinloggatKundArende;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo); //Check (Partially done)
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        respMess.Content = new StringContent(validateStatus.ErrorMessage);
                        return respMess;
                    }
                    // Find customer according to rules
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); // CHECK - Matchning

                    // Find possible Leads and connect possible purchaseInfo. //Ska det tas bort?
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                                new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                                new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                                new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                                new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                            }
                        });

                    // Object used for update.
                    ContactEntity updContact = new ContactEntity()
                    {
                        ContactId = contact.ContactId
                    };

                    bool update = false;
                    foreach (LeadEntity lead in leads)
                    {
                        if (ContactEntity.UpdateContactWithLead(ref contact, ref updContact, lead))
                            update = true;
                        if (update)
                        {
                            contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKundArende;
                            localContext.OrganizationService.Update(contact);
                        }
                    }

                    if (update)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKundArende;
                        updContact.ed_InformationSource = Generated.ed_informationsource.OinloggatKundArende;
                        localContext.OrganizationService.Update(updContact);
                    }

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                    };
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage NonLoginPurchase(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    customerInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.OinloggatKop;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo); //CHECK
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat($"Th={threadId} - NonLoginPurchase, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); //CHECK ??
                    string contactString = contact == null ? "null" : contact.ContactId.ToString();
                    _log.DebugFormat($"Th={threadId} - NonLoginPurchase, Found/Created contact {contactString}");

                    // Attach Leads
                    IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            (!String.IsNullOrEmpty(customerInfo.FirstName)) ? new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName) : new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            (!String.IsNullOrEmpty(customerInfo.LastName)) ? new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName) : new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                            }
                        });
                    bool updated = false;

                    // Object used for update.
                    ContactEntity updContact = new ContactEntity()
                    {
                        ContactId = contact.ContactId
                    };

                    foreach (LeadEntity lead in leads)
                    {
                        if (ContactEntity.UpdateContactWithLead(ref contact, ref updContact, lead))
                            updated = true;
                    }
                    if (updated)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKop;
                        updContact.ed_InformationSource = Generated.ed_informationsource.OinloggatKop;
                        localContext.OrganizationService.Update(updContact);
                    }

                    HttpResponseMessage leadResp = new HttpResponseMessage(HttpStatusCode.OK);
                    leadResp.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                    return leadResp;

                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        public static HttpResponseMessage NonLoginRefill(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    customerInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.OinloggatLaddaKort;
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
                        respMess.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
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
                                _log.DebugFormat($"Th={threadId} - Unexpected Exception caught when retrieving Lead. Error messagee: {0}", e.Message);
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static HttpResponseMessage RGOL(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - reating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    _log.DebugFormat($"Th={threadId} - RGOL, validating info");
                    customerInfo.Source = (int)Crm.Schema.Generated.ed_informationsource.RGOL;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo); //CHECK
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    // Find/ Create Customer
                    _log.DebugFormat($"Th={threadId} - RGOL, calling FindOrCreateUnvalidatedContact");
                    ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); //CHECK ??
                    _log.DebugFormat($"Th={threadId} - RGOL, Found/created contact {(contact == null ? "null" : contact.ContactId.ToString())}");

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
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                                }
                            });


                        // Object used for update.
                        ContactEntity updContact = new ContactEntity()
                        {
                            ContactId = contact.ContactId
                        };
                        bool updated = false;
                        foreach (LeadEntity lead in leads)
                        {
                            if (ContactEntity.UpdateContactWithLead(ref contact, ref updContact, lead))
                                updated = true;
                        }
                        if (updated)
                        {
                            contact.ed_InformationSource = Generated.ed_informationsource.RGOL;
                            updContact.ed_InformationSource = Generated.ed_informationsource.RGOL;
                            localContext.OrganizationService.Update(updContact);
                        }
                    }

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message))
                };
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="lead"></param>
        /// <returns></returns>
        public static ContactEntity QualifyLeadToContact(Plugin.LocalPluginContext localContext, LeadEntity lead)
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
            if (resp.CreatedEntities.Count == 0)
            {
                throw new Exception(string.Format("Lead validation failed. Qualify lead returned no result"));
            }
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

            // Object used for update.
            ContactEntity updContact = new ContactEntity()
            {
                ContactId = qualifiedContact.ContactId
            };

            if (ContactEntity.UpdateContactWithLead(ref qualifiedContact, ref updContact, lead))
                localContext.OrganizationService.Update(updContact);

            return qualifiedContact;
        }

        //public static HttpResponseMessage NotifyMKLSent(int threadId, NotificationInfo notificationInfo)
        //{
        //    try
        //    {
        //        CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
        //        _log.DebugFormat("Creating serviceProxy");
        //        // Cast the proxy client to the IOrganizationService interface.
        //        using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
        //        {
        //            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

        //            if (localContext.OrganizationService == null)
        //                throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

        //            StatusBlock status = HandleSingleNotifyMKL(localContext, notificationInfo);
        //            return new HttpResponseMessage(status.TransactionOk ? HttpStatusCode.OK : HttpStatusCode.BadRequest)
        //            {
        //                Content = new StringContent(SerializeNoNull(status))
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //        rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //        return rm;
        //    }
        //    finally
        //    {
        //        ConnectionCacheManager.ReleaseConnection(threadId);
        //    }
        //}

        private static StatusBlock HandleSingleNotifyMKL(Plugin.LocalPluginContext localContext, NotificationInfo notificationInfo)
        {
            try
            {
                StatusBlock verificationStatus = VerifyNotificationInfo(notificationInfo);
                if (!verificationStatus.TransactionOk)
                    return verificationStatus;

                #region Last Seen Date
                {
                    //// 2018-07-31 - Marcus Stenswed
                    //// Update NotifyMKL (existing if found, otherwise create new) with LastSeenDate
                    //// Should only exist one record on every Contact

                    //ContactEntity contactLastSeen = null;
                    //contactLastSeen = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(
                    //    ContactEntity.Fields.FullName,
                    //    ContactEntity.Fields.EMailAddress1,
                    //    ContactEntity.Fields.MobilePhone),
                    //    new FilterExpression
                    //    {
                    //        Conditions =
                    //        {
                    //            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, notificationInfo.Guid)
                    //        }
                    //    });

                    //if (contactLastSeen != null)
                    //{
                    //    NotifyMKLEntity notificationMKLLastSeenDate = null;
                    //    notificationMKLLastSeenDate = XrmRetrieveHelper.RetrieveFirst<NotifyMKLEntity>(localContext, new ColumnSet(NotifyMKLEntity.Fields.StateCode),
                    //        new FilterExpression
                    //        {
                    //            Conditions =
                    //            {
                    //        new ConditionExpression(NotifyMKLEntity.Fields.RegardingObjectId, ConditionOperator.Equal, contactLastSeen.Id),
                    //        new ConditionExpression(NotifyMKLEntity.Fields.ed_LastSeenDate, ConditionOperator.NotNull)
                    //            }
                    //        });
                    //    if (notificationMKLLastSeenDate == null)
                    //    {
                    //        string targetAddressLastSeen = null;
                    //        if ((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType == Generated.ed_notifymkl_ed_method.Mail)
                    //        {
                    //            targetAddressLastSeen = contactLastSeen.EMailAddress1;
                    //        }
                    //        else if ((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType == Generated.ed_notifymkl_ed_method.Sms)
                    //        {
                    //            targetAddressLastSeen = contactLastSeen.MobilePhone;
                    //        }

                    //        NotifyMKLEntity notificationLastSeen = new NotifyMKLEntity
                    //        {
                    //            Subject = $"Last Seen Date - {contactLastSeen.FullName}",
                    //            ed_NotificationType = (Generated.ed_notifymkl_ed_notificationtype)notificationInfo.NotificationType,
                    //            ed_Method = (Generated.ed_notifymkl_ed_method)notificationInfo.ContactType,
                    //            ed_TargetAddress = targetAddressLastSeen,
                    //            ed_TimeStamp = notificationInfo.TimeStamp,
                    //            ed_MKLActivityLogId = notificationInfo.ActivityLogId,
                    //            ed_LastSeenDate = (notificationInfo.TimeStamp.HasValue) ? notificationInfo.TimeStamp.Value.ToLocalTime() : DateTime.Now,
                    //            RegardingObjectId = new EntityReference
                    //            {
                    //                Id = contactLastSeen.Id,
                    //                LogicalName = ContactEntity.EntityLogicalName
                    //            }
                    //        };
                    //        notificationLastSeen.Id = XrmHelper.Create(localContext.OrganizationService, notificationLastSeen);
                    //    }
                    //    else if (notificationMKLLastSeenDate != null)
                    //    {
                    //        if(notificationMKLLastSeenDate.StateCode == Generated.ed_notifymklState.Canceled ||
                    //            notificationMKLLastSeenDate.StateCode == Generated.ed_notifymklState.Completed)
                    //        {
                    //            SetStateRequest req = new SetStateRequest()
                    //            {
                    //                EntityMoniker = notificationMKLLastSeenDate.ToEntityReference(),
                    //                State = new OptionSetValue((int)Generated.ed_notifymklState.Open),
                    //                Status = new OptionSetValue((int)Generated.ed_notifymkl_statuscode.Open)
                    //            };
                    //            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                    //            notificationMKLLastSeenDate = XrmRetrieveHelper.Retrieve<NotifyMKLEntity>(localContext, notificationMKLLastSeenDate.Id, new ColumnSet(false));
                    //        }

                    //        if (notificationInfo.TimeStamp.HasValue)
                    //            notificationMKLLastSeenDate.ed_LastSeenDate = notificationInfo.TimeStamp.Value.ToLocalTime();
                    //        else
                    //            notificationMKLLastSeenDate.ed_LastSeenDate = DateTime.Now;

                    //        if(notificationInfo.ActivityLogId != null)
                    //            notificationMKLLastSeenDate.ed_MKLActivityLogId = notificationInfo.ActivityLogId;
                    //        XrmHelper.Update(localContext.OrganizationService, notificationMKLLastSeenDate);

                    //        SetStateRequest req2 = new SetStateRequest()
                    //        {
                    //            EntityMoniker = notificationMKLLastSeenDate.ToEntityReference(),
                    //            State = new OptionSetValue((int)Generated.ed_notifymklState.Completed),
                    //            Status = new OptionSetValue((int)Generated.ed_notifymkl_statuscode.Completed)
                    //        };
                    //        SetStateResponse resp2 = (SetStateResponse)localContext.OrganizationService.Execute(req2);
                    //    }
                    //}
                }
                #endregion

                if (!((int)Generated.ed_notifymkl_ed_notificationtype.Login == notificationInfo.NotificationType ||
                    (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword == notificationInfo.NotificationType ||
                    (int)Generated.ed_notifymkl_ed_notificationtype.PasswordChanged == notificationInfo.NotificationType))
                {
                    return new StatusBlock
                    {
                        TransactionOk = true,
                        Information = SerializeNoNull(notificationInfo)
                    };
                }

                ContactEntity contact = null;
                contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(
                    ContactEntity.Fields.FullName,
                    ContactEntity.Fields.EMailAddress1,
                    ContactEntity.Fields.Telephone2),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, notificationInfo.Guid)
                        }
                    });
                if (contact == null)
                {
                    return new StatusBlock
                    {
                        ErrorMessage = string.Format(Resources.CouldNotFindContactWithInfo, notificationInfo.Guid),
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.NoContactFound,
                        Information = SerializeNoNull(notificationInfo),
                        TransactionOk = false
                    };
                }
                string failString = "";
                if (notificationInfo.Status.HasValue && notificationInfo.Status.Value == 2)
                    failString = "Failed ";

                string targetAddress = null;
                if ((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType == Generated.ed_notifymkl_ed_method.Mail)
                {
                    targetAddress = contact.EMailAddress1;
                }
                else if ((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType == Generated.ed_notifymkl_ed_method.Sms)
                {
                    targetAddress = contact.Telephone2;
                }

                NotifyMKLEntity notification = new NotifyMKLEntity
                {
                    Subject = $"{failString}{(Generated.ed_notifymkl_ed_notificationtype)notificationInfo.NotificationType} - {contact.FullName}",
                    ed_NotificationType = (Generated.ed_notifymkl_ed_notificationtype)notificationInfo.NotificationType,
                    ed_Method = (Generated.ed_notifymkl_ed_method)notificationInfo.ContactType,
                    ed_TargetAddress = targetAddress,
                    ed_TimeStamp = notificationInfo.TimeStamp,
                    ed_MKLActivityLogId = notificationInfo.ActivityLogId,
                    RegardingObjectId = new EntityReference
                    {
                        Id = contact.Id,
                        LogicalName = ContactEntity.EntityLogicalName
                    }
                };
                notification.Id = XrmHelper.Create(localContext.OrganizationService, notification);

                // Update the contact. 
                if (notificationInfo.NotificationType == (int)Generated.ed_notifymkl_ed_notificationtype.Login && notificationInfo.TimeStamp.HasValue && notificationInfo.Status.Value == 1)
                {
                    ContactEntity updateContact = new ContactEntity
                    {
                        Id = contact.Id,
                        cgi_MyAccount_LastLogin = notificationInfo.TimeStamp.Value.ToLocalTime()
                    };
                    XrmHelper.Update(localContext, updateContact);
                }

                return new StatusBlock
                {
                    TransactionOk = true,
                    Information = SerializeNoNull(notificationInfo)
                };
            }
            catch (Exception e)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    Information = SerializeNoNull(notificationInfo),
                    ErrorMessage = string.Format(Resources.UnexpectedException, e.Message),
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.GenericError
                };
            }
        }

        internal static HttpResponseMessage NotifyMKLsSent(int threadId, NotificationInfo[] notificationInfos)
        {
            try
            {
                List<StatusBlock> results = new List<StatusBlock>();

                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    bool atLeastOneError = false, atLeastOneAccepted = false;

                    _log.Debug($"Th={threadId} - Entered NotifyMKLsSent for {notificationInfos.Length} notification Objects");
                    for (int i = 0; i < notificationInfos.Length; i++)
                    {
                        Guid guid = Guid.Empty;
                        if (notificationInfos[i].Guid == null || !Guid.TryParse(notificationInfos[i].Guid, out guid))
                        {
                            results.Add(new StatusBlock
                            {
                                ErrorMessage = Resources.GuidNotValid,
                                Information = SerializeNoNull(notificationInfos[i]),
                                TransactionOk = false,
                                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput
                            });
                            atLeastOneError = true;
                            continue;
                        }

                        StatusBlock respStatus = HandleSingleNotifyMKL(localContext, notificationInfos[i]);
                        results.Add(respStatus);
                        if (respStatus.TransactionOk == false)
                            atLeastOneError = true;
                        else
                            atLeastOneAccepted = true;
                    }
                    HttpResponseMessage resp = null;
                    if (atLeastOneError && atLeastOneAccepted)
                    {
                        resp = new HttpResponseMessage(HttpStatusCode.Ambiguous);
                    }
                    else if (atLeastOneAccepted)
                    {
                        resp = new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                    resp.Content = new StringContent(SerializeNoNull(results));
                    return resp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        private static StatusBlock VerifyNotificationInfo(NotificationInfo notificationInfo)
        {
            StatusBlock returnStatus = new StatusBlock
            {
                Information = SerializeNoNull(notificationInfo)
            };

            if (string.IsNullOrWhiteSpace(notificationInfo.Guid))
            {
                returnStatus.TransactionOk = false;
                returnStatus.ErrorMessage = Resources.GuidMissing;
                returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
                return returnStatus;
            }


            if (notificationInfo.ContactType != (int)Generated.ed_notifymkl_ed_method.Mail &&
                notificationInfo.ContactType != (int)Generated.ed_notifymkl_ed_method.NotApplicable &&
                notificationInfo.ContactType != (int)Generated.ed_notifymkl_ed_method.Sms)
            {
                returnStatus.TransactionOk = false;
                returnStatus.ErrorMessage = string.Format(Resources.InvalidContactType, notificationInfo.ContactType);
                returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
                return returnStatus;
            }

            //if (notificationInfo.NotificationType != (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword &&
            //    notificationInfo.NotificationType != (int)Generated.ed_notifymkl_ed_notificationtype.Login &&
            //    notificationInfo.NotificationType != (int)Generated.ed_notifymkl_ed_notificationtype.PasswordChanged &&
            //    notificationInfo.NotificationType != (int)Generated.ed_notifymkl_ed_notificationtype.RefreshToken)
            //{
            //    returnStatus.TransactionOk = false;
            //    returnStatus.ErrorMessage = string.Format(Resources.InvalidNotificationType, notificationInfo.NotificationType);
            //    returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
            //    return returnStatus;
            //}
            if (notificationInfo.TimeStamp == null)
            {
                returnStatus.TransactionOk = false;
                returnStatus.ErrorMessage = Resources.TimeStampMissing;
                returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
                return returnStatus;
            }

            //if (notificationInfo.NotificationType == (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword && notificationInfo.ContactType != (int)Generated.ed_notifymkl_ed_method.NotApplicable)
            //{
            //    returnStatus.TransactionOk = false;
            //    returnStatus.ErrorMessage = Resources.NotificationMissingSendTo;
            //    returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
            //    return returnStatus;
            //}
            // StatusFlag Checks - Only allowed when Type is "Login" or "Refresh Token". Must be "1" (Success) or "2" (failure)
            if (notificationInfo.NotificationType == (int)Generated.ed_notifymkl_ed_notificationtype.Login || notificationInfo.NotificationType == (int)Generated.ed_notifymkl_ed_notificationtype.RefreshToken)
            {
                if (!notificationInfo.Status.HasValue)
                {
                    returnStatus.TransactionOk = false;
                    returnStatus.ErrorMessage = Resources.NotificationMissingStatus;
                    returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
                    return returnStatus;
                }
                else if (notificationInfo.Status.Value != 1 && notificationInfo.Status.Value != 2)
                {
                    returnStatus.TransactionOk = false;
                    returnStatus.ErrorMessage = string.Format(Resources.NotificationInvalidStatus, notificationInfo.Status.Value);
                    returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput;
                    return returnStatus;
                }
            }

            returnStatus.TransactionOk = true;
            returnStatus.StatusBlockCode = (int)CustomerUtility.StatusBlockCode.DataValid;
            return returnStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static HttpResponseMessage UpdateContact(int threadId, CustomerInfo customer)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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
                    customer.Source = (int)Generated.ed_informationsource.UppdateraMittKonto;
                    StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer); //CHECK
                    if (!validateStatus.TransactionOk)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(validateStatus.ErrorMessage)
                        };
                    }

                    // Get contact and verify that it is active
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

                    // 2019-07-22 - Marcus Stenswed
                    // If updating Mobile, check for other conflicting Contacts
                    if (contact.Telephone2 != customer.Mobile)
                    {
                        FilterExpression privateCustomerFilter = new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                // Null == true, fallback if no data
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Null),
                            }
                        };

                        ContactEntity conflictContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email),
                                    new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customer.Mobile)
                                },
                                Filters = {
                                    privateCustomerFilter,
                                }
                            });
                        if (conflictContact != null)
                        {
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            response.Content = new StringContent(string.Format(Resources.EmailAlreadyInUse, customer.Email + " och " + customer.Guid));
                            return response;
                        }
                    }


                    // Validera att en eventuell uppdatering inte skulle bryta mot affärsregler (inga dubletter) - Om så returnera lämpligt felmeddelande
                    if (customer.SwedishSocialSecurityNumberSpecified && customer.SwedishSocialSecurityNumber)
                    {

                        FilterExpression privateCustomerFilter = new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                // Null == true, fallback if no data
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Null),
                            }
                        };

                        QueryExpression query = new QueryExpression()
                        {
                            EntityName = ContactEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(false),
                            Criteria = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber), //Change social security number
                                    //new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true), // Check if this should be included
                                    new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.NotEqual, customer.Guid)
                                },
                                Filters = {
                                    privateCustomerFilter,
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
                            else
                            {
                                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                response.Content = new StringContent(Resources.CouldNotUpdateCustomerSocialSecurityConflict);
                                return response;
                            }
                        }
                    }

                    // TODO - Marcus, nedan måste testas!
                    // Done 20190719 /Marcus

                    ContactEntity newInfo = new ContactEntity(localContext, customer);
                    if (newInfo.IsAnyAttributeModified(contact, ContactEntity.ContactInfoBlock.Columns.ToArray()))
                    {
                        //contact.CombineAttributes(newInfo);

                        string[] attrs = new string[newInfo.Attributes.Count];

                        int i = 0;
                        foreach (var attr in newInfo.Attributes)
                        {
                            attrs[i] = attr.Key;
                            i++;
                        }

                        //Entity contact1 = contact.CombineModifiedAttributes(newInfo);
                        AttributeCollection collection = newInfo.GetModifiedAttributes(contact, attrs);
                        Guid contactId = contact.Id;

                        ContactEntity updContact = new ContactEntity()
                        {
                            Attributes = collection,
                            Id = contactId
                        };

                        if (collection.Count > 2)
                        {
                            updContact.ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto;
                            contact.ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto;
                            localContext.OrganizationService.Update(updContact);
                        }
                    }

                    contact.Attributes = newInfo.Attributes;

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message))
                };
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage UpdatePortalCustomer(int threadId, CustomerInfo customerInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                    StatusBlock validateCustomerStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    if (!validateCustomerStatus.TransactionOk)
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent(validateCustomerStatus.ErrorMessage);
                        return responseMessage;
                    }

                    //REMOVE BELLOW - Hur ska jag göra med denna?
                    //if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber)) // Put control on Email and Mobile
                    //{
                    //    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //    responseMessage.Content = new StringContent("SSN for customer cannot be empty.");
                    //    return responseMessage;
                    //}

                    //Ny Matchninh för företagskund (we use CompanyRole for email and telephone)
                    //if (string.IsNullOrWhiteSpace(customerInfo.Email) || string.IsNullOrWhiteSpace(customerInfo.Mobile)) // Put control on Email and Mobile
                    //{
                    //    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //    responseMessage.Content = new StringContent("Email and Mobile for customer cannot be empty.");
                    //    return responseMessage;
                    //}

                    if (customerInfo.CompanyRole != null && string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email) /*|| string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone)*/) // Put control on Email and Mobile
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        responseMessage.Content = new StringContent("Email and Mobile for customer cannot be empty.");
                        return responseMessage;
                    }

                    // MS: If company portal only updates flag to block Contact. Do not validate CompanyRole
                    if (customerInfo.CompanyRole == null)
                    {
                        // Update has changes since we are not working with regular mail and telefone
                        StatusBlock validateBlockContactInfo = CustomerUtility.ValidateBlockContactInfo(localContext, customerInfo);
                        if (!validateBlockContactInfo.TransactionOk)
                        {
                            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            responseMessage.Content = new StringContent(validateBlockContactInfo.ErrorMessage);
                            return responseMessage;
                        }

                        ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext,
                            new Guid(customerInfo.Guid),
                            new ColumnSet(ContactEntity.Fields.ed_isLockedPortal));

                        if (contact.ed_isLockedPortal != null)
                        {
                            if (contact.ed_isLockedPortal != customerInfo.isLockedPortal)
                            {
                                contact.ed_isLockedPortal = customerInfo.isLockedPortal;
                                XrmHelper.Update(localContext.OrganizationService, contact);
                            }
                        }
                        else
                        {
                            contact.ed_isLockedPortal = customerInfo.isLockedPortal;
                            XrmHelper.Update(localContext.OrganizationService, contact);
                        }
                    }
                    else
                    {
                        StatusBlock validateRoleStatus = CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
                        if (!validateRoleStatus.TransactionOk)
                        {
                            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            responseMessage.Content = new StringContent(validateRoleStatus.ErrorMessage);
                            return responseMessage;
                        }

                        CompanyRoleEntity updateRole = CompanyRoleEntity.GetUpdateCompanyRole(localContext, customerInfo);
                        if (updateRole != null)
                        {
                            XrmHelper.Update(localContext, updateRole);
                        }
                    }

                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent("Company customer updated.");
                    return resp;

                }

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// Finds Contact based on socialSecurityNumber and portalId. Updates Contact with new email. Returns ContactId of Contact.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="portalId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static HttpResponseMessage SynchronizeContactData(int threadId, string socialSecurityNumber, string portalId, string emailAddress)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");

                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    // Get Contact(-s)
                    QueryExpression contactQuery = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId, ContactEntity.Fields.EMailAddress1),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.ed_SocialSecurityNumberBlock, ConditionOperator.Equal, socialSecurityNumber),
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.ed_BusinessContact, ConditionOperator.Equal, true)
                            }
                        },
                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = ContactEntity.EntityLogicalName,
                                LinkToEntityName = CompanyRoleEntity.EntityLogicalName,
                                LinkFromAttributeName = ContactEntity.Fields.ContactId,
                                LinkToAttributeName = CompanyRoleEntity.Fields.ed_Contact,
                                EntityAlias = CompanyRoleEntity.EntityLogicalName,
                                JoinOperator = JoinOperator.Inner,
                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(CompanyRoleEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_CompanyRoleState.Active)
                                    }
                                },

                                LinkEntities =
                                {
                                    new LinkEntity()
                                    {
                                        LinkFromEntityName = CompanyRoleEntity.EntityLogicalName,
                                        LinkToEntityName = AccountEntity.EntityLogicalName,
                                        LinkFromAttributeName = CompanyRoleEntity.Fields.ed_Account,
                                        LinkToAttributeName = AccountEntity.Fields.AccountId,
                                        EntityAlias = AccountEntity.EntityLogicalName,
                                        JoinOperator = JoinOperator.Inner,
                                        LinkCriteria =
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, portalId),
                                                new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };

                    List<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, contactQuery);

                    if (contacts == null || contacts.Count > 1)
                    {
                        HttpResponseMessage rmNoContactFound = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rmNoContactFound.Content = new StringContent(string.Format(Resources.UnexpectedException, 
                            $"Found no matching contact for PortalId: {portalId} and SSN: {socialSecurityNumber}"));
                        return rmNoContactFound;
                    }

                    ContactEntity contact = contacts.First();

                    // Update Contact with new email
                    if (contact.EMailAddress1.ToLower() != emailAddress.ToLower())
                    {
                        contact.EMailAddress1 = emailAddress;
                        contact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                        XrmHelper.Update(localContext, contact);
                    }

                    // Return ContactId
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.OK);
                    rm.Content = new StringContent(contact.ContactId.ToString());
                    return rm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customerId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="latestLinkGuid"></param>
        /// <param name="mklId"></param>
        /// <returns></returns>
        public static HttpResponseMessage ValidateEmail(int threadId, Guid customerId, int entityTypeCode, string latestLinkGuid, string mklId)
        {
            try
            {
                _log.DebugFormat($"Th={threadId} - Entered ValidateEmail(), customerId: {customerId}, entityTypeCode: {entityTypeCode}, latestLinkGuid: {latestLinkGuid}");
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
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

                    FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_SplittCompany);

                    switch (entityTypeCode)
                    {
                        #region case Contact
                        case ContactEntity.EntityTypeCode:
                            _log.DebugFormat($"Th={threadId} - Retrieving Contact with Id: {customerId}");

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
                                _log.Error($"Th={threadId} - Could not find a Contact with the provided Guid.");
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
                            //if (contact.ed_LinkExpiryDate.HasValue && contact.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) > 0)
                            if (contact.ed_LinkExpiryDate.HasValue && DateTime.Now.CompareTo(contact.ed_LinkExpiryDate.Value) > 0)
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

                            // Get all attributes (original) from Contact to use later
                            AttributeCollection backupAttr = new AttributeCollection();
                            foreach (var attr in contact.Attributes)
                            {
                                backupAttr.Add(attr.Key, attr.Value);
                            }

                            ContactEntity firstRetrievedContact = (ContactEntity)contact;

                            #region Conflicting Contacts Query
                            FilterExpression conflictFilter = new FilterExpression(LogicalOperator.Or);
                            FilterExpression mailFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                    //new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, contact.MobilePhone),
                                    //(contact.MobilePhone != null) ? new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, contact.MobilePhone) : new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Null),
                                    //new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                    //new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id),
                                    new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true)
                                }
                            };
                            conflictFilter.AddFilter(mailFilter);

                            if (feature != null && feature.ed_SplittCompany == false)
                            {
                                if (contact.ed_HasSwedishSocialSecurityNumber.HasValue && contact.ed_HasSwedishSocialSecurityNumber.Value)
                                {
                                    FilterExpression socsecFilter = new FilterExpression(LogicalOperator.And)
                                    {
                                        Conditions =
                                        {
                                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull),
                                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contact.cgi_socialsecuritynumber), //Change personal number
                                            new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)

                                        }
                                    };
                                    conflictFilter.AddFilter(socsecFilter);
                                }
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
                                if (feature != null && feature.ed_SplittCompany == true)
                                {
                                    if (contact.ed_EmailToBeVerified.Equals(contactConflicts[0].EMailAddress1))
                                    {
                                        _log.DebugFormat($"Th={threadId} - Found existing, validated contact with conflicting email {contact.ed_EmailToBeVerified}. Cannot validate this contact.");
                                        HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                        rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerEmail);
                                        return rm3;
                                    }
                                }
                                else if (feature == null || (feature != null && feature.ed_SplittCompany == false))
                                {
                                    if (contact.ed_EmailToBeVerified.Equals(contactConflicts[0].EMailAddress1))
                                    {
                                        _log.DebugFormat($"Th={threadId} - Found existing, validated contact with conflicting email {contact.ed_EmailToBeVerified}. Cannot validate this contact.");
                                        HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                        rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerEmail);
                                        return rm3;
                                    }
                                    else
                                    {
                                        _log.DebugFormat($"Th={threadId} - Found existing, validated contact with Social Security Number {contactConflicts[0].cgi_socialsecuritynumber}. Cannot validate this contact.");
                                        HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                        rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerSocSec);
                                        return rm3;
                                    }
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
                                            //new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                            (contact.FirstName != null) ? new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName) : new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Null),
                                            //new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                            (contact.LastName != null) ? new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName) : new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Null),
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

                            // If there is a socialSecuriityNumber, add condition
                            if (feature != null && feature.ed_SplittCompany == false)
                            {
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
                            }

                            FilterExpression epostFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                    //new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, contact.MobilePhone),
                                    (contact.MobilePhone != null) ? new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, contact.MobilePhone) : new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Null),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id),
                                    new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.NotEqual, true)
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
                            _log.DebugFormat($"Th={threadId} - Found {mergeContacts.Count} conflicting contacts to merge with the one to be validated");
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
                                        //new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                        (contact.FirstName != null) ? new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName) : new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Null),
                                        //new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                        (contact.LastName != null) ? new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName) : new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Null),
                                        new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                                    }
                                }
                            };
                            #endregion
                            IList<LeadEntity> leadConflicts = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, conflictLeadQuery);
                            _log.DebugFormat($"Th={threadId} - Found {leadConflicts.Count} conflicting leads to merge with the one to be validated");

                            // Do merge if required
                            if (mergeContacts.Count > 0)
                            {
                                contact.CombineContacts(localContext, mergeContacts);
                            }

                            // TODO - Marcus, Uppdatera bara det som är nödvändigt!
                            // Varför inte uppdatera bara det som är ändrat!

                            // Object used for update.
                            ContactEntity updContact = new ContactEntity()
                            {
                                ContactId = contact.ContactId
                            };

                            if (leadConflicts.Count > 0)
                            {
                                foreach (LeadEntity leadConflict in leadConflicts)
                                    ContactEntity.UpdateContactWithLead(ref contact, ref updContact, leadConflict);
                            }

                            //contact.ed_PrivateCustomerContact = true; //?
                            contact.EMailAddress1 = contact.ed_EmailToBeVerified;
                            contact.ed_EmailToBeVerified = ContactEntity._NEWEMAILDONE;
                            contact.EMailAddress2 = null;
                            contact.ed_LatestLinkGuid = null;
                            contact.ed_MklId = mklId;
                            contact.ed_InformationSource = Generated.ed_informationsource.BytEpost;


                            AttributeCollection attributesToUpdate = new AttributeCollection();

                            // todo - marcus
                            // Check if any attributes have been changed since retrieved from CRM (backupAttr)
                            // Store new attributes in new Attribute Collection (attributesToUpdate)
                            foreach (var attr in contact.Attributes)
                            {
                                object value;
                                bool found = backupAttr.TryGetValue(attr.Key, out value);

                                if (backupAttr.ContainsKey(attr.Key) == true && value != attr.Value)
                                {
                                    attributesToUpdate.Add(attr);
                                }
                                else if (backupAttr.ContainsKey(attr.Key) == false)
                                {
                                    attributesToUpdate.Add(attr);
                                }

                                if (attr.Key == ContactEntity.Fields.ContactId)
                                    attributesToUpdate.Add(attr);
                            }

                            updContact.Attributes = attributesToUpdate;
                            localContext.OrganizationService.Update(updContact);

                            CrmPlusUtility.SendConfirmationEmail(localContext, threadId, contact);

                            return new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)))
                            };
                        #endregion

                        #region case Lead
                        case LeadEntity.EntityTypeCode: // Lead
                            #region Find Lead
                            ContactEntity newContact = null;
                            _log.DebugFormat($"Th={threadId} - Retrieving Lead with Id: {customerId}");
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
                                _log.ErrorFormat($"Th={threadId} - Could not find a Lead with the provided Guid: {customerId}");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.NotFound);
                                rml.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customerId));
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(lead.ed_LatestLinkGuid))
                            {
                                _log.ErrorFormat($"Th={threadId} - Could not find a LatestLinkGuid on the retrieved Lead");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.NoLinkGuidFoundOnLead);
                                return rml;
                            }
                            if (!lead.ed_LatestLinkGuid.Equals(latestLinkGuid))
                            {
                                _log.ErrorFormat($"Th={threadId} - LatestLinkGuid used is not the expected one");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.LinkNotMeantForThisLead);
                                return rml;
                            }
                            if (lead.ed_LinkExpiryDate.HasValue && lead.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) < 0)
                            {
                                _log.ErrorFormat($"Th={threadId} - LinkExpiryDate is less that DateTime.Now");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.OldLinkUsed);
                                return rml;
                            }
                            if (string.IsNullOrWhiteSpace(lead.EMailAddress1))
                            {
                                _log.ErrorFormat($"Th={threadId} - Lead to be validated does not contain an email address");
                                HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rml.Content = new StringContent(Resources.NoEmailToBeVerifiedFoundOnLead);
                                return rml;
                            }
                            #endregion


                            // 2020-02-04 - Checking Social Security Number OR (EMailAddress2 & First-/LastName) (OLD)
                            // 2020-05-14 - Checking EMailAddress2 & Telephone2 & ed_PrivateCustomerContact
                            QueryExpression contactConflictQuery = CreateContactConflictQuery(lead);
                            IList<ContactEntity> conflictContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, contactConflictQuery);
                            _log.DebugFormat($"Th={threadId} - Found {conflictContacts.Count} conflicting Contacts");

                            // 2020-02-04 - If no matches found, validate against EMailAddress1 & First-/LastName (OLD)
                            // 2020-05-14 - If no matches found, validate against EMailAddress1 & Telephone2 & ed_PrivateCustomerContact
                            if (conflictContacts == null || conflictContacts.Count() < 1)
                            {
                                contactConflictQuery = CreateContactEMailAddress1ConflictQuery(lead);
                                conflictContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, contactConflictQuery);
                            }

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
                                        new ConditionExpression(LeadEntity.Fields.MobilePhone, ConditionOperator.Equal, lead.MobilePhone),
                                        //new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                                        //new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName),
                                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                                    }
                                }
                            };
                            #endregion
                            IList<LeadEntity> conflictLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, leadConflictQuery);
                            _log.DebugFormat($"Th={threadId} - Found {conflictLeads.Count} conflicting Leads");

                            #region if Contact conflicts are found
                            if (conflictContacts.Count > 0)
                            {

                                //_log.DebugFormat($"Th={threadId} - Found one or more conflicting Contacts with the same EMailAddress / Telephone.");
                                //HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                //respMess.Content = new StringContent("Found one or more conflicting Contacts with the same EMailAddress / Telephone.");
                                //return respMess;

                                //ContactEntity socialSecurityConflict = null;
                                ContactEntity emailAddress1Conflict = null;

                                foreach (ContactEntity c in conflictContacts)
                                {
                                    // 2020-02-19 - Marcus Stenswed
                                    // Remove conflicting SSN (New Business Rules)
                                    /*if (lead.ed_Personnummer != null && lead.ed_Personnummer.Equals(c.cgi_socialsecuritynumber))
                                    {
                                        if (socialSecurityConflict == null)
                                        {
                                            _log.DebugFormat($"Th={threadId} - Found Contact matching on social security number.");
                                            socialSecurityConflict = c;
                                        }
                                        else
                                        {
                                            _log.DebugFormat($"Th={threadId} - Multiple conflicting Contacts with the same socialSecurityNumber were found.");
                                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                            respMess.Content = new StringContent(Resources.MultipleSocSecConflictsFound);
                                            return respMess;
                                        }
                                    }
                                    else */
                                    if (lead.EMailAddress1 != null && lead.EMailAddress1.Equals(c.EMailAddress1))
                                    {
                                        // 2020-06-09 - Marcus Stenswed
                                        // If a conflicting Contact is found with the same EMailAddress1
                                        // it should result in a BadRequest
                                        HttpResponseMessage respMess2 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                        respMess2.Content = new StringContent("Duplicate Contact found with the same primary email.");
                                        return respMess2;

                                        //if (emailAddress1Conflict == null)
                                        //{
                                        //    _log.DebugFormat($"Th={threadId} - Found Contact matching on emailaddress1.");
                                        //    emailAddress1Conflict = c;
                                        //}
                                        //else
                                        //{
                                        //    _log.DebugFormat($"Th={threadId} - Multiple conflicting Contacts with the same EMailAddress1 were found.");
                                        //    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                        //    respMess.Content = new StringContent("Found multiple Contacts with the same first-/lastname and email");
                                        //    return respMess;
                                        //}
                                    }
                                }
                                //if (socialSecurityConflict != null)
                                //{
                                //    conflictContacts.Remove(socialSecurityConflict);
                                //    newContact = socialSecurityConflict;
                                //    UpdateContactWithAuthorityLead(ref newContact, lead);
                                //
                                //    SetStateRequest req = new SetStateRequest()
                                //    {
                                //        EntityMoniker = lead.ToEntityReference(),
                                //        State = new OptionSetValue((int)Generated.LeadState.Disqualified),
                                //        Status = new OptionSetValue((int)Generated.lead_statuscode.Canceled)
                                //    };
                                //    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                                //
                                //    newContact.EMailAddress1 = lead.EMailAddress1;
                                //    newContact.EMailAddress2 = null;
                                //}
                                // If there's a match on existing Business Contact (not Private Customer) based on EMailAddress1
                                //else

                                //if (socialSecurityConflict == null && emailAddress1Conflict != null && conflictContacts.First().ed_PrivateCustomerContact == false)
                                if (emailAddress1Conflict != null)
                                {
                                    conflictContacts.Remove(emailAddress1Conflict);
                                    newContact = emailAddress1Conflict;
                                    UpdateContactWithAuthorityLead(ref newContact, lead);

                                    SetStateRequest req = new SetStateRequest()
                                    {
                                        EntityMoniker = lead.ToEntityReference(),
                                        State = new OptionSetValue((int)Generated.LeadState.Disqualified),
                                        Status = new OptionSetValue((int)Generated.lead_statuscode.Canceled)
                                    };
                                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                                }
                                else
                                {
                                    //newContact = new ContactEntity(lead.ToCustomerInfo());
                                    //newContact.Id = localContext.OrganizationService.Create(newContact);
                                    _log.DebugFormat($"Th={threadId} - No SocialSecurity conflict was found. Qualifying Lead.");
                                    newContact = QualifyLeadToContact(localContext, lead);
                                }
                                _log.DebugFormat($"Th={threadId} - Combining {conflictContacts.Count} conflicting contacts.");
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
                                //Creating New Contact
                                _log.DebugFormat($"Th={threadId} - No valid existing contact found, creating new from the Lead.");
                                newContact = QualifyLeadToContact(localContext, lead);
                            }

                            if (newContact == null)
                            {
                                //return new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                                //                Content = new StringContent(Resources.UnexpectedErrorWhenValidatingEmail)
                                //            };

                                HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                respMess.Content = new StringContent(Resources.UnexpectedErrorWhenValidatingEmail);
                                return respMess;
                            }

                            // Object used for update.
                            ContactEntity updContact2 = new ContactEntity()
                            {
                                ContactId = newContact.ContactId
                            };
                            AttributeCollection attrColl = newContact.Attributes;

                            if (conflictLeads.Count > 0)
                            {
                                _log.DebugFormat($"Th={threadId} - Merging {conflictLeads.Count} Leads with the new Contact");
                                foreach (LeadEntity leadConflict in conflictLeads)
                                {
                                    ContactEntity.UpdateContactWithLead(ref newContact, ref updContact2, leadConflict);
                                }
                            }

                            if (newContact.DoNotEMail == true)
                            {
                                newContact.DoNotEMail = false;
                                updContact2.DoNotEMail = false;
                            }

                            newContact.ed_MklId = mklId;
                            newContact.ed_PrivateCustomerContact = true;
                            updContact2.ed_MklId = mklId;
                            updContact2.ed_PrivateCustomerContact = true;

                            localContext.OrganizationService.Update(updContact2);

                            CrmPlusUtility.SendConfirmationEmail(localContext, threadId, newContact);

                            HttpResponseMessage rm4 = new HttpResponseMessage(HttpStatusCode.OK);
                            rm4.Content = new StringContent(SerializeNoNull(newContact.ToContactInfo(localContext)));
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
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage RegisterBuyAndSendSkaKortPost(int threadId, SkaKortInfo skaKortInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    //TODO: Create Validate Register skaKortInfo method -> Create skaKortUtility. Might not need to validate ContactId / PortalId
                    StatusBlock validateStatus = SkaKortUtility.ValidateSkaKortInfo(localContext, skaKortInfo, true);
                    if (!validateStatus.TransactionOk)
                    {
                        _log.Error($"SkaKort-POST. Validation failed.");
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    //Get Contact
                    FilterExpression contactFilter = new FilterExpression(LogicalOperator.And);
                    contactFilter.AddCondition(ContactEntity.Fields.Id, ConditionOperator.Equal, skaKortInfo.ContactId);
                    ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false), contactFilter);

                    if (contact != null)
                    {
                        FilterExpression skaKortFilter = new FilterExpression(LogicalOperator.And);
                        skaKortFilter.AddCondition(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, skaKortInfo.CardNumber);
                        SkaKortEntity skakort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, new ColumnSet(
                            SkaKortEntity.Fields.Id,
                            SkaKortEntity.Fields.ed_Contact,
                            SkaKortEntity.Fields.ed_Account,
                            SkaKortEntity.Fields.ed_CardNumber,
                            SkaKortEntity.Fields.ed_name), skaKortFilter);

                        if (skakort != null) //Vi ska inte hitta något Resekort med Contact
                        {
                            // If SKÅ Kort is already registered to a Contact, return Bad Request
                            if (skakort.ed_Contact != null)
                            {
                                _log.Error($"SkaKort-POST. SkaKort found with existing Account/Contact. CardNumber: {skaKortInfo.CardNumber}.");
                                HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                error.Content = new StringContent($"SkaKort with CardNumber {skaKortInfo.CardNumber} already exists with Account/Contact.");
                                return error;
                            }
                            else
                            {
                                _log.Debug($"Updating SkaKort with CardNumber {skaKortInfo.CardNumber}.");

                                SkaKortEntity updateSkaKort = new SkaKortEntity();
                                updateSkaKort.Id = skakort.Id;
                                updateSkaKort.ed_Contact = contact.ToEntityReference();

                                if (skakort.ed_name != skaKortInfo.CardNumber)
                                {
                                    updateSkaKort.ed_name = skaKortInfo.CardNumber;
                                }

                                if (skakort.ed_CardNumber != skaKortInfo.CardNumber)
                                {
                                    updateSkaKort.ed_CardNumber = skaKortInfo.CardNumber;
                                }

                                if (skakort.ed_InformationSource != Crm.Schema.Generated.ed_informationsource.KopOchSkicka)
                                {
                                    updateSkaKort.ed_InformationSource = Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                                }

                                XrmHelper.Update(localContext, updateSkaKort);

                                _log.Debug($"SkaKort updated. SkaKortId: {skakort.Id}.");

                                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                                resp.Content = new StringContent("SkaKort updated.");
                                return resp;
                            }
                        }
                        else //Create new
                        {
                            _log.Debug($"Creating new SkaKort with CardNumber {skaKortInfo.CardNumber}.");

                            SkaKortEntity newSkaKort = new SkaKortEntity();
                            newSkaKort.ed_name = skaKortInfo.CardNumber;
                            newSkaKort.ed_CardNumber = skaKortInfo.CardNumber;
                            newSkaKort.ed_Contact = contact.ToEntityReference();
                            newSkaKort.ed_InformationSource = Crm.Schema.Generated.ed_informationsource.KopOchSkicka;

                            newSkaKort.Id = XrmHelper.Create(localContext, newSkaKort);

                            _log.Debug($"New SkaKort created. SkaKortId: {newSkaKort.Id}.");

                            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                            resp.Content = new StringContent("SkaKort created.");
                            return resp;
                        }
                    }
                    else
                    {
                        _log.Error($"SkaKort-POST. No Account found with passed AccountNumber {skaKortInfo.PortalId}.");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"Account with AccountNumber {skaKortInfo.PortalId} does not exist.");
                        return error;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage RegisterCompanySkaKortPost(int threadId, SkaKortInfo skaKortInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    //TODO: Create Validate Register skaKortInfo method -> Create skaKortUtility. Might not need to validate ContactId / PortalId
                    StatusBlock validateStatus = SkaKortUtility.ValidateSkaKortInfo(localContext, skaKortInfo, true);
                    if (!validateStatus.TransactionOk)
                    {
                        _log.Error($"SkaKort-POST. Validation failed.");
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    //Get Account
                    FilterExpression accountFilter = new FilterExpression(LogicalOperator.And);
                    accountFilter.AddCondition(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, skaKortInfo.PortalId);
                    AccountEntity account = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(false), accountFilter);

                    if (account != null)
                    {
                        //TODO: Validate if SkaKort already exists
                        FilterExpression skaKortFilter = new FilterExpression(LogicalOperator.And);
                        skaKortFilter.AddCondition(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, skaKortInfo.CardNumber);

                        SkaKortEntity skakort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, new ColumnSet(
                            SkaKortEntity.Fields.Id,
                            SkaKortEntity.Fields.ed_Contact,
                            SkaKortEntity.Fields.ed_Account,
                            SkaKortEntity.Fields.ed_name,
                            SkaKortEntity.Fields.ed_CardNumber), skaKortFilter);

                        if (skakort != null)
                        {
                            // If SKÅ Kort is already registered to an Account, return Bad Request
                            if (skakort.ed_Account != null)
                            {
                                _log.Error($"SkaKort-POST. SkaKort found with existing Account/Contact. CardNumber: {skaKortInfo.CardNumber}.");
                                HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                error.Content = new StringContent($"SkaKort with CardNumber {skaKortInfo.CardNumber} already exists with Account/Contact.");
                                return error;
                            }
                            else
                            {
                                _log.Debug($"Updating SkaKort with CardNumber {skaKortInfo.CardNumber}.");

                                //TODO: Update SkaKort (Vad gör vi om kortet är inaktiv?)
                                SkaKortEntity updateSkaKort = new SkaKortEntity();
                                updateSkaKort.Id = skakort.Id;
                                updateSkaKort.ed_Account = account.ToEntityReference();

                                if (skakort.ed_name != skaKortInfo.CardNumber)
                                {
                                    updateSkaKort.ed_name = skaKortInfo.CardNumber;
                                }

                                if (skakort.ed_CardNumber != skaKortInfo.CardNumber)
                                {
                                    updateSkaKort.ed_CardNumber = skaKortInfo.CardNumber;
                                }

                                if (skakort.ed_InformationSource != Crm.Schema.Generated.ed_informationsource.ForetagsPortal)
                                {
                                    updateSkaKort.ed_InformationSource = Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
                                }

                                XrmHelper.Update(localContext, updateSkaKort);

                                _log.Debug($"SkaKort updated. SkaKortId: {skakort.Id}.");

                                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                                resp.Content = new StringContent("SkaKort updated.");
                                return resp;
                            }
                        }
                        else
                        {
                            _log.Debug($"Creating new SkaKort with CardNumber {skaKortInfo.CardNumber}.");

                            //TODO: Create SkaKort
                            SkaKortEntity newSkaKort = new SkaKortEntity();
                            newSkaKort.ed_name = skaKortInfo.CardNumber;
                            newSkaKort.ed_CardNumber = skaKortInfo.CardNumber;
                            newSkaKort.ed_Account = account.ToEntityReference();
                            newSkaKort.ed_InformationSource = Crm.Schema.Generated.ed_informationsource.ForetagsPortal;

                            newSkaKort.Id = XrmHelper.Create(localContext, newSkaKort);

                            _log.Debug($"New SkaKort created. SkaKortId: {newSkaKort.Id}.");

                            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                            resp.Content = new StringContent("SkaKort created.");
                            return resp;
                        }

                    }
                    else
                    {
                        _log.Error($"SkaKort-POST. No Account found with AccountNumber {skaKortInfo.PortalId}.");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"Account with AccountNumber {skaKortInfo.PortalId} does not exist.");
                        return error;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage RemoveSkaKortContactOrAccount(int threadId, SkaKortInfo skaKortInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    //TODO: Create Validate Register skaKortInfo method -> Create skaKortUtility. Might not need to validate ContactId / PortalId
                    StatusBlock validateStatus = SkaKortUtility.ValidateSkaKortInfo(localContext, skaKortInfo, false);
                    if (!validateStatus.TransactionOk)
                    {
                        _log.Error($"SkaKort-POST. Validation failed.");
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    //TODO: Get relevant SkaKort
                    SkaKortEntity skakort = null;
                    FilterExpression skaKortFilter = new FilterExpression(LogicalOperator.And);
                    skaKortFilter.AddCondition(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, skaKortInfo.CardNumber);
                    skakort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, new ColumnSet(
                        SkaKortEntity.Fields.Id,
                        SkaKortEntity.Fields.statuscode,
                        SkaKortEntity.Fields.statecode,
                        SkaKortEntity.Fields.ed_Contact,
                        SkaKortEntity.Fields.ed_Account), skaKortFilter);

                    if (skakort == null)
                    {
                        _log.Error($"SkaKort-PUT. Active SkaKort with CardNumber {skaKortInfo.CardNumber} was not found.");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"Active SkaKort with CardNumber {skaKortInfo.CardNumber} does not exist.");
                        return error;
                    }
                    else
                    {
                        if (skakort.statecode != Generated.ed_SKAkortState.Inactive)
                        {
                            SkaKortEntity updateSkakort = new SkaKortEntity();
                            updateSkakort.Id = skakort.Id;

                            if (skaKortInfo.InformationSource == (int)Generated.ed_informationsource.ForetagsPortal &&
                                skakort.ed_Account != null)
                            {
                                updateSkakort.ed_Account = null;
                            }
                            else if (skaKortInfo.InformationSource == (int)Generated.ed_informationsource.KopOchSkicka &&
                                skakort.ed_Contact != null)
                            {
                                updateSkakort.ed_Contact = null;
                            }

                            XrmHelper.Update(localContext, updateSkakort);

                            _log.Debug($"SkaKort updated. SkaKortId: {skakort.Id}.");

                            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                            resp.Content = new StringContent("SkaKort updated.");
                            return resp;
                        }
                        else
                        {
                            _log.Error($"SkaKort-PUT. SkaKort with CardNumber {skaKortInfo.CardNumber} is not an active SkaKort.");
                            HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            error.Content = new StringContent($"SkaKort with CardNumber {skaKortInfo.CardNumber} is not an active SkaKort.");
                            return error;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        internal static HttpResponseMessage SkaKortInactivate(int threadId, SkaKortInfo skaKortInfo)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    //TODO: Create Validate Register skaKortInfo method -> Create skaKortUtility. Might not need to validate ContactId / PortalId
                    StatusBlock validateStatus = SkaKortUtility.ValidateSkaKortInfo(localContext, skaKortInfo, false);
                    if (!validateStatus.TransactionOk)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(validateStatus.ErrorMessage);
                        return rm;
                    }

                    //TODO: Get relevant SkaKort
                    SkaKortEntity skakort = null;
                    FilterExpression skaKortFilter = new FilterExpression(LogicalOperator.And);
                    skaKortFilter.AddCondition(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, skaKortInfo.CardNumber);
                    skakort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, new ColumnSet(SkaKortEntity.Fields.Id, SkaKortEntity.Fields.ed_CardNumber, SkaKortEntity.Fields.statecode, SkaKortEntity.Fields.statuscode), skaKortFilter);

                    if (skakort == null)
                    {
                        _log.Error($"SkaKort-Disconnect. SkaKort with CardNumber {skaKortInfo.CardNumber} was not found.");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"SkaKort with CardNumber {skaKortInfo.CardNumber} does not exist.");
                        return error;
                    }

                    if (skakort.statecode != Generated.ed_SKAkortState.Inactive)
                    {
                        SkaKortEntity updateSkakort = new SkaKortEntity();
                        updateSkakort.Id = skakort.Id;
                        updateSkakort.statecode = Generated.ed_SKAkortState.Inactive;

                        XrmHelper.Update(localContext, updateSkakort);

                        _log.Debug($"SkaKort Disconnected. SkaKortId: {skakort.Id}.");

                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = new StringContent("SkaKort Disconnected.");
                        return resp;
                    }
                    else
                    {
                        _log.Error($"SkaKort-Disconnect. SkaKort with CardNumber {skaKortInfo.CardNumber} is already disconnected.");
                        HttpResponseMessage error = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        error.Content = new StringContent($"SkaKort with CardNumber {skaKortInfo.CardNumber} is already disconnected.");
                        return error;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        //Use this method to return the Guid for the Euro currency
        public static String RetrieveSEKCurrencyId(IOrganizationService service)
        {
            String result = null;

            QueryExpression query = new QueryExpression();
            query.EntityName = "transactioncurrency";
            query.ColumnSet = new ColumnSet(new string[] { "transactioncurrencyid", "currencyname" });
            query.Criteria = new FilterExpression();

            EntityCollection currencies = service.RetrieveMultiple(query);

            foreach (Entity e in currencies.Entities)
            {
                if (e.Attributes.Contains("currencyname"))
                {
                    if (e.Attributes["currencyname"].ToString() == "SEK")
                        result = e.Attributes["transactioncurrencyid"].ToString();
                }
            }

            return result;
        }

        private static QueryExpression CreateContactConflictQuery(LeadEntity lead)
        {
            #region make Contact Conflict Query
            FilterExpression contactConflictFilter = new FilterExpression(LogicalOperator.Or);
            if (lead.ed_HasSwedishSocialSecurityNumber.HasValue && lead.ed_HasSwedishSocialSecurityNumber.Value)
            {
                // 2020-02-19 - Marcus Stenswed
                // Do not check for conflicting Social Security Number
                /*FilterExpression SocSecFilter = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, lead.ed_Personnummer)
                                }
                };
                contactConflictFilter.AddFilter(SocSecFilter);*/
            }
            FilterExpression phoneEmailFilter = new FilterExpression(LogicalOperator.And)
            {
                Conditions =
                {
                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, lead.EMailAddress1),
                    //new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, lead.MobilePhone),
                    new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                },
                //Filters =
                //            {
                //                new FilterExpression(LogicalOperator.Or)
                //                {
                //                    Filters =
                //                    {
                //                        new FilterExpression(LogicalOperator.And)
                //                        {
                //                            Conditions =
                //                            {
                //                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                //                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
                //                            }
                //                        },
                //                        new FilterExpression(LogicalOperator.And)
                //                        {
                //                            Conditions =
                //                            {
                //                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                //                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                //                            }
                //                        }
                //                    }
                //                }
                //            }
            };
            contactConflictFilter.AddFilter(phoneEmailFilter);

            ColumnSet contactConflictColumnSet = ContactEntity.ContactInfoBlock;
            contactConflictColumnSet.AddColumn(ContactEntity.Fields.DoNotEMail);

            QueryExpression contactConflictQuery = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = contactConflictColumnSet,
                Criteria = contactConflictFilter
            };
            #endregion
            return contactConflictQuery;
        }

        private static QueryExpression CreateContactEMailAddress1ConflictQuery(LeadEntity lead)
        {
            #region make Contact Conflict Query

            FilterExpression contactConflictFilter = new FilterExpression(LogicalOperator.Or);

            FilterExpression filterWithName = new FilterExpression();

            //if (!String.IsNullOrEmpty(lead.FirstName) && !String.IsNullOrEmpty(lead.LastName))
            //{
            //    filterWithName = new FilterExpression(LogicalOperator.And)
            //    {
            //        Conditions =
            //        {
            //            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
            //            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
            //        }
            //    };
            //}

            FilterExpression nameMailFilter = new FilterExpression(LogicalOperator.And)
            {
                Conditions =
                {
                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, lead.EMailAddress1),
                    //new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, lead.MobilePhone),
                    new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true)
                },
                //Filters =
                //{
                //    new FilterExpression(LogicalOperator.Or)
                //    {
                //        Filters =
                //        {
                //            // TODO - Marcus Stenswed
                //            // Does it work setting new FilterExpression??
                //            //(filterWithName.Conditions != null && filterWithName.Conditions.Count > 0) ? filterWithName : new FilterExpression(),

                //            //new FilterExpression(LogicalOperator.And)
                //            //{
                //            //    Conditions =
                //            //    {
                //            //        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                //            //        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
                //            //    }
                //            //},

                //            new FilterExpression(LogicalOperator.And)
                //            {
                //                Conditions =
                //                {
                //                    new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                //                    new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                //                }
                //            }
                //        }
                //    }
                //}
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

            return contactConflictQuery;
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

        //public static HttpResponseMessage CreateValueCodeCRM(int threadId, ValueCodeResponseInfo valueCodeInfo)
        //{
        //    try
        //    {
        //        Guid couponGuid = Guid.Empty;

        //        CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
        //        _log.DebugFormat($"Th={threadId} - Creating serviceProxy");



        //        // Cast the proxy client to the IOrganizationService interface.
        //        using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
        //        {
        //            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

        //            if (localContext.OrganizationService == null)
        //                throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

        //            ValueCodeEntity valueCode = new ValueCodeEntity();
        //            valueCode.ed_EanCode = valueCodeInfo.eanCode.ToString();
        //            valueCode.ed_LastRedemptionDate = valueCodeInfo.lastRedemptionDate;
        //            valueCode.ed_LastRedemptionDateSpecified = valueCodeInfo.lastRedemptionDateSpecified;
        //            valueCode.ed_UniqueCode = valueCodeInfo.uniqueCode;

        //            switch (valueCodeInfo.typeOfValueCode)
        //            {
        //                case 1:
        //                    valueCode.ed_ValueCodeType = Generated.ed_valuecode_ed_valuecodetype.UniqueCode;
        //                    break;
        //                case 2:
        //                    valueCode.ed_ValueCodeType = Generated.ed_valuecode_ed_valuecodetype.UniqueCodeean;
        //                    break;
        //                case 3:
        //                    valueCode.ed_ValueCodeType = Generated.ed_valuecode_ed_valuecodetype.MobileCode;
        //                    break;
        //            }

        //            XrmHelper.Create(localContext.OrganizationService, valueCode);

        //        }


        //        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
        //        resp.Content = new StringContent(couponGuid.ToString());
        //        return resp;
        //    }
        //    catch(Exception ex)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //        rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //        return rm;
        //    }
        //    finally
        //    {
        //        ConnectionCacheManager.ReleaseConnection(threadId);
        //    }
        //}

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

        public class GuidsPlaceholder
        {
            public string LinkId { get; set; }

            public string CrmGuid { get; set; }
        }
    }
}
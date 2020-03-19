using CGIXrmWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CGIXrmGetOrders.Classes;
using Microsoft.Xrm.Sdk;

namespace CGIXrmGetOrders
{
    class GetOrdersServiceManager
    {
        #region Declarations ------------------------------------------------------------------------------------------

        private readonly object _lockSql = new object();
        private readonly XrmManager _crmmanager;

        #endregion

        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public GetOrdersServiceManager()
        {
            _crmmanager = _initCrmManager();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="countactid"></param>
        /// <returns></returns>
        public SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid)
        {
            SavedCreditOrderRowsResponse response = new SavedCreditOrderRowsResponse();

            try
            {
                if (string.IsNullOrEmpty(accountid) == false || string.IsNullOrEmpty(countactid) == false)
                {
                    var sqlCon = OpenSql();
                    OrderSqlList orderlist;

                    var accountSearch = !string.IsNullOrEmpty(accountid);

                    if (accountSearch)
                    {
                        using (var command = new SqlCommand("sp_GetCreditOrderRowsForAccount", sqlCon))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter
                            {
                                ParameterName = "@customerid",
                                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                                SqlValue = new Guid(accountid)
                            });

                            var reader = command.ExecuteXmlReader();
                            {
                                var ser = new XmlSerializer(typeof(OrderSqlList));
                                orderlist = ser.Deserialize(reader) as OrderSqlList;
                            }
                            reader.Close();
                            CloseSql(sqlCon);

                            if (orderlist?.CreditOrderRows != null && orderlist.CreditOrderRows.Any())
                            {
                                response.OrderList = new List<CreditOrderRow>();
                                response.OrderList = orderlist.CreditOrderRows;
                                response.ErrorMessage = "";
                            }
                        }
                    }
                    else
                    {
                        using (var command = new SqlCommand("sp_GetCreditOrderRowsForContact", sqlCon))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter
                            {
                                ParameterName = "@customerid",
                                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                                SqlValue = new Guid(countactid)
                            });

                            var reader = command.ExecuteXmlReader();
                            {
                                var ser = new XmlSerializer(typeof(OrderSqlList));
                                orderlist = ser.Deserialize(reader) as OrderSqlList;
                            }
                            reader.Close();
                            CloseSql(sqlCon);

                            if (orderlist?.CreditOrderRows != null && orderlist.CreditOrderRows.Any())
                            {
                                response.OrderList = new List<CreditOrderRow>();
                                response.OrderList = orderlist.CreditOrderRows;
                                response.ErrorMessage = "";
                            }
                        }
                    }
                }
                else
                {
                    // Return empty Data Transfer Object as accountid and contactid was empty so no db search was perfomed.
                    response.OrderList = new List<CreditOrderRow>();
                    response.ErrorMessage = "";

                    return response;
                }
            }
            catch (FaultException faultex)
            {
                response.OrderList = null;
                response.ErrorMessage = "CGIXrmGetOrders.GetSavedCreditOrderRows " + faultex.Message;
            }
            catch (Exception ex)
            {
                response.OrderList = null;
                response.ErrorMessage = "CGIXrmGetOrders.GetSavedCreditOrderRows " + ex.Message;
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetCreditOrderResponse CreditOrder(CreditOrderRequest request)
        {
            var response = new GetCreditOrderResponse
            {
                CreditOrderMessage = new ObservableCollection<CreditOrderMessage>()
            };

            try
            {
                var soapActionAddress = ConfigurationManager.AppSettings["CreditSoapActionAddress"];
                var orderServiveAddress = ConfigurationManager.AppSettings["CreditServiveAddress"];
                var ehandeladdressOrders = ConfigurationManager.AppSettings["Creditehandeladdress"];

                var message = new CreditOrderMessage();

                foreach (var row in request.CreditRows)
                {
                    var data = CreditXMLRequest(row.OrderNumber, row.Sum, row.ProductNumber, row.Quantity,
                        ehandeladdressOrders);
                    string xmlresponse;

                    using (var client = new WebClient())
                    {
                        // the Content-Type needs to be set to XML
                        client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                        client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                        client.Encoding = Encoding.UTF8;
                        xmlresponse = client.UploadString("" + orderServiveAddress + "", data);
                    }


                    if (string.IsNullOrEmpty(xmlresponse)) continue;

                    var error = false;
                    var xdoc = XDocument.Parse(xmlresponse);

                    var faults = _getNodesListFromXML(xdoc, "Fault");

                    foreach (var fault in faults)
                    {
                        GetValueFromXML(fault, "faultcode");

                        var faultstring = GetValueFromXML(fault, "faultstring");

                        message.ErrorMessage = faultstring;
                        error = true;
                    }

                    if (error) continue;

                    var bodys = _getNodesListFromXML(xdoc, "Body");
                    foreach (var body in bodys)
                    {
                        var ordernumber = GetValueFromXML(body, "OrderNumber");
                        var sum = GetValueFromXML(body, "Sum");
                        var referencenumber = GetValueFromXML(body, "ReferenceNumber");
                        var success = GetValueFromXML(body, "Success");
                        var mess = GetValueFromXML(body, "Message");
                        var date = GetValueFromXML(body, "Date");

                        message.OrderNumber = ordernumber;
                        message.Sum = sum;
                        message.ReferenceNumber = referencenumber;
                        message.Success = success;
                        message.Message = mess;

                        if (!string.IsNullOrEmpty(date))
                            message.Date = _convertXMLDateTimeToDate(date);

                        if (!string.IsNullOrEmpty(date))
                            message.Time = _convertXMLDateTimeToTime(date);

                    }

                    message.ProductNumber = row.ProductNumber;

                    if (!string.IsNullOrEmpty(message.Success))
                    {
                        if (message.Success.ToUpper() == "TRUE")
                        {
                            //save credit orderrow to crm.
                            _saveCreditOrderRow(message, row);
                            message.CRMMessage = "OK";
                        }
                        else
                        {
                            message.CRMMessage = "ERROR";
                        }
                    }
                    else
                    {
                        message.CRMMessage = "ERROR";
                    }

                    response.CreditOrderMessage.Add(message);
                }
            }
            catch (Exception ex)
            {
                response.CreditOrderMessage.Add(new CreditOrderMessage
                {
                    CRMMessage = "ERROR",
                    Message = string.Empty,
                    ErrorMessage = ex.Message
                });
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderNumber"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public GetOrderResponse GetOrders(string customerId, string orderNumber, string travelCardNumber, string from, string to, string email)
        {
            var response = new GetOrderResponse();

            try
            {
                response.ErrorMessage = string.Empty;
                response.Orders = new ObservableCollection<OrderHeader>();

                var soapActionAddress = ConfigurationManager.AppSettings["OrderSoapActionAddress"];
                var orderServiveAddress = ConfigurationManager.AppSettings["OrderServiveAddress"];
                var ehandeladdressOrders = ConfigurationManager.AppSettings["Orderehandeladdress"];

                var data = _orderXMLRequest(customerId, orderNumber, travelCardNumber, from, to, email, ehandeladdressOrders);
                string xmlresponse;

                using (var client = new webclientx())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Timeout = 120000;
                    client.Encoding = Encoding.UTF8;
                    xmlresponse = client.UploadString("" + orderServiveAddress + "", data);
                }

                if (!string.IsNullOrEmpty(xmlresponse))
                {
                    var xdoc = XDocument.Parse(xmlresponse);
                    var ordersElement = _getNodesListFromXML(xdoc, "Orders");

                    foreach (var orderElement in ordersElement)
                    {
                        var header = new OrderHeader();
                        var orderConfirmationEmail = GetValueFromXML(orderElement, "OrderConfirmationEmail");
                        var ordernumber = GetValueFromXML(orderElement, "OrderNumber");
                        var orderstatus = GetValueFromXML(orderElement, "OrderStatus");
                        var ordertype = GetValueFromXML(orderElement, "OrderType");
                        var orderdate = GetValueFromXML(orderElement, "OrderDate");
                        var ordertotal = GetValueFromXML(orderElement, "OrderTotal");
                        var ordertotalvat = GetValueFromXML(orderElement, "OrderTotalVat");

                        header.OrderNumber = ordernumber;
                        header.OrderStatus = orderstatus;
                        header.OrderType = ordertype;

                        if (!string.IsNullOrEmpty(orderdate))
                            header.OrderDate = _convertXMLDateTime(orderdate);

                        if (!string.IsNullOrEmpty(ordertotal))
                            header.OrderTotal = _convertXMLDecimal(ordertotal);

                        if (!string.IsNullOrEmpty(ordertotalvat))
                            header.OrderTotalVat = _convertXMLDecimal(ordertotalvat);

                        //Payments
                        header.Payments = new ObservableCollection<Payment>();
                        var payments = _getNodesFromXML(orderElement, "Payments");

                        foreach (var payment in payments)
                        {
                            var code = GetValueFromXML(payment, "PaymentMethodCode"); // 150919 earlier Code, no change in DTO name below so far, see Payment
                            var name = GetValueFromXML(payment, "PaymentMethodName"); // 150919 earlier Name, no change in DTO name below so far, see Payment
                            var sum = GetValueFromXML(payment, "Sum");
                            var referencenumber = GetValueFromXML(payment, "ReferenceNumber");
                            var transactionType = GetValueFromXML(payment, "TransactionType");
                            var giftCardCode = GetValueFromXML(payment, "GiftCardCode");
                            var transactionId = GetValueFromXML(payment, "TransactionId");

                            var pay = new Payment
                            {
                                Code = code,
                                Name = name,
                                TransactionType = transactionType,
                                GiftCardCode = giftCardCode,
                                TransactionId = transactionId
                            };

                            if (!string.IsNullOrEmpty(sum))
                                pay.Sum = _convertXMLDecimal(sum);

                            pay.ReferenceNumber = referencenumber;

                            header.Payments.Add(pay);
                        }

                        var customerParentElement = _getNodesFromXML(orderElement, "Customer");

                        foreach (var customerChildElement in customerParentElement)
                        {
                            var emailAddress = GetValueFromXML(customerChildElement, "Email");
                            var accountNumber = GetValueFromXML(customerChildElement, "AccountNumber");
                            var accountNumber2 = GetValueFromXML(customerChildElement, "AccountNumber2");
                            var extraInfo = GetValueFromXML(customerChildElement, "ExtraInfo");
                            var customerName = GetValueFromXML(customerChildElement, "CustomerName");

                            var customer = new Customer();

                            if (string.IsNullOrEmpty(accountNumber) && string.IsNullOrEmpty(customerName))
                            {
                                if (string.IsNullOrEmpty(orderConfirmationEmail) == false)
                                {
                                    // Från E-commerce: Om det är företagsordrar så kommer alltid kunden att 
                                    //finnas i svaret då företagskunder måste vara inloggade för att köpa.
                                    // Så får ni en ”tom” kund kan ni vara säkra på att denna är en privatkund. 
                                    PopulateCustomerFromCrm(orderConfirmationEmail, customer);
                                }
                            }
                            else
                            {
                                customer.Email = emailAddress;
                                customer.AccountNumber = accountNumber;
                                customer.AccountNumber2 = accountNumber2;
                                customer.ExtraInfo = extraInfo;
                                customer.Name = customerName;

                                // TODO: this proberbly still returns false for null/empty string. 
                                // We might get false from biztalk also on null/empty string. 
                                bool isCompany;
                                if (bool.TryParse(GetValueFromXML(customerChildElement, "IsCompany"), out isCompany))
                                {
                                    customer.IsCompany = isCompany; // INFO: might want to use a regular bool instead of nullable bool
                                }

                            }

                            header.Customer = customer;
                        }

                        //ShippingAddress
                        header.ShippingAddress = new ObservableCollection<ShippingAddress>();
                        var shippingAddressParentElements = _getNodesFromXML(orderElement, "ShippingAddress");

                        foreach (var sa in from shippingAddressChildElement in shippingAddressParentElements
                                           let shipmentAdress = GetValueFromXML(shippingAddressChildElement, "Address")
                                           let city = GetValueFromXML(shippingAddressChildElement, "City")
                                           let co = GetValueFromXML(shippingAddressChildElement, "Co")
                                           let companyName = GetValueFromXML(shippingAddressChildElement, "CompanyName")
                                           let country = GetValueFromXML(shippingAddressChildElement, "Country")
                                           let cellPhoneNumber = GetValueFromXML(shippingAddressChildElement, "CellPhoneNumber")
                                           let emailAdress = GetValueFromXML(shippingAddressChildElement, "Email")
                                           let firstName = GetValueFromXML(shippingAddressChildElement, "FirstName")
                                           let lastName = GetValueFromXML(shippingAddressChildElement, "LastName")
                                           let postalCode = GetValueFromXML(shippingAddressChildElement, "PostalCode")
                                           let extraInfo = GetValueFromXML(shippingAddressChildElement, "ExtraInfo")
                                           select new ShippingAddress
                                           {
                                               Address = shipmentAdress,
                                               City = city,
                                               Co = co,
                                               CompanyName = companyName,
                                               Country = country,
                                               CellPhoneNumber = cellPhoneNumber,
                                               Email = emailAdress,
                                               FirstName = firstName,
                                               LastName = lastName,
                                               PostalCode = postalCode,
                                               ExtraInfo = extraInfo
                                           })
                        {
                            header.ShippingAddress.Add(sa);
                        }

                        header.OrderItems = new ObservableCollection<OrderRow>();
                        var orderItemParentElement = _getNodesFromXML(orderElement, "OrderItems");

                        foreach (var orderItemChildElement in orderItemParentElement)
                        {
                            GetValueFromXML(orderItemChildElement, "Code");

                            var name = GetValueFromXML(orderItemChildElement, "Name");
                            var quantity = GetValueFromXML(orderItemChildElement, "Quantity");
                            var price = GetValueFromXML(orderItemChildElement, "Price");
                            var discount = GetValueFromXML(orderItemChildElement, "Discount");
                            var cardNumber = GetValueFromXML(orderItemChildElement, "CardNumber");

                            var ordrow = new OrderRow
                            {
                                Code = name //code
                            };

                            if (string.IsNullOrEmpty(cardNumber) == false)
                            {
                                ordrow.Name = "Kortnummer: " + cardNumber;//name;
                            }

                            if (!string.IsNullOrEmpty(quantity))
                                ordrow.Quantity = _convertXMLByte(quantity);

                            if (!string.IsNullOrEmpty(price))
                                ordrow.Price = _convertXMLDecimal(price);

                            if (!string.IsNullOrEmpty(discount))
                                ordrow.Discount = _convertXMLDecimal(discount);

                            header.OrderItems.Add(ordrow);
                        }

                        var couponParentElement = _getNodesFromXML(orderElement, "Coupons");

                        foreach (XElement couponChildElement in couponParentElement)
                        {
                            var concatenatedRow = string.Empty;
                            var code = GetValueFromXML(couponChildElement, "CouponCode");

                            if (string.IsNullOrEmpty(code) == false)
                            {
                                concatenatedRow += "Kod: ";
                                concatenatedRow += code;
                            }

                            var blockedCardNumber = GetValueFromXML(couponChildElement, "BlockedCardNumber");

                            if (string.IsNullOrEmpty(blockedCardNumber) == false)
                            {
                                concatenatedRow += " Spärrat kort: ";
                                concatenatedRow += blockedCardNumber;
                            }

                            var yesOrNo = (GetValueFromXML(couponChildElement, "IsSent") == "true" ? "Ja" : "Nej");

                            if (string.IsNullOrEmpty(yesOrNo) == false)
                            {
                                concatenatedRow += " Skickad: ";
                                concatenatedRow += yesOrNo;
                            }

                            var reciever = GetValueFromXML(couponChildElement, "Receiver");

                            if (string.IsNullOrEmpty(reciever) == false)
                            {
                                concatenatedRow += " Mottagare: ";
                                concatenatedRow += reciever;
                            }

                            var name = concatenatedRow;
                            const string quantity = "1";
                            var price = GetValueFromXML(couponChildElement, "CouponSum");
                            const string discount = "0";

                            var ordrow = new OrderRow
                            {
                                Code = GetValueFromXML(couponChildElement, "ShippingMethod"),
                                Name = name
                            };

                            if (!string.IsNullOrEmpty(quantity))
                            {
                                ordrow.Quantity = _convertXMLByte(quantity);
                            }

                            if (!string.IsNullOrEmpty(price))
                            {
                                ordrow.Price = _convertXMLDecimal(price);
                            }

                            if (!string.IsNullOrEmpty(discount))
                            {
                                ordrow.Discount = _convertXMLDecimal(discount);
                            }

                            header.OrderItems.Add(ordrow);
                        }

                        response.Orders.Add(header);
                    }
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Orders = null;
            }

            return response;
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private XrmManager _initCrmManager()
        {
            try
            {
                var crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var username = ConfigurationManager.AppSettings["Username"];
                var password = ConfigurationManager.AppSettings["Password"];

                if (string.IsNullOrEmpty(crmServerUrl) || string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new Exception();
                var xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SqlConnection OpenSql()
        {
            lock (_lockSql)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                return sqlConnection;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        private void CloseSql(SqlConnection connection)
        {
            lock (_lockSql)
            {
                connection?.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="nodename"></param>
        /// <returns></returns>
        private IEnumerable<XElement> _getNodesListFromXML(XDocument xdoc, string nodename)
        {
            IEnumerable<XElement> returnValue;
            try
            {
                returnValue = xdoc.Descendants().Where(p => p.Name.LocalName == nodename);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nod"></param>
        /// <param name="nodename"></param>
        /// <returns></returns>
        private IEnumerable<XElement> _getNodesFromXML(XElement nod, string nodename)
        {
            IEnumerable<XElement> returnValue;
            try
            {
                returnValue = nod.Descendants().Where(p => p.Name.LocalName == nodename);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetValueFromXML(XContainer xmldoc, string node)
        {
            string returnValue;

            try
            {
                var value = from c in xmldoc.Descendants() where c.Name.LocalName == node select c.Value;

                returnValue = value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuestring"></param>
        /// <returns></returns>
        private DateTime _convertXMLDateTime(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuestring"></param>
        /// <returns></returns>
        private string _convertXMLDateTimeToDate(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local).ToShortDateString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuestring"></param>
        /// <returns></returns>
        private string _convertXMLDateTimeToTime(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local).ToShortTimeString();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="valuestring"></param>
        /// <returns></returns>
        private decimal _convertXMLDecimal(string valuestring)
        {
            return XmlConvert.ToDecimal(valuestring);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuestring"></param>
        /// <returns></returns>
        private byte _convertXMLByte(string valuestring)
        {
            return XmlConvert.ToByte(valuestring);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderNumber"></param>
        /// <param name="cardNumber"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="email"></param>
        /// <param name="ehandeladdressOrders"></param>
        /// <returns></returns>
        private string _orderXMLRequest(string customerId, string orderNumber, string cardNumber, string from, string to,
            string email, string ehandeladdressOrders)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            // TODO : customerId not used
            string request = "";
            request += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" +
                       ehandeladdressOrders + "'>";
            request += "    <soapenv:Header/>";
            request += "    <soapenv:Body>";
            request += "        <ns:GetOrdersRequest>";

            if (!string.IsNullOrEmpty(email))
                request += "            <Email>" + email + "</Email>";

            if (!string.IsNullOrEmpty(cardNumber))
                request += "            <CardNumber>" + cardNumber + "</CardNumber>";

            if (!string.IsNullOrEmpty(orderNumber))
                request += "            <OrderNumber>" + orderNumber + "</OrderNumber>";

            if (!string.IsNullOrEmpty(from))
                request += "            <From>" + from + "</From>";

            if (!string.IsNullOrEmpty(to))
                request += "            <To>" + to + "</To>";

            request += "        </ns:GetOrdersRequest>";
            request += "    </soapenv:Body>";
            request += "</soapenv:Envelope>";

            return request;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="sum"></param>
        /// <param name="productnumber"></param>
        /// <param name="quantity"></param>
        /// <param name="ehandeladdressOrders"></param>
        /// <returns></returns>
        private string CreditXMLRequest(string orderNumber, string sum, string productnumber, string quantity, string ehandeladdressOrders)
        {
            var request = string.Empty;
            request += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + ehandeladdressOrders + "'>";
            request += "<soapenv:Header/>";
            request += "<soapenv:Body>";
            request += "    <ns:CreditOrderRequest>";
            request += "        <orderNumber>" + orderNumber + "</orderNumber>";
            request += "        <sum>" + sum + "</sum>";
            request += "        <ProductNumber>" + productnumber + "</ProductNumber>";
            request += "        <Quantity>" + quantity + "</Quantity>";
            request += "    </ns:CreditOrderRequest>";
            request += "</soapenv:Body>";
            request += "</soapenv:Envelope>";

            return request;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="customer"></param>
        private void PopulateCustomerFromCrm(string emailAddress, Customer customer)
        {
            //query contact by filter email
            var contactQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression("contact");
            contactQuery.ColumnSet.AddColumns("fullname");

            contactQuery.Criteria = new Microsoft.Xrm.Sdk.Query.FilterExpression();
            contactQuery.Criteria.AddCondition("emailaddress1", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, emailAddress);

            var contactResults = _crmmanager.Service.RetrieveMultiple(contactQuery);

            //check if contact was found by that email
            if (contactResults?.Entities == null || contactResults.Entities.Count <= 0) return;

            customer.AccountNumber = contactResults.Entities[0].Id.ToString();
            customer.Name = contactResults.Entities[0]["fullname"] as string;
            customer.Email = emailAddress;
            customer.IsCompany = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="creditOrderRow"></param>
        private void _saveCreditOrderRow(CreditOrderMessage row, CreditRow creditOrderRow)
        {
            try
            {
                var ent = _createEntity(row, creditOrderRow);

                if (ent != null)
                {
                    _crmmanager.Create(ent);
                }
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="creditOrderMessage">Is the reply from e-handel API</param>
        /// <param name="creditRow">Is the DTO from our CreditOrder service method, incoming request</param>
        /// <returns></returns>
        private Entity _createEntity(CreditOrderMessage creditOrderMessage, CreditRow creditRow)
        {
            Entity entity;

            try
            {
                entity = new Entity
                {
                    LogicalName = "cgi_creditorderrow",
                    Attributes = new AttributeCollection()
                };

                if (!string.IsNullOrEmpty(creditOrderMessage.OrderNumber))
                {
                    entity.Attributes.Add("cgi_ordernumber", creditOrderMessage.OrderNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Sum))
                {
                    entity.Attributes.Add("cgi_sum", creditOrderMessage.Sum);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.ReferenceNumber))
                {
                    entity.Attributes.Add("cgi_referencenumber", creditOrderMessage.ReferenceNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Success))
                {
                    entity.Attributes.Add("cgi_success", creditOrderMessage.Success);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Message))
                {
                    entity.Attributes.Add("cgi_message", creditOrderMessage.Message);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Date))
                {
                    entity.Attributes.Add("cgi_date", creditOrderMessage.Date);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Time))
                {
                    entity.Attributes.Add("cgi_time", creditOrderMessage.Time);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.ProductNumber))
                {
                    entity.Attributes.Add("cgi_productnumber", creditOrderMessage.ProductNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.OrderNumber))
                {
                    entity.Attributes.Add("cgi_name", creditOrderMessage.OrderNumber);
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.Reason))
                {
                    entity.Attributes.Add("cgi_reason", creditRow.Reason);
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.AccountId))
                {
                    entity.Attributes.Add("cgi_accountid", new EntityReference("account", new Guid(creditRow.AccountId)));
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.ContactId))
                {
                    entity.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(creditRow.ContactId)));
                }

                // Info: not same class as the rest! Its creditOrderRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.CreatedBy))
                {
                    entity.Attributes.Add("cgi_createdby", creditRow.CreatedBy);
                }

            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }

            return entity;
        }

        #endregion
    }
}

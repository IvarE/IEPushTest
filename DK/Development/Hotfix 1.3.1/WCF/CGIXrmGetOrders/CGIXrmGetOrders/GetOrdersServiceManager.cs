using CGIXrmWin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.ServiceModel;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft;
using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace CGIXrmGetOrders
{

    class GetOrdersServiceManager
    {

        private object LockSql = new object();
        private XrmManager _crmmanager;

        public GetOrdersServiceManager()
        {
            _crmmanager = _initCrmManager();
        }

        private XrmManager _initCrmManager()
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

        private IEnumerable<XElement> _getNodesListFromXML(XDocument xdoc, string nodename)
        {
            IEnumerable<XElement> _returnValue = null;
            try
            {
                _returnValue = xdoc.Descendants().Where(p => p.Name.LocalName == nodename);
            }
            catch (Exception ex)
            {

            }
            return _returnValue;
        }

        private IEnumerable<XElement> _getNodesFromXML(XElement nod, string nodename)
        {
            IEnumerable<XElement> _returnValue = null;
            try
            {
                _returnValue = nod.Descendants().Where(p => p.Name.LocalName == nodename);
            }
            catch (Exception ex)
            {

            }
            return _returnValue;
        }

        private string _getValueFromXML(XElement xmldoc, string node)
        {
            string _returnValue = "";

            try
            {
                var _value = from _c in xmldoc.Descendants() where _c.Name.LocalName == node select _c.Value;
                if (_value != null)
                    _returnValue = _value.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return _returnValue;
        }

        private DateTime _convertXMLDateTime(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local);
        }

        private string _convertXMLDateTimeToDate(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local).ToShortDateString();
        }

        private string _convertXMLDateTimeToTime(string valuestring)
        {
            return XmlConvert.ToDateTime(valuestring, XmlDateTimeSerializationMode.Local).ToShortTimeString();
        }

        private decimal _convertXMLDecimal(string valuestring)
        {
            return XmlConvert.ToDecimal(valuestring);
        }

        private byte _convertXMLByte(string valuestring)
        {
            return XmlConvert.ToByte(valuestring);
        }
        /*
        internal GetAccountResponse GetAccount(string accountId)
        {
            GetAccountResponse response = new GetAccountResponse();
            try
            {
                response.ErrorMessage = string.Empty;

                Guid accountId_guid;
                if (Guid.TryParse(accountId, out accountId_guid))
                {
                    Entity account = _crmmanager.Get("account", accountId_guid, "name");
                    response.Name = (string)account["name"];
                    return response;
                }
                {
                    throw new Exception("Parameter accountId is not a valid guid");
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        internal GetContactResponse GetContact(string contactId)
        {
            GetContactResponse response = new GetContactResponse();
            try
            {
                response.ErrorMessage = string.Empty;

                Guid contactId_guid;
                if (Guid.TryParse(contactId, out contactId_guid))
                {
                    Entity contact = _crmmanager.Get("contact", contactId_guid, "fullname");
                    response.Name = (string)contact["fullname"];
                    return response;
                }
                {
                    throw new Exception("Parameter contactId is not a valid guid");
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
        */
        internal GetOrderResponse GetOrders(string customerId, string orderNumber, string travelCardNumber, string from, string to, string email)
        {
            GetOrderResponse _response = new GetOrderResponse();

            try
            {
                _response.ErrorMessage = "";
                _response.Orders = new ObservableCollection<OrderHeader>();

                string _soapActionAddress = ConfigurationManager.AppSettings["OrderSoapActionAddress"].ToString();
                string _orderServiveAddress = ConfigurationManager.AppSettings["OrderServiveAddress"].ToString();
                string _ehandeladdressOrders = ConfigurationManager.AppSettings["Orderehandeladdress"].ToString();

                string _data = _orderXMLRequest(customerId, orderNumber, travelCardNumber, from, to, email, _ehandeladdressOrders);
                string _xmlresponse = "";


                /*
                using (var client = new WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _xmlresponse = client.UploadString("" + _orderServiveAddress + "", _data);
                }
                */

                using (var _client = new webclientx())
                {
                    // the Content-Type needs to be set to XML
                    _client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    _client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                    _client.Timeout = 120000;
                    _client.Encoding = Encoding.UTF8;
                    _xmlresponse = _client.UploadString("" + _orderServiveAddress + "", _data);
                }

                if (!string.IsNullOrEmpty(_xmlresponse))
                {
                    XDocument _xdoc = XDocument.Parse(_xmlresponse);

                    IEnumerable<XElement> _orders_Element = _getNodesListFromXML(_xdoc, "Orders");
                    foreach (XElement _order_Element in _orders_Element)
                    {
                        OrderHeader _header = new OrderHeader();

                        string orderConfirmationEmail = _getValueFromXML(_order_Element, "OrderConfirmationEmail"); 

                        //<OrderNumber>PO5000106</OrderNumber> 
                        string _ordernumber = _getValueFromXML(_order_Element, "OrderNumber");
                        //<OrderStatus>Completed</OrderStatus> 
                        string _orderstatus = _getValueFromXML(_order_Element, "OrderStatus");
                        //<OrderType>Order</OrderType> 
                        string _ordertype = _getValueFromXML(_order_Element, "OrderType");
                        //<OrderDate>2014-11-14T13:13:37.163Z</OrderDate> 
                        string _orderdate = _getValueFromXML(_order_Element, "OrderDate");
                        //<OrderTotal>220</OrderTotal> 
                        string _ordertotal = _getValueFromXML(_order_Element, "OrderTotal");
                        //<OrderTotalVat>0</OrderTotalVat> 
                        string _ordertotalvat = _getValueFromXML(_order_Element, "OrderTotalVat");

                        _header.OrderNumber = _ordernumber;
                        _header.OrderStatus = _orderstatus;
                        _header.OrderType = _ordertype;
                        if (!string.IsNullOrEmpty(_orderdate))
                            _header.OrderDate = _convertXMLDateTime(_orderdate);

                        if (!string.IsNullOrEmpty(_ordertotal))
                            _header.OrderTotal = _convertXMLDecimal(_ordertotal);

                        if (!string.IsNullOrEmpty(_ordertotalvat))
                            _header.OrderTotalVat = _convertXMLDecimal(_ordertotalvat);

                        //Payments
                        _header.Payments = new ObservableCollection<Payment>();
                        IEnumerable<XElement> _payments = _getNodesFromXML(_order_Element, "Payments");
                        foreach (XElement _payment in _payments)
                        {
                            //<Code>7af5b796-0600-4b11-be36-b2acfa94e77a</Code>
                            string _code = _getValueFromXML(_payment, "PaymentMethodCode"); // 150919 earlier Code, no change in DTO name below so far, see Payment
                            //<Name>Kreditkort</Name> 
                            string _name = _getValueFromXML(_payment, "PaymentMethodName"); // 150919 earlier Name, no change in DTO name below so far, see Payment
                            //<Sum>220</Sum> 
                            string _sum = _getValueFromXML(_payment, "Sum");
                            //<ReferenceNumber>282718834</ReferenceNumber>
                            string _referencenumber = _getValueFromXML(_payment, "ReferenceNumber");

                            string transactionType = _getValueFromXML(_payment, "TransactionType");

                            string giftCardCode = _getValueFromXML(_payment, "GiftCardCode");

                            string transactionId = _getValueFromXML(_payment, "TransactionId");

                            Payment _pay = new Payment();
                            _pay.Code = _code;
                            _pay.Name = _name;
                            _pay.TransactionType = transactionType;
                            _pay.GiftCardCode = giftCardCode;
                            _pay.TransactionId = transactionId;

                            if (!string.IsNullOrEmpty(_sum))
                                _pay.Sum = _convertXMLDecimal(_sum);

                            _pay.ReferenceNumber = _referencenumber;

                            _header.Payments.Add(_pay);
                        }

                        IEnumerable<XElement> customerParentElement = _getNodesFromXML(_order_Element, "Customer");
                        foreach (XElement customerChildElement in customerParentElement)
                        {

                            /*
                                <Customer>
                                    <Email>user@user.se</Email>
                                    <AccountNumber>A273A13D-746B-454E-BC66-143A012EF5E9 (Id i SeKund om kunden är finns eller är inloggad)</AccountNumber>
                                    <AccountNumber2>ST123456 (Kundnummer i SeKund)</AccountNumber2>
                                    <IsCompany>false</IsCompany>
                                    <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
                                </Customer>
                             */

                            string emailAddress = _getValueFromXML(customerChildElement, "Email");
                            string accountNumber = _getValueFromXML(customerChildElement, "AccountNumber");//"F0BAD711-D1E4-E411-80D7-005056906AE2";
                            string accountNumber2 = _getValueFromXML(customerChildElement, "AccountNumber2");
                            bool isCompany;
                            string extraInfo = _getValueFromXML(customerChildElement, "ExtraInfo");
                            string customerName = _getValueFromXML(customerChildElement, "CustomerName");

                            Customer customer = new Customer();

                            if (string.IsNullOrEmpty(accountNumber) == true && string.IsNullOrEmpty(customerName) == true)
                            {
                                if (string.IsNullOrEmpty(orderConfirmationEmail) == false)
                                {
                                    // Från Marcus Star: Om det är företagsordrar så kommer alltid kunden att finnas i svaret då företagskunder måste vara inloggade för att köpa.
                                    // Så får ni en ”tom” kund kan ni vara säkra på att denna är en privatkund. 
                                    PopulateCustomerFromCRM(orderConfirmationEmail, customer);
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
                                if (bool.TryParse(_getValueFromXML(customerChildElement, "IsCompany"), out isCompany))
                                {
                                    customer.IsCompany = isCompany; // INFO: might want to use a regular bool instead of nullable bool
                                }

                            }

                            _header.Customer = customer;
                        }

                        //ShippingAddress
                        _header.ShippingAddress = new ObservableCollection<ShippingAddress>();
                        IEnumerable<XElement> shippingAddressParentElements = _getNodesFromXML(_order_Element, "ShippingAddress");
                        foreach (XElement shippingAddressChildElement in shippingAddressParentElements)
                        {

                            /*
                               <Address>Gata 1B</Address>
                               <City>Stad</City>
                               <Co>C/O</Co>
                               <CompanyName>Företagsnamn (om beställaren är en företagskund)</CompanyName>
                               <Country>Land</Country>
                               <CellPhoneNumber>0701234567</CellPhoneNumber>
                               <Email>E-post</Email>
                               <FirstName>Förnamn</FirstName>
                               <LastName>Efternamn</LastName>
                               <PostalCode>43125</PostalCode>
                               <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
                            */

                            string shipmentAdress = _getValueFromXML(shippingAddressChildElement, "Address");
                            string city = _getValueFromXML(shippingAddressChildElement, "City");
                            string co = _getValueFromXML(shippingAddressChildElement, "Co");
                            string companyName = _getValueFromXML(shippingAddressChildElement, "CompanyName");
                            string country = _getValueFromXML(shippingAddressChildElement, "Country");
                            string cellPhoneNumber = _getValueFromXML(shippingAddressChildElement, "CellPhoneNumber");
                            string emailAdress = _getValueFromXML(shippingAddressChildElement, "Email");
                            string firstName = _getValueFromXML(shippingAddressChildElement, "FirstName");
                            string lastName = _getValueFromXML(shippingAddressChildElement, "LastName");
                            string postalCode = _getValueFromXML(shippingAddressChildElement, "PostalCode");
                            string extraInfo = _getValueFromXML(shippingAddressChildElement, "ExtraInfo");

                            ShippingAddress sa = new ShippingAddress();
                            sa.Address = shipmentAdress;
                            sa.City = city;
                            sa.Co = co;
                            sa.CompanyName = companyName;
                            sa.Country = country;
                            sa.CellPhoneNumber = cellPhoneNumber;
                            sa.Email = emailAdress;
                            sa.FirstName = firstName;
                            sa.LastName = lastName;
                            sa.PostalCode = postalCode;
                            sa.ExtraInfo = extraInfo;

                            _header.ShippingAddress.Add(sa);
                        }

                        _header.OrderItems = new ObservableCollection<OrderRow>();
                        IEnumerable<XElement> _orderItemParentElement = _getNodesFromXML(_order_Element, "OrderItems");
                        foreach (XElement _orderItemChildElement in _orderItemParentElement)
                        {
                            //<Code>PrivateCardJojo200</Code>
                            string _code = _getValueFromXML(_orderItemChildElement, "Code");
                            //<Name>Jojo 200</Name> 
                            string _name = _getValueFromXML(_orderItemChildElement, "Name");
                            //<Quantity>1</Quantity> 
                            string _quantity = _getValueFromXML(_orderItemChildElement, "Quantity");
                            //<Price>200</Price> 
                            string _price = _getValueFromXML(_orderItemChildElement, "Price");
                            //<Discount>0</Discount> 
                            string _discount = _getValueFromXML(_orderItemChildElement, "Discount");

                            string cardNumber = _getValueFromXML(_orderItemChildElement, "CardNumber");

                            OrderRow _ordrow = new OrderRow();
                            _ordrow.Code = _name;//_code;

                            if (string.IsNullOrEmpty(cardNumber) == false)
                            {
                                _ordrow.Name = "Kortnummer: " + cardNumber;//_name;
                            }

                            if (!string.IsNullOrEmpty(_quantity))
                                _ordrow.Quantity = _convertXMLByte(_quantity);

                            if (!string.IsNullOrEmpty(_price))
                                _ordrow.Price = _convertXMLDecimal(_price);

                            if (!string.IsNullOrEmpty(_discount))
                                _ordrow.Discount = _convertXMLDecimal(_discount);

                            _header.OrderItems.Add(_ordrow);
                        }

                        IEnumerable<XElement> _couponParentElement = _getNodesFromXML(_order_Element, "Coupons");
                        foreach (XElement _couponChildElement in _couponParentElement)
                        {
                            string concatenatedRow = string.Empty;
                            string code = _getValueFromXML(_couponChildElement, "CouponCode");

                            // TODO ta bort tomma?
                            if (string.IsNullOrEmpty(code) == false)
                            {
                                concatenatedRow += "Kod: ";
                                concatenatedRow += code;
                            }

                            string blockedCardNumber = _getValueFromXML(_couponChildElement, "BlockedCardNumber");
                            if (string.IsNullOrEmpty(blockedCardNumber) == false)
                            {
                                concatenatedRow += " Spärrat kort: ";
                                concatenatedRow += blockedCardNumber;
                            }

                            string yesOrNo = (_getValueFromXML(_couponChildElement, "IsSent") == "true" ? "Ja" : "Nej");
                            if (string.IsNullOrEmpty(yesOrNo) == false)
                            {
                                concatenatedRow += " Skickad: ";
                                concatenatedRow += yesOrNo;
                            }

                            string reciever = _getValueFromXML(_couponChildElement, "Receiver");
                            if (string.IsNullOrEmpty(reciever) == false)
                            {
                                concatenatedRow += " Mottagare: ";
                                concatenatedRow += reciever;
                            }

                            string name = concatenatedRow;

                            //<Quantity>1</Quantity> 
                            string quantity = "1";
                            //<Price>200</Price> 
                            string price = _getValueFromXML(_couponChildElement, "CouponSum");
                            //<Discount>0</Discount> 
                            string discount = "0";

                            OrderRow _ordrow = new OrderRow();
                            _ordrow.Code = _getValueFromXML(_couponChildElement, "ShippingMethod"); //code;
                            _ordrow.Name = name;

                            if (!string.IsNullOrEmpty(quantity))
                            {
                                _ordrow.Quantity = _convertXMLByte(quantity);
                            }

                            if (!string.IsNullOrEmpty(price))
                            {
                                _ordrow.Price = _convertXMLDecimal(price);
                            }

                            if (!string.IsNullOrEmpty(discount))
                            {
                                _ordrow.Discount = _convertXMLDecimal(discount);
                            }

                            _header.OrderItems.Add(_ordrow);
                        }

                        _response.Orders.Add(_header);
                    }
                }

            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("CGIXrmGetOrders", "Error occured in GetOrdersServiceManager.GetOrders.\n" + ex.Message + "\n" + ex.StackTrace, EventLogEntryType.Error);
                _response.ErrorMessage = ex.Message;
                _response.Orders = null;
            }

            return _response;
        }

        private string _orderXMLRequest(string customerId, string orderNumber, string cardNumber, string from, string to, string email, string ehandeladdressOrders)
        {
            string _request = "";
            _request += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + ehandeladdressOrders + "'>";
            _request += "    <soapenv:Header/>";
            _request += "    <soapenv:Body>";
            _request += "        <ns:GetOrdersRequest>";

            if (!string.IsNullOrEmpty(email))
                _request += "            <Email>" + email + "</Email>";

            if (!string.IsNullOrEmpty(cardNumber))
                _request += "            <CardNumber>" + cardNumber + "</CardNumber>";

            if (!string.IsNullOrEmpty(orderNumber))
                _request += "            <OrderNumber>" + orderNumber + "</OrderNumber>";

            if (!string.IsNullOrEmpty(from))
                _request += "            <From>" + from + "</From>";

            if (!string.IsNullOrEmpty(to))
                _request += "            <To>" + to + "</To>";

            _request += "        </ns:GetOrdersRequest>";
            _request += "    </soapenv:Body>";
            _request += "</soapenv:Envelope>";

            return _request;
        }

        internal GetCreditOrderResponse CreditOrder(CreditOrderRequest request)
        {
            GetCreditOrderResponse _response = new GetCreditOrderResponse();
            _response.CreditOrderMessage = new System.Collections.ObjectModel.ObservableCollection<CreditOrderMessage>();

            try
            {
                string _soapActionAddress = ConfigurationManager.AppSettings["CreditSoapActionAddress"].ToString();
                string _orderServiveAddress = ConfigurationManager.AppSettings["CreditServiveAddress"].ToString();
                string _ehandeladdressOrders = ConfigurationManager.AppSettings["Creditehandeladdress"].ToString();

                CreditOrderMessage _message = new CreditOrderMessage();

                foreach (CreditRow _row in request.CreditRows)
                {
                    string _data = _creditXMLRequest(_row.OrderNumber, _row.Sum, _row.ProductNumber, _row.Quantity, _ehandeladdressOrders);
                    string _xmlresponse = "";

                    using (var client = new WebClient())
                    {
                        // the Content-Type needs to be set to XML
                        client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                        client.Headers.Add("SOAPAction", "\"" + _soapActionAddress + "");
                        client.Encoding = Encoding.UTF8;
                        _xmlresponse = client.UploadString("" + _orderServiveAddress + "", _data);
                    }


                    if (!string.IsNullOrEmpty(_xmlresponse))
                    {
                        bool _error = false;
                        //<faultcode xmlns:a="http://schemas.microsoft.com/net/2005/12/windowscommunicationfoundation/dispatcher">a:InternalServiceFault</faultcode>
                        //<faultstring xml:lang="sv-SE">The server was unable to process the request due to an internal error.  For more information about the error, either turn on IncludeExceptionDetailInFaults (either from ServiceBehaviorAttribute or from the &lt;serviceDebug> configuration behavior) on the server in order to send the exception information back to the client, or turn on tracing as per the Microsoft .NET Framework SDK documentation and inspect the server trace logs.</faultstring>
                        XDocument _xdoc = XDocument.Parse(_xmlresponse);
                        IEnumerable<XElement> _faults = _getNodesListFromXML(_xdoc, "Fault");
                        foreach (XElement _fault in _faults)
                        {
                            //<faultcode xmlns:a="http://schemas.microsoft.com/net/2005/12/windowscommunicationfoundation/dispatcher">a:InternalServiceFault</faultcode>
                            string _faultcode = _getValueFromXML(_fault, "faultcode");
                            //<faultstring xml:lang="sv-SE">The server was unable to process the request due to an internal error.  For more information about the error, either turn on IncludeExceptionDetailInFaults (either from ServiceBehaviorAttribute or from the &lt;serviceDebug> configuration behavior) on the server in order to send the exception information back to the client, or turn on tracing as per the Microsoft .NET Framework SDK documentation and inspect the server trace logs.</faultstring>
                            string _faultstring = _getValueFromXML(_fault, "faultstring");
                            _message.ErrorMessage = _faultstring;
                            _error = true;
                        }

                        if (_error == false)
                        {
                            IEnumerable<XElement> _bodys = _getNodesListFromXML(_xdoc, "Body");
                            foreach (XElement _body in _bodys)
                            {
                                //<OrderNumber>PO5000139</OrderNumber>
                                string _ordernumber = _getValueFromXML(_body, "OrderNumber");
                                //<Sum>99</Sum>
                                string _sum = _getValueFromXML(_body, "Sum");
                                //<ReferenceNumber>PO5000139</ReferenceNumber>
                                string _referencenumber = _getValueFromXML(_body, "ReferenceNumber");
                                //<Success>true</Success>
                                string _success = _getValueFromXML(_body, "Success");
                                //<Message/>
                                string _mess = _getValueFromXML(_body, "Message");
                                //<Date>2014-11-24T10:32:47.4699271+01:00</Date>
                                string _date = _getValueFromXML(_body, "Date");

                                _message.OrderNumber = _ordernumber;
                                _message.Sum = _sum;
                                _message.ReferenceNumber = _referencenumber;
                                _message.Success = _success;
                                _message.Message = _mess;

                                if (!string.IsNullOrEmpty(_date))
                                    _message.Date = _convertXMLDateTimeToDate(_date);

                                if (!string.IsNullOrEmpty(_date))
                                    _message.Time = _convertXMLDateTimeToTime(_date);

                            }

                            _message.ProductNumber = _row.ProductNumber;

                            if (!string.IsNullOrEmpty(_message.Success))
                            {
                                if (_message.Success.ToUpper() == "TRUE")
                                {
                                    //save credit orderrow to crm.
                                    bool _ok = _saveCreditOrderRow(_message, _row);
                                    _message.CRMMessage = "OK";
                                }
                                else
                                {
                                    _message.CRMMessage = "ERROR";
                                }
                            }
                            else
                            {
                                _message.CRMMessage = "ERROR";
                            }

                            _response.CreditOrderMessage.Add(_message);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _response.CreditOrderMessage.Add(new CreditOrderMessage { CRMMessage = "ERROR", Message = "", ErrorMessage = ex.Message.ToString() });
            }

            return _response;
        }

        private string _creditXMLRequest(string orderNumber, string sum, string productnumber, string quantity, string ehandeladdressOrders)
        {
            string _request = "";
            _request += "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='" + ehandeladdressOrders + "'>";
            _request += "<soapenv:Header/>";
            _request += "<soapenv:Body>";
            _request += "    <ns:CreditOrderRequest>";
            _request += "        <orderNumber>" + orderNumber + "</orderNumber>";
            _request += "        <sum>" + sum + "</sum>";
            _request += "        <ProductNumber>" + productnumber + "</ProductNumber>";
            _request += "        <Quantity>" + quantity + "</Quantity>";
            _request += "    </ns:CreditOrderRequest>";
            _request += "</soapenv:Body>";
            _request += "</soapenv:Envelope>";

            return _request;
        }

        internal SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid)
        {
            SavedCreditOrderRowsResponse _response = new SavedCreditOrderRowsResponse();

            try
            {
                if (string.IsNullOrEmpty(accountid) == false || string.IsNullOrEmpty(countactid) == false)
                {
                    SqlConnection sqlCon = OpenSQL();
                    OrderSqlList orderlist = null;
                    bool _accountSearch = false;

                    if (!string.IsNullOrEmpty(accountid))
                        _accountSearch = true;
                    else
                        _accountSearch = false;

                    if (_accountSearch == true)
                    {
                        using (SqlCommand command = new SqlCommand("sp_GetCreditOrderRowsForAccount", sqlCon))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter { ParameterName = "@customerid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = new Guid(accountid) });

                            XmlReader reader = command.ExecuteXmlReader();
                            if (reader != null)
                            {
                                XmlSerializer ser = new XmlSerializer(typeof(OrderSqlList));
                                orderlist = ser.Deserialize(reader) as OrderSqlList;
                            }
                            reader.Close();
                            CloseSQL(sqlCon);

                            if (orderlist != null && orderlist.CreditOrderRows != null && orderlist.CreditOrderRows.Count() > 0)
                            {
                                _response.OrderList = new List<CreditOrderRow>();
                                _response.OrderList = orderlist.CreditOrderRows;
                                _response.ErrorMessage = "";
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("sp_GetCreditOrderRowsForContact", sqlCon))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter { ParameterName = "@customerid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = new Guid(countactid) });

                            XmlReader reader = command.ExecuteXmlReader();
                            if (reader != null)
                            {
                                XmlSerializer ser = new XmlSerializer(typeof(OrderSqlList));
                                orderlist = ser.Deserialize(reader) as OrderSqlList;
                            }
                            reader.Close();
                            CloseSQL(sqlCon);

                            if (orderlist != null && orderlist.CreditOrderRows != null && orderlist.CreditOrderRows.Count() > 0)
                            {
                                _response.OrderList = new List<CreditOrderRow>();
                                _response.OrderList = orderlist.CreditOrderRows;
                                _response.ErrorMessage = "";
                            }
                        }
                    }
                }
                else
                {
                    // Return empty Data Transfer Object as accountid and contactid was empty so no db search was perfomed.
                    _response.OrderList = new List<CreditOrderRow>();
                    _response.ErrorMessage = "";
                    return _response;
                    //return null;
                }
            }
            catch (FaultException faultex)
            {
                _response.OrderList = null;
                _response.ErrorMessage = "CGIXrmGetOrders.GetSavedCreditOrderRows " + faultex.Message;
            }
            catch (Exception ex)
            {
                _response.OrderList = null;
                _response.ErrorMessage = "CGIXrmGetOrders.GetSavedCreditOrderRows " + ex.Message;
            }

            return _response;
        }

        private void PopulateCustomerFromCRM(string emailAddress, Customer customer)
        {
            try
            {
                //query contact by filter email
                Microsoft.Xrm.Sdk.Query.QueryExpression contactQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression("contact");
                contactQuery.ColumnSet.AddColumns("fullname");

                contactQuery.Criteria = new Microsoft.Xrm.Sdk.Query.FilterExpression();
                contactQuery.Criteria.AddCondition("emailaddress1", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, emailAddress);

                EntityCollection contactResults = _crmmanager.Service.RetrieveMultiple(contactQuery);

                //check if contact was found by that email
                if (contactResults != null && contactResults.Entities != null && contactResults.Entities.Count > 0)
                {
                    customer.AccountNumber = contactResults.Entities[0].Id.ToString();
                    customer.Name = contactResults.Entities[0]["fullname"] as string;
                    customer.Email = emailAddress;
                    customer.IsCompany = false;
                }
            }
            catch { /* do nothing, if something goes wrong we simply do not return the name and id of customer */}
        }

        private bool _saveCreditOrderRow(CreditOrderMessage row, CreditRow creditOrderRow)
        {
            bool _returnValue = true;

            try
            {
                Entity _ent = _createEntity(row, creditOrderRow);
                if (_ent != null)
                {
                    Guid _g = _crmmanager.Create(_ent);
                }
            }
            catch (Exception ex)
            {
                _returnValue = false;
                throw;
            }

            return _returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="creditOrderMessage">Is the reply from e-handel API</param>
        /// <param name="creditRow">Is the DTO from our CreditOrder service method, incoming request</param>
        /// <returns></returns>
        private Entity _createEntity(CreditOrderMessage creditOrderMessage, CreditRow creditRow)
        {
            Entity _entity = null;

            try
            {
                _entity = new Entity();
                _entity.LogicalName = "cgi_creditorderrow";
                _entity.Attributes = new AttributeCollection();

                if (!string.IsNullOrEmpty(creditOrderMessage.OrderNumber))
                {
                    _entity.Attributes.Add("cgi_ordernumber", creditOrderMessage.OrderNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Sum))
                {
                    _entity.Attributes.Add("cgi_sum", creditOrderMessage.Sum);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.ReferenceNumber))
                {
                    _entity.Attributes.Add("cgi_referencenumber", creditOrderMessage.ReferenceNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Success))
                {
                    _entity.Attributes.Add("cgi_success", creditOrderMessage.Success);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Message))
                {
                    _entity.Attributes.Add("cgi_message", creditOrderMessage.Message);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Date))
                {
                    _entity.Attributes.Add("cgi_date", creditOrderMessage.Date);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.Time))
                {
                    _entity.Attributes.Add("cgi_time", creditOrderMessage.Time);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.ProductNumber))
                {
                    _entity.Attributes.Add("cgi_productnumber", creditOrderMessage.ProductNumber);
                }

                if (!string.IsNullOrEmpty(creditOrderMessage.OrderNumber))
                {
                    _entity.Attributes.Add("cgi_name", creditOrderMessage.OrderNumber);
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.Reason))
                {
                    _entity.Attributes.Add("cgi_reason", creditRow.Reason);
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.AccountId))
                {
                    _entity.Attributes.Add("cgi_accountid", new EntityReference("account", new Guid(creditRow.AccountId)));
                }

                // Info: not same class as the rest! Its creditRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.ContactId))
                {
                    _entity.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(creditRow.ContactId)));
                }

                // Info: not same class as the rest! Its creditOrderRow, not creditOrderMessage.
                if (!string.IsNullOrEmpty(creditRow.CreatedBy))
                {
                    _entity.Attributes.Add("cgi_createdby", creditRow.CreatedBy);
                }

            }
            catch (Exception ex)
            {
                _entity = null;
            }

            return _entity;
        }

    }
}

/*
Collects orders from source and returns a collection of orders
*/

if (typeof (Endeavor) == "undefined") {

    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {

    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.getOrders) == "undefined") {
    Endeavor.Skanetrafiken.getOrders = {

        document: null,
        formContext: null,

        onLoad: function (executionContext) {
            // clear notifications? Xrm.Page.ui.clearFormNotification(Endeavor.Nibe.LoyaltyProgramRow._loadNotificationHolder);
            Endeavor.Skanetrafiken.getOrders.formContext = executionContext.getFormContext();
        },

        setDocument: function (document) {
            Endeavor.Skanetrafiken.getOrders.document = document;
            Endeavor.Skanetrafiken.getOrders.getCreditOrders();
        },

        /* Initiate order search from clicking search button */
        orderSearch: function (document) {

            Endeavor.Skanetrafiken.getOrders.callOrderSearch();
            Endeavor.Skanetrafiken.getOrders.getCreditOrders();
        },

        /* After head.load call this function to begin search and make request */
        callOrderSearch: function () {

            var document = Endeavor.Skanetrafiken.getOrders.document;
            document.getElementById("searchButton").disable = true; // ADD ASYNC STUFF ?

            /* Local function for reading search date */
            function parseDateString(datestring) {
                var formattedstring = "";

                for (var i = 0; i < datestring.length; i++) {
                    if (Number.isNaN(parseInt(datestring[i])) == false) {
                        formattedstring = formattedstring + datestring[i];
                    }
                }

                var yyyy = parseInt(formattedstring.substring(0, 4));
                var mm = parseInt(formattedstring.substring(4, 6)) - 1;
                var dd = parseInt(formattedstring.substring(6, 8));

                var date = new Date(yyyy, mm, dd, 0, 0, 0, 0);
                if (Number.isNaN(date.getTime())) {
                    date = new Date();
                }
                return date.toISOString();
            }

            //COLLECT INPUT VALUES
            var OrderNumber = document.getElementById('orderNr').value;
            var CardNumber = document.getElementById('cardNr').value;
            var StartDate = document.getElementById('startDate').value;
            var EndDate = document.getElementById('endDate').value;

            //CHECK IF EMAIL EXISTS
            var emailattribute1 = Endeavor.Skanetrafiken.getOrders.formContext.getAttribute("emailaddress1");
            var emailattribute2 = Endeavor.Skanetrafiken.getOrders.formContext.getAttribute("emailaddress2");
            var EmailAddress = "";
            if (emailattribute1 && emailattribute1.getValue()) {
                EmailAddress = emailattribute1.getValue();
                if (emailattribute2 && emailattribute2.getValue())
                    if (emailattribute2.getValue() != emailattribute1.getValue())
                        Endeavor.formscriptfunctions.AlertCustomDialog("Multiple email addresses. Default email used");
            } // If email1 is empty, fallback on email2
            else if (emailattribute2 && emailattribute2.getValue())
                EmailAddress = emailattribute2.getValue();

            //IF EMAIL EXISTS, CALL ORDERSREQUEST
            if (EmailAddress) {
                // CHECK IF CARD OR ORDER IS SPECIFIED
                if (CardNumber || OrderNumber) {
                    StartDate = "";
                    EndDate = "";
                }

                var inputParameters = [{ "Field": "EmailAddress", "Value": EmailAddress, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "CardNumber", "Value": CardNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "OrderNumber", "Value": OrderNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "StartDate", "Value": StartDate, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "EndDate", "Value": EndDate, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                Endeavor.formscriptfunctions.callGlobalAction("ed_GetOrders", inputParameters,
                    function (ordersresponse) {
                        Endeavor.Skanetrafiken.getOrders.populateTables(ordersresponse);
                    },
                    function (error) {
                        var errorMessage = "Get Orders service is unavailable. Please contact your systems administrator. Details: " + error.message;
                        console.log(errorMessage);
                        Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                    });
            }
            else 
                Endeavor.formscriptfunctions.AlertCustomDialog("Ange e-post för att söka");
        },

        /* TAKES INPUT FROM FIELDS AND DISPLAYS SEARCH RESULTS IN ORDERTABLE AND CREDITORDERTABLE */
        populateTables: function (ordersresponse) {

            var document = Endeavor.Skanetrafiken.getOrders.document;

            /* Local function for not getting null reference from xml objects */
            function getElementValue(xmlelement, tagname) {
                if (xmlelement && xmlelement.getElementsByTagName(tagname)[0] != null) {
                    if (xmlelement.getElementsByTagName(tagname)[0].firstChild != null) {
                        return xmlelement.getElementsByTagName(tagname)[0].firstChild.nodeValue;
                    }
                    else {
                        return "";
                    }
                }
                else {
                    return "";
                }
            }
            //EMPTY TABLE
            var old_ordertable = document.getElementById("orders").getElementsByTagName('tbody')[0];
            var ordertable = document.createElement('tbody');

            //FOR EACH RESULT ADD ORDER ROW "i"
            for (var i = 0; i < ordersresponse.getElementsByTagName("Orders").length; i++) {

                var xmlorder = ordersresponse.getElementsByTagName("Orders")[i];

                //insert order row
                var orderrow = ordertable.insertRow();
                orderrow.className = "orderrow";

                //insert row with hidden table
                var hiddenrow = ordertable.insertRow();
                hiddenrow.className = "hiddenrow";

                var tablecell = hiddenrow.insertCell(0);
                tablecell.colSpan = "100";
                var tbl = document.createElement('table');
                tbl.className = "hiddentable";

                tablecell.appendChild(tbl);

                //draw expand button
                var cell = orderrow.insertCell();
                var details = document.createElement("a");
                details.style = "text-decoration:none";
                details.style.cursor = "pointer";
                details.onclick = Endeavor.Skanetrafiken.getOrders.toggleRowsFunction(hiddenrow);
                details.innerHTML = ' + ';
                cell.appendChild(details);

                //fill cells with order values

                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderNumber");
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderDate").substring(0, 10);
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderTotal"); // CURRENCY?
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderTotalVat");
                cell = orderrow.insertCell();
                cell.innerHTML = (parseInt(getElementValue(xmlorder, "OrderTotal")) || 0) - (parseInt(getElementValue(xmlorder, "OrderCreditedTotal") || 0));
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderCreditedTotal");
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderStatus");
                cell = orderrow.insertCell();
                cell.innerHTML = getElementValue(xmlorder, "OrderType");
                cell = orderrow.insertCell();

                //draw detail heading (hidden)
                var detailsheadingrow = tbl.insertRow();
                detailsheadingrow.className = "hiddenheading";
                var detailsheadingcell = detailsheadingrow.insertCell();
                detailsheadingcell.innerHTML = "Orderrader";
                detailsheadingcell.colSpan = "11";

                //draw details labels (hidden)
                var detailslabels = tbl.insertRow();
                detailslabels.className = "hiddenlabel";

                cell = detailslabels.insertCell();
                cell.innerHTML = 'Produkt';
                cell.colSpan = "2";
                cell = detailslabels.insertCell();
                cell.innerHTML = 'Rabatt';
                cell = detailslabels.insertCell();
                cell.innerHTML = 'Pris';
                cell = detailslabels.insertCell();
                cell.innerHTML = 'Antal';
                cell = detailslabels.insertCell();
                cell = detailslabels.insertCell();
                cell = detailslabels.insertCell();
                cell = detailslabels.insertCell();
                cell.innerHTML = 'Kredit';
                cell = detailslabels.insertCell();
                cell.innerHTML = 'Anledning';
                cell = detailslabels.insertCell();

                //draw all detail rows (hidden)
                for (var j = 0; j < xmlorder.getElementsByTagName("OrderItems").length; j++) {

                    var orderdetails = xmlorder.getElementsByTagName("OrderItems")[j];

                    var detailsrow = tbl.insertRow();
                    detailsrow.class = "hiddenrow";

                    cell = detailsrow.insertCell();
                    cell.innerHTML = getElementValue(orderdetails, "Name");
                    cell.colSpan = "2";

                    cell = detailsrow.insertCell();
                    cell.innerHTML = getElementValue(orderdetails, "Discount");

                    cell = detailsrow.insertCell();
                    cell.innerHTML = getElementValue(orderdetails, "Price");

                    cell = detailsrow.insertCell();
                    cell.innerHTML = getElementValue(orderdetails, "Quantity");

                    cell = detailsrow.insertCell();
                    cell = detailsrow.insertCell();
                    cell = detailsrow.insertCell();

                    cell = detailsrow.insertCell();
                    var credit = document.createElement("INPUT");
                    credit.type = "text";
                    cell.id = "credit";
                    cell.appendChild(credit);

                    cell = detailsrow.insertCell();
                    var reason = document.createElement("INPUT");
                    reason.type = "text";
                    cell.id = "reason";
                    cell.appendChild(reason);

                    cell = detailsrow.insertCell();
                    var creditbutton = document.createElement("BUTTON");
                    var buttontext = document.createTextNode("Lägg till");
                    creditbutton.appendChild(buttontext);

                    var orderNumber = getElementValue(xmlorder, "OrderNumber");
                    var productNumber = getElementValue(orderdetails, "Name");
                    var creditQuantity = getElementValue(orderdetails, "Quantity");

                    creditbutton.onclick = Endeavor.Skanetrafiken.getOrders.creditRequestFunction(orderNumber, productNumber, creditQuantity, detailsrow);
                    cell.appendChild(creditbutton);
                }

                //draw payment heading (hidden)
                var paymentheadingrow = tbl.insertRow();
                paymentheadingrow.className = "hiddenheading";

                var paymentstablerow = tbl.insertRow();
                paymentstablerow.className = "hiddenrow";

                var paymentstablecell = paymentstablerow.insertCell(0);
                paymentstablecell.colSpan = "11";
                var paymentstbl = document.createElement('table');
                paymentstbl.className = "hiddentable";

                paymentstablecell.appendChild(paymentstbl);

                //draw expand button
                var cell = paymentheadingrow.insertCell();
                var payments = document.createElement("a");
                payments.style = "text-decoration:none";
                payments.style.cursor = "pointer";
                payments.onclick = Endeavor.Skanetrafiken.getOrders.toggleSpecialRowsFunction(paymentstablerow);
                payments.innerHTML = 'Betalningar';
                cell.appendChild(payments);
                cell.colSpan = "11";


                //draw payment headings (hidden)
                var paymentheadings = paymentstbl.insertRow();
                paymentheadings.className = "hiddenlabel";

                cell = paymentheadings.insertCell();
                cell.innerHTML = 'Betalning';
                cell = paymentheadings.insertCell();
                cell.innerHTML = 'Summa';
                cell = paymentheadings.insertCell();
                cell.innerHTML = 'Typ';
                cell = paymentheadings.insertCell();
                cell.innerHTML = 'Status';
                cell = paymentheadings.insertCell();
                cell = paymentheadings.insertCell();
                cell = paymentheadings.insertCell();
                cell = paymentheadings.insertCell();      // OR JUST COLSPAN = 7...
                cell = paymentheadings.insertCell();
                cell = paymentheadings.insertCell();
                cell = paymentheadings.insertCell();

                //draw all payments
                for (var j = 0; j < xmlorder.getElementsByTagName("Payments").length; j++) {

                    var paymentrow = paymentstbl.insertRow();
                    paymentrow.class = "hiddenrow";

                    var payment = xmlorder.getElementsByTagName("Payments")[j];

                    cell = paymentrow.insertCell();
                    cell.innerHTML = getElementValue(payment, "PaymentMethodName");
                    cell = paymentrow.insertCell();
                    cell.innerHTML = getElementValue(payment, "Sum");
                    cell = paymentrow.insertCell();
                    cell.innerHTML = getElementValue(payment, "TransactionType");
                    cell = paymentrow.insertCell();
                    cell.innerHTML = getElementValue(payment, "Status");
                }

                //draw shipping heading (hidden)
                var shippingheadingrow = tbl.insertRow();
                shippingheadingrow.className = "hiddenheading";

                var shippingtablerow = tbl.insertRow();
                shippingtablerow.className = "hiddenrow";

                var shippingtablecell = shippingtablerow.insertCell();
                shippingtablecell.colSpan = "11";
                var shippingtbl = document.createElement('table');
                shippingtbl.className = "hiddentable";

                shippingtablecell.appendChild(shippingtbl);

                //draw expand button
                var cell = shippingheadingrow.insertCell();
                var shippings = document.createElement("a");
                shippings.style = "text-decoration:none";
                shippings.style.cursor = "pointer";
                shippings.onclick = Endeavor.Skanetrafiken.getOrders.toggleSpecialRowsFunction(shippingtablerow);
                shippings.innerHTML = 'Leveransadresser';
                cell.appendChild(shippings);
                cell.colSpan = "11";

                //draw shipping labels (hidden)
                var shippingheading = shippingtbl.insertRow();
                shippingheading.className = "hiddenlabel";

                cell = shippingheading.insertCell();
                cell.innerHTML = 'Företag';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Förnamn';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Efternamn';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Adress';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'c/o';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Postnr.';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Stad';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Land';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Mobilnr';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'E-post';
                cell = shippingheading.insertCell();
                cell.innerHTML = 'Info';

                //draw all shippingaddresses
                for (var j = 0; j < xmlorder.getElementsByTagName("ShippingAddress").length; j++) {

                    var shippingrow = shippingtbl.insertRow();
                    shippingrow.class = "hiddenrow";

                    var shipping = xmlorder.getElementsByTagName("ShippingAddress")[j];

                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "CompanyName");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "FirstName");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "LastName");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "Address");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "Co");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "PostalCode");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "City");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "Country");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "CellPhoneNumber");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "Email");
                    cell = shippingrow.insertCell();
                    cell.innerHTML = getElementValue(shipping, "ExtraInfo");

                }
            }

            old_ordertable.parentNode.replaceChild(ordertable, old_ordertable);
            Endeavor.Skanetrafiken.getOrders.document.getElementById("searchButton").disable = false;
        },

        /* EXPAND ORDER ROWS */
        toggleRowsFunction: function (row) {
            return function () {

                if (row.style.display == "") {
                    row.style.display = "table-row";
                    this.text = ' - ';
                }
                else {
                    row.style.display = "";
                    this.text = ' + ';
                }
            };
        },

        /* EXPAND OTHER ROWS */
        toggleSpecialRowsFunction: function (row) {
            return function () {

                if (row.style.display == "") {
                    row.style.display = "table-row";
                }
                else {
                    row.style.display = "";
                }
            };
        },

        /* CREDIT ORDER FUNCTIONS */

        /* SEND SOAP REQUEST FOR ADDING CREDITS */
        creditRequestFunction: function (orderNumber, productNumber, creditQuantity, row) {

            return function () {

                try {
                    var ordernr = orderNumber;
                    var productnumber = productNumber;
                    var credit = row.cells.namedItem("credit").firstChild.value;
                    var reason = row.cells.namedItem("reason").firstChild.value; // max characters?
                    var quantity = creditQuantity;

                    var inputParameters = [{ "Field": "OrderNumber", "Value": ordernr, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "ProductNumber", "Value": productnumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "Credit", "Value": credit, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "Reason", "Value": reason, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "Quantity", "Value": quantity, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                    Endeavor.formscriptfunctions.callGlobalAction("ed_CreateCreditOrder", inputParameters,
                        function (ordersresponse) {

                            if (ordersresponse.getElementsByTagName('Success') && ordersresponse.getElementsByTagName('Success')[0] && ordersresponse.getElementsByTagName('Success')[0].firstChild && ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue == 'false') {
                                if (ordersresponse.getElementsByTagName('ErrorMessage') && ordersresponse.getElementsByTagName('ErrorMessage')[0] && ordersresponse.getElementsByTagName('ErrorMessage')[0].firstChild)
                                    Endeavor.formscriptfunctions.AlertCustomDialog(ordersresponse.getElementsByTagName('ErrorMessage')[0].firstChild.nodeValue);
                                
                                if (ordersresponse.getElementsByTagName('Message') && ordersresponse.getElementsByTagName('Message')[0] && ordersresponse.getElementsByTagName('Message')[0].firstChild)
                                    Endeavor.formscriptfunctions.AlertCustomDialog(ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue);
                            }

                            if (ordersresponse.getElementsByTagName('Success') && ordersresponse.getElementsByTagName('Success')[0] && ordersresponse.getElementsByTagName('Success')[0].firstChild && ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue == 'true') {

                                var globalContext = Xrm.Utility.getGlobalContext();
                                var userName = globalContext.userSettings.userName;

                                var entity =
                                {
                                    "cgi_ordernumber": ordernr,
                                    "cgi_sum": credit,
                                    "cgi_productnumber": productnumber,
                                    "cgi_reason": reason,
                                    "cgi_createdby": userName,
                                    "cgi_date": ordersresponse.getElementsByTagName('Date')[0].firstChild.nodeValue.substring(0, 10),
                                    "cgi_name": ordernr,
                                    "cgi_time": ordersresponse.getElementsByTagName('Date')[0].firstChild.nodeValue.substring(11, 16),
                                    "cgi_referencenumber": ordersresponse.getElementsByTagName('ReferenceNumber')[0].firstChild.nodeValue,
                                    "cgi_success": ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue
                                }

                                if (ordersresponse.getElementsByTagName('Message')[0].firstChild && ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue)
                                    entity["cgi_message"] = ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue;

                                var entityId = Endeavor.Skanetrafiken.getOrders.formContext.data.entity.getId();
                                var entityName = Endeavor.Skanetrafiken.getOrders.formContext.data.entity.getEntityName();

                                if (entityId) {
                                    entityId = entityId.substring(1, entityId.length - 1);

                                    if (entityName == "contact")
                                        entity["cgi_contactid@odata.bind"] = "/" + entityName + "s(" + entityId + ")";
                                    else if (entityName == "account")
                                        entity["cgi_accountid@odata.bind"] = "/" + entityName + "s(" + entityId + ")";;
                                }

                                row.cells.namedItem("credit").firstChild.value = "";
                                row.cells.namedItem("reason").firstChild.value = "";

                                Xrm.WebApi.createRecord("cgi_creditorderrow", entity).then(
                                    function success(CompletedResponse) {
                                    },
                                    function (errorHandler) {
                                        Endeavor.formscriptfunctions.AlertCustomDialog("An error occurred when saving Credit Order Row: " + errorHandler.message);
                                    }
                                );

                                Endeavor.Skanetrafiken.getOrders.getCreditOrders();
                            }
                        },
                        function (error) {
                            var errorMessage = "Credit Order service is unavailable. Please contact your systems administrator. Details: " + error.message;
                            console.log(errorMessage);
                            Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                        });
                }
                catch (err) {
                    Endeavor.formscriptfunctions.AlertCustomDialog(err.message);
                }
            };
        },

        /* SEARCH FOR AND FETCH CREDIT ORDERS IN DYNAMICS */
        getCreditOrders: function () {

            var entityId = Endeavor.Skanetrafiken.getOrders.formContext.data.entity.getId();
            var entityName = Endeavor.Skanetrafiken.getOrders.formContext.data.entity.getEntityName();

            if (entityId) {
                entityId = entityId.substring(1, entityId.length - 1);

                try {

                    var columnSet = "cgi_creditorderrowid,cgi_name,createdon,cgi_sum,cgi_ordernumber,cgi_date,cgi_time,cgi_referencenumber,cgi_productnumber,cgi_createdby,cgi_reason";
                    Xrm.WebApi.retrieveMultipleRecords("cgi_creditorderrow", "?$select=" + columnSet + "&$filter=cgi_" + entityName + "id eq " + entityId + ")").then(
                        function success(creditresults) {
                            Endeavor.Skanetrafiken.getOrders.populateCreditTable(creditresults);
                        },
                        function (error) {
                            console.log(error.message);
                            Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                        }
                    );
                }
                catch (err) {
                    Endeavor.formscriptfunctions.AlertCustomDialog("Error in credit orders: " + err.message);
                }
            }
        },

        populateCreditTable: function (results) {

            //ordernummer, datum, summa
            var document = Endeavor.Skanetrafiken.getOrders.document;

            //EMPTY TABLE
            var old_creditordertable = document.getElementById("creditorders").getElementsByTagName('tbody')[0];
            var creditordertable = document.createElement('tbody');

            for (var i = 0; i < results.entities.length; i++) {

                //insert credit row
                var creditorderrow = creditordertable.insertRow();

                var cell = creditorderrow.insertCell();
                cell.innerHTML = ' ';

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_ordernumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_date;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_time;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_productnumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_referencenumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_createdby;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_reason;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results.entities[i].cgi_sum;
            }
            old_creditordertable.parentNode.replaceChild(creditordertable, old_creditordertable);
        }
    }
}
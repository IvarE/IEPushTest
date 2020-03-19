/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

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

        onLoad: function () {
            // clear notifications? Xrm.Page.ui.clearFormNotification(Endeavor.Nibe.LoyaltyProgramRow._loadNotificationHolder);
            try {
                Endeavor.Skanetrafiken.getOrders.headLoad(Endeavor.Skanetrafiken.getOrders.loadSuccessCallback);
            } catch (e) {
                Xrm.Utility.alertDialog(e.message);
            }
        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            if (typeof SDK == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Endeavor == "undefined" || typeof Endeavor.Common == "undefined" || typeof Endeavor.Common.Data == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/Endeavor.Common.Data.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetOrdersRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetOrders.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_CreateCreditOrderRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_CreateCreditOrder.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof head.load != "function") {
                console.error("head.load function is not defined.");
                throw new Error("head.load function is not defined.");
            }

            if (jsUrls.length > 0) {
                // Load required JavaScripts
                head.load(jsUrls, successCallback);
            }
            else {
                successCallback();
            }
        },

        loadSuccessCallback: function () {
            console.log("Everything loaded!");
        
            
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
            var emailattribute1 = Xrm.Page.getAttribute("emailaddress1");
            var emailattribute2 = Xrm.Page.getAttribute("emailaddress2");
            var EmailAddress = "";
            if (emailattribute1 && emailattribute1.getValue()) {
                EmailAddress = emailattribute1.getValue();
                if (emailattribute2 && emailattribute2.getValue()) {
                    if (emailattribute2.getValue() != emailattribute1.getValue()) {
                        Xrm.Utility.alertDialog("Multiple email addresses. Default email used");
                    }
                    // TODO DISPLAY MORE THAN ONE EMAIL WARNING
                }
            } // If email1 is empty, fallback on email2
            else if (emailattribute2 && emailattribute2.getValue()) {
                EmailAddress = emailattribute2.getValue();
            }

            //IF EMAIL EXISTS, CALL ORDERSREQUEST
            if (EmailAddress) {
                // CHECK IF CARD OR ORDER IS SPECIFIED
                if (CardNumber || OrderNumber) {
                    StartDate = "";
                    EndDate = "";
                }

                // TODO EXCEPTION IF REQUEST FAILS
                var request = new Sdk.ed_GetOrdersRequest(EmailAddress, CardNumber, OrderNumber, StartDate, EndDate);
                var response = Sdk.Sync.execute(request);

                var ordersresponsetext = response.getGetOrdersResponse();

                // PARSE RESPONSE
                var parser = new DOMParser();
                var ordersresponse = parser.parseFromString(ordersresponsetext, "text/xml");

                // POPULATE ORDER TABLES
                var parsererror = ordersresponse.getElementsByTagName('parsererror');
                var errormessage = ordersresponse.getElementsByTagName('ErrorMessage');

                if (parsererror && parsererror.length > 0) {
                    Xrm.Utility.alertDialog("Get Orders service is unavailable. Please contact your systems administrator.");
                }
                else if (errormessage && errormessage.length > 0 && errormessage[0].innerHTML) {
                    Xrm.Utility.alertDialog(errormessage[0].innerHTML);
                }
                else {
                    Endeavor.Skanetrafiken.getOrders.populateTables(ordersresponse);
                }
            }
            else {
                Xrm.Utility.alertDialog("Ange e-post för att söka");
            }
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

                    var request = new Sdk.ed_CreateCreditOrderRequest(ordernr, productnumber, credit, reason, quantity);
                    var response = Sdk.Sync.execute(request);
                    var ordersresponsetext = response.getCreditOrderResponse();

                    var parser = new DOMParser();
                    var ordersresponse = parser.parseFromString(ordersresponsetext, "text/xml");

                    var parsererror = ordersresponse.getElementsByTagName('parsererror');
                    var errormessage = ordersresponse.getElementsByTagName('ErrorMessage');

                    if (parsererror && parsererror.length > 0) {
                        Xrm.Utility.alertDialog("Credit Order service is unavailable. Please contact your systems administrator.");
                    }
                    else if (errormessage && errormessage.length > 0 && errormessage[0].innerHTML) {
                        Xrm.Utility.alertDialog(errormessage[0].innerHTML);
                    }
                    else {
                        if (ordersresponse.getElementsByTagName('Success') && ordersresponse.getElementsByTagName('Success')[0] && ordersresponse.getElementsByTagName('Success')[0].firstChild && ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue == 'false') {
                            if (ordersresponse.getElementsByTagName('ErrorMessage') && ordersresponse.getElementsByTagName('ErrorMessage')[0] && ordersresponse.getElementsByTagName('ErrorMessage')[0].firstChild) {
                                Xrm.Utility.alertDialog(ordersresponse.getElementsByTagName('ErrorMessage')[0].firstChild.nodeValue);
                            }
                            if (ordersresponse.getElementsByTagName('Message') && ordersresponse.getElementsByTagName('Message')[0] && ordersresponse.getElementsByTagName('Message')[0].firstChild) {
                                Xrm.Utility.alertDialog(ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue);
                            }
                        }

                        if (ordersresponse.getElementsByTagName('Success') && ordersresponse.getElementsByTagName('Success')[0] && ordersresponse.getElementsByTagName('Success')[0].firstChild && ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue == 'true') {

                            var entity = {
                                cgi_OrderNumber: ordernr,
                                cgi_Sum: credit,
                                cgi_ProductNumber: productnumber,
                                cgi_Reason: reason,
                                cgi_CreatedBy: Xrm.Page.context.getUserName(),
                                cgi_Date: ordersresponse.getElementsByTagName('Date')[0].firstChild.nodeValue.substring(0, 10),
                                cgi_name: ordernr,
                                cgi_Time: ordersresponse.getElementsByTagName('Date')[0].firstChild.nodeValue.substring(11, 16),
                                cgi_ReferenceNumber: ordersresponse.getElementsByTagName('ReferenceNumber')[0].firstChild.nodeValue,
                                cgi_Success: ordersresponse.getElementsByTagName('Success')[0].firstChild.nodeValue
                            };

                            if (ordersresponse.getElementsByTagName('Message')[0].firstChild && ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue){
                                entity.cgi_Message = ordersresponse.getElementsByTagName('Message')[0].firstChild.nodeValue;
                            }

                            var entityId = Xrm.Page.data.entity.getId();
                            var entityName = Xrm.Page.data.entity.getEntityName();

                            if (entityId) {
                                entityId = entityId.substring(1, entityId.length - 1);

                                if (entityName == "contact") {

                                    var contact = {
                                        Id: entityId,
                                        LogicalName: "contact",
                                        Name: ""
                                    };

                                    entity.cgi_Contactid = contact;
                                }
                                else {
                                    var account = {
                                        Id: entityId,
                                        LogicalName: "account",
                                        Name: ""
                                    };

                                    entity.cgi_Accountid = account;
                                }
                            }
                            
                            row.cells.namedItem("credit").firstChild.value = "";
                            row.cells.namedItem("reason").firstChild.value = "";

                            SDK.REST.createRecord(entity, "cgi_creditorderrow", function (CompletedResponse) {

                            }, function (errorHandler) {
                                debugger;
                                Xrm.Utility.alertDialog("An error occurred when saving Credit Order Row: " + errorHandler);
                            });

                            Endeavor.Skanetrafiken.getOrders.getCreditOrders();
                        }
                    }
                }
                catch (err) {
                    Xrm.Utility.alertDialog(err.message);
                }
            };
        },

        /* SEARCH FOR AND FETCH CREDIT ORDERS IN DYNAMICS */
        getCreditOrders: function () {

            var entityId = Xrm.Page.data.entity.getId();
            var entityName = Xrm.Page.data.entity.getEntityName();

            if (entityName == "contact") {
                entityName = "Contact";
            }
            else if (entityName == "account") {
                entityName = "Account";
            }

            if (entityId) {
                entityId = entityId.substring(1, entityId.length - 1);

                try {
                    
                    var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_creditorderrowSet?$select=cgi_creditorderrowId,%20cgi_name,CreatedOn,cgi_Sum,cgi_OrderNumber,cgi_Date,cgi_Time,cgi_ReferenceNumber,cgi_ProductNumber,cgi_CreatedBy,cgi_Reason&$filter=cgi_" + entityName + "id/Id%20eq(guid'" + entityId + "')";
                    var creditresults = Endeavor.Common.Data.fetchJSONResults(url);


                    Endeavor.Skanetrafiken.getOrders.populateCreditTable(creditresults);
                }
                catch (err) {
                    Xrm.Utility.alertDialog("Error in credit orders: " + err.message);
                }
            }
        },

        populateCreditTable: function (results) {
            //ordernummer, datum, summa

            var document = Endeavor.Skanetrafiken.getOrders.document;

            //EMPTY TABLE
            var old_creditordertable = document.getElementById("creditorders").getElementsByTagName('tbody')[0];
            var creditordertable = document.createElement('tbody');

            for (var i = 0; i < results.length; i++) {

                //insert credit row
                var creditorderrow = creditordertable.insertRow();

                var cell = creditorderrow.insertCell();
                cell.innerHTML = ' ';

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_OrderNumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_Date;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_Time;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_ProductNumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_ReferenceNumber;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_CreatedBy;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_Reason;

                cell = creditorderrow.insertCell();
                cell.innerHTML = results[i].cgi_Sum;
            }
            old_creditordertable.parentNode.replaceChild(creditordertable, old_creditordertable);
        }
    }
}
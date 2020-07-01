/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

/*
Endeavor BiffTransactions functions
*/

if (typeof (Endeavor) == "undefined") {

    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {

    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.BiffTransactions) == "undefined") {

    Endeavor.Skanetrafiken.BiffTransactions = {

        // REFERENCE TO BIFFTRANSACTIONS DOCUMENT TEST
        htmldocument: null,
        // REFERENCE TO ACTIVE CARD (PAGE DEFAULT OR LATEST SEARCH)
        cardnumber: null,
        missingcard: true,
        travelcardid: null,
        executioncontext: null,

        openCustomForm: function (entityName, entityId) {

            var entityFormOptions = {};
            entityFormOptions["entityName"] = entityName;
            entityFormOptions["entityId"] = entityId;

            // Open the form.
            Xrm.Navigation.openForm(entityFormOptions).then(
                function (success) {
                    console.log(success);
                },
                function (error) {
                    console.log(error);
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(error.message);
                });
        },

        alertCustomDialog: function (msgText) {

            var message = { confirmButtonLabel: "Ok", text: msgText };
            var alertOptions = { height: 150, width: 280 };

            Xrm.Navigation.openAlertDialog(message, alertOptions).then(
                function success(result) {
                    console.log("Alert dialog closed");
                },
                function (error) {
                    console.log(error.message);
                }
            );
        },

        onLoad: function (context) {
            // clear notifications? Xrm.Page.ui.clearFormNotification(Endeavor.Nibe.LoyaltyProgramRow._loadNotificationHolder);
            try {
                Endeavor.Skanetrafiken.BiffTransactions.executioncontext = context;
                Endeavor.Skanetrafiken.BiffTransactions.headLoad(Endeavor.Skanetrafiken.BiffTransactions.loadSuccessCallback);
            } catch (e) {
                Xrm.Utility.alertDialog(e.message);
            }
        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            var globalContext = Xrm.Utility.getGlobalContext();

            if (typeof SDK == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/edp_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Endeavor == "undefined" || typeof Endeavor.Common == "undefined" || typeof Endeavor.Common.Data == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/edp_/script/Endeavor.Common.Data.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetCardDetailsRequest == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetCardDetails.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetCardTransactionsRequest == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetCardTransactions.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_RechargeCardRequest == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_RechargeCard.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetOutstandingChargesRequest == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetOutstandingCharges.min.js";
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
            console.log("Everything loaded!"); // TODO ADD INITIAL LOADS
        },

        setDocument: function (document) {
            Endeavor.Skanetrafiken.BiffTransactions.htmldocument = document;

            if (Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('cardnr').value) {
                Endeavor.Skanetrafiken.BiffTransactions.getDetailsAndTransactions();
            }
        },

        // CALLED WHEN SEARCH BUTTON IS PRESSED
        getDetailsAndTransactions: function () {

            Endeavor.Skanetrafiken.BiffTransactions.cardnumber = Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('cardnr').value;

            if (Endeavor.Skanetrafiken.BiffTransactions.cardnumber) {

                try {
                    Endeavor.Skanetrafiken.BiffTransactions.getDetails();
                }
                catch (e) {
                    debugger;
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Error in TravelCard details: " + e.message);
                }

                try {
                    Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable();
                }
                catch (e) {
                    debugger;
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Error in saved transactions: " + e.message);
                }
            }
            else {
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Inget angivet kortnummer");
            }
        },

        // REQUESTS AND SETS ALL CARD DETAILS FROM BIZTALK AND CRM
        getDetails: function () {

            var htmldocument = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;
            var cardnumber = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;

            // BIFFTALK SOAP REQUEST   
            var carddetailsrequest = new Sdk.ed_GetCardDetailsRequest(cardnumber);
            var response = Sdk.Sync.execute(carddetailsrequest);
            var detailsresponsetext = response.getCardDetailsResponse();

            // CRM ODATA REQUEST - TODO - Web.API
            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_travelcardSet?$select=cgi_travelcardnumber,cgi_travelcardId,cgi_TravelCardName,cgi_FailedAttemptsToChargeMoney,cgi_CreditCardMask,cgi_Contactid,cgi_AutoloadDisconnectionDate,cgi_AutoloadConnectionDate,cgi_AutoloadStatus,ed_BlockedCard,cgi_Accountid&$filter=cgi_travelcardnumber eq('" + cardnumber + "')";
            var detailsresults = Endeavor.Common.Data.fetchJSONResults(url);

            // TRY SET DETAILS. WILL FAIL IF NO CONNECTION TO BIZTALK SERVER
            var parser = new DOMParser();

            var detailsresponse = null;
            try {
                detailsresponse = parser.parseFromString(detailsresponsetext, "text/xml");
            }
            catch (e) {
                //Internet explorer error message
                throw new Error("Get Card Details service is unavailable. Please contact your systems administrator.");
            }

            var detailsparsererror = detailsresponse.getElementsByTagName('parsererror');
            var detailserrormessage = detailsresponse.getElementsByTagName('ErrorMessage');

            if (detailsparsererror && detailsparsererror.length > 0) {
                debugger;
                //Chrome erorr message
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Get Card Details service is unavailable. Please contact your systems administrator.");
            }
            else if (detailserrormessage && detailserrormessage.length > 0 && detailserrormessage[0].innerHTML) {
                debugger;
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(detailserrormessage[0].innerHTML);
            }
            else {
                // CALL FUNCTION
                Endeavor.Skanetrafiken.BiffTransactions.populateValues(detailsresponse, detailsresults);

                // GET OUTSTANDING CHARGES (ENABLES / DISABLES RELOAD BUTTON)

                if (cardnumber && !Endeavor.Skanetrafiken.BiffTransactions.missingcard) {
                    var outstandingchargesrequest = new Sdk.ed_GetOutstandingChargesRequest(cardnumber);

                    var outstandingchargesrequest = Sdk.Sync.execute(outstandingchargesrequest);
                    var outstandingchargesresponsetext = outstandingchargesrequest.getGetOutstandingChargesResponse(); // TODO CHECK IF VALID XML

                    var outstandingchargesresponse = null;
                    try {
                        outstandingchargesresponse = parser.parseFromString(outstandingchargesresponsetext, "text/xml");
                    }
                    catch (e) {
                        //Internet explorer error message
                        throw new Error("Outstanding charges service unavailable. Please contact your systems administrator.");
                    }

                    var chargesparsererror = outstandingchargesresponse.getElementsByTagName('parsererror');
                    var chargeserrormessage = outstandingchargesresponse.getElementsByTagName('ErrorMessage');

                    if (chargesparsererror && chargesparsererror.length > 0) {
                        debugger;
                        //Chrome error message
                        Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Outstanding charges service unavailable. Please contact your systems administrator.");
                    }
                    else if (chargeserrormessage && chargeserrormessage.length > 0 && chargeserrormessage[0].innerHTML) {
                        debugger;
                        Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(chargeserrormessage[0].innerHTML);
                    }
                    else {
                        Endeavor.Skanetrafiken.BiffTransactions.hasOutstandingCharges(outstandingchargesresponse);
                    }
                }
            }
        },

        // POPULATE DETAILS VALUES AND ZONE TABLE
        // detailsresponse = retrieved from biztalk
        // detailsresults = retrieved from oData
        populateValues: function (detailsresponse, detailsresults) {

            var htmldocument = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

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

            /* Local function for converting true/false to ja/nej */
            function getJaNej(str) {

                if (str && str.toLowerCase() == 'true') {
                    return "Ja";
                }

                if (str && str.toLowerCase() == 'false') {
                    return "Nej";
                }

                return "";
            }

            if (detailsresults.entities.length < 1) {
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Kortet hittades inte");
            }
            else {
                if (detailsresponse.getElementsByTagName("ns0:CardInformation") && detailsresponse.getElementsByTagName("ns0:CardInformation").length > 1) {
                    throw new Error("Unexpected response length");
                }
                else {

                    if (detailsresponse.getElementsByTagName("ns0:CardInformation") && detailsresponse.getElementsByTagName("ns0:CardInformation").length < 1) {
                        Endeavor.Skanetrafiken.BiffTransactions.missingcard = true;
                        Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Kortnumret saknas i BizTalk.");
                    }
                    else {
                        Endeavor.Skanetrafiken.BiffTransactions.missingcard = false;
                    }

                    // SET GLOBAL TRAVELCARDID
                    Endeavor.Skanetrafiken.BiffTransactions.travelcardid = detailsresults.entities[0].cgi_travelcardId;

                    // POPULATE INFORMATION DISPLAY
                    var cardinformation = detailsresponse.getElementsByTagName("ns0:CardInformation")[0];
                    var pursedetails = detailsresponse.getElementsByTagName("ns0:PurseDetails")[0];
                    var perioddetails = detailsresponse.getElementsByTagName("ns0:PeriodDetails")[0];

                    var zonelists = [];
                    for (var i = 0; i < detailsresponse.getElementsByTagName("ns0:ZoneLists").length; i++) {
                        zonelists.push(detailsresponse.getElementsByTagName("ns0:ZoneLists")[i]);
                    }

                    var routelists = [];
                    for (var i = 0; i < detailsresponse.getElementsByTagName("ns0:RouteLists").length; i++) {
                        routelists.push(detailsresponse.getElementsByTagName("ns0:RouteLists")[i]);
                    }

                    // CHECK FOR ACCOUNT OR CONTACT
                    if (detailsresults.entities[0].cgi_Contactid.Id) {

                        //TODO- WebAPI
                        var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "ContactSet?$select=FirstName,LastName&$filter=ContactId%20eq(guid'" + detailsresults.entities[0].cgi_Contactid.Id + "')";
                        var contactresults = Endeavor.Common.Data.fetchJSONResults(url);

                        var link = htmldocument.createElement("a");

                        var name = "";
                        if (contactresults.entities[0].FirstName) {
                            name += contactresults.entities[0].FirstName;
                        }
                        if (contactresults.entities[0].LastName) {
                            name += " " + contactresults.entities[0].LastName;
                        }

                        link.innerHTML = name;
                        link.style.cursor = "pointer";
                        link.onclick = function f() {
                            Endeavor.Skanetrafiken.BiffTransactions.openCustomForm("contact", detailsresults.entities[0].cgi_Contactid.Id);
                        };
                        htmldocument.getElementById('name').innerHTML = "";
                        htmldocument.getElementById('name').appendChild(link);
                    }
                    else {

                        //TODO- WebAPI
                        var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "AccountSet?$select=Name&$filter=AccountId eq(guid'" + detailsresults.entities[0].cgi_Accountid.Id + "')";
                        var accountresults = Endeavor.Common.Data.fetchJSONResults(url);

                        var link = htmldocument.createElement("a");
                        link.innerHTML = accountresults.entities[0].Name;
                        link.style.cursor = "pointer";
                        link.onclick = function f() {
                            Endeavor.Skanetrafiken.BiffTransactions.openCustomForm("account", detailsresults.entities[0].cgi_Accountid.Id);
                        };
                        htmldocument.getElementById('name').innerHTML = "";
                        htmldocument.getElementById('name').appendChild(link);
                    }

                    htmldocument.getElementById('cardname').innerHTML = detailsresults.entities[0].cgi_TravelCardName;

                    var autoloadstatus = detailsresults.entities[0].cgi_AutoloadStatus;
                    if (autoloadstatus == 1) {
                        htmldocument.getElementById('autoloadstatus').innerHTML = "Ja";
                    }
                    else {
                        htmldocument.getElementById('autoloadstatus').innerHTML = "Nej";
                    }

                    if (detailsresults.entities[0].cgi_AutoloadConnectionDate && detailsresults.entities[0].cgi_AutoloadConnectionDate.length == 21 && detailsresults.entities[0].cgi_AutoloadConnectionDate.charAt(0) == "/") {
                        htmldocument.getElementById('autoloadstartdate').innerHTML = (new Date(parseInt(detailsresults.entities[0].cgi_AutoloadConnectionDate.substring(6, 19)))).toISOString().substring(0, 10);
                    }
                    else {
                        if (detailsresults.entities[0].cgi_AutoloadConnectionDate) {
                            htmldocument.getElementById('autoloadstartdate').innerHTML = detailsresults.entities[0].cgi_AutoloadConnectionDate;
                        }
                    }

                    if (detailsresults.entities[0].cgi_AutoloadDisconnectionDate) {
                        htmldocument.getElementById('autoloadenddate').innerHTML = detailsresults.entities[0].cgi_AutoloadDisconnectionDate;
                    }

                    htmldocument.getElementById('creditcardnr').innerHTML = "";
                    htmldocument.getElementById('failedattempts').innerHTML = detailsresults.entities[0].cgi_FailedAttemptsToChargeMoney;

                    htmldocument.getElementById('status').innerHTML = getJaNej(getElementValue(cardinformation, 'ns0:CardHotlisted'));
                    htmldocument.getElementById('reason').innerHTML = getElementValue(cardinformation, 'ns0:HotlistReason');
                    htmldocument.getElementById('hotlisted').innerHTML = getJaNej(getElementValue(perioddetails, 'ns0:Hotlisted'));
                    htmldocument.getElementById('blocked').innerHTML = getJaNej(getElementValue(pursedetails, 'ns0:Hotlisted'));

                    htmldocument.getElementById('loadstatus').innerHTML = getJaNej(getElementValue(pursedetails, 'ns0:OutstandingDirectedAutoload'));
                    htmldocument.getElementById('balance').innerHTML = getElementValue(pursedetails, 'ns0:Balance');

                    htmldocument.getElementById('cardtype').innerHTML = getElementValue(perioddetails, 'ns0:ProductType');
                    htmldocument.getElementById('startdate').innerHTML = getElementValue(perioddetails, 'ns0:PeriodStart').substring(0, 10);
                    htmldocument.getElementById('enddate').innerHTML = getElementValue(perioddetails, 'ns0:PeriodEnd').substring(0, 10);
                    htmldocument.getElementById('pricepaid').innerHTML = getElementValue(perioddetails, 'ns0:PricePaid') + " " + getElementValue(perioddetails, 'ns0:Currency');
                    htmldocument.getElementById('waitingperiods').innerHTML = getElementValue(perioddetails, 'ns0:WaitingPeriods');
                    htmldocument.getElementById('activatedautoload').innerHTML = getJaNej(getElementValue(perioddetails, 'ns0:OutstandingEnableThresholdAutoload'));

                    // POPULATE ZONE TABLE
                    htmldocument.getElementById('zonetable').innerHTML = "";

                    for (var j = 0; j < zonelists.length; j++) {
                        var zonenumber = getElementValue(zonelists[j], 'ns0:Zone');

                        var row = htmldocument.getElementById('zonetable').insertRow();
                        var cell = row.insertCell();
                        cell.innerHTML = zonenumber;
                        cell = row.insertCell();

                        try {

                            //TODO- WebAPI
                            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_zonenameSet?$select=cgi_name&$filter=cgi_ZoneID eq('" + zonenumber + "')";
                            var zoneresults = Endeavor.Common.Data.fetchJSONResults(url);
                            cell.innerHTML = zoneresults.entities[0].cgi_name;
                        }
                        catch (e) {
                            Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Error when fetching Zone :" + e.message);
                        }
                    }

                    for (var j = 0; j < routelists.length; j++) {
                        var routenumber = getElementValue(routelists[j], 'ns0:Route');

                        //var row = htmldocument.getElementById('zonetable').insertRow();
                        //var cell = row.insertCell();
                        //cell.innerHTML = zonenumber;
                        //cell = row.insertCell();

                        /*
                        try {
                            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_zonenameSet?$select=cgi_name&$filter=cgi_ZoneID eq('" + zonenumber + "')";
                            var zoneresults = Endeavor.Common.Data.fetchJSONResults(url);
                            cell.innerHTML = zoneresults[0].cgi_name;
                        }
                        catch (e) {
                            Xrm.Utility.alertDialog("Error when fetching Zone :" + e.message);
                        }*/
                    }
                }
            }
        },

        transactionSearch: function (executionContext) {

            var formContext = executionContext.getFormContext();

            if (Endeavor.Skanetrafiken.BiffTransactions.cardnumber) {

                function parseDateString(datestring) {
                    var formattedstring = "";

                    for (var i = 0; i < datestring.length; i++) {
                        if (isNaN(parseInt(datestring[i])) == false) {
                            formattedstring = formattedstring + datestring[i];
                        }
                    }

                    var yyyy = parseInt(formattedstring.substring(0, 4));
                    var mm = parseInt(formattedstring.substring(4, 6)) - 1;
                    var dd = parseInt(formattedstring.substring(6, 8));

                    var date = new Date(yyyy, mm, dd, 0, 0, 0, 0);
                    if (isNaN(date.getTime())) {
                        date = new Date();
                    }
                    return date.toISOString();
                }

                var cardnr = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;
                var datefrom = parseDateString(Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('datefrom').value);
                var dateto = parseDateString(Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('dateto').value);
                var maxtransactions = '100';

                if (datefrom && dateto) {

                    var request = new Sdk.ed_GetCardTransactionsRequest(cardnr, maxtransactions, datefrom, dateto);
                    var response = Sdk.Sync.execute(request);

                    // '<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body><ns1:GetCardTransactionsResponse xmlns:ns1="http://www.skanetrafiken.com/DK/INTSTDK004/GetCardTransactionResponse/20141216" xmlns:ns0="http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"><GetCardTransactionsResult><ns0:CardTransactions><ns0:Transactions><ns0:Date>2015-03-02T17:12:35+01:00</ns0:Date><ns0:DeviceID>4912</ns0:DeviceID><ns0:TxnNum>45498</ns0:TxnNum><ns0:CardSect>2</ns0:CardSect><ns0:RecType>70</ns0:RecType><ns0:TxnType>0</ns0:TxnType><ns0:Route>Bunkeflostrand - Limhamn - Segevång - Bernstorp</ns0:Route><ns0:Currency>SEK</ns0:Currency><ns0:Amount>10.4</ns0:Amount><ns0:OrigZone>4050</ns0:OrigZone></ns0:Transactions><ns0:CardDetails><ns0:CardSerialNumber>1200668118</ns0:CardSerialNumber><ns0:NumTxnsAvailable>61</ns0:NumTxnsAvailable></ns0:CardDetails></ns0:CardTransactions></GetCardTransactionsResult></ns1:GetCardTransactionsResponse></s:Body></s:Envelope>';
                    var transactionsresponsetext = response.getCardTransactionsResponse();

                    //POPULATE TABLES
                    var parser = new DOMParser();

                    var transactionsresponse = null;
                    try {
                        transactionsresponse = parser.parseFromString(transactionsresponsetext, "text/xml");
                    }
                    catch (e) {
                        //Internet explorer error message
                        throw new Error("TravelCard Transactions service is unavailable. Please contact your systems administrator.");
                    }

                    var parsererror = transactionsresponse.getElementsByTagName('parsererror');
                    var errormessage = transactionsresponse.getElementsByTagName('ErrorMessage');

                    if (parsererror && parsererror.length > 0) {
                        debugger;
                        //Chrome error message
                        Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("TravelCard Transactions service is unavailable. Please contact your systems administrator.");
                    }
                    else if (errormessage && errormessage.length > 0 && errormessage[0].innerHTML) {
                        debugger;
                        Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(errormessage[0].innerHTML);
                    }
                    else {
                        Endeavor.Skanetrafiken.BiffTransactions.populateTransactionTable(transactionsresponse, formContext);
                    }
                }
            }
            else {
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Inget angivet kortnummer");
            }
        },

        populateTransactionTable: function (transactionsresponse, formContext) {

            var document = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

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

            var transactionstable = document.getElementById('transactionsbody');

            if (!transactionsresponse.getElementsByTagName("ns0:Transactions") || transactionsresponse.getElementsByTagName("ns0:Transactions").length == 0) {
                transactionstable.innerHTML = "<tr  class=\"blankrow\"></tr>";
            }
            else {

                transactionstable.innerHTML = "";
                for (var i = 0; i < transactionsresponse.getElementsByTagName("ns0:Transactions").length; i++) {

                    var transaction = transactionsresponse.getElementsByTagName("ns0:Transactions")[i];

                    // cgi_travelcardid, cgi_cardsect, cgi_date, cgi_time, cgi_amount, cgi_deviceid, cgi_origzone, cgi_origzonename, cgi_rectype, cgi_route, cgi_txntype, cgi_caseid, cgi_travelcard, cgi_travelcardtransaction
                    // cgi_travelcardid, cgi_origzonename, cgi_caseid, cgi_travelcard, cgi_travelcardtransaction

                    var travelcardid = Endeavor.Skanetrafiken.BiffTransactions.travelcardid;
                    var travelcard = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;
                    var date = getElementValue(transaction, 'ns0:Date').substring(0, 10);
                    var time = getElementValue(transaction, 'ns0:Date').substring(11, 16);
                    var cardsect = Endeavor.Skanetrafiken.BiffTransactions.convertCardSect(getElementValue(transaction, 'ns0:CardSect'));
                    var rectype = Endeavor.Skanetrafiken.BiffTransactions.convertRecType(getElementValue(transaction, 'ns0:RecType'));
                    var txntype = Endeavor.Skanetrafiken.BiffTransactions.convertTransactionType(getElementValue(transaction, 'ns0:TxnType'));
                    var amount = getElementValue(transaction, 'ns0:Amount');
                    var origzone = getElementValue(transaction, 'ns0:OrigZone');
                    var origzonename = ""; //???
                    var route = getElementValue(transaction, 'ns0:Route');
                    var deviceid = getElementValue(transaction, 'ns0:DeviceID');

                    var row = transactionstable.insertRow();
                    var buttoncell = row.insertCell();

                    var formtype = formContext.data.entity.getEntityName();

                    if (formtype.toUpperCase() == "INCIDENT") {
                        var caseid = formContext.data.entity.getId();

                        var save = document.createElement("a");
                        save.style = "font-weight: bold";
                        save.style.cursor = "pointer";
                        save.onclick = Endeavor.Skanetrafiken.BiffTransactions.saveTransaction(travelcardid, cardsect, date, time, amount, deviceid, origzone, origzonename, rectype, route, txntype, caseid, travelcard);
                        save.innerHTML = '+';
                        buttoncell.appendChild(save);
                        buttoncell.title = "Lägg till";
                        buttoncell.style = "text-align: center";
                    }
                    else {
                        buttoncell.innerHTML = " ";
                    }

                    row.insertCell().innerHTML = date;
                    row.insertCell().innerHTML = time;
                    row.insertCell().innerHTML = cardsect;
                    row.insertCell().innerHTML = rectype;
                    var txncell = row.insertCell();
                    txncell.colSpan = "2";
                    txncell.innerHTML = txntype;
                    row.insertCell().innerHTML = amount;
                    row.insertCell().innerHTML = origzone;
                    var routecell = row.insertCell();
                    routecell.colSpan = "3";
                    routecell.innerHTML = route;
                    row.insertCell().innerHTML = deviceid;
                }
            }
        },

        populateSavedTransactionsTable: function (formContext) {

            var document = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

            // POPULATE SAVED TRANSACTIONS TABLE IF CASE???
            var formtype = formContext.data.entity.getEntityName();

            if (formtype.toUpperCase() == "INCIDENT") {

                var cgi_caseid = formContext.data.entity.getId();
                cgi_caseid = cgi_caseid.substring(1, cgi_caseid.length - 1);

                //TODO - WebAPI
                var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_travelcardtransactionSet?$select=cgi_travelcardtransactionId,cgi_TravelCard,cgi_date,cgi_time,cgi_cardsect,cgi_rectype,cgi_txntype,cgi_Amount,cgi_origzone,cgi_route,cgi_deviceid&$filter=cgi_caseId/Id%20eq(guid'" + cgi_caseid + "')";
                var savedtransactionsresults = Endeavor.Common.Data.fetchJSONResults(url);

                var savedtransactionstable = document.getElementById('savedtransactionsbody');

                if (!savedtransactionsresults) {
                    savedtransactionstable.innerHTML = "<tr  class=\"blankrow\"></tr>";
                }
                else {
                    savedtransactionstable.innerHTML = "";
                    for (var i = 0; i < savedtransactionsresults.entities.length; i++) {

                        var row = savedtransactionstable.insertRow();

                        var cell = row.insertCell(); // EMPTY CELL FOR BUTTON-SPACE
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_TravelCard;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_date;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_time;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_cardsect;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_rectype;
                        cell = row.insertCell();
                        cell.colSpan = "2";
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_txntype;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_Amount;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_origzone;
                        cell = row.insertCell();
                        cell.colSpan = "2";
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_route;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults.entities[i].cgi_deviceid;
                        cell = row.insertCell();

                        var del = document.createElement("a");
                        del.style = "font-weight: bold";
                        del.style.cursor = "pointer";
                        del.onclick = Endeavor.Skanetrafiken.BiffTransactions.deleteTransaction(savedtransactionsresults.entities[i].cgi_travelcardtransactionId);
                        del.innerHTML = "x";
                        cell.appendChild(del);
                        cell.title = "Ta bort";
                        cell.style = "text-align: center";

                    }
                }
            }
        },

        /* Function for converting Transaction number to names */
        convertTransactionType: function (type) {
            var returnValue = "";

            if (!type) {
                return _returnValue;
            }

            if (type == "0") {
                _returnValue = "Ordinarie";
            }
            else if (type == "1") {
                _returnValue = "Utställning";
            }
            else if (type == "2") {
                _returnValue = "Refund";
            }
            else if (type == "3") {
                _returnValue = "Hämtad laddning Reskassa";
            }
            else if (type == "4") {
                _returnValue = "Hämtad Autoladda Period";
            }
            else if (type == "+16") {
                _returnValue = "Cancellation";
            }
            else if (type == "+32") {
                _returnValue = "Card Write Error";
            }
            else if (type == "+64") {
                _returnValue = "Walk Away";
            }
            else if (type == "+128") {
                _returnValue = "Info";
            }
            else {
                _returnValue = "Ordinarie";
            }

            return _returnValue;
        },

        /* Function for converting Rectype number to names */
        convertRecType: function (recordtype) {

            var returnvalue = "";

            if (!recordtype) {
                return returnValue;
            }
            if (recordtype == "70") {
                returnValue = "Reskassa";
            }
            else if (recordtype == "71") {
                returnValue = "Value Payment";
            }
            else if (recordtype == "72") {
                returnValue = "Periodkort";
            }
            else if (recordtype == "74") {
                returnValue = "Övergång";
            }
            else if (recordtype == "75") {
                returnValue = "Övergång";
            }
            else if (recordtype == "78") {
                returnValue = "Inspektion";
            }
            else if (recordtype == "79") {
                returnValue = "Activating of Waiting Period";
            }
            else if (recordtype == "80") {
                returnValue = "Laddning Reskassa";
            }
            else if (recordtype == "81") {
                returnValue = "Laddning Periodkort";
            }
            else if (recordtype == "91") {
                returnValue = "Use of Card on hotlist";
            }
            else {
                returnValue = "Reskassa";
            }

            return returnValue;
        },

        /* Function for converting Cardsect number to names */
        convertCardSect: function (cardsect) {
            var returnValue = "";

            if (!cardsect) {
                return returnValue;
            }
            if (cardsect == "0") {
                _returnValue = "Hela kortet";
            }
            else if (cardsect == "1") {
                _returnValue = "Periodkort";
            }
            else if (cardsect == "2") {
                _returnValue = "Reskassa";
            }
            else {
                _returnValue = "Hela kortet";
            }

            return _returnValue;
        },

        saveTransaction: function (cgi_travelcardid, cgi_cardsect, cgi_date, cgi_time, cgi_amount, cgi_deviceid, cgi_origzone, cgi_origzonename, cgi_rectype, cgi_route, cgi_txntype, cgi_caseid, cgi_travelcard) {
            return function () {
                var entity = {

                    cgi_caseId: {
                        Id: cgi_caseid,
                        LogicalName: "incident",
                        Name: "CaseTest"
                    },

                    cgi_TravelCardid: {
                        Id: cgi_travelcardid,
                        LogicalName: "cgi_travelcard",
                        Name: cgi_travelcard
                    },

                    // cgi_TravelCard,cgi_date,cgi_time,cgi_cardsect,cgi_rectype,cgi_txntype,cgi_Amount,cgi_origzone,cgi_route,cgi_deviceid

                    cgi_cardsect: cgi_cardsect,
                    cgi_Amount: cgi_amount,
                    cgi_currency: "",
                    cgi_date: cgi_date,
                    cgi_time: cgi_time,
                    cgi_deviceid: cgi_deviceid,
                    cgi_origzone: cgi_origzone,
                    cgi_OrigZoneName: cgi_origzonename,
                    cgi_rectype: cgi_rectype,
                    cgi_route: cgi_route,
                    cgi_txntype: cgi_txntype,
                    cgi_txnnum: "",
                    cgi_travelcardtransaction: cgi_date + cgi_time + cgi_route,
                    cgi_TravelCard: cgi_travelcard  // TODO CHECK IN SOLUTION
                };

                /* Do the create*/ //TODO - WebAPI
                SDK.REST.createRecord(entity, "cgi_travelcardtransaction", function (CompletedResponse) {

                    Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable();

                }, function (errorHandler) {
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Error function called, error:" + errorHandler);
                });
            };
        },

        deleteTransaction: function (travelcardtransactionid) {
            return function () {

                /* Do the delete*/ //TODO - WebAPI
                SDK.REST.deleteRecord(travelcardtransactionid, "cgi_travelcardtransaction", function (CompletedResponse) {

                    Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable();

                }, function (errorHandler) {
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Error function called, error:" + errorHandler);
                });
            };
        },

        hasOutstandingCharges: function (outstandingchargesresponse) {

            var hasoutstandingcharges = 'false';

            if (outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0] != null) {
                if (outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0].firstChild != null) {
                    hasoutstandingcharges = outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0].firstChild.nodeValue;
                }
                else {
                    hasoutstandingcharges = "false";
                }
            }
            else {
                hasoutstandingcharges = "false";
            }

            if (hasoutstandingcharges == "true") {
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('reloadbutton').disabled = false;
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('outstandingcharges').innerHTML = "Ja";
            }
            else {
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('outstandingcharges').innerHTML = "Nej";
            }
        },

        reload: function () {

            var cardnumber = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;

            if (cardnumber) {
                var request = new Sdk.ed_RechargeCardRequest(cardnumber);

                var response = Sdk.Sync.execute(request);
                var rechargeresponsetext = response.getRechargeCardResponse();

                var parser = new DOMParser();

                var rechargeresponse = null;
                try {
                    rechargeresponse = parser.parseFromString(rechargeresponsetext, "text/xml");
                }
                catch (e) {
                    //Internet explorer error message
                    throw new Error("Recharge Card service is unavailable. Please contact your systems administrator.");
                }

                var parsererror = rechargeresponse.getElementsByTagName('parsererror');
                var errormessage = rechargeresponse.getElementsByTagName('ErrorMessage');

                if (parsererror && parsererror.length > 0) {
                    debugger;
                    //Chrome error message
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Recharge Card service is unavailable. Please contact your systems administrator.");
                }
                else if (errormessage && errormessage.length > 0 && errormessage[0].innerHTML) {
                    debugger;
                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(errormessage[0].innerHTML);
                }
                else {
                    Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('reloadbutton').disabled = false; // TODO

                    Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog(rechargeresponse.getElementsByTagName('Message')[0].firstChild.nodeValue);
                }
            }
            else {
                Endeavor.Skanetrafiken.BiffTransactions.alertCustomDialog("Inget kortnummer hittat");
            }
        }
    };
}
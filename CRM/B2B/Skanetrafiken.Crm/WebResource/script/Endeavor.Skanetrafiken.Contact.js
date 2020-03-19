/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;


// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Contact) == "undefined") {
    Endeavor.Skanetrafiken.Contact = {

        _ticketMovesErrorHolder: "ticketMovesErrorHolder",

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            if (typeof SDK == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.Soap.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetTicketMoveDataFromMKLRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetTicketMoveDataFromMKL.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_BlockCustomerPortalRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_BlockCustomerPortal.min.js";
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

        onLoad: function () {
            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:

                    debugger;
                    //Check if quick create
                    var stateCode = Xrm.Page.getAttribute("statecode");
                    if (stateCode == null) {
                        var privateContact = Xrm.Page.getAttribute("ed_privatecustomercontact");
                        if (privateContact != null) {
                            privateContact.setValue(true);
                        }

                        Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName();
                    }

                    //debugger;
                    var emailaddress1 = Xrm.Page.getAttribute("emailaddress1");
                    var emailaddress2 = Xrm.Page.getAttribute("emailaddress2");
                    // Objects in form?
                    if (emailaddress1 != null && emailaddress2 != null) {
                        // email1 present? Move to email2...
                        if (emailaddress1.getValue() != null) {
                            emailaddress2.setValue(emailaddress1.getValue());
                            emailaddress1.setValue(null);
                        }
                    }

                    Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName();

                    // getOrders prevent load
                    if (Xrm.Page.ui.tabs.get('tab_5'))
                        Xrm.Page.ui.tabs.get('tab_5').setVisible(false);
                    
                    break;
                case FORM_TYPE_UPDATE:
                    Endeavor.Skanetrafiken.Contact.showHideCompanyEngagementTab();
                    Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact();
                    break;
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                    break;
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    break;
            }
        },

        // Install on onSave
        onSave: function (ctx) {
            Xrm.Page.ui.clearFormNotification("IsMoreThanPrivate")

            var formType = Xrm.Page.ui.getFormType();
            if (formType == 1) { //Create
                Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact();
                Xrm.Page.getAttribute("ed_informationsource").setValue(8); // 8-AdmSkapaKund
            } else if (formType == 2) { //Update
                Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact();
                Xrm.Page.getAttribute("ed_informationsource").setValue(12); // 12-AdmAndraKund
            } else {
                // TODO teo - Vad gör vi om formuläret sparas och det varken är Create eller Update?
            }
        },

        resetRequiredLevel: function () {

            var stateCode = Xrm.Page.getAttribute("statecode");
            if (stateCode == null) {

                var businessContact = Xrm.Page.getAttribute("ed_businesscontact");
                var agentContact = Xrm.Page.getAttribute("ed_agentcontact");
                var seniorContact = Xrm.Page.getAttribute("ed_seniorcontact");
                var schoolContact = Xrm.Page.getAttribute("ed_schoolcontact");
                var infotainmentContact = Xrm.Page.getAttribute("ed_infotainmentcontact");
                var privateContact = Xrm.Page.getAttribute("ed_privatecustomercontact");

                if (businessContact != null && agentContact != null && seniorContact != null &&
                    schoolContact != null && infotainmentContact != null && privateContact != null) {

                    var businessValue = businessContact.getValue();
                    var agentValue = agentContact.getValue();
                    var seniorValue = seniorContact.getValue();
                    var schoolValue = schoolContact.getValue();
                    var infotainmentValue = infotainmentContact.getValue();
                    var privateValue = privateContact.getValue();

                    if (businessValue != false || agentValue != false || seniorValue != false ||
                        schoolValue != false || infotainmentValue != false || privateValue != false) {

                        businessContact.setRequiredLevel("none");
                        agentContact.setRequiredLevel("none");
                        seniorContact.setRequiredLevel("none");
                        schoolContact.setRequiredLevel("none");
                        infotainmentContact.setRequiredLevel("none");
                        privateContact.setRequiredLevel("none");

                    }
                    else if (businessValue == false && agentValue == false && seniorValue == false && schoolValue == false && infotainmentValue == false && privateValue == false) {

                        businessContact.setRequiredLevel("required");
                        agentContact.setRequiredLevel("required");
                        seniorContact.setRequiredLevel("required");
                        schoolContact.setRequiredLevel("required");
                        infotainmentContact.setRequiredLevel("required");
                        privateContact.setRequiredLevel("required");

                    }
                }
            }
        },

        isMoreThanPrivateContact: function ()
        {

            debugger;
            Xrm.Page.ui.clearFormNotification("IsMoreThanPrivate")

            var notOnlyPrivateContact = false;
            var privateContact = Xrm.Page.getAttribute("ed_privatecustomercontact").getValue();
            var schoolContact = Xrm.Page.getAttribute("ed_schoolcontact").getValue();
            var seniorContact = Xrm.Page.getAttribute("ed_seniorcontact").getValue();
            var agentContact = Xrm.Page.getAttribute("ed_agentcontact").getValue();
            var infotainmentContact = Xrm.Page.getAttribute("ed_infotainmentcontact").getValue();
            var businessContact = Xrm.Page.getAttribute("ed_businesscontact").getValue();

            if (privateContact != null && (schoolContact != null ||
                seniorContact != null || agentContact != null ||
                infotainmentContact != null || businessContact != null))
            {
                if (privateContact == true && (schoolContact == true || seniorContact == true ||
                    agentContact == true || infotainmentContact == true || businessContact == true))
                {
                    notOnlyPrivateContact = true;
                }
            }

            if (notOnlyPrivateContact == true)
            {
                var additionalTraits = "";
                var school = "School Contact";
                var senior = "Senior Contact";
                var agent = "Agent Contact";
                var infotainment = "Infotainment Contact";
                var business = "Business Contact";
                if (schoolContact == true)
                {
                    additionalTraits = additionalTraits + ", " + school;
                }
                if (seniorContact == true) {
                    additionalTraits = additionalTraits + ", " + senior;
                }
                if (agentContact == true) {
                    additionalTraits = additionalTraits + ", " + agent;
                }
                if (infotainmentContact == true) {
                    additionalTraits = additionalTraits + ", " + infotainment;
                }
                if (businessContact == true) {
                    additionalTraits = additionalTraits + ", " + business;
                }
                
                Xrm.Page.ui.setFormNotification("This Contact is a Private Contact as well as" + additionalTraits + ".", "INFO", "IsMoreThanPrivate");
            }
        },

        showHideCompanyEngagementTab: function () {
            var contactId = Xrm.Page.data.entity.getId();

            // CRM ODATA REQUEST
            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "ed_CompanyRoleSet?$filter=ed_Contact/Id eq(guid'" + contactId + "')";
            var detailsresults = Endeavor.Common.Data.fetchJSONResults(url);

            if (detailsresults == null || detailsresults.length == 0) {
                // Hide Company Engagement tab
                Xrm.Page.ui.tabs.get("Portal roles").setVisible(false);
                Xrm.Page.ui.controls.get("ed_islockedportal").setVisible(false);
            } else {
                // Show Company Engagement tab
                Xrm.Page.ui.tabs.get("Portal roles").setVisible(true);
            }



        },

        onBlockContactShow: function () {
            var lockedPortal = Xrm.Page.getAttribute("ed_islockedportal").getValue();

            var showButton = false;
            var userId = Xrm.Page.context.getUserId();
            var currentUser = userId.slice(1, -1);

            var correctUser = [];
            correctUser[0] = "A15204E6-71D8-E411-80D8-005056903A38"; //Daniel Lopez
            correctUser[1] = "891A2C00-E5DF-E711-80E6-0050569071BE"; //Marie-Louise Åhlén
            correctUser[2] = "892E5446-53A1-E411-80D4-005056903A38"; //CRMADmin Admin

            for (var i = 0; i < correctUser.length; i++) {
                if (currentUser == correctUser[i]) {
                    showButton = true;
                }
            }

            if (lockedPortal == true || showButton != true) {
                return false;
            } else {
                return true;
            }
        },

        onUnblockContactShow: function () {
            var lockedPortal = Xrm.Page.getAttribute("ed_islockedportal").getValue();
            var showButton = false;
            var userId = Xrm.Page.context.getUserId();
            var currentUser = userId.slice(1, -1);

            var correctUser = [];
            correctUser[0] = "A15204E6-71D8-E411-80D8-005056903A38"; //Daniel Lopez
            correctUser[1] = "891A2C00-E5DF-E711-80E6-0050569071BE"; //Marie-Louise Åhlén
            correctUser[2] = "892E5446-53A1-E411-80D4-005056903A38"; //CRMADmin Admin

            for (var i = 0; i < correctUser.length; i++) {
                if (currentUser == correctUser[i]) {
                    showButton = true;
                }
            }

            if (lockedPortal == true && showButton == true) {
                return true;
            } else {
                return false;
            }
        },

        // 
        onSocialSecurityNumberChange: function () {
            var contactNumberControlTag = "cgi_socialsecuritynumber";

            var contactNumber = Xrm.Page.getAttribute(contactNumberControlTag);
            if (!(!contactNumber.getValue() || 0 === contactNumber.getValue().length)) {
                if (!Endeavor.Skanetrafiken.Contact.checkSocSecNumber(contactNumber.getValue())) {
                    Xrm.Page.getControl(contactNumberControlTag).setNotification("Ogiltigt Persnonnummer<BR>(giltiga format: ååmmdd, ååååmmdd, ååååmmddxxxx, ååååmmdd-xxxx)");
                } else {
                    Xrm.Page.getControl(contactNumberControlTag).clearNotification();
                }
            } else {
                Xrm.Page.getControl(contactNumberControlTag).clearNotification();
            }
        },

        onMarkForCreditsafeUpdate: function () {
            var guid = Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");

            if (!Xrm.Page.getAttribute("cgi_socialsecuritynumber")) {
                Xrm.Utility.alertDialog("Inget personnummer funnet på formuläret. Vänligen lägg till.");
                return;
            }
            var socSecNr = Xrm.Page.getAttribute("cgi_socialsecuritynumber").getValue();

            if (Xrm.Page.getAttribute("firstname") && Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue() + " " + Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("firstname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue();
            else
                var fullName = "Namn saknas";

            var deltabatchQueue = {};
            deltabatchQueue.ed_ContactGuid = guid;
            deltabatchQueue.ed_ContactNumber = socSecNr;
            deltabatchQueue.ed_DeltabatchOperation = { Value: 899310000 };
            deltabatchQueue.ed_name = "ForceUpdate: " + fullName + ", " + Endeavor.Common.Data.dateToString(new Date(), "yyyy/MM/dd", "-");

            SDK.REST.createRecord(
                deltabatchQueue,
                "ed_DeltabatchQueue",
                function (deltabatchQueue) {
                    if (!deltabatchQueue)
                        Xrm.Utility.alertDialog("Uppdatering kanske inte är schemalagd. Inget returvärde då köpost skapades");
                },
                function (deltabatchQueueError) {
                    Xrm.Utility.alertDialog("Något gick fel när uppdatering skulle schemaläggas:\n\n" + deltabatchQueueError.message);
                });
        },

        onDisplayTicketMoves: function () {
            Xrm.Page.ui.clearFormNotification(Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder);
            try {
                Endeavor.Skanetrafiken.Contact.headLoad(Endeavor.Skanetrafiken.Contact.displayTicketMovesSuccessCallback);
            } catch (e) {
                if (!Xrm.Page.ui.setFormNotification(e.message, "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                    Xrm.Utility.alertDialog(e.message);
            }
        },

        displayTicketMovesSuccessCallback: function () {
            var guid = Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");

            // Om kunden inte har något värde i fältet "Mitt Konto-ID" så visar vi ett varningsmeddelande
            var mittKontoField = Xrm.Page.getAttribute("ed_mklid");
            if (mittKontoField && mittKontoField.getValue) {
                if (!mittKontoField.getValue()) {
                    if (!Xrm.Page.ui.setFormNotification("Denna kund har inte 'Mitt Konto'", "WARNING", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                        Xrm.Utility.alertDialog(results);
                    return;
                }
            } else {
                if (!Endeavor || !Endeavor.Common || !Endeavor.Common.Data) {
                    if (!Xrm.Page.ui.setFormNotification("Vänligen lägg till javascript-filen Endeavor.Common.Data.js i detta formulär", "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                        Xrm.Utility.alertDialog(results);
                    return;
                }
                var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "ContactSet?$select=ed_MklId&$filter=ContactId eq (guid'" + Xrm.Page.data.entity.getId() + "')"
                var result = Endeavor.Common.Data.fetchJSONResults(url);

                if (!result || !result.length > 0 || !result[0] || !result[0].ed_MklId) {
                    if (!Xrm.Page.ui.setFormNotification("Denna kund har inte 'Mitt Konto'", "WARNING", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                        Xrm.Utility.alertDialog(results);
                    return;
                }
            }

            if (Xrm.Page.getAttribute("firstname") && Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue() + " " + Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("firstname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue();
            else
                var fullName = "Namn saknas";

            // call MKL
           
            var req = new Sdk.ed_GetTicketMoveDataFromMKLRequest(guid);
            var mklInfoResult = Sdk.Sync.execute(req);
            var results = mklInfoResult.getGetTicketMoveDataFromMKLResponse();

            try {
                var jsonObj = JSON.parse(results);
            } catch (e) {
                if (!Xrm.Page.ui.setFormNotification(results, "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                    Xrm.Utility.alertDialog(results);
                return;
            }
            var movesDone = jsonObj.user.accountMoved;

            var url = "ed_/html/Endeavor.Skanetrafiken.TicketMoveManager.html";
            window.mklData = fullName + ";" + guid + ";" + movesDone;
            var windowOptions = { openInNewWindow: true, height: 300, width: 600 };
            var manager = Xrm.Utility.openWebResource(url, (fullName + ";" + guid + ";" + movesDone), 600, 300);
        },

        checkSocSecNumber: function (nr) {
            if (Endeavor.Skanetrafiken.Contact.checkPersonnummer(nr)) {
                var sweSocSec = Xrm.Page.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(true);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            } else if (Endeavor.Skanetrafiken.Contact.checkNonSwedishSocSecNumber(nr)) {
                var sweSocSec = Xrm.Page.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(false);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            }
            return false;
        },

        checkPersonnummer: function (nr) {
            this.valid = false;
            //if(!nr.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})$/)){ return false; }
            if (nr.match(/^(\d{4})(\d{2})(\d{2})-(\d{4})$/)) {
                nr = nr.replace("-", "");
            }
            if (!nr.match(/^(\d{4})(\d{2})(\d{2})(\d{4})$/)) {
                return false;
            }

            this.fullYear = RegExp.$1;
            this.year = this.fullYear.substring(2, 4);
            this.month = RegExp.$2;
            this.day = RegExp.$3;
            this.controldigits = RegExp.$4;

            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }

            this.alldigits = this.year + this.month + this.day + this.controldigits;

            var nn = "";
            for (var n = 0; n < this.alldigits.length; n++) {
                nn += ((((n + 1) % 2) + 1) * this.alldigits.substring(n, n + 1));
            }
            this.checksum = 0;

            for (var n = 0; n < nn.length; n++) {
                this.checksum += nn.substring(n, n + 1) * 1;
            }
            this.valid = (this.checksum % 10 == 0) ? true : false;
            this.sex = parseInt(this.controldigits.substring(2, 3)) % 2;
            return this.valid;
        },

        checkNonSwedishSocSecNumber: function (nr) {
            if (nr.match(/^(\d{4})(\d{2})(\d{2})$/)) {
                this.fullYear = RegExp.$1;
                this.year = this.fullYear.substring(2, 4);
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else if (nr.match(/^(\d{2})(\d{2})(\d{2})$/)) {
                this.year = RegExp.$1;
                this.fullYear = parseInt(this.year > (new Date()).getFullYear() % 100) ? "19" : "20" + this.year;
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else {
                return false;
            }
            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }
            return true;
        },

        checkDateFormat: function () {
            var months = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
            if (this.fullYear % 400 == 0 || this.fullYear % 4 == 0 && this.fullYear % 100 != 0) {
                months[1] = 29;
            }

            if (this.month * 1 < 1 || this.month * 1 > 12 || this.day * 1 < 1 || this.day * 1 > months[this.month * 1 - 1]) {
                return false;
            }
            return true;
        },

        blockCustomerPortal: function () {
            //Xrm.Page.ui.clearFormNotification(Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder);
            try {
                Endeavor.Skanetrafiken.Contact.headLoad(Endeavor.Skanetrafiken.Contact.blockCustomerPortalSuccessCallback);
            } catch (e) {
                Xrm.Utility.alertDialog(e.message);
            }
        },

        blockCustomerPortalSuccessCallback: function () {
            var SSN = "";
            var blocked = "";

            //if (Xrm.Page.getAttribute("cgi_socialsecuritynumber")) {
            //    SSN = Xrm.Page.getAttribute("cgi_socialsecuritynumber").getValue();
            //}

            //Använda ed_socialsecuritynumberblock
            if (Xrm.Page.getAttribute("ed_socialsecuritynumberblock")) {
                SSN = Xrm.Page.getAttribute("ed_socialsecuritynumberblock").getValue();
            }

            if (Xrm.Page.getAttribute("ed_islockedportal")) {
                blocked = Xrm.Page.getAttribute("ed_islockedportal").getValue();
            }

            if (SSN) {
                if (blocked === true) {
                    try {
                        var request = new Sdk.ed_BlockCustomerPortalRequest(SSN, false);
                        var response = Sdk.Sync.execute(request);
                        Xrm.Utility.alertDialog("Kund avblockerad!");
                    }
                    catch (e) {
                        Xrm.Utility.alertDialog("Kunde inte avblockera kund. Var god försök igen senare.");
                    }
                }
                else {
                    try {
                        var request = new Sdk.ed_BlockCustomerPortalRequest(SSN, true);
                        var response = Sdk.Sync.execute(request);
                        Xrm.Utility.alertDialog("Kund spärrad!");
                    }
                    catch (e) {
                        Xrm.Utility.alertDialog("Kunde inte spärra kund. Var god försök igen senare.");
                    }
                }

                Xrm.Page.data.refresh();
            }
            else {
                Xrm.Utility.alertDialog("Kunden saknar personnummer.");
            }
        },

        setFocusQuickCreateContactFirstName: function () {
            try {
                Xrm.Page.getControl("firstname").setFocus();
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName\n\n" + e.Message);
            }
            
        },

        //Senare fix med säkerhetsroller
        hideShowNewButton: function (primaryControl) {

            //debugger;

            var roles = [];
            roles[0] = "BBC User";
            var isUserBBC = Account.currentUserHasSecurityRole(roles);

            return isUserBBC;

        },

        currentUserHasSecurityRole: function (roles) {
            var fetchXml =
                "<fetch mapping='logical'>" +
                "<entity name='systemuser'>" +
                "<attribute name='systemuserid' />" +
                "<filter type='and'>" +
                "<condition attribute='systemuserid' operator='eq-userid' />" +
                "</filter>" +
                "<link-entity name='systemuserroles' from='systemuserid' to='systemuserid' visible='false' intersect='true'>" +
                "<link-entity name='role' from='roleid' to='roleid' alias='r'>" +
                "<filter type='or'>";

            for (var i = 0; i < roles.length; i++) {
                fetchXml += "<condition attribute='name' operator='eq' value='" + roles[i] + "' />";
            }

            fetchXml += "            </filter>" +
                "</link-entity>" +
                "</link-entity>" +
                "</entity>" +
                "</fetch>";
            var modifiedFetchXml = fetchXml.replace("&", "&amp;");
            var users = Account.ExecuteFetch(modifiedFetchXml, "systemusers");
            if (users > 0)
                return false;
            else
                return true;
        },

        ExecuteFetch: function (originalFetch, entityname) {
            var count = 0;
            var fetch = encodeURI(originalFetch);

            var serverURL = Xrm.Page.context.getClientUrl();
            var Query = entityname + "?fetchXml=" + fetch;
            var req = new XMLHttpRequest();
            req.open("GET", serverURL + "/api/data/v8.0/" + Query, false);
            req.setRequestHeader("Accept", "application/json");
            req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            req.setRequestHeader("OData-MaxVersion", "4.0");
            req.setRequestHeader("OData-Version", "4.0");
            req.onreadystatechange = function () {
                if (this.readyState == 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status == 200) {
                        var data = JSON.parse(this.response);
                        if (data != null) {
                            count = data.value.length;
                        }
                    }
                    else {
                        var error = JSON.parse(this.response).error;
                        alert(error.message);
                    }
                }
            };
            req.send();
            return count;
        },

        // Senare fix
    };
}
/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;

// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Account) == "undefined") {
    Endeavor.Skanetrafiken.Account = {

        onLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();

            //Check if this is a Quick Create Form
            var stateCode = formContext.getAttribute("statecode");
            if (stateCode == null) {
                var businessCustomer = formContext.getAttribute("ed_businesscustomer");
                var agent = formContext.getAttribute("ed_agent");
                var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

                if (businessCustomer != null && agent != null && infotainment != null) {
                    businessCustomer.setRequiredLevel("required");
                    agent.setRequiredLevel("required");
                    infotainment.setRequiredLevel("required");
                }
            }
            else 
                Endeavor.Skanetrafiken.Account.showInfoAccountPortal(formContext);

            //Hide/Show Õvrig Information
            Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            var globalContext = Xrm.Utility.getGlobalContext();

            if (typeof SDK == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.Soap.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk == "undefined" || typeof Sdk.ed_BlockAccountPortalRequest == "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_BlockAccountPortal.min.js";
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

        resetRequiredLevel: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var stateCode = formContext.getAttribute("statecode");
            if (stateCode == null) {
                var businessCustomer = formContext.getAttribute("ed_businesscustomer");
                var agent = formContext.getAttribute("ed_agent");
                var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

                if (businessCustomer != null && agent != null && infotainment != null) {

                    var businessValue = businessCustomer.getValue();
                    var agentValue = agent.getValue();
                    var infotainmentValue = infotainment.getValue();

                    if (businessValue != false || agentValue != false || infotainmentValue != false) {
                        businessCustomer.setRequiredLevel("none");
                        agent.setRequiredLevel("none");
                        infotainment.setRequiredLevel("none");
                    }
                    else if (businessValue == false && agentValue == false && infotainmentValue == false) {
                        businessCustomer.setRequiredLevel("required");
                        agent.setRequiredLevel("required");
                        infotainment.setRequiredLevel("required");
                    }
                }
            }
        },

        onChangeTypeOfAccount: function (executionContext) {

            var formContext = executionContext.getFormContext();

            //Hide/Show Õvrig Information
            Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Block button
        onBlockAccountShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == false && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Unblock button
        onUnblockAccountShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            } else 
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == true && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Block button
        onBlockCostSiteShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == false && showButton != true)
                return true;
            // If Account = Cost Site (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Unblock button
        onUnblockCostSiteShow: function (formContext) {
            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == true && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        showOvrigInformation: function (formContext) {

            var infotaimentCustomer = formContext.getAttribute("ed_infotainmentcustomer").getValue();
            var customer = formContext.getAttribute("ed_customer").getValue();
            var agent = formContext.getAttribute("ed_agent").getValue();
            var samarbete = formContext.getAttribute("ed_collaborationcustomer").getValue();
            var reseller = formContext.getAttribute("ed_reseller").getValue();
            var school = formContext.getAttribute("ed_schoolcustomer").getValue();
            var senior = formContext.getAttribute("ed_seniorcustomer").getValue();
            var portal = formContext.getAttribute("ed_portalcustomer").getValue();

            if (infotaimentCustomer && !customer && !agent && !samarbete && !reseller && !school && !senior && !portal) {

                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(false);
                formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(false);
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(false);
                formContext.ui.tabs.get("Cost Sites").setVisible(false);

            }
            else {
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(true);
                formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(true);
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(true);
                formContext.ui.tabs.get("Cost Sites").setVisible(true);
            }
        },

        showInfoAccountPortal: function (formContext) {

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            if (parentAccount === null) {
                if (formContext.ui.tabs.get("Cost Sites") !== null && formContext.ui.tabs.get("Cost Sites") !== 'undefined')
                    formContext.ui.tabs.get("Cost Sites").setVisible(true);

                if (formContext.ui.tabs.get("Sales History") !== null && formContext.ui.tabs.get("Sales History") !== 'undefined')
                    formContext.ui.tabs.get("Sales History").setVisible(false);
            }
            else {
                if (formContext.ui.tabs.get("Cost Sites") !== null && formContext.ui.tabs.get("Cost Sites") !== 'undefined')
                    formContext.ui.tabs.get("Cost Sites").setVisible(false);

                if (formContext.ui.tabs.get("Sales History") !== null && formContext.ui.tabs.get("Sales History") !== 'undefined')
                    formContext.ui.tabs.get("Sales History").setVisible(true);
            }

            var isLocked = formContext.getAttribute("ed_islockedportal");
            if (isLocked != null) {
                var isLockedBool = isLocked.getValue();
                if (isLockedBool == true) 
                    formContext.ui.setFormNotification("Detta företag är låst från företagssidan", "WARNING", "2");
            }

            var AllowCreate = formContext.getAttribute("ed_allowcreate");
            if (AllowCreate != null) {
                var allowCreateBool = AllowCreate.getValue();
                if (allowCreateBool == false)
                    formContext.ui.setFormNotification("OBS - Detta företag kan inte skapa upp nya administratörer via företagssidan", "INFO", "1");
            }
        },

        blockAccountPortal: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var blocked = formContext.getAttribute("ed_islockedportal").getValue();
            var msgText = "";

            if (parentAccount == null && blocked === false) {
                msgText = "Är du säker på att du vill spärra hela organisationen från åtkomst till hemsidan?";
            }
            else if (parentAccount == null && blocked === true) {
                msgText = "Är du säker på att du vill avblockera hela organisationen och ge åtkomst till hemsidan?";
            } else if (parentAccount != null && blocked === false) {
                msgText = "Är du säker på att du vill spärra detta kostnadsställe från åtkomst till hemsidan?";
            }
            else {
                msgText = "Är du säker på att du vill avblockera detta kostnadsställe och ge åtkomst till hemsidan?";
            }

            Endeavor.formscriptfunctions.ConfirmCustomDialog(msgText,

                function () {
                    try {
                        Endeavor.Skanetrafiken.Account.headLoad(function () { Endeavor.Skanetrafiken.Account.blockAccountPortalSuccessCallback(formContext); });
                    } catch (e) {
                        Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
                    }
                });
        },

        blockAccountPortalSuccessCallback: function (formContext) {

            var portalID = formContext.getAttribute("accountnumber").getValue();
            var blocked = formContext.getAttribute("ed_islockedportal").getValue();
            var parentID = "";
            var organizationNumber = "";

            if (formContext.getAttribute("parentaccountid").getValue()) {

                if (blocked === true) {
                    try {
                        var request = new Sdk.ed_BlockAccountPortalRequest(portalID, parentID, organizationNumber, false);
                        var response = Sdk.Sync.execute(request);
                        Endeavor.formscriptfunctions.AlertCustomDialog("Företag avblockerat!");
                    }
                    catch (e) {
                        Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte avblockera företag. Var god försök igen senare.");
                    }
                }
                else {
                    try {
                        var request = new Sdk.ed_BlockAccountPortalRequest(portalID, parentID, organizationNumber, true);
                        var response = Sdk.Sync.execute(request);
                        Endeavor.formscriptfunctions.AlertCustomDialog("Företag spärrat!");
                    }
                    catch (e) {
                        Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte spärra företag. Var god försök igen senare.");
                    }
                }
            }
            else {
                var ID = formContext.data.entity.getId();
                ID = ID.substring(1, ID.length - 1);

                Xrm.WebApi.retrieveMultipleRecords("account", "?$select=accountnumber&$filter=parentaccountid eq " + ID).then(
                    function success(results) {

                        var success = "";
                        var failed = "";

                        for (var i = 0; i < results.entities.length; i++) {

                            var account = results.entities[i];
                            if (account.AccountNumber) {

                                try {
                                    var request = new Sdk.ed_BlockAccountPortalRequest(account.AccountNumber, parentID, organizationNumber, !blocked);
                                    var response = Sdk.Sync.execute(request);
                                    success = success + ", " + account.AccountNumber;
                                }
                                catch (e) {
                                    failed = failed + ", " + account.AccountNumber;
                                }
                            }
                            else
                                failed = failed + ", (PortalID saknas)";
                        }

                        if (blocked === true) {
                            if (!failed)
                                Endeavor.formscriptfunctions.AlertCustomDialog("Företag avblockerat.");
                            else
                                Endeavor.formscriptfunctions.AlertCustomDialog("Avblockering av kostnadsställe(n) misslyckades" + failed);
                        }
                        else {
                            if (!failed)
                                Endeavor.formscriptfunctions.AlertCustomDialog("Företag spärrat.");
                            else
                                Endeavor.formscriptfunctions.AlertCustomDialog("Spärr av kostnadsställe(n) misslyckades" + failed);
                        }

                    },
                    function (error) {
                        console.log(error.message);
                        Endeavor.formscriptfunctions.ErrorCustomDialog(error.message, "Retrieve Multiple Records Error");
                    }
                );
            }
        },

        //Senare fix med säkerhetsroller
        showBlockButton: function () {

            var roles = [];
            roles[0] = "Block Portal Account"; //Borde heta "access block button" eller liknande
            var isUserCheck = Endeavor.Skanetrafiken.Account.currentUserHasSecurityRole(roles);

            return isUserCheck;
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
            var users = Endeavor.Skanetrafiken.Account.executeFetch(modifiedFetchXml, "systemusers");
            if (users > 0)
                return false;
            else
                return true;
        },

        executeFetch: function (originalFetch, entityname) {
            var count = 0;
            var fetch = encodeURI(originalFetch);
            var query = entityname + "?fetchXml=" + fetch;

            var globalContext = Xrm.Utility.getGlobalContext();
            var serverURL = globalContext.getClientUrl();

            var req = new XMLHttpRequest();
            req.open("GET", serverURL + "/api/data/v9.0/" + query, false);
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


        //Form Methods CGI Account (from accountLibrary.js)
        onFormLoad: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                switch (formContext.ui.getFormType()) {
                    case FORM_TYPE_CREATE:
                        break;
                    case FORM_TYPE_UPDATE:
                        Endeavor.Skanetrafiken.Account.checkIfUserHasSecRole(formContext);
                        Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext);
                        break;
                    case FORM_TYPE_READONLY:
                    case FORM_TYPE_DISABLED:
                    case FORM_TYPE_QUICKCREATE:
                    case FORM_TYPE_BULKEDIT:
                        break;
                    default:
                        alert("Form type error!");
                        break;
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.onFormLoad\n\n" + e.Message);
            }
        },

        checkIfUserHasSecRole: function (formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var currentUserRoles = globalContext.userSettings.securityRoles();
                for (var i = 0; i < currentUserRoles.length; i++) {
                    var userRoleId = currentUserRoles[i];
                    Endeavor.OData_Querys.GetSecRolesNameAccount(userRoleId, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.checkIfUserHasRole\n\n" + e.Message);
            }
        },

        checkIfUserHasRole_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Inga säkerhetsroller definierade!");
                }
                else {
                    var _roleName = result[0].Name;
                    var emailField = formContext.getAttribute("emailaddress1").getValue();

                    if (emailField && emailField.Length !== 0 && _roleName.indexOf("Handläggare") > 0)
                        Endeavor.formscriptfunctions.SetState("emailaddress1", "true", formContext); //The field should only be editable until it has content
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.checkIfUserHasRole_callback\n\n" + e.Message);
            }
        },

        timerfunction_eHandel: function (formContext) {
            try {
                var arg = 'WebResource_eHandelOrders';
                var obj = formContext.getControl(arg).getObject();
                var entid = formContext.data.entity.getId();

                try {
                    obj.contentWindow.SetID(entid);
                }
                catch (e) {
                    setTimeout(function () { Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext); }, TIMEOUT_COUNTER);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.timerfunction_eHandel\n\n" + e.Message);
            }
        },

        format_phonenumber: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var phoneNumberStr = executionContext.getEventSource();
                var control = formContext.getControl(phoneNumberStr.getName());

                // Verify that the field is valid
                if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null) {

                    if (phoneNumberStr.getValue() != null) {

                        // replace any "-" with a blank space
                        var oldNumberStr = phoneNumberStr.getValue();
                        var newNumberStr = oldNumberStr.replace(/-/g, "");
                        newNumberStr = newNumberStr.replace(/ /g, "");
                        phoneNumberStr.setValue(newNumberStr);
                        if (newNumberStr.indexOf("+") > -1)
                            control.setNotification("Ange telefonnummer utan landsprefix.");
                        else
                            control.clearNotification();
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.format_phonenumber\n\n" + e.Message);
            }
        },

        //--------------------------------------------------------------RIBBON-------------------------------------------------------------

        //Ribbon Method CGI Account Ribbon (from accountRibbon.js)
        createNewCase: function (accountid) {
            try {
                var _accountid = accountid;

                var parameters = {};
                parameters["entityName"] = "incident";
                parameters["cgi_accountid"] = _accountid;

                // Open the window.
                Xrm.Navigation.openForm(parameters).then(

                    function (success) {
                        console.log(success);
                    },
                    function (error) {
                        console.log(error);
                    });
            }
            catch (e) {
                alert("Error in Endeavor.Skanetrafiken.Account.CreateNewCase\n\n", e.Message);
            }
        },

        openMigration_WebResource: function () {
            try {

                var windowOptions = { height: 400, width: 400 };
                Xrm.Navigation.openWebResource("ed_/html/Endeavor.Skanetrafiken.AccountMigration.html", windowOptions);

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Account.openMigration_WebResource");
            }
        },

        hideMigration_Button: function () {
            try {

                return true;

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Account.hideMigration_Button. Error: +" + e.message);
            }
        },
    };
}
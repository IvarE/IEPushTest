/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

// Begin scoping
if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) === "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ValueCodeApproval) === "undefined") {
    Endeavor.Skanetrafiken.ValueCodeApproval = {

        callGlobalAction: function (actionName, inputParameters, sucessCallback, errorCallback) {

            var req = {};

            var parameterTypes = {};
            for (var i = 0; i < inputParameters.length; i++) {
                var parameter = inputParameters[i];

                req[parameter.Field] = parameter.Value;
                parameterTypes[parameter.Field] = { "typeName": parameter.TypeName, "structuralProperty": parameter.StructuralProperty };
            }

            req.getMetadata = function () {

                return {
                    boundParameter: null,
                    parameterTypes: parameterTypes,
                    operationType: 0,
                    operationName: actionName
                };
            };

            Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
        },

        onLoad: function () {
            debugger;
        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            var globalContext = Xrm.Utility.getGlobalContext();

            if (typeof SDK === "undefined" || typeof SDK.REST === "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk === "undefined" || typeof SDK.REST === "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.Soap.min.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Endeavor === "undefined" || typeof Endeavor.Common === "undefined" || typeof Endeavor.Common.Data === "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Endeavor.Common.Data.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Sdk === "undefined" || typeof Sdk.ed_BlockAccountPortalRequest === "undefined") {
                jsUrl = globalContext.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_BlockAccountPortal.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof head.load !== "function") {
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

        approveValueCode: function (formContext) {
            debugger;
            var valueCodeId;
            formContext.data.entity.save();
            formContext.ui.setFormNotification("Skapar och skickar värdekod. Vänligen vänta.", "INFO");

            try {
                //var clearonTemplateId = Xrm.Page.getAttribute("ed_clearontemplateid").getValue();
                var amount = formContext.getAttribute("ed_amount").getValue();
                var mobile = formContext.getAttribute("ed_mobile").getValue();
                var email = formContext.getAttribute("ed_emailaddress").getValue();
                var validTo = formContext.getAttribute("ed_validto").getValue();
                var contact = formContext.getAttribute("ed_contact").getValue()[0].id;
                var travelCardNumber = formContext.getAttribute("ed_travelcardnumber").getValue();
                var typeOfValueCode = formContext.getAttribute("ed_typeofvaluecode").getText();
                var valCodeApprovId;

                var formType = formContext.ui.getFormType();
                //if (formType != 1) //Not Create
                //    valCodeApprovId = Xrm.Page.data.entity.getId();
                //else {
                //    Xrm.Page.ui.setFormNotification("Skapar och skickar värdekod. Vänligen vänta.", "INFO");
                //    return;
                //}

                // Create the ValueCode
                Process.callAction("ed_CreateValueCodeGeneric",
                    [
                        {
                            key: "ClearonTemplateId",
                            type: Process.Type.Int,
                            value: 239
                        },
                        {
                            key: "Amount",
                            type: Process.Type.Float,
                            value: amount
                        },
                        {
                            key: "Mobile",
                            type: Process.Type.String,
                            value: mobile
                        },
                        {
                            key: "Email",
                            type: Process.Type.String,
                            value: email
                        },
                        {
                            key: "ContactId",
                            type: Process.Type.EntityReference,
                            value: new Process.EntityReference("contact", contact)
                        },
                        {
                            key: "ValidTo",
                            type: Process.Type.DateTime,
                            value: validTo
                        },
                        {
                            key: "TypeOfValueCode",
                            type: Process.Type.String,
                            value: typeOfValueCode
                        },
                        {
                            key: "ValueCodeApprovalId",
                            type: Process.Type.EntityReference,
                            value: new Process.EntityReference("ed_valuecodeapproval", valCodeApprovId)
                        }
                    ],
                    function (data) {
                        debugger;

                        valueCodeId = Object.values(data)[0];
                        formContext.ui.setFormNotification("Värdekod skapad.", "INFO");

                        try {

                            Process.callAction("ed_ApproveValueCode",
                                [
                                    {
                                        key: "ValueCodeId",
                                        type: Process.Type.String,
                                        value: valueCodeId.id
                                    },
                                    {
                                        key: "Target",
                                        type: Process.Type.EntityReference,
                                        value: new Process.EntityReference("ed_valuecodeapproval", valCodeApprovId)

                                    },
                                    {
                                        key: "Mobile",
                                        type: Process.Type.String,
                                        value: mobile
                                    },
                                    {
                                        key: "Email",
                                        type: Process.Type.String,
                                        value: email
                                    },
                                    {
                                        key: "TravelCardNumber",
                                        type: Process.Type.String,
                                        value: travelCardNumber
                                    }
                                ],
                                function () {

                                    try {
                                        // Sending the created ValueCode
                                        Process.callAction("ed_SendValueCode",
                                            [{
                                                key: "Target",
                                                type: Process.Type.EntityReference,
                                                value: new Process.EntityReference("ed_valuecode", valueCodeId.id)
                                            }],
                                            function () {
                                                debugger;

                                                Xrm.Page.data.refresh();
                                                Xrm.Page.ui.setFormNotification("Värdekod skickad.", "INFO");
                                            },
                                            function (e, t) {
                                                debugger;
                                                // Error
                                                Xrm.Page.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                                                // Write the trace log to the dev console
                                                if (window.console && console.error) {
                                                    console.error(e + "\n" + t);
                                                }
                                            });
                                    } catch (error) {
                                        var err = error;
                                    }

                                    formContext.data.refresh();
                                    formContext.ui.setFormNotification("Värdekod godkänd.", "INFO");
                                },
                                function (er, tr) {
                                    debugger;
                                    // Error
                                    formContext.ui.setFormNotification("Någonting gick fel: " + er, "INFO");

                                    // Write the trace log to the dev console
                                    if (window.console && console.error) {
                                        console.error(er + "\n" + tr);
                                    }
                                });

                        } catch (err) {
                            var error = err;
                        }
                    },
                    function (e, t) {
                        debugger;
                        // Error
                        formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                        // Write the trace log to the dev console
                        if (window.console && console.error) {
                            console.error(e + "\n" + t);
                        }
                    });
            }
            catch (ex) {

                var error = ex;
                // Error
            }
        },

        declineValueCode: function (formContext) {
            debugger;

            try {

                var valCodeApprovId = Xrm.Page.data.entity.getId();
                // Sending the created ValueCode
                Process.callAction("ed_DeclineValueCode",
                    [{
                        key: "Target",
                        type: Process.Type.EntityReference,
                        value: new Process.EntityReference("ed_valuecodeapproval", valCodeApprovId)
                    }],
                    function () {
                        debugger;

                        formContext.data.refresh(true);
                        formContext.ui.setFormNotification("Värdekod nekad.", "INFO");
                    },
                    function (e, t) {
                        debugger;
                        // Error
                        formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                        // Write the trace log to the dev console
                        if (window.console && console.error) {
                            console.error(e + "\n" + t);
                        }
                    });
            }
            catch (error) {
                var err = error;
            }
        },

        showHideApproveButton: function (formContext) {

            var stat = formContext.getAttribute("statuscode").getValue();
            if (stat != 899310000)
                return true;
            else return false;
        },

        showHideDeclineButton: function (formContext) {

            var stat = formContext.getAttribute("statuscode").getValue();
            if (stat != 899310001 && stat != 899310000)
                return true;
            else return false;
        }

    };
}
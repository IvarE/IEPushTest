/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ValueCode) == "undefined") {
    Endeavor.Skanetrafiken.ValueCode = {

        callAction: function (actionName, entityName, targetId, sucessCallback, errorCallback) {

            var target = {};
            target.entityType = entityName;
            target.id = targetId;

            var parameterTypes = {};
            parameterTypes["entity"] = { "typeName": "mscrm." + entityName, "structuralProperty": 5};

            var req = {};
            req.entity = target;
            req.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: parameterTypes,
                    operationType: 0,
                    operationName: actionName
                };
            };

            Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
        },

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

        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            var globalContext = Xrm.Utility.getGlobalContext();

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

        createMultipleValueCodes: function (formContext) {

            debugger;
            var numberOfValueCodes = prompt("Ange antal värdekoder du vill skapa", "1");
            var isNum = /^\d+$/.test(numberOfValueCodes);
            if (numberOfValueCodes == null && isNum != true) {
                alert("Ogiltigt värde i antal värdekoder. Försök igen");
                return;
            }
            var numberOfValueCodesInt = parseInt(numberOfValueCodes);

            var amountInValueCodes = prompt("Ange belopp på värdekod(-er)", "50");
            isNum = /^\d+$/.test(amountInValueCodes);
            if (amountInValueCodes == null && isNum != true) {
                alert("Ogiltigt belopp. Försök igen");
                return;
            }
            var amountInValueCodesFloat = parseFloat(amountInValueCodes);

            formContext.ui.setFormNotification("Skapar värdekoder. Vänligen vänta.", "INFO");

            var inputParameters = [];
            var parameterAmount = { "Field": "Amount", "Value": amountInValueCodesFloat, "TypeName": "Edm.Decimal", "StructuralProperty": 1 };
            var parameterCount = { "Field": "Count", "Value": numberOfValueCodesInt, "TypeName": "Edm.Int32", "StructuralProperty": 1 };

            inputParameters.push(parameterAmount);
            inputParameters.push(parameterCount);

            Endeavor.Skanetrafiken.ValueCode.callGlobalAction("ed_CreateMultipleValueCodes", inputParameters,
                function () {
                    alert("3 nya värdekoder skapade.");
                    location.reload();
                },
                function (e, t) {
                    alert("Misslyckades att skapa nya värdekoder.");

                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },

        sendValueCode: function (formContext) {
            debugger;
            formContext.data.save();
            formContext.ui.setFormNotification("Skickar värdekod. Vänligen vänta.", "INFO");

            Endeavor.Skanetrafiken.ValueCode.callAction("ed_SendValueCode", "ed_valuecode", formContext.data.entity.getId(),
                function () {
                    formContext.data.refresh();
                    formContext.ui.setFormNotification("Värdekod skickad.", "INFO");
                },
                function (e, t) {
                    // Error
                    formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },
    }
}
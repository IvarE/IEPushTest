/*! head.load - v1.0.3 */
(function (n, t) { "use strict"; function w() { } function u(n, t) { if (n) { typeof n == "object" && (n = [].slice.call(n)); for (var i = 0, r = n.length; i < r; i++) t.call(n, n[i], i) } } function it(n, i) { var r = Object.prototype.toString.call(i).slice(8, -1); return i !== t && i !== null && r === n } function s(n) { return it("Function", n) } function a(n) { return it("Array", n) } function et(n) { var i = n.split("/"), t = i[i.length - 1], r = t.indexOf("?"); return r !== -1 ? t.substring(0, r) : t } function f(n) { (n = n || w, n._done) || (n(), n._done = 1) } function ot(n, t, r, u) { var f = typeof n == "object" ? n : { test: n, success: !t ? !1 : a(t) ? t : [t], failure: !r ? !1 : a(r) ? r : [r], callback: u || w }, e = !!f.test; return e && !!f.success ? (f.success.push(f.callback), i.load.apply(null, f.success)) : e || !f.failure ? u() : (f.failure.push(f.callback), i.load.apply(null, f.failure)), i } function v(n) { var t = {}, i, r; if (typeof n == "object") for (i in n) !n[i] || (t = { name: i, url: n[i] }); else t = { name: et(n), url: n }; return (r = c[t.name], r && r.url === t.url) ? r : (c[t.name] = t, t) } function y(n) { n = n || c; for (var t in n) if (n.hasOwnProperty(t) && n[t].state !== l) return !1; return !0 } function st(n) { n.state = ft; u(n.onpreload, function (n) { n.call() }) } function ht(n) { n.state === t && (n.state = nt, n.onpreload = [], rt({ url: n.url, type: "cache" }, function () { st(n) })) } function ct() { var n = arguments, t = n[n.length - 1], r = [].slice.call(n, 1), f = r[0]; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (f ? (u(r, function (n) { s(n) || !n || ht(v(n)) }), b(v(n[0]), s(f) ? f : function () { i.load.apply(null, r) })) : b(v(n[0])), i) } function lt() { var n = arguments, t = n[n.length - 1], r = {}; return (s(t) || (t = null), a(n[0])) ? (n[0].push(t), i.load.apply(null, n[0]), i) : (u(n, function (n) { n !== t && (n = v(n), r[n.name] = n) }), u(n, function (n) { n !== t && (n = v(n), b(n, function () { y(r) && f(t) })) }), i) } function b(n, t) { if (t = t || w, n.state === l) { t(); return } if (n.state === tt) { i.ready(n.name, t); return } if (n.state === nt) { n.onpreload.push(function () { b(n, t) }); return } n.state = tt; rt(n, function () { n.state = l; t(); u(h[n.name], function (n) { f(n) }); o && y() && u(h.ALL, function (n) { f(n) }) }) } function at(n) { n = n || ""; var t = n.split("?")[0].split("."); return t[t.length - 1].toLowerCase() } function rt(t, i) { function e(t) { t = t || n.event; u.onload = u.onreadystatechange = u.onerror = null; i() } function o(f) { f = f || n.event; (f.type === "load" || /loaded|complete/.test(u.readyState) && (!r.documentMode || r.documentMode < 9)) && (n.clearTimeout(t.errorTimeout), n.clearTimeout(t.cssTimeout), u.onload = u.onreadystatechange = u.onerror = null, i()) } function s() { if (t.state !== l && t.cssRetries <= 20) { for (var i = 0, f = r.styleSheets.length; i < f; i++) if (r.styleSheets[i].href === u.href) { o({ type: "load" }); return } t.cssRetries++; t.cssTimeout = n.setTimeout(s, 250) } } var u, h, f; i = i || w; h = at(t.url); h === "css" ? (u = r.createElement("link"), u.type = "text/" + (t.type || "css"), u.rel = "stylesheet", u.href = t.url, t.cssRetries = 0, t.cssTimeout = n.setTimeout(s, 500)) : (u = r.createElement("script"), u.type = "text/" + (t.type || "javascript"), u.src = t.url); u.onload = u.onreadystatechange = o; u.onerror = e; u.async = !1; u.defer = !1; t.errorTimeout = n.setTimeout(function () { e({ type: "timeout" }) }, 7e3); f = r.head || r.getElementsByTagName("head")[0]; f.insertBefore(u, f.lastChild) } function vt() { for (var t, u = r.getElementsByTagName("script"), n = 0, f = u.length; n < f; n++) if (t = u[n].getAttribute("data-headjs-load"), !!t) { i.load(t); return } } function yt(n, t) { var v, p, e; return n === r ? (o ? f(t) : d.push(t), i) : (s(n) && (t = n, n = "ALL"), a(n)) ? (v = {}, u(n, function (n) { v[n] = c[n]; i.ready(n, function () { y(v) && f(t) }) }), i) : typeof n != "string" || !s(t) ? i : (p = c[n], p && p.state === l || n === "ALL" && y() && o) ? (f(t), i) : (e = h[n], e ? e.push(t) : e = h[n] = [t], i) } function e() { if (!r.body) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(e, 50); return } o || (o = !0, vt(), u(d, function (n) { f(n) })) } function k() { r.addEventListener ? (r.removeEventListener("DOMContentLoaded", k, !1), e()) : r.readyState === "complete" && (r.detachEvent("onreadystatechange", k), e()) } var r = n.document, d = [], h = {}, c = {}, ut = "async" in r.createElement("script") || "MozAppearance" in r.documentElement.style || n.opera, o, g = n.head_conf && n.head_conf.head || "head", i = n[g] = n[g] || function () { i.ready.apply(null, arguments) }, nt = 1, ft = 2, tt = 3, l = 4, p; if (r.readyState === "complete") e(); else if (r.addEventListener) r.addEventListener("DOMContentLoaded", k, !1), n.addEventListener("load", e, !1); else { r.attachEvent("onreadystatechange", k); n.attachEvent("onload", e); p = !1; try { p = !n.frameElement && r.documentElement } catch (wt) { } p && p.doScroll && function pt() { if (!o) { try { p.doScroll("left") } catch (t) { n.clearTimeout(i.readyTimeout); i.readyTimeout = n.setTimeout(pt, 50); return } e() } }() } i.load = i.js = ut ? lt : ct; i.test = ot; i.ready = yt; i.ready(r, function () { y() && u(h.ALL, function (n) { f(n) }); i.feature && i.feature("domloaded", !0) }) })(window);
/*
//# sourceMappingURL=head.load.min.js.map
*/

/*
Endeavor TravelInformation functions
*/

if (typeof (Endeavor) == "undefined") {

    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {

    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.TravelInformation) == "undefined") {

    Endeavor.Skanetrafiken.TravelInformation = {

        contractors: null,
        organisations: null,
        cities: null,
        stopareas: null,

        currentLine: null,
        currentCity: "",

        document: null,
        saveInProgress: false,
        response: null,

        onLoad: function () {
            // clear notifications? Xrm.Page.ui.clearFormNotification(Endeavor.Nibe.LoyaltyProgramRow._loadNotificationHolder);
            try {
                Endeavor.Skanetrafiken.TravelInformation.headLoad(Endeavor.Skanetrafiken.TravelInformation.loadSuccessCallback);
            } catch (e) {
                Xrm.Utility.alertDialog(e);
            }
        },

        headLoad: function () {
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

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetLineDetailsRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetLineDetails.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetContractorsRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetContractors.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetOrganisationalUnitsRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetOrganisationalUnits.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetDirectJourneysRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetDirectJourneys.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof Sdk == "undefined" || typeof Sdk.ed_GetDirectJourneysRequest == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/ed_/script/Sdk.ed_GetServiceJourney.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof head.load != "function") {
                console.error("head.load function is not defined.");
                throw new Error("head.load function is not defined.");
            }

            if (jsUrls.length > 0) {
                // Load required JavaScripts
                head.load(jsUrls, Endeavor.Skanetrafiken.TravelInformation.loadSuccessCallback);
            }
            else {
                Endeavor.Skanetrafiken.TravelInformation.loadSuccessCallback();
            }
        },

        loadSuccessCallback: function () {
            console.log("Everything loaded!");
            Endeavor.Skanetrafiken.TravelInformation.getContractors();
            Endeavor.Skanetrafiken.TravelInformation.getOrganisationalUnits();
        },

        setDocument: function (document) {
            Endeavor.Skanetrafiken.TravelInformation.document = document;
        },

        search: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var transportlist = document.getElementById("transportlist");
            var transport = transportlist.value;

            var citylist = document.getElementById("citylist");
            var city = citylist.value;

            var linelist = document.getElementById("linelist");
            var line = linelist.value;

            var timestamp = document.getElementById('timestamp');

            var time = timestamp.value;
            if (time && time.length == 16) {
                time = time.substring(0, 16).replace(" ", "T");
            }

            var date = new Date(time);
            date.setHours(date.getHours() + 1)

            var validDate = false;
            if (!isNaN(date.getTime())) {
                time = date.toISOString();
                validDate = true;
            }

            var fromlist = document.getElementById("fromlist");
            var fromarea = fromlist.value;

            var tolist = document.getElementById("tolist");
            var toarea = tolist.value;

            if (fromarea && toarea) {
                // var response = '<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body><ns0:GetDirectJourneysBetweenStopsResponse xmlns:ns0="http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneysBetweenStopsResponse/20141023"><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352389</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800050</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>50</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959048</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T10:35:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959049</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T10:35:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T10:35:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352474</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800054</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>54</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959098</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T11:05:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959099</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T11:05:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T11:05:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352559</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800058</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>58</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959148</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T11:35:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959149</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T11:35:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T11:35:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352644</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800062</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>62</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959198</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T12:05:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959199</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T12:05:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T12:05:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops></ns0:GetDirectJourneysBetweenStopsResponse></s:Body></s:Envelope>';

                // (transportType, fromStopAreaGid, toStopAreaGid, tripDateTime, forLineGids);

                var request = new Sdk.ed_GetDirectJourneysRequest(transport, fromarea, toarea, time, line);
                var response = Sdk.Sync.execute(request);

                var responsetext = response.getDirectJourneysResponse();
                var parser = new DOMParser();

                var responsedoc = null;
                try {
                    responsedoc = parser.parseFromString(responsetext, "text/xml");
                }
                catch (e) {
                    throw new Error("Get Direct Journeys service is unavailable. Please contact your systems administrator.");
                }

                var parsererror = responsedoc.getElementsByTagName('parsererror');
                var errormessage = responsedoc.getElementsByTagName('ErrorMessage');

                if (parsererror && parsererror.length > 0) {
                    debugger;
                    Xrm.Utility.alertDialog("Get Direct Journeys service is unavailable. Please contact your systems administrator.");
                }
                else if (errormessage && errormessage.length) {
                    debugger;
                    Xrm.Utility.alertDialog(errormessage);
                }
                else {
                    Endeavor.Skanetrafiken.TravelInformation.populateTravelInformation(transport, city, responsedoc);
                }
            }
            else if ((transport == "REGIONBUS" || transport == "STRADBUS") && line) {
                // GetContractors called onLoad, i.e. no request made in method populateContractorInformation
                Endeavor.Skanetrafiken.TravelInformation.populateContractorInformation(transport, city, line);
            }
        },

        getContractorFromGid: function (gid) {

            var contractorsdoc = Endeavor.Skanetrafiken.TravelInformation.contractors;

            if (contractorsdoc && contractorsdoc.firstChild && contractorsdoc.firstChild.childNodes && contractorsdoc.firstChild.childNodes.length > 0) {

                for (var i = 0; i < contractorsdoc.firstChild.childNodes.length; i++) {

                    var contractor = contractorsdoc.firstChild.childNodes[i];

                    if (gid == contractor.getAttribute("Gid")) {
                        return Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromId(contractor.getAttribute("IsOrganisationId"));
                    }
                }

            }

            return "";
        },

        getOrganisationFromLine: function (cityGid, lineGid) {

            var responsedoc = Endeavor.Skanetrafiken.TravelInformation.response;
            var organisationsdoc = Endeavor.Skanetrafiken.TravelInformation.organisations;

            var line = Endeavor.Skanetrafiken.TravelInformation.getLine(cityGid, lineGid);

            if (line && line.getElementsByTagName("LineOperatorCode") && line.getElementsByTagName("LineOperatorCode")[0]) {

                var lineOpCode = line.getElementsByTagName("LineOperatorCode")[0].firstChild.nodeValue;

                if (organisationsdoc && organisationsdoc.firstChild && organisationsdoc.firstChild.childNodes && organisationsdoc.firstChild.childNodes.length > 0) {

                    for (var i = 0; i < organisationsdoc.firstChild.childNodes.length; i++) {

                        var organisation = organisationsdoc.firstChild.childNodes[i];

                        if (lineOpCode == organisation.getAttribute("Code")) {
                            return organisation;
                        }
                    }
                }
            }

            return "";
        },

        getOrganisationFromId: function (id) {

            var organisationsdoc = Endeavor.Skanetrafiken.TravelInformation.organisations;

            if (organisationsdoc && organisationsdoc.firstChild && organisationsdoc.firstChild.childNodes && organisationsdoc.firstChild.childNodes.length > 0) {

                for (var i = 0; i < organisationsdoc.firstChild.childNodes.length; i++) {

                    var organisation = organisationsdoc.firstChild.childNodes[i];

                    if (id == organisation.getAttribute("Id")) {
                        return organisation.getAttribute("Name");
                    }
                }

            }

            return "";

        },

        setTransportType: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var transportlist = document.getElementById("transportlist");
            var citylist = document.getElementById("citylist");
            citylist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var linelist = document.getElementById("linelist");
            linelist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var fromlist = document.getElementById("fromlist");
            fromlist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var tolist = document.getElementById("tolist");
            tolist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';

            var value = transportlist.value;

            var request = new Sdk.ed_GetLineDetailsRequest(value);
            var response = Sdk.Sync.execute(request);

            var responsetext = response.getGetLineDetailsResponse();
            var parser = new DOMParser();

            try {
                responsedoc = parser.parseFromString(responsetext, "text/xml");
            }
            catch (e) {
                throw new Error("TravelInformationDB is unavailable. Please contact your systems administrator.");
            }

            Endeavor.Skanetrafiken.TravelInformation.response = parser.parseFromString(responsetext, "text/xml");

            var parsererror = Endeavor.Skanetrafiken.TravelInformation.response.getElementsByTagName('parsererror');
            var errormessage = Endeavor.Skanetrafiken.TravelInformation.response.getElementsByTagName('ErrorMessage');

            if (parsererror && parsererror.length > 0) {
                debugger;
                Xrm.Utility.alertDialog("TravelInformationDB is unavailable. Please contact your systems administrator.");
            }
            else if (errormessage && errormessage.length > 0) {
                debugger;
                Xrm.Utility.alertDialog(errormessage[0]);
            }
            else if (value == "TRAIN") {
                citylist.style.visibility = "hidden";
                linelist.style.visibility = "hidden";

                Endeavor.Skanetrafiken.TravelInformation.dropDownTrain();

            }
            else if (value == "REGIONBUS") {
                citylist.style.visibility = "hidden";
                linelist.style.visibility = "visible";

                Endeavor.Skanetrafiken.TravelInformation.dropDownRegionbus();
            }
            else if (value == "STRADBUS") {
                citylist.style.visibility = "visible";
                linelist.style.visibility = "visible";

                Endeavor.Skanetrafiken.TravelInformation.dropDownStradbus();
            }

        },

        dropDownTrain: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var trainresponse = Endeavor.Skanetrafiken.TravelInformation.response;

            var error = trainresponse.getElementsByTagName('ErrorMessage');

            if (error.length > 0) {
                debugger;
                Xrm.Utility.alertDialog(error[0]);
            }
            else {

                var fromlist = document.getElementById('fromlist');
                var options = [];

                var stopareas = trainresponse.getElementsByTagName("StopAreas")[0].childNodes;
                for (var i = 0; i < stopareas.length; i++) {

                    var stoparea = stopareas[i];

                    var name = stoparea.getElementsByTagName("StopAreaName")[0].firstChild.nodeValue;
                    var id = stoparea.getElementsByTagName("StopAreaGid")[0].firstChild.nodeValue;

                    var option = { name: name, value: id };

                    options.push(option);

                }

                Endeavor.Skanetrafiken.TravelInformation.stopareas = options;
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(fromlist, options, true);

            }
        },

        dropDownRegionbus: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var regionbusresponse = Endeavor.Skanetrafiken.TravelInformation.response;

            var error = regionbusresponse.getElementsByTagName('ErrorMessage');

            if (error.length > 0) {
                debugger;
                Xrm.Utility.alertDialog(error[0]);
            }
            else {

                var linelist = document.getElementById('linelist');
                var options = [];

                var lines = regionbusresponse.getElementsByTagName("Lines")[0].childNodes
                for (var i = 0; i < lines.length; i++) {

                    var line = lines[i];
                    var name = line.getElementsByTagName("LineNumber")[0].firstChild.nodeValue;
                    var id = line.getElementsByTagName("LineGid")[0].firstChild.nodeValue;

                    var option = { name: name, value: id };

                    options.push(option);

                }

                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(linelist, options, false);

            }
        },

        dropDownStradbus: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var stradbusresponse = Endeavor.Skanetrafiken.TravelInformation.response;

            var error = stradbusresponse.getElementsByTagName('ErrorMessage');

            if (error.length > 0) {
                debugger;
                Xrm.Utility.alertDialog(error[0]);
            }
            else {

                var citylist = document.getElementById('citylist');
                var options = [];

                var cities = stradbusresponse.getElementsByTagName("Zones")[0].childNodes;
                for (var i = 0; i < cities.length; i++) {

                    var city = cities[i];
                    var name = city.getElementsByTagName("ZoneName")[0].firstChild.nodeValue;
                    var id = city.getElementsByTagName("ZoneId")[0].firstChild.nodeValue;

                    var option = { name: name, value: id };

                    options.push(option);

                }

                Endeavor.Skanetrafiken.TravelInformation.cities = options;
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(citylist, options, true);
            }
        },

        setCity: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var fromlist = document.getElementById("fromlist");
            var tolist = document.getElementById("tolist");

            var linelist = document.getElementById("linelist");
            var options = [];

            var citylist = document.getElementById("citylist");
            var cityid = citylist.value; // WHAT IF CITY IS NOT SELECTED? POSSIBLE? NO?

            var city = Endeavor.Skanetrafiken.TravelInformation.getCity(cityid);

            var lines = city.getElementsByTagName("Lines")[0].childNodes;
            for (var i = 0; i < lines.length; i++) {

                var line = lines[i];

                var isLineActive = true;
                if (line.getElementsByTagName("StopExistsUptoDate")[0] && line.getElementsByTagName("StopExistsUptoDate")[0].firstChild.nodeValue) {
                    var date = new Date(line.getElementsByTagName("StopExistsUptoDate")[0].firstChild.nodeValue);

                    //debugger;
                }

                if (isLineActive) {
                    var name = line.getElementsByTagName("LineDesignation")[0].firstChild.nodeValue;
                    var id = line.getElementsByTagName("LineGid")[0].firstChild.nodeValue;

                    var option = { name: name, value: id };

                    options.push(option);
                }
            }

            fromlist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            tolist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(linelist, options, false);
        },

        getCityFromGid: function (cityGid) {

            var cities = Endeavor.Skanetrafiken.TravelInformation.cities;

            if (cities) {

                for (var i = 0; i < cities.length; i++) {
                    if (cityGid == cities[i].value) {
                        return cities[i].name;
                    }
                }
            }

            return "";
        },

        getStopAreaFromGid: function (areaGid) {

            var stopareas = Endeavor.Skanetrafiken.TravelInformation.stopareas;

            if (stopareas) {

                for (var i = 0; i < stopareas.length; i++) {
                    if (areaGid == stopareas[i].value) {
                        return stopareas[i].name;
                    }
                }
            }

            return "";
        },

        setLine: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var fromlist = document.getElementById("fromlist");
            var fromoptions = [];

            var tolist = document.getElementById("tolist");
            var tooptions = [];

            var cityid = document.getElementById("citylist").value;
            var lineid = document.getElementById("linelist").value;

            var line = Endeavor.Skanetrafiken.TravelInformation.getLine(cityid, lineid);
            Endeavor.Skanetrafiken.TravelInformation.currentLine = line;
            Endeavor.Skanetrafiken.TravelInformation.currentCity = document.getElementById("citylist").value;

            if (line.getElementsByTagName("StopAreas")[0]) {
                var stopareas = line.getElementsByTagName("StopAreas")[0].childNodes;
                for (var i = 0; i < stopareas.length; i++) {

                    var stoparea = stopareas[i];

                    //if (!stoparea.getElementsByTagName("StopExistsUptoDate")[0]) {

                        var name = stoparea.getElementsByTagName("StopAreaName")[0].firstChild.nodeValue;
                        var id = stoparea.getElementsByTagName("StopAreaGid")[0].firstChild.nodeValue;

                        var fromoption = { name: name, value: id };
                        fromoptions.push(fromoption);

                        var tooption = { name: name, value: id };
                        tooptions.push(tooption);
                    //}
                }

                Endeavor.Skanetrafiken.TravelInformation.stopareas = fromoptions;
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(fromlist, fromoptions, true);
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(tolist, tooptions, true);
            }
            else {
                Xrm.Utility.alertDialog("Linjen saknar hållplatser.")
            }
        },

        setFrom: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var transporttype = document.getElementById("transportlist").value;
            if (transporttype == "TRAIN") {

                var fromid = document.getElementById("fromlist").value;

                var tolist = document.getElementById("tolist");
                var options = [];

                var fromstoparea = Endeavor.Skanetrafiken.TravelInformation.getStopArea(fromid);

                var uptostopareas = fromstoparea.getElementsByTagName("UptoStopAreas")[0].childNodes
                for (var i = 0; i < uptostopareas.length; i++) {

                    var stoparea = uptostopareas[i];

                    // CHECK IF STOP HAS ENDED? UNKNOWN NAME TAG
                    var name = stoparea.getElementsByTagName("StopAreaName")[0].firstChild.nodeValue;
                    var id = stoparea.getElementsByTagName("StopAreaGid")[0].firstChild.nodeValue;

                    var tooption = { name: name, value: id };

                    options.push(tooption);

                }

                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(tolist, options, true);
            }
        },

        populateSelectionList: function (list, options, sorting) {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            list.innerHTML = '<option value="" selected disabled hidden >Välj</option>';

            if (sorting) {  // IF TEXT
                // SORTING FUNCTION
                Array.prototype.sortOn = function (key) {
                    this.sort(function (a, b) {
                        if (a[key] < b[key]) {
                            return -1;
                        } else if (a[key] > b[key]) {
                            return 1;
                        }
                        return 0;
                    });
                }

                options.sortOn('name');
            }
            else { // ELSE NUMERICAL
                // SORTING FUNCTION
                Array.prototype.sortOn = function (key) {
                    this.sort(function (a, b) {
                        return a[key] - b[key];
                    });
                }

                options.sortOn('name');
            }

            for (var i = 0; i < options.length; i++) {

                var option = document.createElement("option");
                option.value = options[i].value;
                option.text = options[i].name;

                list.appendChild(option);
            }
        },

        getCity: function (cityid) {

            var response = Endeavor.Skanetrafiken.TravelInformation.response;
            var returncity = null;

            var cities = response.getElementsByTagName("Zones")[0].childNodes;
            for (var i = 0; i < cities.length; i++) {

                var city = cities[i];
                var id = city.getElementsByTagName("ZoneId")[0].firstChild.nodeValue;

                if (id == cityid) {
                    returncity = city;
                }
            }
            return returncity;
        },

        getLine: function (cityid, lineid) {

            var response = Endeavor.Skanetrafiken.TravelInformation.response;
            var returnline = null;

            var city;

            if (cityid) {   // OM CITYID EJ ANGIVET SÅ ANVÄNDS REGION ISTÄLLET...
                city = Endeavor.Skanetrafiken.TravelInformation.getCity(cityid);
            }
            else {
                city = response;
            }

            var lines = city.getElementsByTagName("Lines")[0].childNodes;
            for (var i = 0; i < lines.length; i++) {

                var line = lines[i];
                var id = line.getElementsByTagName("LineGid")[0].firstChild.nodeValue;

                if (id == lineid) {
                    returnline = line;
                }
            }
            return returnline;
        },

        getStopArea: function (stopareaid) {
            var response = Endeavor.Skanetrafiken.TravelInformation.response;
            var returnarea = null;

            var stopareas = response.getElementsByTagName("StopAreas")[0].childNodes;
            for (var i = 0; i < stopareas.length; i++) {

                var stoparea = stopareas[i];
                var id = stoparea.getElementsByTagName("StopAreaGid")[0].firstChild.nodeValue;

                if (id == stopareaid) {
                    returnarea = stoparea;
                }
            }
            return returnarea;
        },

        populateContractorInformation: function (transporttype, city, line) {

            //if ((city == null || city == "") && line) {
            //    alert("Denna typ av sökning är förnärvarande inte funktionell.")
            //    return;
            //}

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var travelinformationbody = document.getElementById("travelinformationbody");
            travelinformationbody.innerHTML = "";

            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                        return elements[0].firstChild.nodeValue;
                    }
                    else {
                        return "-";
                    }
                }
                else {
                    return "-";
                }
            }

            debugger;
            var organisation = Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromLine(city, line);
            var contractor = organisation != "" ? organisation.getAttribute("Name") : "";
            //if (organisation == "" || organisation == null)
            //    return;

            var contractorrow = travelinformationbody.insertRow();
            var cell = contractorrow.insertCell();
            cell.style = "text-align: center";

            var saveEntity = {
                transporttype: transporttype,
                city: city,
                directjourney: null,
                line: line,
                contractorName: contractor
            };


            var savebutton = document.createElement("BUTTON");
            savebutton.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
            savebutton.onclick = Endeavor.Skanetrafiken.TravelInformation.saveFunction(saveEntity); //(transporttype, city, null);
            savebutton.innerHTML = '+';
            cell.appendChild(savebutton);

            cell = contractorrow.insertCell(); //Planerad Avgång
            cell = contractorrow.insertCell(); //Aktuell Avgång
            cell = contractorrow.insertCell(); //Planerad Ankomst
            cell = contractorrow.insertCell(); //Aktuell Ankomst
            cell = contractorrow.insertCell(); //Operatör
            cell.innerHTML = contractor;
        },

        populateTravelInformation: function (transporttype, city, response) {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            directjourneys = response.getElementsByTagName("DirectJourneysBetweenStops");

            var travelinformationbody = document.getElementById("travelinformationbody");
            travelinformationbody.innerHTML = "";

            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                        return elements[0].firstChild.nodeValue;
                    }
                    else {
                        return "-";
                    }
                }
                else {
                    return "-";
                }
            }

            // LOCAL FUNCTION
            getFormattedDate = function (datestring) {

                datestring = datestring.replace("T", " ");

                if (datestring.length > 10) {
                    return datestring.substring(0, 16);
                }
                else {
                    return "-";
                }
            }

            // LOCAL FUNCTION
            getTime = function (datestring) {

                if (datestring.length > 10) {
                    return datestring.substring(11, 16);
                }
                else {
                    return "-";
                }
            }

            debugger;
            for (var i = 0; i < directjourneys.length; i++) {

                var directjourney = directjourneys[i];

                var journeyrow = travelinformationbody.insertRow();

                var cell = journeyrow.insertCell();
                cell.style = "text-align: center";

                var saveEntity = {
                    transporttype: transporttype,
                    city: city,
                    directjourney: directjourney,
                    line: null,
                    contractorName: null
                };

                //draw save button
                var savebutton = document.createElement("BUTTON");
                savebutton.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
                savebutton.onclick = Endeavor.Skanetrafiken.TravelInformation.saveFunction(saveEntity); //(transporttype, city, directjourney);
                savebutton.innerHTML = '+';
                cell.appendChild(savebutton);

                cell = journeyrow.insertCell();
                cell.innerHTML = getFormattedDate(getElementValue(directjourney, "PlannedDepartureDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getTime(getElementValue(directjourney, "ObservedDepartureDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getFormattedDate(getElementValue(directjourney, "PlannedArrivalDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getTime(getElementValue(directjourney, "ObservedArrivalDateTime"));

                var contractorGid = getElementValue(directjourney, "ContractorGid");
                var contractor = Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(contractorGid);
                if (!contractor) {
                    contractor = "-";
                }

                cell = journeyrow.insertCell();
                cell.innerHTML = contractor;

                var journeyNumber = getElementValue(directjourney, "JourneyNumber") + " ";
                var directionOfLineDescription = getElementValue(directjourney, "DirectionOfLineDescription");

                cell = journeyrow.insertCell();
                cell.innerHTML = journeyNumber.concat(directionOfLineDescription);

            }

        },

        //saveFunction: function (transporttype, city, directjourney, line) {
        saveFunction: function (saveEntity) {
            return function () {

                // LOCAL FUNCTION
                getElementValue = function (element, value) {
                    if (element != null) {
                        var elements = element.getElementsByTagName(value);
                        if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                            return elements[0].firstChild.nodeValue;
                        }
                        else {
                            return "-";
                        }
                    }
                    else {
                        return "-";
                    }
                }

                if (!Endeavor.Skanetrafiken.TravelInformation.saveInProgress) {

                    var document = Endeavor.Skanetrafiken.TravelInformation.document;

                    /*
                    var atOperatingDate = getElementValue(directjourney, "PlannedDepartureDateTime");
                    var forServiceJourney = getElementValue(directjourney, "ServiceJourneyGid");
                    var atStopGid = getElementValue(directjourney, "DepartureStopPointGid");

                    var request = new Sdk.ed_GetServiceJourneyRequest(atOperatingDate, forServiceJourney, atStopGid);
                    var response = Sdk.Sync.execute(request);

                    var responsetext = response.getGetServiceJourneyResponse();
                    var parser = new DOMParser();
                    var responsedoc = parser.parseFromString(responsetext, "text/xml");

                    var parsererror = responsedoc.getElementsByTagName('parsererror');
                    var errormessage = responsedoc.getElementsByTagName('ErrorMessage');
                    */

                    var parsererror = null;
                    var errormessage = null;

                    if (parsererror && parsererror.length > 0) {
                        debugger;
                        Xrm.Utility.alertDialog("Contractors service is unavailable. Please contact your systems administrator.");
                    }
                    else if (errormessage && errormessage.length) {
                        debugger;
                        Xrm.Utility.alertDialog(errormessage);
                    }
                    else {

                        var entity = null;

                        debugger;
                        if (saveEntity.transporttype == "TRAIN") {
                            var travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");

                            entity = {
                                cgi_Caseid: {
                                    Id: Xrm.Page.data.entity.getId().substring(1, 37),
                                    LogicalName: "incident",
                                    Name: travelinformation
                                },

                                cgi_travelinformation: travelinformation,
                                cgi_JourneyNumber: getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                cgi_Transport: getElementValue(saveEntity.directjourney, "LineDesignation"),
                                cgi_Tour: getElementValue(saveEntity.directjourney, "LineDesignation"),
                                cgi_linedesignation: getElementValue(saveEntity.directjourney, "PrimaryDestinationName"),
                                cgi_StartPlanned: getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime"),
                                cgi_StartActual: getElementValue(saveEntity.directjourney, "ActualDepartureTime"),
                                cgi_ArivalPlanned: getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime"),
                                cgi_ArivalActual: getElementValue(saveEntity.directjourney, 'ActualArrivalTime'),
                                cgi_DirectionText: getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                                cgi_Start: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                                cgi_Stop: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),
                                cgi_Contractor: Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid")), // NUMERICAL
                                cgi_Deviationmessage: getElementValue(saveEntity.directjourney, "Details"),
                                cgi_DisplayText: ""
                            }

                            if (entity != null)
                                Endeavor.Skanetrafiken.TravelInformation.setDisplayTextTrain(entity);

                        }
                        else if (saveEntity.transporttype == "REGIONBUS") {
                            var travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");


                            if (saveEntity.directjourney == null) {

                                entity = {
                                    cgi_Caseid: {
                                        Id: Xrm.Page.data.entity.getId().substring(1, 37),
                                        LogicalName: "incident",
                                        Name: travelinformation
                                    },

                                    cgi_travelinformation: travelinformation,
                                    cgi_JourneyNumber: "",
                                    cgi_Transport: "Regionbuss",
                                    cgi_City: "",
                                    cgi_Line: getElementValue(Endeavor.Skanetrafiken.TravelInformation.getLine(saveEntity.city, saveEntity.line), "LineNumber"),
                                    cgi_Tour: "",
                                    cgi_linedesignation: "",
                                    cgi_StartPlanned: new Date(1999),
                                    cgi_StartActual: "",
                                    cgi_ArivalPlanned: new Date(1999),
                                    cgi_ArivalActual: "",
                                    cgi_DirectionText: "",
                                    cgi_Start: "",
                                    cgi_Stop: "",
                                    cgi_Contractor: saveEntity.contractorName, // NUMERICAL
                                    cgi_Deviationmessage: "",
                                    cgi_DisplayText: ""
                                }
                            } else {
                                entity = {
                                    cgi_Caseid: {
                                        Id: Xrm.Page.data.entity.getId().substring(1, 37),
                                        LogicalName: "incident",
                                        Name: travelinformation
                                    },

                                    cgi_travelinformation: travelinformation,
                                    cgi_JourneyNumber: getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                    cgi_Tour: getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                    cgi_Transport: "Regionbuss",
                                    cgi_Line: getElementValue(saveEntity.directjourney, "LineDesignation"),
                                    cgi_linedesignation: getElementValue(saveEntity.directjourney, "PrimaryDestinationName"),
                                    cgi_StartPlanned: getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime"),
                                    cgi_StartActual: getElementValue(saveEntity.directjourney, "ActualDepartureTime"),
                                    cgi_ArivalPlanned: getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime"),
                                    cgi_ArivalActual: getElementValue(saveEntity.directjourney, 'ActualArrivalTime'),
                                    cgi_DirectionText: getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                                    cgi_Start: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                                    cgi_Stop: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),
                                    cgi_Contractor: Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid")), // NUMERICAL
                                    cgi_Deviationmessage: getElementValue(saveEntity.directjourney, "Details"),
                                    cgi_DisplayText: ""
                                }
                            }

                            if (entity != null)
                                Endeavor.Skanetrafiken.TravelInformation.setDisplayTextRegionbus(entity);

                        }
                        else if (saveEntity.transporttype == "STRADBUS") {
                            var travelinformation = "";
                            //if (saveEntity.directjourney != null)
                            travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");
                            //else travelinformation = "Stadsbuss";

                            if (saveEntity.directjourney == null) {

                                entity = {
                                    cgi_Caseid: {
                                        Id: Xrm.Page.data.entity.getId().substring(1, 37),
                                        LogicalName: "incident",
                                        Name: travelinformation
                                    },

                                    cgi_travelinformation: travelinformation,
                                    cgi_JourneyNumber: "",
                                    cgi_Transport: "Stadsbuss",
                                    cgi_City: Endeavor.Skanetrafiken.TravelInformation.getCityFromGid(saveEntity.city),
                                    cgi_Line: getElementValue(Endeavor.Skanetrafiken.TravelInformation.getLine(saveEntity.city, saveEntity.line), "LineNumber"),
                                    cgi_Tour: "",
                                    cgi_linedesignation: "",
                                    cgi_StartPlanned: new Date(1999),
                                    cgi_StartActual: "",
                                    cgi_ArivalPlanned: new Date(1999),
                                    cgi_ArivalActual: "",
                                    cgi_DirectionText: "",
                                    cgi_Start: "",
                                    cgi_Stop: "",
                                    cgi_Contractor: saveEntity.contractorName, // NUMERICAL
                                    cgi_Deviationmessage: "",
                                    cgi_DisplayText: ""
                                }

                            } else {
                                entity = {
                                    cgi_Caseid: {
                                        Id: Xrm.Page.data.entity.getId().substring(1, 37),
                                        LogicalName: "incident",
                                        Name: travelinformation
                                    },

                                    cgi_travelinformation: travelinformation,
                                    cgi_JourneyNumber: getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                    cgi_Transport: "Stadsbuss",
                                    cgi_City: Endeavor.Skanetrafiken.TravelInformation.getCityFromGid(saveEntity.city),
                                    cgi_Line: getElementValue(saveEntity.directjourney, "LineDesignation"),
                                    cgi_Tour: getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                    cgi_linedesignation: getElementValue(saveEntity.directjourney, "PrimaryDestinationName"),
                                    cgi_StartPlanned: getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime"),
                                    cgi_StartActual: getElementValue(saveEntity.directjourney, 'ActualDepartureTime'),
                                    cgi_ArivalPlanned: getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime"),
                                    cgi_ArivalActual: getElementValue(saveEntity.directjourney, 'ActualArrivalTime'),
                                    cgi_DirectionText: getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                                    cgi_Start: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                                    cgi_Stop: Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),
                                    cgi_Contractor: Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid")), // NUMERICAL
                                    cgi_Deviationmessage: getElementValue(saveEntity.directjourney, "Details"),
                                    cgi_DisplayText: ""

                                }
                            }

                            if (entity != null)
                                Endeavor.Skanetrafiken.TravelInformation.setDisplayTextCitybus(entity);
                        }

                        if (entity == null)
                            Xrm.Utility.alertDialog("Kunde inte skapa Reseinformation. Vänligen kontakta admin.");

                        /* Do the create*/
                        if (Xrm.Page.getAttribute("statecode") && Xrm.Page.getAttribute("statecode").getValue() == 0) {
                            Endeavor.Skanetrafiken.TravelInformation.saveInProgress = true;
                            SDK.REST.createRecord(entity, "cgi_travelinformation", function (CompletedResponse) {

                                // Xrm.Utility.alertDialog("TravelInformation created!")

                                Endeavor.Skanetrafiken.TravelInformation.populateSavedTravelInformationTable();
                                Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;

                            }, function (errorHandler) {
                                debugger;
                                Xrm.Utility.alertDialog("An error occurred when saving Travel Information: " + errorHandler);
                                Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                            });
                        }
                        else {
                            Xrm.Utility.alertDialog("Kan inte spara resa för avslutat ärende.");
                        }
                    }

                }
            }
        },

        deleteTravelInformation: function (travelinformationid) {
            return function () {

                if (!Endeavor.Skanetrafiken.TravelInformation.saveInProgress) {
                    /* Do the delete*/
                    Endeavor.Skanetrafiken.TravelInformation.saveInProgress = true;
                    SDK.REST.deleteRecord(travelinformationid, "cgi_travelinformation", function (CompletedResponse) {

                        // Xrm.Utility.alertDialog("TravelInformation " + travelinformationid + " deleted!")

                        Endeavor.Skanetrafiken.TravelInformation.populateSavedTravelInformationTable();
                        Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                    }, function (errorHandler) {
                        debugger;
                        Xrm.Utility.alertDialog("An error occurred when deleting Travel Information: " + errorHandler);
                        Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                    });
                }
            }
        },

        populateSavedTravelInformationTable: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var formtype = Xrm.Page.data.entity.getEntityName();
            if (formtype.toUpperCase() == "INCIDENT" && Xrm.Page.ui.getFormType() && Xrm.Page.ui.getFormType() != 1) {

                var cgi_caseid = Xrm.Page.data.entity.getId();
                cgi_caseid = cgi_caseid.substring(1, cgi_caseid.length - 1);

                var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_travelinformationSet?$select=cgi_travelinformationId,cgi_DisplayText,cgi_Deviationmessage&$filter=cgi_Caseid/Id%20eq(guid'" + cgi_caseid + "')";
                var savedtravelsresults = Endeavor.Common.Data.fetchJSONResults(url);

                var travelinformationtable = document.getElementById("savedtravelsbody");
                travelinformationtable.innerHTML = "";

                if (savedtravelsresults) {
                    for (var i = 0; i < savedtravelsresults.length; i++) {

                        var row = travelinformationtable.insertRow();

                        var cell = row.insertCell();
                        var del = document.createElement("BUTTON");
                        del.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
                        cell.onclick = Endeavor.Skanetrafiken.TravelInformation.deleteTravelInformation(savedtravelsresults[i].cgi_travelinformationId);
                        del.innerHTML = "-";
                        cell.appendChild(del);
                        cell.style = "text-align: center";

                        cell = row.insertCell();
                        cell.innerHTML = savedtravelsresults[i].cgi_DisplayText;
                        cell = row.insertCell();
                        cell.innerHTML = savedtravelsresults[i].cgi_Deviationmessage;
                    }
                }
                else {

                }
            }
        },

        getContractors: function () {

            var request = new Sdk.ed_GetContractorsRequest();
            var response = Sdk.Sync.execute(request);

            var responsetext = "<Contractors>" + response.getGetContractorsResponse() + "</Contractors>";
            var parser = new DOMParser();
            var responsedoc = parser.parseFromString(responsetext, "text/xml");

            var parsererror = responsedoc.getElementsByTagName('parsererror');
            var errormessage = responsedoc.getElementsByTagName('ErrorMessage');

            if (parsererror && parsererror.length > 0) {
                debugger;
                Xrm.Utility.alertDialog("Contractors service is unavailable. Please contact your systems administrator.");
            }
            else if (errormessage && errormessage.length) {
                debugger;
                Xrm.Utility.alertDialog(errormessage);
            }
            else {
                Endeavor.Skanetrafiken.TravelInformation.contractors = responsedoc;
            }
        },

        getOrganisationalUnits: function () {

            var request = new Sdk.ed_GetOrganisationalUnitsRequest();
            var response = Sdk.Sync.execute(request);

            var responsetext = "<Organisations>" + response.getGetOrganisationalUnitsResponse() + "</Organisations>";
            var parser = new DOMParser();
            var responsedoc = parser.parseFromString(responsetext, "text/xml");

            var parsererror = responsedoc.getElementsByTagName('parsererror');
            var errormessage = responsedoc.getElementsByTagName('ErrorMessage');

            if (parsererror && parsererror.length > 0) {
                debugger;
                Xrm.Utility.alertDialog("Organisations service is unavailable. Please contact your systems administrator.");
            }
            else if (errormessage && errormessage.length) {
                debugger;
                Xrm.Utility.alertDialog(errormessage);
            }
            else {
                Endeavor.Skanetrafiken.TravelInformation.organisations = responsedoc;
            }
        },

        setDisplayTextTrain: function (entity) {

            debugger;

            var tour = "Linje: " + entity.cgi_JourneyNumber;

            var startplanned = entity.cgi_StartPlanned;
            if (startplanned && startplanned.length > 16) {
                startplanned = startplanned.substring(0, 16).replace("T", " ");
            }
            var arrivalplanned = entity.cgi_ArivalPlanned;
            if (arrivalplanned && arrivalplanned.length > 16) {
                arrivalplanned = arrivalplanned.substring(0, 16).replace("T", " ");
            }
            var startactual = entity.cgi_StartActual;
            if (startactual && startactual.length > 16) {
                startactual = startactual.substring(0, 16).replace("T", " ");
            }
            var arrivalactual = entity.cgi_ArivalActual;
            if (arrivalactual && arrivalactual.length > 16) {
                arrivalactual = arrivalactual.substring(0, 16).replace("T", " ");
            }

            var line = "Tur: " + entity.cgi_JourneyNumber + " " + startplanned + " [" + startactual + "] " + entity.cgi_Start + " - " + arrivalplanned + " [" + arrivalactual + "] " + entity.cgi_Stop;
            var contractor = "Entreprenör: " + entity.cgi_Contractor;

            entity.cgi_DisplayText = tour + " " + line + " " + contractor;
        },

        setDisplayTextRegionbus: function (entity) {

            debugger;

            var line = Endeavor.Skanetrafiken.TravelInformation.currentLine;

            var trafik = "Trafikslag: " + line.getElementsByTagName("LineName")[0].firstChild.nodeValue;
            var tour = "Linje: " + entity.cgi_Transport + " [" + entity.cgi_JourneyNumber + "] (" + entity.cgi_DirectionText + ")";

            var startplanned = entity.cgi_StartPlanned;
            if (startplanned && startplanned.length > 16) {
                startplanned = startplanned.substring(0, 16).replace("T", " ");
            }
            var arrivalplanned = entity.cgi_ArivalPlanned;
            if (arrivalplanned && arrivalplanned.length > 16) {
                arrivalplanned = arrivalplanned.substring(0, 16).replace("T", " ");
            }
            var startactual = entity.cgi_StartActual;
            if (startactual && startactual.length > 16) {
                startactual = startactual.substring(0, 16).replace("T", " ");
            }
            var arrivalactual = entity.cgi_ArivalActual;
            if (arrivalactual && arrivalactual.length > 16) {
                arrivalactual = arrivalactual.substring(0, 16).replace("T", " ");
            }

            var line = "Tur: " + entity.cgi_JourneyNumber + " " + startplanned + " [" + startactual + "] " + entity.cgi_Start + " - " + arrivalplanned + " [" + arrivalactual + "] " + entity.cgi_Stop;
            var contractor = "Entreprenör: " + entity.cgi_Contractor;

            entity.cgi_DisplayText = trafik + " " + tour + " " + line + " " + contractor;
        },

        setDisplayTextCitybus: function (entity) {
            debugger;

            var line = Endeavor.Skanetrafiken.TravelInformation.currentLine;

            var trafik = "Trafikslag: " + line.getElementsByTagName("LineName")[0].firstChild.nodeValue;
            var city = "Stad: " + entity.cgi_City;
            var tour = "Linje: " + entity.cgi_Tour + " [" + entity.cgi_JourneyNumber + "] (" + entity.cgi_DirectionText + ")";

            var startplanned = entity.cgi_StartPlanned;
            if (startplanned && startplanned.length > 16) {
                startplanned = startplanned.substring(0, 16).replace("T", " ");
            }
            var arrivalplanned = entity.cgi_ArivalPlanned;
            if (arrivalplanned && arrivalplanned.length > 16) {
                arrivalplanned = arrivalplanned.substring(0, 16).replace("T", " ");
            }
            var startactual = entity.cgi_StartActual;
            if (startactual && startactual.length > 16) {
                startactual = startactual.substring(0, 16).replace("T", " ");
            }
            var arrivalactual = entity.cgi_ArivalActual;
            if (arrivalactual && arrivalactual.length > 16) {
                arrivalactual = arrivalactual.substring(0, 16).replace("T", " ");
            }

            var line = "Tur: " + entity.cgi_JourneyNumber + " " + startplanned + " [" + startactual + "] " + entity.cgi_Start + " - " + arrivalplanned + " [" + arrivalactual + "] " + entity.cgi_Stop;
            var contractor = "Entreprenör: " + entity.cgi_Contractor;

            entity.cgi_DisplayText = trafik + " " + city + " " + tour + " " + line + " " + contractor;
        },

        getCaseEventDate: function (timestamp_label) {
            try {

                var formType = Xrm.Page.ui.getFormType();
                if (formType == 1)
                    return;

                var handelseDatumAttr = Xrm.Page.getAttribute("cgi_handelsedatum");
                var handelseDatumVal = null;

                var actionDatumAttr = Xrm.Page.getAttribute("cgi_actiondate");
                var actionDatumVal = null;

                var arrivalAttr = Xrm.Page.getAttribute("cgi_arrival_date");
                var arrivalVal = null;

                if (handelseDatumAttr)
                    handelseDatumVal = handelseDatumAttr.getValue();
                if (actionDatumAttr)
                    actionDatumVal = actionDatumAttr.getValue();
                if (arrivalAttr)
                    arrivalVal = arrivalAttr.getValue();
                else {
                    timestamp_label.value = new Date();
                    return;
                }

                debugger;

                var year = null;
                var month = null;
                var day = null;
                var hour = null;
                var minute = null;
                var dateTime = null;

                if (handelseDatumVal != null && handelseDatumVal != "") {
                    handelseDatumVal = handelseDatumVal.replace(" ", "");
                    handelseDatumVal = handelseDatumVal.replace("-", "");
                    handelseDatumVal = handelseDatumVal.replace("kl", "");

                    if (isNaN(handelseDatumVal)) {
                        console.log("Handelsedatum is not a number.");
                        dateTime = new Date();
                    }

                    //Ex 20190105
                    if (handelseDatumVal.length == 8) {
                        year = handelseDatumVal.substring(0, 4);
                        month = handelseDatumVal.substring(4, 6);
                        day = handelseDatumVal.substring(6, 8);
                        dateTime = new Date(year, month, day);
                    }

                    //Ex 201901051424
                    else if (handelseDatumVal.length == 12) {
                        year = handelseDatumVal.substring(0, 4);
                        month = handelseDatumVal.substring(4, 6);
                        day = handelseDatumVal.substring(6, 8);
                        hour = handelseDatumVal.substring(8, 10);
                        minute = handelseDatumVal.substring(10, 12);

                        //In JS month start with index 0.
                        var parsedMonth = parseInt(month);
                        parsedMonth--;
                        month = "0" + parsedMonth;

                        dateTime = new Date(year, month, day, hour, minute);
                    }

                    //Ex 1901051424
                    else if (handelseDatumVal.length == 10) {
                        year = handelseDatumVal.substring(0, 2);
                        month = handelseDatumVal.substring(2, 4);
                        day = handelseDatumVal.substring(4, 6);
                        hour = handelseDatumVal.substring(6, 8);
                        minute = handelseDatumVal.substring(8, 10);

                        //In JS month start with index 0.
                        var parsedMonth = parseInt(month);
                        parsedMonth--;
                        month = "0" + parsedMonth;

                        dateTime = new Date(year, month, day, hour, minute);
                    }
                }

                else if (actionDatumVal != null && actionDatumVal != "") {
                    dateTime = actionDatumVal;
                }

                else if (arrivalVal != null && arrivalVal != "") {
                    dateTime = arrivalVal;
                }

                else
                    dateTime = new Date();

                timestamp_label.value = dateTime;

            } catch (e) {
                Xrm.Utility.alertDialog("Exception caught in Endeavor.Skanetrafiken.TravelInformation.getCaseActionDate. Error: " + e.message);
            }
        },

    }
}

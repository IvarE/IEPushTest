"use strict";
(function () {
    this.ed_GetDirectJourneysRequest = function (n, t, i, r, u) {
        function l(n) {
            if (typeof n == "string") e = n;
            else throw new Error("Sdk.ed_GetDirectJourneysRequest TransportType property is required and must be a String.");
        }
        function a(n) {
            if (typeof n == "string") o = n;
            else throw new Error("Sdk.ed_GetDirectJourneysRequest FromStopAreaGid property is required and must be a String.");
        }
        function v(n) {
            if (typeof n == "string") s = n;
            else throw new Error("Sdk.ed_GetDirectJourneysRequest ToStopAreaGid property is required and must be a String.");
        }
        function y(n) {
            if (typeof n == "string") h = n;
            else throw new Error("Sdk.ed_GetDirectJourneysRequest TripDateTime property is required and must be a String.");
        }
        function p(n) {
            if (typeof n == "string") c = n;
            else throw new Error("Sdk.ed_GetDirectJourneysRequest ForLineGids property is required and must be a String.");
        }
        function f() {
            return [
                "<d:request>",
                "<a:Parameters>",
                "<a:KeyValuePairOfstringanyType>",
                "<b:key>TransportType</b:key>",
                e == null ? '<b:value i:nil="true" />' : ['<b:value i:type="c:string">', e, "</b:value>"].join(""),
                "</a:KeyValuePairOfstringanyType>",
                "<a:KeyValuePairOfstringanyType>",
                "<b:key>FromStopAreaGid</b:key>",
                o == null ? '<b:value i:nil="true" />' : ['<b:value i:type="c:string">', o, "</b:value>"].join(""),
                "</a:KeyValuePairOfstringanyType>",
                "<a:KeyValuePairOfstringanyType>",
                "<b:key>ToStopAreaGid</b:key>",
                s == null ? '<b:value i:nil="true" />' : ['<b:value i:type="c:string">', s, "</b:value>"].join(""),
                "</a:KeyValuePairOfstringanyType>",
                "<a:KeyValuePairOfstringanyType>",
                "<b:key>TripDateTime</b:key>",
                h == null ? '<b:value i:nil="true" />' : ['<b:value i:type="c:string">', h, "</b:value>"].join(""),
                "</a:KeyValuePairOfstringanyType>",
                "<a:KeyValuePairOfstringanyType>",
                "<b:key>ForLineGids</b:key>",
                c == null ? '<b:value i:nil="true" />' : ['<b:value i:type="c:string">', c, "</b:value>"].join(""),
                "</a:KeyValuePairOfstringanyType>",
                "</a:Parameters>",
                '<a:RequestId i:nil="true" />',
                "<a:RequestName>ed_GetDirectJourneys</a:RequestName>",
                "</d:request>",
            ].join("");
        }
        if (!(this instanceof Sdk.ed_GetDirectJourneysRequest)) return new Sdk.ed_GetDirectJourneysRequest(n, t, i, r, u);
        Sdk.OrganizationRequest.call(this);
        var e = null,
            o = null,
            s = null,
            h = null,
            c = null;
        typeof n != "undefined" && l(n);
        typeof t != "undefined" && a(t);
        typeof i != "undefined" && v(i);
        typeof r != "undefined" && y(r);
        typeof u != "undefined" && p(u);
        this.setResponseType(Sdk.ed_GetDirectJourneysResponse);
        this.setRequestXml(f());
        this.setTransportType = function (n) {
            l(n);
            this.setRequestXml(f());
        };
        this.setFromStopAreaGid = function (n) {
            a(n);
            this.setRequestXml(f());
        };
        this.setToStopAreaGid = function (n) {
            v(n);
            this.setRequestXml(f());
        };
        this.setTripDateTime = function (n) {
            y(n);
            this.setRequestXml(f());
        };
        this.setForLineGids = function (n) {
            p(n);
            this.setRequestXml(f());
        };
    };
    this.ed_GetDirectJourneysRequest.__class = !0;
    this.ed_GetDirectJourneysResponse = function (n) {
        function i(n) {
            var i = Sdk.Xml.selectSingleNode(n, "//a:KeyValuePairOfstringanyType[b:key='DirectJourneysResponse']/b:value");
            Sdk.Xml.isNodeNull(i) || (t = Sdk.Xml.getNodeText(i));
        }
        if (!(this instanceof Sdk.ed_GetDirectJourneysResponse)) return new Sdk.ed_GetDirectJourneysResponse(n);
        Sdk.OrganizationResponse.call(this);
        var t = null;
        this.getDirectJourneysResponse = function () {
            return t;
        };
        i(n);
    };
    this.ed_GetDirectJourneysResponse.__class = !0;
}.call(Sdk));
Sdk.ed_GetDirectJourneysRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetDirectJourneysResponse.prototype = new Sdk.OrganizationResponse();

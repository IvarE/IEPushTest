"use strict";
(function () {
this.ed_GetDirectJourneysRequest = function (
transportType,
fromStopAreaGid,
toStopAreaGid,
tripDateTime,
forLineGids
)
{
///<summary>
/// 
///</summary>
///<param name="transportType"  type="String">
/// [Add Description]
///</param>
///<param name="fromStopAreaGid"  type="String">
/// [Add Description]
///</param>
///<param name="toStopAreaGid"  type="String">
/// [Add Description]
///</param>
///<param name="tripDateTime"  type="String">
/// [Add Description]
///</param>
///<param name="forLineGids"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetDirectJourneysRequest)) {
return new Sdk.ed_GetDirectJourneysRequest(transportType, fromStopAreaGid, toStopAreaGid, tripDateTime, forLineGids);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _TransportType = null;
var _FromStopAreaGid = null;
var _ToStopAreaGid = null;
var _TripDateTime = null;
var _ForLineGids = null;

// internal validation functions

function _setValidTransportType(value) {
 if (typeof value == "string") {
  _TransportType = value;
 }
 else {
  throw new Error("Sdk.ed_GetDirectJourneysRequest TransportType property is required and must be a String.")
 }
}

function _setValidFromStopAreaGid(value) {
 if (typeof value == "string") {
  _FromStopAreaGid = value;
 }
 else {
  throw new Error("Sdk.ed_GetDirectJourneysRequest FromStopAreaGid property is required and must be a String.")
 }
}

function _setValidToStopAreaGid(value) {
 if (typeof value == "string") {
  _ToStopAreaGid = value;
 }
 else {
  throw new Error("Sdk.ed_GetDirectJourneysRequest ToStopAreaGid property is required and must be a String.")
 }
}

function _setValidTripDateTime(value) {
 if (typeof value == "string") {
  _TripDateTime = value;
 }
 else {
  throw new Error("Sdk.ed_GetDirectJourneysRequest TripDateTime property is required and must be a String.")
 }
}

function _setValidForLineGids(value) {
 if (typeof value == "string") {
  _ForLineGids = value;
 }
 else {
  throw new Error("Sdk.ed_GetDirectJourneysRequest ForLineGids property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof transportType != "undefined") {
   _setValidTransportType(transportType);
  }
  if (typeof fromStopAreaGid != "undefined") {
   _setValidFromStopAreaGid(fromStopAreaGid);
  }
  if (typeof toStopAreaGid != "undefined") {
   _setValidToStopAreaGid(toStopAreaGid);
  }
  if (typeof tripDateTime != "undefined") {
   _setValidTripDateTime(tripDateTime);
  }
  if (typeof forLineGids != "undefined") {
   _setValidForLineGids(forLineGids);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>TransportType</b:key>",
           (_TransportType == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _TransportType, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>FromStopAreaGid</b:key>",
           (_FromStopAreaGid == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _FromStopAreaGid, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ToStopAreaGid</b:key>",
           (_ToStopAreaGid == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ToStopAreaGid, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>TripDateTime</b:key>",
           (_TripDateTime == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _TripDateTime, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ForLineGids</b:key>",
           (_ForLineGids == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ForLineGids, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetDirectJourneys</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetDirectJourneysResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setTransportType = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidTransportType(value);
   this.setRequestXml(getRequestXml());
  }

  this.setFromStopAreaGid = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidFromStopAreaGid(value);
   this.setRequestXml(getRequestXml());
  }

  this.setToStopAreaGid = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidToStopAreaGid(value);
   this.setRequestXml(getRequestXml());
  }

  this.setTripDateTime = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidTripDateTime(value);
   this.setRequestXml(getRequestXml());
  }

  this.setForLineGids = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidForLineGids(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetDirectJourneysRequest.__class = true;

this.ed_GetDirectJourneysResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetDirectJourneysRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetDirectJourneysResponse)) {
   return new Sdk.ed_GetDirectJourneysResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _directJourneysResponse = null;

  // Internal property setter functions

  function _setDirectJourneysResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='DirectJourneysResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _directJourneysResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getDirectJourneysResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _directJourneysResponse;
  }

  //Set property values from responseXml constructor parameter
  _setDirectJourneysResponse(responseXml);
 }
 this.ed_GetDirectJourneysResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetDirectJourneysRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetDirectJourneysResponse.prototype = new Sdk.OrganizationResponse();

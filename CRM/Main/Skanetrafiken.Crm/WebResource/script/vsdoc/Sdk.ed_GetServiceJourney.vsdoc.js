"use strict";
(function () {
this.ed_GetServiceJourneyRequest = function (
atOperatingDate,
forServiceJourney,
atStopGid
)
{
///<summary>
/// 
///</summary>
///<param name="atOperatingDate"  type="String">
/// [Add Description]
///</param>
///<param name="forServiceJourney"  type="String">
/// [Add Description]
///</param>
///<param name="atStopGid"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetServiceJourneyRequest)) {
return new Sdk.ed_GetServiceJourneyRequest(atOperatingDate, forServiceJourney, atStopGid);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _AtOperatingDate = null;
var _ForServiceJourney = null;
var _AtStopGid = null;

// internal validation functions

function _setValidAtOperatingDate(value) {
 if (typeof value == "string") {
  _AtOperatingDate = value;
 }
 else {
  throw new Error("Sdk.ed_GetServiceJourneyRequest AtOperatingDate property is required and must be a String.")
 }
}

function _setValidForServiceJourney(value) {
 if (typeof value == "string") {
  _ForServiceJourney = value;
 }
 else {
  throw new Error("Sdk.ed_GetServiceJourneyRequest ForServiceJourney property is required and must be a String.")
 }
}

function _setValidAtStopGid(value) {
 if (typeof value == "string") {
  _AtStopGid = value;
 }
 else {
  throw new Error("Sdk.ed_GetServiceJourneyRequest AtStopGid property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof atOperatingDate != "undefined") {
   _setValidAtOperatingDate(atOperatingDate);
  }
  if (typeof forServiceJourney != "undefined") {
   _setValidForServiceJourney(forServiceJourney);
  }
  if (typeof atStopGid != "undefined") {
   _setValidAtStopGid(atStopGid);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>AtOperatingDate</b:key>",
           (_AtOperatingDate == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _AtOperatingDate, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ForServiceJourney</b:key>",
           (_ForServiceJourney == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ForServiceJourney, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>AtStopGid</b:key>",
           (_AtStopGid == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _AtStopGid, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetServiceJourney</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetServiceJourneyResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setAtOperatingDate = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidAtOperatingDate(value);
   this.setRequestXml(getRequestXml());
  }

  this.setForServiceJourney = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidForServiceJourney(value);
   this.setRequestXml(getRequestXml());
  }

  this.setAtStopGid = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidAtStopGid(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetServiceJourneyRequest.__class = true;

this.ed_GetServiceJourneyResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetServiceJourneyRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetServiceJourneyResponse)) {
   return new Sdk.ed_GetServiceJourneyResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getServiceJourneyResponse = null;

  // Internal property setter functions

  function _setGetServiceJourneyResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetServiceJourneyResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getServiceJourneyResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetServiceJourneyResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getServiceJourneyResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetServiceJourneyResponse(responseXml);
 }
 this.ed_GetServiceJourneyResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetServiceJourneyRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetServiceJourneyResponse.prototype = new Sdk.OrganizationResponse();

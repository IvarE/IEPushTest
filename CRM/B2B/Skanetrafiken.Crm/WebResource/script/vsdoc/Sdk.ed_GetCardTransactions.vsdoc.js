"use strict";
(function () {
this.ed_GetCardTransactionsRequest = function (
travelCardNumber,
maxTransactions,
dateFrom,
dateTo
)
{
///<summary>
/// 
///</summary>
///<param name="travelCardNumber"  type="String">
/// [Add Description]
///</param>
///<param name="maxTransactions"  type="String">
/// [Add Description]
///</param>
///<param name="dateFrom"  type="String">
/// [Add Description]
///</param>
///<param name="dateTo"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetCardTransactionsRequest)) {
return new Sdk.ed_GetCardTransactionsRequest(travelCardNumber, maxTransactions, dateFrom, dateTo);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _TravelCardNumber = null;
var _MaxTransactions = null;
var _DateFrom = null;
var _DateTo = null;

// internal validation functions

function _setValidTravelCardNumber(value) {
 if (typeof value == "string") {
  _TravelCardNumber = value;
 }
 else {
  throw new Error("Sdk.ed_GetCardTransactionsRequest TravelCardNumber property is required and must be a String.")
 }
}

function _setValidMaxTransactions(value) {
 if (typeof value == "string") {
  _MaxTransactions = value;
 }
 else {
  throw new Error("Sdk.ed_GetCardTransactionsRequest MaxTransactions property is required and must be a String.")
 }
}

function _setValidDateFrom(value) {
 if (typeof value == "string") {
  _DateFrom = value;
 }
 else {
  throw new Error("Sdk.ed_GetCardTransactionsRequest DateFrom property is required and must be a String.")
 }
}

function _setValidDateTo(value) {
 if (typeof value == "string") {
  _DateTo = value;
 }
 else {
  throw new Error("Sdk.ed_GetCardTransactionsRequest DateTo property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof travelCardNumber != "undefined") {
   _setValidTravelCardNumber(travelCardNumber);
  }
  if (typeof maxTransactions != "undefined") {
   _setValidMaxTransactions(maxTransactions);
  }
  if (typeof dateFrom != "undefined") {
   _setValidDateFrom(dateFrom);
  }
  if (typeof dateTo != "undefined") {
   _setValidDateTo(dateTo);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>TravelCardNumber</b:key>",
           (_TravelCardNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _TravelCardNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>MaxTransactions</b:key>",
           (_MaxTransactions == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _MaxTransactions, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>DateFrom</b:key>",
           (_DateFrom == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _DateFrom, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>DateTo</b:key>",
           (_DateTo == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _DateTo, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetCardTransactions</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetCardTransactionsResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setTravelCardNumber = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidTravelCardNumber(value);
   this.setRequestXml(getRequestXml());
  }

  this.setMaxTransactions = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidMaxTransactions(value);
   this.setRequestXml(getRequestXml());
  }

  this.setDateFrom = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidDateFrom(value);
   this.setRequestXml(getRequestXml());
  }

  this.setDateTo = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidDateTo(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetCardTransactionsRequest.__class = true;

this.ed_GetCardTransactionsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetCardTransactionsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetCardTransactionsResponse)) {
   return new Sdk.ed_GetCardTransactionsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _cardTransactionsResponse = null;

  // Internal property setter functions

  function _setCardTransactionsResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='CardTransactionsResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _cardTransactionsResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getCardTransactionsResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _cardTransactionsResponse;
  }

  //Set property values from responseXml constructor parameter
  _setCardTransactionsResponse(responseXml);
 }
 this.ed_GetCardTransactionsResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetCardTransactionsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetCardTransactionsResponse.prototype = new Sdk.OrganizationResponse();

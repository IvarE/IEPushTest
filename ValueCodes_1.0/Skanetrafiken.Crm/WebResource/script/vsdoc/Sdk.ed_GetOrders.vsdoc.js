"use strict";
(function () {
this.ed_GetOrdersRequest = function (
emailAddress,
cardNumber,
orderNumber,
startDate,
endDate
)
{
///<summary>
/// 
///</summary>
///<param name="emailAddress"  type="String">
/// [Add Description]
///</param>
///<param name="cardNumber"  type="String">
/// [Add Description]
///</param>
///<param name="orderNumber"  type="String">
/// [Add Description]
///</param>
///<param name="startDate"  type="String">
/// [Add Description]
///</param>
///<param name="endDate"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetOrdersRequest)) {
return new Sdk.ed_GetOrdersRequest(emailAddress, cardNumber, orderNumber, startDate, endDate);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _EmailAddress = null;
var _CardNumber = null;
var _OrderNumber = null;
var _StartDate = null;
var _EndDate = null;

// internal validation functions

function _setValidEmailAddress(value) {
 if (typeof value == "string") {
  _EmailAddress = value;
 }
 else {
  throw new Error("Sdk.ed_GetOrdersRequest EmailAddress property is required and must be a String.")
 }
}

function _setValidCardNumber(value) {
 if (typeof value == "string") {
  _CardNumber = value;
 }
 else {
  throw new Error("Sdk.ed_GetOrdersRequest CardNumber property is required and must be a String.")
 }
}

function _setValidOrderNumber(value) {
 if (typeof value == "string") {
  _OrderNumber = value;
 }
 else {
  throw new Error("Sdk.ed_GetOrdersRequest OrderNumber property is required and must be a String.")
 }
}

function _setValidStartDate(value) {
 if (typeof value == "string") {
  _StartDate = value;
 }
 else {
  throw new Error("Sdk.ed_GetOrdersRequest StartDate property is required and must be a String.")
 }
}

function _setValidEndDate(value) {
 if (typeof value == "string") {
  _EndDate = value;
 }
 else {
  throw new Error("Sdk.ed_GetOrdersRequest EndDate property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof emailAddress != "undefined") {
   _setValidEmailAddress(emailAddress);
  }
  if (typeof cardNumber != "undefined") {
   _setValidCardNumber(cardNumber);
  }
  if (typeof orderNumber != "undefined") {
   _setValidOrderNumber(orderNumber);
  }
  if (typeof startDate != "undefined") {
   _setValidStartDate(startDate);
  }
  if (typeof endDate != "undefined") {
   _setValidEndDate(endDate);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>EmailAddress</b:key>",
           (_EmailAddress == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _EmailAddress, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>CardNumber</b:key>",
           (_CardNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _CardNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>OrderNumber</b:key>",
           (_OrderNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _OrderNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>StartDate</b:key>",
           (_StartDate == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _StartDate, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>EndDate</b:key>",
           (_EndDate == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _EndDate, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetOrders</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetOrdersResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setEmailAddress = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidEmailAddress(value);
   this.setRequestXml(getRequestXml());
  }

  this.setCardNumber = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidCardNumber(value);
   this.setRequestXml(getRequestXml());
  }

  this.setOrderNumber = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidOrderNumber(value);
   this.setRequestXml(getRequestXml());
  }

  this.setStartDate = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidStartDate(value);
   this.setRequestXml(getRequestXml());
  }

  this.setEndDate = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidEndDate(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetOrdersRequest.__class = true;

this.ed_GetOrdersResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetOrdersRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetOrdersResponse)) {
   return new Sdk.ed_GetOrdersResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getOrdersResponse = null;

  // Internal property setter functions

  function _setGetOrdersResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetOrdersResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getOrdersResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetOrdersResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getOrdersResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetOrdersResponse(responseXml);
 }
 this.ed_GetOrdersResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetOrdersRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetOrdersResponse.prototype = new Sdk.OrganizationResponse();

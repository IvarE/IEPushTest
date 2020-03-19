"use strict";
(function () {
this.ed_CreateCreditOrderRequest = function (
orderNumber,
productNumber,
credit,
reason,
quantity
)
{
///<summary>
/// 
///</summary>
///<param name="orderNumber"  type="String">
/// [Add Description]
///</param>
///<param name="productNumber"  type="String">
/// [Add Description]
///</param>
///<param name="credit"  type="String">
/// [Add Description]
///</param>
///<param name="reason"  type="String">
/// [Add Description]
///</param>
///<param name="quantity"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_CreateCreditOrderRequest)) {
return new Sdk.ed_CreateCreditOrderRequest(orderNumber, productNumber, credit, reason, quantity);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _OrderNumber = null;
var _ProductNumber = null;
var _Credit = null;
var _Reason = null;
var _Quantity = null;

// internal validation functions

function _setValidOrderNumber(value) {
 if (typeof value == "string") {
  _OrderNumber = value;
 }
 else {
  throw new Error("Sdk.ed_CreateCreditOrderRequest OrderNumber property is required and must be a String.")
 }
}

function _setValidProductNumber(value) {
 if (typeof value == "string") {
  _ProductNumber = value;
 }
 else {
  throw new Error("Sdk.ed_CreateCreditOrderRequest ProductNumber property is required and must be a String.")
 }
}

function _setValidCredit(value) {
 if (typeof value == "string") {
  _Credit = value;
 }
 else {
  throw new Error("Sdk.ed_CreateCreditOrderRequest Credit property is required and must be a String.")
 }
}

function _setValidReason(value) {
 if (typeof value == "string") {
  _Reason = value;
 }
 else {
  throw new Error("Sdk.ed_CreateCreditOrderRequest Reason property is required and must be a String.")
 }
}

function _setValidQuantity(value) {
 if (typeof value == "string") {
  _Quantity = value;
 }
 else {
  throw new Error("Sdk.ed_CreateCreditOrderRequest Quantity property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof orderNumber != "undefined") {
   _setValidOrderNumber(orderNumber);
  }
  if (typeof productNumber != "undefined") {
   _setValidProductNumber(productNumber);
  }
  if (typeof credit != "undefined") {
   _setValidCredit(credit);
  }
  if (typeof reason != "undefined") {
   _setValidReason(reason);
  }
  if (typeof quantity != "undefined") {
   _setValidQuantity(quantity);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>OrderNumber</b:key>",
           (_OrderNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _OrderNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ProductNumber</b:key>",
           (_ProductNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ProductNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Credit</b:key>",
           (_Credit == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _Credit, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Reason</b:key>",
           (_Reason == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _Reason, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Quantity</b:key>",
           (_Quantity == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _Quantity, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_CreateCreditOrder</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_CreateCreditOrderResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
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

  this.setProductNumber = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidProductNumber(value);
   this.setRequestXml(getRequestXml());
  }

  this.setCredit = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidCredit(value);
   this.setRequestXml(getRequestXml());
  }

  this.setReason = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidReason(value);
   this.setRequestXml(getRequestXml());
  }

  this.setQuantity = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidQuantity(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_CreateCreditOrderRequest.__class = true;

this.ed_CreateCreditOrderResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_CreateCreditOrderRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_CreateCreditOrderResponse)) {
   return new Sdk.ed_CreateCreditOrderResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _creditOrderResponse = null;

  // Internal property setter functions

  function _setCreditOrderResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='CreditOrderResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _creditOrderResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getCreditOrderResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _creditOrderResponse;
  }

  //Set property values from responseXml constructor parameter
  _setCreditOrderResponse(responseXml);
 }
 this.ed_CreateCreditOrderResponse.__class = true;
}).call(Sdk)

Sdk.ed_CreateCreditOrderRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_CreateCreditOrderResponse.prototype = new Sdk.OrganizationResponse();

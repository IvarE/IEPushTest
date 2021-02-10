"use strict";
(function () {
this.ed_RechargeCardRequest = function (
travelCardNumber
)
{
///<summary>
/// 
///</summary>
///<param name="travelCardNumber"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_RechargeCardRequest)) {
return new Sdk.ed_RechargeCardRequest(travelCardNumber);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _TravelCardNumber = null;

// internal validation functions

function _setValidTravelCardNumber(value) {
 if (typeof value == "string") {
  _TravelCardNumber = value;
 }
 else {
  throw new Error("Sdk.ed_RechargeCardRequest TravelCardNumber property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof travelCardNumber != "undefined") {
   _setValidTravelCardNumber(travelCardNumber);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>TravelCardNumber</b:key>",
           (_TravelCardNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _TravelCardNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_RechargeCard</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_RechargeCardResponse);
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

 }
 this.ed_RechargeCardRequest.__class = true;

this.ed_RechargeCardResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_RechargeCardRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_RechargeCardResponse)) {
   return new Sdk.ed_RechargeCardResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _rechargeCardResponse = null;

  // Internal property setter functions

  function _setRechargeCardResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='RechargeCardResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _rechargeCardResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getRechargeCardResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _rechargeCardResponse;
  }

  //Set property values from responseXml constructor parameter
  _setRechargeCardResponse(responseXml);
 }
 this.ed_RechargeCardResponse.__class = true;
}).call(Sdk)

Sdk.ed_RechargeCardRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_RechargeCardResponse.prototype = new Sdk.OrganizationResponse();

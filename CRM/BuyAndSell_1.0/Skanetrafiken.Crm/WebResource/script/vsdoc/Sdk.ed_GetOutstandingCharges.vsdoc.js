"use strict";
(function () {
this.ed_GetOutstandingChargesRequest = function (
travelCardNumber
)
{
///<summary>
/// 
///</summary>
///<param name="travelCardNumber"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetOutstandingChargesRequest)) {
return new Sdk.ed_GetOutstandingChargesRequest(travelCardNumber);
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
  throw new Error("Sdk.ed_GetOutstandingChargesRequest TravelCardNumber property is required and must be a String.")
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
        "<a:RequestName>ed_GetOutstandingCharges</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetOutstandingChargesResponse);
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
 this.ed_GetOutstandingChargesRequest.__class = true;

this.ed_GetOutstandingChargesResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetOutstandingChargesRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetOutstandingChargesResponse)) {
   return new Sdk.ed_GetOutstandingChargesResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getOutstandingChargesResponse = null;

  // Internal property setter functions

  function _setGetOutstandingChargesResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetOutstandingChargesResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getOutstandingChargesResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetOutstandingChargesResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getOutstandingChargesResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetOutstandingChargesResponse(responseXml);
 }
 this.ed_GetOutstandingChargesResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetOutstandingChargesRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetOutstandingChargesResponse.prototype = new Sdk.OrganizationResponse();

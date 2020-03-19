"use strict";
(function () {
this.ed_GetCardDetailsRequest = function (
travelCardNumber
)
{
///<summary>
/// 
///</summary>
///<param name="travelCardNumber"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetCardDetailsRequest)) {
return new Sdk.ed_GetCardDetailsRequest(travelCardNumber);
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
  throw new Error("Sdk.ed_GetCardDetailsRequest TravelCardNumber property is required and must be a String.")
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
        "<a:RequestName>ed_GetCardDetails</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetCardDetailsResponse);
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
 this.ed_GetCardDetailsRequest.__class = true;

this.ed_GetCardDetailsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetCardDetailsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetCardDetailsResponse)) {
   return new Sdk.ed_GetCardDetailsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _cardDetailsResponse = null;

  // Internal property setter functions

  function _setCardDetailsResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='CardDetailsResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _cardDetailsResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getCardDetailsResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _cardDetailsResponse;
  }

  //Set property values from responseXml constructor parameter
  _setCardDetailsResponse(responseXml);
 }
 this.ed_GetCardDetailsResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetCardDetailsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetCardDetailsResponse.prototype = new Sdk.OrganizationResponse();

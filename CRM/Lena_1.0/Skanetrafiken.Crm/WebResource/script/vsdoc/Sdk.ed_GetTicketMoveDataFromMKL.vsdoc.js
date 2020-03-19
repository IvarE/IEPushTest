"use strict";
(function () {
this.ed_GetTicketMoveDataFromMKLRequest = function (
contactGuid
)
{
///<summary>
/// 
///</summary>
///<param name="contactGuid"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetTicketMoveDataFromMKLRequest)) {
return new Sdk.ed_GetTicketMoveDataFromMKLRequest(contactGuid);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _ContactGuid = null;

// internal validation functions

function _setValidContactGuid(value) {
 if (typeof value == "string") {
  _ContactGuid = value;
 }
 else {
  throw new Error("Sdk.ed_GetTicketMoveDataFromMKLRequest ContactGuid property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof contactGuid != "undefined") {
   _setValidContactGuid(contactGuid);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ContactGuid</b:key>",
           (_ContactGuid == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ContactGuid, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetTicketMoveDataFromMKL</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetTicketMoveDataFromMKLResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setContactGuid = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidContactGuid(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetTicketMoveDataFromMKLRequest.__class = true;

this.ed_GetTicketMoveDataFromMKLResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetTicketMoveDataFromMKLRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetTicketMoveDataFromMKLResponse)) {
   return new Sdk.ed_GetTicketMoveDataFromMKLResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getTicketMoveDataFromMKLResponse = null;

  // Internal property setter functions

  function _setGetTicketMoveDataFromMKLResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetTicketMoveDataFromMKLResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getTicketMoveDataFromMKLResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetTicketMoveDataFromMKLResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getTicketMoveDataFromMKLResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetTicketMoveDataFromMKLResponse(responseXml);
 }
 this.ed_GetTicketMoveDataFromMKLResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetTicketMoveDataFromMKLRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetTicketMoveDataFromMKLResponse.prototype = new Sdk.OrganizationResponse();

"use strict";
(function () {
this.ed_AllowOneMoreTicketMoveRequest = function (
contactGuid
)
{
///<summary>
/// 
///</summary>
///<param name="contactGuid"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_AllowOneMoreTicketMoveRequest)) {
return new Sdk.ed_AllowOneMoreTicketMoveRequest(contactGuid);
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
  throw new Error("Sdk.ed_AllowOneMoreTicketMoveRequest ContactGuid property is required and must be a String.")
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
        "<a:RequestName>ed_AllowOneMoreTicketMove</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_AllowOneMoreTicketMoveResponse);
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
 this.ed_AllowOneMoreTicketMoveRequest.__class = true;

this.ed_AllowOneMoreTicketMoveResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_AllowOneMoreTicketMoveRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_AllowOneMoreTicketMoveResponse)) {
   return new Sdk.ed_AllowOneMoreTicketMoveResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _results = null;

  // Internal property setter functions

  function _setResults(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='Results']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _results = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getResults = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _results;
  }

  //Set property values from responseXml constructor parameter
  _setResults(responseXml);
 }
 this.ed_AllowOneMoreTicketMoveResponse.__class = true;
}).call(Sdk)

Sdk.ed_AllowOneMoreTicketMoveRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_AllowOneMoreTicketMoveResponse.prototype = new Sdk.OrganizationResponse();

"use strict";
(function () {
this.ed_BlockCustomerPortalRequest = function (
sSN,
blocked
)
{
///<summary>
/// 
///</summary>
///<param name="sSN"  type="String">
/// [Add Description]
///</param>
///<param name="blocked"  type="Boolean">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_BlockCustomerPortalRequest)) {
return new Sdk.ed_BlockCustomerPortalRequest(sSN, blocked);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _SSN = null;
var _Blocked = null;

// internal validation functions

function _setValidSSN(value) {
 if (typeof value == "string") {
  _SSN = value;
 }
 else {
  throw new Error("Sdk.ed_BlockCustomerPortalRequest SSN property is required and must be a String.")
 }
}

function _setValidBlocked(value) {
 if (typeof value == "boolean") {
  _Blocked = value;
 }
 else {
  throw new Error("Sdk.ed_BlockCustomerPortalRequest Blocked property is required and must be a Boolean.")
 }
}

//Set internal properties from constructor parameters
  if (typeof sSN != "undefined") {
   _setValidSSN(sSN);
  }
  if (typeof blocked != "undefined") {
   _setValidBlocked(blocked);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>SSN</b:key>",
           (_SSN == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _SSN, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Blocked</b:key>",
           (_Blocked == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:boolean\">", _Blocked, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_BlockCustomerPortal</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_BlockCustomerPortalResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setSSN = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidSSN(value);
   this.setRequestXml(getRequestXml());
  }

  this.setBlocked = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="Boolean">
  /// [Add Description]
  ///</param>
   _setValidBlocked(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_BlockCustomerPortalRequest.__class = true;

this.ed_BlockCustomerPortalResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_BlockCustomerPortalRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_BlockCustomerPortalResponse)) {
   return new Sdk.ed_BlockCustomerPortalResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // This message returns no values






 }
 this.ed_BlockCustomerPortalResponse.__class = true;
}).call(Sdk)

Sdk.ed_BlockCustomerPortalRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_BlockCustomerPortalResponse.prototype = new Sdk.OrganizationResponse();

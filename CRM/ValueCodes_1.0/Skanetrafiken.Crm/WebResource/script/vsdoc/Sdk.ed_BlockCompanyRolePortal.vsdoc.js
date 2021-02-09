"use strict";
(function () {
this.ed_BlockCompanyRolePortalRequest = function (
sSN,
portalId,
blocked
)
{
///<summary>
/// 
///</summary>
///<param name="sSN"  type="String">
/// [Add Description]
///</param>
///<param name="portalId"  type="String">
/// [Add Description]
///</param>
///<param name="blocked"  type="Boolean">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_BlockCompanyRolePortalRequest)) {
return new Sdk.ed_BlockCompanyRolePortalRequest(sSN, portalId, blocked);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _SSN = null;
var _PortalId = null;
var _Blocked = null;

// internal validation functions

function _setValidSSN(value) {
 if (typeof value == "string") {
  _SSN = value;
 }
 else {
  throw new Error("Sdk.ed_BlockCompanyRolePortalRequest SSN property is required and must be a String.")
 }
}

function _setValidPortalId(value) {
 if (typeof value == "string") {
  _PortalId = value;
 }
 else {
  throw new Error("Sdk.ed_BlockCompanyRolePortalRequest PortalId property is required and must be a String.")
 }
}

function _setValidBlocked(value) {
 if (typeof value == "boolean") {
  _Blocked = value;
 }
 else {
  throw new Error("Sdk.ed_BlockCompanyRolePortalRequest Blocked property is required and must be a Boolean.")
 }
}

//Set internal properties from constructor parameters
  if (typeof sSN != "undefined") {
   _setValidSSN(sSN);
  }
  if (typeof portalId != "undefined") {
   _setValidPortalId(portalId);
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
            "<b:key>PortalId</b:key>",
           (_PortalId == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _PortalId, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Blocked</b:key>",
           (_Blocked == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:boolean\">", _Blocked, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_BlockCompanyRolePortal</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_BlockCompanyRolePortalResponse);
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

  this.setPortalId = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidPortalId(value);
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
 this.ed_BlockCompanyRolePortalRequest.__class = true;

this.ed_BlockCompanyRolePortalResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_BlockCompanyRolePortalRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_BlockCompanyRolePortalResponse)) {
   return new Sdk.ed_BlockCompanyRolePortalResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // This message returns no values






 }
 this.ed_BlockCompanyRolePortalResponse.__class = true;
}).call(Sdk)

Sdk.ed_BlockCompanyRolePortalRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_BlockCompanyRolePortalResponse.prototype = new Sdk.OrganizationResponse();

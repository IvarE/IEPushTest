"use strict";
(function () {
this.ed_BlockAccountPortalRequest = function (
portalID,
parentID,
organizationNumber,
blocked
)
{
///<summary>
/// 
///</summary>
///<param name="portalID"  type="String">
/// [Add Description]
///</param>
///<param name="parentID"  type="String">
/// [Add Description]
///</param>
///<param name="organizationNumber"  type="String">
/// [Add Description]
///</param>
///<param name="blocked"  type="Boolean">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_BlockAccountPortalRequest)) {
return new Sdk.ed_BlockAccountPortalRequest(portalID, parentID, organizationNumber, blocked);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _PortalID = null;
var _ParentID = null;
var _OrganizationNumber = null;
var _Blocked = null;

// internal validation functions

function _setValidPortalID(value) {
 if (typeof value == "string") {
  _PortalID = value;
 }
 else {
  throw new Error("Sdk.ed_BlockAccountPortalRequest PortalID property is required and must be a String.")
 }
}

function _setValidParentID(value) {
 if (typeof value == "string") {
  _ParentID = value;
 }
 else {
  throw new Error("Sdk.ed_BlockAccountPortalRequest ParentID property is required and must be a String.")
 }
}

function _setValidOrganizationNumber(value) {
 if (typeof value == "string") {
  _OrganizationNumber = value;
 }
 else {
  throw new Error("Sdk.ed_BlockAccountPortalRequest OrganizationNumber property is required and must be a String.")
 }
}

function _setValidBlocked(value) {
 if (typeof value == "boolean") {
  _Blocked = value;
 }
 else {
  throw new Error("Sdk.ed_BlockAccountPortalRequest Blocked property is required and must be a Boolean.")
 }
}

//Set internal properties from constructor parameters
  if (typeof portalID != "undefined") {
   _setValidPortalID(portalID);
  }
  if (typeof parentID != "undefined") {
   _setValidParentID(parentID);
  }
  if (typeof organizationNumber != "undefined") {
   _setValidOrganizationNumber(organizationNumber);
  }
  if (typeof blocked != "undefined") {
   _setValidBlocked(blocked);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>PortalID</b:key>",
           (_PortalID == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _PortalID, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>ParentID</b:key>",
           (_ParentID == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _ParentID, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>OrganizationNumber</b:key>",
           (_OrganizationNumber == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _OrganizationNumber, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>Blocked</b:key>",
           (_Blocked == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:boolean\">", _Blocked, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_BlockAccountPortal</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_BlockAccountPortalResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setPortalID = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidPortalID(value);
   this.setRequestXml(getRequestXml());
  }

  this.setParentID = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidParentID(value);
   this.setRequestXml(getRequestXml());
  }

  this.setOrganizationNumber = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidOrganizationNumber(value);
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
 this.ed_BlockAccountPortalRequest.__class = true;

this.ed_BlockAccountPortalResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_BlockAccountPortalRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_BlockAccountPortalResponse)) {
   return new Sdk.ed_BlockAccountPortalResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // This message returns no values






 }
 this.ed_BlockAccountPortalResponse.__class = true;
}).call(Sdk)

Sdk.ed_BlockAccountPortalRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_BlockAccountPortalResponse.prototype = new Sdk.OrganizationResponse();

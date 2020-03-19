"use strict";
(function () {
this.ed_PrintRefundReportsRequest = function (
startDate,
endDate
)
{
///<summary>
/// 
///</summary>
///<param name="startDate"  type="Date">
/// [Add Description]
///</param>
///<param name="endDate"  type="Date">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_PrintRefundReportsRequest)) {
return new Sdk.ed_PrintRefundReportsRequest(startDate, endDate);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _StartDate = null;
var _EndDate = null;

// internal validation functions

function _setValidStartDate(value) {
 if (value instanceof Date) {
  _StartDate = value;
 }
 else {
  throw new Error("Sdk.ed_PrintRefundReportsRequest StartDate property is required and must be a Date.")
 }
}

function _setValidEndDate(value) {
 if (value instanceof Date) {
  _EndDate = value;
 }
 else {
  throw new Error("Sdk.ed_PrintRefundReportsRequest EndDate property is required and must be a Date.")
 }
}

//Set internal properties from constructor parameters
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
            "<b:key>StartDate</b:key>",
           (_StartDate == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:dateTime\">", _StartDate.toISOString(), "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>EndDate</b:key>",
           (_EndDate == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:dateTime\">", _EndDate.toISOString(), "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_PrintRefundReports</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_PrintRefundReportsResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setStartDate = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="Date">
  /// [Add Description]
  ///</param>
   _setValidStartDate(value);
   this.setRequestXml(getRequestXml());
  }

  this.setEndDate = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="Date">
  /// [Add Description]
  ///</param>
   _setValidEndDate(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_PrintRefundReportsRequest.__class = true;

this.ed_PrintRefundReportsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_PrintRefundReportsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_PrintRefundReportsResponse)) {
   return new Sdk.ed_PrintRefundReportsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // This message returns no values






 }
 this.ed_PrintRefundReportsResponse.__class = true;
}).call(Sdk)

Sdk.ed_PrintRefundReportsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_PrintRefundReportsResponse.prototype = new Sdk.OrganizationResponse();

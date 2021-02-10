"use strict";
(function () {
this.ed_GetLineDetailsRequest = function (
lineType
)
{
///<summary>
/// 
///</summary>
///<param name="lineType"  type="String">
/// [Add Description]
///</param>
if (!(this instanceof Sdk.ed_GetLineDetailsRequest)) {
return new Sdk.ed_GetLineDetailsRequest(lineType);
}
Sdk.OrganizationRequest.call(this);

  // Internal properties
var _LineType = null;

// internal validation functions

function _setValidLineType(value) {
 if (typeof value == "string") {
  _LineType = value;
 }
 else {
  throw new Error("Sdk.ed_GetLineDetailsRequest LineType property is required and must be a String.")
 }
}

//Set internal properties from constructor parameters
  if (typeof lineType != "undefined") {
   _setValidLineType(lineType);
  }

  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters>",

          "<a:KeyValuePairOfstringanyType>",
            "<b:key>LineType</b:key>",
           (_LineType == null) ? "<b:value i:nil=\"true\" />" :
           ["<b:value i:type=\"c:string\">", _LineType, "</b:value>"].join(""),
          "</a:KeyValuePairOfstringanyType>",

        "</a:Parameters>",
        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetLineDetails</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetLineDetailsResponse);
  this.setRequestXml(getRequestXml());

  // Public methods to set properties
  this.setLineType = function (value) {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<param name="value" type="String">
  /// [Add Description]
  ///</param>
   _setValidLineType(value);
   this.setRequestXml(getRequestXml());
  }

 }
 this.ed_GetLineDetailsRequest.__class = true;

this.ed_GetLineDetailsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetLineDetailsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetLineDetailsResponse)) {
   return new Sdk.ed_GetLineDetailsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getLineDetailsResponse = null;

  // Internal property setter functions

  function _setGetLineDetailsResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetLineDetailsResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getLineDetailsResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetLineDetailsResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getLineDetailsResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetLineDetailsResponse(responseXml);
 }
 this.ed_GetLineDetailsResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetLineDetailsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetLineDetailsResponse.prototype = new Sdk.OrganizationResponse();

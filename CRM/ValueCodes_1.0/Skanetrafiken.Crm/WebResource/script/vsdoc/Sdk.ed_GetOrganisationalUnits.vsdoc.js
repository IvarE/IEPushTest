"use strict";
(function () {
this.ed_GetOrganisationalUnitsRequest = function (

)
{
///<summary>
/// 
///</summary>
if (!(this instanceof Sdk.ed_GetOrganisationalUnitsRequest)) {
return new Sdk.ed_GetOrganisationalUnitsRequest();
}
Sdk.OrganizationRequest.call(this);

  // This message has no parameters





  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters />",


        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetOrganisationalUnits</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetOrganisationalUnitsResponse);
  this.setRequestXml(getRequestXml());


 }
 this.ed_GetOrganisationalUnitsRequest.__class = true;

this.ed_GetOrganisationalUnitsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetOrganisationalUnitsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetOrganisationalUnitsResponse)) {
   return new Sdk.ed_GetOrganisationalUnitsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getOrganisationalUnitsResponse = null;

  // Internal property setter functions

  function _setGetOrganisationalUnitsResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetOrganisationalUnitsResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getOrganisationalUnitsResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetOrganisationalUnitsResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getOrganisationalUnitsResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetOrganisationalUnitsResponse(responseXml);
 }
 this.ed_GetOrganisationalUnitsResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetOrganisationalUnitsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetOrganisationalUnitsResponse.prototype = new Sdk.OrganizationResponse();

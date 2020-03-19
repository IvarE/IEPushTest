"use strict";
(function () {
this.ed_GetContractorsRequest = function (

)
{
///<summary>
/// 
///</summary>
if (!(this instanceof Sdk.ed_GetContractorsRequest)) {
return new Sdk.ed_GetContractorsRequest();
}
Sdk.OrganizationRequest.call(this);

  // This message has no parameters





  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters />",


        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_GetContractors</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_GetContractorsResponse);
  this.setRequestXml(getRequestXml());


 }
 this.ed_GetContractorsRequest.__class = true;

this.ed_GetContractorsResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_GetContractorsRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_GetContractorsResponse)) {
   return new Sdk.ed_GetContractorsResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _getContractorsResponse = null;

  // Internal property setter functions

  function _setGetContractorsResponse(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='GetContractorsResponse']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _getContractorsResponse = Sdk.Xml.getNodeText(valueNode);
   }
  }
  //Public Methods to retrieve properties
  this.getGetContractorsResponse = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="String">
  /// [Add Description]
  ///</returns>
   return _getContractorsResponse;
  }

  //Set property values from responseXml constructor parameter
  _setGetContractorsResponse(responseXml);
 }
 this.ed_GetContractorsResponse.__class = true;
}).call(Sdk)

Sdk.ed_GetContractorsRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_GetContractorsResponse.prototype = new Sdk.OrganizationResponse();

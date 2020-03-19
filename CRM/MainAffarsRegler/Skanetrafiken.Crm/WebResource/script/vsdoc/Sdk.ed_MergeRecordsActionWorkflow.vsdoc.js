"use strict";
(function () {
this.ed_MergeRecordsActionWorkflowRequest = function (

)
{
///<summary>
/// 
///</summary>
if (!(this instanceof Sdk.ed_MergeRecordsActionWorkflowRequest)) {
return new Sdk.ed_MergeRecordsActionWorkflowRequest();
}
Sdk.OrganizationRequest.call(this);

  // This message has no parameters





  function getRequestXml() {
return ["<d:request>",
        "<a:Parameters />",


        "<a:RequestId i:nil=\"true\" />",
        "<a:RequestName>ed_MergeRecordsActionWorkflow</a:RequestName>",
      "</d:request>"].join("");
  }

  this.setResponseType(Sdk.ed_MergeRecordsActionWorkflowResponse);
  this.setRequestXml(getRequestXml());


 }
 this.ed_MergeRecordsActionWorkflowRequest.__class = true;

this.ed_MergeRecordsActionWorkflowResponse = function (responseXml) {
  ///<summary>
  /// Response to ed_MergeRecordsActionWorkflowRequest
  ///</summary>
  if (!(this instanceof Sdk.ed_MergeRecordsActionWorkflowResponse)) {
   return new Sdk.ed_MergeRecordsActionWorkflowResponse(responseXml);
  }
  Sdk.OrganizationResponse.call(this)

  // Internal properties
  var _remainingRecords = null;

  // Internal property setter functions

  function _setRemainingRecords(xml) {
   var valueNode = Sdk.Xml.selectSingleNode(xml, "//a:KeyValuePairOfstringanyType[b:key='RemainingRecords']/b:value");
   if (!Sdk.Xml.isNodeNull(valueNode)) {
    _remainingRecords = parseInt(Sdk.Xml.getNodeText(valueNode), 10);
   }
  }
  //Public Methods to retrieve properties
  this.getRemainingRecords = function () {
  ///<summary>
  /// [Add Description]
  ///</summary>
  ///<returns type="Number">
  /// [Add Description]
  ///</returns>
   return _remainingRecords;
  }

  //Set property values from responseXml constructor parameter
  _setRemainingRecords(responseXml);
 }
 this.ed_MergeRecordsActionWorkflowResponse.__class = true;
}).call(Sdk)

Sdk.ed_MergeRecordsActionWorkflowRequest.prototype = new Sdk.OrganizationRequest();
Sdk.ed_MergeRecordsActionWorkflowResponse.prototype = new Sdk.OrganizationResponse();

using System;
using System.Collections.Generic;
using CGIXrmWin;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Data.Odbc;
using System.Configuration;
using System.Text;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CGIXrmLogger;
using System.Collections.ObjectModel;
using CGIXrmHandler.CallGuide.Models;
using CGIXrmHandler.CrmClasses;
using CGIXrmHandler.Shared;

namespace CGIXrmHandler
{
    public class CallGuideBatchHandler
    {
        #region Declarations
        private readonly XrmManager _xrmManager;
        private readonly XrmHelper _xrmHelper;
        #endregion

        #region Constructors
        public CallGuideBatchHandler()
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }
        public CallGuideBatchHandler(Guid callerId)
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        public CallGuideBatchHandler(Guid callerId, Settings settings)
        {
            _xrmHelper = new XrmHelper(settings);
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        #endregion 

        #region Public Properties
        string _ApplicationName;
        public string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationName))
                    _ApplicationName = "CGIXRMHandlers - CallGuideBatchHandler";

                return _ApplicationName;
            }
            set { _ApplicationName = value; }
        }

        LogToCrm _Logger;
        public LogToCrm Logger
        {
            get
            {
                if (_Logger == null)
                    _Logger = new LogToCrm();
                return _Logger;
            }
            set { _Logger = value; }
        }

        #endregion

        #region Private Methods

        private string GetFbUrl(string message)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                string urlRegexPattern = "\\S+\\.\\S+";
                Match resultValue = Regex.Match(message, urlRegexPattern);

                if (resultValue.Success)
                {
                    returnValue = resultValue.Value;
                }
            }
            return returnValue;
        }

        private CategoryDetail GetCategoryDetailByCallGuideCategory(string callguideCategory)
        {
            CategoryDetail categoryDetail = null;
            QueryByAttribute queryByAttribute = new QueryByAttribute
            {
                EntityName = "cgi_categorydetail",
                ColumnSet = new ColumnSet(true)
            };

            queryByAttribute.AddAttributeValue("cgi_callguidecategory", callguideCategory);
            ObservableCollection<CategoryDetail> categoryDetails = _xrmManager.Get<CategoryDetail>(queryByAttribute);
            if (categoryDetails != null && categoryDetails.Count > 0)
                categoryDetail = categoryDetails[0];

            return categoryDetail;
        }

        private Entity GetCaseEntityFromWorkitem(WorkItem workitem, CallGuideBatchActivity activiyType)
        {

            CategoryDetail categoryDetail = GetCategoryDetailByCallGuideCategory(workitem.ErrandTaskType);

            Entity entity = new Entity()
            {
                LogicalName = "incident",
                Attributes ={
                        new KeyValuePair<string,object>("cgi_accountid",new EntityReference("account",workitem.AccountId)),
                        new KeyValuePair<string,object>("customerid",new EntityReference("account",workitem.AccountId)),
                        new KeyValuePair<string,object>("caseorigincode",new OptionSetValue((int)activiyType)),                                                                                
                        new KeyValuePair<string, object>("description", workitem.Description),
                        new KeyValuePair<string, object>("cgi_callguideinfo", new EntityReference("cgi_callguideinfo",workitem.CallGuideInfoId)),
                        activiyType == CallGuideBatchActivity.FaceBook ? new KeyValuePair<string, object>("cgi_facebookpostid",new EntityReference("cgi_callguidefacebook", workitem.ActivityId)):new KeyValuePair<string, object>("cgi_facebookpostid",Guid.Empty),
                        activiyType == CallGuideBatchActivity.Chat ?new KeyValuePair<string,object>("cgi_chatid",new EntityReference("cgi_callguidechat", workitem.ActivityId)):new KeyValuePair<string,object>("cgi_chatid",Guid.Empty)
                        }
            };
            if (categoryDetail != null)
            {
                switch (categoryDetail.Level)
                {
                    case "1":
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat1id", new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId)));
                        break;
                    case "2":
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat2id", new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId)));
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat1id", categoryDetail.ParentLevel1 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel1.Id) : null));
                        break;
                    case "3":
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat3id", new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId)));
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat2id", categoryDetail.ParentLevel2 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel2.Id) : null));
                        entity.Attributes.Add(new KeyValuePair<string, object>("cgi_casdet_row1_cat1id", categoryDetail.ParentLevel1 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel1.Id) : null));
                        break;
                }
            }
            return entity;



        }
        #endregion

        #region Public Methods

        #region [Shared]
        public List<CallGuideRecord> GetCallGuideDBRecord(string interactionIds, CallGuideBatchActivity callguideBatchAction)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["callguideDB"].ToString();
            using (OdbcConnection CallGuideDBCon = new OdbcConnection(connectionString))
            {


                StringBuilder sqlCommand = new StringBuilder();
                switch (callguideBatchAction)
                {
                    case CallGuideBatchActivity.Chat:
                        sqlCommand.Append("select a.contact_id contactid,a.interaction_id interactionid,b.chat_history archive from [dbo].[cg_iv_interaction] a Join [dbo].[cg_iv_chat_archive] b on a.interaction_id = b.interaction_id WHERE a.contact_id in(");
                        sqlCommand.Append(interactionIds);
                        sqlCommand.Append(")");
                        break;
                    case CallGuideBatchActivity.FaceBook:
                        sqlCommand.Append("select a.contact_id contactid,a.interaction_id interactionid,b.mime_object archive from [dbo].[cg_iv_interaction] a Join [dbo].[cg_iv_email_archive] b on a.interaction_id = b.interaction_id WHERE a.contact_id in(");
                        sqlCommand.Append(interactionIds);
                        sqlCommand.Append(")");
                        break;
                }

                CallGuideDBCon.Open();
                //
                // The following code uses an SqlCommand based on the SqlConnection.
                //
                using (OdbcCommand command = new OdbcCommand(sqlCommand.ToString(), CallGuideDBCon))
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    List<CallGuideRecord> lstCallguideRecord = new List<CallGuideRecord>();
                    while (reader.Read())
                    {
                        CallGuideRecord callguideRecord = new CallGuideRecord();
                        callguideRecord.ContactId = Convert.ToString(reader["contactid"]);
                        callguideRecord.InteractionId = Convert.ToString(reader["interactionid"]);
                        if (callguideBatchAction == CallGuideBatchActivity.FaceBook)
                        {

                            string mailBody = MimeObjectParser.GetMailContent(Convert.ToString(reader["archive"]), false);
                            callguideRecord.Data = mailBody;
                            callguideRecord.FbUrl = GetFbUrl(mailBody);
                        }
                        else
                        {
                            callguideRecord.Data = Convert.ToString(reader["archive"]);
                        }

                        callguideRecord.CallGuideActivity = callguideBatchAction;
                        lstCallguideRecord.Add(callguideRecord);
                    }

                    Logger.LogMessage("Callguide Record Count =" + lstCallguideRecord.Count, ApplicationName, _xrmManager.Service);
                    return lstCallguideRecord;
                }
            }


        }

        public List<WorkItem> BulkCreateCase(List<WorkItem> lstToCreatCase, CallGuideBatchActivity activiyType)
        {
            ExecuteMultipleRequest multipleCreateCaseRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            multipleCreateCaseRequest.Requests.AddRange(lstToCreatCase.Select(e =>
                                                            new CreateRequest()
                                                            {
                                                                RequestId = e.ActivityId,
                                                                Target = GetCaseEntityFromWorkitem(e, activiyType)
                                                            }
                                                            ));

            ExecuteMultipleResponse multipleCreateCaseResponse = (ExecuteMultipleResponse)_xrmManager.Service.Execute(multipleCreateCaseRequest);

            List<WorkItem> lstCaseCreated = lstToCreatCase;

            if (multipleCreateCaseResponse.Responses.Count > 0)
            {
                foreach (var responseItem in multipleCreateCaseResponse.Responses)
                {
                    if (responseItem.Fault != null)
                    {
                        lstCaseCreated.Remove((WorkItem)lstCaseCreated.Find(e => e.ActivityId == ((CreateRequest)multipleCreateCaseRequest.Requests[responseItem.RequestIndex]).RequestId));
                    }
                    else
                    {
                        (lstCaseCreated.Find(e => e.ActivityId == ((CreateRequest)multipleCreateCaseRequest.Requests[responseItem.RequestIndex]).RequestId)).CreatedCaseId = new Guid(responseItem.Response.Results["id"].ToString());
                    }
                }
            }
            else
            {
                Task.Factory.StartNew(() => Logger.LogMessage("All account records have been updated successfully.", ApplicationName, _xrmManager.Service));
                Console.WriteLine("All account records have been updated successfully.");
            }
            return lstCaseCreated;
        }

        public List<WorkItem> BulkCreateCase1(List<WorkItem> lstToCreatCase, CallGuideBatchActivity activiyType)
        {
            ExecuteMultipleRequest multipleCreateCaseRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            multipleCreateCaseRequest.Requests.AddRange(lstToCreatCase.Select(e =>
                                                            new CreateRequest()
                                                            {
                                                                RequestId = e.ActivityId,
                                                                Target = new Entity()
                                                                {
                                                                    LogicalName = "incident",
                                                                    Attributes ={
                                                                                new KeyValuePair<string,object>("cgi_accountid",new EntityReference("account",e.AccountId)),
                                                                                new KeyValuePair<string,object>("customerid",new EntityReference("account",e.AccountId)),
                                                                                new KeyValuePair<string,object>("caseorigincode",new OptionSetValue((int)activiyType)),                                                                                
                                                                                new KeyValuePair<string, object>("description", e.Description),
                                                                                new KeyValuePair<string, object>("cgi_callguideinfo", new EntityReference("cgi_callguideinfo",e.CallGuideInfoId)),
                                                                                activiyType == CallGuideBatchActivity.FaceBook ? new KeyValuePair<string, object>("cgi_facebookpostid",new EntityReference("cgi_callguidefacebook", e.ActivityId)):new KeyValuePair<string, object>("cgi_facebookpostid",Guid.Empty),
                                                                                activiyType == CallGuideBatchActivity.Chat ?new KeyValuePair<string,object>("cgi_chatid",new EntityReference("cgi_callguidechat", e.ActivityId)):new KeyValuePair<string,object>("cgi_chatid",Guid.Empty)
                                                                                }
                                                                }
                                                            }
                                                            ));

            ExecuteMultipleResponse multipleCreateCaseResponse = (ExecuteMultipleResponse)_xrmManager.Service.Execute(multipleCreateCaseRequest);

            List<WorkItem> lstCaseCreated = lstToCreatCase;

            if (multipleCreateCaseResponse.Responses.Count > 0)
            {
                foreach (var responseItem in multipleCreateCaseResponse.Responses)
                {
                    if (responseItem.Fault != null)
                    {
                        lstCaseCreated.Remove((WorkItem)lstCaseCreated.Find(e => e.ActivityId == ((CreateRequest)multipleCreateCaseRequest.Requests[responseItem.RequestIndex]).RequestId));
                    }
                    else
                    {
                        (lstCaseCreated.Find(e => e.ActivityId == ((CreateRequest)multipleCreateCaseRequest.Requests[responseItem.RequestIndex]).RequestId)).CreatedCaseId = new Guid(responseItem.Response.Results["id"].ToString());
                    }
                }
            }
            else
            {
                Task.Factory.StartNew(() => Logger.LogMessage("All account records have been updated successfully.", ApplicationName, _xrmManager.Service));
                Console.WriteLine("All account records have been updated successfully.");
            }
            return lstCaseCreated;
        }

        public List<WorkItem> BulkCreateCaseCategory(List<WorkItem> lstToCreatCaseCategory, CallGuideBatchActivity activiyType)
        {
            List<WorkItem> lstCaseCategoryCreated = new List<WorkItem>();

            foreach (WorkItem caseCatergoryToCreate in lstToCreatCaseCategory)
            {
                if (_xrmHelper.CreateCaseCategory(caseCatergoryToCreate.CreatedCaseId, caseCatergoryToCreate.ErrandTaskType, _xrmManager) != Guid.Empty)
                    lstCaseCategoryCreated.Add((WorkItem)lstToCreatCaseCategory.Find(e => e.ActivityId == caseCatergoryToCreate.ActivityId));
            }
            return lstCaseCategoryCreated;
        }

        public bool BulkCompleteActivty(List<WorkItem> lstActivityToComplete, CallGuideBatchActivity activiyType)
        {
            bool returnValue = true;
            try
            {
                foreach (WorkItem activityToComplete in lstActivityToComplete)
                {
                    try
                    {
                        SetStateRequest setStateRequest = new SetStateRequest
                        {
                            EntityMoniker =
                                new EntityReference(
                                    activiyType == CallGuideBatchActivity.Chat
                                        ? "cgi_callguidechat"
                                        : "cgi_callguidefacebook", activityToComplete.ActivityId),
                            State = new OptionSetValue(1),
                            Status = new OptionSetValue(2)
                        };





                        _xrmManager.Service.Execute(setStateRequest);
                    }
                    catch
                    {
                        returnValue = false;
                    }
                }
                return returnValue;
            }
            catch
            {
                returnValue = false;
                return returnValue;
            }
        }

        public List<WorkItem> BulkUpdateRegarding(List<WorkItem> lstToUpdate, CallGuideBatchActivity activiyType)
        {
            ExecuteMultipleRequest multipleUpdateActivityRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            multipleUpdateActivityRequest.Requests.AddRange(lstToUpdate.Select(e =>
                                                            new UpdateRequest()
                                                            {
                                                                Target = new Entity()
                                                                {
                                                                    LogicalName = (activiyType == CallGuideBatchActivity.Chat ? "cgi_callguidechat" : "cgi_callguidefacebook"),
                                                                    Attributes ={
                                                                                new KeyValuePair<string,object>("activityid",e.ActivityId),
                                                                                new KeyValuePair<string,object>("regardingobjectid",new EntityReference("incident",e.CreatedCaseId))
                                                                             }
                                                                }
                                                            }
                                                            ));

            ExecuteMultipleResponse multipleUpdateActivityResponse = (ExecuteMultipleResponse)_xrmManager.Service.Execute(multipleUpdateActivityRequest);

            List<WorkItem> lstUpdated = lstToUpdate;

            if (multipleUpdateActivityResponse.Responses.Count > 0)
            {
                foreach (var responseItem in multipleUpdateActivityResponse.Responses)
                {
                    if (responseItem.Fault != null)
                        lstUpdated.Remove((WorkItem)lstUpdated.Find(e => e.ActivityId == ((UpdateRequest)multipleUpdateActivityRequest.Requests[responseItem.RequestIndex]).Target.Id));
                }
            }
            else
            {

                Task.Factory.StartNew(() => Logger.LogMessage("All account records have been updated successfully.", ApplicationName, _xrmManager.Service));

                Console.WriteLine("All account records have been updated successfully.");
            }
            return lstUpdated;
        }
        #endregion

        #region [Chat]

        public List<WorkItem> GetOpenCrmChatActivity()
        {
            List<WorkItem> lstJobItem = new List<WorkItem>();
            QueryExpression queryExpChatActivity = new QueryExpression("cgi_callguidechat");
            queryExpChatActivity.ColumnSet = new ColumnSet();
            queryExpChatActivity.ColumnSet.AddColumn("activityid");
            queryExpChatActivity.ColumnSet.AddColumn("from");
            queryExpChatActivity.ColumnSet.AddColumn("description");

            queryExpChatActivity.LinkEntities.Add(new LinkEntity("cgi_callguidechat", "cgi_callguideinfo", "cgi_callguideinfoid", "cgi_callguideinfoid", JoinOperator.Inner));
            queryExpChatActivity.LinkEntities[0].Columns.AddColumns("cgi_callguideinfoid", "cgi_callguidesessionid", "cgi_errandtasktype");
            queryExpChatActivity.LinkEntities[0].EntityAlias = "callguidechatinfo";

            queryExpChatActivity.Criteria = new FilterExpression(LogicalOperator.And);
            queryExpChatActivity.Criteria.AddCondition(new ConditionExpression("cgi_createcase", ConditionOperator.Equal, true));
            queryExpChatActivity.Criteria.AddCondition(new ConditionExpression("statuscode", ConditionOperator.Equal, 1));
            queryExpChatActivity.Criteria.AddCondition(new ConditionExpression("cgi_callguideinfoid", ConditionOperator.NotNull));

            EntityCollection entityChatActivityCollection = _xrmManager.Service.RetrieveMultiple(queryExpChatActivity);
            if (entityChatActivityCollection != null)
            {
                if (entityChatActivityCollection.Entities != null && entityChatActivityCollection.Entities.Count > 0)
                {
                    lstJobItem = entityChatActivityCollection.Entities
                                .Select(e =>
                                    new WorkItem
                                    {
                                        AccountId = e.Attributes.Contains("from") ? e.GetAttributeValue<EntityCollection>("from").Entities[0].Attributes.Contains("partyid") ? e.GetAttributeValue<EntityCollection>("from").Entities[0].GetAttributeValue<EntityReference>("partyid").Id : System.Guid.Empty : System.Guid.Empty,
                                        ActivityId = e.Attributes.Contains("activityid") ? e.GetAttributeValue<Guid>("activityid") : System.Guid.NewGuid(),
                                        Description = e.Attributes.Contains("description") ? e.GetAttributeValue<string>("description") : string.Empty,
                                        CallguideSessionId = e.Attributes.Contains("callguidechatinfo.cgi_callguidesessionid") ? (e.GetAttributeValue<AliasedValue>("callguidechatinfo.cgi_callguidesessionid")).Value.ToString() : string.Empty,
                                        ErrandTaskType = e.Attributes.Contains("callguidechatinfo.cgi_errandtasktype") ? (e.GetAttributeValue<AliasedValue>("callguidechatinfo.cgi_errandtasktype")).Value.ToString() : string.Empty,
                                        CallGuideInfoId = e.Attributes.Contains("callguidechatinfo.cgi_callguideinfoid") ? (Guid)(e.GetAttributeValue<AliasedValue>("callguidechatinfo.cgi_callguideinfoid")).Value : Guid.Empty
                                    }).ToList();
                }
            }
            return lstJobItem;
        }

        public List<WorkItem> BulkUpdateChatConversation(List<WorkItem> lstToUpdate)
        {
            ExecuteMultipleRequest multipleUpdateActivityRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            multipleUpdateActivityRequest.Requests.AddRange(lstToUpdate.Select(e =>
                                                            new UpdateRequest()
                                                            {
                                                                Target = new Entity()
                                                                {
                                                                    LogicalName = "cgi_callguidechat",
                                                                    Attributes ={
                                                                                new KeyValuePair<string,object>("activityid",e.ActivityId),
                                                                                new KeyValuePair<string,object>("cgi_chatconversation",e.UpdateData)
                                                                             }
                                                                }
                                                            }
                                                            ));

            ExecuteMultipleResponse multipleUpdateActivityResponse = (ExecuteMultipleResponse)_xrmManager.Service.Execute(multipleUpdateActivityRequest);

            List<WorkItem> lstUpdated = lstToUpdate;

            if (multipleUpdateActivityResponse.Responses.Count > 0)
            {
                foreach (var responseItem in multipleUpdateActivityResponse.Responses)
                {
                    if (responseItem.Fault != null)
                        lstUpdated.Remove((WorkItem)lstUpdated.Find(e => e.ActivityId == ((UpdateRequest)multipleUpdateActivityRequest.Requests[responseItem.RequestIndex]).Target.Id));
                }
            }
            else
            {
                Console.WriteLine("All account records have been updated successfully.");
            }
            return lstUpdated;
        }


        #endregion

        #region [FB]
        public List<WorkItem> GetOpenCrmFaceBookActivity()
        {
            List<WorkItem> lstJobItem = new List<WorkItem>();

            QueryExpression queryExpFBActivity = new QueryExpression("cgi_callguidefacebook");
            queryExpFBActivity.ColumnSet = new ColumnSet();

            queryExpFBActivity.ColumnSet.AddColumn("activityid");
            queryExpFBActivity.ColumnSet.AddColumn("from");
            queryExpFBActivity.ColumnSet.AddColumn("description");

            queryExpFBActivity.LinkEntities.Add(new LinkEntity("cgi_callguidefacebook", "cgi_callguideinfo", "cgi_callguideinfoid", "cgi_callguideinfoid", JoinOperator.Inner));
            queryExpFBActivity.LinkEntities[0].Columns.AddColumns("cgi_callguideinfoid", "cgi_callguidesessionid", "cgi_errandtasktype");
            queryExpFBActivity.LinkEntities[0].EntityAlias = "callguidefbinfo";

            queryExpFBActivity.Criteria = new FilterExpression(LogicalOperator.And);
            queryExpFBActivity.Criteria.AddCondition(new ConditionExpression("cgi_createcase", ConditionOperator.Equal, true));
            queryExpFBActivity.Criteria.AddCondition(new ConditionExpression("statuscode", ConditionOperator.Equal, 1));
            queryExpFBActivity.Criteria.AddCondition(new ConditionExpression("cgi_callguideinfoid", ConditionOperator.NotNull));

            EntityCollection entityFbActivityCollection = _xrmManager.Service.RetrieveMultiple(queryExpFBActivity);
            if (entityFbActivityCollection != null && entityFbActivityCollection.Entities.Count > 0)
            {
                lstJobItem = entityFbActivityCollection.Entities
                            .Select(e =>
                                new WorkItem
                                {
                                    AccountId = e.Attributes.Contains("from") ? e.GetAttributeValue<EntityCollection>("from").Entities[0].Attributes.Contains("partyid") ? e.GetAttributeValue<EntityCollection>("from").Entities[0].GetAttributeValue<EntityReference>("partyid").Id : System.Guid.Empty : System.Guid.Empty,
                                    ActivityId = e.Attributes.Contains("activityid") ? e.GetAttributeValue<Guid>("activityid") : System.Guid.NewGuid(),
                                    Description = e.Attributes.Contains("description") ? e.GetAttributeValue<string>("description") : string.Empty,
                                    CallguideSessionId = e.Attributes.Contains("callguidefbinfo.cgi_callguidesessionid") ? (e.GetAttributeValue<AliasedValue>("callguidefbinfo.cgi_callguidesessionid")).Value.ToString() : string.Empty,
                                    ErrandTaskType = e.Attributes.Contains("callguidefbinfo.cgi_errandtasktype") ? (e.GetAttributeValue<AliasedValue>("callguidefbinfo.cgi_errandtasktype")).Value.ToString() : string.Empty,
                                    CallGuideInfoId = e.Attributes.Contains("callguidefbinfo.cgi_callguideinfoid") ? (Guid)(e.GetAttributeValue<AliasedValue>("callguidefbinfo.cgi_callguideinfoid")).Value : Guid.Empty

                                }).ToList();

            }
            return lstJobItem;
        }

        public List<WorkItem> BulkUpdateFaceBookPost(List<WorkItem> lstToUpdate)
        {
            ExecuteMultipleRequest multipleUpdateActivityRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            multipleUpdateActivityRequest.Requests.AddRange(lstToUpdate.Select(e =>
                                                            new UpdateRequest()
                                                            {
                                                                Target = new Entity()
                                                                {
                                                                    LogicalName = "cgi_callguidefacebook",
                                                                    Attributes ={
                                                                                new KeyValuePair<string,object>("activityid",e.ActivityId),
                                                                                new KeyValuePair<string,object>("cgi_facebookurl",e.FbUrl),
                                                                                new KeyValuePair<string,object>("cgi_facebookpost",e.UpdateData)
                                                                             }
                                                                }
                                                            }
                                                            ));

            ExecuteMultipleResponse multipleUpdateActivityResponse = (ExecuteMultipleResponse)_xrmManager.Service.Execute(multipleUpdateActivityRequest);

            List<WorkItem> lstUpdated = lstToUpdate;

            if (multipleUpdateActivityResponse.Responses.Count > 0)
            {
                foreach (var responseItem in multipleUpdateActivityResponse.Responses)
                {
                    if (responseItem.Fault != null)
                        lstUpdated.Remove((WorkItem)lstUpdated.Find(e => e.ActivityId == ((UpdateRequest)multipleUpdateActivityRequest.Requests[responseItem.RequestIndex]).Target.Id));
                }
            }
            else
            {
                Console.WriteLine("All account records have been updated successfully.");
            }
            return lstUpdated;
        }
        #endregion

        #endregion
    }
}

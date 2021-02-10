using System;
using System.Collections.Generic;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;

using CGIXrmHandler.CrmClasses;

namespace CGIXrmHandler
{
    public class CallGuideHandler : IDisposable
    {
        private XrmManager xrmManager;
        private XrmHelper xrmHelper;
        public class IncidentExtended : Incident
        {
            // nulable didnt work in CgiXrmWin attribute decorated classes
            // always serialize to false, even with defaultvalue attribute 
            // to fix CgiXrmWin needs to be handeling bool? diffrently
            // therefor subclassed for this specific instance
            private bool _cgi_ContactCustomer;
            [Xrm("cgi_contactcustomer")]
            public bool ContactCustomer
            {
                get { return _cgi_ContactCustomer; }
                set { _cgi_ContactCustomer = value; }
            }
        }

        #region [Constructor]
        public CallGuideHandler()
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }
        public CallGuideHandler(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        public CallGuideHandler(Guid callerId, Settings settings)
        {
            xrmHelper = new XrmHelper(settings);
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        #endregion

        #region [Private Methods]

        private Guid CreateIncident(CallGuideRequest callguideRequest, Guid accountIdOrContactId, Guid callguideInfoId)
        {
            CategoryDetail categoryDetail = GetCategoryDetailByCallGuideCategory(callguideRequest.ErrandTaskType);
            IncidentExtended incident = new IncidentExtended
            {
                //Title = GetCaseTitle(callguideRequest),
                Account = accountIdOrContactId == Guid.Empty ? null : (callguideRequest.CustomerType == AccountCategoryCode.Company ? new EntityReference("account", accountIdOrContactId) : null),
                Contact = accountIdOrContactId == Guid.Empty ? null : (callguideRequest.CustomerType == AccountCategoryCode.Private ? new EntityReference("contact", accountIdOrContactId) : null),
                DefaultCustomer = accountIdOrContactId == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : (callguideRequest.CustomerType == AccountCategoryCode.Company ? new EntityReference("account", accountIdOrContactId) : new EntityReference("contact", accountIdOrContactId)),
                CaseOrigin = GetCaseOrigin(callguideRequest.ContactSourceType), //new Microsoft.Xrm.Sdk.OptionSetValue((int)CaseOrgin.PhoneCall), 
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                TelephoneNumber = _formatPhoneNumber(callguideRequest.APhoneNumber),
                ContactCustomer = false
            };

            if (categoryDetail != null)
            {
                switch (categoryDetail.Level)
                {
                    case "1":
                        incident.Category1 = new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId);
                        break;
                    case "2":
                        incident.Category2 = new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId);
                        incident.Category1 = categoryDetail.ParentLevel1 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel1.Id) : null;
                        break;
                    case "3":
                        incident.Category3 = new EntityReference("cgi_categorydetail", categoryDetail.CategoryDetailId);
                        incident.Category2 = categoryDetail.ParentLevel2 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel2.Id) : null;
                        incident.Category1 = categoryDetail.ParentLevel1 != null ? new EntityReference("cgi_categorydetail", categoryDetail.ParentLevel1.Id) : null;
                        break;

                    default:
                        break;
                }
            }

            Entity createCase = xrmManager.Create<IncidentExtended>(incident);

            return createCase.Id;
        }

        private Microsoft.Xrm.Sdk.OptionSetValue GetCaseOrigin(string contactSourceType)
        {
            switch (contactSourceType.ToLowerInvariant())
            {
                case "ivr":
                    return new OptionSetValue((int)CaseOrgin.PhoneCall); // ivr is only phonecall?
                case "chat":
                    return new OptionSetValue((int)CaseOrgin.Chat);
                case "emailserver":
                    return new OptionSetValue((int)CaseOrgin.FaceBook);
                default:
                    return null;
            }
        }

        private string GetCaseTitle(CallGuideRequest callGuideRequest)
        {
            switch (callGuideRequest.ContactSourceType.ToLowerInvariant())
            {
                case "ivr":
                    return "Incoming call from " + callGuideRequest.APhoneNumber; // ivr is only phonecall?
                case "chat":
                    return "Incoming Chat :" + callGuideRequest.ChatCustomerAlias;
                case "emailserver":
                    return "Incoming Post " + callGuideRequest.APhoneNumber;
                default:
                    return string.Empty;
            }
        }

        private Guid CreatePhoneCallActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity();
            entityFrom.LogicalName = "activityparty";
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>();
            lstFromParty.Add(entityFrom);


            Entity entityTo = new Entity();
            entityTo.LogicalName = "activityparty";
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>();
            lstToParty.Add(entityTo);
            EntityCollection ActivityPartyTo = new EntityCollection(lstToParty);

            PhoneCallActivity phoneCallActivity = new PhoneCallActivity
            {
                Subject = "Incoming call from " + CallGuideRequest.APhoneNumber,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Direction = Convert.ToBoolean(CallGuideRequest.CallDirection),
                PhoneNumber = CallGuideRequest.APhoneNumber,
                Regarding = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdPhoneCallActivity = xrmManager.Create<PhoneCallActivity>(phoneCallActivity);
            return createdPhoneCallActivity.Id;
        }

        private Guid CreateChatActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity();
            entityFrom.LogicalName = "activityparty";
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>();
            lstFromParty.Add(entityFrom);


            Entity entityTo = new Entity();
            entityTo.LogicalName = "activityparty";
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>();
            lstToParty.Add(entityTo);
            EntityCollection ActivityPartyTo = new EntityCollection(lstToParty);

            ChatActivity chatActivity = new ChatActivity
            {
                Subject = "Incoming Chat :" + CallGuideRequest.ChatCustomerAlias,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Regarding = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdChatActivity = xrmManager.Create<ChatActivity>(chatActivity);
            return createdChatActivity.Id;
        }

        private Guid CreateFBActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity();
            entityFrom.LogicalName = "activityparty";
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>();
            lstFromParty.Add(entityFrom);


            Entity entityTo = new Entity();
            entityTo.LogicalName = "activityparty";
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>();
            lstToParty.Add(entityTo);
            EntityCollection ActivityPartyTo = new EntityCollection(lstToParty);

            FacebookActivity fbActivity = new FacebookActivity
            {
                Subject = "Incoming Post " + CallGuideRequest.APhoneNumber,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Regarding = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdFBActivity = xrmManager.Create<FacebookActivity>(fbActivity);
            return createdFBActivity.Id;
        }

        private Guid CreateCallGuideInfo(CallGuideRequest callGuideRequest)
        {
            CallGuideInfo callguideInfo = new CallGuideInfo
            {

                CallGuideInfoName = callGuideRequest.CallGuideSessionId,
                AgentName = callGuideRequest.AgentName,
                APhoneNumber = callGuideRequest.APhoneNumber,
                BPhoneNumber = callGuideRequest.BPhoneNumber,
                CallGuideSessionID = callGuideRequest.CallGuideSessionId,
                CId = callGuideRequest.CId,
                ContactSourceType = callGuideRequest.ContactSourceType,
                ErrandTaskType = callGuideRequest.ErrandTaskType,
                QueueTime = callGuideRequest.QueueTime,
                ScreenPopChoice = callGuideRequest.ScreenPopChoice,
                ChatCustomerAlias = callGuideRequest.ChatCustomerAlias


            };

            Entity createdCallGuideInfo = xrmManager.Create<CallGuideInfo>(callguideInfo);
            return createdCallGuideInfo.Id;
        }



        private Guid GetAccountByPhoneNumber(string phoneNumber)
        {
            Guid retValue = Guid.Empty;

            if (string.IsNullOrEmpty(phoneNumber))
                return retValue;

            string _number = _formatPhoneNumber(phoneNumber);

            QueryExpression queryExpression = new QueryExpression("account");

            queryExpression.ColumnSet = new ColumnSet();
            queryExpression.ColumnSet.AddColumn("accountid");
            //FilterExpression filterExpression = new FilterExpression();
            //filterExpression.AddCondition(new ConditionExpression("telephone1", ConditionOperator.Equal, phoneNumber));
            //filterExpression.AddCondition(new ConditionExpression("telephone2", ConditionOperator.Equal, phoneNumber));
            //filterExpression.AddCondition(new ConditionExpression("telephone3", ConditionOperator.Equal, phoneNumber));

            //filterExpression.FilterOperator = LogicalOperator.Or;
            //queryExpression.Criteria = filterExpression;

            FilterExpression filterPhoneExpression = new FilterExpression();
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone1", ConditionOperator.Equal, _number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone2", ConditionOperator.Equal, _number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone3", ConditionOperator.Equal, _number));
            filterPhoneExpression.FilterOperator = LogicalOperator.Or;

            FilterExpression filterStateExpression = new FilterExpression();
            filterStateExpression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            filterStateExpression.Filters.Add(filterPhoneExpression);
            filterStateExpression.FilterOperator = LogicalOperator.And;
            queryExpression.Criteria = filterStateExpression;


            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                if (entityCollection.Entities.Count == 1)
                {
                    retValue = entityCollection.Entities[0].Attributes.Contains("accountid") ? entityCollection.Entities[0].GetAttributeValue<Guid>("accountid") : Guid.Empty;
                }

                return retValue;
            }
            else
            {
                return retValue;
                //throw new Exception("No Matching User Found");
            }
        }

        private Guid GetContactByPhoneNumber(string phoneNumber)
        {
            Guid retValue = Guid.Empty;

            if (string.IsNullOrEmpty(phoneNumber))
                return retValue;

            string _number = _formatPhoneNumber(phoneNumber);

            QueryExpression queryExpression = new QueryExpression("contact");

            queryExpression.ColumnSet = new ColumnSet();
            queryExpression.ColumnSet.AddColumn("contactid");
            FilterExpression filterPhoneExpression = new FilterExpression();
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone1", ConditionOperator.Equal, _number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone2", ConditionOperator.Equal, _number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone3", ConditionOperator.Equal, _number));
            filterPhoneExpression.FilterOperator = LogicalOperator.Or;

            FilterExpression filterStateExpression = new FilterExpression();
            filterStateExpression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            filterStateExpression.Filters.Add(filterPhoneExpression);
            filterStateExpression.FilterOperator = LogicalOperator.And;
            queryExpression.Criteria = filterStateExpression;



            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                if (entityCollection.Entities.Count == 1)
                {
                    retValue = entityCollection.Entities[0].Attributes.Contains("contactid") ? entityCollection.Entities[0].GetAttributeValue<Guid>("contactid") : Guid.Empty;
                }

                //return retValue;
            }
            //else
            //{
            return retValue;
            //throw new Exception("No Matching User Found");
            //}
        }

        private string _formatPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string _phonenumber = "";

            try
            {
                _phonenumber = number;

                if (number.Substring(0, 1) == "+")
                {
                    _phonenumber = _phonenumber.Substring(3, (number.Length - 3));
                }

                int _first = 0;
                int.TryParse(_phonenumber.Substring(0, 1), out _first);
                if (_first > 0)
                {
                    _phonenumber = string.Format("0{0}", _phonenumber);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _phonenumber;
        }

        private bool CheckCallGuideInfoExist(string sessionid, out Guid callguideInfoId)
        {
            callguideInfoId = xrmHelper.GetId(sessionid, "cgi_callguidesessionid", "cgi_callguideinfo", xrmManager);
            if (callguideInfoId != Guid.Empty)
                return true;

            return false;
        }

        public bool IsDuplicateRequest(CallGuideRequest callguideRequest)
        {
            QueryExpression queryCallGuideInfo = new QueryExpression("cgi_callguideinfo");

            ConditionExpression conditionExpression1 = new ConditionExpression("cgi_callguidesessionid", ConditionOperator.Equal, callguideRequest.CallGuideSessionId);
            ConditionExpression conditionExpression2 = new ConditionExpression("cgi_aphonenumber", ConditionOperator.Equal, callguideRequest.APhoneNumber);
            ConditionExpression conditionExpression3 = new ConditionExpression("cgi_bphonenumber", ConditionOperator.Equal, callguideRequest.BPhoneNumber);
            ConditionExpression conditionExpression4 = new ConditionExpression("cgi_agentname", ConditionOperator.Equal, callguideRequest.AgentName);

            FilterExpression filterExpression = new FilterExpression();
            filterExpression.AddCondition(conditionExpression1);
            filterExpression.AddCondition(conditionExpression2);
            filterExpression.AddCondition(conditionExpression3);
            filterExpression.AddCondition(conditionExpression4);
            filterExpression.FilterOperator = LogicalOperator.And;

            queryCallGuideInfo.ColumnSet = new ColumnSet(new string[] { "cgi_callguideinfoid" });
            queryCallGuideInfo.Criteria.AddFilter(filterExpression);

            EntityCollection callguideInfos = xrmManager.Service.RetrieveMultiple(queryCallGuideInfo);
            if (callguideInfos == null)
                return false;
            else
            {
                if (callguideInfos.Entities != null && callguideInfos.Entities.Count > 0)
                    return true;
                else
                    return false;
            }


        }
        private CategoryDetail GetCategoryDetailByCallGuideCategory(string callguideCategory)
        {
            CategoryDetail categoryDetail = null;
            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName = "cgi_categorydetail";
            queryByAttribute.ColumnSet = new ColumnSet(true);

            queryByAttribute.AddAttributeValue("cgi_callguidecategory", callguideCategory);
            ObservableCollection<CategoryDetail> categoryDetails = xrmManager.Get<CategoryDetail>(queryByAttribute);
            if (categoryDetails != null && categoryDetails.Count > 0)
                categoryDetail = categoryDetails[0];

            return categoryDetail;
        }
        private EntityCollection GetOpenChatActivity()
        {
            QueryExpression queryExpChatActivity = new QueryExpression("cgi_callguidechat");
            queryExpChatActivity.ColumnSet = new ColumnSet();
            queryExpChatActivity.ColumnSet.AddColumn("activityid");

            queryExpChatActivity.LinkEntities.Add(new LinkEntity("cgi_callguidechat", "cgi_callguideinfo", "cgi_callguideinfoid", "cgi_callguideinfoid", JoinOperator.Inner));
            queryExpChatActivity.LinkEntities[0].Columns.AddColumns("cgi_callguidesessionid");
            queryExpChatActivity.LinkEntities[0].EntityAlias = "callguidechatinfo";

            queryExpChatActivity.Criteria = new FilterExpression(LogicalOperator.And);
            queryExpChatActivity.Criteria.AddCondition(new ConditionExpression("cgi_createcase", ConditionOperator.Equal, true));
            queryExpChatActivity.Criteria.AddCondition(new ConditionExpression("statuscode", ConditionOperator.Equal, 1));
            return xrmManager.Service.RetrieveMultiple(queryExpChatActivity);
        }

        #endregion

        #region [Public Methods]

        #region [Shared]
        public Guid GetAgentUserId(string callguideAgentName)
        {
            Guid callerId = xrmHelper.GetId(callguideAgentName, "cgi_callguideusername", "systemuser", xrmManager);
            if (callerId != Guid.Empty)
                ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;

            return callerId;
        }
        public bool UpdateDuration(CallGuideRequest callguideRequest, Guid callerId)
        {

            xrmHelper.UpdateXrmManagerUser(ref xrmManager, callerId);

            Guid callGuideInfoId = xrmHelper.GetId(callguideRequest.CallGuideSessionId, "cgi_callguidesessionid", "cgi_callguideinfo", xrmManager);


            if (callGuideInfoId != null && callGuideInfoId != Guid.Empty)
            {
                Entity updateEntity = new Entity("cgi_callguideinfo");
                updateEntity.Id = callGuideInfoId;
                updateEntity.Attributes.Add("cgi_duration", callguideRequest.CallDuration);
                xrmManager.Update(updateEntity);
                return true;
            }
            else
            {
                //throw new Exception("");
                return false;
            }
        }
        public Settings GetXrmSettings()
        {
            return xrmHelper.XrmSettings;
        }
        #endregion

        #region [Chat]

        public Guid ExecuteChatRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            xrmHelper.UpdateXrmManagerUser(ref xrmManager, callerId);

            Guid returnValue = Guid.Empty;
            Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
            if (callguideInfoId != null && callguideInfoId != Guid.Empty)
            {
                //Guid accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);

                Guid chatActivityId = CreateChatActivity(callguideRequest, callguideInfoId, Guid.Empty, callerId);
                returnValue = chatActivityId;
            }
            return returnValue;
        }

        #endregion

        #region [FB]
        public Guid ExecuteFBRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            xrmHelper.UpdateXrmManagerUser(ref xrmManager, callerId);

            Guid returnValue = Guid.Empty;
            Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
            if (callguideInfoId != null && callguideInfoId != Guid.Empty)
            {
                //Guid accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
                Guid fbActivityId = CreateFBActivity(callguideRequest, callguideInfoId, Guid.Empty, callerId);
                returnValue = fbActivityId;
            }
            return returnValue;
        }
        #endregion

        #region [PhoneCall]
        public CallGuideRouteAction GetCallRouteAction(string bPhoneNumber)
        {
            CallGuideRouteAction retValCallGuideRouteAction = CallGuideRouteAction.None;

            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName = "cgi_routingaction";
            queryByAttribute.ColumnSet = new ColumnSet();
            queryByAttribute.ColumnSet.AddColumn("cgi_action");
            queryByAttribute.AddAttributeValue("cgi_routingactionnumber", bPhoneNumber);
            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                int valRouteAction = entityCollection.Entities[0].Attributes.Contains("cgi_action") ? entityCollection.Entities[0].GetAttributeValue<OptionSetValue>("cgi_action").Value : 0;
                if (valRouteAction > 0)
                    retValCallGuideRouteAction = (CallGuideRouteAction)valRouteAction;

            }


            return retValCallGuideRouteAction;
        }
        public Guid ExecuteNewCallRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            xrmHelper.UpdateXrmManagerUser(ref xrmManager, callerId);

            Guid returnValue = Guid.Empty;

            Guid callguideInfoId;
            //if (!CheckCallGuideInfoExist(callguideRequest.CallGuideSessionId, out callguideInfoId))
            callguideInfoId = CreateCallGuideInfo(callguideRequest);

            if (callguideInfoId != null && callguideInfoId != Guid.Empty)
            {
                Guid accountId = GetContactByPhoneNumber(callguideRequest.APhoneNumber);
                callguideRequest.CustomerType = AccountCategoryCode.None;
                if (accountId == Guid.Empty)
                {
                    accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
                    if (accountId != Guid.Empty)
                    {
                        callguideRequest.CustomerType = AccountCategoryCode.Company;
                    }
                }
                else
                {
                    callguideRequest.CustomerType = AccountCategoryCode.Private;
                }
                Guid phoneCallActivityId = CreatePhoneCallActivity(callguideRequest, callguideInfoId, accountId, callerId);

                if (callguideRequest.CallRouteAction == CallGuideRouteAction.Case)
                {
                    Guid incidentId = CreateIncident(callguideRequest, accountId, callguideInfoId);
                    //if (incidentId != null && incidentId != Guid.Empty)
                    //{
                    //    xrmHelper.CreateCaseCategory(incidentId, callguideRequest.ErrandTaskType, xrmManager);
                    //}
                    returnValue = incidentId;
                }
                else if (callguideRequest.CallRouteAction == CallGuideRouteAction.Account)
                {
                    returnValue = accountId;
                }
                else if (callguideRequest.CallRouteAction == CallGuideRouteAction.Activity)
                {
                    returnValue = phoneCallActivityId;
                }
            }
            return returnValue;
        }

        public void LogString(string path, string message)
        {
            try
            {
                StreamWriter _sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                string _message = string.Format("{0} : {1}", DateTime.Now, message);
                _sw.WriteLine(_message);
                _sw.Flush();
                _sw.Close();
            }
            catch
            {
                throw;
            }
        }

        public Guid ExecuteManualRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            xrmHelper.UpdateXrmManagerUser(ref xrmManager, callerId);

            Guid returnValue = Guid.Empty;

            LogString("C:\\Temp\\CallGuide.txt", "Enter ExecuteManualRequest");
            //if (!CheckCallGuideInfoExist(callguideRequest.CallGuideSessionId, out callguideInfoId))
            Guid callguideInfoId = xrmHelper.GetId(callguideRequest.CallGuideSessionId, "cgi_callguidesessionid", "cgi_callguideinfo", xrmManager);
            if (callguideInfoId == Guid.Empty)
            {
                callguideInfoId = CreateCallGuideInfo(callguideRequest);
            }
            if (callguideInfoId != Guid.Empty)
            {
                Guid accountId = GetContactByPhoneNumber(callguideRequest.APhoneNumber);
                callguideRequest.CustomerType = AccountCategoryCode.None;
                if (accountId == Guid.Empty)
                {
                    accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
                    if (accountId != Guid.Empty)
                    {
                        callguideRequest.CustomerType = AccountCategoryCode.Company;
                    }
                }
                else
                {
                    callguideRequest.CustomerType = AccountCategoryCode.Private;
                }
                //Guid phoneCallActivityId = CreatePhoneCallActivity(callguideRequest, callguideInfoId, accountId, callerId);
                LogString("C:\\Temp\\CallGuide.txt", "Before Create Incident");
                Guid incidentId = CreateIncident(callguideRequest, accountId, callguideInfoId);
                LogString("C:\\Temp\\CallGuide.txt", "After Create Incident");
                //if (incidentId != null && incidentId != Guid.Empty)
                //{
                //    xrmHelper.CreateCaseCategory(incidentId, callguideRequest.ErrandTaskType, xrmManager);
                //}
                returnValue = incidentId;


            }
            return returnValue;
        }
        #endregion






        //public Guid ExecuteCallTransferRequest(Guid callguideInfoId, Guid callerId)
        //{   
        //    //Guid caseId = xrmHelper.GetId(callguideRequest.CallGuideSessionId, "cgi_callguidesessionid", "cgi_callguideinfo", xrmManager);
        //    return callguideInfoId;
        //}



        #endregion

        #region [IDispose]
        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        ~CallGuideHandler()
        {
            Dispose(false);
        }
        #endregion



    }
}

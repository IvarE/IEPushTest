using System;
using System.Collections.Generic;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.ObjectModel;
using System.IO;
using CGIXrmHandler.CallGuide.Models;
using CGIXrmHandler.CrmClasses;
using CGIXrmHandler.Shared;

namespace CGIXrmHandler
{
    public class CallGuideHandler : IDisposable
    {
        #region Declarations
        private XrmManager _xrmManager;
        private readonly XrmHelper _xrmHelper;
        // Flag: Has Dispose already been called? 
        bool _disposed;
        #endregion

        #region Constructors
        public CallGuideHandler()
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }
        public CallGuideHandler(Guid callerId)
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        public CallGuideHandler(Guid callerId, Settings settings)
        {
            _xrmHelper = new XrmHelper(settings);
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        #endregion

        #region Private Methods

        private Guid CreateIncident(CallGuideRequest callguideRequest, Guid accountIdOrContactId, Guid callguideInfoId)
        {
            CategoryDetail categoryDetail = GetCategoryDetailByCallGuideCategory(callguideRequest.ErrandTaskType);
            IncidentExtended incident = new IncidentExtended
            {
                Account = accountIdOrContactId == Guid.Empty ? null : (callguideRequest.CustomerType == AccountCategoryCode.Company ? new EntityReference("account", accountIdOrContactId) : null),
                Contact = accountIdOrContactId == Guid.Empty ? null : (callguideRequest.CustomerType == AccountCategoryCode.Private ? new EntityReference("contact", accountIdOrContactId) : null),
                DefaultCustomer = accountIdOrContactId == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : (callguideRequest.CustomerType == AccountCategoryCode.Company ? new EntityReference("account", accountIdOrContactId) : new EntityReference("contact", accountIdOrContactId)),
                CaseOrigin = GetCaseOrigin(callguideRequest.ContactSourceType),
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
                }
            }

            Entity createCase = _xrmManager.Create(incident);

            return createCase.Id;
        }

        private OptionSetValue GetCaseOrigin(string contactSourceType)
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

        private Guid CreatePhoneCallActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity
            {
                LogicalName = "activityparty"
            };
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>
            {
                entityFrom
            };


            Entity entityTo = new Entity
            {
                LogicalName = "activityparty"
            };
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>
            {
                entityTo
            };

            PhoneCallActivity phoneCallActivity = new PhoneCallActivity
            {
                Subject = "Incoming call from " + CallGuideRequest.APhoneNumber,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Direction = Convert.ToBoolean(CallGuideRequest.CallDirection),
                PhoneNumber = CallGuideRequest.APhoneNumber,
                Regarding = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdPhoneCallActivity = _xrmManager.Create(phoneCallActivity);
            return createdPhoneCallActivity.Id;
        }

        private Guid CreateChatActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity
            {
                LogicalName = "activityparty"
            };
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>
            {
                entityFrom
            };


            Entity entityTo = new Entity
            {
                LogicalName = "activityparty"
            };
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>
            {
                entityTo
            };

            ChatActivity chatActivity = new ChatActivity
            {
                Subject = "Incoming Chat :" + CallGuideRequest.ChatCustomerAlias,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Regarding = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdChatActivity = _xrmManager.Create(chatActivity);
            return createdChatActivity.Id;
        }

        private Guid CreateFBActivity(CallGuideRequest CallGuideRequest, Guid callguideInfoId, Guid accountid, Guid callerId)
        {
            Entity entityFrom = new Entity
            {
                LogicalName = "activityparty"
            };
            entityFrom.Attributes["partyid"] = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid);


            List<Entity> lstFromParty = new List<Entity>
            {
                entityFrom
            };


            Entity entityTo = new Entity
            {
                LogicalName = "activityparty"
            };
            entityTo.Attributes["partyid"] = new EntityReference("systemuser", callerId);

            List<Entity> lstToParty = new List<Entity>
            {
                entityTo
            };

            FacebookActivity fbActivity = new FacebookActivity
            {
                Subject = "Incoming Post " + CallGuideRequest.APhoneNumber,
                CallFrom = new EntityCollection(lstFromParty),
                CallTo = new EntityCollection(lstToParty),
                CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
                Regarding = accountid == Guid.Empty ? _xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
            };

            Entity createdFbActivity = _xrmManager.Create(fbActivity);
            return createdFbActivity.Id;
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

            Entity createdCallGuideInfo = _xrmManager.Create(callguideInfo);
            return createdCallGuideInfo.Id;
        }



        private Guid GetAccountByPhoneNumber(string phoneNumber)
        {
            Guid retValue = Guid.Empty;

            if (string.IsNullOrEmpty(phoneNumber))
                return retValue;

            string number = _formatPhoneNumber(phoneNumber);

            QueryExpression queryExpression = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet()
            };

            queryExpression.ColumnSet.AddColumn("accountid");

            FilterExpression filterPhoneExpression = new FilterExpression();
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone1", ConditionOperator.Equal, number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone2", ConditionOperator.Equal, number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone3", ConditionOperator.Equal, number));
            filterPhoneExpression.FilterOperator = LogicalOperator.Or;

            FilterExpression filterStateExpression = new FilterExpression();
            filterStateExpression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            filterStateExpression.Filters.Add(filterPhoneExpression);
            filterStateExpression.FilterOperator = LogicalOperator.And;
            queryExpression.Criteria = filterStateExpression;


            EntityCollection entityCollection = _xrmManager.Service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                if (entityCollection.Entities.Count == 1)
                {
                    retValue = entityCollection.Entities[0].Attributes.Contains("accountid") ? entityCollection.Entities[0].GetAttributeValue<Guid>("accountid") : Guid.Empty;
                }

                return retValue;
            }
            return retValue;
        }

        private Guid GetContactByPhoneNumber(string phoneNumber)
        {
            Guid retValue = Guid.Empty;

            if (string.IsNullOrEmpty(phoneNumber))
                return retValue;

            string number = _formatPhoneNumber(phoneNumber);

            QueryExpression queryExpression = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet()
            };

            queryExpression.ColumnSet.AddColumn("contactid");
            FilterExpression filterPhoneExpression = new FilterExpression();
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone1", ConditionOperator.Equal, number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone2", ConditionOperator.Equal, number));
            filterPhoneExpression.AddCondition(new ConditionExpression("telephone3", ConditionOperator.Equal, number));
            filterPhoneExpression.FilterOperator = LogicalOperator.Or;

            FilterExpression filterStateExpression = new FilterExpression();
            filterStateExpression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            filterStateExpression.Filters.Add(filterPhoneExpression);
            filterStateExpression.FilterOperator = LogicalOperator.And;
            queryExpression.Criteria = filterStateExpression;



            EntityCollection entityCollection = _xrmManager.Service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                if (entityCollection.Entities.Count == 1)
                {
                    retValue = entityCollection.Entities[0].Attributes.Contains("contactid") ? entityCollection.Entities[0].GetAttributeValue<Guid>("contactid") : Guid.Empty;
                }
            }
            return retValue;
        }

        private string _formatPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string phonenumber;

            try
            {
                phonenumber = number;

                if (number.Substring(0, 1) == "+")
                {
                    phonenumber = phonenumber.Substring(3, (number.Length - 3));
                }

                int first = 0;
                int.TryParse(phonenumber.Substring(0, 1), out first);
                if (first > 0)
                {
                    phonenumber = string.Format("0{0}", phonenumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return phonenumber;
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

        #endregion

        #region Public Methods

        #region [Shared]
        public Guid GetAgentUserId(string callguideAgentName)
        {
            Guid callerId = _xrmHelper.GetId(callguideAgentName, "cgi_callguideusername", "systemuser", _xrmManager);
            if (callerId != Guid.Empty)
                ((OrganizationServiceProxy)_xrmManager.Service.InnerService).CallerId = callerId;

            return callerId;
        }
        public bool UpdateDuration(CallGuideRequest callguideRequest, Guid callerId)
        {

            _xrmHelper.UpdateXrmManagerUser(ref _xrmManager, callerId);

            Guid callGuideInfoId = _xrmHelper.GetId(callguideRequest.CallGuideSessionId, "cgi_callguidesessionid", "cgi_callguideinfo", _xrmManager);

            if (callGuideInfoId != Guid.Empty)
            {
                Entity updateEntity = new Entity("cgi_callguideinfo")
                {
                    Id = callGuideInfoId
                };
                updateEntity.Attributes.Add("cgi_duration", callguideRequest.CallDuration);
                _xrmManager.Update(updateEntity);
                return true;
            }
            return false;
        }
        public Settings GetXrmSettings()
        {
            return _xrmHelper.XrmSettings;
        }
        #endregion

        #region [Chat]

        public Guid ExecuteChatRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            _xrmHelper.UpdateXrmManagerUser(ref _xrmManager, callerId);

            Guid returnValue = Guid.Empty;
            Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
            if (callguideInfoId != Guid.Empty)
            {
                Guid chatActivityId = CreateChatActivity(callguideRequest, callguideInfoId, Guid.Empty, callerId);
                returnValue = chatActivityId;
            }
            return returnValue;
        }

        #endregion

        #region [FB]
        public Guid ExecuteFBRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            _xrmHelper.UpdateXrmManagerUser(ref _xrmManager, callerId);

            Guid returnValue = Guid.Empty;
            Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
            if (callguideInfoId != Guid.Empty)
            {
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

            QueryByAttribute queryByAttribute = new QueryByAttribute
            {
                EntityName = "cgi_routingaction",
                ColumnSet = new ColumnSet()
            };
            queryByAttribute.ColumnSet.AddColumn("cgi_action");
            queryByAttribute.AddAttributeValue("cgi_routingactionnumber", bPhoneNumber);
            EntityCollection entityCollection = _xrmManager.Service.RetrieveMultiple(queryByAttribute);
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
            _xrmHelper.UpdateXrmManagerUser(ref _xrmManager, callerId);

            Guid returnValue = Guid.Empty;

            var callguideInfoId = CreateCallGuideInfo(callguideRequest);

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
                Guid phoneCallActivityId = CreatePhoneCallActivity(callguideRequest, callguideInfoId, accountId, callerId);

                if (callguideRequest.CallRouteAction == CallGuideRouteAction.Case)
                {
                    Guid incidentId = CreateIncident(callguideRequest, accountId, callguideInfoId);
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
                StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                message = string.Format("{0} : {1}", DateTime.Now, message);
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Guid ExecuteManualRequest(CallGuideRequest callguideRequest, Guid callerId)
        {
            _xrmHelper.UpdateXrmManagerUser(ref _xrmManager, callerId);

            Guid returnValue = Guid.Empty;

            LogString("C:\\Temp\\CallGuide.txt", "Enter ExecuteManualRequest");
            Guid callguideInfoId = _xrmHelper.GetId(callguideRequest.CallGuideSessionId, "cgi_callguidesessionid", "cgi_callguideinfo", _xrmManager);
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
                LogString("C:\\Temp\\CallGuide.txt", "Before Create Incident");
                Guid incidentId = CreateIncident(callguideRequest, accountId, callguideInfoId);
                LogString("C:\\Temp\\CallGuide.txt", "After Create Incident");
                returnValue = incidentId;
            }
            return returnValue;
        }
        #endregion

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

            queryCallGuideInfo.ColumnSet = new ColumnSet("cgi_callguideinfoid");
            queryCallGuideInfo.Criteria.AddFilter(filterExpression);

            EntityCollection callguideInfos = _xrmManager.Service.RetrieveMultiple(queryCallGuideInfo);
            if (callguideInfos == null)
                return false;
            if (callguideInfos.Entities != null && callguideInfos.Entities.Count > 0)
                return true;
            return false;
        }



        #endregion

        #region [IDispose]

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        ~CallGuideHandler()
        {
            Dispose(false);
        }
        #endregion
    }
}

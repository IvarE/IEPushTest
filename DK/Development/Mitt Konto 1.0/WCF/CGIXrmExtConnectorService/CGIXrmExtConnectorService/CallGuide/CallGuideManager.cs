using System;
using CGIXrmExtConnectorService;
using CGIXrmWin;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
public class CallGuideManager
{
    private XrmManager xrmManager;
    private XrmHelper xrmHelper;

    #region [Constructor]
    public CallGuideManager()
    {
        xrmHelper = new XrmHelper();
        xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
    }
    public CallGuideManager(Guid callerId)
    {
        xrmHelper = new XrmHelper();
        xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
    }
    #endregion

    #region [Private Methods]

    private Guid CreateIncident(CallGuideRequest CallGuideRequest,Guid accountId,Guid callguideInfoId)
    {
        Incident incident = new Incident
        {
            Account = accountId == Guid.Empty ? null : new EntityReference("account", accountId),
            DefaultCustomer = accountId == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountId),            
            CaseOrigin = new Microsoft.Xrm.Sdk.OptionSetValue((int)CaseOrgin.PhoneCall),
            CallGuideInfo = new EntityReference("cgi_callguideinfo",callguideInfoId),
            TelephoneNumber = CallGuideRequest.APhoneNumber
        };
        Entity createCase = xrmManager.Create<Incident>(incident);
        return createCase.Id;
    }

    private Guid CreatePhoneCallActivity(CallGuideRequest CallGuideRequest,Guid callguideInfoId, Guid accountid,Guid callerId)
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
            Subject = "Incoming chat " + CallGuideRequest.APhoneNumber,
            CallFrom = new EntityCollection(lstFromParty),
            CallTo = new EntityCollection(lstToParty),
            CallGuideInfo = new EntityReference("cgi_callguideinfo", callguideInfoId),
            Direction = Convert.ToBoolean(CallGuideRequest.CallDirection),
            PhoneNumber = CallGuideRequest.APhoneNumber,
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
            Direction = Convert.ToBoolean(CallGuideRequest.CallDirection),
            PhoneNumber = CallGuideRequest.APhoneNumber,
            Regarding = accountid == Guid.Empty ? xrmHelper.XrmSettings.DefaultCustomerOnCase : new EntityReference("account", accountid),
        };

        Entity createdChatActivity = xrmManager.Create<FacebookActivity>(fbActivity);
        return createdChatActivity.Id;
    }

    private Guid CreateCallGuideInfo(CallGuideRequest CallGuideRequest)
    {
        CallGuideInfo callguideInfo = new CallGuideInfo
        {
            
            AgentName = CallGuideRequest.AgentName,
            APhoneNumber = CallGuideRequest.APhoneNumber,
            BPhoneNumber = CallGuideRequest.BPhoneNumber,
            CallGuideSessionID = CallGuideRequest.CallGuideSessionID,
            
            CId = CallGuideRequest.CId,
            ContactSourceType = CallGuideRequest.ContactSourceType,
            ErrandTaskType = CallGuideRequest.ErrandTaskType,
            QueueTime = CallGuideRequest.QueueTime,
            ScreenPopChoice = CallGuideRequest.ScreenPopChoice,
            

        };

        Entity createdCallGuideInfo= xrmManager.Create<CallGuideInfo>(callguideInfo);
        return createdCallGuideInfo.Id;
    }

    private Guid CreateCaseCategory(Guid caseId, string errandTaskType)
    {
        CaseCatergory caseCategory = new CaseCatergory();
        Entity catergoryDetail = GetCategoryDetail(errandTaskType, "cgi_callguidecategory");
        string caseCategoryName = string.Empty;

        if (catergoryDetail != null)
        {
            caseCategory.CaseCategory1 = new EntityReference("cgi_categorydetail", catergoryDetail.Id);
            caseCategoryName = catergoryDetail.Contains("cgi_categorydetailname") ? catergoryDetail.GetAttributeValue<string>("cgi_categorydetailname") : string.Empty;

            if (catergoryDetail.Attributes.Contains("cgi_parentid"))
            {
                EntityReference entityReference1 = catergoryDetail.GetAttributeValue<EntityReference>("cgi_parentid");
                caseCategory.CaseCategory2 = caseCategory.CaseCategory1;
                caseCategory.CaseCategory1 = entityReference1;

                caseCategoryName = entityReference1.Name + " " + caseCategoryName;
                catergoryDetail = GetCategoryDetail(entityReference1.Id.ToString(), "cgi_categorydetailid");
                if (catergoryDetail.Attributes.Contains("cgi_parentid"))
                {
                    EntityReference entityReference2 = catergoryDetail.GetAttributeValue<EntityReference>("cgi_parentid");
                    caseCategory.CaseCategory3 = caseCategory.CaseCategory2;
                    caseCategory.CaseCategory2 = caseCategory.CaseCategory1;
                    caseCategory.CaseCategory1 = entityReference2;
                    caseCategoryName = entityReference2.Name + " " + caseCategoryName;
                }


            }
            caseCategory.CaseCategoryName = caseCategoryName;
            caseCategory.CaseId = new EntityReference("incident", caseId);

        }
        Entity createdCaseCategory = xrmManager.Create<CaseCatergory>(caseCategory);
        return createdCaseCategory.Id;
    }

    private Guid GetAccountByPhoneNumber(string phoneNumber)
    {
        Guid retValue = Guid.Empty;
        QueryByAttribute queryByAttribute = new QueryByAttribute();
        queryByAttribute.EntityName = "cgi_telephone";
        ColumnSet resultColumnSet = new ColumnSet();
        resultColumnSet.AddColumns(new string[] { "cgi_accountid", "cgi_contactid" });
        queryByAttribute.ColumnSet = resultColumnSet;
        queryByAttribute.AddAttributeValue("cgi_phonenumber", phoneNumber);
        EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
        if (entityCollection != null && entityCollection.Entities.Count > 0)
        {
            if (entityCollection.Entities.Count == 1)
            {
                retValue = entityCollection.Entities[0].Attributes.Contains("cgi_accountid") ? entityCollection.Entities[0].GetAttributeValue<EntityReference>("cgi_accountid").Id : entityCollection.Entities[0].Attributes.Contains("cgi_contactid") ?entityCollection.Entities[0].GetAttributeValue<EntityReference>("cgi_contactid").Id:Guid.Empty;
            }

            return retValue;
        }
        else
        {
            throw new Exception("No Matching User Found");
        }
    }

    private Entity GetCategoryDetail(string searchValue, string searchAttribute)
    {
        QueryByAttribute queryByAttribute = new QueryByAttribute();
        queryByAttribute.EntityName = "cgi_categorydetail";
        queryByAttribute.ColumnSet = new ColumnSet(true);        
        queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
        EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
        if (entityCollection != null && entityCollection.Entities.Count > 0)
            return entityCollection[0];
        else
            throw new Exception("No Matching User Found");
    }

   
    //private string GenerateCaseUrl(Guid caseId)
    //{
    //    string baseCaseUrl = ConfigurationManager.AppSettings["CaseUrl"].ToString();
    //    return string.Format(baseCaseUrl, caseId);
    //}

    //private string GenerateActivityUrl(Guid activityId)
    //{
    //    string baseCaseUrl = ConfigurationManager.AppSettings["CaseUrl"].ToString();
    //    return string.Format(baseCaseUrl, activityId);
    //}
    #endregion

   
    internal string GetChat(string interactionId)
    {
        return "";
    }

    internal string GetFBMessage(string interactionId)
    {
        return "";
    }

    internal Guid ExecuteNewCallRequest(CallGuideRequest callguideRequest,Guid callerId)
    {
        Guid returnValue = Guid.Empty;
        Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);

        if (callguideInfoId != null && callguideInfoId != Guid.Empty)
        {
            Guid accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
            Guid phoneCallActivityId = CreatePhoneCallActivity(callguideRequest, callguideInfoId, accountId, callerId);
            
            if (callguideRequest.CallGuideAction == CallGuideAction.Case)
            {
                Guid incidentId = CreateIncident(callguideRequest, accountId, callguideInfoId);
                if (incidentId != null && incidentId != Guid.Empty)
                {
                    CreateCaseCategory(incidentId, callguideRequest.ErrandTaskType);                    
                }
            }
            else if (callguideRequest.CallGuideAction == CallGuideAction.Account)
            {
                returnValue = accountId;
            }
        }
        return returnValue;
    }

    internal Guid ExecuteCallTransferRequest(string callguidSessionId)
    {
        Guid caseId = xrmHelper.GetId(callguidSessionId, "cgi_callguidesessionid", "incident", xrmManager);
        return caseId;
    }

    internal Guid ExecuteChatRequest(CallGuideRequest callguideRequest,Guid callerId)
    {
        Guid returnValue = Guid.Empty;
        Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
        if (callguideInfoId != null && callguideInfoId != Guid.Empty)
        {
            Guid accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
            Guid chatActivityId = CreateChatActivity(callguideRequest, callguideInfoId, accountId, callerId);
            returnValue = chatActivityId;
        }
        return returnValue;
    }

    internal Guid ExecuteFBRequest(CallGuideRequest callguideRequest, Guid callerId)
    {
        Guid returnValue = Guid.Empty;
        Guid callguideInfoId = CreateCallGuideInfo(callguideRequest);
        if (callguideInfoId != null && callguideInfoId != Guid.Empty)
        {
            Guid accountId = GetAccountByPhoneNumber(callguideRequest.APhoneNumber);
            Guid chatActivityId = CreateChatActivity(callguideRequest, callguideInfoId, accountId, callerId);
            returnValue = chatActivityId;
        }
        return returnValue;
    }

}
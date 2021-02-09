using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Generic=System.Collections.Generic;
using System.Threading;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Dynamic;
using CGIXrm;
using CGIXrm.CrmSdk;

namespace Crm.FormAssistantPanel.Utilities
{
    public class CrmHelper
    {
        WebParameters webParams;
        CrmManager crmManager;
        public CrmHelper()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                webParams = new WebParameters();
                crmManager = new CrmManager(webParams.ServerAddress);
            });
        }
        
        public delegate void GetPanelConfigurationCallbackHandler(List<PanelConfiguration> panelConfigurations,bool operationSuccessState);        
        //public delegate void ProcessViewDataRequestCallbackHandler(ObservableCollection<View> ViewData, string controlName, string title, bool operationSuccessState);
        public delegate void ProcessViewDataRequestCallbackHandler(ObservableCollection<View> ViewData, string panelConfigRecordGuid, bool operationSuccessState);        

        private static IOrganizationService crmService;
        public static IOrganizationService CrmService
        {
            get
            {
                if (crmService == null)
                {
                    crmService = XRMUtility.GetSoapService();
                }
                return crmService;
            }
        }


        internal void GetPanelConfiguration(string entityName,GetPanelConfigurationCallbackHandler getPanelConfigurationCallbackHandler)
        {
            QueryExpression queryExpression = new QueryExpression();
            queryExpression.EntityName = "cgi_uapanelconfiguration";
            queryExpression.ColumnSet = new ColumnSet() { AllColumns = true };
            queryExpression.Criteria = new FilterExpression()
            {
                Conditions = new ObservableCollection<ConditionExpression>() 
                { 
                    new ConditionExpression() 
                    {
                        AttributeName = "cgi_entityscope", 
                        Operator = ConditionOperator.Equal, 
                        Values = new ObservableCollection<object>() 
                        { 
                            entityName
                        } 
                    
                    },
                    new ConditionExpression()
                    {
                        AttributeName="statecode",
                        Operator=ConditionOperator.Equal,
                        Values=new ObservableCollection<object>()
                        {
                            0
                        }

                    }
                },
                FilterOperator = LogicalOperator.And
            };
            
            CrmService.BeginRetrieveMultiple(queryExpression, GetPanelConfiguration_CallBack, getPanelConfigurationCallbackHandler);
            
            

        }
        private void GetPanelConfiguration_CallBack(IAsyncResult result)
        {
            //SilverlightUtility.SetUICulture(System.Threading.Thread.CurrentThread);
            List<PanelConfiguration> panelConfigurations = new List<PanelConfiguration>();
            
            EntityCollection panelConfigs = CrmService.EndRetrieveMultiple(result);

            panelConfigurations = (from pc in panelConfigs.Entities                                   
                                  select new PanelConfiguration()
                                  {
                                      ConfigurationID = pc.Attributes.ContainsKey("cgi_panelconfigid") ? pc.GetAttributeValue<string>("cgi_panelconfigid") : string.Empty,
                                      ColumnHeader = pc.Attributes.ContainsKey("cgi_columnheader") ? pc.GetAttributeValue<string>("cgi_columnheader") : string.Empty,
                                      PageSize = pc.Attributes.ContainsKey("cgi_pagesize") ? pc.GetAttributeValue<string>("cgi_pagesize") : string.Empty,
                                      ControlName = pc.Attributes.ContainsKey("cgi_controlname") ? pc.GetAttributeValue<string>("cgi_controlname") : string.Empty,
                                      DataSource = pc.Attributes.ContainsKey("cgi_datasource") ? (pc.GetAttributeValue<OptionSetValue>("cgi_datasource")).Value : 0,
                                      DataSourceName = pc.Attributes.ContainsKey("cgi_datasource") ? ((DataSource)((pc.GetAttributeValue<OptionSetValue>("cgi_datasource")).Value)).ToString(): "",
                                      EntityScope = pc.Attributes.ContainsKey("cgi_entityscope") ? pc.GetAttributeValue<string>("cgi_entityscope") : string.Empty,
                                      FetchXML=pc.Attributes.ContainsKey("cgi_fetchxml") ? pc.GetAttributeValue<string>("cgi_fetchxml") : string.Empty,
                                      FilterAttribute = pc.Attributes.ContainsKey("cgi_filterattribute") ? pc.GetAttributeValue<string>("cgi_filterattribute") : string.Empty,
                                      ReturnAttribute = pc.Attributes.ContainsKey("cgi_returnattribute") ? pc.GetAttributeValue<string>("cgi_returnattribute") : string.Empty,
                                      TargetAttribute = pc.Attributes.ContainsKey("cgi_targetattribute") ? pc.GetAttributeValue<string>("cgi_targetattribute") : string.Empty,
                                      QueryType = pc.Attributes.ContainsKey("cgi_querytype") ? (pc.GetAttributeValue<OptionSetValue>("cgi_querytype")).Value : 0,
                                      QueryTypeName = pc.Attributes.ContainsKey("cgi_querytype") ? ((QueryType)((pc.GetAttributeValue<OptionSetValue>("cgi_querytype")).Value)).ToString() : "",
                                      Title = pc.Attributes.ContainsKey("cgi_title") ? pc.GetAttributeValue<string>("cgi_title") : string.Empty,
                                      ViewGUID = pc.Attributes.ContainsKey("cgi_viewguid") ? pc.GetAttributeValue<string>("cgi_viewguid") : string.Empty,
                                      ViewName = pc.Attributes.ContainsKey("cgi_viewname") ? pc.GetAttributeValue<string>("cgi_viewname") : string.Empty,
                                      ViewPrimaryEntity = pc.Attributes.ContainsKey("cgi_viewprimaryentity") ? pc.GetAttributeValue<string>("cgi_viewprimaryentity") : string.Empty,
                                      VisibleForCreate = pc.Attributes.ContainsKey("cgi_create") ? pc.GetAttributeValue<bool>("cgi_create") : false,
                                      VisibleForUpdate = pc.Attributes.ContainsKey("cgi_update") ? pc.GetAttributeValue<bool>("cgi_update"): false,
                                      ShowDescriptionPanel = pc.Attributes.ContainsKey("cgi_showdescriptionpanel") ? pc.GetAttributeValue<bool>("cgi_showdescriptionpanel") : false,
                                      DescriptionAttribute = pc.Attributes.ContainsKey("cgi_descriptionpanelattribute") ? pc.GetAttributeValue<string>("cgi_descriptionpanelattribute") : string.Empty,
                                      RecordGuid=pc.Id.ToString(),                                      
                                  }).ToList();


            GetPanelConfigurationCallbackHandler getPanelConfigurationCallbackHandler = (GetPanelConfigurationCallbackHandler)result.AsyncState;
            getPanelConfigurationCallbackHandler(panelConfigurations, true);
        }

        #region Logic 1
        //internal void ProcessCrmViewRequest(string viewGUID,string controlName,string columnHeader,string title, bool isSytemView,ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler)
        //{

        //    OrganizationRequest orgExecuteByIdUserQueryRequest = new CrmSdk.OrganizationRequest();

        //    dynamic asyncStateParams = new ExpandoObject();
        //    asyncStateParams.handler = processViewDataRequestCallbackHandler;
        //    asyncStateParams.controlName = controlName;
        //    asyncStateParams.columnHeader = columnHeader;
        //    asyncStateParams.title = title;

        //    if (!isSytemView)
        //    {
        //        orgExecuteByIdUserQueryRequest.RequestName = "ExecuteByIdUserQuery";
        //    }
        //    else
        //    {
        //        orgExecuteByIdUserQueryRequest.RequestName = "ExecuteByIdSavedQuery";
        //    }
        //    EntityReference viewReference = new EntityReference { Id = new Guid(viewGUID) };

        //    orgExecuteByIdUserQueryRequest.Parameters = new CrmSdk.ParameterCollection();
        //    orgExecuteByIdUserQueryRequest.Parameters.Add(new CrmSdk.KeyValuePair<string, object> { Key = "EntityId", Value = new Guid(viewGUID) });            
        //    CrmService.BeginExecute(orgExecuteByIdUserQueryRequest, new AsyncCallback(ProcessCrmViewRequest_CallBack), asyncStateParams);

        //}
        //private void ProcessCrmViewRequest_CallBack(IAsyncResult result)
        //{
        //    OrganizationResponse orgResponse = CrmService.EndExecute(result);
        //    if (orgResponse.Results.Count> 0)
        //    {
                
        //        XDocument responseDoc = XDocument.Parse(((CrmSdk.KeyValuePair<string, object>)orgResponse.Results[0]).Value.ToString());
        //        var resultNodes = responseDoc.Descendants("result").ToList();
                
        //        dynamic asyncStateParams = (dynamic)result.AsyncState;

        //        string[] colHeadders = ((string)asyncStateParams.columnHeader).Split(',','|');

        //        for (int iPos = 1; iPos <= colHeadders.Count(); iPos += 2)
        //        {
        //            View.AddProperty(colHeadders[iPos], typeof(string));
        //        }


        //        ObservableCollection<View> ViewData = new ObservableCollection<View>();
        //        foreach (var resultNode in resultNodes)
        //        {

        //            View ViewDataRow = new View();

        //            for (int iPos = 0; iPos < colHeadders.Count(); iPos += 2)
        //            {

        //                var childNode = resultNode.Element(colHeadders[iPos]);

        //                string nodValue = string.Empty;
        //                if (childNode != null)
        //                {
        //                    if (childNode.Attribute("name") != null)
        //                    {
        //                        nodValue = (string)childNode.Attribute("name");
        //                    }
        //                    else if (childNode.Attribute("formattedvalue") != null)
        //                    {
        //                        nodValue = (string)childNode.Attribute("formattedvalue");
        //                    }
        //                    else if (childNode.Attribute("date") != null)
        //                    {
        //                        nodValue = (string)childNode.Attribute("date") + " " + (string)childNode.Attribute("time");
        //                    }
        //                    else
        //                    {
        //                        nodValue = childNode.Value;
        //                    }
        //                }
        //                ViewDataRow.SetPropertyValue(colHeadders[iPos + 1], nodValue);
        //            }
        //            ViewData.Add(ViewDataRow);
        //        }

        //        ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = asyncStateParams.handler;

        //        processViewDataRequestCallbackHandler(ViewData, asyncStateParams.controlName, asyncStateParams.title, true);
        //    }
            
        //}

        //internal void ProcessFetchXMLRequest(string fetchXML,string controlName, string columnHeader, string title, ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler)
        //{
        //    dynamic asyncStateParams = new ExpandoObject();
        //    asyncStateParams.handler = processViewDataRequestCallbackHandler;
        //    asyncStateParams.controlName = controlName;
        //    asyncStateParams.columnHeader = columnHeader;
        //    asyncStateParams.title = title;

        //    //if (fetchXML.Contains("###VALUE###"))
        //    //{
        //    //    if (!string.IsNullOrEmpty(filterAttributeValue))
        //    //    {
        //    //        fetchXML = fetchXML.Replace("###VALUE###", filterAttributeValue);
        //    //    }
        //    //    else
        //    //    {
        //    //        filterAttributeValue = SilverlightUtility.GetAttributeValue(filterAttribute);
        //    //        fetchXML = fetchXML.Replace("###VALUE###", filterAttributeValue);
        //    //    }
        //    //}

        //    CrmService.BeginRetrieveMultiple(new FetchExpression() { Query = fetchXML },new AsyncCallback(ProcessFetchXMLRequest_CallBack), asyncStateParams);
        //}
        //private void ProcessFetchXMLRequest_CallBack(IAsyncResult result)
        //{

        //    EntityCollection entityCollection = CrmService.EndRetrieveMultiple(result);

        //    dynamic asyncStateParams = (dynamic)result.AsyncState;

        //    string[] colHeadders = ((string)asyncStateParams.columnHeader).Split(',', '|');

        //    for (int iPos = 1; iPos <= colHeadders.Count(); iPos += 2)
        //    {
        //        View.AddProperty(colHeadders[iPos], typeof(string));
        //    }

        //    ObservableCollection<View> ViewData = new ObservableCollection<View>();
        //    foreach (Entity entity in entityCollection.Entities)
        //    {

        //        View ViewDataRow = new View();
        //        for (int iPos = 0; iPos < colHeadders.Count(); iPos += 2)
        //        {
        //            var attributeData = entity.GetAttributeValue<dynamic>(colHeadders[iPos]);
        //            var type = attributeData.GetType();
        //            string attributeVal = string.Empty;
        //            if (type == typeof(EntityReference))
        //            {
        //                attributeVal = ((EntityReference)attributeData).Name;
        //            }
        //            else if (type == typeof(DateTime))
        //            {
        //                attributeVal = ((DateTime)attributeData).ToString();
        //            }
        //            else if (type == typeof(Guid))
        //            {
        //                attributeVal = ((Guid)attributeData).ToString();
        //            }
        //            else if (type == typeof(Boolean))
        //            {
        //                attributeVal = ((Boolean)attributeData).ToString();
        //            }
        //            else
        //            {
        //                attributeVal = (string)attributeData;
        //            }
        //            ViewDataRow.SetPropertyValue(colHeadders[iPos + 1], attributeVal);

        //        }
        //        ViewData.Add(ViewDataRow);
        //    }
        //    ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = asyncStateParams.handler;

        //    processViewDataRequestCallbackHandler(ViewData, asyncStateParams.controlName, asyncStateParams.title, true);

        //}
        #endregion

        #region Logic 2
        internal void ProcessCrmViewRequest(string viewGUID,string viewPrimaryEntity,string columnHeader, bool isSytemView,string panelConfigRecordGuid, ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler)
        {

            OrganizationRequest orgExecuteByIdUserQueryRequest = new OrganizationRequest();

            dynamic asyncStateParams = new ExpandoObject();
            asyncStateParams.handler = processViewDataRequestCallbackHandler;            
            asyncStateParams.columnHeader = columnHeader;
            asyncStateParams.entityName = viewPrimaryEntity;
            asyncStateParams.panelConfigRecordGuid = panelConfigRecordGuid;
            orgExecuteByIdUserQueryRequest.Parameters = new ParameterCollection();
            if (!isSytemView)
            {
                orgExecuteByIdUserQueryRequest.RequestName = "ExecuteByIdUserQuery";

                EntityReference viewReference = new EntityReference { Id = new Guid(viewGUID), LogicalName = "userquery"};
                
                
                
                orgExecuteByIdUserQueryRequest.Parameters.Add(new CGIXrm.CrmSdk.KeyValuePair<string, object> { Key = "EntityId", Value = viewReference });
                //orgExecuteByIdUserQueryRequest.Parameters.Add(new CGIXrm.CrmSdk.KeyValuePair<string, object> { Key = "EntityId", Value = new Guid(viewGUID) });
                //ProcessViewRequest(viewGUID);
               
            }
            else
            {
                orgExecuteByIdUserQueryRequest.RequestName = "ExecuteByIdSavedQuery";
                orgExecuteByIdUserQueryRequest.Parameters.Add(new CGIXrm.CrmSdk.KeyValuePair<string, object> { Key = "EntityId", Value = new Guid(viewGUID)});
            
            }
            
            
            CrmService.BeginExecute(orgExecuteByIdUserQueryRequest, new AsyncCallback(ProcessCrmViewRequest_CallBack), asyncStateParams);

        }
        private void ProcessCrmViewRequest_CallBack(IAsyncResult result)
        {
            OrganizationResponse orgResponse = CrmService.EndExecute(result);
            if (orgResponse.Results.Count > 0)
            {

                XDocument responseDoc = XDocument.Parse(((CGIXrm.CrmSdk.KeyValuePair<string, object>)orgResponse.Results[0]).Value.ToString());
                var resultNodes = responseDoc.Descendants("result").ToList();

                dynamic asyncStateParams = (dynamic)result.AsyncState;
                
                string[] colHeadders = ((string)asyncStateParams.columnHeader).Split(',', '|');

                for (int iPos = 1; iPos <= colHeadders.Count(); iPos += 2)
                {
                    if (colHeadders[iPos].ToLowerInvariant().Contains("[p]"))
                        View.AddProperty(colHeadders[iPos].Replace("[P]", "").Replace("[p]", ""), typeof(string));
                    else
                        View.AddProperty(colHeadders[iPos], typeof(string));
                }


                ObservableCollection<View> ViewData = new ObservableCollection<View>();
                foreach (var resultNode in resultNodes)
                {

                    View ViewDataRow = new View();
                    ViewDataRow.PanelConfigId = asyncStateParams.panelConfigRecordGuid;
                    ViewDataRow.ViewEntityName = asyncStateParams.entityName;
                    for (int iPos = 0; iPos < colHeadders.Count(); iPos += 2)
                    {

                        var childNode = resultNode.Element(colHeadders[iPos]);
                        
                        string nodValue = string.Empty;
                        if (childNode != null)
                        {
                            if (childNode.Attribute("name") != null)
                            {
                                nodValue = (string)childNode.Attribute("name");
                               
                                
                            }
                            else if (childNode.Attribute("formattedvalue") != null)
                            {
                                nodValue = (string)childNode.Attribute("formattedvalue");
                                
                            }
                            else if (childNode.Attribute("date") != null)
                            {
                                nodValue = (string)childNode.Attribute("date") + " " + (string)childNode.Attribute("time");
                                
                            }
                            else
                            {
                                nodValue = childNode.Value;
                            }
                            ProcessAttributeDetails(childNode, asyncStateParams.entityName, colHeadders[iPos], ref ViewDataRow);
                        }
                        if (colHeadders[iPos + 1].ToLowerInvariant().Contains("[p]"))
                            ViewDataRow.SetPropertyValue(colHeadders[iPos + 1].Replace("[P]", "").Replace("[p]", ""), nodValue); ;
                        
                        
                        ViewDataRow.SetPropertyValue(colHeadders[iPos + 1], nodValue);
                    }
                    ViewData.Add(ViewDataRow);
                }

                ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = asyncStateParams.handler;

                processViewDataRequestCallbackHandler(ViewData, asyncStateParams.panelConfigRecordGuid, true);
            }

        }

        private void ProcessAttributeDetails(XElement attribureNode,string entityName,string attribute,ref View ViewDataRow)
        {
            if (attribureNode.Name.ToString() == "statecode")
            {
                ViewDataRow.AttributeDetails.Add(attribute, "statecode");
                ViewDataRow.UnFormattedValues.Add(attribute, attribureNode.Value);
            }
            else if (attribureNode.Attribute("formattedvalue") != null)
            {
                ViewDataRow.UnFormattedValues.Add(attribute, attribureNode.Value);
                if (attribureNode.Value == (string)attribureNode.Attribute("formattedvalue"))
                {
                    ViewDataRow.AttributeDetails.Add(attribute, "optionset");
                }
            }
            else if (attribureNode.Attribute("type") != null)
            {
                ViewDataRow.AttributeDetails.Add(attribute, "lookup");
                ViewDataRow.LookUpDetails.Add(attribute, new LookUpDetails() { Id = new Guid(attribureNode.Value), EntityType = (string)attribureNode.Attribute("type"), Name = (string)attribureNode.Attribute("name") });
            }
            else if (attribureNode.Attribute("date") != null)
            {
                ViewDataRow.AttributeDetails.Add(attribute, "datetime");
                ViewDataRow.UnFormattedValues.Add(attribute, attribureNode.Value);
            }
            else
            {
                if (entityName + "id" != attribute)
                {
                    ViewDataRow.AttributeDetails.Add(attribute, "text");
                }
                else
                {
                    ViewDataRow.AttributeDetails.Add(attribute, "lookup");
                    ViewDataRow.LookUpDetails.Add(attribute, new LookUpDetails() { Id = new Guid(attribureNode.Value), EntityType = entityName, Name = "" });
                }
            }
        }

        internal void ProcessFetchXMLRequest(string fetchXML,string columnHeader, string panelConfigRecordGuid, ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler)
        {
            dynamic asyncStateParams = new ExpandoObject();
            asyncStateParams.handler = processViewDataRequestCallbackHandler;           
            asyncStateParams.columnHeader = columnHeader;
            
            asyncStateParams.panelConfigRecordGuid = panelConfigRecordGuid;

            //if (fetchXML.Contains("###VALUE###"))
            //{
            //    if (!string.IsNullOrEmpty(filterAttributeValue))
            //    {
            //        fetchXML = fetchXML.Replace("###VALUE###", filterAttributeValue);
            //    }
            //    else
            //    {
            //        filterAttributeValue = SilverlightUtility.GetAttributeValue(filterAttribute);
            //        fetchXML = fetchXML.Replace("###VALUE###", filterAttributeValue);
            //    }
            //}

            CrmService.BeginRetrieveMultiple(new FetchExpression() { Query = fetchXML },new AsyncCallback(ProcessFetchXMLRequest_CallBack), asyncStateParams);
        }
        private void ProcessFetchXMLRequest_CallBack(IAsyncResult result)
        {

            EntityCollection entityCollection = CrmService.EndRetrieveMultiple(result);

            dynamic asyncStateParams = (dynamic)result.AsyncState;

            string[] colHeadders = ((string)asyncStateParams.columnHeader).Split(',', '|');

            for (int iPos = 1; iPos <= colHeadders.Count(); iPos += 2)
            {
                if (colHeadders[iPos].ToLowerInvariant().Contains("[p]"))
                    View.AddProperty(colHeadders[iPos].Replace("[P]", "").Replace("[p]", ""), typeof(string));
                else
                    View.AddProperty(colHeadders[iPos], typeof(string));
            }

             ObservableCollection<View> ViewData = new ObservableCollection<View>();
             foreach (Entity entity in entityCollection.Entities)
             {

                 View ViewDataRow = new View();
                 ViewDataRow.PanelConfigId = asyncStateParams.panelConfigRecordGuid;
                 ViewDataRow.ViewEntityName = entity.LogicalName;

                 for (int iPos = 0; iPos < colHeadders.Count(); iPos += 2)
                 {
                     string attribute = colHeadders[iPos];
                     var attributeData = entity.GetAttributeValue<dynamic>(attribute);
                     if (attributeData == null)
                         continue;
                     var type = attributeData.GetType();
                     string attributeVal=string.Empty;

                     if (type == typeof(EntityReference))
                     {
                         attributeVal = ((EntityReference)attributeData).Name;
                         ViewDataRow.AttributeDetails.Add(attribute, "lookup");
                         ViewDataRow.LookUpDetails.Add(attribute, new LookUpDetails() { Id = ((EntityReference)attributeData).Id, EntityType = ((EntityReference)attributeData).LogicalName, Name = ((EntityReference)attributeData).Name });

                     }
                     else if (type == typeof(DateTime))
                     {
                         attributeVal = ((DateTime)attributeData).ToString();
                         ViewDataRow.AttributeDetails.Add(attribute, "datetime");
                         ViewDataRow.UnFormattedValues.Add(attribute, ((DateTime)attributeData).ToUniversalTime().ToString());
                     }
                     else if (type == typeof(Guid))
                     {
                         //var lstIndex = from x in colHeadders
                         //        where x.IndexOf("[P]")>0
                         //        select x.IndexOf("[P]");                        

                         List<int> lstPrimaryAttributeIndex = colHeadders.Select((s, index) => new { s, index })
                                            .Where(x => x.s.Contains("[P]"))
                                            .Select(x => x.index)
                                            .ToList();
                         
                         if (lstPrimaryAttributeIndex != null && lstPrimaryAttributeIndex.Count() > 0)
                         {
                             string LookNameVal = entity.GetAttributeValue<string>(colHeadders[lstPrimaryAttributeIndex[0] - 1]);
                             ViewDataRow.LookUpDetails.Add(attribute, new LookUpDetails() { Id = attributeData, EntityType = entity.LogicalName, Name = LookNameVal });
                         }

                         attributeVal = ((Guid)attributeData).ToString();
                         ViewDataRow.AttributeDetails.Add(attribute, "lookup");
                         
                     }
                     else if (type == typeof(Boolean))
                     {
                         attributeVal = ((Boolean)attributeData).ToString();
                         ViewDataRow.AttributeDetails.Add(attribute, "boolean");
                         ViewDataRow.UnFormattedValues.Add(attribute, attributeVal);
                     }
                     else if (type == typeof(OptionSetValue))
                     {
                         attributeVal = ((OptionSetValue)attributeData).Value.ToString();
                         ViewDataRow.AttributeDetails.Add(attribute, "optionset");
                         ViewDataRow.UnFormattedValues.Add(attribute, attributeVal);
                     }
                     else
                     {
                       
                         
                         attributeVal = (string)attributeData;                         
                         ViewDataRow.AttributeDetails.Add(attribute, "text");
                     }

                     if (colHeadders[iPos + 1].ToLowerInvariant().Contains("[p]"))
                         ViewDataRow.SetPropertyValue(colHeadders[iPos + 1].Replace("[P]","").Replace("[p]", ""), attributeVal);
                     else
                         ViewDataRow.SetPropertyValue(colHeadders[iPos + 1], attributeVal);
                     
                 }
                 ViewData.Add(ViewDataRow);
             }
             ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = asyncStateParams.handler;

             processViewDataRequestCallbackHandler(ViewData, asyncStateParams.panelConfigRecordGuid, true);
             
        }

        #endregion
        private void ProcessViewRequest(string viewGuid)
        {
            //QueryExpression queryViewDatils = new CrmSdk.QueryExpression();
            //queryViewDatils.EntityName = "savedquery";

            //ConditionExpression conditionExpression = new CrmSdk.ConditionExpression();
            //conditionExpression.AttributeName="savedqueryid";
            //conditionExpression.Values = new ObservableCollection<object>() { new Guid(viewGuid) };

            //queryViewDatils.Criteria = new CrmSdk.FilterExpression() { Conditions = new ObservableCollection<CrmSdk.ConditionExpression>(){conditionExpression}};
            //queryViewDatils.ColumnSet = new CrmSdk.ColumnSet()
            //{
            //    Columns = new ObservableCollection<string>() { "layoutxml" }
            //};


          

            //CrmService.BeginRetrieve("savedquery", new Guid(viewGuid), new ColumnSet() { AllColumns=true }, ProcessViewHeadderRequest_CallBack, CrmService);
            CrmService.BeginRetrieve("userquery", new Guid(viewGuid), new ColumnSet() { AllColumns = true }, ProcessViewHeadderRequest_CallBack, CrmService);

        }
        private void ProcessViewHeadderRequest_CallBack(IAsyncResult result)
        {
            Entity enty = crmService.EndRetrieve(result);

        }
    }
}
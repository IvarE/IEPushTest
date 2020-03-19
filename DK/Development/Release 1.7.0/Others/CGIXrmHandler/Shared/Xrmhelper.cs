using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using CGIXrmHandler.CrmClasses;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmHandler.Shared
{
    internal class XrmHelper
    {
        #region Public Properties
        Settings _XrmSettings;
        public Settings XrmSettings
        {
          get {
              if (_XrmSettings == null)
                  _XrmSettings = GetXrmSettings();
            
              return _XrmSettings;
            }
          set { _XrmSettings = value; }
        }
        #endregion

        #region Constructors
        public XrmHelper()
        {

        }
        public XrmHelper(Settings xrmSettings)
        {
            if (_XrmSettings == null)
                _XrmSettings = xrmSettings;
        }
        #endregion

        #region Private Methods

        Settings GetXrmSettings()
        {
            XrmManager xrmManager = GetXrmManagerFromAppSettings(Guid.Empty);


            string fetchXml = string.Format(@"<fetch mapping='logical' distinct='false'>
                                            <entity name='cgi_setting'>
                                            <all-attributes/>
                                            <order attribute='cgi_name' descending='false'/>
                                            <filter type='and'>
                                            <filter type='or'>
                                            <condition attribute='cgi_validto' operator='on-or-before' value='{0}'/>
                                            <condition attribute='cgi_validto' operator='null'/>
                                            </filter>
                                            <condition attribute='statecode' operator='eq' value='0'/>
                                            </filter>
                                            </entity>
                                            </fetch>", DateTime.Today.ToString(CultureInfo.GetCultureInfo("sv-SE").DateTimeFormat.ShortDatePattern));

             //EntityCollection entityCollection= xrmManager.Service.RetrieveMultiple(new FetchExpression(fetchXml));
            ObservableCollection<Settings> lstSettings = xrmManager.Get<Settings>(fetchXml);
            if (lstSettings != null && lstSettings.Count > 0)
                return lstSettings[0];
            else
                throw new Exception("No matching setting was found");
        }

        #endregion

        #region internal Methods
        internal XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {   
                
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                    if (callerId != Guid.Empty)
                        ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                    return xrmManager;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        internal XrmManager GetXrmManagerFromAppSettings()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                    return xrmManager;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }
    

        internal void UpdateXrmManagerUser(ref XrmManager xrmManager,Guid callerId)
        {
            try
            {
                if (callerId != Guid.Empty)
                    ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        internal Guid GetId(string searchValue,string searchAttribute,string entityName,XrmManager xrmManager)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute
            {
                EntityName = entityName,
                ColumnSet = new ColumnSet()
            };
            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                return entityCollection[0].Id;
            else
                return Guid.Empty;
         }

        internal Entity GetCategoryDetail(string searchValue, string searchAttribute,XrmManager xrmManager)
        {
            Entity categoryDetail=null;
            QueryByAttribute queryByAttCategoryDetail = new QueryByAttribute
            {
                EntityName = "cgi_categorydetail",
                ColumnSet = new ColumnSet(true)
            };
            queryByAttCategoryDetail.AddAttributeValue(searchAttribute, searchValue);
            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttCategoryDetail);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                categoryDetail=entityCollection[0];
           

            return categoryDetail;
        }
        internal Guid CreateCaseCategory(Guid caseId, string errandTaskType,XrmManager xrmManager)
        {
            if (string.IsNullOrEmpty(errandTaskType))
                return Guid.Empty;

            CaseCatergory caseCategory = new CaseCatergory();
            Entity catergoryDetail = GetCategoryDetail(errandTaskType, "cgi_callguidecategory",xrmManager);

            if (catergoryDetail != null)
            {
                caseCategory.CaseCategory1 = new EntityReference("cgi_categorydetail", catergoryDetail.Id);
                var caseCategoryName = catergoryDetail.Contains("cgi_categorydetailname") ? catergoryDetail.GetAttributeValue<string>("cgi_categorydetailname") : string.Empty;

                if (catergoryDetail.Attributes.Contains("cgi_parentid"))
                {
                    EntityReference entityReference1 = catergoryDetail.GetAttributeValue<EntityReference>("cgi_parentid");
                    caseCategory.CaseCategory2 = caseCategory.CaseCategory1;
                    caseCategory.CaseCategory1 = entityReference1;

                    caseCategoryName = entityReference1.Name + " " + caseCategoryName;
                    catergoryDetail = GetCategoryDetail(entityReference1.Id.ToString(), "cgi_categorydetailid",xrmManager);
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
                Entity createdCaseCategory = xrmManager.Create(caseCategory);
                return createdCaseCategory.Id;
            }
            return Guid.Empty;
        }

        #endregion
        
    }
}

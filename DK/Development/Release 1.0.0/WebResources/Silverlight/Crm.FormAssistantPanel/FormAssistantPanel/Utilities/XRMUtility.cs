using System;
using System.Windows.Browser;
using System.ServiceModel;
using CGIXrm.CrmSdk;
using System.Linq;
using System.Threading;
using CGIXrm;
using Generic=System.Collections.Generic;
using System.Dynamic;


namespace Crm.FormAssistantPanel.Utilities
{
    
    enum DeploymentType
    {
       EMBEDDED=1,
       WEBPAGE=2,
       POPUP=3
    }
    internal partial class XRMUtility
    {   
        static bool isDebug = false;
        static WebParameters webParameters = new WebParameters();
        static CGIXrm.CrmManager crmManager = new CGIXrm.CrmManager(webParameters.ServerAddress);

        
        internal static IOrganizationService GetSoapService()
        {
            return crmManager.Service;
        }

        static DeploymentType _deployment;
        internal static DeploymentType Deployment
        {
            get { return XRMUtility._deployment; }
            set { XRMUtility._deployment = value; }
        }

        // set defatultLCID to 0 before deploying
        // static double DefaultLCID = 1053;        
        // static double DefaultLCID = 1033;
        static double DefaultLCID = 0;
        static double lcid = 0;
        internal static double LCID
        {
            get 
            {
                if (lcid == 0)
                {
                    lcid = Convert.ToDouble(webParameters.UserLcId);
                }
                return lcid; 
            }
        }

        // set defaultServerBaseURL to null before deploying        
        //   FormAssistantPanel.html?serveraddress=http://dev03/SknetrafikenLab/&userlcid=1033&TypeName=incident;
        /// This function calls SetUICulture fucntion of Globalization.cs, to set current thread's UI culture.
        /// </summary>
        internal static void SetUICulture()
        {
            Globalization.SetUICulture(LCID);
        }

        /// <summary>
        /// This function calls SetUICulture fucntion of Globalization.cs, to set given thread's UI culture.
        /// </summary>

        static dynamic _xrm = null;
        private static dynamic XRM
        {
            get
            {
                if (XRMUtility._xrm == null)
                {
                    if (Deployment == DeploymentType.EMBEDDED)
                    {
                        //Deployment Type is Embedded webresource
                        XRM = (ScriptObject)HtmlPage.Window.GetProperty("Xrm");
                    }
                    else if (Deployment == DeploymentType.WEBPAGE)
                    {
                        //Deployment Type is Inside HTML Page
                        var parent = (HtmlWindow)HtmlPage.Window.GetProperty("parent");
                        XRM = (ScriptObject)parent.GetProperty("Xrm");
                    }
                }
                return XRMUtility._xrm;
            }
            set
            {
                XRMUtility._xrm = value;
            }
        }
        static Generic.Dictionary<string, string> _customParameters = new Generic.Dictionary<string, string>();
        internal static Generic.Dictionary<string, string> CustomParameters
        {
            get
            {
                if (_customParameters.Count <= 0)
                    GetCustomParameters();

                return _customParameters;
                
            }
            set
            {
                _customParameters = value;
            }

        }

        internal static void SetUICulture(Thread thread)
        {
            Globalization.SetUICulture(thread, LCID);
        }

        internal static Guid GetEntityId()
        {
            return new Guid(webParameters.Id);
        }

        internal static string GetEntityName()
        {
           return webParameters.TypeName;
            
        }
        
        internal static void OpenCrmRecord(string entityName, Guid recordGuid)
        {
            XRM.Utility.openEntityForm(entityName, recordGuid);
        }

        internal static void ShowAlertDialog(string message)
        {
            XRM.Utility.alertDialog(message);
        }

        //internal static bool CheckIfCrmFormModifed()
        //{
        //    //if (isDebug)
        //    //{

        //    //}
        //    //if (xrm.Page.data.entity.getIsDirty())
        //    //       return true;
            
        //    //return false;
        //}


        private static void GetCustomParameters()
        {
            if ((App.Current.Host.InitParams.ContainsKey("data")) && (!string.IsNullOrEmpty(App.Current.Host.InitParams["data"])))
            {
                _customParameters = App.Current.Host.InitParams["data"].Split('&').Select(s => s.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
            }            
            if (!string.IsNullOrEmpty(webParameters.GetWebParameter("data")))
            {
                _customParameters = webParameters.GetWebParameter("data").Split('&').Select(s => s.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
            }
        }

        internal static void SetAttributeValue(string attributeName,dynamic attributeValue)
        {

            if (isDebug)
                return;

            if(string.IsNullOrEmpty(attributeName))
                return;
            
            if(attributeValue==null)
                XRM.Page.getAttribute(attributeName).setValue(null);

            string strTargetAttributeType = (string)XRM.Page.getAttribute(attributeName).getAttributeType();
            switch(strTargetAttributeType)
            { 
                case "lookup":
                    HtmlPage.Window.Invoke("setLookUpValue", XRM, attributeName,attributeValue.id, attributeValue.name, attributeValue.entityType, attributeValue.isTypeSpecified);
                    //HtmlPage.Window.Eval(attributeValue);
                    break;
                default:
                    XRM.Page.getAttribute(attributeName).setValue(attributeValue);
                    break;
            }
        }

        internal static void ShowNotification(string message,string level)
        {
            if (isDebug)
                return;
            
            XRM.Page.ui.setFormNotification(message, level, Guid.NewGuid());
        }
        internal static string GetAttributeType(string attributeName)
        {
            string attributeType = string.Empty;
            if (isDebug)
            {
                
            }
            attributeType = (string)XRM.Page.getAttribute(attributeName).getAttributeType(); 
            return attributeType;
        }
        internal static dynamic GetAttributeValue(string attributeName)
        {
            dynamic attributeValue = null;
            if (isDebug)
            {
                switch (attributeName)
                {
                    case "statuscode":
                        return "1";
                }
            }
            else
            {
                //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                    attributeValue = XRM.Page.getAttribute(attributeName).getValue();
                    //attributeValue = xrm.Page.getAttribute(attributeName).getValue()[0].id;
                //});
                //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{


                    //ScriptObject dynxrm = (ScriptObject)HtmlPage.Window.GetProperty("Xrm");

                    //ScriptObject pageProperty = (ScriptObject)dynxrm.GetProperty("Page");
                    ////ScriptObject attributesProperty = (ScriptObject)pageProperty.GetProperty("attributes");
                    //ScriptObject objAttribute = (ScriptObject)pageProperty.Invoke("getAttribute", attributeName);
                    
                    ////pagePropertyScriptObject objAttribute = (ScriptObject)attributesProperty.Invoke("get", attributeName);
                    //ScriptObject objAttributeValue = (ScriptObject)objAttribute.Invoke("getValue");


                //});

                return attributeValue;
            }
            return attributeValue;
        }

        /// <summary>
        /// Gets attribute value from crm form
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        //internal static string GetAttributeValue(string attributeName, string type)
        //{
        //    string attributeValue = string.Empty;
        //    if (isDebug)
        //    {
                
        //        switch (type.ToLowerInvariant())
        //        {
        //            case "lookup":
        //                {
        //                    attributeValue = "90269575-92C7-E211-88FB-00155D0D0411";// Secondary PriceList
        //                }
        //                break;
        //            case "lookupname":
        //                {
        //                    attributeValue = "Secondary Price List";
        //                }
        //                break;
        //        }
        //    }
        //    else
        //    {
                
        //        switch (type.ToLowerInvariant())
        //        {
        //            case "lookup":
        //                {
        //                    if (xrm.Page.getAttribute(attributeName).getValue() != null)
        //                    {
        //                        attributeValue = xrm.Page.getAttribute(attributeName).getValue()[0].id;
        //                    }
        //                }
        //                break;
        //            case "lookupname":
        //                {
        //                    if (xrm.Page.getAttribute(attributeName).getValue() != null)
        //                    {
        //                        attributeValue = xrm.Page.getAttribute(attributeName).getValue()[0].name;
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //    return attributeValue;
        //}

        internal static int GetFormType()
        {
            try
            {

                if (isDebug)
                {
                    return 1;
                }
                else
                {

                    //if (xrm == null)
                    //{
                    //    xrm = (ScriptObject)HtmlPage.Window.GetProperty("Xrm");
                    //}
                    return Convert.ToInt32(XRM.Page.ui.getFormType());
                    
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static string GetOrientation()
        {
            try
            {
                

                if (isDebug)
                {
                    return "horizantal";
                }
                else
                {
                    if (CustomParameters.ContainsKey("orientation"))
                    {
                         return CustomParameters["orientation"].ToLowerInvariant();
                    }
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static DateTime GetDateTime(double value)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime().AddSeconds(value);
        }

        internal static double GetUnixTime(DateTime value)
        {
            TimeSpan span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return span.TotalSeconds;
        }


        #region Commented Code

        //internal static Guid GetAccount()
        //{
        //    try
        //    {
        //        //bugbug get details from script obejct
        
        //        Guid accountId;
        //        //accountId = new Guid("56887B9B-24D9-E011-A9D7-00155D0D0404");

        //        HtmlWindow parent = (HtmlWindow)HtmlPage.Window.Invoke("GetCrmForm");
        
        //        ScriptObject entityProperty = SilverlightUtility.GetEntity(parent);
        //        ScriptObject attributesProperty = (ScriptObject)entityProperty.GetProperty("attributes");

        //        if (null == attributesProperty)
        //        {
        //            throw new InvalidOperationException("Property \"Xrm.Page.data.entity.attributes\" is null");
        //        }

        //        ScriptObject accountIdLookUp = (ScriptObject)attributesProperty.Invoke("get", "lgc_account");
        //        ScriptObject accountIdValue = (ScriptObject)accountIdLookUp.Invoke("getValue");
        
        //        string accountIdStr = ((ScriptObject)accountIdValue.GetProperty(0)).GetProperty("id") as string;
        //        accountIdStr = accountIdStr.Substring(1, accountIdStr.Length - 2);
        //        accountId = new Guid(accountIdStr);

        //        return accountId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion


    private void SilverlightSetLookupValue(string fieldName, Guid? id, string entityLogicalName, string name)
    {
        // Define eval statements for setting lookup to a value and null
        string setLookupJscript = @"Xrm.Page.getAttribute(""{0}"").setValue([ {{ id: ""{1:B}"", typename: ""{2}"", name: ""{3}"" }}])";
        string setLookupToNullJscript = @"Xrm.Page.getAttribute(""{0}"").setValue(null)";
        string evalStatement = null;
     
        // Set the statement to be evaluated based upon the value of the id argument
        if (id.GetValueOrDefault().Equals(Guid.Empty))
       {
           // Setting the lookup to null
           evalStatement = string.Format(setLookupToNullJscript, fieldName);
       }
       else
       {
           // Setting the lookup to a value
           evalStatement = string.Format(setLookupJscript, fieldName, id, entityLogicalName, name);
       }
    
       // Set the lookup
       HtmlPage.Window.Eval(evalStatement);
     }
       
    }
}
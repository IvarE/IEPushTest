using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class Utilities
    {
        #region Public Methods
        public static string GetSetting(IOrganizationService service, string serviceAttributeNameInLowerCase)
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_settingid' />";
            xml += "       <attribute name='" + serviceAttributeNameInLowerCase + "' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            #endregion

            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains(serviceAttributeNameInLowerCase) && settings[serviceAttributeNameInLowerCase] != null)
            {
                return settings.GetAttributeValue<string>(serviceAttributeNameInLowerCase);
            }
            throw new Exception("Required setting is missing: " + serviceAttributeNameInLowerCase);
        }

        public static T GetSetting<T>(IOrganizationService service, string serviceAttributeNameInLowerCase)
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_settingid' />";
            xml += "       <attribute name='" + serviceAttributeNameInLowerCase + "' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            #endregion

            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains(serviceAttributeNameInLowerCase) && settings[serviceAttributeNameInLowerCase] != null)
            {
                return settings.GetAttributeValue<T>(serviceAttributeNameInLowerCase);
            }
            throw new Exception("Required setting is missing: " + serviceAttributeNameInLowerCase);
        }
        #endregion
    }
}

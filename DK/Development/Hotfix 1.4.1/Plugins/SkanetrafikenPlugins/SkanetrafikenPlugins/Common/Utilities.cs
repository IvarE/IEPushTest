using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='" + serviceAttributeNameInLowerCase + "' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains(serviceAttributeNameInLowerCase) && settings[serviceAttributeNameInLowerCase] != null)
            {
                return settings.GetAttributeValue<string>(serviceAttributeNameInLowerCase);
            }
            else
            {
                throw new Exception("Required setting is missing: " + serviceAttributeNameInLowerCase);
            }
        }

        public static T GetSetting<T>(IOrganizationService service, string serviceAttributeNameInLowerCase)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='" + serviceAttributeNameInLowerCase + "' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains(serviceAttributeNameInLowerCase) && settings[serviceAttributeNameInLowerCase] != null)
            {
                return settings.GetAttributeValue<T>(serviceAttributeNameInLowerCase);
            }
            else
            {
                throw new Exception("Required setting is missing: " + serviceAttributeNameInLowerCase);
            }
        }
        #endregion
    }
}

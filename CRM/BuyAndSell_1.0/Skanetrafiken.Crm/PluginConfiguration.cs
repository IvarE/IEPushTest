using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace Endeavor.Crm
{
    class PluginConfiguration
    {
        const string ConfigDataReportPath = "ReportPath";
        const string ConfigDataReportServiceUrl = "ReportServiceUrl";
        const string ConfigDataDomain = "Domain";
        const string ConfigDataUserName = "UserName";
        const string ConfigDataPassword = "Password";

        private static string GetValueNode(XmlDocument doc, string key)
        {
            XmlNode node = doc.SelectSingleNode(string.Format("Settings/setting[@name='{0}']", key));
            if (node != null) { return node.SelectSingleNode("value").InnerText; } return string.Empty;
        }

        public static string GetConfigDataString(XmlDocument doc, string label)
        {
            return GetValueNode(doc, label);
        }

        public static void GetReportConfigData(XmlDocument doc, out string reportPath, out string reportServiceUrl, out string domain, out string userName, out string password)
        {
            reportPath = GetConfigDataString(doc, ConfigDataReportPath);
            reportServiceUrl = GetConfigDataString(doc, ConfigDataReportServiceUrl);
            domain = GetConfigDataString(doc, ConfigDataDomain);
            userName = GetConfigDataString(doc, ConfigDataUserName);
            password = GetConfigDataString(doc, ConfigDataPassword);
        }
    }
}

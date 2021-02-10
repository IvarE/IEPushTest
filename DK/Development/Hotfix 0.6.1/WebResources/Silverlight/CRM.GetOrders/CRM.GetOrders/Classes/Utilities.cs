using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CRM.GetOrders
{
    public class Utilities
    {
        public string XmlGetLanguageLabels(string LocalizedLabelGroup, string LaguageCode)
        {
            string _fetchXML =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                    "<entity name='cgi_localizedlabel'>" +
                        "<attribute name='cgi_localizedlabelid' />" +
                        "<attribute name='cgi_localizedlabelname' />" +
                        "<attribute name='cgi_localizedcontrolid' />" +
                        "<link-entity name='cgi_localizationlanguage' from='cgi_localizationlanguageid' to='cgi_localizationlanguageid' alias='ae'>" +
                            "<filter type='and'>" +
                                "<condition attribute='cgi_localizationlanguagenumber' operator='eq' value='{1}' />" +
                            "</filter>" +
                        "</link-entity>" +
                        "<link-entity name='cgi_localizedlabelgroup' from='cgi_localizedlabelgroupid' to='cgi_localizedlabelgroupid' alias='af'>" +
                            "<filter type='and'>" +
                                "<condition attribute='cgi_localizedlabelgroupname' operator='eq' value='{0}' />" +
                            "</filter>" +
                        "</link-entity>" +
                    "</entity>" +
                "</fetch>";

            return string.Format(_fetchXML, LocalizedLabelGroup, LaguageCode);
        }
    }
}

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

namespace CRM.Travel.Information.Classes
{
    public class FetchQueries
    {

        public string XmlGetLanguageLabels(string LocalizedLabelGroup)
        {
            string _xml = "";
            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += "   <entity name='cgi_localizedlabel'>";
            _xml += "       <attribute name='cgi_localizedlabelid'/>";
            _xml += "       <attribute name='cgi_localizedlabelname'/>";
            _xml += "       <attribute name='cgi_localizedlabelgroupid'/>";
            _xml += "       <attribute name='cgi_localizedcontrolid'/>";
            _xml += "       <attribute name='cgi_localizationlanguageid'/>";
            _xml += "       <link-entity name='cgi_localizedlabelgroup' alias='ac' to='cgi_localizedlabelgroupid' from='cgi_localizedlabelgroupid'>";
            _xml += "           <filter type='and'>";
            _xml += "               <condition attribute='cgi_localizedlabelgroupname' value='" + LocalizedLabelGroup + "' operator='eq'/>";
            _xml += "               <condition attribute='statecode' value='0' operator='eq'/>";
            _xml += "           </filter>";
            _xml += "       </link-entity>";
            _xml += "       <link-entity name='cgi_localizationlanguage' alias='ai' to='cgi_localizationlanguageid' from='cgi_localizationlanguageid'>";
            _xml += "           <attribute name='cgi_localizationlanguagenumber'/>";
            _xml += "           <attribute name='cgi_localizationlanguagename'/>";
            _xml += "           <filter type='and'>";
            _xml += "               <condition attribute='statecode' value='0' operator='eq'/>";
            _xml += "           </filter>";
            _xml += "       </link-entity>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }
        
    }
}

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

using CGIXrm;
using CGIXrm.CrmSdk;

namespace CRM.Travel.Information.Classes
{
    public class Language : XrmBaseEntity
    {
        //cgi_localizedlabelid
        [Xrm("cgi_localizedlabelid")]
        public Guid LocalizedLabelId { get; set; }

        //cgi_localizedlabelname
        [Xrm("cgi_localizedlabelname")]
        public string LocalizedlabelName { get; set; }

        //cgi_localizedlabelgroupid
        [Xrm("cgi_localizedlabelgroupid", DecodePart=XrmDecode.Value)]
        public Guid LocalizedLabelGroupId { get; set; }
        [Xrm("cgi_localizedlabelgroupid", DecodePart=XrmDecode.Name)]
        public string LocalizedLabelGroupIdName { get; set; }

        //cgi_localizedcontrolid
        [Xrm("cgi_localizedcontrolid")]
        public string LocalizedControlId { get; set; }
        
        //cgi_localizationlanguageid
        [Xrm("cgi_localizationlanguageid", DecodePart=XrmDecode.Value)]
        public Guid LocalizationLanguageId { get; set; }
        [Xrm("cgi_localizationlanguageid", DecodePart=XrmDecode.Name)]
        public string LocalizationLanguageIdName { get; set; }

        //cgi_localizationlanguagenumber
        [Xrm("ai.cgi_localizationlanguagenumber", DecodePart=XrmDecode.Value)]
        public int LocalizationLanguageNumber { get; set; }
        
        //cgi_localizationlanguagename
        [Xrm("ai.cgi_localizationlanguagename")]
        public string LocalizationLanguageName { get; set; }

    }
}

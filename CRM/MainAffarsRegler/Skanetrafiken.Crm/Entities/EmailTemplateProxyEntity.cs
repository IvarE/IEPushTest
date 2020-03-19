using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Globalization;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class EmailTemplateProxyEntity : Generated.ed_emailtemplateproxy
    {
        public EmailTemplateProxyEntity()
        {}

        public EmailTemplateProxyEntity(TemplateEntity template)
        {
            this.ed_TemplateGuid = template.Id != null ? template.Id.ToString() : null;
            this.ed_name = template.Title;
           }

    }
}
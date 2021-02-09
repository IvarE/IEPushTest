using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Endeavor.Crm;
using System.Diagnostics;
using System.Net;
//using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Skanetrafiken.Crm.Entities
{
    public class ValueCodeTemplateEntity : Generated.ed_valuecodetemplate
    {
        public enum casetypecode
        {
            ViewpointRequests = 899310004,

            Complaint = 899310002,

            Question = 899310005,

            Praise = 899310001,

            Irregularity = 899310000,

            InjuryIncident = 899310007,

            Travelwarranty = 899310003,
        }

        public TemplateEntity GetEmailTemplate(Plugin.LocalPluginContext localContext)
        {
            if(this.ed_EmailTemplateProxy == null)
            {
                throw new InvalidPluginExecutionException("Value Code template has no associated Email template");
            }

            EmailTemplateProxyEntity proxy = XrmRetrieveHelper.Retrieve<EmailTemplateProxyEntity>(localContext, ed_EmailTemplateProxy, new ColumnSet(EmailTemplateProxyEntity.Fields.ed_TemplateGuid));

            Guid templateID = new Guid(proxy.ed_TemplateGuid);
            return XrmRetrieveHelper.Retrieve<TemplateEntity>(localContext, templateID, new ColumnSet(true));
        }

        public void SetTemplateNumber(Plugin.LocalPluginContext localContext, ValueCodeTemplateEntity preImage)
        {
            if (preImage == null || this.IsAttributeModified(preImage, ValueCodeTemplateEntity.Fields.ed_CaseTypeCode))
            {
                if(this.ed_CaseTypeCode == null)
                {
                    this.ed_TemplateId = null;
                    return;
                }

                int templateId = (int)this.ed_CaseTypeCode.Value % 10000;

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(ValueCodeTemplateEntity.Fields.ed_TemplateId, ConditionOperator.Equal, templateId);
                filter.AddCondition(ValueCodeTemplateEntity.Fields.statecode, ConditionOperator.Equal, Generated.ed_valuecodetemplateState.Active);
                ValueCodeTemplateEntity otherTemplate = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(false), filter);

                if(otherTemplate == null)
                {
                    this.ed_TemplateId = templateId;
                }
                else
                {
                    throw new Exception("There exists already a Value Code Template for this Case Type.");
                }
            }
        }
        
    }
}

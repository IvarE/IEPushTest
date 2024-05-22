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
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class TemplateEntity : Generated.Template
    {
        public EmailEntity InstantiateTemplate(Plugin.LocalPluginContext localContext, EntityReference to)
        {
            // Create an e-mail message using a template.
            InstantiateTemplateRequest templateReq = new InstantiateTemplateRequest
            {
                TemplateId = this.Id,
                ObjectId = to.Id,
                ObjectType = to.LogicalName
            };
            InstantiateTemplateResponse templateResp = null;
            templateResp = (InstantiateTemplateResponse)localContext.OrganizationService.Execute(templateReq);

            if (templateResp.EntityCollection.Entities.Count == 0)
            {
                throw new InvalidPluginExecutionException(string.Format(Properties.Resources.InstantiateTemplateFailed, this.Title ?? this.Id.ToString()));
            }

            EmailEntity email = templateResp.EntityCollection.Entities[0].ToEntityNull<EmailEntity>();

            return email;
        }
    }

    
}
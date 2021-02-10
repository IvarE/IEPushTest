using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGI.CRM2013.Skanetrafiken.IncidentPlugin.Common
{
    internal class Common
    {
        #region Global Variables
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }
        public bool _isDebug = true;
        #endregion

        #region Publicmethods
        public Common(IOrganizationService service, ITracingService tracingService)
        {
            Service = service;
            TracingService = tracingService;
        }
         

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute, string destAttribute)
        {
            // Verify if attribute exists
            if (destEntity.Attributes.Contains(destAttribute) || !sourceEntity.Attributes.Contains(attribute))
                return;

            destEntity.Attributes.Add(new KeyValuePair<string, object>(destAttribute, sourceEntity.Attributes[attribute]));
        }

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute)
        {
            SetMissingEntityRef(destEntity, sourceEntity, attribute, attribute);
        }
        #endregion
    }
}

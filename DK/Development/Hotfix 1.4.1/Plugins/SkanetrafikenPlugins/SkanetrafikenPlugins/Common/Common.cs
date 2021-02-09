using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    internal class Common
    {
        #region Global Variables
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }
        public bool _isDebug = true;
        #endregion

        #region Constructors
        public Common(IOrganizationService service, ITracingService tracingService)
        {
            Service = service;
            TracingService = tracingService;
        }
        #endregion

        #region Public Methods
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

        public static QueryByAttribute CreateQueryAttribute(string entityName, string[] columns, string[] attributes, object[] values)
        {
            QueryByAttribute query = new QueryByAttribute(entityName);
            query.ColumnSet = new ColumnSet();
            query.ColumnSet.AddColumns(columns);
            query.Attributes.AddRange(attributes);
            query.Values.AddRange(values);
            return query;
        }

        public static SetStateRequest CreateStateRequest(Entity entity, int state, int status)
        {
            SetStateRequest stateRequest = new SetStateRequest();
            stateRequest.EntityMoniker = entity.ToEntityReference();
            stateRequest.State = new OptionSetValue(state);
            stateRequest.Status = new OptionSetValue(status);
            return stateRequest;
        }

        public static bool EntityCollectionHasItems(EntityCollection result)
        {
            return (result != null && result.Entities != null && result.Entities.Count > 0);
        }
        #endregion
    }
}

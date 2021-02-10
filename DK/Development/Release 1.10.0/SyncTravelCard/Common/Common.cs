using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SyncTravelCardPlugin.Common
{
    internal class Common
    {
        #region Declarations
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }
        public bool IsDebug = true;
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
            QueryByAttribute query = new QueryByAttribute(entityName)
            {
                ColumnSet = new ColumnSet()
            };
            query.ColumnSet.AddColumns(columns);
            query.Attributes.AddRange(attributes);
            query.Values.AddRange(values);
            return query;
        }

        public static SetStateRequest CreateStateRequest(Entity entity, int state, int status)
        {
            SetStateRequest stateRequest = new SetStateRequest
            {
                EntityMoniker = entity.ToEntityReference(),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };
            return stateRequest;
        }

        public static bool EntityCollectionHasItems(EntityCollection result)
        {
            return (result != null && result.Entities != null && result.Entities.Count > 0);
        }
        #endregion
    }
}

using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.Entities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm.Entities
{
    public class GetValueCode : CodeActivity
    {
        [Input("ValueCodeApproval")]
        [ReferenceTarget("ed_valuecodeapproval")]
        [RequiredArgument()]
        public InArgument<EntityReference> ValueCodeApproval { get; set; }

        [Output("ValueCode")]
        [ReferenceTarget("ed_valuecode")]
        public OutArgument<EntityReference> ValueCode { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();


            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

            localContext.TracingService.Trace($"GetValueCode started.");

            //TRY EXECUTE
            try
            {
                var valueCodeApproval = ValueCodeApproval.Get(activityContext);

                var executionResult = ExecuteCodeActivity(localContext, valueCodeApproval);
                ValueCode.Set(activityContext, executionResult);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"GetValueCode failed. Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetch value code that is associated with a value code approval
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="valueCodeApproval"></param>
        /// <returns></returns>
        public static EntityReference ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference valueCodeApproval)
        {
            if (valueCodeApproval == null)
                throw new InvalidPluginExecutionException($"Argument <{nameof(valueCodeApproval)}> is null.");

            try
            {

                var queryExpression = new QueryExpression()
                {
                    EntityName = ValueCodeEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(ValueCodeEntity.Fields.ed_ValueCodeId),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ValueCodeEntity.Fields.ed_ValueCodeApprovalId, ConditionOperator.Equal, valueCodeApproval.Id)
                        }
                    }
                };

                var valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, queryExpression);

                if (valueCode == null)
                    throw new InvalidPluginExecutionException($"(GetValueCode) Error: Could not find value code associated with a value code approval.");

                localContext.TracingService.Trace($"Found ed_valuecodeid: {valueCode.ed_ValueCodeId}");

                return new EntityReference(ValueCodeEntity.EntityLogicalName, valueCode.ed_ValueCodeId.Value);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("(GetValueCode) Error: " + ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM2013.SkanetrafikenPlugins.CubicService;
using CRM2013.SkanetrafikenPlugins.CreateGiftcardService;
using Microsoft.Crm.Sdk.Messages;

namespace CRM2013.SkanetrafikenPlugins
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Query;
    using System.Xml.Linq;

    public sealed class ExecuteRefundTransactions : CodeActivity
    {
        ITracingService tracingService;
        IOrganizationService service;
        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            // Create the tracing service
            tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered ReExecuteFaildeRefundTransactions.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            tracingService.Trace("ReExecuteFaildeRefundTransactions.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                //ExecuteRefundAndUpdatesStatus(context.PrimaryEntityId);

                ReimbursementHandler rh = new ReimbursementHandler();
                rh.ExecuteRefundAndUpdatesStatus(context.PrimaryEntityId, service);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());

                // Handle the exception.
                throw;
            }

            tracingService.Trace("Exiting ReExecuteFaildeRefundTransactions.Execute(), Correlation Id: {0}", context.CorrelationId);
        }

    }
}

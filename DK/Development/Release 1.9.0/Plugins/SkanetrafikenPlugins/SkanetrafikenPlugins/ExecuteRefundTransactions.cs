using CRM2013.SkanetrafikenPlugins.Common;
using System.Activities;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;


namespace CRM2013.SkanetrafikenPlugins
{
    public sealed class ExecuteRefundTransactions : CodeActivity
    {
        #region Declarations
        ITracingService _tracingService;
        IOrganizationService _service;
        #endregion

        #region Protected Methods
        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            // Create the tracing service
            _tracingService = executionContext.GetExtension<ITracingService>();

            if (_tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            _tracingService.Trace("Entered ReExecuteFaildeRefundTransactions.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            _tracingService.Trace("ReExecuteFaildeRefundTransactions.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            _service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                //ExecuteRefundAndUpdatesStatus(context.PrimaryEntityId);

                ReimbursementHandler rh = new ReimbursementHandler();
                rh.ExecuteRefundAndUpdatesStatus(context.PrimaryEntityId, _service);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                _tracingService.Trace("Exception: {0}", e.ToString());

                // Handle the exception.
                throw;
            }

            _tracingService.Trace("Exiting ReExecuteFaildeRefundTransactions.Execute(), Correlation Id: {0}", context.CorrelationId);
        }
        #endregion
    }
}

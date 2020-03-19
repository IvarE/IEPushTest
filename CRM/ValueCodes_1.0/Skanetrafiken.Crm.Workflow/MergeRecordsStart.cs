using System;
using System.Activities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using System.Threading;

namespace Skanetrafiken.Crm.Entities
{
    public class MergeRecordsStart : CodeActivity
    {
        //[Input("KampanjId")]
        //[RequiredArgument()]
        //[ReferenceTarget(MergeRecordsEntity.EntityLogicalName)]
        //public InArgument<EntityReference> campaingReference { get; set; }

        [Output("RemainingRecords")]
        public OutArgument<int> RemainingRecords { get; set; }


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
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.TracingService.Trace("Plugin för att kolla vilka som ska skickas till startat.");
            try
            {
               int iRemaining = ExecuteCodeActivity(localContext);

                RemainingRecords.Set(activityContext, iRemaining);
                //throw new Exception(string.Format("Found {0} included and {1} excluded", response.Item1, response.Item2));

            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Fel i MergeRecordsStart:\n{0}", e.ToString()));
            }
        }

        /// <summary>
        /// Execute for all records that does not have an error.
        /// </summary>
        /// <param name="localContext"></param>
        public static int ExecuteCodeActivity(Plugin.LocalPluginContext localContext)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            IList<MergeRecordsEntity> recordsToMerge = XrmRetrieveHelper.RetrieveMultiple<MergeRecordsEntity>(localContext, new ColumnSet(MergeRecordsEntity.Fields.ed_MergeFromContact, MergeRecordsEntity.Fields.ed_MergeToContact, MergeRecordsEntity.Fields.ed_Message),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(MergeRecordsEntity.Fields.ed_Message, ConditionOperator.Null)
                    }
                });

            localContext.TracingService.Trace($"Found {recordsToMerge.Count} records to merge. Starting....");
            int iProcessed = 0;
            foreach (var recordToMerge in recordsToMerge)
            {
                recordToMerge.PerformeMerge(localContext);
                
                iProcessed++;

                // For test only
                //Thread.Sleep(1000);

                // If process takes to long time, start another process
                if (stopwatch.ElapsedMilliseconds > 20000)           // More than 20sek?
                {

                    return recordsToMerge.Count - iProcessed;
                    //// Reached max?
                    //if(localContext.PluginExecutionContext.Depth >= 8)
                    //{
                    //    // We can not run processes any longer. Max depth is reached. 
                    //    break;
                    //}


                    //localContext.TracingService.Trace("GetWorkFlowByName");

                    //localContext.TracingService.Trace("Start new workflow");
                    //ExecuteWorkflowRequest req = new ExecuteWorkflowRequest();
                    //// Specify the guid of the workflow
                    //req.WorkflowId = new Guid("8709478A-DE7C-4FB3-914C-80C458D522B1");
                    //// Specify the guid of the related entity instance
                    //req.EntityId = recordToMerge.Id;
                    //localContext.OrganizationService.Execute(req);
                    ////ExecuteWorkflowResponse resp =

                    //break;
                }

            }

            return 0;
        }

    }
}

using System;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;

namespace Skanetrafiken.Crm
{
    public class PreTemplateDelete : Plugin
    {
        /// <summary>
        /// </summary>
        private readonly string preImageAlias = "preImage";

        public PreTemplateDelete()
            : base(typeof(PreTemplateDelete))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)Plugin.SdkMessageProcessingStepStage.PreOperation, Plugin.SdkMessageName.Delete, TemplateEntity.EntityLogicalName, new Action<LocalPluginContext>(PreExecuteTemplateDelete)));

            // Note : you can register for more events here if this plugin is not specific to an individual entity and message combination.
            // You may also need to update your RegisterFile.crmregister plug-in registration file to reflect any change.
        }

        /// <summary>
        /// Executes the plug-in.
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics CRM caches plug-in instances.
        /// The plug-in's Execute method should be written to be stateless as the constructor
        /// is not called for every invocation of the plug-in. Also, multiple system threads
        /// could execute the plug-in at the same time. All per invocation state information
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        protected void PreExecuteTemplateDelete(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // Must be Pre operation
            if (localContext.PluginExecutionContext.Stage != (int)Plugin.SdkMessageProcessingStepStage.PreOperation)
            {
                throw new InvalidPluginExecutionException("Plugin must run in Pre-Validation mode!");
            }

            // INFO: (Endeavor) Don't do anything in offline mode
            if (localContext.PluginExecutionContext.IsExecutingOffline)
                return;

            localContext.TracingService.Trace("{0} {1} {2} {3}", localContext.PluginExecutionContext.Stage, localContext.PluginExecutionContext.MessageName, localContext.PluginExecutionContext.PrimaryEntityName, localContext.PluginExecutionContext.Depth);

            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is EntityReference)
            {
                EntityReference target = (EntityReference)localContext.PluginExecutionContext.InputParameters["Target"];

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(EmailTemplateProxyEntity.Fields.ed_TemplateGuid, ConditionOperator.Equal, target.Id.ToString());
                EmailTemplateProxyEntity proxy = XrmRetrieveHelper.RetrieveFirst<EmailTemplateProxyEntity>(localContext, new ColumnSet(true), filter);

                if(proxy != null)
                {
                    XrmHelper.Delete(localContext, proxy.ToEntityReference());
                }
            }
        }

    }
}

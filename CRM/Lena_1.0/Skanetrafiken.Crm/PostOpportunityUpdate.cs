using System;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Endeavor.Crm.Entities;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm
{
    public class PostOpportunityUpdate : Plugin
    {
        /// <summary>
        /// </summary>

        private const string PreImageAlias = "preImage";

        /// <summary>
        /// </summary>
        public PostOpportunityUpdate()
            : base(typeof(PostOpportunityUpdate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Update", OpportunityEntity.EntityLogicalName, new Action<LocalPluginContext>(PostExecuteOpportunityUpdate)));

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
        protected void PostExecuteOpportunityUpdate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // Must be Post operation
            if (localContext.PluginExecutionContext.Stage != 40)
            {
                throw new InvalidPluginExecutionException("Plugin must run in Post-operation mode!");
            }

            // INFO: (joan) Don't do anything in offline mode
            if (localContext.PluginExecutionContext.IsExecutingOffline)
                return;

            localContext.TracingService.Trace("{0} {1} {2} {3}", localContext.PluginExecutionContext.Stage, localContext.PluginExecutionContext.MessageName, localContext.PluginExecutionContext.PrimaryEntityName, localContext.PluginExecutionContext.Depth);

            OpportunityEntity preImage = Plugin.GetPreImage<OpportunityEntity>(localContext, PreImageAlias);

            if (preImage == null)
            {
                throw new InvalidPluginExecutionException("Pre-Image not registered correctly.");
            }

            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is Entity)
            {

                // Obtain the target entity from the input parameters.
                OpportunityEntity target = ((Entity)localContext.PluginExecutionContext.InputParameters["Target"]).ToEntity<OpportunityEntity>();

                target.Trace(localContext.TracingService);

                try
                {
                    // If changed parent
                    if (target.Contains(OpportunityEntity.Fields.ed_ParentOpportunityId))
                    {
                        // Update previous parent
                        if (preImage.ed_ParentOpportunityId != null)
                        {
                            OpportunityEntity.SetEstimatedValueFromChildren(localContext, preImage.ed_ParentOpportunityId);
                        }

                        // If new parent
                        if (target.ed_ParentOpportunityId != null)
                        {
                            OpportunityEntity.SetEstimatedValueFromChildren(localContext, target.ed_ParentOpportunityId);
                        }
                    }
                    // Changed estimated value and has a parent
                    else if (preImage.ed_ParentOpportunityId != null &&
                        (target.Contains(OpportunityEntity.Fields.EstimatedValue) || target.Contains(OpportunityEntity.Fields.StateCode)))
                    {
                        // Update parent
                        OpportunityEntity.SetEstimatedValueFromChildren(localContext, preImage.ed_ParentOpportunityId);
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }

                //throw new InvalidPluginExecutionException("Debug @patlin3");

            }

        }
    }
}
//</snippetAccountNumberPlugin>

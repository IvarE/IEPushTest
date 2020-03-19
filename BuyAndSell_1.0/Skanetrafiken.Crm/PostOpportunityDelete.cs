using System;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Endeavor.Crm.Entities;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm
{
    public class PostOpportunityDelete : Plugin
    {
        /// <summary>
        /// </summary>
        private readonly string preImageAlias = "preImage";

        /// <summary>
        /// </summary>
        public PostOpportunityDelete()
            : base(typeof(PostOpportunityDelete))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Delete", OpportunityEntity.EntityLogicalName, new Action<LocalPluginContext>(PostExecuteOpportunityDelete)));

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
        protected void PostExecuteOpportunityDelete(LocalPluginContext localContext)
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

            // Target on Delete is EntityReference!
            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is EntityReference)
            {

                OpportunityEntity preImage = Plugin.GetPreImage<OpportunityEntity>(localContext, preImageAlias);

                if (preImage == null)
                    throw new InvalidPluginExecutionException("Pre-Image not registered correctly.");

                try
                {
                    localContext.TracingService.Trace("preImage Trace");
                    preImage.Trace(localContext.TracingService);

                    localContext.TracingService.Trace("");
                    localContext.TracingService.Trace("preImage.ed_ParentOpportunityId != null: {0}", preImage.ed_ParentOpportunityId != null ? "true" : "false");
                    localContext.TracingService.Trace("preImage.Contains(OpportunityEntity.Fields.ed_ParentOpportunityId): {0}", preImage.Contains(OpportunityEntity.Fields.ed_ParentOpportunityId).ToString());

                    // Update Parent value
                    //if (preImage.Contains(OpportunityEntity.Fields.ed_ParentOpportunityId))
                    if (preImage.ed_ParentOpportunityId != null)
                    {
                        localContext.TracingService.Trace("PostOpportunityDelete before SetEstimatedValueFromChildren(), preImage.ed_ParentOpportunityId (ID/Name): {0}/{1}",
                            preImage.ed_ParentOpportunityId.Id != null ? preImage.ed_ParentOpportunityId.Id.ToString() : "null", !string.IsNullOrEmpty(preImage.ed_ParentOpportunityId.Name) ? preImage.ed_ParentOpportunityId.Name.ToString() : "null");

                        localContext.TracingService.Trace("preImage.opportunityId (ID): {0}",
                            preImage.OpportunityId.HasValue ? preImage.OpportunityId.Value.ToString() : "null");

                        OpportunityEntity.SetEstimatedValueFromChildren(localContext, preImage.ed_ParentOpportunityId);
                    }
                        

                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }

                //throw new InvalidPluginExecutionException("Debug @patric");
            }
        }
    }
}
//</snippetAccountNumberPlugin>

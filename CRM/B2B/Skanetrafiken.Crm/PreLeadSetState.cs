using System;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class PreLeadSetState : Plugin
    {
        /// <summary>
        /// </summary>
        private readonly string preImageAlias = "preImage";

        public PreLeadSetState()
            : base(typeof(PreLeadSetState))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)Plugin.SdkMessageProcessingStepStage.PreOperation,
                Plugin.SdkMessageName.SetState,
                LeadEntity.EntityLogicalName,
                new Action<LocalPluginContext>(PreExecuteLeadSetState)));
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)Plugin.SdkMessageProcessingStepStage.PreOperation,
                Plugin.SdkMessageName.SetStateDynamicEntity,
                LeadEntity.EntityLogicalName,
                new Action<LocalPluginContext>(PreExecuteLeadSetState)));
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)Plugin.SdkMessageProcessingStepStage.PreOperation,
                "QualifyLead",
                LeadEntity.EntityLogicalName,
                new Action<LocalPluginContext>(PreExecuteLeadSetState)));

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
        protected void PreExecuteLeadSetState(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // Must be Post operation
            if (localContext.PluginExecutionContext.Stage != (int)Plugin.SdkMessageProcessingStepStage.PreOperation)
            {
                throw new InvalidPluginExecutionException("Plugin must run in Pre-operation mode!");
            }

            // INFO: (Endeavor) Don't do anything in offline mode
            if (localContext.PluginExecutionContext.IsExecutingOffline)
                return;

            localContext.TracingService.Trace("{0} {1} {2} {3}", localContext.PluginExecutionContext.Stage, localContext.PluginExecutionContext.MessageName, localContext.PluginExecutionContext.PrimaryEntityName, localContext.PluginExecutionContext.Depth);
            
            // Set state is EntityReference!
            if (localContext.PluginExecutionContext.InputParameters.Contains("EntityMoniker") &&
                localContext.PluginExecutionContext.InputParameters["EntityMoniker"] is EntityReference)
            {
                var myEntityRef = (EntityReference)localContext.PluginExecutionContext.InputParameters["EntityMoniker"];
                var state = (OptionSetValue)localContext.PluginExecutionContext.InputParameters["State"];
                var status = (OptionSetValue)localContext.PluginExecutionContext.InputParameters["Status"];

                localContext.TracingService.Trace("Found entity:{0}, name {2}:{3}, state:{1}", myEntityRef.Id, state.Value, myEntityRef.LogicalName, myEntityRef.Name);

                try
                {
                    LeadEntity.HandlePreLeadSetState(localContext, myEntityRef, state, status);
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }

                //throw new InvalidPluginExecutionException("Debug @joan");
            }

        }

    }
}
//</snippetAccountNumberPlugin>

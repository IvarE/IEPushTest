using System;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Entities;
using static Endeavor.Crm.Plugin;

namespace Skanetrafiken.Crm
{
    public class PreValidationOrderProductDelete : Plugin
    {
        
        /// <summary>
        /// </summary>
        public PreValidationOrderProductDelete()
            : base(typeof(PreValidationOrderProductDelete))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)Plugin.SdkMessageProcessingStepStage.PreValidation, Plugin.SdkMessageName.Delete, OrderProductEntity.EntityLogicalName, new Action<LocalPluginContext>(Execute)));

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
        protected void Execute(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // Must be Post operation
            if (localContext.PluginExecutionContext.Stage != (int)Plugin.SdkMessageProcessingStepStage.PreValidation)
            {
                throw new InvalidPluginExecutionException("Plugin must run in Pre-validation mode!");
            }

            // INFO: (joan) Don't do anything in offline mode
            if (localContext.PluginExecutionContext.IsExecutingOffline)
                return;

            localContext.TracingService.Trace("{0} {1} {2} {3}", localContext.PluginExecutionContext.Stage, localContext.PluginExecutionContext.MessageName, localContext.PluginExecutionContext.PrimaryEntityName, localContext.PluginExecutionContext.Depth);

            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is EntityReference)
            {
                try
                {
                    
                    // Obtain the target entity from the input parameters.
                    EntityReference targetER = ((EntityReference)localContext.PluginExecutionContext.InputParameters["Target"]);

                    OrderProductEntity.HandlePreValidationOrderProductEntityDelete(localContext,targetER);
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }
            }
            else
            {
                throw new InvalidPluginExecutionException("Error Validating Target or Target is not EntityReference Type.");
            }
            
        }
    }
}
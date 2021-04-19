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
    public class PostQuoteUpdate : Plugin
    {
        private readonly string preImageAlias = "preImage";

        /// <summary>
        /// </summary>
        public PostQuoteUpdate()
            : base(typeof(PostQuoteUpdate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)Plugin.SdkMessageProcessingStepStage.PostOperation, Plugin.SdkMessageName.Update, QuoteEntity.EntityLogicalName, new Action<LocalPluginContext>(PostExecuteQuoteUpdate)));

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
        protected void PostExecuteQuoteUpdate(LocalPluginContext localContext)
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

            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is Entity)
            {
                try
                {
                    // Obtain the target entity from the input parameters.
                    QuoteEntity target = ((Entity)localContext.PluginExecutionContext.InputParameters["Target"]).ToEntity<QuoteEntity>();

                    QuoteEntity preImage = Plugin.GetPreImage<QuoteEntity>(localContext, preImageAlias);

                    if (preImage == null)
                        throw new InvalidPluginExecutionException("Pre-Image not registered correctly.");

                    QuoteEntity.HandleQuoteEntityUpdate(localContext, target,preImage);
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }
            }
        }
    }
}

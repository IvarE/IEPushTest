
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm
{
    /// <summary>
    /// Base class for all Plugins.
    /// </summary>    
    public abstract class Plugin : IPlugin
    {
        public enum SdkMessageProcessingStepStage
        {
            PreValidation = 10,
            PreOperation = 20,
            PostOperation = 40,
        };

        public class SdkMessageName
        {
            public const string Assign = "Assign";
            public const string Create = "Create";
            public const string Delete = "Delete";
            public const string GrantAccess = "GrantAccess";
            public const string ModifyAccess = "ModifyAccess";
            public const string Retrieve = "Retrieve";
            public const string RetrieveMultiple = "RetrieveMultiple";
            public const string RetrievePrincipalAccess = "RetrievePrincipalAccess";
            public const string RetrieveSharedPrincipalsAndAccess = "RetrieveSharedPrincipalsAndAccess";
            public const string RevokeAccess = "RevokeAccess";
            public const string SetState = "SetState";
            public const string SetStateDynamicEntity = "SetStateDynamicEntity";
            public const string Update = "Update";
            public const string CalculatePrice = "CalculatePrice"; //Contact Svensson - need to update nuget. 
        };

        public class LocalPluginContext
        {
            public Guid executeAsUserId
            {
                get;
                set;
            }

            public IServiceProvider ServiceProvider
            {
                get;

                private set;
            }

            public IOrganizationService OrganizationService
            {
                get;

                private set;
            }

            public IPluginExecutionContext PluginExecutionContext
            {
                get;

                private set;
            }

            public ITracingService TracingService
            {
                get;

                private set;
            }

            private LocalPluginContext(Guid? executeAs = null)
            {
                if (executeAs != null)
                    executeAsUserId = (Guid)executeAs;
            }

            /// <summary>
            /// This constructor can be used for unit testing.
            /// </summary>
            public LocalPluginContext(IServiceProvider serviceProvider, IOrganizationService service, IPluginExecutionContext context, ITracingService tracingService)
            {
                // Obtain the execution context service from the service provider.
                this.PluginExecutionContext = context;

                // Obtain the tracing service from the service provider.
                this.TracingService = tracingService;

                // Use the factory to generate the Organization Service.
                this.OrganizationService = service;

            }

            public LocalPluginContext(IServiceProvider serviceProvider)
            {
                if (serviceProvider == null)
                {
                    throw new ArgumentNullException("serviceProvider");
                }

                // Obtain the execution context service from the service provider.
                this.PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // Obtain the tracing service from the service provider.
                this.TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                // Obtain the Organization Service factory service from the service provider
                IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                if (executeAsUserId == null)
                    executeAsUserId = this.PluginExecutionContext.UserId;

                // Use the factory to generate the Organization Service.
                this.OrganizationService = factory.CreateOrganizationService(executeAsUserId);
            }

            public void Trace(string message)
            {
                if (string.IsNullOrWhiteSpace(message) || this.TracingService == null)
                {
                    return;
                }

                this.TracingService.Trace(message);
            }

            public void Trace(string message, params object[] args)
            {
                if (string.IsNullOrWhiteSpace(message) || this.TracingService == null)
                {
                    return;
                }

                this.TracingService.Trace(message, args);
            }
        }

        private Collection<Tuple<int, string, string, Action<LocalPluginContext>>> registeredEvents;

        /// <summary>
        /// Gets the List of events that the plug-in should fire for. Each List
        /// Item is a <see cref="System.Tuple"/> containing the Pipeline Stage, Message and (optionally) the Primary Entity. 
        /// In addition, the fourth parameter provide the delegate to invoke on a matching registration.
        /// </summary>
        protected Collection<Tuple<int, string, string, Action<LocalPluginContext>>> RegisteredEvents
        {
            get
            {
                if (this.registeredEvents == null)
                {
                    this.registeredEvents = new Collection<Tuple<int, string, string, Action<LocalPluginContext>>>();
                }

                return this.registeredEvents;
            }
        }

        /// <summary>
        /// Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string ChildClassName
        {
            get;

            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref=" cred="Type"/> of the derived class.</param>
        public Plugin(Type childClassName)
        {
            this.ChildClassName = childClassName.ToString();
        }

        /// <summary>
        /// Executes the plug-in.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics CRM caches plug-in instances. 
        /// The plug-in's Execute method should be written to be stateless as the constructor 
        /// is not called for every invocation of the plug-in. Also, multiple system threads 
        /// could execute the plug-in at the same time. All per invocation state information 
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            // Construct the Local plug-in context.
            LocalPluginContext localcontext = new LocalPluginContext(serviceProvider);

            localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));

            try
            {
                // Iterate over all of the expected registered events to ensure that the plugin
                // has been invoked by an expected event
                // For any given plug-in event at an instance in time, we would expect at most 1 result to match.
                Action<LocalPluginContext> entityAction =
                    (from a in this.RegisteredEvents
                     where (
                     a.Item1 == localcontext.PluginExecutionContext.Stage &&
                     a.Item2 == localcontext.PluginExecutionContext.MessageName &&
                     (string.IsNullOrWhiteSpace(a.Item3) ? true : a.Item3 == localcontext.PluginExecutionContext.PrimaryEntityName)
                     )
                     select a.Item4).FirstOrDefault();

                if (entityAction != null)
                {
                    localcontext.Trace(string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} is firing for Entity: {1}, Message: {2}",
                        this.ChildClassName,
                        localcontext.PluginExecutionContext.PrimaryEntityName,
                        localcontext.PluginExecutionContext.MessageName));

                    entityAction.Invoke(localcontext);

                    // now exit - if the derived plug-in has incorrectly registered overlapping event registrations,
                    // guard against multiple executions.
                    return;
                }
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e.ToString()));

                // Handle the exception.
                throw;
            }
            finally
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", this.ChildClassName));
            }
        }

        /// <summary>
        /// Get post image and typecast it.
        /// </summary>
        public static T GetPostImage<T>(LocalPluginContext localContext, string imageAlias) where T : Entity
        {
            Entity imageEntity = (localContext.PluginExecutionContext.PostEntityImages != null && localContext.PluginExecutionContext.PostEntityImages.Contains(imageAlias)) ? localContext.PluginExecutionContext.PostEntityImages[imageAlias] : null;
            return imageEntity == null ? null : imageEntity.ToEntity<T>();
        }

        /// <summary>
        /// Get pre image and typecast it.
        /// </summary>
        public static T GetPreImage<T>(LocalPluginContext localContext, string imageAlias) where T : Entity
        {
            Entity imageEntity = (localContext.PluginExecutionContext.PreEntityImages != null && localContext.PluginExecutionContext.PreEntityImages.Contains(imageAlias)) ? localContext.PluginExecutionContext.PreEntityImages[imageAlias] : null;
            return imageEntity == null ? null : imageEntity.ToEntity<T>();
        }
    }
}
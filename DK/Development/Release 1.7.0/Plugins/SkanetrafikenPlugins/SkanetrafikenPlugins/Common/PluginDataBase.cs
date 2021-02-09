using System;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    internal abstract class PluginDataBase
    {
        #region Public Properties
        public Common Common { get; set; }

        public IPluginExecutionContext Context { get; set; }
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public Entity Target { get; set; }
        public Entity PreImage { get; set; }
        public Entity PostImage { get; set; }
        public EntityReference TargetReference { get; set; }
        #endregion

        #region Constructors

        protected PluginDataBase(IServiceProvider serviceProvider)
        {
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = serviceFactory.CreateOrganizationService(Context.UserId);
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Common = new Common(Service, TracingService);
        }
        #endregion

        #region Public Methods
        public void InitPreImage(string preImageName)
        {
            TracingService.Trace("Try to init pre image {0}", preImageName);
            if (Context.PreEntityImages.ContainsKey(preImageName) && Context.PreEntityImages[preImageName] != null)
            {
                PreImage = Context.PreEntityImages[preImageName];
            }
            TracingService.Trace("Pre image {0} : {1}", preImageName, (PreImage != null).ToString());
        }

        public void InitPostImage(string postImageName)
        {
            TracingService.Trace("Try to init post image {0}", postImageName);
            if (Context.PostEntityImages.ContainsKey(postImageName) && Context.PostEntityImages[postImageName] != null)
            {
                PostImage = Context.PostEntityImages[postImageName];
            }
            TracingService.Trace("Post image {0} : {1}", postImageName, (PostImage != null).ToString());
        }
        #endregion
    }
}

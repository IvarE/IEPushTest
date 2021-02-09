using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGI.CRM2013.Skanetrafiken.CaseCategory
{

    public static class EntityExtensions
    {
        ///<summary> 
        ///Extension method to get an attribute value from the entity or image 
        ///</summary> 
        ///<typeparam name="T">The attribute type</typeparam> 
        ///<param name="entity">The primary entity</param> 
        ///<param name="attributeLogicalName">Logical name of the attribute</param> 
        ///<param name="image">Image (pre/post) of the primary entity</param> 
        ///<param name="defaultValue">The default value to use</param> 
        ///<returns>The attribute value of type T</returns> 
        public static T GetAttributeValue<T>(this Entity entity, string attributeLogicalName, Entity image, T defaultValue)
        {
            return entity.Contains(attributeLogicalName)
                ? entity.GetAttributeValue<T>(attributeLogicalName)
                : image != null && image.Contains(attributeLogicalName)
                    ? image.GetAttributeValue<T>(attributeLogicalName)
                    : defaultValue;
        }

        public static T GetValue<T>(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Attributes.ContainsKey(attributeLogicalName))
                return default(T);

            if (entity[attributeLogicalName] is AliasedValue)
            {
                if (((AliasedValue)entity[attributeLogicalName]).Value == null)
                {
                    return default(T);
                }

                T _value = default(T);
                try
                {
                    _value = (T)((AliasedValue)entity[attributeLogicalName]).Value;
                }
                catch { }

                return _value;
            }
            else
            {
                return entity.GetAttributeValue<T>(attributeLogicalName);
            }
        }
    }

    internal class Common
    {
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public Common(IOrganizationService service, ITracingService tracingService)
        {
            Service = service;
            TracingService = tracingService;
        }

        public bool _isDebug = true;

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute, string destAttribute)
        {
            // Verify if attribute exists
            if (destEntity.Attributes.Contains(destAttribute) || !sourceEntity.Attributes.Contains(attribute))
                return;

            destEntity.Attributes.Add(new KeyValuePair<string, object>(destAttribute, sourceEntity.Attributes[attribute]));
        }

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute)
        {
            SetMissingEntityRef(destEntity, sourceEntity, attribute, attribute);
        }


        
    }

    internal abstract class PlugindataBase
    {
        public PlugindataBase(IServiceProvider serviceProvider)
        {
            Context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = serviceFactory.CreateOrganizationService(Context.UserId);
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Common = new Common(Service, TracingService);
        }

        public Common Common { get; set; }

        public IPluginExecutionContext Context { get; set; }
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public Entity Target { get; set; }
        public Entity PreImage { get; set; }
        public Entity PostImage { get; set; }
        public EntityReference TargetReference { get; set; }

        public void InitPreImage(string preImageName)
        {
            TracingService.Trace("Try to init pre image {0}", preImageName);
            if (Context.PreEntityImages.ContainsKey(preImageName) && Context.PreEntityImages[preImageName] is Entity)
            {
                PreImage = Context.PreEntityImages[preImageName];
            }
            TracingService.Trace("Pre image {0} : {1}", preImageName, (PreImage != null).ToString());
        }

        public void InitPostImage(string postImageName)
        {
            TracingService.Trace("Try to init post image {0}", postImageName);
            if (Context.PostEntityImages.ContainsKey(postImageName) && Context.PostEntityImages[postImageName] is Entity)
            {
                PostImage = Context.PostEntityImages[postImageName];
            }
            TracingService.Trace("Post image {0} : {1}", postImageName, (PostImage != null).ToString());
        }
    }

}

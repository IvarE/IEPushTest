using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Newtonsoft.Json.Serialization;
using log4net.Config;
using System.Net.Http;

namespace Skanetrafiken.Crm
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableCors();
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            // Converts name of properties to Camel Case while serializing the objects to JSON
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Removes XML formatter to make sure JSON data is returned for each request
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "GenericGet",
                routeTemplate: "api/{controller}",
                defaults: new
                {
                    action = "Get"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );


            config.Routes.MapHttpRoute(
                name: "Gets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    action = "GetWithId"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

#if !PRODUKTION
            config.Routes.MapHttpRoute(
                name: "Links",
                routeTemplate: "api/{controller}/GetLatestLinkGuid/{email}",
                defaults: new
                {
                    action = "GetLatestLinkGuid"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

            config.Routes.MapHttpRoute(
                name: "AngeNamn",
                routeTemplate: "api/{controller}/AngeNamnDebugPost",
                defaults: new
                {
                    action = "AngeNamnDebugPost"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );
#endif

            config.Routes.MapHttpRoute(
                name: "Posts",
                routeTemplate: "api/{controller}",
                defaults: new
                {
                    action = "Post"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );

            config.Routes.MapHttpRoute(
                name: "ArrayPosts",
                routeTemplate: "api/{controller}/ArrayPost",
                defaults: new
                {
                    action = "ArrayPost"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );


            //config.Routes.MapHttpRoute(
            //    name: "ParamPosts",
            //    routeTemplate: "api/{controller}/{time}/{price}",
            //    defaults: new
            //    {
            //        action = "Post2Param"
            //    },
            //    constraints: new
            //    {
            //        time = @"^\d{4}-\d{2}-\d{2}$",
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Post)
            //    }
            //    );


            config.Routes.MapHttpRoute(
                name: "Puts",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Put)
                }
                );

            //config.Routes.MapHttpRoute(
            //    name: "Gets and Puts",
            //    routeTemplate: "api/{controller}/{idOrEmail}",
            //    defaults: new
            //    {
            //    },
            //    constraints: new
            //    {
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put)
            //    }
            //    );

        }
    }
}

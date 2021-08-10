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
                name: "Leads",
                routeTemplate: "api/{controller}/GetLeadInfo/{campaigncode}",
                defaults: new
                {
                    action = "GetLeadInfo"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

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

            //config.Routes.MapHttpRoute(
            //    name: "Revoke",
            //    routeTemplate: "api/{controller}/Revoke",
            //    defaults: new
            //    {
            //        action = "Revoke"
            //    },

            //    constraints: new
            //    {
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Put)
            //    }
            //    );

            //config.Routes.MapHttpRoute(
            //    name: "Disconnect",
            //    routeTemplate: "api/{controller}/Disconnect",
            //    defaults: new
            //    {
            //        action = "Disconnect"
            //    },

            //    constraints: new
            //    {
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Put)
            //    }
            //    );

            config.Routes.MapHttpRoute(
                name: "CreateValueCodes",
                routeTemplate: "api/{controller}/CreateValueCode",
                defaults: new
                {
                    action = "CreateValueCode"
                },

                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );

            config.Routes.MapHttpRoute(
                name: "CreateValueCodeLossCompensations",
                routeTemplate: "api/{controller}/CreateValueCodeLossCompensation",
                defaults: new
                {
                    action = "CreateValueCodeLossCompensation"
                },

                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );

            config.Routes.MapHttpRoute(
                name: "BlockTravelCards",
                routeTemplate: "api/{controller}/BlockTravelCard",
                defaults: new
                {
                    action = "BlockTravelCard"
                },

                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );

            config.Routes.MapHttpRoute(
                name: "SyncContact",
                routeTemplate: "api/{controller}/SyncContact",
                defaults: new
                {
                    action = "SyncContact"
                },

                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
                );

            config.Routes.MapHttpRoute(
                name: "GetOrders",
                routeTemplate: "api/{controller}/GetOrders/probability",
                defaults: new
                {
                    action = "GetOrders"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

            config.Routes.MapHttpRoute(
                name: "GetCardWithCardNumber",
                routeTemplate: "api/{controller}/GetCardWithCardNumber/cardNumber",
                defaults: new
                {
                    action = "GetCardWithCardNumber"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

            config.Routes.MapHttpRoute(
                name: "PlaceOrderWithCardNumber",
                routeTemplate: "api/{controller}/PlaceOrderWithCardNumber/cardNumber",
                defaults: new
                {
                    action = "PlaceOrderWithCardNumber"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

            config.Routes.MapHttpRoute(
                name: "CancelOrderWithCardNumber",
                routeTemplate: "api/{controller}/CancelOrderWithCardNumber/cardNumber",
                defaults: new
                {
                    action = "CancelOrderWithCardNumber"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

            config.Routes.MapHttpRoute(
                name: "CaptureOrderWithCardNumber",
                routeTemplate: "api/{controller}/CaptureOrderWithCardNumber/cardNumber",
                defaults: new
                {
                    action = "CaptureOrderWithCardNumber"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
                );

                config.Routes.MapHttpRoute(
                    name: "PostDeliveryReport",
                    routeTemplate: "api/{controller}/PostDeliveryReport",
                    defaults: new
                    {
                        action = "PostDeliveryReport"
                    },

                    constraints: new
                    {
                        httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                    }
                    );

                config.Routes.MapHttpRoute(
                    name: "CreateExcelSlots",
                    routeTemplate: "api/{controller}/GetExcelBase64/slots",
                    defaults: new
                    {
                        action = "GetExcelBase64"
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                    }
                    );

            //See ValueCodeController.GetMaxAmountValueCode (Route is configured there)
            //config.Routes.MapHttpRoute(
            //    name: "GetMaxAmountValueCodes",
            //    routeTemplate: "api/{controller}/GetMaxAmountValueCode/{q}",
            //    defaults: new
            //    {
            //        action = "GetMaxAmountValueCode",
            //        q = RouteParameter.Optional
            //    },

            //    constraints: new
            //    {
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Get)
            //    }
            //    );

        }
    }
}

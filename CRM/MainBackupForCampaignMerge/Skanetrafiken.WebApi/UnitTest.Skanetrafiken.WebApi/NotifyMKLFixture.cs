using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Skanetrafiken.Crm;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Endeavor.Crm.Extensions;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class NotifyMKLFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }

        [Test, Category("Run Always")]
        public void CreateNotifyMKL()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
#if DEV
                    QueryExpression query = new QueryExpression
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(false),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                            }
                        },
                        TopCount = 5
                    };
                    IList<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);

                    Random rnd = new Random();
                    #region Create many NotifyMKL
                    {
                        var httpWebRequestMulti = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications/ArrayPost");
                        this.CreateTokenForTest(localContext, httpWebRequestMulti);
                        httpWebRequestMulti.ContentType = "application/json";
                        httpWebRequestMulti.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequestMulti.GetRequestStream()))
                        {
                            NotificationInfo[] notificationInfos = {
                            new NotificationInfo
                            {
                                Guid = contacts[0].Id.ToString(),
                                ContactType = rnd.Next(2),
                                MessageType = "MeddelandeTypExempel1",
                                SendTo = "telefonnummer eller mailadress"
                            },
                            new NotificationInfo
                            {
                                Guid = contacts[1].Id.ToString(),
                                ContactType = rnd.Next(2),
                                MessageType = "MeddelandeTypExempel2",
                                SendTo = "telefonnummer eller mailadress"
                            },
                            new NotificationInfo
                            {
                                Guid = contacts[2].Id.ToString(),
                                ContactType = rnd.Next(2),
                                MessageType = "MeddelandeTypExempel3",
                                SendTo = "telefonnummer eller mailadress"
                            },
                            new NotificationInfo
                            {
                                Guid = contacts[3].Id.ToString(),
                                ContactType = rnd.Next(2),
                                MessageType = "MeddelandeTypExempel4",
                                SendTo = "telefonnummer eller mailadress"
                            }
                        };
                            string InputJSON = SerializeNotificationInfos(localContext, notificationInfos);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponseMulti = (HttpWebResponse)httpWebRequestMulti.GetResponse();
                        using (var streamReader = new StreamReader(httpResponseMulti.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
                    #endregion

                    #region NotifyMKL
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        this.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo = new NotificationInfo
                            {
                                Guid = contacts[4].Id.ToString(),
                                ContactType = rnd.Next(2),
                                MessageType = "MeddelandeTypExempel",
                                SendTo = "telefonnummer eller mailadress"
                            };
                            string InputJSON = SerializeNotificationInfo(localContext, notificationInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
                    #endregion
#endif

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        [Test, Category("Run Always")]
        public void UseNotifyForLatestLogin()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
#if DEV
                    ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                            }
                        });


                    #region NotifyMKL
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        this.CreateTokenForTest(localContext, httpWebRequest, contact.Id);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo = new NotificationInfo
                            {
                                Guid = contact.Id.ToString(),
                                LoginDate = DateTime.Now,
                                LoginDateSpecified = true
                            };
                            string InputJSON = SerializeNotificationInfo(localContext, notificationInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
                    #endregion
#endif
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        [Test, Category("Run Always")]
        public void VerifyNotifyMKLBadRequests()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                        }
                    });

                #region No MessageType
                {
                    try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        this.CreateTokenForTest(localContext, httpWebRequest, contact.Id);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo = new NotificationInfo
                            {
                                Guid = contact.Id.ToString(),
                                ContactType = 0,
                                //MessageType = "MeddelandeTypExempel",
                                SendTo = "telefonnummer eller mailadress"
                            };
                            string InputJSON = SerializeNotificationInfo(localContext, notificationInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        if (httpResponse.StatusCode != HttpStatusCode.BadRequest)
                        {
                            throw new Exception("Could Create NotifyMKL without MessageType");
                        }

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response.StatusCode != HttpStatusCode.BadRequest)
                        {
                            throw new Exception($"NotifyMKL returned {response.StatusCode} when it should have returned {HttpStatusCode.BadRequest}");
                        }
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            //throw new Exception(result, we);
                        }
                    }
                }
                #endregion

                #region No SendTo
                {
                    try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        this.CreateTokenForTest(localContext, httpWebRequest, contact.Id);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo = new NotificationInfo
                            {
                                Guid = contact.Id.ToString(),
                                ContactType = 0,
                                MessageType = "MeddelandeTypExempel",
                                //SendTo = "telefonnummer eller mailadress"
                            };
                            string InputJSON = SerializeNotificationInfo(localContext, notificationInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        if (httpResponse.StatusCode != HttpStatusCode.BadRequest)
                        {
                            throw new Exception("Could Create NotifyMKL without MessageType");
                        }

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response.StatusCode != HttpStatusCode.BadRequest)
                        {
                            throw new Exception($"NotifyMKL returned {response.StatusCode} when it should have returned {HttpStatusCode.BadRequest}");
                        }
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            //throw new Exception(result, we);
                        }
                    }
                }
                #endregion
            }
        }


        private static string SerializeNotificationInfo(Plugin.LocalPluginContext localContext, NotificationInfo notification)
        {
            return JsonConvert.SerializeObject(notification, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private string SerializeNotificationInfos(Plugin.LocalPluginContext localContext, NotificationInfo[] notificationInfos)
        {
            return JsonConvert.SerializeObject(notificationInfos, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}

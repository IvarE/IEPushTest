using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using CGI.CRM2013.Skanetrafiken.KBArticlePlugin.ExtConnectorService;
using CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal;
using System.ServiceModel;
using System.Xml;
using Microsoft.Crm.Sdk.Messages;


namespace CGI.CRM2013.Skanetrafiken.KBArticlePlugin
{
    public class KBArticlePlugin : IPlugin
    {
        #region Declarations
        public string UnsecureConfig;
        public XmlDocument PluginConfigXml;
        public ServiceEndpoints _serviceEndpoints;
        public IOrganizationService _service;
        public ITracingService _tracingService;
        public enum Statuscode
        {
            Draft = 1,
            Unapproved = 2,
            Published = 3
        };
        #endregion



        #region Constructors
        public KBArticlePlugin()
        {
            UnsecureConfig = string.Empty;
        }

        #endregion

        #region Public Methods -----------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // get reference to CRM Web ServiceIOrganizationServiceFactory 
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            _tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            _service = service;
            _serviceEndpoints = getSettings(service);
            //_tracingService = tracingService;

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];
                    if (entity.LogicalName != "kbarticle")
                        return;
                                            
                    if (entity.Attributes.Contains("statuscode"))
                    {
                        _tracingService.Trace("Statuscode: " + entity.GetAttributeValue<OptionSetValue>("statuscode").Value);
                        if (((OptionSetValue)entity.Attributes["statuscode"]).Value == (int)Statuscode.Unapproved)
                        {
                            UnPublishArticle(context, service, entity);
                        }
                        else if (((OptionSetValue)entity.Attributes["statuscode"]).Value == (int)Statuscode.Published)
                        {
                            PublishArticle(context, service, entity);
                        }
                        _tracingService.Trace("End..");
                    }
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace("Execution method. Catch.");
                throw new InvalidPluginExecutionException(OperationStatus.Failed, ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="preImage"></param>
        /// <returns></returns>

        public KBArtiklarRequest GetKBArticleIntranet(Entity preImage)
        {
            _tracingService.Trace("GetKBArticleIntranet.. start");
            KBArtiklarRequest kbArticleIntranet = new KBArtiklarRequest();

            kbArticleIntranet.Title = preImage.Attributes["title"].ToString();
            //throw new InvalidPluginExecutionException("Title ok");
            kbArticleIntranet.content = "";
            if (preImage.Contains("content"))
            {
                kbArticleIntranet.content = System.Net.WebUtility.HtmlEncode(preImage.Attributes["content"].ToString());
            }

            kbArticleIntranet.ExternalContent = "";
            if (preImage.Contains("cgi_externaltext"))
            {
                kbArticleIntranet.ExternalContent = System.Net.WebUtility.HtmlEncode(preImage.Attributes["cgi_externaltext"].ToString());
            }
            //kbArticleIntranet.ExternalContent = preImage.Attributes.Contains("cgi_externaltext") ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "") : string.Empty;



            //kbArticleIntranet.CreatedByUser = ((EntityReference)preImage.Attributes["createdby"]).Name.ToString();
            kbArticleIntranet.CreatedByUser = ((EntityReference)preImage.Attributes["modifiedby"]).Name.ToString();

            kbArticleIntranet.Keywords = preImage.Attributes["keywords"].ToString();

            kbArticleIntranet.modifiedon = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd HH:mm}", RetrieveUTCTimeFromLocalTime((DateTime)preImage.Attributes["modifiedon"], _service)));
            kbArticleIntranet.modifiedonSpecified = true;

            kbArticleIntranet.Number = preImage.Attributes["number"].ToString();
            
            kbArticleIntranet.subject = ((EntityReference)preImage.Attributes["subjectid"]).Name;
            
            kbArticleIntranet.Action = "";

            _tracingService.Trace("GetKBArticleIntranet.. end");

            return kbArticleIntranet;


            //return kbArticleIntranet;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preImage"></param>
        /// <returns></returns>
        public Article GetKBArticleExternalWeb(Entity preImage)
        {
            Article kbArticleExternalWeb = new Article
            {
                Title = preImage.Attributes["title"].ToString(),
                KnowledgeBaseArticleId = preImage.Attributes["number"].ToString(),
                ExternalText =
                    preImage.Attributes.Contains("cgi_externaltext")
                        ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "") ///TODO Find other solution to Replace function...
                        : string.Empty,
                Authors = new[] {((EntityReference) preImage.Attributes["modifiedby"]).Name},
                PublishDate = (DateTime) preImage.Attributes["createdon"],
                Category = preImage.Attributes["keywords"].ToString(),
                UpdateDate = (DateTime) preImage.Attributes["modifiedon"],
                Summary =
                    preImage.Attributes.Contains("subjectid")
                        ? ((EntityReference) preImage.Attributes["subjectid"]).Name
                        : string.Empty
            };

            return kbArticleExternalWeb;
        }

        public ICMSIndexService GetChannel(IOrganizationService service)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding
            {
                Security =
                {
                    Mode = BasicHttpSecurityMode.None,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.None,
                        ProxyCredentialType = HttpProxyCredentialType.None
                    },
                    Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
                }
            };

            
            EndpointAddress endPointAddress = new EndpointAddress(_serviceEndpoints.WebEndpoint);
            ChannelFactory<ICMSIndexService> factory = new ChannelFactory<ICMSIndexService>(myBinding, endPointAddress);
            ICMSIndexService channel = factory.CreateChannel();

            return channel;
        }

        public Kunskapsartiklar kbArticleChannel() {
            _tracingService.Trace(string.Format("{0}", _serviceEndpoints.IntranetEndpoint.ToString()));

            EndpointAddress endPointAddress = new EndpointAddress(_serviceEndpoints.IntranetEndpoint);

            _tracingService.Trace(string.Format("{0}", endPointAddress.ToString()));

            BasicHttpBinding myBinding = new BasicHttpBinding
            {
                Security =
                {
                    Mode = BasicHttpSecurityMode.None,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.None,
                        ProxyCredentialType = HttpProxyCredentialType.None
                    },
                    Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
                }
            };
           
            ChannelFactory<Kunskapsartiklar> factory = new ChannelFactory<Kunskapsartiklar>(myBinding, endPointAddress);
            var channel= factory.CreateChannel();

            return channel;

        }

        public static string GetConfigDataString(XmlDocument doc, string strXml)
        {
            return GetValueNode(doc, strXml);
        }

        public static string GetSearchHitTypeNameString(XmlDocument doc, string strXml)
        {
            return GetValueNode(doc, strXml);
        }
        #endregion

        #region Private Methods

        private void UnPublishArticle(IPluginExecutionContext context, IOrganizationService service, Entity entity)
        {
            _tracingService.Trace("UnPublishArticle method..");
            _tracingService.Trace("Contains PreImage: " + context.PreEntityImages.Contains("PreImage").ToString());
            if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] != null)
            {
                Entity preImage = context.PreEntityImages["PreImage"];
                _tracingService.Trace("PreImage contains externalwebindex: " + preImage.Attributes.Contains("cgi_externalwebindex").ToString());
                if (preImage.Attributes.Contains("cgi_externalwebindex"))
                {
                    // unpublish article on web
                    _tracingService.Trace("unpublish article on web, cgi_externalwebindex: " + preImage.GetAttributeValue<string>("cgi_externalwebindex"));
                
                    string strExternalWebIndex = string.Empty;

                    if (preImage.GetAttributeValue<string>("cgi_externalwebindex") != null || preImage.GetAttributeValue<string>("cgi_externalwebindex") != string.Empty)
                        strExternalWebIndex = preImage.GetAttributeValue<string>("cgi_externalwebindex");


                    RemoveIndexRequest removeRequest = new RemoveIndexRequest();

                    if (strExternalWebIndex != string.Empty)
                    {
                        removeRequest.ExternalWebIndexId = strExternalWebIndex;

                        ICMSIndexService channel = GetChannel(service);
                        RemoveIndexResponse removeResponse = channel.RemoveIndex(removeRequest);

                        _tracingService.Trace("Remove response status: " + removeResponse.Status.ToString());
                        if (removeResponse.Status.ToString().ToLower() == "success")
                        {
                            entity.Attributes.Add("cgi_intranetindex", string.Empty);
                            entity.Attributes.Add("cgi_externalwebindex", string.Empty);

                            if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                            {
                                OptionSetValue o = new OptionSetValue(285050001);  //Pending
                                if (!entity.Attributes.Contains("cgi_appoval"))
                                    entity.Attributes.Add("cgi_appoval", o);
                                else
                                    entity.Attributes["cgi_appoval"] = o;
                            }
                        }
                    }
                }

                //throw new Exception();
                
                Kunskapsartiklar intranetClient = kbArticleChannel();

                KBArtiklarRequest kbArticleIntranetRequest = GetKBArticleIntranet(preImage);
                kbArticleIntranetRequest.Action = "DELETE";

                _tracingService.Trace(string.Format("Title: {0}; Subject: {1}; Number: {2}; Content: {3}; ExternalContent: {4}; Action: {5}; CreatedByUser: {6}; ModifedOn: {7}; Keywords: {8};",
                    kbArticleIntranetRequest.Title, kbArticleIntranetRequest.subject, kbArticleIntranetRequest.Number, kbArticleIntranetRequest.content, kbArticleIntranetRequest.ExternalContent,
                    kbArticleIntranetRequest.Action, kbArticleIntranetRequest.CreatedByUser, kbArticleIntranetRequest.modifiedon, kbArticleIntranetRequest.Keywords));

                try
                    {
                    KBArtiklarRequest1 request1 = new KBArtiklarRequest1();
                    request1.KBArtiklarRequest = kbArticleIntranetRequest;
                    KBArtiklarResponse1 response = intranetClient.KBArtiklar(request1);

                    _tracingService.Trace("Message: " + response.KBArtiklarResponse.Message);
                    if (response.KBArtiklarResponse.Message == "Error")
                    {
                        _tracingService.Trace("ErrorMessage: " + response.KBArtiklarResponse.Error.ErrorMessage);
                        _tracingService.Trace((response.KBArtiklarResponse.Error.ErrorMessage != "Mismatch between articles found and expected min max hitcount: 1-1").ToString());
                        if (response.KBArtiklarResponse.Error.ErrorMessage != "Object reference not set to an instance of an object." &&
                            response.KBArtiklarResponse.Error.HttpStatusCode != "500")
                        {
                            throw new InvalidPluginExecutionException("Error: " + response.KBArtiklarResponse.Error.ErrorMessage);
                        }
                    }

                }
                catch (Exception e)
                {
                    _tracingService.Trace("UnPublishArticle.. Catch Error: " + e.Message);
                    throw new InvalidPluginExecutionException("Error: " + e.Message);
                }


            }
        }

        private void PublishArticle(IPluginExecutionContext context, IOrganizationService service, Entity entity)
        {
            _tracingService.Trace("PublishArticle method..");
            //_tracingService.Trace("Contains PreImage: " + context.PreEntityImages.Contains("PreImage").ToString());
            if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] != null)
            {
                Entity preImage = context.PreEntityImages["PreImage"];
                
                CreateIndexRequest createRequest = new CreateIndexRequest();

                //_tracingService.Trace("cgi_appoval: " + preImage.GetAttributeValue<OptionSetValue>("cgi_appoval").Value);

                //Internal publishing //Approved
                if (((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050002 || ((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050003)   
                {
                    // if publish on web
                    if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                    {
                        _tracingService.Trace("if publish on web");

                        Article kbArticleExternalWeb = GetKBArticleExternalWeb(preImage);

                        //if (kbArticleExternalWeb != null)
                        //    _tracingService.Trace("kbArticleExternalWeb: " + kbArticleExternalWeb.Title);
                        //else
                        //    _tracingService.Trace("kbArticleExternalWeb is null");

                        createRequest.RequestActionType = ActionType.ExternalWeb;
                        createRequest.KbArticleForExternalWeb = kbArticleExternalWeb;

                        ICMSIndexService channel = GetChannel(service);
                        CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

                        _tracingService.Trace("createResponse.ExternalWebIndexId: " + createResponse.ExternalWebIndexId);

                        if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                        {

                            if (entity.Attributes.Contains("cgi_externalwebindex"))
                                entity.Attributes["cgi_externalwebindex"] = createResponse.ExternalWebIndexId;
                            else
                                entity.Attributes.Add("cgi_externalwebindex", createResponse.ExternalWebIndexId);
                        }
                    }   
                }

                //throw new Exception("Hello world!");

                // publish on intranet
                _tracingService.Trace("publish on intranet");



                Kunskapsartiklar intranetClient = kbArticleChannel();

                KBArtiklarRequest kbArticleIntranetRequest = GetKBArticleIntranet(preImage);
                kbArticleIntranetRequest.Action = "UPDATE";

                _tracingService.Trace(string.Format("Title: {0}; Subject: {1}; Number: {2}; Content: {3}; ExternalContent: {4}; Action: {5}; CreatedByUser: {6}; ModifedOn: {7}; Keywords: {8};",
                    kbArticleIntranetRequest.Title, kbArticleIntranetRequest.subject, kbArticleIntranetRequest.Number, kbArticleIntranetRequest.content, kbArticleIntranetRequest.ExternalContent,
                    kbArticleIntranetRequest.Action, kbArticleIntranetRequest.CreatedByUser, kbArticleIntranetRequest.modifiedon, kbArticleIntranetRequest.Keywords));
                                
                try
                {
                    KBArtiklarRequest1 request1 = new KBArtiklarRequest1();
                    request1.KBArtiklarRequest = kbArticleIntranetRequest;
                    KBArtiklarResponse1 response = intranetClient.KBArtiklar(request1);
                    _tracingService.Trace(string.Format("Response Message: {0}", response.KBArtiklarResponse.Message));

                    if (response.KBArtiklarResponse.Message == "Error")
                    {
                        //_tracingService.Trace("response.KBArtiklarResponse.Error.ErrorMessage");
                        throw new InvalidPluginExecutionException("Biztalk Error: " + response.KBArtiklarResponse.Error.ErrorMessage);
                    }

                    //throw new InvalidPluginExecutionException();

                }
                catch (Exception e)
                {
                    _tracingService.Trace("PublishArticle.. Catch Error: " + e.Message);
                    throw new InvalidPluginExecutionException("Error: " + e.Message);
                }
            }
        }

        private static string GetValueNode(XmlDocument doc, string strXml)
        {
            XmlNode node = doc.SelectSingleNode(strXml);//(String.Format("Configuration/EndPoint"));
            if (node != null)
            {
                return node.InnerXml;
            }
            return string.Empty;
        }

        private ServiceEndpoints getSettings(IOrganizationService service)
        {
            ServiceEndpoints serviceEndpoints = new ServiceEndpoints();

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_extconnectorservice' />";
            xml += "       <attribute name='cgi_kbarticlesforintranetservice' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            FetchExpression f = new FetchExpression(xml);
            EntityCollection ents = service.RetrieveMultiple(f);
            if (ents != null && ents.Entities.Any())
            {
                Entity ent = ents[0];
                if (ent.Attributes.Contains("cgi_extconnectorservice"))
                {
                    serviceEndpoints.WebEndpoint = ent.Attributes["cgi_extconnectorservice"].ToString();
                }
                
                if (ent.Attributes.Contains("cgi_kbarticlesforintranetservice"))
                {
                    serviceEndpoints.IntranetEndpoint = ent.Attributes["cgi_kbarticlesforintranetservice"].ToString();
                }
            }
            else
            {
                throw new InvalidPluginExecutionException("Settings is missing!");
            }

            
            return serviceEndpoints;
        }

        public static DateTime RetrieveUTCTimeFromLocalTime(DateTime localTime, IOrganizationService service)
        {

            LocalTimeFromUtcTimeRequest request = new LocalTimeFromUtcTimeRequest();
            request.UtcTime = localTime;
            request.TimeZoneCode = 110;

            var response = (LocalTimeFromUtcTimeResponse)service.Execute(request);
            

            return response.LocalTime;


        }
        #endregion
    }
}

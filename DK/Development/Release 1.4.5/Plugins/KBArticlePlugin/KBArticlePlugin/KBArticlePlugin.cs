using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService;
using System.ServiceModel;
using System.Xml;


namespace CGI.CRM2013.Skanetrafiken.KBArticlePlugin
{
    public class KBArticlePlugin : IPlugin
    {
        #region Declarations
        public string UnsecureConfig;
        public XmlDocument PluginConfigXml;
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
            try
            {
                // Obtain the execution context from the service provider.
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // get reference to CRM Web ServiceIOrganizationServiceFactory 
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];
                    if (entity.LogicalName != "kbarticle")
                        return;

                    if (entity.Attributes.Contains("statuscode"))
                    {
                        if (((OptionSetValue)entity.Attributes["statuscode"]).Value == 2)
                        {
                            Test(context, service, entity);
                        }
                        else if (((OptionSetValue)entity.Attributes["statuscode"]).Value == 3)
                        {
                            Test2(context, service, entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="preImage"></param>
        /// <returns></returns>
        public Article GetKBArticleIntranet(Entity preImage)
        {
            Article kbArticleIntranet = new Article
            {
                Title = preImage.Attributes["title"].ToString(),
                KnowledgeBaseArticleId = preImage.Attributes["number"].ToString(),
                InternalText =
                    preImage.Attributes.Contains("content")
                        ? preImage.Attributes["content"].ToString().Replace("___", "")
                        : string.Empty,
                ExternalText =
                    preImage.Attributes.Contains("cgi_externaltext")
                        ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "")
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

            return kbArticleIntranet;

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
                        ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "")
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


            string strEndPointAddress = _getSetting(service);
            EndpointAddress endPointAddress = new EndpointAddress(strEndPointAddress);
            ChannelFactory<ICMSIndexService> factory = new ChannelFactory<ICMSIndexService>(myBinding, endPointAddress);
            ICMSIndexService channel = factory.CreateChannel();

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

        private void Test(IPluginExecutionContext context, IOrganizationService service, Entity entity)
        {
            if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] != null)
            {
                Entity preImage = context.PreEntityImages["PreImage"];
                if (preImage.Attributes.Contains("cgi_intranetindex") || preImage.Attributes.Contains("cgi_externalwebindex"))
                {
                    string strIntranetIndex = string.Empty;
                    string strExternalWebIndex = string.Empty;

                    if (preImage.GetAttributeValue<string>("cgi_intranetindex") != null || preImage.GetAttributeValue<string>("cgi_intranetindex") != string.Empty)
                        strIntranetIndex = preImage.GetAttributeValue<string>("cgi_intranetindex");
                    if (preImage.GetAttributeValue<string>("cgi_externalwebindex") != null || preImage.GetAttributeValue<string>("cgi_externalwebindex") != string.Empty)
                        strExternalWebIndex = preImage.GetAttributeValue<string>("cgi_externalwebindex");

                    if (strIntranetIndex == string.Empty && strExternalWebIndex == string.Empty)
                    {
                        return;
                    }
                    RemoveIndexRequest removeRequest = new RemoveIndexRequest();
                    if (strIntranetIndex != string.Empty)
                    {
                        removeRequest.IntranetIndexId = strIntranetIndex;
                        removeRequest.ExternalWebIndexId = string.Empty;
                    }
                    if (strExternalWebIndex != string.Empty)
                        removeRequest.ExternalWebIndexId = strExternalWebIndex;

                    removeRequest.IntranetIndexId = strIntranetIndex;
                    removeRequest.ExternalWebIndexId = strExternalWebIndex;

                    ICMSIndexService channel = GetChannel(service);
                    RemoveIndexResponse removeResponse = channel.RemoveIndex(removeRequest);

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
                                entity.Attributes["cgi_apoval"] = o;
                        }
                    }
                }
            }
        }

        private void Test2(IPluginExecutionContext context, IOrganizationService service, Entity entity)
        {
            if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] != null)
            {
                Entity preImage = context.PreEntityImages["PreImage"];

                CreateIndexRequest createRequest = new CreateIndexRequest();

                if (((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050002 || //Internal publishing
                  ((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050003)   //Approved
                {
                    if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                    {
                        Article kbArticleExternalWeb = GetKBArticleExternalWeb(preImage);
                        var kbArticleIntranet = GetKBArticleIntranet(preImage);

                        createRequest.RequestActionType = ActionType.Both;
                        createRequest.KbArticleForExternalWeb = kbArticleExternalWeb;
                        createRequest.KbArticleForIntranet = kbArticleIntranet;
                    }
                    else
                    {
                        Article kbArticleIntranet = GetKBArticleIntranet(preImage);

                        createRequest.RequestActionType = ActionType.Intranet;
                        createRequest.KbArticleForIntranet = kbArticleIntranet;
                    }

                    ICMSIndexService channel = GetChannel(service);
                    CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

                    if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                    {
                        if (entity.Attributes.Contains("cgi_intranetindex"))
                            entity.Attributes["cgi_intranetindex"] = createResponse.IntranetIndexId;
                        else
                            entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId);

                        if (entity.Attributes.Contains("cgi_externalwebindex"))
                            entity.Attributes["cgi_externalwebindex"] = createResponse.ExternalWebIndexId;
                        else
                            entity.Attributes.Add("cgi_externalwebindex", createResponse.ExternalWebIndexId);
                    }
                    else
                    {
                        entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId);
                    }
                }
                else if (((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050001)   //Pending
                {
                    if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                    {
                        Article kbArticleIntranet = GetKBArticleIntranet(preImage);

                        createRequest.RequestActionType = ActionType.Intranet;
                        createRequest.KbArticleForIntranet = kbArticleIntranet;

                        ICMSIndexService channel = GetChannel(service);
                        CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

                        if (preImage.GetAttributeValue<bool>("cgi_publishonweb"))
                        {
                            if (preImage.Attributes.Contains("cgi_intranetindex"))
                            {
                                entity.Attributes["cgi_intranetindex"] = createResponse.IntranetIndexId;
                            }
                            else
                            {
                                entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId);
                            }
                        }
                    }
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

        private string _getSetting(IOrganizationService service)
        {
            string returnValue = "";

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_extconnectorservice' />";
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
                    returnValue = ent.Attributes["cgi_extconnectorservice"].ToString();
                }
            }
            else
            {
                throw new InvalidPluginExecutionException("Settings is missing!");
            }

            return returnValue;
        }
        #endregion
    }
}
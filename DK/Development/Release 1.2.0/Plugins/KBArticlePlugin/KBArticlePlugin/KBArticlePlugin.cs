using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService;
using System.ServiceModel;
using System.Xml;


namespace CGI.CRM2013.Skanetrafiken.KBArticlePlugin {
  public class KBArticlePlugin : IPlugin {
	//string strEndPointAddress = string.Empty;
	public string _unsecureConfig = string.Empty;
	public XmlDocument _pluginConfigXML;
	public KBArticlePlugin(string unsecureConfig, string secureConfig) {

	}

	public void Execute(IServiceProvider serviceProvider) {
	  try {
		// Obtain the execution context from the service provider.
		IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

		// get reference to CRM Web ServiceIOrganizationServiceFactory 
		IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
		IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

		if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity) {
		  Entity entity = (Entity)context.InputParameters["Target"];
		  if (!(entity.LogicalName == "kbarticle"))
			return;

		  if (entity.Attributes.Contains("statuscode")) {
			if (((OptionSetValue)entity.Attributes["statuscode"]).Value == 2) {
			  //entity.Attributes.Add("cgi_intranetindex", "draft");
			  if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity) {
				Entity preImage = (Entity)context.PreEntityImages["PreImage"];
				if (preImage.Attributes.Contains("cgi_intranetindex") || preImage.Attributes.Contains("cgi_externalwebindex")) {
				  string strIntranetIndex = string.Empty;
				  string strExternalWebIndex = string.Empty;

				  if (preImage.GetAttributeValue<string>("cgi_intranetindex") != null || preImage.GetAttributeValue<string>("cgi_intranetindex") != string.Empty)
					strIntranetIndex = preImage.GetAttributeValue<string>("cgi_intranetindex");
				  if (preImage.GetAttributeValue<string>("cgi_externalwebindex") != null || preImage.GetAttributeValue<string>("cgi_externalwebindex") != string.Empty)
					strExternalWebIndex = preImage.GetAttributeValue<string>("cgi_externalwebindex");

				  if (strIntranetIndex == string.Empty && strExternalWebIndex == string.Empty)
					return;
				  else {
					WCFService.RemoveIndexRequest removeRequest = new WCFService.RemoveIndexRequest();
					if (strIntranetIndex != string.Empty) {
					  removeRequest.IntranetIndexId = strIntranetIndex;
					  removeRequest.ExternalWebIndexId = string.Empty;
					}
					if (strExternalWebIndex != string.Empty)
					  removeRequest.ExternalWebIndexId = strExternalWebIndex;

					removeRequest.IntranetIndexId = strIntranetIndex;
					removeRequest.ExternalWebIndexId = strExternalWebIndex;

					ICMSIndexService channel = GetChannel(service);
					WCFService.RemoveIndexResponse removeResponse = channel.RemoveIndex(removeRequest);

					if (removeResponse.Status.ToString().ToLower() == "success") {
					  entity.Attributes.Add("cgi_intranetindex", string.Empty);
					  entity.Attributes.Add("cgi_externalwebindex", string.Empty);

					  if (preImage.GetAttributeValue<bool>("cgi_publishonweb") == true) {
						OptionSetValue _o = new OptionSetValue(285050001);  //Pending
						if (!entity.Attributes.Contains("cgi_appoval"))
						  entity.Attributes.Add("cgi_appoval", _o);
						else
						  entity.Attributes["cgi_apoval"] = _o;
					  }
					}
				  }
				}
			  }
			}
			else if (((OptionSetValue)entity.Attributes["statuscode"]).Value == 3) {
			  if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity) {
				Entity preImage = (Entity)context.PreEntityImages["PreImage"];

				WCFService.CreateIndexRequest createRequest = new WCFService.CreateIndexRequest();

				if (((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050002 || //Internal publishing
				  ((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050003)   //Approved
                                 {
				  if (preImage.GetAttributeValue<bool>("cgi_publishonweb") == true) {
					// entity.Attributes.Add("keywords", str);

					WCFService.Article kbArticleExternalWeb = GetKBArticleExternalWeb(preImage);
					WCFService.Article kbArticleIntranet = GetKBArticleIntranet(preImage);

					createRequest.RequestActionType = WCFService.ActionType.Both;
					createRequest.KbArticleForExternalWeb = kbArticleExternalWeb;
					createRequest.KbArticleForIntranet = kbArticleIntranet;
				  }
				  else {
					// entity.Attributes.Add("cgi_intranetindex", "Intranet");
					WCFService.Article kbArticleIntranet = GetKBArticleIntranet(preImage);

					createRequest.RequestActionType = WCFService.ActionType.Intranet;
					createRequest.KbArticleForIntranet = kbArticleIntranet;
				  }

				  ICMSIndexService channel = GetChannel(service);
				  // entity.Attributes.Add("keywords", _unsecureConfig);
				  WCFService.CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

				  if (preImage.GetAttributeValue<bool>("cgi_publishonweb") == true) {
					if (entity.Attributes.Contains("cgi_intranetindex"))
					  entity.Attributes["cgi_intranetindex"] = createResponse.IntranetIndexId.ToString();
					else
					  entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId.ToString());

					if (entity.Attributes.Contains("cgi_externalwebindex"))
					  entity.Attributes["cgi_externalwebindex"] = createResponse.ExternalWebIndexId.ToString();
					else
					  entity.Attributes.Add("cgi_externalwebindex", createResponse.ExternalWebIndexId.ToString());
				  }
				  else {
					entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId.ToString());
				  }
				}
				else if (((OptionSetValue)preImage.Attributes["cgi_appoval"]).Value == 285050001)   //Pending
                                 {
				  if (preImage.GetAttributeValue<bool>("cgi_publishonweb") == true) {
					// entity.Attributes.Add("keywords", str);

					WCFService.Article kbArticleIntranet = GetKBArticleIntranet(preImage);

					createRequest.RequestActionType = WCFService.ActionType.Intranet;
					createRequest.KbArticleForIntranet = kbArticleIntranet;

					ICMSIndexService channel = GetChannel(service);
					// entity.Attributes.Add("keywords", _unsecureConfig);
					WCFService.CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

					if (preImage.GetAttributeValue<bool>("cgi_publishonweb") == true) {
					  if (preImage.Attributes.Contains("cgi_intranetindex")) {
						entity.Attributes["cgi_intranetindex"] = createResponse.IntranetIndexId.ToString();
					  }
					  else {
						entity.Attributes.Add("cgi_intranetindex", createResponse.IntranetIndexId.ToString());
					  }
					}
				  }
				}
			  }
			}
		  }
		}
	  }
	  catch (Exception ex) {
		throw new InvalidPluginExecutionException(ex.ToString());
	  }
	}

	//public WCFService.KBArticle GetKBArticleIntranet(Entity preImage)
	//{
	//    WCFService.KBArticle kbArticleIntranet = new WCFService.KBArticle();
	//    kbArticleIntranet.SearchTitle = preImage.Attributes["title"].ToString();
	//    kbArticleIntranet.SearchHitTypeName = GetSearchHitTypeNameString(_pluginConfigXML, "Configuration/SearchHitTypeName");
	//    kbArticleIntranet.SearchSection = preImage.Attributes["number"].ToString();
	//    kbArticleIntranet.SearchText = preImage.Attributes.Contains("content") ? preImage.Attributes["content"].ToString() : string.Empty;
	//    // kbArticleIntranet.SearchAuthors[0] = preImage.Attributes["createdby"].ToString();
	//    kbArticleIntranet.SearchPublishDate = (DateTime)preImage.Attributes["createdon"];
	//    //kbArticle. = preImage.Attributes["keywords"].ToString();
	//    kbArticleIntranet.SearchUpdateDate = (DateTime)preImage.Attributes["modifiedon"];
	//    kbArticleIntranet.SearchSummary = preImage.Attributes["subjectid"].ToString();

	//    return kbArticleIntranet;

	//}

	//public WCFService.KBArticle GetKBArticleExternalWeb(Entity preImage)
	//{
	//    WCFService.KBArticle kbArticleExternalWeb = new WCFService.KBArticle();
	//    kbArticleExternalWeb.SearchTitle = preImage.Attributes["title"].ToString();
	//    kbArticleExternalWeb.SearchSection = preImage.Attributes["number"].ToString();
	//    kbArticleExternalWeb.SearchHitTypeName = GetSearchHitTypeNameString(_pluginConfigXML, "Configuration/SearchHitTypeName");
	//    kbArticleExternalWeb.SearchText = preImage.Attributes.Contains("cgi_externaltext") ? preImage.Attributes["cgi_externaltext"].ToString() : string.Empty;
	//    //  kbArticleExternalWeb.SearchAuthors[0] = preImage.Attributes["createdby"].ToString();
	//    kbArticleExternalWeb.SearchPublishDate = (DateTime)preImage.Attributes["createdon"];
	//    // kbArticleExternalWeb. = preImage.Attributes["keywords"].ToString();
	//    kbArticleExternalWeb.SearchUpdateDate = (DateTime)preImage.Attributes["modifiedon"];
	//    kbArticleExternalWeb.SearchSummary = preImage.Attributes["subjectid"].ToString();

	//    return kbArticleExternalWeb;
	//}

	public WCFService.Article GetKBArticleIntranet(Entity preImage) {
	  WCFService.Article kbArticleIntranet = new WCFService.Article();
	  kbArticleIntranet.Title = preImage.Attributes["title"].ToString();
	  //kbArticleIntranet.SearchHitTypeName = GetSearchHitTypeNameString(_pluginConfigXML, "Configuration/SearchHitTypeName");
	  kbArticleIntranet.KnowledgeBaseArticleId = preImage.Attributes["number"].ToString();
	  kbArticleIntranet.InternalText = preImage.Attributes.Contains("content") ? preImage.Attributes["content"].ToString().Replace("___","") : string.Empty;
      kbArticleIntranet.ExternalText = preImage.Attributes.Contains("cgi_externaltext") ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "") : string.Empty;
	  kbArticleIntranet.Authors = new string[] { ((EntityReference)preImage.Attributes["modifiedby"]).Name };
	  kbArticleIntranet.PublishDate = (DateTime)preImage.Attributes["createdon"];
	  kbArticleIntranet.Category = preImage.Attributes["keywords"].ToString();
	  kbArticleIntranet.UpdateDate = (DateTime)preImage.Attributes["modifiedon"];
	  kbArticleIntranet.Summary = preImage.Attributes.Contains("subjectid") ? ((EntityReference)preImage.Attributes["subjectid"]).Name : string.Empty; ;

	  return kbArticleIntranet;

	}

	public WCFService.Article GetKBArticleExternalWeb(Entity preImage) {
	  WCFService.Article kbArticleExternalWeb = new WCFService.Article();
	  kbArticleExternalWeb.Title = preImage.Attributes["title"].ToString();
	  kbArticleExternalWeb.KnowledgeBaseArticleId = preImage.Attributes["number"].ToString();
	  //kbArticleExternalWeb.SearchHitTypeName = GetSearchHitTypeNameString(_pluginConfigXML, "Configuration/SearchHitTypeName");
      kbArticleExternalWeb.ExternalText = preImage.Attributes.Contains("cgi_externaltext") ? preImage.Attributes["cgi_externaltext"].ToString().Replace("___", "") : string.Empty;
	  kbArticleExternalWeb.Authors = new string[] { ((EntityReference)preImage.Attributes["modifiedby"]).Name };
	  kbArticleExternalWeb.PublishDate = (DateTime)preImage.Attributes["createdon"];
	  kbArticleExternalWeb.Category = preImage.Attributes["keywords"].ToString();
	  kbArticleExternalWeb.UpdateDate = (DateTime)preImage.Attributes["modifiedon"];
	  kbArticleExternalWeb.Summary = preImage.Attributes.Contains("subjectid") ? ((EntityReference)preImage.Attributes["subjectid"]).Name : string.Empty;

	  return kbArticleExternalWeb;
	}
	public ICMSIndexService GetChannel(IOrganizationService service) {
	  BasicHttpBinding myBinding = new BasicHttpBinding();

	  myBinding.Security.Mode = BasicHttpSecurityMode.None;
	  myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
	  myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
	  myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

	  string strEndPointAddress = _getSetting(service);
	  EndpointAddress endPointAddress = new EndpointAddress(strEndPointAddress);
	  ChannelFactory<ICMSIndexService> factory = new ChannelFactory<ICMSIndexService>(myBinding, endPointAddress);
	  ICMSIndexService channel = factory.CreateChannel();

	  return channel;
	}

	public static string GetConfigDataString(XmlDocument doc, string strXML) {
	  return GetValueNode(doc, strXML);
	}

	public static string GetSearchHitTypeNameString(XmlDocument doc, string strXML) {
	  return GetValueNode(doc, strXML);
	}

	private static string GetValueNode(XmlDocument doc, string strXML) {
	  XmlNode node = doc.SelectSingleNode(strXML);//(String.Format("Configuration/EndPoint"));
	  if (node != null) {
		return node.InnerXml.ToString();
	  }
	  return string.Empty;
	}

	private string _getSetting(IOrganizationService service) {
	  string _returnValue = "";

	  string _now = DateTime.Now.ToString("s");
	  string _xml = "";
	  _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
	  _xml += "   <entity name='cgi_setting'>";
	  _xml += "       <attribute name='cgi_extconnectorservice' />";
	  _xml += "       <filter type='and'>";
	  _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
	  _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
	  _xml += "           <filter type='or'>";
	  _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
	  _xml += "               <condition attribute='cgi_validto' operator='null' />";
	  _xml += "           </filter>";
	  _xml += "       </filter>";
	  _xml += "   </entity>";
	  _xml += "</fetch>";

	  FetchExpression _f = new FetchExpression(_xml);
	  EntityCollection _ents = service.RetrieveMultiple(_f);
	  if (_ents != null && _ents.Entities.Count() > 0) {
		Entity _ent = _ents[0] as Entity;
		if (_ent.Attributes.Contains("cgi_extconnectorservice")) {
		  _returnValue = _ent.Attributes["cgi_extconnectorservice"].ToString();
		}
	  }
	  else {
		throw new InvalidPluginExecutionException("Settings is missing!");
	  }

	  return _returnValue;
	}


  }
}

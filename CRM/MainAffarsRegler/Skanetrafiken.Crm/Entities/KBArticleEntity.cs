using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class ServiceEndpoints
    {
        public string WebEndpoint { get; set; }
        public string IntranetEndpoint { get; set; }
    }

    public class KBArticleEntity : Generated.KbArticle
    {
        public ServiceEndpoints _serviceEndpoints;

        //ADD Service References for the Extra Models

        //internal void HandlePreKBArticleUpdateSync(Plugin.LocalPluginContext localContext)
        //{
        //    try
        //    {
        //        _serviceEndpoints = getSettings(localContext);

        //        if (this.LogicalName != KBArticleEntity.EntityLogicalName)
        //            return;

        //        if (this.StatusCode != null)
        //        {
        //            KBArticleEntity preImage = Plugin.GetPreImage<KBArticleEntity>(localContext, "PreImage");

        //            localContext.Trace("Statuscode: " + this.StatusCode.Value);
        //            if ((int)this.StatusCode.Value == (int)kbarticle_statuscode.Unapproved)
        //            {
        //                UnPublishArticle(localContext, preImage);
        //            }
        //            else if ((int)this.StatusCode.Value == (int)kbarticle_statuscode.Published)
        //            {
        //                PublishArticle(localContext, preImage);
        //            }
        //            localContext.Trace("End..");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        localContext.Trace($"HandlePreKBArticleUpdateSync threw an unexpected exception: {e.Message}");
        //        throw e;
        //    }
        //}

        //private void UnPublishArticle(Plugin.LocalPluginContext localContext, KBArticleEntity preImage)
        //{
        //    localContext.Trace("UnPublishArticle method..");
        //    localContext.Trace("Contains PreImage: " + localContext.PluginExecutionContext.PreEntityImages.Contains("PreImage").ToString());

        //    if (preImage != null)
        //    {
        //        localContext.Trace("PreImage contains externalwebindex: " + preImage.cgi_ExternalWebIndex);
        //        if (preImage.cgi_ExternalWebIndex != null && preImage.cgi_ExternalWebIndex != string.Empty)
        //        {
        //            string strExternalWebIndex = preImage.cgi_ExternalWebIndex;

        //            // unpublish article on web
        //            localContext.Trace("unpublish article on web, cgi_externalwebindex: " + strExternalWebIndex);

        //            RemoveIndexRequest removeRequest = new RemoveIndexRequest();

        //            if (strExternalWebIndex != string.Empty)
        //            {
        //                removeRequest.ExternalWebIndexId = strExternalWebIndex;

        //                ICMSIndexService channel = GetChannel(localContext.OrganizationService);
        //                RemoveIndexResponse removeResponse = channel.RemoveIndex(removeRequest);

        //                localContext.Trace("Remove response status: " + removeResponse.Status.ToString());
        //                if (removeResponse.Status.ToString().ToLower() == "success")
        //                {
        //                    this.cgi_IntranetIndex = string.Empty;
        //                    this.cgi_ExternalWebIndex = string.Empty;

        //                    if (preImage.cgi_PublishonWeb != null && (bool)preImage.cgi_PublishonWeb)
        //                    {
        //                        OptionSetValue o = new OptionSetValue(285050001);  //Pending
        //                        this.cgi_Appoval = o;
        //                    }
        //                }
        //            }
        //        }

        //        Kunskapsartiklar intranetClient = kbArticleChannel(localContext);

        //        KBArtiklarRequest kbArticleIntranetRequest = GetKBArticleIntranet(localContext, preImage);
        //        kbArticleIntranetRequest.Action = "DELETE";

        //        localContext.Trace(string.Format("Title: {0}; Subject: {1}; Number: {2}; Content: {3}; ExternalContent: {4}; Action: {5}; CreatedByUser: {6}; ModifedOn: {7}; Keywords: {8};",
        //            kbArticleIntranetRequest.Title, kbArticleIntranetRequest.subject, kbArticleIntranetRequest.Number, kbArticleIntranetRequest.content, kbArticleIntranetRequest.ExternalContent,
        //            kbArticleIntranetRequest.Action, kbArticleIntranetRequest.CreatedByUser, kbArticleIntranetRequest.modifiedon, kbArticleIntranetRequest.Keywords));

        //        try
        //        {
        //            KBArtiklarRequest1 request1 = new KBArtiklarRequest1();
        //            request1.KBArtiklarRequest = kbArticleIntranetRequest;
        //            KBArtiklarResponse1 response = intranetClient.KBArtiklar(request1);

        //            localContext.Trace("Message: " + response.KBArtiklarResponse.Message);
        //            if (response.KBArtiklarResponse.Message == "Error")
        //            {
        //                localContext.Trace("ErrorMessage: " + response.KBArtiklarResponse.Error.ErrorMessage);
        //                localContext.Trace((response.KBArtiklarResponse.Error.ErrorMessage != "Mismatch between articles found and expected min max hitcount: 1-1").ToString());
        //                if (response.KBArtiklarResponse.Error.ErrorMessage != "Object reference not set to an instance of an object." &&
        //                    response.KBArtiklarResponse.Error.HttpStatusCode != "500")
        //                {
        //                    throw new InvalidPluginExecutionException("Error: " + response.KBArtiklarResponse.Error.ErrorMessage);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            localContext.Trace("UnPublishArticle.. Catch Error: " + e.Message);
        //            throw new InvalidPluginExecutionException("Error: " + e.Message);
        //        }
        //    }
        //}

        //private void PublishArticle(Plugin.LocalPluginContext localContext, KBArticleEntity preImage)
        //{
        //    localContext.Trace("PublishArticle method..");
        //    localContext.Trace("Contains PreImage: " + localContext.PluginExecutionContext.PreEntityImages.Contains("PreImage").ToString());

        //    if (preImage != null)
        //    {
        //        CreateIndexRequest createRequest = new CreateIndexRequest();

        //        //Internal publishing //Approved
        //        if (preImage.cgi_Appoval?.Value == 285050002 || preImage.cgi_Appoval?.Value == 285050003)
        //        {
        //            // if publish on web
        //            if (preImage.cgi_PublishonWeb != null && (bool)preImage.cgi_PublishonWeb)
        //            {
        //                localContext.Trace("if publish on web");
        //                Article kbArticleExternalWeb = GetKBArticleExternalWeb(preImage);

        //                createRequest.RequestActionType = ActionType.ExternalWeb;
        //                createRequest.KbArticleForExternalWeb = kbArticleExternalWeb;

        //                ICMSIndexService channel = GetChannel(localContext.OrganizationService);
        //                CreateIndexResponse createResponse = channel.CreateIndex(createRequest);

        //                localContext.Trace("createResponse.ExternalWebIndexId: " + createResponse.ExternalWebIndexId);

        //                this.cgi_ExternalWebIndex = createResponse.ExternalWebIndexId;
        //            }
        //        }

        //        // publish on intranet
        //        localContext.Trace("publish on intranet");

        //        Kunskapsartiklar intranetClient = kbArticleChannel();

        //        KBArtiklarRequest kbArticleIntranetRequest = GetKBArticleIntranet(localContext, preImage);
        //        kbArticleIntranetRequest.Action = "UPDATE";

        //        localContext.Trace(string.Format("Title: {0}; Subject: {1}; Number: {2}; Content: {3}; ExternalContent: {4}; Action: {5}; CreatedByUser: {6}; ModifedOn: {7}; Keywords: {8};",
        //            kbArticleIntranetRequest.Title, kbArticleIntranetRequest.subject, kbArticleIntranetRequest.Number, kbArticleIntranetRequest.content, kbArticleIntranetRequest.ExternalContent,
        //            kbArticleIntranetRequest.Action, kbArticleIntranetRequest.CreatedByUser, kbArticleIntranetRequest.modifiedon, kbArticleIntranetRequest.Keywords));

        //        try
        //        {
        //            KBArtiklarRequest1 request1 = new KBArtiklarRequest1();
        //            request1.KBArtiklarRequest = kbArticleIntranetRequest;
        //            KBArtiklarResponse1 response = intranetClient.KBArtiklar(request1);
        //            localContext.Trace(string.Format("Response Message: {0}", response.KBArtiklarResponse.Message));

        //            if (response.KBArtiklarResponse.Message == "Error")
        //            {
        //                throw new InvalidPluginExecutionException("Biztalk Error: " + response.KBArtiklarResponse.Error.ErrorMessage);
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            localContext.Trace("PublishArticle.. Catch Error: " + e.Message);
        //            throw new InvalidPluginExecutionException("Error: " + e.Message);
        //        }
        //    }
        //}

        //public static DateTime RetrieveUTCTimeFromLocalTime(DateTime localTime, IOrganizationService service)
        //{
        //    LocalTimeFromUtcTimeRequest request = new LocalTimeFromUtcTimeRequest();
        //    request.UtcTime = localTime;
        //    request.TimeZoneCode = 110;

        //    var response = (LocalTimeFromUtcTimeResponse)service.Execute(request);

        //    return response.LocalTime;
        //}

        //public KBArtiklarRequest GetKBArticleIntranet(Plugin.LocalPluginContext localContext, KBArticleEntity preImage)
        //{
        //    localContext.Trace("GetKBArticleIntranet.. start");
        //    KBArtiklarRequest kbArticleIntranet = new KBArtiklarRequest();

        //    kbArticleIntranet.Title = preImage.Title;
        //    kbArticleIntranet.content = "";
        //    if (preImage.Content != null)
        //        kbArticleIntranet.content = System.Net.WebUtility.HtmlEncode(preImage.Content);

        //    kbArticleIntranet.ExternalContent = "";
        //    if (preImage.cgi_ExternalText != null)
        //        kbArticleIntranet.ExternalContent = System.Net.WebUtility.HtmlEncode(preImage.cgi_ExternalText);

        //    kbArticleIntranet.CreatedByUser = preImage.ModifiedBy?.Name;
        //    kbArticleIntranet.Keywords = preImage.KeyWords;

        //    kbArticleIntranet.modifiedon = Convert.ToDateTime(String.Format("{0:yyyy-MM-dd HH:mm}", RetrieveUTCTimeFromLocalTime((DateTime)preImage.ModifiedOn, localContext.OrganizationService)));
        //    kbArticleIntranet.modifiedonSpecified = true;

        //    kbArticleIntranet.Number = preImage.Number;
        //    kbArticleIntranet.subject = preImage.SubjectId?.Name;
        //    kbArticleIntranet.Action = "";

        //    localContext.Trace("GetKBArticleIntranet.. end");

        //    return kbArticleIntranet;
        //}

        //public Article GetKBArticleExternalWeb(KBArticleEntity preImage)
        //{
        //    Article kbArticleExternalWeb = new Article
        //    {
        //        Title = preImage.Title,
        //        KnowledgeBaseArticleId = preImage.Number,
        //        ExternalText =
        //            preImage.cgi_ExternalText != null
        //                ? preImage.cgi_ExternalText.Replace("___", "") ///TODO Find other solution to Replace function...
        //                : string.Empty,
        //        Authors = new[] { preImage.ModifiedBy?.Name},
        //        PublishDate = preImage.CreatedOn,
        //        Category = preImage.KeyWords,
        //        UpdateDate = preImage.ModifiedOn,
        //        Summary =
        //            preImage.SubjectId != null
        //                ? preImage.SubjectId?.Name
        //                : string.Empty
        //    };

        //    return kbArticleExternalWeb;
        //}

        //public ICMSIndexService GetChannel(IOrganizationService service)
        //{
        //    BasicHttpBinding myBinding = new BasicHttpBinding
        //    {
        //        Security =
        //        {
        //            Mode = BasicHttpSecurityMode.None,
        //            Transport =
        //            {
        //                ClientCredentialType = HttpClientCredentialType.None,
        //                ProxyCredentialType = HttpProxyCredentialType.None
        //            },
        //            Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
        //        }
        //    };

        //    EndpointAddress endPointAddress = new EndpointAddress(_serviceEndpoints.WebEndpoint);
        //    ChannelFactory<ICMSIndexService> factory = new ChannelFactory<ICMSIndexService>(myBinding, endPointAddress);
        //    ICMSIndexService channel = factory.CreateChannel();

        //    return channel;
        //}

        //public Kunskapsartiklar kbArticleChannel(Plugin.LocalPluginContext localContext)
        //{
        //    localContext.Trace(string.Format("{0}", _serviceEndpoints.IntranetEndpoint.ToString()));

        //    EndpointAddress endPointAddress = new EndpointAddress(_serviceEndpoints.IntranetEndpoint);
        //    localContext.Trace(string.Format("{0}", endPointAddress.ToString()));

        //    BasicHttpBinding myBinding = new BasicHttpBinding
        //    {
        //        Security =
        //        {
        //            Mode = BasicHttpSecurityMode.None,
        //            Transport =
        //            {
        //                ClientCredentialType = HttpClientCredentialType.None,
        //                ProxyCredentialType = HttpProxyCredentialType.None
        //            },
        //            Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
        //        }
        //    };

        //    ChannelFactory<Kunskapsartiklar> factory = new ChannelFactory<Kunskapsartiklar>(myBinding, endPointAddress);
        //    var channel = factory.CreateChannel();

        //    return channel;
        //}

        //private ServiceEndpoints getSettings(Plugin.LocalPluginContext localContext)
        //{
        //    ServiceEndpoints serviceEndpoints = new ServiceEndpoints();

        //    string now = DateTime.Now.ToString("s");
        //    string xml = "";
        //    xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
        //    xml += "   <entity name='cgi_setting'>";
        //    xml += "       <attribute name='cgi_extconnectorservice' />";
        //    xml += "       <attribute name='cgi_kbarticlesforintranetservice' />";
        //    xml += "       <filter type='and'>";
        //    xml += "           <condition attribute='statecode' operator='eq' value='0' />";
        //    xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
        //    xml += "           <filter type='or'>";
        //    xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
        //    xml += "               <condition attribute='cgi_validto' operator='null' />";
        //    xml += "           </filter>";
        //    xml += "       </filter>";
        //    xml += "   </entity>";
        //    xml += "</fetch>";

        //    FetchExpression f = new FetchExpression(xml);
        //    EntityCollection ents = localContext.OrganizationService.RetrieveMultiple(f);
        //    if (ents != null && ents.Entities.Any())
        //    {
        //        Entity ent = ents[0];
        //        if (ent.Attributes.Contains("cgi_extconnectorservice"))
        //        {
        //            serviceEndpoints.WebEndpoint = ent.Attributes["cgi_extconnectorservice"].ToString();
        //        }

        //        if (ent.Attributes.Contains("cgi_kbarticlesforintranetservice"))
        //        {
        //            serviceEndpoints.IntranetEndpoint = ent.Attributes["cgi_kbarticlesforintranetservice"].ToString();
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidPluginExecutionException("Settings is missing!");
        //    }


        //    return serviceEndpoints;
        //}
    }
}

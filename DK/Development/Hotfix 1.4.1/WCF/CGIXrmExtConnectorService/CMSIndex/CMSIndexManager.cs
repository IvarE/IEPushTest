using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Json;

namespace CGIXrmExtConnectorService
{
    internal class CMSIndexManager
    {
        #region Internal
        string externalWebConfigSection = "KbExternalWeb";
        string intranetConfigSection = "KbIntranet";
        internal CreateIndexResponse CreateIndex(CreateIndexRequest createIndexRequest)
        {
            CreateIndexResponse createIndexResponse = new CreateIndexResponse();
            try
            {
                if (createIndexRequest == null)
                {
                    createIndexResponse.Status = ProcessingStatus.FAILED;
                    createIndexResponse.Message = "CreateIndexRequest is Null or not an Object";
                    return createIndexResponse;
                }

                IClient kbExternalWebClient = CreateClientFromConfig(externalWebConfigSection);
                IClient kbIntranetClient = CreateClientFromConfig(intranetConfigSection);
                IndexResult kbExternalWebResult = new IndexResult();
                IndexResult kbIntranetResult = new IndexResult();

                switch (createIndexRequest.RequestActionType)
                {
                    case ActionType.Intranet:
                        if (createIndexRequest.KbArticleForIntranet == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.FAILED;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object";
                            return createIndexResponse;
                        }

                        kbIntranetResult = kbIntranetClient.Index(GetKBArticleFromArticle(createIndexRequest.KbArticleForIntranet));
                        if (kbIntranetResult.Ok)
                        {
                            createIndexResponse.IntranetIndexStatus = kbIntranetResult.Ok;
                            createIndexResponse.IntranetIndexId = kbIntranetResult.Id;
                        }
                        break;
                    case ActionType.ExternalWeb:
                        if (createIndexRequest.KbArticleForExternalWeb == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.FAILED;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object";
                            return createIndexResponse;
                        }
                        kbExternalWebResult = kbExternalWebClient.Index(GetKBArticleFromArticle(createIndexRequest.KbArticleForExternalWeb));
                        if (kbExternalWebResult.Ok)
                        {
                            createIndexResponse.ExternalWebIndexStatus = kbExternalWebResult.Ok;
                            createIndexResponse.ExternalWebIndexId = kbExternalWebResult.Id;
                        }
                        break;
                    case ActionType.Both:
                        if (createIndexRequest.KbArticleForIntranet == null || createIndexRequest.KbArticleForExternalWeb == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.FAILED;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object for either Intranet or ExternalWeb or Both";
                            return createIndexResponse;
                        }
                        kbIntranetResult = kbIntranetClient.Index(GetKBArticleFromArticle(createIndexRequest.KbArticleForIntranet));
                        if (kbIntranetResult.Ok)
                        {
                            createIndexResponse.IntranetIndexStatus = kbIntranetResult.Ok;
                            createIndexResponse.IntranetIndexId = kbIntranetResult.Id;
                            kbExternalWebResult = kbExternalWebClient.Index(GetKBArticleFromArticle(createIndexRequest.KbArticleForExternalWeb));
                            if (kbExternalWebResult.Ok)
                            {
                                createIndexResponse.ExternalWebIndexStatus = kbExternalWebResult.Ok;
                                createIndexResponse.ExternalWebIndexId = kbExternalWebResult.Id;
                            }
                            else
                            {
                                DeleteResult deleteIntranetIndexResult = kbIntranetClient.Delete<KBArticle>(kbIntranetResult.Id);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                createIndexResponse.Status = ProcessingStatus.FAILED;
                createIndexResponse.Message = ExceptionMsg;
            }
            catch (Exception ex)
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                createIndexResponse.Status = ProcessingStatus.FAILED;
                createIndexResponse.Message = ExceptionMsg;
            }
            return createIndexResponse;
        }

        internal RemoveIndexResponse RemoveIndex(RemoveIndexRequest removeIndexRequest)
        {
            RemoveIndexResponse removeIndexResponse = new RemoveIndexResponse();

            try
            {
                if (removeIndexRequest == null)
                {
                    removeIndexResponse.Status = ProcessingStatus.FAILED;
                    removeIndexResponse.Message = "RemoveIndexRequest is Null or not an Object";

                    return removeIndexResponse;
                }
                IClient kbExternalWebClient = CreateClientFromConfig(externalWebConfigSection);
                IClient kbIntranetClient = CreateClientFromConfig(intranetConfigSection);

                if (!string.IsNullOrEmpty(removeIndexRequest.ExternalWebIndexId))
                {
                    DeleteResult deleteExternalWebIndexResult = kbExternalWebClient.Delete<KBArticle>(removeIndexRequest.ExternalWebIndexId);
                }
                if (!string.IsNullOrEmpty(removeIndexRequest.IntranetIndexId))
                {
                    DeleteResult deleteIntranetIndexResult = kbIntranetClient.Delete<KBArticle>(removeIndexRequest.IntranetIndexId);
                }
                removeIndexResponse.Status = ProcessingStatus.SUCCESS;
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                removeIndexResponse.Status = ProcessingStatus.FAILED;
                removeIndexResponse.Message = ExceptionMsg;
            }
            catch (Exception ex)
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                removeIndexResponse.Status = ProcessingStatus.FAILED;
                removeIndexResponse.Message = ExceptionMsg;
            }

            return removeIndexResponse;
        }
    #endregion

        #region Private Methods
        private KBArticle GetKBArticleFromArticle(Article article)
        {
            KBArticle kbArticle = new KBArticle()
            {
                Authors = article.Authors,
                Category = article.Category,
                ExternalText = article.ExternalText,
                InternalText = article.InternalText,
                Keywords = article.Keywords,
                KnowledgeBaseArticleId = article.KnowledgeBaseArticleId,
                PublishDate = article.PublishDate,
                Summary = article.Summary,
                Title = article.Title,
                UpdateDate = article.UpdateDate
            };
            return kbArticle;
        }
        private IClient CreateClientFromConfig(string sectionName)
        {
            EPiServer.Find.Configuration config = (EPiServer.Find.Configuration)ConfigurationManager.GetSection(sectionName);
            IClient client = new Client(config.ServiceUrl, config.DefaultIndex);
            return client;
        }
      #endregion
    }
}

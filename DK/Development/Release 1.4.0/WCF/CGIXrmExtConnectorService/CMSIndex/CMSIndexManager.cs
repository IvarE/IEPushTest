using System;
using System.Configuration;
using CGIXrmExtConnectorService.CMSIndex.Models;
using CGIXrmExtConnectorService.Shared.Models;
using EPiServer.Find;
using EPiServer.Find.Api;

namespace CGIXrmExtConnectorService.CMSIndex
{
    internal class CmsIndexManager
    {
        #region Declarations ------------------------------------------------------------------------------------------

        private const string ExternalWebConfigSection = "KbExternalWeb";
        private const string IntranetConfigSection = "KbIntranet";

        #endregion

        #region Internal Methods --------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createIndexRequest"></param>
        /// <returns></returns>
        internal CreateIndexResponse CreateIndex(CreateIndexRequest createIndexRequest)
        {
            var createIndexResponse = new CreateIndexResponse();
            try
            {
                if (createIndexRequest == null)
                {
                    createIndexResponse.Status = ProcessingStatus.Failed;
                    createIndexResponse.Message = "CreateIndexRequest is Null or not an Object";
                    return createIndexResponse;
                }

                var kbExternalWebClient = CreateClientFromConfig(ExternalWebConfigSection);
                var kbIntranetClient = CreateClientFromConfig(IntranetConfigSection);
                IndexResult kbExternalWebResult;
                IndexResult kbIntranetResult;

                switch (createIndexRequest.RequestActionType)
                {
                    case ActionType.Intranet:
                        if (createIndexRequest.KbArticleForIntranet == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.Failed;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object";
                            return createIndexResponse;
                        }

                        kbIntranetResult = kbIntranetClient.Index(GetKbArticleFromArticle(createIndexRequest.KbArticleForIntranet));
                        if (kbIntranetResult.Ok)
                        {
                            createIndexResponse.IntranetIndexStatus = kbIntranetResult.Ok;
                            createIndexResponse.IntranetIndexId = kbIntranetResult.Id;
                        }
                        break;
                    case ActionType.ExternalWeb:
                        if (createIndexRequest.KbArticleForExternalWeb == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.Failed;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object";
                            return createIndexResponse;
                        }
                        kbExternalWebResult = kbExternalWebClient.Index(GetKbArticleFromArticle(createIndexRequest.KbArticleForExternalWeb));
                        if (kbExternalWebResult.Ok)
                        {
                            createIndexResponse.ExternalWebIndexStatus = kbExternalWebResult.Ok;
                            createIndexResponse.ExternalWebIndexId = kbExternalWebResult.Id;
                        }
                        break;
                    case ActionType.Both:
                        if (createIndexRequest.KbArticleForIntranet == null || createIndexRequest.KbArticleForExternalWeb == null)
                        {
                            createIndexResponse.Status = ProcessingStatus.Failed;
                            createIndexResponse.Message = "KBArticle is Null or Not an Object for either Intranet or ExternalWeb or Both";
                            return createIndexResponse;
                        }
                        kbIntranetResult = kbIntranetClient.Index(GetKbArticleFromArticle(createIndexRequest.KbArticleForIntranet));
                        if (kbIntranetResult.Ok)
                        {
                            createIndexResponse.IntranetIndexStatus = kbIntranetResult.Ok;
                            createIndexResponse.IntranetIndexId = kbIntranetResult.Id;
                            kbExternalWebResult = kbExternalWebClient.Index(GetKbArticleFromArticle(createIndexRequest.KbArticleForExternalWeb));
                            if (kbExternalWebResult.Ok)
                            {
                                createIndexResponse.ExternalWebIndexStatus = kbExternalWebResult.Ok;
                                createIndexResponse.ExternalWebIndexId = kbExternalWebResult.Id;
                            }
                            else
                            {
                                kbIntranetClient.Delete<KbArticle>(kbIntranetResult.Id);
                            }
                        }
                        break;
                }
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" +
                    ex.Message + " Stack Trace:" +
                    ex.StackTrace + " Inner Fault: {0}" +
                    ex.InnerException.Message;

                createIndexResponse.Status = ProcessingStatus.Failed;
                createIndexResponse.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                string exceptionMessage = "The application terminated with an error." + ex.Message;
                createIndexResponse.Status = ProcessingStatus.Failed;
                createIndexResponse.Message = exceptionMessage;
            }
            return createIndexResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="removeIndexRequest"></param>
        /// <returns></returns>
        internal RemoveIndexResponse RemoveIndex(RemoveIndexRequest removeIndexRequest)
        {
            var removeIndexResponse = new RemoveIndexResponse();

            try
            {
                if (removeIndexRequest == null)
                {
                    removeIndexResponse.Status = ProcessingStatus.Failed;
                    removeIndexResponse.Message = "RemoveIndexRequest is Null or not an Object";

                    return removeIndexResponse;
                }

                var kbExternalWebClient = CreateClientFromConfig(ExternalWebConfigSection);
                var kbIntranetClient = CreateClientFromConfig(IntranetConfigSection);

                if (!string.IsNullOrEmpty(removeIndexRequest.ExternalWebIndexId))
                {
                    kbExternalWebClient.Delete<KbArticle>(removeIndexRequest.ExternalWebIndexId);
                }
                if (!string.IsNullOrEmpty(removeIndexRequest.IntranetIndexId))
                {
                    kbIntranetClient.Delete<KbArticle>(removeIndexRequest.IntranetIndexId);
                }
                removeIndexResponse.Status = ProcessingStatus.Success;
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:"
                    + ex.Message + " Stack Trace:"
                    + ex.StackTrace + " Inner Fault: {0}"
                    + ex.InnerException.Message;
                removeIndexResponse.Status = ProcessingStatus.Failed;
                removeIndexResponse.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                var exceptionMessage = "The application terminated with an error." + ex.Message;
                removeIndexResponse.Status = ProcessingStatus.Failed;
                removeIndexResponse.Message = exceptionMessage;
            }

            return removeIndexResponse;
        }
        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private static KbArticle GetKbArticleFromArticle(Article article)
        {
            var kbArticle = new KbArticle()
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        private IClient CreateClientFromConfig(string sectionName)
        {
            var config = (EPiServer.Find.Configuration)ConfigurationManager.GetSection(sectionName);
            IClient client = new Client(config.ServiceUrl, config.DefaultIndex);
            return client;
        }

        #endregion
    }
}
